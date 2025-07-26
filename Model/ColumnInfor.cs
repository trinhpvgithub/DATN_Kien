using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Kien.Model
{
	public class ColumnInfor
	{
		public FamilyInstance Column { get; set; }
		public double Width { get; set; }
		public double Height { get; set; }
		public double Length { get; set; }
		public XYZ Location { get; set; }
		public XYZ HandOrientation {  get; set; }
		public XYZ FacingOrientation { get; set; }
		public double Cover { get; set; } = 50 / 304.8;	 // Default cover value in mm converted to feet (50 mm)
		public ColumnInfor(FamilyInstance column)
		{
			Column = column;
			Width = Column.GetParameter("b").AsDouble();
			Height = Column.GetParameter("h").AsDouble();
			Length = Column.GetParameter(BuiltInParameter.INSTANCE_LENGTH_PARAM).AsDouble();
			Location = Column.Location as LocationPoint != null ? (Column.Location as LocationPoint).Point : new XYZ(0, 0, 0);
			HandOrientation = Column.HandOrientation;
			FacingOrientation = Column.FacingOrientation;
		}
	}
}
