using System;
using System.Collections.Generic;
using UnrealBuildTool;


namespace Conan
{
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

    public class ConanBuildInfo {
    
        private static Dictionary<string, ConanLibraryInfo> libraries;

        public static void Setup() 
        {
            if(libraries == null) {
                libraries = new Dictionary<string, ConanLibraryInfo>();
                {{libs_init}}
            }
        }

        public static ConanLibraryInfo GetLibraryInfo(string libname) 
        {
            return libraries[libname];
        }

        public static Dictionary<string, ConanLibraryInfo> GetAllLibraryInfo() 
        {
            return libraries;
        }

        private ConanBuildInfo() 
        {
        }
    }

    public static class ModuleRulesExtension
    {
        private static void LinkConanLibrary(ModuleRules rules, ConanLibraryInfo libraryInfo)
        {
            rules.PublicSystemIncludePaths.AddRange(libraryInfo.IncludePaths);
#if UE_4_21_OR_LATER
            rules.PublicRuntimeLibraryPaths.AddRange(libraryInfo.BinPaths);
#endif
            rules.PublicLibraryPaths.AddRange(libraryInfo.LibPaths);
            rules.PublicAdditionalLibraries.AddRange(libraryInfo.Libraries);
            rules.PublicDefinitions.AddRange(libraryInfo.Definitions);
        }

        public static void LinkConanLibrary(this ModuleRules rules, string libraryName) 
        {
            ConanLibraryInfo libraryInfo = ConanBuildInfo.GetLibraryInfo(libraryName);
            LinkConanLibrary(rules, libraryInfo);
        }

        public static void LinkConanLibraries(this ModuleRules rules, string[] libraryNames)
        {
            foreach (string name in libraryNames) {
                LinkConanLibrary(rules, name);
            }
        }

        public static void LinkConanLibraries(this ModuleRules rules, List<string> libraryNames) 
        {
            foreach (string name in libraryNames) {
                LinkConanLibrary(rules, name);
            }
        }

        public static void LinkAllConanLibraries(this ModuleRules rules) {
            foreach (KeyValuePair<string, ConanLibraryInfo> item in ConanBuildInfo.GetAllLibraryInfo()) {
                LinkConanLibrary(rules, item.Value);
            }
        }
    }

    public static class TargetRulesExtension
    {
        private static void LinkConanLibrary(TargetRules rules, ConanLibraryInfo libraryInfo)
        {
            rules.AdditionalCompilerArguments += String.Join(" ", libraryInfo.CppFlags);
            rules.AdditionalLinkerArguments += String.Join(" ", libraryInfo.LinkFlags) + String.Join(" ", libraryInfo.SharedLinkFlags);
        }

        public static void LinkConanLibrary(this TargetRules rules, string libraryName) 
        {
            ConanLibraryInfo libraryInfo = ConanBuildInfo.GetLibraryInfo(libraryName);
            LinkConanLibrary(rules, libraryInfo);
        }

        public static void LinkConanLibraries(this TargetRules rules, string[] libraryNames)
        {
            foreach (string name in libraryNames) {
                LinkConanLibrary(rules, name);
            }
        }

        public static void LinkConanLibraries(this TargetRules rules, List<string> libraryNames) 
        {
            foreach (string name in libraryNames) {
                LinkConanLibrary(rules, name);
            }
        }

        public static void LinkAllConanLibraries(this TargetRules rules) {
            foreach (KeyValuePair<string, ConanLibraryInfo> item in ConanBuildInfo.GetAllLibraryInfo()) {
                LinkConanLibrary(rules, item.Value);
            }
        }
    }
}
