using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceInvaders.Simulation
{
    public class AliensFiringInput
    {
        public readonly bool Fire;
        public readonly int AlienIndex;

        public AliensFiringInput(bool fire, int alienIndex)
        {
            Fire = fire;
            AlienIndex = alienIndex;
        }
    }
}
