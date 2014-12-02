using SrtEditor.Controls;

namespace SrtEditor.Commands
{
    public class StopCommand : ModelCommand<VideoControl>
    {
        public StopCommand(VideoControl control) : base(control)
        {
        }

        public override bool CanExecute(object parameter)
        {
            return Model.Player != null && Model.Player.Source != null;
        }

        public override void Execute(object parameter)
        {
            Model.Player.Stop();
            Model.IsPaused = true;
            Model.CurrentTime = 0;
            Model.Timer.Stop();
        }
    }
}