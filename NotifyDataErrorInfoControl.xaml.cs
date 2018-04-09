using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace ErrorInfoBinding {

	/// <summary>
	/// Interaction logic for DataErrorInfoControl.xaml
	/// </summary>
	public partial class NotifyDataErrorInfoControl : UserControl {
		public NotifyDataErrorInfoControl() {
			InitializeComponent();
		}
	}

	public class NotifyDataErrorInfoControlVM : INotifyPropertyChanged, INotifyDataErrorInfo {

		private readonly Dictionary<string, string> _errors = new Dictionary<string, string>();

		private string _a;
		private string _b;

		public string A {
			get { return _a; }
			set {
				if (value != _b) {
					_errors["A"] = "A must equal B!";
				}
				else {
					_errors.Remove("A");
					_errors.Remove("B");
				}
				_a = value;
				OnPropertyChanged();
				OnErrorsChanged();
				OnErrorsChanged("B");
			}
		}

		public string B {
			get { return _b; }
			set {
				if (value != _a) {
					_errors["B"] = "B must equal A!";
				}
				else {
					_errors.Remove("A");
					_errors.Remove("B");
				}
				_b = value;
				OnPropertyChanged();
				OnErrorsChanged();
				OnErrorsChanged("A");
			}
		}

		#region INotifyPropertyChanged
		
		private void OnPropertyChanged([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region INotifyDataErrorInfo

		public IEnumerable GetErrors(string propertyName) => _errors.TryGetValue(propertyName, out var error) ? new []{error} : null;

		public bool HasErrors => _errors.Count > 0;

		public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

		private void OnErrorsChanged([CallerMemberName] string propertyName = null) {
			ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
		}

		#endregion
	}
}
