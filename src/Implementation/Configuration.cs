using System;
using System.Collections.Generic;
using Genyman.Core;

namespace Stefandevo.Genyman.XamarinIcons.Implementation
{
	[Documentation(Source = "https://github.com/stefandevo/Genyman.XamarinIcons")]
	public class Configuration
	{
		[Description("List of platforms")]
		[Required]
		public List<PlatformClass> Platforms { get; set; }
	}

	[Documentation]
	public class PlatformClass
	{
		[Description("Platform")]
		[Required]
		public Platforms Type { get; set; }

		[Description("Path to the platform, do not include filename")]
		[Required]
		public string ProjectPath { get; set; }

		[Description("Filename (svg) of the file to be used")]
		[Required]
		public string IconFileName { get; set; }

		[Description("Extra Android options")]
		public AndroidOptions AndroidOptions { get; set; }
	}

	[Documentation]
	public enum Platforms
	{
		[Description("iOS")] iOS,
		[Description("Android")] Android,
		[Description("UWP")] UWP,
		[Description("Apple Watch")] AppleWatch,
		[Description("Mac OS")] MacOs
	}

	[Documentation]
	public class AndroidOptions
	{
		[Description("The folder where resources are")]
		public AndroidResourceFolder AssetFolderPrefix { get; set; }
	}

	[Documentation]
	public enum AndroidResourceFolder
	{
		[Description("Use mipmap folders")] mipmap,
		[Description("Use drawable folders")] drawable
	}
}