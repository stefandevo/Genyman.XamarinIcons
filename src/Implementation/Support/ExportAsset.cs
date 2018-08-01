using System.IO;
using Genyman.Core;

namespace Stefandevo.Genyman.XamarinIcons.Implementation.Support
{
	internal class ExportAsset
	{
		readonly string _source;
		readonly string _targetPath;
		readonly string _targetFileName;

		public ExportAsset(string source, string targetPath, string targetFileName, double sourceWidth, double sourceHeight, double width, double height)
		{
			_source = source;
			_targetPath = targetPath;
			_targetFileName = targetFileName;
			
			Ratio = width / sourceWidth;

			ScaledWidth = sourceWidth * Ratio;
			ScaledHeight = sourceHeight * Ratio;
		}
		
		public double Ratio { get; }
		public double ScaledWidth { get; }
		public double ScaledHeight { get; }

		public string FileName
		{
			get
			{
				_targetPath.EnsureFolderExists();
				return Path.Combine(_targetPath, _targetFileName);
			}
		}

		
	}
}