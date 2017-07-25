using System.Collections.Generic;

namespace SpaceInvaders.Simulation
{
    public class AliensState
    {
        public readonly Vector2i TopLeft;

        public readonly bool[,] Present;

        public AliensState(Vector2i topLeft, bool[,] present)
        {
            TopLeft = topLeft;
            Present = present;
        }

        public bool GetAbsoluteBoundingBox(out Vector2i topLeft, out Vector2i bottomRight)
        {
            Vector2i min = new Vector2i(Present.GetLength(0), Present.GetLength(1));
            Vector2i max = new Vector2i(-1, -1);

            bool present = false;

            for (int y = 0; y < Present.GetLength(1); y++)
                for (int x = 0; x < Present.GetLength(0); x++)
                {
                    if (Present[x, y])
                    {
                        min = Vector2i.Min(min, new Vector2i(x, y));
                        max = Vector2i.Max(max, new Vector2i(x, y));
                        present = true;
                    }
                }

            if (present)
            {
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

        public List<Vector2i> GetPresentAliens()
        {
            List<Vector2i> presentAliens = new List<Vector2i>();

            for (int y = 0; y < Present.GetLength(1); y++)
                for (int x = 0; x < Present.GetLength(0); x++)
                {
                    if (Present[x, y])
                        presentAliens.Add(new Vector2i(x, y) + TopLeft);
                }

            return presentAliens;
        }
    }
}
