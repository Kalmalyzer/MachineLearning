using SpaceInvaders.Simulation;
using System;
using System.Threading;

namespace SpaceInvaders.Interactive
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            int width = 40;
            int height = 20;
            int maxRockets = 3;

            WorldState worldState = Simulate.CreateNewWorldState(width, height, maxRockets, width / 2, height / 4);

            while (true)
            {
                Simulate.PlayerInput playerInput = KeyboardInput.ReadPlayerInput();
                Display.PrintWorld(worldState);
                worldState = Simulate.Tick(worldState, playerInput);
                Thread.Sleep(250);
            }
        }
    }
}
