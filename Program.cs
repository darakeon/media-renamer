using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace FileRenamer
{
	class Program
	{
        static readonly String origin = ConfigurationManager.AppSettings["Origin"];
        static readonly String destiny = ConfigurationManager.AppSettings["Destiny"];

        static Regex regex = new Regex(@"(\d{4})\-(\d{2}).*");

        public static void Main(string[] args)
		{
			var files = Directory.GetFiles(origin)
				.OrderBy(n => n).ToList();

			foreach (var file in files)
			{
				String newName = null;

				for (var s = 0; newName == null; s++)
				{
					newName = MediaHelper.GetNewName(file, s);

					if (newName == "") continue;

					newName = regex.Replace(newName, @"$1\$2\$0");

					var newCompleteName = Path.Combine(destiny, newName);

					if (File.Exists(newCompleteName))
					{
						newName = null;
						continue;
					}

					var dir = Directory
						.GetParent(newCompleteName)
						.FullName;

					if (!Directory.Exists(dir))
						Directory.CreateDirectory(dir);

					File.Move(file, newCompleteName);
				}

				Console.WriteLine($"{files.IndexOf(file)+1}/{files.Count}");
			}

			Console.ReadLine();
		}
	}
}
