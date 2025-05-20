
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Myrmidon.Core.Map;
using Myrmidon.Core.Utilities.Geometry;

namespace Myrmidon.Rules {

    internal struct Path {
        public readonly Direction startDirection; // Direction of first step along this path.
        public readonly Vector pos;
        public readonly int length;
        public readonly int cost; // Total cost spent to walk this path so far.

        public Path(Direction startDirection, Vector pos, int length, int cost) {
            this.startDirection = startDirection;
            this.pos = pos;
            this.length = length;
            this.cost = cost;
        }

        String toString() => "$startDirection pos:$pos cost:$cost";
    }

    internal abstract class Pathfinder<T> {

        /// An abstract class encapsulating the core A* search algorithm.
        ///
        /// Subclasses of this fill in the cost, heuristics, and goal conditions.
        ///
        /// Using jump point search would be nice because it's faster. However, it
        /// assumes all tiles have a uniform cost to enter, which isn't the case for
        /// us. Monster pathfinding treats things like door and occupied tiles as
        /// accessible but expensive. Likewise, sound pathfinding treats doors as
        /// blocking some but not all sound.
        readonly Map stage;
        readonly Vector start;
        readonly Vector end;

        public Pathfinder(Map stage, Vector start, Vector end) {
            this.stage = stage;
            this.start = start;
            this.end = end;
        }

        public class PathingBucketQueue<T> {
            private Queue<T>[] _buckets = new Queue<T>[0];
            private int _bucket = 0;

            public bool IsEmpty {
                get { return (_bucket == 0); }
            }

            public void Reset() {
                Array.Clear(_buckets);
            }

            public void Add(T value, int cost) {
                _bucket = Math.Min(_bucket, cost);

                // Grow the bucket array if needed.
                if (_buckets.Count() <= cost + 1) Array.Resize(ref _buckets, cost + 1);


                // Find the bucket, or create it if needed.
                var bucket = _buckets[cost];
                if (bucket == null) {
                    bucket = new();
                    _buckets[cost] = bucket;
                }

                bucket.Enqueue(value);
            }

            /// Removes the best item from the queue or returns `null` if the queue is
            /// empty.
            public T RemoveNext() {
                // Advance past any empty buckets.
                while (_bucket < _buckets.Count() &&
                    (_buckets[_bucket] == null ? true : _buckets[_bucket].Count() == 0)) {
                    _bucket++;
                }

                // If we ran out of buckets, the queue is empty.
                if (_bucket >= _buckets.Count()) return default(T);

                //return _buckets[_bucket].Dequeue();
                return default(T);
            }
        }

        /// Perform an A* search from [start] trying to reach [end].
        internal T Search() {
            var paths = new PathingBucketQueue<Path>();

            // The set of tiles we have completely explored already.
            var explored = new HashSet<Vector>();

            var startPath = new Path(Direction.None, start, 0, 0);
            paths.Add(startPath, Priority(startPath, end));

            while (true) {
                // OBS: DER KAN VÆRE ET PROBLEM HER:
                // Det tjekkes om køen er tom efter sidste medlem lige et fjernet,
                // så den vil aldrig kigge pås idste medlem.
                var path = paths.RemoveNext();
                if (paths.IsEmpty) break;

                if (path.pos == end) return ReachedGoal(path);

                // A given tile may get enqueued more than once because we don't check to
                // see if it's already been enqueued at a different cost. When that
                // happens, we will visit the best one first, which is what we want. That
                // means if we later visit the other one, we know it's worse and can just
                // skip it.
                //
                // While it seems weird to redundantly add the same tile to the priority
                // queue, in practice, it's faster than updating the priority of the
                // previously queued item.
                //
                // See: https://www.redblobgames.com/pathfinding/a-star/implementation.html#algorithm
                if (!explored.Add(path.pos)) continue;

                var result = ProcessStep(path);
                if (result != null) return result;

                // Find the steps we can take.
                foreach (var dir in Direction.All) {
                        var neighbor = path.pos + dir;

                        if (explored.Contains(neighbor)) continue;
                        if (!stage.Bounds.Contains(neighbor)) continue;

                        var cost = StepCost(neighbor, stage[neighbor]);
                        if (cost == null) continue;

                        var newPath = new Path(
                            path.startDirection == Direction.None ? dir : path.startDirection,
                            neighbor,
                            path.length + 1,
                            path.cost + cost);
                        paths.Add(newPath, Priority(newPath, end));
                }
            }

            // If we get here, it means we definitively determined there is no path to
            // the goal.
            return UnreachableGoal();
        }

        int Priority(Path path, Vector end) {
            return path.cost + Heuristic(path.pos, end);
        }

        /// The estimated cost from [pos] to [end].
        /// By default, uses the king length.
        internal int Heuristic(Vector pos, Vector end) => (end - pos).KingLength;

        /// The cost required to enter [tile] at [pos] from a neighboring tile or
        /// `null` if the tile cannot be entered.
        internal abstract int StepCost(Vector pos, Tile tile);

        /// Called for each step of pathfinding where [path] is the current path
        /// being processed.
        /// If the pathfinder wants to immediately stop processing and return a value,
        /// this should return a non-`null` value. Otherwise, return `null` and the
        /// pathfinder will continue.
        internal abstract T ProcessStep(Path path);

        /// Called when the pathfinder has found a [path] to the end point.
        /// Override this to return the desired value upon success.
        internal abstract T ReachedGoal(Path path);

        /// Called when the pathfinder determines it cannot reach the end point.
        /// Override this to return an appropriate failure value.
        internal abstract T UnreachableGoal();

        }
}
