using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using CommunityToolkit.Mvvm.DependencyInjection;
using DATN_Kien.Model;
using DATN_Kien.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DATN_Kien.ViewModel
{
	public partial class RebarColumnViewModel : ObservableObject
	{
		private Document Document { get; set; }
		private UIDocument UiDocument { get; set; }

		private List<Element> Elements { get; set; }
		public MainView View { get; set; }
		[ObservableProperty]
		private bool _isRound;
		[ObservableProperty]
		private bool _isMirror;
		[ObservableProperty]
		private bool _level1;
		[ObservableProperty]
		private bool _level2;
		[ObservableProperty]
		private bool _level3;
		[ObservableProperty]
		private string _imagePath;
		[ObservableProperty]
		private List<string> _columnMarks;
		[ObservableProperty]
		private string _selectedColumnMark;
		[ObservableProperty]
		private List<RebarBarType> _rebarType;
		[ObservableProperty]
		private RebarBarType _rebar1;
		[ObservableProperty]
		private RebarBarType _rebar2;
		[ObservableProperty]
		private RebarBarType _rebar3;
		[ObservableProperty]
		private List<int> _sothanh;
		[ObservableProperty]
		private int _thanh1;
		[ObservableProperty]
		private int _thanh2;
		[ObservableProperty]
		private int _thanh3;
		[RelayCommand]
		private void Close()
		{
			View.Close();
		}
		[RelayCommand]
		private void Ok()
		{
			var rebarShape = new FilteredElementCollector(Document)
				.OfClass(typeof(RebarShape))
				.Cast<RebarShape>()
				.FirstOrDefault(x => x.Name.Equals("M_T1"));
            using (Transaction transaction = new Transaction(Document, "Create Rebar Column"))
			{
				transaction.Start();
				foreach (var element in Elements)
				{
					var familyIntance = element as FamilyInstance;
					if (familyIntance != null)
					{
						var column = new ColumnInfor(familyIntance);
						var points = column.GetPointForRebar(Model.Type.Level1, Thanh1);
						foreach (var item in points)
						{
							var l = Line.CreateBound(item[0], item[1]) as Curve;
							Document.CreateRebarSingle(RebarStyle.Standard, Rebar1, familyIntance, column.FacingOrientation, new List<Curve>() { l });
						}
						if (IsMirror)
						{
							if(Level2)
							{
								points = column.GetPointForRebar(Model.Type.Level2, Thanh2);
								foreach (var item in points)
								{
									var l = Line.CreateBound(item[0], item[1]) as Curve;
									Document.CreateRebarSingle(RebarStyle.Standard, Rebar2, familyIntance, column.FacingOrientation, new List<Curve>() { l });
								}
							}
							if(Level3)
							{
								points = column.GetPointForRebar(Model.Type.Level3, Thanh3);
								foreach (var item in points)
								{
									var l = Line.CreateBound(item[0], item[1]) as Curve;
									Document.CreateRebarSingle(RebarStyle.Standard, Rebar3, familyIntance, column.FacingOrientation, new List<Curve>() { l });
								}
							}
						}
						var pps = column.GetPointRebarStirrup();
                        foreach (var item in pps)
                        {
							var origin = item[0];
							var xVector = item[1];
							var yVector = item[2];
                            Rebar stirrup = Rebar.CreateFromRebarShape(Document, rebarShape, Rebar1, column.Column, origin, xVector, yVector);
                            stirrup.GetShapeDrivenAccessor().ScaleToBox(origin, xVector, yVector);
                        }
                    }
				}
				transaction.Commit();
			}
		}
		public RebarColumnViewModel(Document document, UIDocument uIDocument)
		{
			Document = document ?? throw new ArgumentNullException(nameof(document), "Document cannot be null");
			UiDocument = uIDocument;
			GetData();
			Level1 = true;
			Level2 = true;
			Level3 = true;
			IsMirror = true;
		}
		private void GetData()
		{
			ColumnMarks = GetAllColumnMarks();
			SelectedColumnMark = ColumnMarks.FirstOrDefault() ?? string.Empty;
			// Lấy danh sách các loại thanh thép (RebarBarType) từ tài liệu Revit
			RebarType = new FilteredElementCollector(Document)
				.OfClass(typeof(RebarBarType))
				.Cast<RebarBarType>()
				.ToList();
			Rebar1 = RebarType.FirstOrDefault() ?? null;
			Rebar2 = RebarType.FirstOrDefault() ?? null;
			Rebar3 = RebarType.FirstOrDefault() ?? null;
			Sothanh = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
			Thanh1 = Sothanh.FirstOrDefault();
			Thanh2 = Sothanh.FirstOrDefault();
			Thanh3 = Sothanh.FirstOrDefault();
		}
		public void Run()
		{
			View = new MainView();
			View.DataContext = this;
            if(!ColumnMarks.Any())
			{
				TaskDialog.Show("Thông báo", "Không tìm thấy cột nào có Mark. Vui lòng kiểm tra lại.");
				return;
            }
            View.ShowDialog();
		}
		protected override void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			// Additional logic if needed when a property changes
			if (e.PropertyName == nameof(IsRound) || e.PropertyName == nameof(IsMirror))
			{
				if (IsRound)
				{
					Level1 = true;
					Level2 = false;
					Level3 = false;
				}
				else
				{
					Level1 = true;
					Level2 = true;
					Level3 = true;
				}
				SetImagePath();
			}
			if (e.PropertyName == nameof(SelectedColumnMark))
			{
				HightLightColumns();
			}
		}
		private void SetImagePath()
		{
			if (IsRound) ImagePath = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))!, @$"DATN_Kien\Resources\chuvi.png");
			else ImagePath = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))!, @$"DATN_Kien\Resources\doixung.png");
		}

		public List<string> GetAllColumnMarks()
		{
			// Lọc tất cả các cột trong model (FamilyInstance loại Structural Columns)
			var collector = new FilteredElementCollector(Document)
				.OfCategory(BuiltInCategory.OST_StructuralColumns)
				.WhereElementIsNotElementType()
				.Cast<FamilyInstance>();

			List<string> markList = new List<string>();

			foreach (var column in collector)
			{
				// Lấy giá trị của parameter "Mark"
				Parameter markParam = column.get_Parameter(BuiltInParameter.ALL_MODEL_MARK);

				if (markParam != null && markParam.HasValue)
				{
					string mark = markParam.AsString();
					if (!string.IsNullOrEmpty(mark))
					{
						markList.Add(mark);
					}
				}
			}

			return markList.Distinct().ToList(); // Loại bỏ trùng
		}

		private void HightLightColumns()
		{
			var collector = new FilteredElementCollector(Document)
							.OfCategory(BuiltInCategory.OST_StructuralColumns)
							.WhereElementIsNotElementType();

			// Lọc các cột có cùng giá trị Mark
			var matchingColumns = collector
				.Where(e =>
				{
					string mark = e.get_Parameter(BuiltInParameter.ALL_MODEL_MARK)?.AsString();
					return !string.IsNullOrEmpty(mark) && mark.Equals(SelectedColumnMark, StringComparison.OrdinalIgnoreCase);
				})
				.ToList();
			Elements = matchingColumns;
			// Lấy danh sách ElementId
			List<ElementId> ids = matchingColumns.Select(e => e.Id).ToList();

			// Highlight trong giao diện người dùng
			UiDocument.Selection.SetElementIds(ids);
		}
	}
}
