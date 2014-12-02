using System;
using System.Windows.Input;
using SrtEditor.Models;

namespace SrtEditor.Commands
{
    public abstract class ModelCommand<TModel> : ICommand
        where TModel : IModel
    {
        protected ModelCommand(TModel model)
        {
            Model = model;
        }

        public TModel Model { get; private set; }

        public abstract bool CanExecute(object parameter);
        public abstract void Execute(object parameter);
        public event EventHandler CanExecuteChanged;

        public virtual void OnCanExecuteChanged()
        {
            EventHandler handler = CanExecuteChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }
    }
}