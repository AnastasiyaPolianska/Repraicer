using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;

namespace Repraicer.Commands
{
    /// <summary>
    /// Simple command class
    /// </summary>
    /// <typeparam name="T1">The type of the 1.</typeparam>
    /// <typeparam name="T2">The type of the 2.</typeparam>
    /// <seealso cref="System.Windows.Input.ICommand" />
    public class SimpleCommand<T1, T2> : ICommand
    {
        private readonly Func<T1, bool> _canExecuteMethod;

        private readonly Action<T2> _executeMethod;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleCommand{T1, T2}"/> class.
        /// </summary>
        /// <param name="canExecuteMethod">The can execute method.</param>
        /// <param name="executeMethod">The execute method.</param>
        public SimpleCommand(Func<T1, bool> canExecuteMethod, Action<T2> executeMethod)
        {
            _executeMethod = executeMethod;
            _canExecuteMethod = canExecuteMethod;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleCommand{T1, T2}"/> class.
        /// </summary>
        /// <param name="executeMethod">The execute method.</param>
        public SimpleCommand(Action<T2> executeMethod)
        {
            _executeMethod = executeMethod;
            _canExecuteMethod = x => true;
        }

        /// <summary>
        /// Determines whether this instance can execute the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>
        /// <c>true</c> if this instance can execute the specified parameter; otherwise, <c>false</c>.
        /// </returns>
        public bool CanExecute(T1 parameter) => _canExecuteMethod == null || _canExecuteMethod(parameter);

        /// <summary>
        /// Executes the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        public void Execute(T2 parameter) => _executeMethod?.Invoke(parameter);

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        public bool CanExecute(object parameter) => CanExecute((T1)parameter);

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public void Execute(object parameter) => Execute((T2)parameter);

        /// <summary>
        /// Occurs when changes occur that affect whether the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecuteMethod != null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }

            remove
            {
                if (_canExecuteMethod != null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="CanExecuteChanged" /> event.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
            Justification = "The this keyword is used in the Silverlight version")]
        [SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate",
            Justification = "This cannot be an event")]
        public void RaiseCanExecuteChanged() => CommandManager.InvalidateRequerySuggested();
    }
}