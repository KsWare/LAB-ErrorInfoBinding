﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Data;

namespace ErrorInfoBinding {
	/// <summary>
	/// Interaction logic for DataErrorInfoControl.xaml
	/// </summary>
	public partial class DataErrorInfo2Control : UserControl {
		public DataErrorInfo2Control() {
			InitializeComponent();
		}
	}

	public class DataErrorInfo2ControlVM : INotifyPropertyChanged, IDataErrorInfo {

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
					OnPropertyChanged(Binding.IndexerName);
				}
				_a = value;
				OnPropertyChanged();
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
					OnPropertyChanged(Binding.IndexerName);
				}
				_b = value;
				OnPropertyChanged();
			}
		}

		#region INotifyPropertyChanged

		private void OnPropertyChanged([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region IDataErrorInfo

		public string this[string columnName] => _errors.TryGetValue(columnName, out var error) ? error : null;

		public string Error => _errors.Count > 0 ? $"{_errors.Count} errors." : null;

		#endregion

	}
}
