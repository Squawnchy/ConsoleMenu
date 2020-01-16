using ConsoleMenu.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;

namespace ConsoleMenu
{
	public class CMenu
	{
		#region constants
		private const string CMENU_FOOTER_TEXT = "[UP-ARROW / DOWN-ARROW] Navigate, [ENTER] Select, [ESC] Exit";
		private const string CMENU_DEFAULT_TITLE = "MENU";
		private const int CMENU_SELECTION_INDICATOR_X_POS = 0;
		private const int CMENU_SELECTION_INDICATOR_Y_START_POS = 4;
		private const ConsoleColor CMENU_DEFAULT_TITLE_COLOR = ConsoleColor.White;
		private const ConsoleColor CMENU_DEFAULT_FOOTER_COLOR = ConsoleColor.DarkGreen;
		private const ConsoleColor CMENU_DEFAULT_HIGHLIGHT_COLOR = ConsoleColor.White;
		private const ConsoleColor CMENU_DEFAULT_TEXT_COLOR = ConsoleColor.Black;
		#endregion


		#region privates
		private int _selection = 0;
		private int[] _itemsYPositions;
		private bool _isMenuDrawn;
		private CMenuDrawer _cMenuDrawer;
		private ObservableCollection<IConsoleProgram> _items;
		#endregion


		#region properties
		public ReadOnlyObservableCollection<IConsoleProgram> Items { get; private set; }

		public string Title { get; set; } = CMENU_DEFAULT_TITLE;
		public string Footer { get; set; } = CMENU_FOOTER_TEXT;

		public ConsoleColor TitleColor { get; set; } = CMENU_DEFAULT_TITLE_COLOR;
		public ConsoleColor FooterColor { get; set; } = CMENU_DEFAULT_FOOTER_COLOR;
		public ConsoleColor HiglightColor { get; set; } = CMENU_DEFAULT_HIGHLIGHT_COLOR;
		public ConsoleColor TextColor { get; set; } = CMENU_DEFAULT_TEXT_COLOR;
		#endregion


		#region ctor
		public CMenu()
		{
			_items = new ObservableCollection<IConsoleProgram>();
			Items = new ReadOnlyObservableCollection<IConsoleProgram>(_items);
		}
		#endregion


		#region public methods
		/// <summary>
		/// generic add a Type whic derives from <see cref="IConsoleProgram"/> to the menu-item collection
		/// </summary>
		/// <typeparam name="T"></typeparam>
		public void AddItem<T>() where T : IConsoleProgram , new() =>_items.Add(new T());

		/// <summary>
		/// display the menu
		/// </summary>
		public void Show()
		{
			_cMenuDrawer = new CMenuDrawer();

			Console.CursorVisible = false;
			do
			{
				if (!_isMenuDrawn)
					PrintMenu();
				else
					RefreshAllMenuItems(_selection);
			} while (AcceptNavigate());
		}

		/// <summary>
		/// redraw the menu
		/// </summary>
		public void Redraw() => PrintMenu();
		#endregion


		#region private methods
		private void PrintMenu()
		{
			_cMenuDrawer.ClearScreen();
			_cMenuDrawer.PrintFooter(Footer, FooterColor);
			_cMenuDrawer.FillLineWithChar('-');
			_cMenuDrawer.PrintTitle(Title, TitleColor);
			_cMenuDrawer.FillLineWithChar('-');
			_cMenuDrawer.NewLine();
			PrintAllMenuItems();
			_isMenuDrawn = true;
		}

		private void PrintAllMenuItems()
		{
			_itemsYPositions = new int[_items.Count];
			for (int i = 0; i < _items.Count; i++)
				PrintMenuItem(_selection == i, i);
		}

		private void RefreshAllMenuItems(int selection)
		{
			for (int i = 0; i < _itemsYPositions.Length; i++)
			{
				Console.SetCursorPosition(CMENU_SELECTION_INDICATOR_X_POS , i + CMENU_SELECTION_INDICATOR_Y_START_POS - 1);
				PrintMenuItem(selection == i, i);
			}
		}

		private void PrintMenuItem(bool marked, int itemIndex)
		{
			if (marked)
				_cMenuDrawer.PrintHighlightedElement(_items[itemIndex], HiglightColor, TextColor);
			else
				Console.WriteLine($"	{_items[itemIndex].Name}");
		}

		private void StartSelectedItem(int index)
		{
			Console.Clear();
			_isMenuDrawn = false;
			try
			{
				_items[index].Start();
			}
			catch (Exception e)
			{
				Console.Clear();
				Console.WriteLine($"\"{_items[index]?.Name}\" crashed");
				ConsoleFormattingUtil.PushForeground(ConsoleColor.Red);
				ConsoleFormattingUtil.PopForeground();
				Console.WriteLine(JsonConvert.SerializeObject(e, Formatting.Indented));
			}

			Console.ReadKey();
		}

		// false = exit
		private bool AcceptNavigate()
		{
			switch (Console.ReadKey().Key)
			{
				case ConsoleKey.Enter:
					StartSelectedItem(_selection);
					return true;
				case ConsoleKey.UpArrow:
					Navigate(true);
					return true;
				case ConsoleKey.DownArrow:
					Navigate(false);
					return true;
				case ConsoleKey.Escape:
					return false;
				default:
					return false;
			}
		}

		private void CleanSelection()
		{
			Console.SetCursorPosition(CMENU_SELECTION_INDICATOR_X_POS, _selection + CMENU_SELECTION_INDICATOR_Y_START_POS - 1);
			Console.WriteLine($"\t{(_items[_selection] as IConsoleProgram).Name}");
		}

		private void Navigate(bool up)
		{
			CleanSelection();
			if(up)
				_selection = _selection == 0 ? _items.Count - 1 : _selection - 1;
			else
			_selection = _selection == _items.Count - 1 ? 0 : _selection + 1;
		}
	}
	#endregion
}