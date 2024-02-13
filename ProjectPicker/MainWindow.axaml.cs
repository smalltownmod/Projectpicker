using Avalonia.Controls;
using System.Collections.Generic;
using MsBox.Avalonia;
using ProjectPicker.models;

namespace ProjectPicker {
	public partial class MainWindow : Window {
		private List<string> typelist { get; set; }
		private Properties _props;
		public MainWindow() {
			InitializeComponent();
			typelist = new() { "Avalonia App", "Avalonia MVVM", "WPF", "Winforms", "Console" };
			TypeSel.ItemsSource = typelist;
			TypeSel.SelectedIndex = 0;
			List<string> targetFW = new() {"net8.0", "net7.0", "net6.0" };
		}

		public void BtnCreate_click(object sender, Avalonia.Interactivity.RoutedEventArgs e) {
			if(ProjName.Text == null || ProjName.Text.Trim() == string.Empty) MessageBoxManager.GetMessageBoxStandard("Error", "Project has no Name!!").ShowAsync();	
			else {
				DirectoryWindow dirwin = new();
				
				_props = new Properties {
					Title = ProjName.Text ?? string.Empty,
					Type = TypeSel.SelectedItem.ToString() ?? string.Empty,
					hasTest = Tests.IsChecked ?? false
				};
				dirwin.Props = _props;
				dirwin.ShowDialog(this);
			}
		}
	}
}