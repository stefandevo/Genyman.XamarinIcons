using System.Collections.Generic;

namespace Stefandevo.Genyman.XamarinIcons.Implementation
{
	public class NewTemplate : Configuration
	{
		public NewTemplate()
		{
			Platforms = new List<PlatformClass>()
			{
				new PlatformClass()
				{
					Type = Implementation.Platforms.iOS,
					ProjectPath = "YourProject.iOS",
					IconFileName = "YourIOSIconFile.svg"
				},
				new PlatformClass()
				{
					Type = Implementation.Platforms.Android,
					ProjectPath = "YourProject.Droid",
					IconFileName = "YourAndroidIconFile.svg",
					AndroidOptions = new AndroidOptions()
					{
						AssetFolderPrefix = "mipmap"
					}
				}
			};
		}
	}
}