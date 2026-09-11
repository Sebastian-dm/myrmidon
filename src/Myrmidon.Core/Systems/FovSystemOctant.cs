using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bramble.Core;
using Myrmidon.Core.Components;
using Myrmidon.Core.Utilities.Geometry;
using Myrmidon.Core.Maps.Tiles;
using Myrmidon.Core.Entities;
using Myrmidon.Core.Maps;

namespace Myrmidon.Core.Systems;

public class FovSystemOctant : IFovSystem {

    private int _range;
    private int _rangeSqrt;

    public FovSystemOctant(int range = 8) {
        _range = range;
        _rangeSqrt = _range * _range;
    }


    // Recompute the visible area based on a given location.
    public void Recompute(TileMap map, Vec origin) {
        
        // Todo: Figure out how to include entities in the tile fov update
        RefreshEntities(map, origin);
        
        ResetLightLevelInBoundDist(map, origin);
        for (var octant = 0; octant < 8; octant++) {
            RefreshOctant(map, octant, origin);
        }
        
        // Set origin to be visible
        UpdatePerceptible(map, origin, origin);
    }

    private void ResetLightLevelInBoundDist(TileMap map, Vec origin) {
        int margin = 1;
        int left = Math.Max(0, origin.X - _range - margin);
        int top = Math.Max(0, origin.Y - _range  - margin);
        int right = Math.Min(map.Width, origin.X + _range + margin);
        int bottom = Math.Min(map.Height, origin.Y + _range + margin);

        // Update tile visiblity
        for (int x = left; x < right; x++) {
            for (int y = top; y < bottom; y++) {
                map.GetPerceptibleComponent(x,y).LightLevel = 0.0f;
            }
        }
    }

    private void RefreshEntities(TileMap map, Vec origin) {
        // Update entity visibility
        foreach (Entity entity in map.Entities.Items) {
            if (Vec.IsDistanceWithin(origin, entity.Position, _range)) {
                entity.isVisible = true;
            }
            else {
                entity.isVisible = false;
            }
        }
    }
    
    
    private List<Shadow> RefreshOctant(TileMap map, int octant, Vec origin) {
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

                // If we know the entire row is in shadow, we don't need to be more
                // specific.
                if (fullShadow) {
                    continue;
                    //var renderComp = map.GetRenderComponent(pos);
                    //renderComp.ColorBase = "black";
                    //renderComp.Dimfactor = 0.0f;
                    //renderComp.Explored = false;
                }
                else {
                    var projection = _projectTile(row, col);

                    // Set the visibility of this tile.
                    var visible = !line.IsInShadow(projection);

                    //renderComp.Explored = true;

                    // Add any opaque tiles to the shadow map.
                    var tile = map[pos];
                    
                    if (visible) {
                        UpdatePerceptible(map, origin, pos);
                        
                        if (tile.IsBlockingLos) {
                            line.Add(projection);
                            fullShadow = line.IsFullShadow;
                        }
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

    private void UpdatePerceptible(TileMap map, Vec origin, Vec target) {
        var prcpt = map.GetPerceptibleComponent(target);

        int distSqrt = (target - origin).LengthSquared;
        if (distSqrt <= _rangeSqrt)
            prcpt.Explored = true;

        float clampedDist = (float)Math.Clamp(Math.Pow(distSqrt / _rangeSqrt, 1.0f),  0.0f, 1.0f);
        prcpt.LightLevel = 1f - clampedDist;
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