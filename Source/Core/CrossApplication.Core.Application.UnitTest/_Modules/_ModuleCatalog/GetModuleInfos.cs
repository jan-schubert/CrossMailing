﻿using System.Linq;
using CrossApplication.Core.Application.Modules;
using CrossApplication.Core.Application.UnitTest._Modules.TestClasses;
using CrossApplication.Core.Contracts.Application.Modules;
using FluentAssertions;
using Xunit;

namespace CrossApplication.Core.Application.UnitTest._Modules._ModuleCatalog
{
    public class GetModuleInfos
    {
        [Fact]
        public void ShouldReturnAddedModuleInfos()
        {
            var catalog = new ModuleCatalog();
            catalog.AddModuleInfo(new ModuleInfo {ModuleType = typeof(ModuleA), Name = "ModuleA"});
            catalog.AddModuleInfo(new ModuleInfo {ModuleType = typeof(ModuleB), Name = "ModuleB"});
            catalog.AddModuleInfo(new ModuleInfo {ModuleType = typeof(ModuleC), Name = "ModuleC"});

            var moduleInfos = catalog.GetModuleInfos().ToArray();

            moduleInfos.Length.Should().Be(3);
            moduleInfos[0].Name.Should().Be("ModuleA");
            moduleInfos[0].ModuleType.Should().Be(typeof(ModuleA));
            moduleInfos[1].Name.Should().Be("ModuleB");
            moduleInfos[1].ModuleType.Should().Be(typeof(ModuleB));
            moduleInfos[2].Name.Should().Be("ModuleC");
            moduleInfos[2].ModuleType.Should().Be(typeof(ModuleC));
        }
    }
}