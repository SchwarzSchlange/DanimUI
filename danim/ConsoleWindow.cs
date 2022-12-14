using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using danim;
using Component = danim.Component;
using System.Windows.Forms;
using System.Security.Cryptography.X509Certificates;
using Button = danim.Button;

public static class ConsoleWindow
{
    private static class NativeFunctions
    {
        public enum StdHandle : int
        {
            STD_INPUT_HANDLE = -10,
            STD_OUTPUT_HANDLE = -11,
            STD_ERROR_HANDLE = -12,
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetStdHandle(int nStdHandle); //returns Handle

        public enum ConsoleMode : uint
        {
            ENABLE_ECHO_INPUT = 0x0004,
            ENABLE_EXTENDED_FLAGS = 0x0080,
            ENABLE_INSERT_MODE = 0x0020,
            ENABLE_LINE_INPUT = 0x0002,
            ENABLE_MOUSE_INPUT = 0x0010,
            ENABLE_PROCESSED_INPUT = 0x0001,
            ENABLE_QUICK_EDIT_MODE = 0x0040,
            ENABLE_WINDOW_INPUT = 0x0008,
            ENABLE_VIRTUAL_TERMINAL_INPUT = 0x0200,

            //screen buffer handle
            ENABLE_PROCESSED_OUTPUT = 0x0001,
            ENABLE_WRAP_AT_EOL_OUTPUT = 0x0002,
            ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004,
            DISABLE_NEWLINE_AUTO_RETURN = 0x0008,
            ENABLE_LVB_GRID_WORLDWIDE = 0x0010
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);
    }

    public static void QuickEditMode(bool Enable)
    {
        IntPtr consoleHandle = NativeFunctions.GetStdHandle((int)NativeFunctions.StdHandle.STD_INPUT_HANDLE);
        UInt32 consoleMode;

        NativeFunctions.GetConsoleMode(consoleHandle, out consoleMode);
        if (Enable)
            consoleMode |= ((uint)NativeFunctions.ConsoleMode.ENABLE_QUICK_EDIT_MODE);
        else
            consoleMode &= ~((uint)NativeFunctions.ConsoleMode.ENABLE_QUICK_EDIT_MODE);

        consoleMode |= ((uint)NativeFunctions.ConsoleMode.ENABLE_EXTENDED_FLAGS);

        NativeFunctions.SetConsoleMode(consoleHandle, consoleMode);
    }

    private const int MF_BYCOMMAND = 0x00000000;
    public const int SC_CLOSE = 0xF060;
    public const int SC_MINIMIZE = 0xF020;
    public const int SC_MAXIMIZE = 0xF030;
    public const int SC_SIZE = 0xF000;

    [DllImport("user32.dll")]
    public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

    [DllImport("user32.dll")]
    private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

    [DllImport("kernel32.dll", ExactSpelling = true)]
    private static extern IntPtr GetConsoleWindow();

    public static void CloseResize()
    {
        IntPtr handle = GetConsoleWindow();
        IntPtr sysMenu = GetSystemMenu(handle, false);

        if (handle != IntPtr.Zero)
        {
            //DeleteMenu(sysMenu, SC_CLOSE, MF_BYCOMMAND);
            DeleteMenu(sysMenu, SC_MINIMIZE, MF_BYCOMMAND);
            DeleteMenu(sysMenu, SC_MAXIMIZE, MF_BYCOMMAND);
            DeleteMenu(sysMenu, SC_SIZE, MF_BYCOMMAND);
        }

    }


    [DllImport("USER32.DLL")]
    public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
    static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

    [DllImport("user32.dll")]
    static extern bool DrawMenuBar(IntPtr hWnd);

