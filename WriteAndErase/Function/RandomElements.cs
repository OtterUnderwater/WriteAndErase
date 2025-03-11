using Avalonia.Media;
using Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WriteAndErase.Function
{
	public class RandomElements
	{
		private Random _rnd = new Random();
		public string GetRandomText() => _rnd.Next(2) == 0 ? Convert.ToString((char)_rnd.Next('a', 'z' + 1)) : Convert.ToString(_rnd.Next(10));
		public SolidColorBrush GetRandomColor() => new SolidColorBrush(Color.FromRgb(Convert.ToByte(_rnd.Next(256)),
			Convert.ToByte(_rnd.Next(255)), Convert.ToByte(_rnd.Next(255))));
		public Thickness GetRandomThickness(int number) => new Thickness(50 * number, _rnd.Next(40));
	}
}
