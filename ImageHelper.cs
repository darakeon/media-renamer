using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace FileRenamer
{
    class ImageHelper : IDisposable
    {
        public ImageHelper(String path)
        {
            info = new FileInfo(path);
            read = info.OpenRead();

            try
            {
                file = Image.FromStream(read, false, false);
                IsImage = true;
            }
            catch (ArgumentException)
            {
                IsImage = false;
            }
        }



        private static readonly Regex regex = new Regex(":");

        public Boolean IsImage { get; private set; }

        private FileInfo info { get; set; }
        private FileStream read { get; set; }
        private Image file { get; set; }



        public String Extension
        {
            get { return info.Extension; }
        }

        public DateTime DateTaken
        {
            get
            {
                try
                {
                    var propertyItem = file.GetPropertyItem(36867);
                    var decoded = Encoding.UTF8.GetString(propertyItem.Value);
                    var dateTaken = regex.Replace(decoded, "-", 2);

                    return DateTime.Parse(dateTaken);
                }
                catch (ArgumentException)
                {
                    var date = info.CreationTime;

                    if (date > info.LastAccessTime)
                        date = info.LastAccessTime;

                    if (date > info.LastWriteTime)
                        date = info.LastWriteTime;

                    return date;
                }
            }
        }
        

        
        public void Dispose()
        {
            if (read != null)
                read.Dispose();

            if (file != null)
                file.Dispose();
        }


    }
}
