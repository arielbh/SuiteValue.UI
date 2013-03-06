using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace SuiteValue.UI.MVVM
{
    public partial class CompositeCommand : ICommand
    {
        private readonly List<ICommand> registeredCommands = new List<ICommand>();
        private readonly bool monitorCommandActivity;
        private readonly EventHandler onRegisteredCommandCanExecuteChangedHandler;

#if !SILVERLIGHT
        private List<WeakReference> _canExecuteChangedHandlers;
#endif


        /// <summary>
        /// Initializes a new instance of <see cref="CompositeCommand"/>.
        /// </summary>
        public CompositeCommand()
        {
            this.onRegisteredCommandCanExecuteChangedHandler = new EventHandler(this.OnRegisteredCommandCanExecuteChanged);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="CompositeCommand"/>.
        /// </summary>
        /// <param name="monitorCommandActivity">Indicates when the command activity is going to be monitored.</param>
        public CompositeCommand(bool monitorCommandActivity)
            : this()
        {
            this.monitorCommandActivity = monitorCommandActivity;
        }

        /// <summary>
        /// Adds a command to the collection and signs up for the <see cref="ICommand.CanExecuteChanged"/> event of it.
        /// </summary>
        ///  <remarks>
        /// If this command is set to monitor command activity, and <paramref name="command"/> 
        /// implements the <see cref="IActiveAware"/> interface, this method will subscribe to its
        /// <see cref="IActiveAware.IsActiveChanged"/> event.
        /// </remarks>
        /// <param name="command">The command to register.</param>
        public virtual void RegisterCommand(ICommand command)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (command == this)
            {
                throw new ArgumentException("CannotRegisterCompositeCommandInItself");
            }

            lock (this.registeredCommands)
            {
                if (this.registeredCommands.Contains(command))
                {
                    throw new InvalidOperationException("CannotRegisterSameCommandTwice");
                }
                this.registeredCommands.Add(command);
            }

            command.CanExecuteChanged += this.onRegisteredCommandCanExecuteChangedHandler;
            this.OnCanExecuteChanged();
        }

        /// <summary>
        /// Removes a command from the collection and removes itself from the <see cref="ICommand.CanExecuteChanged"/> event of it.
        /// </summary>
        /// <param name="command">The command to unregister.</param>
        public virtual void UnregisterCommand(ICommand command)
        {
            if (command == null) throw new ArgumentNullException("command");
            bool removed;
            lock (this.registeredCommands)
            {
                removed = this.registeredCommands.Remove(command);
            }

            if (removed)
            {
                command.CanExecuteChanged -= this.onRegisteredCommandCanExecuteChangedHandler;
                this.OnCanExecuteChanged();
            }
        }

        private void OnRegisteredCommandCanExecuteChanged(object sender, EventArgs e)
        {
            this.OnCanExecuteChanged();
        }


        /// <summary>
        /// Forwards <see cref="ICommand.CanExecute"/> to the registered commands and returns
        /// <see langword="true" /> if all of the commands return <see langword="true" />.
        /// </summary>
        /// <param name="parameter">Data used by the command.
        /// If the command does not require data to be passed, this object can be set to <see langword="null" />.
        /// </param>
        /// <returns><see langword="true" /> if all of the commands return <see langword="true" />; otherwise, <see langword="false" />.</returns>
        public virtual bool CanExecute(object parameter)
        {
            bool hasEnabledCommandsThatShouldBeExecuted = false;

            ICommand[] commandList;
            lock (this.registeredCommands)
            {
                commandList = this.registeredCommands.ToArray();
            }
            foreach (ICommand command in commandList)
            {
                if (!command.CanExecute(parameter))
                {
                    return false;
                }

                hasEnabledCommandsThatShouldBeExecuted = true;
            }

            return hasEnabledCommandsThatShouldBeExecuted;
        }

#if SILVERLIGHT
    /// <summary>
    /// Occurs when changes occur that affect whether or not the command should execute. 
    /// </summary>
        public event EventHandler CanExecuteChanged;
#else
        /// <summary>
        /// Occurs when any of the registered commands raise <see cref="ICommand.CanExecuteChanged"/>. You must keep a hard
        /// reference to the handler to avoid garbage collection and unexpected results. See remarks for more information.
        /// </summary>
        /// <remarks>
        /// When subscribing to the <see cref="ICommand.CanExecuteChanged"/> event using 
        /// code (not when binding using XAML) will need to keep a hard reference to the event handler. This is to prevent 
        /// garbage collection of the event handler because the command implements the Weak Event pattern so it does not have
        /// a hard reference to this handler. An example implementation can be seen in the CompositeCommand and CommandBehaviorBase
        /// classes. In most scenarios, there is no reason to sign up to the CanExecuteChanged event directly, but if you do, you
        /// are responsible for maintaining the reference.
        /// </remarks>
        /// <example>
        /// The following code holds a reference to the event handler. The myEventHandlerReference value should be stored
        /// in an instance member to avoid it from being garbage collected.
        /// <code>
        /// EventHandler myEventHandlerReference = new EventHandler(this.OnCanExecuteChanged);
        /// command.CanExecuteChanged += myEventHandlerReference;
        /// </code>
        /// </example>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                WeakEventHandlerManager.AddWeakReferenceHandler(ref _canExecuteChangedHandlers, value, 2);
            }
            remove
            {
                WeakEventHandlerManager.RemoveWeakReferenceHandler(_canExecuteChangedHandlers, value);
            }
        }
