using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WriteAndErase.Converters
{
	public class ImageConverter : IValueConverter
	{
		public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			return (value == null || value == "")
				? new Bitmap(AssetLoader.Open(new Uri("avares://WriteAndErase/Assets/Photo/picture.png")))
				: new Bitmap(AssetLoader.Open(new Uri($"avares://WriteAndErase/Assets/Photo/{value.ToString()!.Trim()}")));
		}

		public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
