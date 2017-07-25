﻿using SpaceInvaders.Simulation;
using System;
using System.Collections.Generic;

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

        private static void DrawAliens(WorldState worldState, AliensState aliensState, char[,] view)
        {
            List<Vector2i> alienPositions = aliensState.GetPresentAliens();
            foreach (Vector2i alienPosition in alienPositions)
                view[alienPosition.X, alienPosition.Y] = 'A';
        }

        private static void DrawRockets(RocketsState rocketsState, char[,] view)
        {
            foreach (Vector2i rocketPosition in rocketsState.Positions)
                view[rocketPosition.X, rocketPosition.Y] = 'R';
        }

        private static char[,] GenerateView(WorldState worldState)
        {
            char[,] view = new char[worldState.Width, worldState.Height];

            ClearView(view);
            DrawPlayer(worldState, worldState.PlayerState, view);
            DrawRockets(worldState.RocketsState, view);
            DrawAliens(worldState, worldState.AliensState, view);

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
