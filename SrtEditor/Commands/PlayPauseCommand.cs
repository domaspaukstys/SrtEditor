using SrtEditor.Controls;

namespace SrtEditor.Commands
{
    public class PlayPauseCommand : ModelCommand<VideoControl>
    {
        public PlayPauseCommand(VideoControl model) : base(model)
        {
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            if (Model.IsPaused)
            {
                Model.Play.Execute(parameter);
            }
            else
            {
                Model.Pause.Execute(parameter);
            }
        }
    }
}
