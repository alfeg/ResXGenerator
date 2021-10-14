# ResXFileCodeGenerator
ResX Designer Source Generator. Generates strongly-typed resource classes for looking up localized strings.

## Usage

Install the `VocaDb.ResXFileCodeGenerator` package:

```psl
dotnet add package VocaDb.ResXFileCodeGenerator
```

Generated source from [ActivityEntrySortRuleNames.resx](https://github.com/VocaDB/vocadb/blob/6ac18dd2981f82100c8c99566537e4916920219e/VocaDbWeb.Resources/App_GlobalResources/ActivityEntrySortRuleNames.resx):

```cs
// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
#nullable enable
namespace Resources
{
    using System.Globalization;
    using System.Resources;

    public static class ActivityEntrySortRuleNames
    {
        private static ResourceManager? s_resourceManager;
        public static ResourceManager ResourceManager => s_resourceManager ??= new ResourceManager("VocaDb.Web.App_GlobalResources.ActivityEntrySortRuleNames", typeof(ActivityEntrySortRuleNames).Assembly);
        public static CultureInfo? CultureInfo { get; set; }

        /// <summary>
        /// Looks up a localized string similar to Oldest.
        /// </summary>
        public static string CreateDate => ResourceManager.GetString(nameof(CreateDate), CultureInfo)!;
        /// <summary>
        /// Looks up a localized string similar to Newest.
        /// </summary>
        public static string CreateDateDescending => ResourceManager.GetString(nameof(CreateDateDescending), CultureInfo)!;
    }
}
```

## References
- [Introducing C# Source Generators | .NET Blog](https://devblogs.microsoft.com/dotnet/introducing-c-source-generators/)
- [microsoft/CsWin32: A source generator to add a user-defined set of Win32 P/Invoke methods and supporting types to a C# project.](https://github.com/microsoft/cswin32)
- [kenkendk/mdresxfilecodegenerator: Resx Designer Generator](https://github.com/kenkendk/mdresxfilecodegenerator)
- [dotnet/ResXResourceManager: Manage localization of all ResX-Based resources in one central place.](https://github.com/dotnet/ResXResourceManager)
- [roslyn/source-generators.cookbook.md at master · dotnet/roslyn](https://github.com/dotnet/roslyn/blob/master/docs/features/source-generators.cookbook.md)
- [roslyn/Using Additional Files.md at master · dotnet/roslyn](https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Using%20Additional%20Files.md)
- [ufcpp - YouTube](https://www.youtube.com/channel/UCY-z_9mau6X-Vr4gk2aWtMQ)
