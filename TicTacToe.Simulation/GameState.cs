using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe.Simulation
{
    public class GameState
    {
        public readonly BoardState BoardState;
        public readonly BoardState.Player NextPlayer;
        public readonly BoardState.Winner Winner;

        public GameState(BoardState boardState, BoardState.Player nextPlayer, BoardState.Winner winner)
        {
            BoardState = boardState;
            NextPlayer = nextPlayer;
            Winner = winner;
        }
    }
}
