using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceInvaders.Simulation
{
    public class Simulate
    {
        public static WorldState CreateNewWorldState(int width, int height)
        {
            PlayerState playerState = new PlayerState(width / 2);
            WorldState worldState = new WorldState(width, height, playerState);
            return worldState;
        }

        public enum PlayerInput
        {
            None,
            MoveLeft,
            MoveRight,
        }

        public static PlayerState TickPlayer(WorldState worldState, PlayerState playerState, PlayerInput playerInput)
        {
            switch (playerInput)
            {
                case PlayerInput.MoveLeft:
                    return new PlayerState(Math.Max(playerState.Position - 1, 0));
                case PlayerInput.MoveRight:
                    return new PlayerState(Math.Min(playerState.Position + 1, worldState.Width - 1));
                default:
                    return playerState;
            }
        }

        public static WorldState Tick(WorldState worldState, PlayerInput playerInput)
        {
            PlayerState newPlayerState = TickPlayer(worldState, worldState.PlayerState, playerInput);

            return new WorldState(worldState.Width, worldState.Height, newPlayerState);
        }
    }
}
