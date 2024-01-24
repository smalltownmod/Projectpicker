using Avalonia.Controls;
using System;
using System.Collections.Generic;
using MsBox.Avalonia;

namespace ProjectPicker {
	public partial class MainWindow : Window {
		public List<string> typelist { get; set; }
		public MainWindow() {
			InitializeComponent();
			typelist =   new() { "Avalonia App", "Avalonia MVVM", "WPF", "Winforms", "Console" };
			Type.ItemsSource = typelist;
			Type.SelectedIndex = 0;
		}

		public void BtnCreate_click(object sender, Avalonia.Interactivity.RoutedEventArgs e) {
			if(ProjName.Text == null || ProjName.Text.Trim() == string.Empty) {
				MessageBoxManager.GetMessageBoxStandard("Error", "Project has no Name!!").ShowAsync();	
			}
		}
	}
}