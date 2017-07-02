using System;
using System.IO;

namespace FileRenamer
{
    internal class VideoHelper : IDisposable
    {
        private FileInfo info { get; set; }

        public VideoHelper(String filename)
        {
            info = new FileInfo(filename);
        }

        public void Dispose()
        {
        }

        public DateTime DateTaken
        {
            get
            {
                var date = info.CreationTime;

                if (date > info.LastAccessTime)
                    date = info.LastAccessTime;

                if (date > info.LastWriteTime)
                    date = info.LastWriteTime;

                return date;
            }
        }

        public String Extension
        {
            get { return info.Extension; }
        }

        public Boolean IsVideo
        {
            get { return info.Extension == ".mp4"; }
        }
    }
}