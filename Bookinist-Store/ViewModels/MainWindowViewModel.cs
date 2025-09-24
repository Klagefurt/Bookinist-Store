using MathCore.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookinist_Store.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
		/// <summary>
		/// Title
		/// </summary>
		private string _title;

		public string Title
		{
			get { return _title; }
			set { _title = value; }
		}




	}
}
