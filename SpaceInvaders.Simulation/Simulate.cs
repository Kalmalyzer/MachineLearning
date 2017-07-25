using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceInvaders.Simulation
{
    public class Simulate
    {
        private static AliensState CreateNewAliensState(int width, int height, int aliensWidth, int aliensHeight)
        {
            Vector2i topLeft = new Vector2i((width - aliensWidth) / 2, 0);
            bool[,] present = new bool[aliensWidth, aliensHeight];

            for (int y = 0; y < aliensHeight; y++)
                for (int x = 0; x < aliensWidth; x++)
                    present[x, y] = true;

            AliensState aliensState = new AliensState(topLeft, present);

            return aliensState;
        }

        public static WorldState CreateNewWorldState(int width, int height, int aliensWidth, int aliensHeight)
        {
            PlayerState playerState = new PlayerState(width / 2);
            AliensState aliensState = CreateNewAliensState(width, height, aliensWidth, aliensHeight);
            AliensMovementState aliensMovementState = new AliensMovementState(AliensMovementState.MovementDirection.Right);
            WorldState worldState = new WorldState(width, height, playerState, aliensState, aliensMovementState);
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

        public static void TickAliens(WorldState worldState, AliensState aliensState, AliensMovementState aliensMovementState,
            out AliensState newAliensState, out AliensMovementState newAliensMovementState)
        {
            Vector2i topLeft, bottomRight;
            aliensState.GetAbsoluteBoundingBox(out topLeft, out bottomRight);

            AliensMovementState.MovementDirection nextMovementDirection;

            switch (aliensMovementState.PreviousMovementDirection)
            {
                case AliensMovementState.MovementDirection.Left:
                    if (topLeft.X == 0)
                        nextMovementDirection = AliensMovementState.MovementDirection.Down;
                    else
                        nextMovementDirection = aliensMovementState.PreviousMovementDirection;
                    break;
                case AliensMovementState.MovementDirection.Right:
                    if (bottomRight.X == worldState.Width)
                        nextMovementDirection = AliensMovementState.MovementDirection.Down;
                    else
                        nextMovementDirection = aliensMovementState.PreviousMovementDirection;
                    break;
                case AliensMovementState.MovementDirection.Down:
                    if (topLeft.X == 0)
                        nextMovementDirection = AliensMovementState.MovementDirection.Right;
                    else
                        nextMovementDirection = AliensMovementState.MovementDirection.Left;
                    break;
                default:
                    throw new NotImplementedException();
            }

            Dictionary<AliensMovementState.MovementDirection, Vector2i> movementDirectionToDelta =
                new Dictionary<AliensMovementState.MovementDirection, Vector2i>
                {
                    { AliensMovementState.MovementDirection.Left, new Vector2i(-1, 0) },
                    { AliensMovementState.MovementDirection.Right, new Vector2i(1, 0) },
                    { AliensMovementState.MovementDirection.Down, new Vector2i(0, 1) },
                };

            Vector2i movementDelta = movementDirectionToDelta[nextMovementDirection];

            newAliensState = new AliensState(aliensState.TopLeft + movementDelta, aliensState.Present);
            newAliensMovementState = new AliensMovementState(nextMovementDirection);
        }

        public static WorldState Tick(WorldState worldState, PlayerInput playerInput)
        {
            PlayerState newPlayerState = TickPlayer(worldState, worldState.PlayerState, playerInput);
            AliensState newAliensState;
            AliensMovementState newAliensMovementState;
            TickAliens(worldState, worldState.AliensState, worldState.AliensMovementState, out newAliensState, out newAliensMovementState);

            return new WorldState(worldState.Width, worldState.Height, newPlayerState, newAliensState, newAliensMovementState);
        }
    }
}
