using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPicker.models {
	public class ScriptModel {
	
	 public ScriptModel() {
	}
		public string PublishScript(string proj, string sf, string target) {
			string pbSkript = $"$singlefile=\"-p:PublishSingleFile={sf}\"\n" +
				$"$cmd =\"dotnet publish {proj}/{proj}.csproj -o ./build/{proj} --sc -r {target} $singlefile -c Release \"\n" +
				$"Invoke-Expression $cmd;\n";

			return pbSkript;
		}
	}
}
