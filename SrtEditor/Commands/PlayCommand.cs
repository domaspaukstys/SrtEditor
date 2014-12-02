using SrtEditor.Controls;

namespace SrtEditor.Commands
{
    public class PlayCommand : ModelCommand<VideoControl>
    {
        public PlayCommand(VideoControl control) : base(control)
        {
        }

        public override bool CanExecute(object parameter)
        {
            return Model.Player != null && Model.Player.Source != null;
        }

        public override void Execute(object parameter)
        {
            Model.Player.Play();
            Model.IsPaused = false;
            Model.Timer.Start();
        }
    }
}