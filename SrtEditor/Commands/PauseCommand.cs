using SrtEditor.Controls;

namespace SrtEditor.Commands
{
    public class PauseCommand : ModelCommand<VideoControl>
    {
        public PauseCommand(VideoControl control) : base(control)
        {
        }

        public override bool CanExecute(object parameter)
        {
            return Model.Player != null && Model.Player.Source != null;
        }

        public override void Execute(object parameter)
        {
            Model.Player.Pause();
            Model.IsPaused = true;
            Model.Timer.Stop();
        }
    }
}