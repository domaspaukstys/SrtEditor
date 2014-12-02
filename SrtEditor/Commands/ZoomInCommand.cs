using SrtEditor.Controls;

namespace SrtEditor.Commands
{
    public class ZoomInCommand : ModelCommand<TimelineControl>
    {
        public ZoomInCommand(TimelineControl model) : base(model)
        {
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            Model.ChangeZoomLevel(0.5);
        }
    }
}