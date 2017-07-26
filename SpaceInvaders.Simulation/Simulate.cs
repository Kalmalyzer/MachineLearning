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
            List<Vector2i> positions = new List<Vector2i>();

            for (int y = 0; y < aliensHeight; y++)
                for (int x = 0; x < aliensWidth; x++)
                    positions.Add(new Vector2i(x, y));

            AliensState aliensState = new AliensState(topLeft, positions);

            return aliensState;
        }

        public static WorldState CreateNewWorldState(int width, int height, int maxRockets, int aliensWidth, int aliensHeight, int initialLives)
        {
            GameConfigState gameConfigState = new GameConfigState(width, height, maxRockets, aliensWidth, aliensHeight);
            PlayerState playerState = new PlayerState(width / 2);
            AliensState aliensState = CreateNewAliensState(width, height, aliensWidth, aliensHeight);
            AliensMovementState aliensMovementState = new AliensMovementState(AliensMovementState.MovementDirection.Right);
            RocketsState rocketsState = new RocketsState(new List<Vector2i>());
            BombsState bombsState = new BombsState(new List<Vector2i>());
            GameProgressState gameProgressState = new GameProgressState(0, initialLives, false);
            WorldState worldState = new WorldState(gameConfigState, playerState, aliensState, aliensMovementState, rocketsState, bombsState, gameProgressState);
            return worldState;
        }

        public enum PlayerInput
        {
            None,
            MoveLeft,
            MoveRight,
            Fire,
        }

        public static PlayerState TickPlayer(GameConfigState gameConfigState, PlayerState playerState, PlayerInput playerInput)
        {
            switch (playerInput)
            {
                case PlayerInput.MoveLeft:
                    return new PlayerState(Math.Max(playerState.Position - 1, 0));
                case PlayerInput.MoveRight:
                    return new PlayerState(Math.Min(playerState.Position + 1, gameConfigState.Width - 1));
                default:
                    return playerState;
            }
        }

        public static void TickAliensFiring(AliensState aliensState, out AliensFiringInput newAliensFiringInput)
        {
            Random r = new Random();

            if (r.Next(10) == 0)
            {
                List<int> bottomRow = aliensState.GetBottomRow();
                newAliensFiringInput = new AliensFiringInput(true, bottomRow[r.Next(bottomRow.Count)]);
            }
            else
                newAliensFiringInput = new AliensFiringInput(false, 0);
        }

        public static void TickAliens(GameConfigState gameConfigState, AliensState aliensState, AliensMovementState aliensMovementState,
            out AliensState newAliensState, out AliensMovementState newAliensMovementState, out AliensFiringInput newAliensFiringInput)
        {
            Vector2i topLeft, bottomRight;
            aliensState.GetAbsoluteBoundingBox(out topLeft, out bottomRight);
            AliensMovementState.MovementDirection nextMovementDirection = ChooseNewAliensMovementDirection(gameConfigState, aliensMovementState, topLeft, bottomRight);

            Dictionary<AliensMovementState.MovementDirection, Vector2i> movementDirectionToDelta =
                new Dictionary<AliensMovementState.MovementDirection, Vector2i>
                {
                    { AliensMovementState.MovementDirection.Left, new Vector2i(-1, 0) },
                    { AliensMovementState.MovementDirection.Right, new Vector2i(1, 0) },
                    { AliensMovementState.MovementDirection.Down, new Vector2i(0, 1) },
                };

            Vector2i movementDelta = movementDirectionToDelta[nextMovementDirection];

            newAliensState = new AliensState(aliensState.TopLeft + movementDelta, aliensState.RelativePositions);
            newAliensMovementState = new AliensMovementState(nextMovementDirection);

            TickAliensFiring(aliensState, out newAliensFiringInput);
        }

        private static AliensMovementState.MovementDirection ChooseNewAliensMovementDirection(GameConfigState gameConfigState, AliensMovementState aliensMovementState, Vector2i topLeft, Vector2i bottomRight)
        {
            switch (aliensMovementState.PreviousMovementDirection)
            {
                case AliensMovementState.MovementDirection.Left:
                    if (topLeft.X == 0)
                        return AliensMovementState.MovementDirection.Down;
                    else
                        return aliensMovementState.PreviousMovementDirection;

                case AliensMovementState.MovementDirection.Right:
                    if (bottomRight.X == gameConfigState.Width)
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

        public static RocketsState TickRockets(GameConfigState gameConfigState, RocketsState rocketsState, PlayerState playerState, PlayerInput playerInput)
        {
            List<Vector2i> newPositions = rocketsState.Positions.Select(position => position + new Vector2i(0, -1)).Where(position => position.Y >= 0).ToList();
            if (playerInput == PlayerInput.Fire && newPositions.Count < gameConfigState.MaxRockets)
                newPositions.Add(new Vector2i(playerState.Position, gameConfigState.Height - 1));
            return new RocketsState(newPositions);
        }

        public static BombsState TickBombs(GameConfigState gameConfigState, BombsState bombsState, PlayerState playerState, AliensState aliensState, AliensFiringInput aliensFiringInput)
        {
            List<Vector2i> newPositions = bombsState.Positions.Select(position => position + new Vector2i(0, 1)).Where(position => position.Y < gameConfigState.Height).ToList();
            if (aliensFiringInput.Fire)
                newPositions.Add(aliensState.TopLeft + aliensState.RelativePositions[aliensFiringInput.AlienIndex]);
            return new BombsState(newPositions);
        }

        private static List<Tuple<int, int>> FindAlienRocketCollisions(AliensState aliensState, RocketsState rocketsState)
        {
            List<Tuple<int, int>> collisions = new List<Tuple<int, int>>();

            for (int rocketIndex = 0; rocketIndex < rocketsState.Positions.Count; rocketIndex++)
            {
                Vector2i rocketPosition = rocketsState.Positions[rocketIndex];
                int alienIndex = aliensState.RelativePositions.FindIndex(alienRelativePosition => (alienRelativePosition + aliensState.TopLeft) == rocketPosition);
                if (alienIndex != -1)
                    collisions.Add(new Tuple<int, int>(alienIndex, rocketIndex));
            }

            return collisions;
        }

        private static List<int> FindPlayerBombCollisions(GameConfigState gameConfigState, BombsState bombsState, PlayerState playerState)
        {
            Vector2i playerPosition = new Vector2i(playerState.Position, gameConfigState.Height - 1);

            List<int> collisions = bombsState.Positions.Select((position, index) => new Tuple<Vector2i, int>(position, index))
                .Where(positionAndIndex => playerPosition == positionAndIndex.Item1)
                .Select(positionAndIndex => positionAndIndex.Item2)
                .ToList();

            return collisions;
        }

        private static AliensState ResolveAlienCollisions(AliensState aliensState, List<Tuple<int, int>> alienRocketCollisions)
        {
            if (alienRocketCollisions.Count != 0)
            {
                List<Vector2i> remainingPositions = aliensState.RelativePositions.Where((position, index) => !alienRocketCollisions.Exists(collision => index == collision.Item1)).ToList();
                return new AliensState(aliensState.TopLeft, remainingPositions);
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

        public static GameProgressState TickGameProgress(GameConfigState gameConfigState, GameProgressState gameProgressState, AliensState aliensState, int aliensKilled, bool playerCollidedWithBomb)
        {
            Vector2i topLeft, bottomRight;
            aliensState.GetAbsoluteBoundingBox(out topLeft, out bottomRight);

            int newScore = gameProgressState.Score + aliensKilled;
            int newLives = Math.Max(gameProgressState.Lives - (playerCollidedWithBomb ? 1 : 0), 0);
            bool newGameOver = gameProgressState.GameOver || (newLives == 0 || (bottomRight.Y >= gameConfigState.Height));

            return new GameProgressState(newScore, newLives, newGameOver);
        }

        public static WorldState Tick(WorldState worldState, PlayerInput playerInput)
        {
            PlayerState newPlayerState = TickPlayer(worldState.GameConfigState, worldState.PlayerState, playerInput);

            AliensState newAliensState;
            AliensMovementState newAliensMovementState;
            AliensFiringInput newAliensFiringInput;

            bool createNewAliens = (worldState.AliensState.RelativePositions.Count == 0);

            if (createNewAliens)
            {
                newAliensState = CreateNewAliensState(worldState.GameConfigState.Width, worldState.GameConfigState.Height,
                    worldState.GameConfigState.AliensWidth, worldState.GameConfigState.AliensHeight);
                newAliensMovementState = new AliensMovementState(AliensMovementState.MovementDirection.Right);
                newAliensFiringInput = new AliensFiringInput(false, 0);
            }
            else
                TickAliens(worldState.GameConfigState, worldState.AliensState, worldState.AliensMovementState, out newAliensState, out newAliensMovementState, out newAliensFiringInput);

            RocketsState newRocketsState = TickRockets(worldState.GameConfigState, worldState.RocketsState, worldState.PlayerState, playerInput);

            BombsState newBombsState = TickBombs(worldState.GameConfigState, worldState.BombsState, worldState.PlayerState, worldState.AliensState, newAliensFiringInput);

            List<Tuple<int, int>> alienRocketCollisions = FindAlienRocketCollisions(newAliensState, newRocketsState);
            List<int> playerBombCollisions = FindPlayerBombCollisions(worldState.GameConfigState, newBombsState, newPlayerState);

            AliensState newAliensState2 = ResolveAlienCollisions(newAliensState, alienRocketCollisions);
            RocketsState newRocketsState2 = ResolveRocketCollisions(newRocketsState, alienRocketCollisions);

            GameProgressState newGameProgressState = TickGameProgress(worldState.GameConfigState, worldState.GameProgressState, newAliensState2, alienRocketCollisions.Count, playerBombCollisions.Count != 0);

            return new WorldState(worldState.GameConfigState, newPlayerState, newAliensState2, newAliensMovementState, newRocketsState2, newBombsState, newGameProgressState);
        }
    }
}
