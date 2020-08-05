using System;
using System.Collections.Generic;
using UnrealBuildTool;


namespace Conan
{
    public class {name} : ModuleRules
    {
        public {name}(ReadOnlyTargetRules Target) : base(Target)
        {
            Type = ModuleType.External;

            PublicSystemIncludePaths.AddRange(new string[] {
                {include_paths}
            });
#if UE_4_21_OR_LATER
            PublicRuntimeLibraryPaths.AddRange(new string[] {
                {bin_paths}
            });
#endif
            PublicLibraryPaths.AddRange(new string[] {
                {lib_paths}
            });
            PublicAdditionalLibraries.AddRange(new string[] {
                {libs}
            });
            PublicDefinitions.AddRange(new string[] {
                {definitions}
            });
        }

        public static void Link(TargetRules rules)
        {
            rules.AdditionalCompilerArguments += String.Join(" ", libraryInfo.CppFlags);
            rules.AdditionalLinkerArguments += String.Join(" ", libraryInfo.LinkFlags) + String.Join(" ", libraryInfo.SharedLinkFlags);
        }
    }


    public class ConanLibraryInfo {

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

        public ConanLibraryInfo(string name) 
        {
            this.name = name;
        }

        public string GetName() 
        {
            return name;
        }
    }
}
