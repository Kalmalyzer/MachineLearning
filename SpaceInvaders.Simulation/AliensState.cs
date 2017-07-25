using System.Collections.Generic;

namespace SpaceInvaders.Simulation
{
    public class AliensState
    {
        public readonly Vector2i TopLeft;

        public readonly List<Vector2i> RelativePositions;

        public AliensState(Vector2i topLeft, List<Vector2i> relativePositions)
        {
            TopLeft = topLeft;
            RelativePositions = relativePositions;
        }

        public bool GetAbsoluteBoundingBox(out Vector2i topLeft, out Vector2i bottomRight)
        {
            if (RelativePositions.Count != 0)
            {
                Vector2i min = new Vector2i(int.MaxValue, int.MaxValue);
                Vector2i max = new Vector2i(int.MinValue, int.MinValue);

                foreach (Vector2i position in RelativePositions)
                {
                    min = Vector2i.Min(min, position);
                    max = Vector2i.Max(max, position);
                }

                topLeft = min + TopLeft;
                bottomRight = max + Vector2i.One + TopLeft;
                return true;
            }
            else
            {
                topLeft = new Vector2i(-1, -1);
                bottomRight = new Vector2i(-1, -1);
                return false;
            }
        }
    }
}
