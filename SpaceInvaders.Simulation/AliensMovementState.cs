using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceInvaders.Simulation
{
    public class AliensMovementState
    {
        public enum MovementDirection { Left, Right, Down };

        public readonly MovementDirection PreviousMovementDirection;

        public AliensMovementState(MovementDirection movementDirection)
        {
            PreviousMovementDirection = movementDirection;
        }
    }
}