#endif

        /// <summary>
        /// Forwards <see cref="ICommand.Execute"/> to the registered commands.
        /// </summary>
        /// <param name="parameter">Data used by the command.
        /// If the command does not require data to be passed, this object can be set to <see langword="null" />.
        /// </param>
        public virtual void Execute(object parameter)
        {
            Queue<ICommand> commands;
            lock (this.registeredCommands)
            {
                commands = new Queue<ICommand>(this.registeredCommands.Where(this.ShouldExecute).ToList());
            }

            while (commands.Count > 0)
            {
                ICommand command = commands.Dequeue();
                command.Execute(parameter);
            }
        }

        /// <summary>
        /// Gets the list of all the registered commands.
        /// </summary>
        /// <value>A list of registered commands.</value>
        /// <remarks>This returns a copy of the commands subscribed to the CompositeCommand.</remarks>
        public IList<ICommand> RegisteredCommands
        {
            get
            {
                IList<ICommand> commandList;
                lock (this.registeredCommands)
                {
                    commandList = this.registeredCommands.ToList();
                }

                return commandList;
            }
        }

#if SILVERLIGHT
        /// <summary>
        /// Raises <see cref="ICommand.CanExecuteChanged"/> on the UI thread so every 
        /// command invoker can requery <see cref="ICommand.CanExecute"/> to check if the
        /// <see cref="CompositeCommand"/> can execute.
        /// </summary>
        protected virtual void OnCanExecuteChanged()
        {
            var handlers = CanExecuteChanged;
            if (handlers != null)
            {
                handlers(this, EventArgs.Empty);
            }
        }
#else
        /// <summary>
        /// Raises <see cref="ICommand.CanExecuteChanged"/> on the UI thread so every 
        /// command invoker can requery <see cref="ICommand.CanExecute"/> to check if the
        /// <see cref="CompositeCommand"/> can execute.
        /// </summary>
        protected virtual void OnCanExecuteChanged()
        {
            WeakEventHandlerManager.CallWeakReferenceHandlers(this, _canExecuteChangedHandlers);
        }
#endif

        /// <summary>
        /// Handler for IsActiveChanged events of registered commands.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">EventArgs to pass to the event.</param>
        private void Command_IsActiveChanged(object sender, EventArgs e)
        {
            this.OnCanExecuteChanged();
        }
    }
}