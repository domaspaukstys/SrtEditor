using SrtEditor.Controls;

namespace SrtEditor.Commands
{
    public class ZoomOutCommand : ModelCommand<TimelineControl>
    {
        public ZoomOutCommand(TimelineControl model)
            : base(model)
        {
        }

        public override bool CanExecute(object parameter)
        {
            return Model.ZoomLevel - 0.5 >= 1;
        }

        public override void Execute(object parameter)
        {
            Model.ChangeZoomLevel(-0.5);
        }
    }
}