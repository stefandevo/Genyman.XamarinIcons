using System;
using System.Collections.Generic;
using Genyman.Core;

namespace Stefandevo.Genyman.XamarinIcons.Implementation
{
	public class Configuration
	{
		public List<PlatformClass> Platforms { get; set; }
	}

	public class PlatformClass
	{
		public Platforms Type { get; set; }
		public string ProjectPath { get; set; }
		public string IconFileName { get; set; }
		public AndroidOptions AndroidOptions { get; set; }
	}

	public class AndroidOptions
	{
		public string AssetFolderPrefix { get; set; }
	}

	public enum Platforms
	{
		iOS,
		Android,
		UWP,
		AppleWatch,
		MacOs
	}
}		