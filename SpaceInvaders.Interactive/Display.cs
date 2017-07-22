using SpaceInvaders.Simulation;
using System;

namespace SpaceInvaders.Interactive
{
    class Display
    {
        private static void ClearView(char[,] view)
        {
            for (int x = 0; x < view.GetLength(0); x++)
                for (int y = 0; y < view.GetLength(1); y++)
                    view[x, y] = ' ';
        }

        private static void DrawPlayer(WorldState worldState, PlayerState playerState, char[,] view)
        {
            int playerX = playerState.Position;
            int playerY = worldState.Height - 1;
            view[playerX, playerY] = 'P';
        }

        private static char[,] GenerateView(WorldState worldState)
        {
            char[,] view = new char[worldState.Width, worldState.Height];

            ClearView(view);
            DrawPlayer(worldState, worldState.PlayerState, view);

            return view;
        }

        private static void PrintView(char[,] view)
        {
            Console.Clear();
            Console.WriteLine("+" + new String('-', view.GetLength(0)) + "+");
            for (int y = 0; y < view.GetLength(1); y++)
            {
                string row = "|";
                for (int x = 0; x < view.GetLength(0); x++)
                    row += view[x, y];
                row += "|";
                Console.WriteLine(row);
            }
            Console.WriteLine("+" + new String('-', view.GetLength(0)) + "+");
        }

        public static void PrintWorld(WorldState worldState)
        {
            char[,] view = GenerateView(worldState);
            PrintView(view);
        }
    }
}
