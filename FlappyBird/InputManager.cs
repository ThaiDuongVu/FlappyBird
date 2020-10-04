using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FlappyBird
{
    internal class InputManager : Game
    {
        private static KeyboardState _currentKeyState;
        private static KeyboardState _previousKeyState;

        private static GamePadState _currentGamePadState;
        private static GamePadState _previousGamePadState;

        // Get current keyboard state
        public static KeyboardState GetKeyState()
        {
            _previousKeyState = _currentKeyState;
            _currentKeyState = Keyboard.GetState();

            return _currentKeyState;
        }

        // Get current gamepad state
        public static GamePadState GetGamePadState(PlayerIndex playerIndex)
        {
            _previousGamePadState = _currentGamePadState;
            _currentGamePadState = GamePad.GetState(playerIndex);

            return _currentGamePadState;
        }

        // If a key is press and hold
        public bool IsKeyHold(Keys key)
        {
            return _currentKeyState.IsKeyDown(key);
        }

        // If a button is pressed and hold
        public bool IsButtonHold(Buttons buttons)
        {
            return _currentGamePadState.IsButtonDown(buttons);
        }

        // If a key is pressed then let go
        public static bool IsKeyPressed(Keys key)
        {
            return _currentKeyState.IsKeyDown(key) && !_previousKeyState.IsKeyDown(key);
        }

        // If a button is pressed then let go
        public static bool IsButtonPressed(Buttons buttons)
        {
            return _currentGamePadState.IsButtonDown(buttons) && !_previousGamePadState.IsButtonDown(buttons);
        }
    }
}