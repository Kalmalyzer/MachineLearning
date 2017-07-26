using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceInvaders.Simulation
{
    public class BombsState
    {
        public readonly List<Vector2i> Positions;

        public BombsState(List<Vector2i> positions)
        {
            Positions = positions;
        }
    }
}
