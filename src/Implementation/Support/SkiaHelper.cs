using System.IO;
using System.Xml;
using System.Xml.Linq;
using Genyman.Core;
using SkiaSharp.Extended.Svg;

namespace Stefandevo.Genyman.XamarinIcons.Implementation.Support
{
	internal class SkiaHelper
	{
		static readonly XNamespace XlinkNamespace = "http://www.w3.org/1999/xlink";
		static readonly XNamespace SvgNamespace = "http://www.w3.org/2000/svg";

		static readonly XmlReaderSettings XmlReaderSettings = new XmlReaderSettings
		{
			DtdProcessing = DtdProcessing.Ignore,
			IgnoreComments = true
		};

		static XmlParserContext CreateSvgXmlContext()
		{
			var table = new NameTable();
			var manager = new XmlNamespaceManager(table);
			manager.AddNamespace(string.Empty, SvgNamespace.NamespaceName);
			manager.AddNamespace("xlink", XlinkNamespace.NamespaceName);
			return new XmlParserContext(null, manager, null, XmlSpace.None);
		}

		internal static SKSvg Load(string fileName)
		{
			var result = new SKSvg();

			using (var stream = File.OpenRead(fileName))
			{
				using (var reader = XmlReader.Create(stream, XmlReaderSettings, CreateSvgXmlContext()))
				{
					var preloadSvg = XDocument.Load(reader);

					var root = preloadSvg.Root;

					// In order to avoid rounding errors, always create a 1024x1024 SVG from the input
					var width = root.Attribute("width");
					var height = root.Attribute("height");

					if (width.Value != "1024px" || height.Value != "1024px")
						Log.Warning("For best result the source app icon should be 1024x1024");

					width.Value = "1024px";
					height.Value = "1024px";

					result.Load(preloadSvg.CreateReader());
				}
			}

			return result;
		}
	}
}