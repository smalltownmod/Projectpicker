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
		if (ProjPath.Text == null || ProjPath.Text.Trim() == string.Empty) MsBox.Avalonia.MessageBoxManager.GetMessageBoxStandard("error", "select directory!!!");
		else createProject();
	}
	private void createProject() {
		var workdir = Directory.CreateDirectory(ProjPath.Text + "/" + Props.Title);
		ProcInvoker.Run("dotnet", $"new sln -n {Props.Title} -o {workdir.FullName}");
		if (Props.Type[0] == 'A') createCLI(Props.Title, Props.Type.Split(' ').First().ToLower() + "." + Props.Type.Split(' ').Last().ToLower(), workdir.FullName + "/" + Props.Title);
		else createCLI(Props.Title, Props.Type.ToLower(),workdir.FullName + "/" + Props.Title);
		if (Props.hasTest) {
			createCLI($"Tests.{Props.Title}", "xunit", $"{workdir.FullName}/Tests");
			ProcInvoker.Run("dotnet", $"add {workdir.FullName}/{Props.Title}/{Props.Title}.csproj reference {workdir.FullName}/Test/Tests.{Props.Title}.csproj");
		}
		Close();
	}
	private void createCLI(string name, string type, string folder) {
		ProcInvoker.Run("dotnet", $" new {type} -n {name} -o {folder}");
		ProcInvoker.Run("dotnet", $"sln {ProjPath.Text}/{Props.Title}/{Props.Title}.sln add {folder}/{name}.csproj");
	}
}