using SpaceInvaders.Simulation;
using System.Windows.Input;

namespace SpaceInvaders.Interactive
{
    public class KeyboardInput
    {
        public static Simulate.PlayerInput ReadPlayerInput()
        {
            if ((Keyboard.GetKeyStates(Key.Left) & KeyStates.Down) != 0)
                return Simulate.PlayerInput.MoveLeft;
            if ((Keyboard.GetKeyStates(Key.Right) & KeyStates.Down) != 0)
                return Simulate.PlayerInput.MoveRight;
            if ((Keyboard.GetKeyStates(Key.Space) & KeyStates.Down) != 0)
                return Simulate.PlayerInput.Fire;

            return Simulate.PlayerInput.None;
        }
    }
}
