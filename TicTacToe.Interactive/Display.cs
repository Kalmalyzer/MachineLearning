using System;
using System.Collections.Generic;
using TicTacToe.Simulation;

namespace TicTacToe.Interactive
{
    public class Display
    {
        private static void PrintBoard(BoardState boardState)
        {
            Dictionary<BoardState.Player, char> PlayersToChars = new Dictionary<BoardState.Player, char>
            {
                { BoardState.Player.None, ' ' },
                { BoardState.Player.Player1, '1' },
                { BoardState.Player.Player2, '2' },
            };

            Console.WriteLine("+" + new String('-', boardState.Positions.GetLength(0)) + "+");
            for (int y = 0; y < boardState.Positions.GetLength(1); y++)
            {
                string row = "|";
                for (int x = 0; x < boardState.Positions.GetLength(0); x++)
                    row += PlayersToChars[boardState.Positions[x, y]];
                row += "|";
                Console.WriteLine(row);
            }
            Console.WriteLine("+" + new String('-', boardState.Positions.GetLength(0)) + "+");
            Console.WriteLine();
        }

        private static void PrintPlayerInfo(GameState gameState)
        {
            if (gameState.Winner != BoardState.Winner.None)
                Console.WriteLine("Winner: " + gameState.Winner.ToString());
            else
                Console.WriteLine("Next player: " + gameState.NextPlayer.ToString());
            Console.WriteLine();
        }

        public static void PrintGame(GameState gameState)
        {
            PrintBoard(gameState.BoardState);
            PrintPlayerInfo(gameState);
        }

        public static void PrintWinnerStats(Simulate.WinnerStats winnerStats)
        {
            int total = winnerStats.Player1Wins + winnerStats.Player2Wins + winnerStats.Draws;
            Console.WriteLine("Player 1 wins: " + winnerStats.Player1Wins.ToString() + " (" + (winnerStats.Player1Wins * 100 / total).ToString() + "%)");
            Console.WriteLine("Player 2 wins: " + winnerStats.Player2Wins.ToString() + " (" + (winnerStats.Player2Wins * 100 / total).ToString() + "%)");
            Console.WriteLine("Draws: " + winnerStats.Draws.ToString() + " (" + (winnerStats.Draws * 100 / total).ToString() + "%)");
        }
    }
}
