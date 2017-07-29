using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe.Simulation
{
    public class PlayerInput
    {
        public readonly BoardState.Player Player;
        public readonly int X;
        public readonly int Y;

        public PlayerInput(BoardState.Player player, int x, int y)
        {
            Player = player;
            X = x;
            Y = y;
        }
    }
}
