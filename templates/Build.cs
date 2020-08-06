using System;
using System.Collections.Generic;
using UnrealBuildTool;

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
        rules.AdditionalCompilerArguments += "{cppflags}";
        rules.AdditionalLinkerArguments += "{sharedlinkflags} {exelinkflags}";
    }
}
