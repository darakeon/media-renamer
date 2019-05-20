using System;
using System.IO;

namespace FileRenamer
{
	internal abstract class MediaHelper : IDisposable
	{
		public abstract void Dispose();

		protected abstract Boolean isRightType { get; }

		protected FileInfo info { get; }
		private Int32 addCounter { get; }

		protected MediaHelper(String path, Int32 addCounter)
		{
			this.addCounter = addCounter;
			info = new FileInfo(path);
		}

		public DateTime? DateTaken => getDate();

		public String Extension => info.Extension;

		protected abstract DateTime? getDate();

		protected DateTime? getDateFromInfo()
		{
			var date = info.CreationTime;

			if (date > info.LastAccessTime)
				date = info.LastAccessTime;

			if (date > info.LastWriteTime)
				date = info.LastWriteTime;

			return date;
		}

		private String getNewName()
		{
			if (DateTaken == null)
				return "";

			var date = DateTaken.Value;

			var format = date == date.Date
				? "yyyy-MM-dd_HH-mm-ss_fff"
				: "yyyy-MM-dd";

			var name = DateTaken.Value.ToString(format);

			if (addCounter > 0)
				name += $"_{addCounter}";

			return name + Extension;
		}

		internal static String GetNewName(String file, Int32 sumMilliseconds)
		{
			using (var media = new ImageHelper(file, sumMilliseconds))
			{
				if (media.isRightType)
				{
					return media.getNewName();
				}
			}

			using (var media = new VideoHelper(file, sumMilliseconds))
			{
				if (media.isRightType)
				{
					return media.getNewName();
				}
			}

			return "";
		}
	}
}
