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
	public partial class NotifyDataErrorInfo2Control : UserControl {
		public NotifyDataErrorInfo2Control() {
			InitializeComponent();
		}
	}

	public class NotifyDataErrorInfo2ControlVM : INotifyPropertyChanged, INotifyDataErrorInfo, IDataErrorInfo {

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

		// used if Binding.ValidatesOnDataErrors=False

		IEnumerable INotifyDataErrorInfo.GetErrors(string propertyName) => _errors.TryGetValue(propertyName, out var error) ? new []{error} : null;

		bool INotifyDataErrorInfo.HasErrors => _errors.Count > 0;

		public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

		private void OnErrorsChanged([CallerMemberName] string propertyName = null) {
			ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
		}

		#endregion


		#region IDataErrorInfo

		// used if Binding.ValidatesOnDataErrors=True (default)

		public string this[string columnName] => _errors.TryGetValue(columnName, out var error) ? error : null;

		string IDataErrorInfo.Error => _errors.Count > 0 ? $"{_errors.Count} errors." : null;

		#endregion
	}
}
