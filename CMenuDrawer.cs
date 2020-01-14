using ConsoleMenu.Utils;
using System;
using System.Text;

namespace ConsoleMenu
{
	internal class CMenuDrawer
	{
		#region privates
		private StringBuilder _stringBuilder = new StringBuilder();
		#endregion



		#region methods
		public void PrintTitle(string title, ConsoleColor color)
		{
			_stringBuilder.Clear();
			for (int i = 0; i < Console.WindowWidth / 2 - title.Length / 2; i++)
				_stringBuilder.Append(" ");
			ConsoleFormattingUtil.PushForeground(color);
			Console.WriteLine(_stringBuilder + title);
			ConsoleFormattingUtil.PopForeground();
		}

		public void FillLineWithChar(char c)
		{
			_stringBuilder.Clear();
			for (int i = 0; i < Console.WindowWidth; i++)
				_stringBuilder.Append(c);
			Console.Write(_stringBuilder);
		}

		public void PrintFooter(string footerString, ConsoleColor color)
		{
			ConsoleFormattingUtil.PushForeground(color);
			int spacesCount = Console.WindowWidth / 2 - footerString.Length / 2;
			for (int i = 0; i < spacesCount; i++)
				footerString = footerString.Insert(0, " ");
			ConsoleFormattingUtil.WriteOnBottomLine(footerString, 1);
			ConsoleFormattingUtil.PopForeground();
		}

		public void PrintHighlightedElement(IConsoleProgram program, ConsoleColor highlightColor, ConsoleColor textColor)
		{
			ConsoleFormattingUtil.PushBackground(highlightColor);
			ConsoleFormattingUtil.PushForeground(textColor);
			Console.WriteLine($"	{program?.Name}");
			ConsoleFormattingUtil.PopBackgorund();
			ConsoleFormattingUtil.PopForeground();
		}

		public void NewLine() => Console.WriteLine();

		public void ClearScreen() => Console.Clear();
		#endregion
	}
}
