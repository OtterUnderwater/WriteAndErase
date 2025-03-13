using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WriteAndErase.Models
{
	public partial class Product
	{
		[NotMapped]
		public string Background
		{
			get
			{
				if (CurrentDiscount > 15) return "#7fff00";
				else return "#FFFFFF";
			}
		}
		
		[NotMapped]
		public double CostWithDiscount
		{
			get
			{
				if (CurrentDiscount != 0) return ((double)Cost / 100.0) * (100.0 - (double)CurrentDiscount);
				else return (double)Cost;
			}
		}
		
		[NotMapped]
		public double? CostPreview
		{
			get
			{
				if (CurrentDiscount != 0) return (double)Cost;
				else return null;
			}
		}
	}
}
