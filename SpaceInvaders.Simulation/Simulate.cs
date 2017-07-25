using System;
using System.Collections.Generic;
using System.Linq;
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

        public static WorldState CreateNewWorldState(int width, int height, int maxRockets, int aliensWidth, int aliensHeight)
        {
            PlayerState playerState = new PlayerState(width / 2);
            AliensState aliensState = CreateNewAliensState(width, height, aliensWidth, aliensHeight);
            AliensMovementState aliensMovementState = new AliensMovementState(AliensMovementState.MovementDirection.Right);
            RocketsState rocketsState = new RocketsState(new List<Vector2i>());
            WorldState worldState = new WorldState(width, height, maxRockets, playerState, aliensState, aliensMovementState, rocketsState);
            return worldState;
        }

        public enum PlayerInput
        {
            None,
            MoveLeft,
            MoveRight,
            Fire,
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
            AliensMovementState.MovementDirection nextMovementDirection = ChooseNewAliensMovementDirection(worldState, aliensMovementState, topLeft, bottomRight);

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

        private static AliensMovementState.MovementDirection ChooseNewAliensMovementDirection(WorldState worldState, AliensMovementState aliensMovementState, Vector2i topLeft, Vector2i bottomRight)
        {
            switch (aliensMovementState.PreviousMovementDirection)
            {
                case AliensMovementState.MovementDirection.Left:
                    if (topLeft.X == 0)
                        return AliensMovementState.MovementDirection.Down;
                    else
                        return aliensMovementState.PreviousMovementDirection;

                case AliensMovementState.MovementDirection.Right:
                    if (bottomRight.X == worldState.Width)
                        return AliensMovementState.MovementDirection.Down;
                    else
                        return aliensMovementState.PreviousMovementDirection;

                case AliensMovementState.MovementDirection.Down:
                    if (topLeft.X == 0)
                        return AliensMovementState.MovementDirection.Right;
                    else
                        return AliensMovementState.MovementDirection.Left;

                default:
                    throw new NotImplementedException();
            }
        }

        public static RocketsState TickRockets(WorldState worldState, RocketsState rocketsState, PlayerState playerState, PlayerInput playerInput)
        {
            List<Vector2i> newPositions = rocketsState.Positions.Select(position => position + new Vector2i(0, -1)).Where(position => position.Y >= 0).ToList();
            if (playerInput == PlayerInput.Fire && newPositions.Count < worldState.MaxRockets)
                newPositions.Add(new Vector2i(playerState.Position, worldState.Height - 1));
            return new RocketsState(newPositions);
        }

        private static List<Tuple<int, int>> FindAlienRocketCollisions(AliensState aliensState, RocketsState rocketsState)
        {
            List<Vector2i> alienPositions = aliensState.GetPresentAliens();

            List<Tuple<int, int>> collisions = new List<Tuple<int, int>>();

            for (int rocketIndex = 0; rocketIndex < rocketsState.Positions.Count; rocketIndex++)
            {
                Vector2i rocketPosition = rocketsState.Positions[rocketIndex];
                int alienIndex = alienPositions.FindIndex(alienPosition => alienPosition == rocketPosition);
                if (alienIndex != -1)
                    collisions.Add(new Tuple<int, int>(alienIndex, rocketIndex));
            }

            return collisions;
        }

        private static AliensState ResolveAlienCollisions(AliensState aliensState, List<Tuple<int, int>> alienRocketCollisions)
        {
            List<Vector2i> alienPositions = aliensState.GetPresentAliens();

            if (alienRocketCollisions.Count != 0)
            {
                bool[,] newPresent = (bool[,])aliensState.Present.Clone();
                foreach (Tuple<int, int> collision in alienRocketCollisions)
                {
                    Vector2i collisionPosition = alienPositions[collision.Item1];
                    newPresent[collisionPosition.X - aliensState.TopLeft.X, collisionPosition.Y - aliensState.TopLeft.Y] = false;
                }

                return new AliensState(aliensState.TopLeft, newPresent);
            }
            else
                return aliensState;
        }

        private static RocketsState ResolveRocketCollisions(RocketsState rocketsState, List<Tuple<int, int>> alienRocketCollisions)
        {
            if (alienRocketCollisions.Count != 0)
            {
                List<Vector2i> remainingPositions = rocketsState.Positions.Where((position, index) => !alienRocketCollisions.Exists(collision => index == collision.Item2)).ToList();
                return new RocketsState(remainingPositions);
            }
            else
                return rocketsState;
        }

        public static WorldState Tick(WorldState worldState, PlayerInput playerInput)
        {
            PlayerState newPlayerState = TickPlayer(worldState, worldState.PlayerState, playerInput);

            AliensState newAliensState;
            AliensMovementState newAliensMovementState;
            TickAliens(worldState, worldState.AliensState, worldState.AliensMovementState, out newAliensState, out newAliensMovementState);

            RocketsState newRocketsState = TickRockets(worldState, worldState.RocketsState, worldState.PlayerState, playerInput);

            List<Tuple<int, int>> alienRocketCollisions = FindAlienRocketCollisions(newAliensState, newRocketsState);

            AliensState newAliensState2 = ResolveAlienCollisions(newAliensState, alienRocketCollisions);
            RocketsState newRocketsState2 = ResolveRocketCollisions(newRocketsState, alienRocketCollisions);

            return new WorldState(worldState.Width, worldState.Height, worldState.MaxRockets, newPlayerState, newAliensState2, newAliensMovementState, newRocketsState2);
        }
    }
}
