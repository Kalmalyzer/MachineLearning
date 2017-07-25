using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceInvaders.Simulation
{
    public class RocketsState
    {
        public readonly List<Vector2i> Positions;

        public RocketsState(List<Vector2i> positions)
        {
            Positions = positions;
        }
    }
}
