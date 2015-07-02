using System;
using OpenTK;
using OpenTK.Input;
using System.Drawing;

namespace OpenTkEngine.Core
{
    public class Input
    {
        private static KeyboardState prevKeyState = Keyboard.GetState();
        private static KeyboardState currentKeyState = prevKeyState;

        private static MouseState prevMouseState = Mouse.GetState();
        private static MouseState currentMouseState = prevMouseState;

        private static Vector2 currentMousePosition = new Vector2(currentMouseState.X, currentMouseState.Y);
        private static Vector2 prevMousePosition = currentMousePosition;

        public static void Update()
        {
            prevKeyState = currentKeyState;
            currentKeyState = Keyboard.GetState();

            prevMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            prevMousePosition = currentMousePosition;
            int mouseX = currentMouseState.X;
            int mouseY = currentMouseState.Y;
            currentMousePosition = new Vector2(mouseX, mouseY);
            
        }

        //
        // Keyboard Events
        //
        public static Boolean KeyDown(Key key)
        {
            return currentKeyState.IsKeyDown(key);
        }

        public static Boolean KeyUp(Key key)
        {
            return currentKeyState.IsKeyUp(key);
        }

        public static Boolean KeyTriggered(Key key)
        {
            return KeyDown(key) && prevKeyState.IsKeyUp(key);
        }

        public static Boolean KeyReleased(Key key)
        {
            return KeyUp(key) && prevKeyState.IsKeyDown(key);
        }

        //
        // Mouse Left Events
        //
        public static Boolean MouseLeftDown()
        {
            return currentMouseState.LeftButton == ButtonState.Pressed;
        }

        public static Boolean MouseLeftUp()
        {
            return currentMouseState.LeftButton == ButtonState.Released;
        }

        public static Boolean MouseLeftTriggered()
        {
            return MouseLeftDown() && prevMouseState.LeftButton == ButtonState.Released;
        }

        public static Boolean MouseLeftReleased()
        {
            return MouseLeftUp() && prevMouseState.LeftButton == ButtonState.Pressed;
        }

        //
        // Mouse Right Events
        //
        public static Boolean MouseRightDown()
        {
            return currentMouseState.RightButton == ButtonState.Pressed;
        }

        public static Boolean MouseRightUp()
        {
            return currentMouseState.RightButton == ButtonState.Released;
        }

        public static Boolean MouseRightTriggered()
        {
            return MouseRightDown() && prevMouseState.RightButton == ButtonState.Released;
        }

        public static Boolean MouseRightReleased()
        {
            return MouseRightUp() && prevMouseState.RightButton == ButtonState.Pressed;
        }

        //
        // Mouse Middle Events
        //
        public static Boolean MouseMiddleDown()
        {
            return currentMouseState.MiddleButton == ButtonState.Pressed;
        }

        public static Boolean MouseMiddleUp()
        {
            return currentMouseState.MiddleButton == ButtonState.Released;
        }

        public static Boolean MouseMiddleTriggered()
        {
            return MouseMiddleDown() && prevMouseState.MiddleButton == ButtonState.Released;
        }

        public static Boolean MouseMiddleReleased()
        {
            return MouseMiddleUp() && prevMouseState.MiddleButton == ButtonState.Pressed;
        }

        public static void CenterMouse()
        {
            //Rectangle bounds = StateHandler.GameWindow.Bounds;
            //Point center = new Point(bounds.Left + (bounds.Width / 2),bounds.Top + (bounds.Height / 2));
            //System.Windows.Forms.Cursor.Position = center;

            //int mouseX = currentMouseState.X;
            //int mouseY = currentMouseState.Y;
            //currentMousePosition = new Vector2(mouseX, mouseY);
            //prevMousePosition = currentMousePosition;
        }

        //
        // Public Mouse Variables
        //

        public static int GetRelativeMouseX()
        {
            return Global.window.Mouse.X;
        }

        public static int GetRelativeMouseY()
        {
            return Global.window.Mouse.Y;
        }

        public static Vector2 GetRelativeMousePosition()
        {
            return new Vector2(GetRelativeMouseX(), GetRelativeMouseY());
        }

        public static int GetMouseX()
        {
            return (int)currentMousePosition.X;
        }

        public static int GetMouseY()
        {
            return (int)currentMousePosition.Y;
        }

        public static Vector2 GetMousePosition()
        {
            return new Vector2(GetMouseX(), GetMouseY());
        }

        public static int GetMouseLastX()
        {
            return (int)prevMousePosition.X;
        }

        public static int GetMouseLastY()
        {
            return (int)prevMousePosition.Y;
        }

        public static Vector2 GetMouseLastPosition()
        {
            return new Vector2(GetMouseLastX(), GetMouseLastY());
        }

        public static int GetMouseScrolledX()
        {
            return GetMouseX() - GetMouseLastX();
        }

        public static int GetMouseScrolledY()
        {
            return GetMouseY() - GetMouseLastY();
        }

        public static Vector2 GetMouseScroll()
        {
            return new Vector2(GetMouseScrolledX(), GetMouseScrolledY());
        }

        public static int GetMouseWheelScroll()
        {
            return currentMouseState.ScrollWheelValue;
        }
    }
}