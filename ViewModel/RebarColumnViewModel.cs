using DATN_Kien.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Kien.ViewModel
{
    public partial class RebarColumnViewModel: ObservableObject
	{
		public MainView View { get; set; }
		[ObservableProperty]
        private bool _isRound = false;
		[ObservableProperty]
		private bool _isMirror = true;
		public RebarColumnViewModel()
		{
			// Constructor logic if needed
		}
		public void Run()
		{
			View = new MainView();
			View.DataContext = this;
			View.ShowDialog();
		}
	}
}
