using System;

namespace ConsoleMenu
{
	public interface IConsoleProgram
	{
		void Start();
		string Name { get; }
	}
}
