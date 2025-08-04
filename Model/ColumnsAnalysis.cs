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
		public static List<List<XYZ>> GetPointRebarStirrup(this ColumnInfor column)
		{
			var points = new List<List<XYZ>>();
			var ps= new List<XYZ>();
			var cover = column.Cover;
			var width = column.Width - cover * 2;
			var height = column.Height - cover * 2;
			var p1 = column.Location.Add(column.HandOrientation * -width / 2).Add(column.FacingOrientation * -height / 2);
			var p2 = column.Location.Add(column.HandOrientation * -width / 2).Add(column.FacingOrientation * height / 2);
			var p3 = column.Location.Add(column.HandOrientation * width / 2).Add(column.FacingOrientation * height / 2);
			var p4 = column.Location.Add(column.HandOrientation * width / 2).Add(column.FacingOrientation * -height / 2);
            ps.Add(p1);
			var xVec = p4 - p1;
			var yVec = p2 - p1;
			ps.Add(xVec);
			ps.Add(yVec);
			points.Add(ps);
			var length = column.Length;
			var spacing = 200 / 304.8;
			var count = (int)length / spacing;
            for (int i = 1; i < count; i++)
            {
				var pps = new List<XYZ>();
				var p = p1.EditZ(i * spacing);
				pps.Add(p);
				pps.Add(xVec);
				pps.Add(yVec);
				points.Add(pps);
            }
            return points;
        }
		private static XYZ EditZ(this XYZ point, double z)
		{
			return new XYZ(point.X, point.Y, z);
        }
		//public static List<List<XYZ>> GetPointForRebarRound(this ColumnInfor column, int quarityX,int quarityY)
		//{
  //          var cover = column.Cover;
  //          var width = column.Width - cover * 2;
  //          var height = column.Height - cover * 2;
  //          double spacingX = width / (quarityX - 1);
  //          double spacingY = height / (quarityY - 1);
  //          List<XYZ> rebarPositions = new List<XYZ>();

  //          for (int i = 0; i < quarityX; i++)
  //          {
  //              double xOffset = min.X + cover / 304.8 + i * spacingX;
  //              rebarPositions.Add(new XYZ(xOffset, min.Y + cover / 304.8, min.Z + cover / 304.8));
  //              rebarPositions.Add(new XYZ(xOffset, max.Y - cover / 304.8, min.Z + cover / 304.8));
  //          }

  //          for (int i = 1; i < quarityY - 1; i++)
  //          {
  //              double yOffset = min.Y + cover / 304.8 + i * spacingY;
  //              rebarPositions.Add(new XYZ(min.X + cover / 304.8, yOffset, min.Z + cover / 304.8));
  //              rebarPositions.Add(new XYZ(max.X - cover / 304.8, yOffset, min.Z + cover / 304.8));

  //          }
  //      }
    }
}
