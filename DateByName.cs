using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace FileRenamer
{
	class DateByName
	{
		private readonly String pattern;
		private readonly String format;

		private DateByName([RegexPattern] String pattern, String format)
		{
			this.pattern = pattern;
			this.format = format;
		}

		private DateTime? get(String name)
		{
			var regexDatedName = new Regex(pattern + @"\.\w{3,4}$");
			var match = regexDatedName.Match(name);

			if (!match.Success)
				return null;

			var date = match.Groups[1].Value;
			var culture = new CultureInfo("pt-BR");

			return DateTime.ParseExact(date, format, culture);
		}

		private static readonly List<DateByName> imageFormats =
			new List<DateByName>
			{
				new DateByName(
					@"(\d{4}-\d{2}-\d{2} \d{2}.\d{2}.\d{2})",
					"yyyy-MM-dd HH.mm.ss"
				),

				new DateByName(
					@"IMG_(\d{8}_\d{6})",
					"yyyyMMdd_HHmmss"
				),

				new DateByName(
					@"IMG_(\d{8}_\d{6})_\d+",
					"yyyyMMdd_HHmmss"
				),

				new DateByName(
					@"IMG-(\d{8})-WA\d{4}",
					"yyyyMMdd"
				),

				new DateByName(
					@"(\d{4}-\d{2}-\d{2}_\d{2}-\d{2}-\d{2}_\d{3})-ANIMATION",
					"yyyy-MM-dd_HH-mm-ss_fff"
				),

				new DateByName(
					@"V_(\d{8}_\d{6})_N\d+-ANIMATION",
					"yyyyMMdd_HHmmss"
				),

				new DateByName(
					@"V_(\d{8}_\d{6})_N\d+-ANIMATION",
					"yyyyMMdd_HHmmss"
				),

				new DateByName(
					@"Screenshot_(\d{4}-\d{2}-\d{2}-\d{2}-\d{2}-\d{2})",
					"yyyy-MM-dd-HH-mm-ss"
				),

				new DateByName(
					@"Screenshot_(\d{4}-\d{2}-\d{2}-\d{2}-\d{2}-\d{2})-scale-\d+",
					"yyyy-MM-dd-HH-mm-ss"
				),

				new DateByName(
					@"Screenshot_(\d{4}-\d{2}-\d{2}-\d{2}-\d{2}-\d{2})_\d+",
					"yyyy-MM-dd-HH-mm-ss"
				),

				new DateByName(
					@"Screenshot_(\d{8}-\d{6})",
					"yyyyMMdd-HHmmss"
				),

				new DateByName(
					@"Screenshot_(\d{8}-\d{6})-scale-\d+",
					"yyyyMMdd-HHmmss"
				),

				new DateByName(
					@"Screenshot_(\d{8}-\d{6})_\d+",
					"yyyyMMdd-HHmmss"
				),

				new DateByName(
					@"Point Blur_(\w{3}\d{6}_\d{6})",
					"MMMddyyyy_HHmmss"
				),

				new DateByName(
					@"P_(\d{8}_\d{6})-scale-\d+",
					"yyyyMMdd_HHmmss"
				),

				new DateByName(
					@"P_(\d{8}_\d{6})_\d+",
					"yyyyMMdd_HHmmss"
				),
			};

		private static readonly List<DateByName> videoFormats =
			new List<DateByName>
			{
				new DateByName(
					@"video-(\d{4}-\d{2}-\d{2}-\d{2}-\d{2}-\d{2})",
					"yyyy-MM-dd-HH-mm-ss"
				),

				new DateByName(
					@"VID_(\d{8}_\d{6})",
					"yyyyMMdd_HHmmss"
				),

				new DateByName(
					@"VID_(\d{8}_\d{6}_\d{3})",
					"yyyyMMdd_HHmmss_fff"
				),

				new DateByName(
					@"V_(\d{8}_\d{6})",
					"yyyyMMdd_HHmmss"
				),

				new DateByName(
					@"V_(\d{8}_\d{6})_N\d+",
					"yyyyMMdd_HHmmss"
				),

				new DateByName(
					@"VID-(\d{8})-WA\d+",
					"yyyyMMdd"
				),
			};

		public static DateTime? GetFromImage(FileInfo info)
		{
			return get(info, imageFormats);
		}

		public static DateTime? GetFromVideo(FileInfo info)
		{
			return get(info, videoFormats);
		}

		private static DateTime? get(FileInfo info, IList<DateByName> formats)
		{
			foreach (var dateByName in formats)
			{
				var date = dateByName.get(info.Name);
				if (date != null) return date;
			}

			return null;
		}
	}
}