using SpaceInvaders.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;

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

        private static void DrawPlayer(GameConfigState gameConfigState, PlayerState playerState, char[,] view)
        {
            int playerX = playerState.Position;
            int playerY = gameConfigState.Height - 1;
            view[playerX, playerY] = 'P';
        }

        private static void DrawAliens(AliensState aliensState, char[,] view)
        {
            IEnumerable<Vector2i> positions = aliensState.RelativePositions.Select(relativePosition => relativePosition + aliensState.TopLeft);
            foreach (Vector2i alienPosition in positions)
                view[alienPosition.X, alienPosition.Y] = 'A';
        }

        private static void DrawRockets(RocketsState rocketsState, char[,] view)
        {
            foreach (Vector2i rocketPosition in rocketsState.Positions)
                view[rocketPosition.X, rocketPosition.Y] = 'R';
        }

        private static void DrawBombs(BombsState bombsState, char[,] view)
        {
            foreach (Vector2i bombPosition in bombsState.Positions)
                view[bombPosition.X, bombPosition.Y] = 'B';
        }

        private static char[,] GenerateView(WorldState worldState)
        {
            char[,] view = new char[worldState.GameConfigState.Width, worldState.GameConfigState.Height];

            ClearView(view);
            DrawPlayer(worldState.GameConfigState, worldState.PlayerState, view);
            DrawRockets(worldState.RocketsState, view);
            DrawBombs(worldState.BombsState, view);
            DrawAliens(worldState.AliensState, view);

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

        private static void PrintGameProgress(GameProgressState gameProgressState)
        {
            Console.WriteLine();
            Console.WriteLine("Score: " + gameProgressState.Score.ToString() + "\tLives: " + gameProgressState.Lives.ToString());
        }

        public static void PrintWorld(WorldState worldState)
        {
            char[,] view = GenerateView(worldState);
            PrintView(view);
            PrintGameProgress(worldState.GameProgressState);
        }
    }
}
