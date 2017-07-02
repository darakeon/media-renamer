using System;
using System.Configuration;
using System.IO;

namespace FileRenamer
{
    class Program
    {
        public static void Main(string[] args)
        {
            var path = ConfigurationManager.AppSettings["Path"];
            var format = ConfigurationManager.AppSettings["Format"];

            foreach (var file in Directory.GetFiles(path))
            {
                String newName = null;

                for (var s = 0; newName == null; s++)
                {
                    newName = getNewName(file, format, s);

                    if (newName != "")
                    {
                        var newCompleteName = Path.Combine(path, newName);

                        if (File.Exists(newCompleteName))
                            newName = null;
                        else
                            File.Move(file, newCompleteName);
                    }
                }

            }
            
        }

        private static String getNewName(String file, String format, Int32 sumMilisseconds)
        {
            using (var image = new ImageHelper(file, sumMilisseconds))
            {
                if (image.IsImage)
                {
                    return image.DateTaken.ToString(format)
                        + image.Extension;
                }
            }

            using (var video = new VideoHelper(file, sumMilisseconds))
            {
                if (video.IsVideo)
                {
                    return video.DateTaken.ToString(format)
                        + video.Extension;
                }
            }

            return "";
        }
    }
}
