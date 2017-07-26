using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceInvaders.Simulation
{
    public class GameConfigState
    {
        public readonly int Width;
        public readonly int Height;

        public readonly int MaxRockets;

        public readonly int AliensWidth;
        public readonly int AliensHeight;

        public GameConfigState(int width, int height, int maxRockets, int aliensWidth, int aliensHeight)
        {
            Width = width;
            Height = height;
            MaxRockets = maxRockets;
            AliensWidth = aliensWidth;
            AliensHeight = aliensHeight;
        }
    }
}
