using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceInvaders.Simulation
{
    public class GameProgressState
    {
        public readonly int Score;
        public readonly int Lives;
        public readonly bool GameOver;

        public GameProgressState(int score, int lives, bool gameOver)
        {
            Score = score;
            Lives = lives;
            GameOver = gameOver;
        }
    }
}
