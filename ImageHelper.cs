using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace FileRenamer
{
    class ImageHelper : MediaHelper
    {
        public ImageHelper(String path, Int32 addCounter)
			: base(path, addCounter)
        {
            read = info.OpenRead();

            try
            {
                file = Image.FromStream(read, false, false);
                IsRightType = true;
            }
            catch (ArgumentException)
            {
	            IsRightType = false;
            }
        }

        private static readonly Regex regex = new Regex(":");

        internal virtual Boolean IsRightType { get; }

        private FileStream read { get; }
        private Image file { get; }

        protected override DateTime? getDate()
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
		        return getDateFromInfo();
	        }
        }

        public override void Dispose()
        {
	        read?.Dispose();
	        file?.Dispose();
        }
    }
}
