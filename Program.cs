using System;
using System.Configuration;
using System.IO;
using System.Linq;

namespace FileRenamer
{
	class Program
	{
		public static void Main(string[] args)
		{
			var origin = ConfigurationManager.AppSettings["Origin"];
			var destiny = ConfigurationManager.AppSettings["Destiny"];
			var format = ConfigurationManager.AppSettings["Format"];

			var files = Directory.GetFiles(origin)
				.OrderBy(n => n).ToList();

			foreach (var file in files)
			{
				String newName = null;

				for (var s = 0; newName == null; s++)
				{
					newName = getNewName(file, format, s);

					if (newName == "") continue;

					var newCompleteName = Path.Combine(destiny, newName);

					if (File.Exists(newCompleteName))
						newName = null;
					else
						File.Move(file, newCompleteName);
				}

				Console.WriteLine($"{files.IndexOf(file)}/{files.Count}");
			}

		}

		private static String getNewName(String file, String format, Int32 sumMilliseconds)
		{
			using (var image = new ImageHelper(file, sumMilliseconds))
			{
				if (image.IsImage)
				{
					var date = image.DateTaken;

					if (date == null)
						return "";

					return date.Value.ToString(format)
					       + image.Extension;
				}
			}

			using (var video = new VideoHelper(file, sumMilliseconds))
			{
				if (video.IsVideo)
				{
					var date = video.DateTaken;

					if (date == null)
						return "";

					return date.Value.ToString(format)
					       + video.Extension;
				}
			}

			return "";
		}
	}
}
