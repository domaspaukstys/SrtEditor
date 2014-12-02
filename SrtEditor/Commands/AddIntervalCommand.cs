using System.Collections.Specialized;
using SrtEditor.Controls;
using SrtEditor.Data;

namespace SrtEditor.Commands
{
    public class AddIntervalCommand : ModelCommand<TimelineControl>
    {
        public AddIntervalCommand(TimelineControl model)
            : base(model)
        {
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            SrtInterval interval = new SrtInterval((long)Model.CurrentTime, (long) (Model.CurrentTime + 10000));
            Model.SrtCollection.Add(interval);
            Model.SrtCollection.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, interval));
        }
    }
}