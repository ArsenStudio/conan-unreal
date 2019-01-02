from conans import ConanFile, tools
from conans.model import Generator

class UnrealDeps(object):

    def __init__(self, deps_cpp_info):
        self.include_paths = ",\n".join('"%s"' % p.replace("\\", "/")
                                        for p in deps_cpp_info.include_paths)
        self.lib_paths = ",\n".join('"%s"' % p.replace("\\", "/")
                                    for p in deps_cpp_info.lib_paths)
        self.bin_paths = ",\n".join('"%s"' % p.replace("\\", "/")
                                    for p in deps_cpp_info.bin_paths)
        self.libs = ", ".join('"%s.lib"' % p for p in deps_cpp_info.libs)
        self.defines = ", ".join('"%s"' % p for p in deps_cpp_info.defines)
        self.cppflags = ", ".join('"%s"' % p for p in deps_cpp_info.cppflags)
        self.cflags = ", ".join('"%s"' % p for p in deps_cpp_info.cflags)
        self.sharedlinkflags = ", ".join('"%s"' % p for p in deps_cpp_info.sharedlinkflags)
        self.exelinkflags = ", ".join('"%s"' % p for p in deps_cpp_info.exelinkflags)

        self.rootpath = "%s" % deps_cpp_info.rootpath.replace("\\", "/")

class Unreal(Generator):
    @property
    def filename(self):
        return "Source/ConanBuildInfo/ConanInfo.Build.cs"

    @property
    def content(self):
        template = ('ConanLibInfo {dep}Info = new ConanLibInfo("{dep}");\n'
                    '{dep}Info.IncludePaths.AddRange(new string[] {{{deps.include_paths}}});\n'
                    '{dep}Info.LibPaths.AddRange(new string[] {{{deps.lib_paths}}});\n'
                    '{dep}Info.BinPaths.AddRange(new string[] {{{deps.bin_paths}}});\n'
                    '{dep}Info.Libraries.AddRange(new string[] {{{deps.libs}}});\n'
                    '{dep}Info.Definitions.AddRange(new string[] {{{deps.defines}}});\n'
                    '{dep}Info.CppFlags.AddRange(new string[] {{{deps.cppflags}}});\n'
                    '{dep}Info.CFlags.AddRange(new string[] {{{deps.cflags}}});\n'
                    '{dep}Info.SharedLinkFlags.AddRange(new string[] {{{deps.sharedlinkflags}}});\n'
                    '{dep}Info.LinkFlags.AddRange(new string[] {{{deps.exelinkflags}}});\n'
                    'libs.Add("{dep}", {dep}Info);\n')

        sections = []

        for dep_name, dep_cpp_info in self.deps_build_info.dependencies:
            deps = UnrealDeps(dep_cpp_info)
            dep_name = dep_name.replace("-", "_")
            dep_flags = template.format(dep=dep_name, deps=deps)
            sections.append(dep_flags)

        file_content = tools.load("Build.cs")
        return file_content.replace("{{libs_init}}", "\n".join(sections))

class UnrealGeneratorPackage(ConanFile):
    name = "UnrealGen"
    version = "1.0"
    url = "https://gitlab.com/ArsenStudio/Unreal/conan-unreal"
    license = "MIT"
    exports = "LICENSE"
    exports_sources = "Build.cs"

    def build(self):
        pass

    def package_info(self):
        self.cpp_info.includedirs = []
        self.cpp_info.libdirs = []
        self.cpp_info.bindirs = []
