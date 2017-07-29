using System;
using TicTacToe.AI;
using TicTacToe.Interactive;
using TicTacToe.Simulation;

namespace TicTacToe.PlayAgainstRandomMoveAI
{
    class Program
    {
        static void Main(string[] args)
        {
            GameState gameState = Simulate.CreateNewGameState();

            int randomSeed = (int) ((DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
            RandomMoveAI ai = new RandomMoveAI(randomSeed);

            while (gameState.Winner == BoardState.Player.None)
            {
                Display.PrintGame(gameState);

                PlayerInput playerInput;
                if (gameState.NextPlayer == BoardState.Player.Player1)
                    playerInput = KeyboardInput.GetPlayerInput(gameState);
                else
                    playerInput = ai.GetPlayerInput(gameState);

                gameState = Simulate.Tick(gameState, playerInput);
            }

            Display.PrintGame(gameState);
        }
    }
}
