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
        public readonly AliensState AliensState;
        public readonly AliensMovementState AliensMovementState;

        public WorldState(int width, int height, PlayerState playerState, AliensState aliensState, AliensMovementState aliensMovementState)
        {
            Width = width;
            Height = height;

            PlayerState = playerState;
            AliensState = aliensState;

            AliensMovementState = aliensMovementState;
        }
    }
}
