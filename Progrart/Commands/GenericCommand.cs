using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Progrart.Commands
{
	public class GenericCommand : ICommand
	{
		public event EventHandler? CanExecuteChanged;
		public Func<object?, bool>? Checker = null;
		public Action<object?>? onExecute = null;
		public bool CanExecute(object? parameter)
		{
			return Checker?.Invoke(parameter) ?? true;
		}

		public void Execute(object? parameter)
		{
			onExecute?.Invoke(parameter);
		}
	}
}
