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
            int randomSeed = (int)((DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
            QLearningAI ai1 = new QLearningAI(randomSeed + 0, 0.3f, 0.9f, 0.9f);
            RandomMoveAI ai2 = new RandomMoveAI(randomSeed + 1);

            const int numTrainGames = 100000;
            const int numEvaluateGames = 10000;

            TrainGames(ai1, ai2, numTrainGames);

            Simulate.WinnerStats winnerStats = Simulate.RunGames(Simulate.CreateNewGameState(), (gameState) => {
                switch (gameState.NextPlayer)
                {
                    case BoardState.Player.Player1:
                        return ai1.GetPlayerInput(gameState, false);
                    case BoardState.Player.Player2:
                        return ai2.GetPlayerInput(gameState);
                    default:
                        throw new Exception("Invalid NextPlayer");
                }
            }, numEvaluateGames);

            Display.PrintWinnerStats(winnerStats);

        }

        private static void TrainGames(QLearningAI ai1, RandomMoveAI ai2, int numGames)
        {
            GameState initialGameState = Simulate.CreateNewGameState();
            for (int game = 0; game < numGames; game++)
            {
                GameState gameState0 = initialGameState;
                while (true)
                {
                    PlayerInput playerInput0 = ai1.GetPlayerInput(gameState0, true);
                    GameState gameState1 = Simulate.Tick(gameState0, playerInput0);

                    if (gameState1.Winner != BoardState.Winner.None)
                    {
                        ai1.UpdateQ(gameState0.BoardState, playerInput0, gameState1.BoardState);
                        break;
                    }

                    PlayerInput playerInput1 = ai2.GetPlayerInput(gameState1);
                    GameState gameState2 = Simulate.Tick(gameState1, playerInput1);

                    if (gameState2.Winner != BoardState.Winner.None)
                    {
                        ai1.UpdateQ(gameState1.BoardState, playerInput1, gameState2.BoardState);
                        break;
                    }

                    ai1.UpdateQ(gameState0.BoardState, playerInput0, gameState2.BoardState);

                    gameState0 = gameState2;
                }
            }
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
