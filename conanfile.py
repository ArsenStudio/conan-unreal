import os
from conans import ConanFile, tools, load
from conans.model import Generator
from conans.client.generators import registered_generators

def transform_name(name):
    return name.replace("_", " ").replace("-", " ").title().replace(" ", "")

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

    root_dir = "Plugins/ConanVendor"

    @property
    def filename(self):
        pass

    def generate_module_file(self, name, deps_cpp_info):
        template = load(os.path.join(os.path.dirname(__file__), "templates", "Build.cs"))
        template = template.replace("{name}", name)
        template = template.replace("{include_paths}", ",\n".join('"%s"' % p.replace("\\", "/") for p in deps_cpp_info.include_paths))
        template = template.replace("{bin_paths}", ",\n".join('"%s"' % p.replace("\\", "/") for p in deps_cpp_info.bin_paths))
        template = template.replace("{lib_paths}", ",\n".join('"%s"' % p.replace("\\", "/") for p in deps_cpp_info.lib_paths))
        template = template.replace("{libs}", ", ".join('"%s.lib"' % p for p in deps_cpp_info.libs))
        template = template.replace("{definitions}", ", ".join('"%s"' % p for p in deps_cpp_info.defines))

        return template

    @property
    def content(self):
        files = {}


        for package in self.deps_build_info.deps:
            package_name = transform_name(package)
            package_dir = os.path.join(self.root_dir, package_name)

            modules = []

            if len(self.deps_build_info[package].components) > 0:
                for component in self.deps_build_info[package].components:
                    component_name = transform_name(component)
                    component_dir = os.path.join(package_dir, component_name)

                    files[os.path.join(component_dir, component_name + ".Build.cs")] = self.generate_module_file(component_name, self.deps_build_info[package].components)

                    modules.append(component_name)

            else:
                component_name = package_name
                component_dir = os.path.join(package_dir, component_name)

                files[os.path.join(component_dir, component_name + ".Build.cs")] = self.generate_module_file(component_name, self.deps_build_info[package])

                modules.append(component_name)

            
            file_content = load(os.path.join(os.path.dirname(__file__), "templates", "Plugin.uplugin")).replace("{name}", package_name).replace("{version}", self.deps_build_info[package].version)

            files[os.path.join(package_dir, package_name + ".uplugin")] = file_content

        return files

        # template = ('ConanLibraryInfo {dep}Info = new ConanLibraryInfo("{dep}");\n'
        #             '{dep}Info.IncludePaths.AddRange(new string[] {{{deps.include_paths}}});\n'
        #             '{dep}Info.LibPaths.AddRange(new string[] {{{deps.lib_paths}}});\n'
        #             '{dep}Info.BinPaths.AddRange(new string[] {{{deps.bin_paths}}});\n'
        #             '{dep}Info.Libraries.AddRange(new string[] {{{deps.libs}}});\n'
        #             '{dep}Info.Definitions.AddRange(new string[] {{{deps.defines}}});\n'
        #             '{dep}Info.CppFlags.AddRange(new string[] {{{deps.cppflags}}});\n'
        #             '{dep}Info.CFlags.AddRange(new string[] {{{deps.cflags}}});\n'
        #             '{dep}Info.SharedLinkFlags.AddRange(new string[] {{{deps.sharedlinkflags}}});\n'
        #             '{dep}Info.LinkFlags.AddRange(new string[] {{{deps.exelinkflags}}});\n'
        #             'libraries.Add("{dep}", {dep}Info);\n')

        # sections = []

        # for dep_name, dep_cpp_info in self.deps_build_info.dependencies:
        #     deps = UnrealDeps(dep_cpp_info)
        #     dep_name = dep_name.replace("-", "_")
        #     dep_flags = template.format(dep=dep_name, deps=deps)
        #     sections.append(dep_flags)

        # file_content = load("templates/Build.cs")
        # return file_content.replace("{{libs_init}}", "\n".join(sections))

registered_generators.add("unreal", Unreal)

class UnrealGeneratorPackage(ConanFile):
    name = "UnrealGen"
    version = "2.0"
    url = "https://github.com/FrozenStormInteractive/conan-unreal-generator"
    license = "MIT"
    exports = ["templates/*"]
