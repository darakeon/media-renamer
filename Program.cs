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
                var newName = getNewName(file, format);

                if (newName != null)
                {
                    var newCompleteName = Path.Combine(path, newName);

                    File.Move(file, newCompleteName);
                }

            }
            
        }

        private static String getNewName(String file, String format)
        {
            using(var image = new ImageHelper(file))
            {
                if (image.IsImage)
                {
                    return image.DateTaken.ToString(format)
                        + image.Extension;
                }
            }

            return null;
        }
    }
}
