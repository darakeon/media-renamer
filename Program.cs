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
                var info = new FileInfo(file);

                var newName = info.LastWriteTime.ToString(format);

                var newCompleteName = Path.Combine(path, newName + info.Extension);

                File.Move(file, newCompleteName);
             }
            
        }
    }
}
