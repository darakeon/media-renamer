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

			var files = Directory.GetFiles(origin)
				.OrderBy(n => n).ToList();

			foreach (var file in files)
			{
				String newName = null;

				for (var s = 0; newName == null; s++)
				{
					newName = MediaHelper.GetNewName(file, s);

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
	}
}
