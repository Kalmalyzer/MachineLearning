using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceInvaders.Simulation
{
    public class WorldState
    {
        public readonly int Width;
        public readonly int Height;

        public readonly PlayerState PlayerState;

        public WorldState(int width, int height, PlayerState playerState)
        {
            Width = width;
            Height = height;

            PlayerState = playerState;
        }
    }
}
