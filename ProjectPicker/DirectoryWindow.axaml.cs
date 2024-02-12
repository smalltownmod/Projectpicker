using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;
using ProjectPicker.models;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace ProjectPicker;

public partial class DirectoryWindow : Window {
	public Properties Props { get; set; }
	public DirectoryWindow() {
		InitializeComponent();
		if (File.Exists("default.txt")) ProjPath.Text = File.ReadAllText("default.txt");
	}
	private async void BtnBrowse_click(object sender, Avalonia.Interactivity.RoutedEventArgs e) {
		ProjPath.Text = await new OpenFolderDialog().ShowAsync((Window)this.GetVisualRoot()) ?? string.Empty;
	}

	private void BtnDefault_click(object sender, Avalonia.Interactivity.RoutedEventArgs e) {
		File.WriteAllText("default.txt", ProjPath.Text);
	}
	private void BtnCreate_click(object sender, Avalonia.Interactivity.RoutedEventArgs e) {
		if (ProjPath.Text == null || ProjPath.Text.Trim() == string.Empty) MsBox.Avalonia.MessageBoxManager.GetMessageBoxStandard("error", "select directory!!!").ShowAsync();
		else createProject();
	}
	private void createProject() {
		var workdir = Directory.CreateDirectory(ProjPath.Text + "/" + Props.Title);
		ProcInvoker.Run("dotnet", $"new sln -n {Props.Title} -o {workdir.FullName}");
		createCLI(Props.Title, Props.Type.Replace(' ', '.').ToLower(), workdir.FullName + "/" + Props.Title);
		if (Props.hasTest) {
			createCLI($"Tests.{Props.Title}", "xunit", $"{workdir.FullName}/Tests");
			ProcInvoker.Run("dotnet", $"add {workdir.FullName}/{Props.Title}/{Props.Title}.csproj reference {workdir.FullName}/Tests/Tests.{Props.Title}.csproj");
		}
		scriptCreate(workdir.FullName);
			Close();
	}
	private void createCLI(string name, string type, string folder) {
		ProcInvoker.Run("dotnet", $" new {type} -n {name} -o {folder}");
		ProcInvoker.Run("dotnet", $"sln {ProjPath.Text}/{Props.Title}/{Props.Title}.sln add {folder}/{name}.csproj");
	}
	private void scriptCreate(string path) {
		if ((bool)gitCheck.IsChecked) ProcInvoker.Run("dotnet", $" new gitignore -o {path}");
		if ((bool)readmeCheck.IsChecked) File.WriteAllText($"{path}/readMe.md",$"## Hello {Props.Title}" );
	}
}
