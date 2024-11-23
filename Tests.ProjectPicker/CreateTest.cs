
using ProjectPicker;
using ProjectPicker.models;

namespace Tests.ProjectPicker {
	public class CreateTest {
		string Path = "C://src";
		string proj = "CreateTest";
		[Fact]
		public void Create() {
		string wordir = $"{Path}/{proj}";
			if (Directory.Exists(wordir)) Directory.Delete(wordir, true);
			Directory.CreateDirectory(wordir);
		}
		[Fact]
		public void SlnCreate() {
			//Environment.CurrentDirectory= Path + "/" + proj;
			var Exitcode = ProcInvoker.Run("dotnet", $"new sln -n {proj} -o {Path}/{proj}");
			Assert.True(Exitcode == 0);
		}

		[Fact]
		public void createAva() {
			createProject("Avalonia App", proj, proj);
		}
		[Fact]
		public void avaMVVM() {
			createProject("Avalonia MVVM", proj, proj);
		}

		[Fact]
		public void createWPF() {
			createProject("WPF", proj, proj);
		}
		[Fact]
		public void scribe() {
			File.WriteAllText($"{Path}/{proj}/publish.ps1", PublishSkript("true", "linux-x64"));
		}

		public void createProject(string type, string name, string dir) {
			//var Exitcode =	ProcInvoker.Run("dotnet", $" new {type.Split(' ').First().ToLower()}.{type.Split(' ').Last().ToLower()} -n {name} -o {Path}/{proj}/{dir} ");
			var Exitcode = ProcInvoker.Run("dotnet", $" new {type.Replace(' ', '.').ToLower()} -n {name} -o {Path}/{proj}/{dir}");
			Exitcode += ProcInvoker.Run("dotnet", $" sln {Path}/{proj}/{proj}.sln add {Path}/{proj}/{dir}/{name}.csproj");
			//for targeting .NET 7.0
			//var rep = $"{ Path }/{ proj}/{ dir}/{ name}.csproj";
			//File.WriteAllText(rep, File.ReadAllText(rep).Replace("net8.0", "net7.0"));
			Assert.True(Exitcode == 0);
		}

		public string PublishSkript(string sf, string target) {
			string pbSkript = $"$singlefile=\"-p:PublishSingleFile={sf}\"\n" +
				$"$cmd =\"dotnet publish -o ./build/{proj} --sc -r {target} $singlefile -c Release {proj}/{proj}.csproj\"\n" +
				$"Invoke-Expression $cmd;\n";

			return pbSkript;
		}
	}
}