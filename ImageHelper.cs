using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace FileRenamer
{
    class ImageHelper : IDisposable
    {
        private Int32 sumMilliseconds { get; }

        public ImageHelper(String path, Int32 sumMilliseconds)
        {
            this.sumMilliseconds = sumMilliseconds;

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

        public Boolean IsImage { get; }

        private FileInfo info { get; }
        private FileStream read { get; }
        private Image file { get; }

        public String Extension => info.Extension;

        public DateTime? DateTaken =>
	        getDate()?.AddMilliseconds(sumMilliseconds);

        private DateTime? getDate()
        {
	        var date = DateByName.GetFromImage(info);

	        try
			{
		        var propertyItem = file.GetPropertyItem(36867);
		        var decoded = Encoding.UTF8.GetString(propertyItem.Value);
		        var dateTakenText = regex.Replace(decoded, "-", 2);

		        var dateTaken = DateTime.Parse(dateTakenText);

		        return date.HasValue 
		            && date.Value.AddDays(1) < dateTaken 
						? date 
						: dateTaken;
			}
	        catch (ArgumentException)
	        {
		        date = info.CreationTime;

		        if (date > info.LastAccessTime)
			        date = info.LastAccessTime;

		        if (date > info.LastWriteTime)
			        date = info.LastWriteTime;

		        return date;
	        }
        }

        public void Dispose()
        {
	        read?.Dispose();
	        file?.Dispose();
        }
    }
}
