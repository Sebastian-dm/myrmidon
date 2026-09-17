using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bramble.Core;
using Myrmidon.Core.Parts;
using Myrmidon.Core.Ecs;
using Myrmidon.Core.Utilities.Geometry;
using Myrmidon.Core.Maps.Tiles;
using Myrmidon.Core.Entities;
using Myrmidon.Core.Maps;
using Myrmidon.Core.Zones;

namespace Myrmidon.Core.Systems;

public class FovSystemOctant : IFovSystem {

    private int _range;
    private int _rangeSqrt;
    private EcsWorld _ecs;

    public FovSystemOctant(EcsWorld ecs, int range = 8) {
        _ecs = ecs;
        _range = range;
        _rangeSqrt = _range * _range;
    }


    // Recompute the visible area based on a given location.
    public void Recompute(Zone zone, Vec origin) {
        
        ResetLightLevelInBoundDist(zone, origin);
        for (var octant = 0; octant < 8; octant++) {
            RefreshOctant(zone, octant, origin);
        }
        
        // Set origin to be visible
        UpdatePerceptibleLightFromDistance(zone, new Vec(0,0), zone.Map.GetPerceptibleComponent(origin));
    }

    private void ResetLightLevelInBoundDist(Zone zone, Vec origin) {
        int margin = 1;
        int left = Math.Max(0, origin.X - _range - margin);
        int top = Math.Max(0, origin.Y - _range  - margin);
        int right = Math.Min(zone.Map.Width, origin.X + _range + margin);
        int bottom = Math.Min(zone.Map.Height, origin.Y + _range + margin);

        // Update tile visiblity
        for (int x = left; x < right; x++) {
            for (int y = top; y < bottom; y++) {
                zone.Map.GetPerceptibleComponent(x,y).LightLevel = 0.0f;
            }
        }
    }
    
    
    private List<Shadow> RefreshOctant(Zone zone, int octant, Vec origin) {
        var map = zone.Map;
        var line = new ShadowLine();
        var fullShadow = false;

        // Sweep through the rows ('rows' may be vertical or horizontal based on
        // the incrementors). Start at row 1 to skip the center position.
        for (var row = 1; row < _range; row++) {
            // If we've gone out of bounds, bail.
            if (!map.Bounds.Contains(origin + TransformOctant(row, 0, octant))) break;

            for (var col = 0; col <= row; col++) {
                var pos = origin + TransformOctant(row, col, octant);

                // If we've traversed out of bounds, bail on this row.
                // note: this improves performance, but works on the assumption that
                // the starting tile of the FOV is in bounds.
                if (!map.Bounds.Contains(pos)) break;

                // Skip if we know the entire row is in shadow
                if (fullShadow)
                    continue;

                var projection = _projectTile(row, col);
                var visible = !line.IsInShadow(projection);

                if (visible) {
                    Vec distance = origin - pos;
                    // Set the visibility of this tile.
                    UpdatePerceptibleLightFromDistance(zone, distance, zone.Map.GetPerceptibleComponent(pos));
                    
                    // Set visibility of entities on this tile
                    var entitiesOnTile = zone.SpatialIndex.At(pos);
                    foreach (var entity in entitiesOnTile) {
                        if (_ecs.TryGet(entity, out Perceptible perc));
                            UpdatePerceptibleLightFromDistance(zone, distance, perc);
                    }
                    
                    // Add any opaque tiles to the shadow map.
                    var tile = map[pos];
                    if (tile.IsBlockingLos) {
                        line.Add(projection);
                        fullShadow = line.IsFullShadow;
                    }
                }
            }
        }
        return line.Shadows;
    }
    
    private Vec TransformOctant(int row, int col, int octant) {
        return octant switch {
            0 => new Vec(col, -row),
            1 => new Vec(row, -col),
            2 => new Vec(row, col),
            3 => new Vec(col, row),
            4 => new Vec(-col, row),
            5 => new Vec(-row, col),
            6 => new Vec(-row, -col),
            7 => new Vec(-col, -row),
            _ => new Vec(col, -row)
        };
    }
    
