using DATN_Kien.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Kien.ViewModel
{
	public partial class RebarColumnViewModel : ObservableObject
	{
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
		private string _imagePath = "pack://application:,,,/Resources/Images/RebarColumn.png";
		public RebarColumnViewModel()
		{
			// Constructor logic if needed
			Level1 = true;
			Level2 = true;
			Level3 = true;
			IsRound = true;
		}
		public void Run()
		{
			View = new MainView();
			View.DataContext = this;
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
		}
		private void SetImagePath()
		{
			if (IsRound) ImagePath = "Resources/chuvi.png";
			else ImagePath = "Resources/doixung.png";
		}

	}
}
