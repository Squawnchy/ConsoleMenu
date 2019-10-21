using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMenu.Utils
{
	static class ConsoleFormattingUtil
	{
		public static void WriteOnBottomLine(string text)
		{
			int x = Console.CursorLeft;
			int y = Console.CursorTop;
			Console.CursorTop = Console.WindowTop + Console.WindowHeight - 2;
			Console.Write(text);
			// Restore previous position
			Console.SetCursorPosition(x, y);
		}
	}
}
