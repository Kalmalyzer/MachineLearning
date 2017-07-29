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

        private static Player CheckForWinnerHorizontal(BoardState boardState, Player player, int y)
        {
            for (int x = 0; x < Width; x++)
                if (boardState.Positions[x, y] != player)
                    return Player.None;
            return player;
        }

        private static Player CheckForWinnerHorizontal(BoardState boardState, Player player)
        {
            Player winner;
            for (int y = 0; y < Height; y++)
                if ((winner = CheckForWinnerHorizontal(boardState, player, y)) != Player.None)
                    return winner;
            return Player.None;
        }

        private static Player CheckForWinnerVertical(BoardState boardState, Player player, int x)
        {
            for (int y = 0; y < Height; y++)
                if (boardState.Positions[x, y] != player)
                    return Player.None;
            return player;
        }

        private static Player CheckForWinnerVertical(BoardState boardState, Player player)
        {
            Player winner;
            for (int x = 0; x < Width; x++)
                if ((winner = CheckForWinnerVertical(boardState, player, x)) != Player.None)
                    return winner;
            return Player.None;
        }

        private static Player CheckForWinnerDiagonal(BoardState boardState, Player player, int x0, int y0, int dx, int dy, int length)
        {
            for (int i = 0; i < length; i++)
                if (boardState.Positions[x0 + dx * i, y0 + dy * i] != player)
                    return Player.None;
            return player;
        }

        private static Player CheckForWinnerDiagonal(BoardState boardState, Player player)
        {
            Player winner;
            if ((winner = CheckForWinnerDiagonal(boardState, player, 0, 0, 1, 1, Math.Min(Width, Height))) != Player.None)
                return winner;
            if ((winner = CheckForWinnerDiagonal(boardState, player, 0, Height - 1, 1, -1, Math.Min(Width, Height))) != Player.None)
                return winner;
            return Player.None;
        }

        private static Player CheckForWinner(BoardState boardState, Player player)
        {
            Player winner;
            if ((winner = CheckForWinnerHorizontal(boardState, player)) != Player.None)
                return winner;
            if ((winner = CheckForWinnerVertical(boardState, player)) != Player.None)
                return winner;
            if ((winner = CheckForWinnerDiagonal(boardState, player)) != Player.None)
                return winner;
            return Player.None;
        }

        public static Player CheckForWinner(BoardState boardState)
        {
            Player winner;
            if ((winner = CheckForWinner(boardState, Player.Player1)) != Player.None)
                return winner;
            if ((winner = CheckForWinner(boardState, Player.Player2)) != Player.None)
                return winner;
            return Player.None;
        }
    }
}
