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

        // TODO: Use the full path in PublicAdditionalLibraries. Solution: loop against libs and paths and check if full library path exists.
#if UE_4_24_OR_LATER
        PublicSystemLibraryPaths.AddRange(new string[] {
#else
        PublicLibraryPaths.AddRange(new string[] {
#endif
            {lib_paths}
        });
#if UE_4_24_OR_LATER
        PublicSystemLibraries.AddRange(new string[] {
#else
        PublicAdditionalLibraries.AddRange(new string[] {
#endif
            {libs}
        });

#if UE_4_19_OR_LATER
        PublicDefinitions.AddRange(new string[] {
#else
        Definitions.AddRange(new string[] {
#endif
            {definitions}
        }); 
    }

    public static void Link(TargetRules rules)
    {
        rules.AdditionalCompilerArguments += "{cppflags}";
        rules.AdditionalLinkerArguments += "{sharedlinkflags} {exelinkflags}";
    }
}
