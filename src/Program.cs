using Stefandevo.Genyman.XamarinIcons.Implementation;
using Genyman.Core;

namespace Stefandevo.Genyman.XamarinIcons
{
	internal class Program
	{
		public static void Main(string[] args)
		{
			GenymanApplication.Run<Configuration, NewTemplate, Generator>(args);
		}
	}
}