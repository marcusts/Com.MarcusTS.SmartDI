//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Reflection;

[assembly: System.Reflection.AssemblyCompanyAttribute("Marcus Technical Services, Inc.")]
[assembly: System.Reflection.AssemblyConfigurationAttribute("Debug")]
[assembly: System.Reflection.AssemblyCopyrightAttribute("@2020 Marcus Technical Services, Inc.")]
[assembly: System.Reflection.AssemblyDescriptionAttribute(@"
         A container that creates and then (optionally) stores variables to provide caching and centralized access.

         These sorts of containers are sometimes misdescribed as IOC (""Inversion of Control"") Containers. Since they do not provide any control over program flow, the accurate term is DI (""Dependency Injection"") Container.

         The SmartDI Container is unique in that it:

         * Does not store instantiated objects unnecessarily.

         * Supports object life-cycle management. When an object dies, it is removed from the container.  This requires you to implement an interface.

         * Indexes a shared container class instance to the objects that share it so that when those objects die, the container class instance is also removed.  This does not requires any other interfaces or management steps.


      ")]
[assembly: System.Reflection.AssemblyFileVersionAttribute("1.0.18.0")]
[assembly: System.Reflection.AssemblyInformationalVersionAttribute("1.0.18")]
[assembly: System.Reflection.AssemblyProductAttribute("Smart DI Container")]
[assembly: System.Reflection.AssemblyTitleAttribute("Com.MarcusTS.SmartDI")]
[assembly: System.Reflection.AssemblyVersionAttribute("1.0.18.0")]

// Generated by the MSBuild WriteCodeFragment class.

