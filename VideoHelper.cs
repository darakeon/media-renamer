using System;
using System.IO;

namespace FileRenamer
{
    internal class VideoHelper : IDisposable
    {
        private FileInfo info { get; set; }
        private Int32 sumMilisseconds { get; set; }

        public VideoHelper(string filename, Int32 sumMilisseconds)
        {
            this.sumMilisseconds = sumMilisseconds;
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

                return date.AddMilliseconds(sumMilisseconds);
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