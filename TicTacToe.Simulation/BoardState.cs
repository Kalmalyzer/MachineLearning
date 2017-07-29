using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe.Simulation
{
    public class BoardState
    {
        public enum Player
        {
            None,
            Player1,
            Player2
        }

        public enum Winner
        {
            None,
            Player1,
            Player2,
            Draw
        }

        public const int Width = 3;
        public const int Height = 3;

        public readonly Player[,] Positions;

        public BoardState(Player[,] positions)
        {
            if (positions.GetLength(0) != Width || positions.GetLength(1) != Height)
                throw new ArgumentException("positions must be exactly " + Width.ToString() + "x" + Height.ToString() + " elements large");

            Positions = positions;
        }

        public static BoardState EmptyBoardState()
        {
            return new BoardState(new Player[Width, Height]);
        }

        public static BoardState MakeMove(BoardState boardState, Player player, int x, int y)
        {
            if (boardState.Positions[x, y] != Player.None)
                return null;
            else
            {
                Player[,] newPositions = (Player[,])boardState.Positions.Clone();
                newPositions[x, y] = player;
                return new BoardState(newPositions);
            }
        }

        private static Winner PlayerToWinner(Player player)
        {
            Dictionary<Player, Winner> playersToWinners = new Dictionary<Player, Winner>
            {
                { Player.None, Winner.None },
                { Player.Player1, Winner.Player1 },
                { Player.Player2, Winner.Player2 },
            };

            return playersToWinners[player];
        }

        private static Winner CheckForWinnerHorizontal(BoardState boardState, Player player, int y)
        {
            for (int x = 0; x < Width; x++)
                if (boardState.Positions[x, y] != player)
                    return Winner.None;
            return PlayerToWinner(player);
        }

        private static Winner CheckForWinnerHorizontal(BoardState boardState, Player player)
        {
            Winner winner;
            for (int y = 0; y < Height; y++)
                if ((winner = CheckForWinnerHorizontal(boardState, player, y)) != Winner.None)
                    return winner;
            return Winner.None;
        }

        private static Winner CheckForWinnerVertical(BoardState boardState, Player player, int x)
        {
            for (int y = 0; y < Height; y++)
                if (boardState.Positions[x, y] != player)
                    return Winner.None;
            return PlayerToWinner(player);
        }

        private static Winner CheckForWinnerVertical(BoardState boardState, Player player)
        {
            Winner winner;
            for (int x = 0; x < Width; x++)
                if ((winner = CheckForWinnerVertical(boardState, player, x)) != Winner.None)
                    return winner;
            return Winner.None;
        }

        private static Winner CheckForWinnerDiagonal(BoardState boardState, Player player, int x0, int y0, int dx, int dy, int length)
        {
            for (int i = 0; i < length; i++)
                if (boardState.Positions[x0 + dx * i, y0 + dy * i] != player)
                    return Winner.None;
            return PlayerToWinner(player);
        }

        private static Winner CheckForWinnerDiagonal(BoardState boardState, Player player)
        {
            Winner winner;
            if ((winner = CheckForWinnerDiagonal(boardState, player, 0, 0, 1, 1, Math.Min(Width, Height))) != Winner.None)
                return winner;
            if ((winner = CheckForWinnerDiagonal(boardState, player, 0, Height - 1, 1, -1, Math.Min(Width, Height))) != Winner.None)
                return winner;
            return Winner.None;
        }

        private static Winner CheckForDraw(BoardState boardState)
        {
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    if (boardState.Positions[x, y] == Player.None)
                        return Winner.None;

            return Winner.Draw;
        }

        private static Winner CheckForWinner(BoardState boardState, Player player)
        {
            Winner winner;
            if ((winner = CheckForWinnerHorizontal(boardState, player)) != Winner.None)
                return winner;
            if ((winner = CheckForWinnerVertical(boardState, player)) != Winner.None)
                return winner;
            if ((winner = CheckForWinnerDiagonal(boardState, player)) != Winner.None)
                return winner;
            if ((winner = CheckForDraw(boardState)) != Winner.None)
                return winner;
            return Winner.None;
        }

        public static Winner CheckForWinner(BoardState boardState)
        {
            Winner winner;
            if ((winner = CheckForWinner(boardState, Player.Player1)) != Winner.None)
                return winner;
            if ((winner = CheckForWinner(boardState, Player.Player2)) != Winner.None)
                return winner;
            return Winner.None;
        }
    }
}
