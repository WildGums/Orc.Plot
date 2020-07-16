// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssemblyInfo.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using System.Reflection;
using System.Resources;
using System.Windows;
using System.Windows.Markup;

// All other assembly info is defined in SolutionAssemblyInfo.cs

[assembly: AssemblyTitle("Orc.Plot")]
[assembly: AssemblyProduct("Orc.Plot")]
[assembly: AssemblyDescription("Orc.Plot library")]
[assembly: NeutralResourcesLanguage("en-US")]

[assembly: XmlnsPrefix("http://schemas.wildgums.com/orc/plot", "orcplot")]
[assembly: XmlnsDefinition("http://schemas.wildgums.com/orc/plot", "Orc.Plot")]
//[assembly: XmlnsDefinition("http://schemas.wildgums.com/orc/plot", "Orc.Plot.Behaviors")]
//[assembly: XmlnsDefinition("http://schemas.wildgums.com/orc/plot", "Orc.Plot.Controls")]
//[assembly: XmlnsDefinition("http://schemas.wildgums.com/orc/plot", "Orc.Plot.Fonts")]
//[assembly: XmlnsDefinition("http://schemas.wildgums.com/orc/plot", "Orc.Plot.Markup")]
//[assembly: XmlnsDefinition("http://schemas.wildgums.com/orc/plot", "Orc.Plot.Views")]
//[assembly: XmlnsDefinition("http://schemas.wildgums.com/orc/plot", "Orc.Plot.Windows")]

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
    //(used if a resource is not found in the page, 
    // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
    //(used if a resource is not found in the page, 
    // app, or any theme specific resource dictionaries)
    )]
