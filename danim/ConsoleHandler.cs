using System;
using System.Runtime.InteropServices;
using System.Threading;


namespace danim
{
    public class ConsoleHandler
    {
        [DllImport("user32.dll")]
        public static extern bool GetAsyncKeyState(int button);
        public static bool IsMouseButtonPressed(MouseButton button)
        {
            return GetAsyncKeyState((int)button);
        }
        public enum MouseButton
        {
            LeftMouseButton = 0x01,
            RightMouseButton = 0x02,
            MiddleMouseButton = 0x04,
        }
    }
}