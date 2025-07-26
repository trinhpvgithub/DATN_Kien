using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Kien.Model
{
	public static class ColumnsAnalysis
	{
		public static List<List<XYZ>> GetPointForRebar(this ColumnInfor column, Type type, int quarity)
		{
			if (column == null || column.Column == null || quarity == 1)
			{
				return new List<List<XYZ>>();
			}
			List<List<XYZ>> points = new List<List<XYZ>>();
			var cover = column.Cover;
			switch (type)
			{
				case Type.Radius:
				case Type.Level1:
					cover = column.Cover;
					break;
				case Type.Level2:
					cover = column.Cover * 2;
					break;
				case Type.Level3:
					cover = column.Cover * 3;
					break;
				default:
					cover = column.Cover;
					break;
			}
			var width = column.Width - 2 * cover;
			var height = column.Height - 2 * cover;
			var length = column.Length;
			var location = column.Location;
			var handOrientation = column.HandOrientation;
			var facingOrientation = column.FacingOrientation;
			var crossOrientation = -facingOrientation.CrossProduct(handOrientation);
			var p1 = location.Add(handOrientation * width / 2).Add(facingOrientation * height / 2);
			var p2 = location.Add(handOrientation * width / 2).Add(facingOrientation * -height / 2);
			var p3 = location.Add(handOrientation * -width / 2).Add(facingOrientation * height / 2);
			var p4 = location.Add(handOrientation * -width / 2).Add(facingOrientation * -height / 2);
			points.Add(new List<XYZ> { p1, p1.Add(crossOrientation * length) });
			points.Add(new List<XYZ> { p2, p2.Add(crossOrientation * length) });
			points.Add(new List<XYZ> { p3, p3.Add(crossOrientation * length) });
			points.Add(new List<XYZ> { p4, p4.Add(crossOrientation * length) });

			if (quarity == 3)
			{
				var p5 = location.Add(facingOrientation * height / 2);
				var p6 = location.Add(facingOrientation * -height / 2);
				points.Add(new List<XYZ> { p5, p5.Add(crossOrientation * length) });
				points.Add(new List<XYZ> { p6, p6.Add(crossOrientation * length) });
			}
			else if (quarity == 4)
			{
				var spacing = width / 3;
				var p5 = location.Add(handOrientation * spacing / 2).Add(facingOrientation * height / 2);
				var p6 = location.Add(handOrientation * spacing / 2).Add(facingOrientation * -height / 2);
				var p7 = location.Add(handOrientation * -spacing / 2).Add(facingOrientation * height / 2);
				var p8 = location.Add(handOrientation * -spacing / 2).Add(facingOrientation * -height / 2);
				points.Add(new List<XYZ> { p5, p5.Add(crossOrientation * length) });
				points.Add(new List<XYZ> { p6, p6.Add(crossOrientation * length) });
				points.Add(new List<XYZ> { p7, p7.Add(crossOrientation * length) });
				points.Add(new List<XYZ> { p8, p8.Add(crossOrientation * length) });
			}
			else if (quarity == 5)
			{
				var spacing = width / 4;
				var p5 = location.Add(handOrientation * spacing).Add(facingOrientation * height / 2);
				var p6 = location.Add(handOrientation * spacing).Add(facingOrientation * -height / 2);
				var p7 = location.Add(handOrientation * -spacing).Add(facingOrientation * height / 2);
				var p8 = location.Add(handOrientation * -spacing).Add(facingOrientation * -height / 2);
				var p9 = location.Add(handOrientation.Add(facingOrientation * height / 2));
				var p10 = location.Add(handOrientation.Add(facingOrientation * -height / 2));
				points.Add(new List<XYZ> { p5, p5.Add(crossOrientation * length) });
				points.Add(new List<XYZ> { p6, p6.Add(crossOrientation * length) });
				points.Add(new List<XYZ> { p7, p7.Add(crossOrientation * length) });
				points.Add(new List<XYZ> { p8, p8.Add(crossOrientation * length) });
				points.Add(new List<XYZ> { p9, p9.Add(crossOrientation * length) });
				points.Add(new List<XYZ> { p10, p10.Add(crossOrientation * length) });
			}
			return points;
		}
	}
}
