using System.IO;
using Microsoft.Win32;
using SrtEditor.Data;
using SrtEditor.Models;

namespace SrtEditor.Commands
{
    public class SaveSrtCommand : ModelCommand<MainModel>
    {
        public SaveSrtCommand(MainModel model) : base(model)
        {
        }

        public override bool CanExecute(object parameter)
        {
            return Model.SrtDocument.Count > 0;
        }

        public override void Execute(object parameter)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            const string formats = "SubRip Files | *.srt";
            dialog.Filter = formats;
            if (dialog.ShowDialog() == true && !string.IsNullOrEmpty(dialog.FileName))
            {
                using (FileStream stream = File.Open(dialog.FileName, FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        int i = 1;
                        foreach (SrtInterval element in Model.SrtDocument)
                        {
                            writer.WriteLine(i);
                            writer.Write(element.ToString());
                            writer.WriteLine(System.Environment.NewLine);
                            i++;
                        }
                        writer.Close();
                    }
                    stream.Close();
                }
            }
        }
    }
}
