﻿using System;

namespace FileRenamer
{
	internal class VideoHelper : MediaHelper
	{
		public VideoHelper(string path, int addCounter)
			: base(path, addCounter) { }

		protected override DateTime? getDate()
		{
			return DateByName.GetFromVideo(info)
			       ?? getDateFromInfo();
		}

		protected override Boolean isRightType =>
			Extension == ".mp4"
			|| Extension == ".3gp";

		public override void Dispose()
		{
		}
	}
}
