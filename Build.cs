using System.Collections.Generic;
using UnrealBuildTool;

public class ConanLibInfo {

    private string name;

    public List<string> IncludePaths = new List<string>();

    public List<string> LibPaths = new List<string>();

    public List<string> BinPaths = new List<string>();

    public List<string> Libraries = new List<string>();

    public List<string> Definitions = new List<string>();

    public List<string> CppFlags = new List<string>();

    public List<string> CFlags = new List<string>();

    public List<string> SharedLinkFlags = new List<string>();

    public List<string> LinkFlags = new List<string>();

    public ConanLibInfo(string name) {
        this.name = name;
    }

    public string GetName() {
        return name;
    }
}

public class ConanBuildInfo {

    private ModuleRules rules;

    private static Dictionary<string, ConanLibInfo> libs;

    private static Dictionary<string, ConanLibInfo> libs;

    public static ConanBuildInfo SetupTarget(ModuleRules rules) {
        if(libs == null) {
            libs = new Dictionary<string, ConanLibInfo>();

            {{libs_init}}
        }

        return new ConanBuildInfo(rules);
    }

    private ConanBuildInfo(ModuleRules rules) {
        this.rules = rules;

        rules.ExternalDependencies.Add("ConanInfo.Build.cs");
    }

    public ConanLibInfo GetLibInfo(string libname) {
        return libs[libname];
    }

    private void SetupLibrary(ConanLibInfo libinfo) {
        rules.PublicSystemIncludePaths.AddRange(libinfo.IncludePaths);
        rules.PublicRuntimeLibraryPaths.AddRange(libinfo.BinPaths);
        rules.PublicLibraryPaths.AddRange(libinfo.LibPaths);
        rules.PublicAdditionalLibraries.AddRange(libinfo.Libraries);
        rules.PublicDefinitions.AddRange(libinfo.Definitions);

        rules.Target.AdditionalCompilerArguments.AddRange(libinfo.CppFlags);
        rules.Target.AdditionalLinkerArguments.AddRange(libinfo.LinkFlags);
        rules.Target.AdditionalLinkerArguments.AddRange(libinfo.SharedLinkFlags);
    }

    public void SetupLibrary(string libname) {
        ConanLibInfo libinfo = GetLibInfo(libname);

        SetupLibrary(libinfo);
    }

    public void SetupLibraries(string[] libnames) {
        foreach (string libname in libnames) {
            SetupLibrary(libname);
        }
    }

    public void SetupLibraries(List<string> libnames) {
        SetupLibraries(libnames.ToArray());
    }

    public void SetupAllLibraries() {
        foreach (KeyValuePair<string, ConanLibInfo> item in libs) {
            SetupLibrary(item.Value)
        }
    }
}