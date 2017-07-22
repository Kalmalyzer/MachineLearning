using SpaceInvaders.Simulation;

namespace SpaceInvaders.Interactive
{
    class Program
    {
        static void Main(string[] args)
        {
            int width = 40;
            int height = 20;

            WorldState worldState = Simulate.CreateNewWorldState(width, height);

            Display.PrintWorld(worldState);
        }
    }
}
