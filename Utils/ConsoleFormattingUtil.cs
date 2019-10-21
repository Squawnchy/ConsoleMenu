using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMenu.Utils
{
	static class ConsoleFormattingUtil
	{
		public static void WriteOnBottomLine(string text) => WriteOnBottomLine(text, 0);
		public static void WriteOnBottomLine(string text, int bottomMargin)
		{
			int x = Console.CursorLeft;
			int y = Console.CursorTop;
			Console.CursorTop = Console.WindowTop + Console.WindowHeight - (1 + bottomMargin);
			Console.Write(text);
			// Restore previous position
			Console.SetCursorPosition(x, y);
		}

		public static void PushColor(ConsoleColor color) => Console.ForegroundColor = color;
		public static void PopColor() => Console.ForegroundColor = ConsoleColor.White;
	}
}
