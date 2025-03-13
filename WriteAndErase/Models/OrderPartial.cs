using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WriteAndErase.Models
{
	public partial class Order
	{

		[NotMapped]
		public string FIO { get; set; }

		[NotMapped]
		public string Color { get; set; }

		[NotMapped]
		public double Cost { get; set; }

		[NotMapped]
		public double Discount { get; set; }
	}
}
