using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceInvaders.Simulation
{
    public class WorldState
    {
        public readonly GameConfigState GameConfigState;
        public readonly PlayerState PlayerState;
        public readonly AliensState AliensState;
        public readonly AliensMovementState AliensMovementState;
        public readonly RocketsState RocketsState;
        public readonly BombsState BombsState;
        public readonly GameProgressState GameProgressState;

        public WorldState(GameConfigState gameConfigState, PlayerState playerState, AliensState aliensState, AliensMovementState aliensMovementState,
            RocketsState rocketsState, BombsState bombsState, GameProgressState gameProgressState)
        {
            GameConfigState = gameConfigState;
            PlayerState = playerState;
            AliensState = aliensState;

            AliensMovementState = aliensMovementState;

            RocketsState = rocketsState;

            BombsState = bombsState;

            GameProgressState = gameProgressState;
        }
    }
}
