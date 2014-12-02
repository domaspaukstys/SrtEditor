using System;
using System.Collections.Specialized;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using SrtEditor.Data;
using SrtEditor.Models;

namespace SrtEditor.Commands
{
    public class OpenSrtCommand : ModelCommand<MainModel>
    {
        public OpenSrtCommand(MainModel model)
            : base(model)
        {
        }


        public override bool CanExecute(object parameter)
        {
            return Model.VideoLoaded;
        }

        public override void Execute(object parameter)
        {
            var dialog = new OpenFileDialog {Multiselect = false};
            const string formats = "SubRip Files | *.srt";

            dialog.Filter = formats;

            if (dialog.ShowDialog() == true)
            {
                if (File.Exists(dialog.FileName))
                {
                    Model.SrtDocument.Clear();
                    ReadFile(dialog.FileName);
                    Model.SrtDocument.OnCollectionChanged(
                        new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                    Model.SaveSrt.OnCanExecuteChanged();
                }
            }
        }

        public void ReadFile(string file)
        {
            var regex = new Regex(@"(\d{0,2}:\d{0,2}:\d{0,2},\d{0,3})(\s*-->\s*)(\d{0,2}:\d{0,2}:\d{0,2},\d{0,3})");
            using (StreamReader reader = File.OpenText(file))
            {
                while (!reader.EndOfStream)
                {
                    string row = reader.ReadLine();
                    SrtInterval element = null;
                    int no;
                    while (!int.TryParse(row, out no) && !string.IsNullOrEmpty(row))
                    {
                        if (regex.IsMatch(row))
                        {
                            element = new SrtInterval(ConvertToTime(regex.Match(row).Groups[1].ToString()),
                                ConvertToTime(regex.Match(row).Groups[3].ToString()));
                            Model.SrtDocument.Add(element);
                        }
                        else if (element != null)
                        {
                            element.AppendText(row);
                        }
                        row = reader.ReadLine();
                    }
                }
                reader.Close();
            }
        }

        public static long ConvertToTime(string time)
        {
            var regex = new Regex(@"(\d{0,2}):(\d{0,2}):(\d{0,2}),(\d{0,3})");
            if (regex.IsMatch(time))
            {
                Match m = regex.Match(time);
                long h = long.Parse(m.Groups[1].ToString());
                long min = long.Parse(m.Groups[2].ToString());
                long sec = long.Parse(m.Groups[3].ToString());
                long ms = long.Parse(m.Groups[4].ToString());
                return h*3600000L + min*60000L + sec*1000L + ms;
            }
            throw new FormatException("String didn't matched format hh:mm:ss,ms");
        }
    }
}