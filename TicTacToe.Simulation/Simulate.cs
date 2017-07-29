using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe.Simulation
{
    public class Simulate
    {
        public static GameState CreateNewGameState()
        {
            return new GameState(BoardState.EmptyBoardState(), BoardState.Player.Player1, BoardState.Winner.None);
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
            BoardState.Winner newWinner = BoardState.CheckForWinner(newBoardState);

            GameState newGameState = new GameState(newBoardState, newNextPlayer, newWinner);
            return newGameState;
        }

        public delegate PlayerInput GeneratePlayerInputDelegate(GameState gameState);

        public static GameState RunGame(GameState gameState, GeneratePlayerInputDelegate generatePlayerInput)
        {
            GameState nextGameState = gameState;
            while (true)
            {
                PlayerInput playerInput = generatePlayerInput(nextGameState);
                GameState newGameState = Tick(nextGameState, playerInput);
                nextGameState = newGameState;

                if (newGameState.Winner != BoardState.Winner.None)
                    return newGameState;
            }
        }

        public struct WinnerStats
        {
            public int Player1Wins;
            public int Player2Wins;
            public int Draws;
        }

        public static WinnerStats RunGames(GameState gameState, GeneratePlayerInputDelegate generatePlayerInput, int numGames)
        {
            WinnerStats winnerStats = new WinnerStats();

            for (int game = 0; game < numGames; game++)
            {
                GameState finalGameState = RunGame(gameState, generatePlayerInput);
                switch (finalGameState.Winner)
                {
                    case BoardState.Winner.Player1:
                        winnerStats.Player1Wins++;
                        break;
                    case BoardState.Winner.Player2:
                        winnerStats.Player2Wins++;
                        break;
                    case BoardState.Winner.Draw:
                        winnerStats.Draws++;
                        break;
                    default:
                        throw new Exception("Invalid Winner");
                }
            }

            return winnerStats;
        }
    }
}
