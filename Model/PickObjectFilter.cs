using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Kien.Model
{
    class PickObjectFilter : ISelectionFilter
    {
        private readonly string _markValue;

        public PickObjectFilter(string markValue)
        {
            _markValue = markValue;
        }

        public bool AllowElement(Element elem)
        {
            if (elem is not FamilyInstance familyInstance)
                return false;

            if ((familyInstance.Location as LocationCurve)?.Curve == null)
                return false;

            if (familyInstance.Category.Id.IntegerValue != (int)BuiltInCategory.OST_StructuralColumns)
                return false;

            Parameter markParam = familyInstance.get_Parameter(BuiltInParameter.ALL_MODEL_MARK);
            if (markParam == null || !markParam.HasValue)
                return false;

            string mark = markParam.AsString();

            // So sánh mark với giá trị được truyền vào
            return string.Equals(mark, _markValue, StringComparison.OrdinalIgnoreCase);
        }
        public bool AllowReference(Reference reference, XYZ position)
        {
            throw new NotImplementedException();
        }
    }
}
