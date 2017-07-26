﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceInvaders.Simulation
{
    public class WorldState
    {
        public readonly int Width;
        public readonly int Height;
        public readonly int MaxRockets;

        public readonly PlayerState PlayerState;
        public readonly AliensState AliensState;
        public readonly AliensMovementState AliensMovementState;
        public readonly RocketsState RocketsState;
        public readonly BombsState BombsState;

        public WorldState(int width, int height, int maxRockets, PlayerState playerState, AliensState aliensState, AliensMovementState aliensMovementState,
            RocketsState rocketsState, BombsState bombsState)
        {
            Width = width;
            Height = height;

            MaxRockets = maxRockets;

            PlayerState = playerState;
            AliensState = aliensState;

            AliensMovementState = aliensMovementState;

            RocketsState = rocketsState;

            BombsState = bombsState;
        }
    }
}
