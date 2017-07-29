using TicTacToe.Interactive;
using TicTacToe.Simulation;

namespace TicTacToe.PlayAgainstHuman
{
    class Program
    {
        static void Main(string[] args)
        {
            GameState gameState = Simulate.CreateNewGameState();

            while (gameState.Winner == BoardState.Player.None)
            {
                Display.PrintGame(gameState);
                PlayerInput playerInput = KeyboardInput.GetPlayerInput(gameState);
                gameState = Simulate.Tick(gameState, playerInput);
            }

            Display.PrintGame(gameState);
        }
    }
}