    [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
    public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

    [DllImport("user32", ExactSpelling = true, SetLastError = true)]
    internal static extern int MapWindowPoints(IntPtr hWndFrom, IntPtr hWndTo, [In, Out] ref RECT rect, [MarshalAs(UnmanagedType.U4)] int cPoints);

    [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
    public static extern IntPtr GetDesktopWindow();

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left, top, bottom, right;
    }


    private const int GWL_STYLE = -16;              //hex constant for style changing
    private const int WS_BORDER = 0x00800000;       //window with border
    private const int WS_CAPTION = 0x00C00000;      //window with a title bar
    private const int WS_SYSMENU = 0x00080000;      //window with no borders etc.
    private const int WS_MINIMIZEBOX = 0x00020000;  //window with minimizebox

    public static void MakeBorderless(string title)
    {
        // Get the handle of self
        IntPtr window = FindWindowByCaption(IntPtr.Zero, title);
        RECT rect;
        // Get the rectangle of self (Size)
        GetWindowRect(window, out rect);
        // Get the handle of the desktop
        IntPtr HWND_DESKTOP = GetDesktopWindow();
        // Attempt to get the location of self compared to desktop
        MapWindowPoints(HWND_DESKTOP, window, ref rect, 2);
        // update self
        SetWindowLong(window, GWL_STYLE, WS_SYSMENU);
        // rect.left rect.top should work but they're returning negative values for me. I probably messed up
        SetWindowPos(window, -2, 100, 75, rect.bottom, rect.right, 0x0040);
        DrawMenuBar(window);
    }

    // CLİCK HANDLER
    // CLİCK HANDLER
    // CLİCK HANDLER
    // CLİCK HANDLER
    // CLİCK HANDLER

    public enum MouseButton
    {
        LeftMouseButton = 0x01,
        RightMouseButton = 0x02,
        LeftAndRightMouseButton = 0x03,
        MiddleMouseButton = 0x04,
    }

    public class OnClickEventArgs
    {
        public OnClickEventArgs(Position position,MouseButton Button) { ClickedPosition = position;ClickedButton = Button; }
        public Position ClickedPosition { get; }
        public MouseButton ClickedButton { get; }
    }


    public delegate void OnClickEventHandler(object sender, OnClickEventArgs e);
    public static event OnClickEventHandler OnClickEvent;


    public class OnKeyEventArgs
    {
        public OnKeyEventArgs(char Key,int KeyCode) { this.Key = Key; this.KeyCode = KeyCode; }
        public char Key { get; }
        public int KeyCode { get; }

    }

  
    public delegate void OnKeyEvent(object sender, OnKeyEventArgs e);
    public static event OnKeyEvent OnKey;


    public static Position MousePosition = new Position(0, 0);


    private static void ConsoleWindow_OnClickEvent(object sender, OnClickEventArgs e)
    {
        //Console.Title = e.ClickedPosition.x + $" [{e.ClickedButton.ToString()}] " + e.ClickedPosition.y;

        Component ClickedComponent = Root.CurrentRoot.CheckPosition(Root.CurrentRoot.CurrentPage.components,e.ClickedPosition);

     

        if (ClickedComponent != null && ClickedComponent.Visible && ClickedComponent.Enabled)
        {
            if(LastFocused != null)
            {
                if (!LastFocused.Equals(ClickedComponent))
                {
                    LastFocused.Unfocus();
                    
                }

            }

            LastFocused = ClickedComponent;
            ClickedComponent.Click(e);
        }
        else
        {
            LastFocused?.Unfocus();
            LastFocused = null;
        }


    }

    private static void ConsoleWindow_OnKey(object sender, OnKeyEventArgs e)
    {
        // 8 == Backspace
        // 32 == Space
        // 13 == Enter
        // 37 = <<
        // 39 = >>

        if(LastFocused != null)
        {
            if(LastFocused.ComponentType == typeof(Input) && LastFocused.Visible && LastFocused.Enabled)
            {

                var input = (Input)LastFocused;
                if (LastFocused.Text.Trim() != "")
                {
                    if (e.KeyCode == 8)
                    {

                        LastFocused.SetText(LastFocused.Text.Remove(LastFocused.Text.Length - 1));

                        if (LastFocused.Text == "")
                        {
                            LastFocused.SetText(new string(' ', input.Width));
                        }
                    }
                    else if (e.KeyCode == 20 || e.KeyCode == 16) { return; }
                    else
                    {

                        if (input.Text.Length >= input.Width - 1)
                        {

                            string test = (string)LastFocused.Text.Clone();
                            LastFocused.SetText(test.Substring(input.StartIndex, input.Width - input.StartIndex - 1));
                        }


                        LastFocused.SetText(LastFocused.Text + e.Key.ToString());


                    }
                    
                }
                else
                {
                    if (e.KeyCode != 8 && e.KeyCode != 20 && e.KeyCode != 16 && e.KeyCode != 37 && e.KeyCode != 39)
                    {
                        LastFocused.SetText(e.Key.ToString());

                    }
                    
                }

            }
        }

    }

    public static bool isPressing = false;
    public static bool isClicking = false;
    public static bool isHovering = false;
    private static Component LastHovered = null;
    public static Component LastFocused = null;


    public async static void ListenConsole()
    {
        OnClickEvent += ConsoleWindow_OnClickEvent;
        OnKey += ConsoleWindow_OnKey;

        var handle = NativeMethods.GetStdHandle(NativeMethods.STD_INPUT_HANDLE);

        int mode = 0;
        if (!(NativeMethods.GetConsoleMode(handle, ref mode))) { throw new Win32Exception(); }

        mode |= NativeMethods.ENABLE_MOUSE_INPUT;
        mode &= ~NativeMethods.ENABLE_QUICK_EDIT_MODE;
        mode |= NativeMethods.ENABLE_EXTENDED_FLAGS;

        if (!(NativeMethods.SetConsoleMode(handle, mode))) { throw new Win32Exception(); }

        var record = new NativeMethods.INPUT_RECORD();
        uint recordLen = 0;

        

        while (true)
        {
            if (!(NativeMethods.ReadConsoleInput(handle, ref record, 1, ref recordLen))) { throw new Win32Exception(); }


            switch (record.EventType)
            {
                case NativeMethods.MOUSE_EVENT:
                    {
                        try
                        {
                            if(Root.CurrentRoot.CurrentPage == null) { continue; }
                            MousePosition.x = record.MouseEvent.dwMousePosition.X;
                            MousePosition.y = record.MouseEvent.dwMousePosition.Y;
                            Component HoverComp = Root.CurrentRoot.CheckPosition(Root.CurrentRoot.CurrentPage.components, MousePosition);
                            if (HoverComp != null && HoverComp.Visible && HoverComp.Enabled)
                            {
                                if (HoverComp.ComponentType == typeof(ListLabel))
                                {
                                    if (LastHovered != null) { LastHovered.Color = ConsoleColor.White; }
                                    LastHovered = HoverComp;
                                    HoverComp.Color = ConsoleColor.DarkGreen;
                                    isHovering = true;
                                }
                                else if (HoverComp.ComponentType == typeof(Button))
                                {
                                    if (LastHovered != null) { LastHovered.Color = ConsoleColor.White; }
                                    LastHovered = HoverComp;
                                    HoverComp.Color = ConsoleColor.Black;

                                    isHovering = true;
                                }
                                /*
                                else if(HoverComp.ComponentType == typeof(Box))
                                {
                                    var box = (Box)HoverComp;   
                                    if(record.MouseEvent.dwButtonState == 0x01)
                                    {

                                        var DragPosition = new Position(MousePosition.x, MousePosition.y);
                                        HoverComp.position = DragPosition;

                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                                */

                            }
                            else
                            {
                                isHovering = false;
                                if (LastHovered != null && !isHovering)
                                {
                                    LastHovered.Color = ConsoleColor.White;
                                    LastHovered = null;
                                }
                            }

                            if (record.MouseEvent.dwButtonState == 0x01)
                            {

                                OnClickEvent?.Invoke(null, new OnClickEventArgs(MousePosition, MouseButton.LeftMouseButton));
                                await Task.Delay(100);

                            }

                            //Console.Title = string.Format("    dwButtonState ...: 0x{0:X4}  ", record.MouseEvent.dwButtonState);
                            //Console.Title = string.Format("    dwControlKeyState: 0x{0:X4}  ", record.MouseEvent.dwControlKeyState);
                            //Console.Title = string.Format("    dwEventFlags ....: 0x{0:X4}  ", record.MouseEvent.dwEventFlags);
                        }
                        catch
                        {
                            continue;
                        }
                      
                    }
                    break;

                case NativeMethods.KEY_EVENT:
                    {
                        isPressing = record.KeyEvent.bKeyDown;
                        if(record.KeyEvent.bKeyDown)
                        {
                            OnKey?.Invoke(null,new OnKeyEventArgs(record.KeyEvent.UnicodeChar,record.KeyEvent.wVirtualKeyCode));
                        }
                        //Console.WriteLine(string.Format("    wRepeatCount ....:   {0,4:0}  ", record.KeyEvent.wRepeatCount));
                        Console.Title = string.Format("    wVirtualKeyCode .:   {0,4:0}  ", record.KeyEvent.wVirtualKeyCode);
                        //Console.Title = string.Format("    uChar ...........:      {0}  ", record.KeyEvent.UnicodeChar);
                        //Console.WriteLine(string.Format("    dwControlKeyState: 0x{0:X4}  ", record.KeyEvent.dwControlKeyState));

                        if (record.KeyEvent.wVirtualKeyCode == (int)ConsoleKey.Escape) { return; }
                    }
                    break;
            }

            await Task.Delay(1);
        }
    }


    private class NativeMethods
    {

        public const Int32 STD_INPUT_HANDLE = -10;

        public const Int32 ENABLE_MOUSE_INPUT = 0x0010;
        public const Int32 ENABLE_QUICK_EDIT_MODE = 0x0040;
        public const Int32 ENABLE_EXTENDED_FLAGS = 0x0080;

        public const Int32 KEY_EVENT = 1;
        public const Int32 MOUSE_EVENT = 2;


        [DebuggerDisplay("EventType: {EventType}")]
        [StructLayout(LayoutKind.Explicit)]
        public struct INPUT_RECORD
        {
            [FieldOffset(0)]
            public Int16 EventType;
            [FieldOffset(4)]
            public KEY_EVENT_RECORD KeyEvent;
            [FieldOffset(4)]
            public MOUSE_EVENT_RECORD MouseEvent;
        }

        [DebuggerDisplay("{dwMousePosition.X}, {dwMousePosition.Y}")]
        public struct MOUSE_EVENT_RECORD
        {
            public COORD dwMousePosition;
            public Int32 dwButtonState;
            public Int32 dwControlKeyState;
            public Int32 dwEventFlags;
        }

        [DebuggerDisplay("{X}, {Y}")]
        public struct COORD
        {
            public UInt16 X;
            public UInt16 Y;
        }

        [DebuggerDisplay("KeyCode: {wVirtualKeyCode}")]
        [StructLayout(LayoutKind.Explicit)]
        public struct KEY_EVENT_RECORD
        {
            [FieldOffset(0)]
            [MarshalAsAttribute(UnmanagedType.Bool)]
            public Boolean bKeyDown;
            [FieldOffset(4)]
            public UInt16 wRepeatCount;
            [FieldOffset(6)]
            public UInt16 wVirtualKeyCode;
            [FieldOffset(8)]
            public UInt16 wVirtualScanCode;
            [FieldOffset(10)]
            public Char UnicodeChar;
            [FieldOffset(10)]
            public Byte AsciiChar;
            [FieldOffset(12)]
            public Int32 dwControlKeyState;
        };


        public class ConsoleHandle : SafeHandleMinusOneIsInvalid
        {
            public ConsoleHandle() : base(false) { }

            protected override bool ReleaseHandle()
            {
                return true; //releasing console handle is not our business
            }
        }


        [DllImportAttribute("kernel32.dll", SetLastError = true)]
        [return: MarshalAsAttribute(UnmanagedType.Bool)]
        public static extern Boolean GetConsoleMode(ConsoleHandle hConsoleHandle, ref Int32 lpMode);

        [DllImportAttribute("kernel32.dll", SetLastError = true)]
        public static extern ConsoleHandle GetStdHandle(Int32 nStdHandle);

        [DllImportAttribute("kernel32.dll", SetLastError = true)]
        [return: MarshalAsAttribute(UnmanagedType.Bool)]
        public static extern Boolean ReadConsoleInput(ConsoleHandle hConsoleInput, ref INPUT_RECORD lpBuffer, UInt32 nLength, ref UInt32 lpNumberOfEventsRead);

        [DllImportAttribute("kernel32.dll", SetLastError = true)]
        [return: MarshalAsAttribute(UnmanagedType.Bool)]
        public static extern Boolean SetConsoleMode(ConsoleHandle hConsoleHandle, Int32 dwMode);

    }




}