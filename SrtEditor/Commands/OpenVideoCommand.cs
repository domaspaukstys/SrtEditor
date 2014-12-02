using System.IO;
using Microsoft.Win32;
using SrtEditor.Models;

namespace SrtEditor.Commands
{
    public class OpenVideoCommand : ModelCommand<MainModel>
    {
        public OpenVideoCommand(MainModel model)
            : base(model)
        {
        }


        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            var dialog = new OpenFileDialog {Multiselect = false};
            const string formats =
                "All Videos Files |*.dat; *.wmv; *.3g2; *.3gp; *.3gp2; *.3gpp; *.amv; *.asf;  *.avi; *.bin; *.cue; *.divx; *.dv; *.flv; *.gxf; *.iso; *.m1v; *.m2v; *.m2t; *.m2ts; *.m4v; " +
                " *.mkv; *.mov; *.mp2; *.mp2v; *.mp4; *.mp4v; *.mpa; *.mpe; *.mpeg; *.mpeg1; *.mpeg2; *.mpeg4; *.mpg; *.mpv2; *.mts; *.nsv; *.nuv; *.ogg; *.ogm; *.ogv; *.ogx; *.ps; *.rec; *.rm; *.rmvb; *.tod; *.ts; *.tts; *.vob; *.vro; *.webm";

            dialog.Filter = formats;

            if (dialog.ShowDialog() == true)
            {
                if (File.Exists(dialog.FileName))
                {
                    Model.VideoSource = dialog.FileName;
                    Model.Text = dialog.FileName;
                }
            }
        }
    }
}