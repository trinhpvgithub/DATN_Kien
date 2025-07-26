using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Kien.Model
{
	public static class RebarUtils
	{
		public static Rebar CreateRebarSingle(this Document document, RebarStyle rebarStyle, RebarBarType rebarBarType, Element host, XYZ normal, List<Curve> curves)
		{
			var rebar = Rebar.CreateFromCurves(document, rebarStyle, rebarBarType, null, null, host, normal, curves, RebarHookOrientation.Left,
				RebarHookOrientation.Left, true, true);
			return rebar;
		}
	}
}
