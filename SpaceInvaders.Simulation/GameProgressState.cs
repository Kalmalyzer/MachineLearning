using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceInvaders.Simulation
{
    public class GameProgressState
    {
        public readonly int Score;
        public readonly int Lives;

        public GameProgressState(int score, int lives)
        {
            Score = score;
            Lives = lives;
        }
    }
}
