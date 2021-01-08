using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FlappyBird
{
    internal class InputManager : Game
    {
        private static KeyboardState currentKeyState;
        private static KeyboardState previousKeyState;

        private static GamePadState currentGamePadState;
        private static GamePadState previousGamePadState;

        /// <summary>
        /// Get current keyboard state.
        /// </summary>
        /// <returns></returns>
        public static KeyboardState GetKeyState()
        {
            previousKeyState = currentKeyState;
            currentKeyState = Keyboard.GetState();

            return currentKeyState;
        }

        /// <summary>
        /// Get current gamepad state.
        /// </summary>
        /// <param name="playerIndex">Gamepad index</param>
        /// <returns></returns>
        public static GamePadState GetGamePadState(PlayerIndex playerIndex)
        {
            previousGamePadState = currentGamePadState;
            currentGamePadState = GamePad.GetState(playerIndex);

            return currentGamePadState;
        }

        /// <summary>
        /// Returns true if a key is hold down.
        /// </summary>
        /// <param name="keys">Keys to check</param>
        /// <returns></returns>
        public bool IsKeyDown(Keys keys)
        {
            return currentKeyState.IsKeyDown(keys);
        }

        /// <summary>
        /// Returns true if a button is hold down.
        /// </summary>
        /// <param name="buttons">Buttons to check</param>
        /// <returns></returns>
        public bool IsButtonDown(Buttons buttons)
        {
            return currentGamePadState.IsButtonDown(buttons);
        }

        /// <summary>
        /// Returns true if a key is pressed then let go.
        /// </summary>
        /// <param name="keys">Keys to check</param>
        /// <returns></returns>
        public static bool OnKeyDown(Keys keys)
        {
            return currentKeyState.IsKeyDown(keys) && !previousKeyState.IsKeyDown(keys);
        }

        /// <summary>
        /// Returns true if a button is pressed then let go.
        /// </summary>
        /// <param name="buttons">Buttons to check</param>
        /// <returns></returns>
        public static bool OnButtonDown(Buttons buttons)
        {
            return currentGamePadState.IsButtonDown(buttons) && !previousGamePadState.IsButtonDown(buttons);
        }
    }
}