﻿using System;
using System.Collections.Generic;
using CrossApplication.Core.Application.Modules;
using CrossApplication.Core.Application.UnitTest._Modules.TestClasses;
using CrossApplication.Core.Contracts.Application.Modules;
using FluentAssertions;
using Microsoft.Practices.Unity;
using Moq;
using Xunit;

namespace CrossApplication.Core.Application.UnitTest._Modules._ModuleManager
{
    public class InitializeAsync
    {
        [Fact]
        public async void ShouldInitializeAllModules()
        {
            var moduleCatalogMock = new Mock<IModuleCatalog>();
            moduleCatalogMock.Setup(x => x.GetModuleInfos()).Returns(new List<ModuleInfo>
            {
                new ModuleInfo {Name = "ModuleA", ModuleType = typeof(ModuleA)},
                new ModuleInfo {Name = "ModuleB", ModuleType = typeof(ModuleB), Tag = ModuleTags.Infrastructure},
                new ModuleInfo {Name = "ModuleC", ModuleType = typeof(ModuleC)}
            });

            var unityContainerMock = new Mock<IUnityContainer>();
            unityContainerMock.Setup(x => x.Resolve(It.Is<Type>(y => y == typeof(ModuleA)), It.IsAny<string>(), It.IsAny<ResolverOverride[]>())).Returns(new ModuleA());
            unityContainerMock.Setup(x => x.Resolve(It.Is<Type>(y => y == typeof(ModuleB)), It.IsAny<string>(), It.IsAny<ResolverOverride[]>())).Returns(new ModuleB());
            unityContainerMock.Setup(x => x.Resolve(It.Is<Type>(y => y == typeof(ModuleC)), It.IsAny<string>(), It.IsAny<ResolverOverride[]>())).Returns(new ModuleC());
            var moduleManager = new ModuleManager(unityContainerMock.Object);
            moduleManager.SetModuleCatalog(moduleCatalogMock.Object);

            await moduleManager.InizializeAsync();

            unityContainerMock.Verify(x => x.Resolve(It.Is<Type>(y => y == typeof(ModuleA)), It.IsAny<string>(), It.IsAny<ResolverOverride[]>()));
            unityContainerMock.Verify(x => x.Resolve(It.Is<Type>(y => y == typeof(ModuleB)), It.IsAny<string>(), It.IsAny<ResolverOverride[]>()));
            unityContainerMock.Verify(x => x.Resolve(It.Is<Type>(y => y == typeof(ModuleC)), It.IsAny<string>(), It.IsAny<ResolverOverride[]>()));
        }

        [Fact]
        public async void ShouldInitializeAllModulesWithDefaultTagModule()
        {
            var moduleCatalogMock = new Mock<IModuleCatalog>();
            moduleCatalogMock.Setup(x => x.GetModuleInfos()).Returns(new List<ModuleInfo>
            {
                new ModuleInfo {Name = "ModuleA", ModuleType = typeof(ModuleA)},
                new ModuleInfo {Name = "ModuleB", ModuleType = typeof(ModuleB), Tag = "NotLoadedModule"},
                new ModuleInfo {Name = "ModuleC", ModuleType = typeof(ModuleC)}
            });

            var unityContainerMock = new Mock<IUnityContainer>();
            unityContainerMock.Setup(x => x.Resolve(It.Is<Type>(y => y == typeof(ModuleA)), It.IsAny<string>(), It.IsAny<ResolverOverride[]>())).Returns(new ModuleA());
            unityContainerMock.Setup(x => x.Resolve(It.Is<Type>(y => y == typeof(ModuleC)), It.IsAny<string>(), It.IsAny<ResolverOverride[]>())).Returns(new ModuleC());
            var moduleManager = new ModuleManager(unityContainerMock.Object);
            moduleManager.SetModuleCatalog(moduleCatalogMock.Object);

            await moduleManager.InizializeAsync(ModuleTags.DefaultModule);

            unityContainerMock.Verify(x => x.Resolve(It.Is<Type>(y => y == typeof(ModuleA)), It.IsAny<string>(), It.IsAny<ResolverOverride[]>()));
            unityContainerMock.Verify(x => x.Resolve(It.Is<Type>(y => y == typeof(ModuleC)), It.IsAny<string>(), It.IsAny<ResolverOverride[]>()));
        }

        [Fact]
        public async void ShouldInitializeAllModulesWithDefaultTagInfrastructure()
        {
            var moduleCatalogMock = new Mock<IModuleCatalog>();
            moduleCatalogMock.Setup(x => x.GetModuleInfos()).Returns(new List<ModuleInfo>
            {
                new ModuleInfo {Name = "ModuleA", ModuleType = typeof(ModuleA), Tag = "Infrastructure"},
                new ModuleInfo {Name = "ModuleB", ModuleType = typeof(ModuleB)},
                new ModuleInfo {Name = "ModuleC", ModuleType = typeof(ModuleC), Tag = "Infrastructure"}
            });

            var unityContainerMock = new Mock<IUnityContainer>();
            unityContainerMock.Setup(x => x.Resolve(It.Is<Type>(y => y == typeof(ModuleA)), It.IsAny<string>(), It.IsAny<ResolverOverride[]>())).Returns(new ModuleA());
            unityContainerMock.Setup(x => x.Resolve(It.Is<Type>(y => y == typeof(ModuleC)), It.IsAny<string>(), It.IsAny<ResolverOverride[]>())).Returns(new ModuleC());
            var moduleManager = new ModuleManager(unityContainerMock.Object);
            moduleManager.SetModuleCatalog(moduleCatalogMock.Object);

            await moduleManager.InizializeAsync("Infrastructure");

            unityContainerMock.Verify(x => x.Resolve(It.Is<Type>(y => y == typeof(ModuleA)), It.IsAny<string>(), It.IsAny<ResolverOverride[]>()));
            unityContainerMock.Verify(x => x.Resolve(It.Is<Type>(y => y == typeof(ModuleC)), It.IsAny<string>(), It.IsAny<ResolverOverride[]>()));
        }

        [Fact]
        public void ShouldThrowExceptionIfModuleNotInheritsFromIModule()
        {
            var moduleCatalogMock = new Mock<IModuleCatalog>();
            moduleCatalogMock.Setup(x => x.GetModuleInfos()).Returns(new List<ModuleInfo> { new ModuleInfo {Name = "ModuleWithoutIModule", ModuleType = typeof(ModuleWithoutIModule)}});
            var unityContainerMock = new Mock<IUnityContainer>();
            unityContainerMock.Setup(x => x.Resolve(It.Is<Type>(y => y == typeof(ModuleWithoutIModule)), It.IsAny<string>(), It.IsAny<ResolverOverride[]>())).Returns(new ModuleWithoutIModule());
            var moduleManager = new ModuleManager(unityContainerMock.Object);
            moduleManager.SetModuleCatalog(moduleCatalogMock.Object);

            new Action(() => moduleManager.InizializeAsync().Wait())
                .ShouldThrow<ArgumentException>().WithMessage("CrossApplication.Core.Application.UnitTest._Modules.TestClasses.ModuleWithoutIModule does not inherit from IModule.");
        }
    }
}