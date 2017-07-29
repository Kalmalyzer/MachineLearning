using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe.Simulation
{
    public class Simulate
    {
        public static GameState CreateNewGameState()
        {
            return new GameState(BoardState.EmptyBoardState(), BoardState.Player.Player1, BoardState.Player.None);
        }

        private static BoardState.Player NextPlayer(BoardState.Player player)
        {
            switch (player)
            {
                case BoardState.Player.Player1:
                    return BoardState.Player.Player2;
                case BoardState.Player.Player2:
                    return BoardState.Player.Player1;
                default:
                    throw new ArgumentException("Invalid player");
            }
        }

        public static GameState Tick(GameState gameState, PlayerInput playerInput)
        {
            if (playerInput.Player != gameState.NextPlayer)
                throw new ArgumentException("Attempted to move out-of-turn");
            BoardState newBoardState = BoardState.MakeMove(gameState.BoardState, playerInput.Player, playerInput.X, playerInput.Y);
            BoardState.Player newNextPlayer = NextPlayer(gameState.NextPlayer);
            BoardState.Player newWinner = BoardState.CheckForWinner(newBoardState);

            GameState newGameState = new GameState(newBoardState, newNextPlayer, newWinner);
            return newGameState;
        }

    }
}
