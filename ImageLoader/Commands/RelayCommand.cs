using System.Windows.Input;

namespace ImageLoader.Commands
{
    public class RelayCommand(Action<object?> command) : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private readonly Action<object?> _command = command;

        public bool CanExecute(object? parameter)
        {
            return true;
        }
        public void Execute(object? parameter)
        {
            _command(parameter);
        }
    }
}
