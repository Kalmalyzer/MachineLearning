using System;
using System.Collections.Generic;
using System.Text;
using TicTacToe.Simulation;

namespace TicTacToe.AI
{
    public class RandomMoveAI
    {
        private Random random;

        public RandomMoveAI(int seed)
        {
            random = new Random(seed);
        }

        private static List<Tuple<int, int>> FindAvailablePositions(BoardState boardState)
        {
            List<Tuple<int, int>> availablePositions = new List<Tuple<int, int>>();

            for (int y = 0; y < boardState.Positions.GetLength(1); y++)
                for (int x = 0; x < boardState.Positions.GetLength(0); x++)
                    if (boardState.Positions[x, y] == BoardState.Player.None)
                        availablePositions.Add(new Tuple<int, int>(x, y));

            return availablePositions;
        }

        public PlayerInput GetPlayerInput(GameState gameState)
        {
            List<Tuple<int, int>> availablePositions = FindAvailablePositions(gameState.BoardState);

            Tuple<int, int> position = availablePositions[random.Next(availablePositions.Count)];
            return new PlayerInput(gameState.NextPlayer, position.Item1, position.Item2);
        }
    }
}
