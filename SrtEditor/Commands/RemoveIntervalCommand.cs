using System.Collections.Specialized;
using SrtEditor.Controls;

namespace SrtEditor.Commands
{
    public class RemoveIntervalCommand : ModelCommand<TimelineControl>
    {
        public RemoveIntervalCommand(TimelineControl model)
            : base(model)
        {
        }

        public override bool CanExecute(object parameter)
        {
            return Model.SelectedInterval != null;
        }

        public override void Execute(object parameter)
        {
            Model.SrtCollection.Remove(Model.SelectedInterval.Interval);
            Model.SrtCollection.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, Model.SelectedInterval.Interval));
            Model.SelectedInterval = null;
        }
    }
}