    /// Creates a [Shadow] that corresponds to the projected silhouette of the
    /// given tile. This is used both to determine visibility (if any of the
    /// projection is visible, the tile is) and to add the tile to the shadow map.
    ///
    /// The maximal projection of a square is always from the two opposing
    /// corners. From the perspective of octant zero, we know the square is
    /// above and to the right of the viewpoint, so it will be the top left and
    /// bottom right corners.
    Shadow _projectTile(int row, int col) {
        // The top edge of row 0 is 2 wide.
        float topLeft = (float)col / (row + 2);

        // The bottom edge of row 0 is 1 wide.
        float bottomRight = (float)(col + 1) / (row + 1);
        
        var shadow = new Shadow(
            topLeft,
            bottomRight,
            new Vec(col, row + 2),
            new Vec(col + 1, row + 1)
        );
        return shadow;
    }

    private void UpdatePerceptibleLightFromDistance(Zone zone, Vec distance, Perceptible perceptible) {

        int distSqrt = distance.LengthSquared;
        if (distSqrt <= _rangeSqrt)
            perceptible.Explored = true;

        float clampedDist = (float)Math.Clamp(Math.Pow(distSqrt / _rangeSqrt, 1.0f),  0.0f, 1.0f);
        perceptible.LightLevel = 1f - clampedDist;
    }
}


class ShadowLine {
    public List<Shadow> Shadows { get; } = [];
    
    public new string ToString() => $"({string.Join(',', Shadows.Select(s => s.ToString()).ToArray())})";
    
    public bool IsFullShadow =>
        Shadows.Count == 1 && 
        Shadows[0].Start == 0 && 
        Shadows[0].End == 1;
    
    public bool IsInShadow(Shadow projection) {
        // Check the shadow list.
        foreach (var shadow in Shadows)
            if (shadow.Contains(projection)) return true;
        return false;
    }
    
    /// Add [shadow] to the list of non-overlapping shadows. May merge one or more shadows.
    public void Add(Shadow shadow) {
        // Figure out where to slot the new shadow in the sorted list.
        var index = 0;
        for (; index < Shadows.Count; index++) {
            // Stop when we hit the insertion point.
            if (Shadows[index].Start >= shadow.Start) break;
        }

        // The new shadow is going here. See if it overlaps the previous or next.
        Shadow? overlappingPrevious = null;
        if (index > 0 && Shadows[index - 1].End > shadow.Start) {
            overlappingPrevious = Shadows[index - 1];
        }

        Shadow? overlappingNext = null;
        if (index < Shadows.Count && Shadows[index].Start < shadow.End) {
            overlappingNext = Shadows[index];
        }

        // Insert and unify with overlapping shadows.
        if (overlappingNext != null) {
            if (overlappingPrevious != null) {
                // Overlaps both, so unify one and delete the other.
                overlappingPrevious.End = overlappingNext.End;
                overlappingPrevious.EndPos = overlappingNext.EndPos;
                Shadows.RemoveAt(index);
            } else {
                // Only overlaps the next shadow, so unify it with that.
                overlappingNext.Start = shadow.Start;
                overlappingNext.StartPos = shadow.StartPos;
            }
        } else {
            if (overlappingPrevious != null) {
                // Only overlaps the previous shadow, so unify it with that.
                overlappingPrevious.End = shadow.End;
                overlappingPrevious.EndPos = shadow.EndPos;
            } else {
                // Does not overlap anything, so insert.
                Shadows.Insert(index, shadow);
            }
        }
    }
}


/// Represents the 1D projection of a 2D shadow onto a normalized line. In
/// other words, a range from 0.0 to 1.0.
public class Shadow(float start, float end, Vec startPos, Vec endPos) {
    
    public float Start = start;
    public float End = end;
    public Vec StartPos = startPos;
    public Vec EndPos = endPos;

    public new string ToString() => $"({Start}-{End})";

    /// Returns `true` if [other] is completely covered by this shadow.
    public bool Contains(Shadow other) {
        return Start <= other.Start && End >= other.End;
    }
}