using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ConsoleMenu
{
	public class CMenu
	{
		private int _selection = 0;
		private ObservableCollection<IConsoleProgram> _items = new ObservableCollection<IConsoleProgram>();


		public ReadOnlyObservableCollection<IConsoleProgram> Items { get; private set; }
		public string MarkedIndicator { get; set; } = "[X]";
		public string UnmarkedIndicator { get; set; } = "[ ]";
		public string Title { get; private set; } = "MENU";


		public CMenu()
		{
			Items = new ReadOnlyObservableCollection<IConsoleProgram>(_items);
		}

		public void AddItem(IConsoleProgram program)
		{
			_items.Add(program);
		}

		public void Show()
		{
			Console.CursorVisible = false;
			do
			{
				PrintMenu();
			} while (AcceptNavigate());
		}

		public void SetTitle(string title)
		{
			Title = title;
		}


		private void PrintMenu()
		{
			Console.Clear();
			PrintFooter();
			for (int i = 0; i < Console.WindowWidth; i++)
				Console.Write("-");
			for (int i = 0; i < Console.WindowWidth / 2 - Title.Length / 2; i++)
				Console.Write(" ");
			Console.WriteLine(Title);
			for (int i = 0; i < Console.WindowWidth; i++)
				Console.Write("-");
			Console.WriteLine();
			for (int i = 0; i < _items.Count; i++)
				PrintMenuItem(_selection == i, i);
		}

		private void PrintMenuItem(bool marked, int itemIndex)
		{
			var indicator = marked ? MarkedIndicator : UnmarkedIndicator;
			Console.WriteLine($"{indicator}\t{_items[itemIndex].Name}");
		}

		private void PrintFooter()
		{
			string footerString = "[UP-ARROW / DOWN-ARROW] Navigate, [ENTER] Select, [ESC] Exit";
			Console.ForegroundColor = ConsoleColor.Green;
			int spacesCount = Console.WindowWidth / 2 - footerString.Length / 2;
			for (int i = 0; i < spacesCount; i++)
				footerString = footerString.Insert(0, " ");
			WriteOnBottomLine(footerString + "\n\n");
			Console.ForegroundColor = ConsoleColor.White;
		}

		private void SelectItem(int index)
		{
			Console.Clear();
			_items[index].Start();
			Console.ReadKey();
		}

		private void NavigateUp()
		{
			_selection = _selection == 0 ? _items.Count - 1 : _selection - 1;
		}

		private void NavigateDown()
		{
			_selection = _selection == _items.Count - 1 ? 0 : _selection + 1;
		}

		// false = exit
		private bool AcceptNavigate()
		{
			switch (Console.ReadKey().Key)
			{
				case ConsoleKey.Enter:
					SelectItem(_selection);
					break;
				case ConsoleKey.UpArrow:
					NavigateUp();
					break;
				case ConsoleKey.DownArrow:
					NavigateDown();
					break;
				case ConsoleKey.Escape:
					return false;
			}

			return true;
		}

		private void WriteOnBottomLine(string text)
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
