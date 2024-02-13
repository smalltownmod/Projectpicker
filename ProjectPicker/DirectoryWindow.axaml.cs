using Avalonia.Controls;
using Avalonia.VisualTree;
using ProjectPicker.models;
using System.Collections.Generic;
using System.IO;

namespace ProjectPicker;

public partial class DirectoryWindow : Window {
	public Properties Props { get; set; }
	private List<string> targets {  get; set; }
	public DirectoryWindow() {
		InitializeComponent();
		targets = new List<string>() {"win-x64", "win-x86", "win-arm64", "linux-x64", "linux-arm", "linux-arm64", "osx-x64", "osx-arm64",
		"ios-arm64", "android-arm64"};
		CBTarget.ItemsSource = targets;
		CBTarget.SelectedIndex = 0;
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
			ProcInvoker.Run("dotnet", $"add {workdir.FullName}/Tests/Tests.{Props.Title}.csproj reference {workdir.FullName}/{Props.Title}/{Props.Title}.csproj");
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
		ScriptModel sm = new ScriptModel();
		string sf = (bool)ChSF.IsChecked ? "true" : "false";
		File.WriteAllText(path + $"/publish.ps1", sm.PublishScript(Props.Title, sf, CBTarget.SelectedItem.ToString()));
	}
}
