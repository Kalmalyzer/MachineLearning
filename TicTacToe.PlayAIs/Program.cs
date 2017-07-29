using System;
using TicTacToe.AI;
using TicTacToe.Interactive;
using TicTacToe.Simulation;

namespace TicTacToe.PlayAIs
{
    class Program
    {
        static void Main(string[] args)
        {
            GameState initialGameState = Simulate.CreateNewGameState();

            int randomSeed = (int)((DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
            RandomMoveAI ai1 = new RandomMoveAI(randomSeed + 0);
            RandomMoveAI ai2 = new RandomMoveAI(randomSeed + 1);

            const int numGames = 100000;

            Simulate.WinnerStats winnerStats = Simulate.RunGames(initialGameState, (gameState) => {
                switch (gameState.NextPlayer)
                {
                    case BoardState.Player.Player1:
                        return ai1.GetPlayerInput(gameState);
                    case BoardState.Player.Player2:
                        return ai2.GetPlayerInput(gameState);
                    default:
                        throw new Exception("Invalid NextPlayer");
                }
            }, numGames);

            Display.PrintWinnerStats(winnerStats);

        }
        /*
        static void Main(string[] args)
        {
            GameState initialGameState = Simulate.CreateNewGameState();

            int randomSeed = (int)((DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
            RandomMoveAI ai1 = new RandomMoveAI(randomSeed + 0);
            RandomMoveAI ai2 = new RandomMoveAI(randomSeed + 1);

            GameState finalGameState = Simulate.RunGame(initialGameState, (gameState) => {
                switch (gameState.NextPlayer)
                {
                    case BoardState.Player.Player1:
                        return ai1.GetPlayerInput(gameState);
                    case BoardState.Player.Player2:
                        return ai2.GetPlayerInput(gameState);
                    default:
                        throw new Exception("Invalid NextPlayer");
                }
            });

            Display.PrintGame(finalGameState);

        }
        */
    }
}
