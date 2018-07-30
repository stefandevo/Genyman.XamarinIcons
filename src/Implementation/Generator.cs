using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Genyman.Core;
using Genyman.Core.Helpers;
using ServiceStack;
using ServiceStack.Text;
using SkiaSharp;
using Stefandevo.Genyman.XamarinIcons.Implementation.Support;
using SKSvg = SkiaSharp.Extended.Svg.SKSvg;

namespace Stefandevo.Genyman.XamarinIcons.Implementation
{
	public class Generator : GenymanGenerator<Configuration>
	{
		public override void Execute()
		{
			JsConfig.IncludeNullValues = false;
			JsConfig.EmitCamelCaseNames = true;

			foreach (var platform in Configuration.Platforms)
			{
				var fileName = Path.Combine(WorkingDirectory, platform.IconFileName);
				var fileInfo = new FileInfo(fileName);
				if (!File.Exists(fileInfo.FullName))
				{
					Log.Error($"Icon not found ({fileInfo.FullName})");
					continue;
				}

				var svg = new SKSvg();
				svg.Load(fileInfo.FullName);

				var sourceActualWidth = svg.Picture.CullRect.Width;
				var sourceActualHeight = svg.Picture.CullRect.Height;

				if (sourceActualWidth != 1024 || sourceActualHeight != 1024) Log.Warning("For best result the source app icon should be 1024x1024");

				var assets = new List<ExportAsset>();

				var platformPath = new DirectoryInfo(Path.Combine(WorkingDirectory, platform.ProjectPath));
				if (!platformPath.Exists)
				{
					Log.Error($"Platform path for {platform.Type} not found ({platformPath.FullName})");
					continue;
				}

				switch (platform.Type)
				{
					case Platforms.iOS:
					{
						var targetPath = Path.Combine(platformPath.FullName, "Assets.xcassets", "AppIcon.appiconset");
						var sizes = new[] {20, 29, 40, 58, 60, 76, 80, 87, 120, 152, 167, 180, 1024};
						assets.AddRange(sizes.Select(size => new ExportAsset(fileInfo.FullName, targetPath, $"Icon-App-{size}x{size}.png", sourceActualWidth, sourceActualHeight, size, size)));
						break;
					}
					case Platforms.AppleWatch:
					{
						var targetPath = Path.Combine(platformPath.FullName, "Assets.xcassets", "AppIcon.appiconset");
						var sizes = new[] {48, 55, 58, 80, 87, 88, 172, 196};
						assets.AddRange(sizes.Select(size => new ExportAsset(fileInfo.FullName, targetPath, $"Icon-App-{size}x{size}.png", sourceActualWidth, sourceActualHeight, size, size)));
						break;
					}
					case Platforms.MacOs:
					{
						var targetPath = Path.Combine(platformPath.FullName, "Assets.xcassets", "AppIcon.appiconset");
						var sizes = new[] {16, 32, 64, 128, 256, 512, 1024};
						assets.AddRange(sizes.Select(size => new ExportAsset(fileInfo.FullName, targetPath, $"Icon-App-{size}x{size}.png", sourceActualWidth, sourceActualHeight, size, size)));
						break;
					}
					case Platforms.Android:
					{
						var folderPrefix = "mipmap";
						if (platform.AndroidOptions != null && !string.IsNullOrEmpty(platform.AndroidOptions.AssetFolderPrefix))
							folderPrefix = platform.AndroidOptions.AssetFolderPrefix;
						assets.Add(new ExportAsset(fileInfo.FullName, Path.Combine(platformPath.FullName, "Resources", $"{folderPrefix}-mdpi"), "Icon.png", sourceActualWidth, sourceActualHeight, 48,
							48));
						assets.Add(new ExportAsset(fileInfo.FullName, Path.Combine(platformPath.FullName, "Resources", $"{folderPrefix}-hdpi"), "Icon.png", sourceActualWidth, sourceActualHeight, 72,
							72));
						assets.Add(new ExportAsset(fileInfo.FullName, Path.Combine(platformPath.FullName, "Resources", $"{folderPrefix}-xhdpi"), "Icon.png", sourceActualWidth, sourceActualHeight, 96,
							96));
						assets.Add(new ExportAsset(fileInfo.FullName, Path.Combine(platformPath.FullName, "Resources", $"{folderPrefix}-xxhdpi"), "Icon.png", sourceActualWidth, sourceActualHeight,
							144, 144));
						assets.Add(new ExportAsset(fileInfo.FullName, Path.Combine(platformPath.FullName, "Resources", $"{folderPrefix}-xxxhdpi"), "Icon.png", sourceActualWidth, sourceActualHeight,
							192, 192));
						break;
					}
					case Platforms.UWP:
					{
						// https://docs.microsoft.com/en-us/windows/uwp/design/shell/tiles-and-notifications/app-assets#asset-size-tables
						var targetPath = Path.Combine(platformPath.FullName, "Assets");
						var sizes = new[] {71, 150, 310, 44};
						var scales = new[] {100, 125, 150, 200, 400};
						foreach (var size in sizes)
							assets.AddRange(scales.Select(scale =>
								new ExportAsset(fileInfo.FullName, targetPath, $"Square{size}x{size}Logo.scale-{scale}.png", sourceActualWidth, sourceActualHeight, size, size)));
						break;
					}
					default:
						throw new ArgumentOutOfRangeException();
				}

				foreach (var asset in assets)
				{
					var bmp = new SKBitmap((int) asset.ScaledWidth, (int) asset.ScaledHeight);
					var canvas = new SKCanvas(bmp);
					var matrix = SKMatrix.MakeScale((float) asset.Ratio, (float) asset.Ratio);
					canvas.Clear(SKColors.Transparent);
					canvas.DrawPicture(svg.Picture, ref matrix);
					canvas.Save();

					// Export the canvas
					var img = SKImage.FromBitmap(bmp);

					var data = img.Encode(SKEncodedImageFormat.Png, 100);
					using (var fs = File.Open(asset.FileName, FileMode.Create))
					{
						Log.Information($"Writing {asset.FileName}");
						data.SaveTo(fs);
					}

					var platformProjectFolder = Path.Combine(WorkingDirectory, platform.ProjectPath);

					var destinationFolder = "Resources";
					if (platform.Type == Platforms.UWP)
						destinationFolder = "Assets";
					var destinationFile = Path.Combine(WorkingDirectory, platform.ProjectPath, destinationFolder, asset.FileName);

					switch (platform.Type)
					{
						case Platforms.iOS:
						case Platforms.AppleWatch:
						case Platforms.MacOs:
							platformProjectFolder.AddXamarinIosImageAsset(destinationFile);
							break;
						case Platforms.Android:
							platformProjectFolder.AddXamarinAndroidResource(destinationFile);
							break;
						case Platforms.UWP:
							platformProjectFolder.AddXamarinUWPResource(destinationFile);
							break;
					}
				}

				if (platform.Type == Platforms.iOS || platform.Type == Platforms.AppleWatch || platform.Type == Platforms.MacOs)
				{
					var contentsFile = Path.Combine(platformPath.FullName, "Assets.xcassets", "AppIcon.appiconset", "Contents.json");
					Log.Information($"Writing {contentsFile}");

					var contents = File.ReadAllText(contentsFile).FromJson<XcodeContents>();

					foreach (var contentsImage in contents.Images)
					{
						var process = false;
						switch (platform.Type)
						{
							case Platforms.iOS when contentsImage.Idiom == "iphone" || contentsImage.Idiom == "ipad" || contentsImage.Idiom == "ios-marketing":
							case Platforms.AppleWatch when contentsImage.Idiom == "watch":
							case Platforms.MacOs when contentsImage.Idiom == "mac":
								process = true;
								break;
						}

						if (process)
						{
							// calculate needed size
							var size = double.Parse(contentsImage.Size.Split('x')[0], NumberStyles.Float, new CultureInfo("en-US"));
							var scale = double.Parse(contentsImage.Scale.Substring(0, 1));

							var foundSize = assets.FirstOrDefault(a => a.ScaledWidth == size * scale);
							if (foundSize != null) contentsImage.Filename = new FileInfo(foundSize.FileName).Name;
						}
					}

					File.WriteAllText(contentsFile, contents.ToJson().IndentJson());
				}
			}
		}
	}
}