using System;
using System.IO;

namespace FileRenamer
{
    internal class VideoHelper : IDisposable
    {
        private FileInfo info { get; }
        private Int32 sumMilliseconds { get; }

        public VideoHelper(string filename, Int32 sumMilliseconds)
        {
            this.sumMilliseconds = sumMilliseconds;
            info = new FileInfo(filename);
        }

        public void Dispose()
        {
        }

        public DateTime? DateTaken =>
	        getDate()?.AddMilliseconds(sumMilliseconds);

        private DateTime? getDate()
        {
            var date = DateByName.GetFromVideo(info);

			if (date != null)
	            return date;

            date = info.CreationTime;

            if (date > info.LastAccessTime)
	            date = info.LastAccessTime;

            if (date > info.LastWriteTime)
	            date = info.LastWriteTime;

            return date;
        }

        public String Extension => info.Extension;

        public Boolean IsVideo =>
	        info.Extension == ".mp4"
	        || info.Extension == ".3gp";
    }
}