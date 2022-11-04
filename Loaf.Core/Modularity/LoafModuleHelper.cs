﻿ 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Loaf.Core.Modularity;

public static class LoafModuleHelper
{
    private static readonly List<LoafModule> Modules = new ();

    public static IServiceCollection AddModule<TModule>(this IServiceCollection services, IConfiguration configuration = null)
        where TModule : LoafModule
    {
        LoadModules(services, typeof(TModule));
        InvokeConfigureService(services, configuration);
        return services;
    }

    /// <summary>
    /// 调用模块的ConfigureService
    /// </summary>
    private static void InvokeConfigureService(IServiceCollection services, IConfiguration configuration = null)
    {
        foreach (var preConfigureService in Modules.Select(m => (Action<ServiceConfigurationContext>)m.PreConfigureService))
            preConfigureService(new(services,configuration));
        foreach (var configureService in Modules.Select(m => (Action<ServiceConfigurationContext>)m.ConfigureService))
            configureService(new(services,configuration));
        foreach (var postConfigureService in Modules.Select(m => (Action<ServiceConfigurationContext>)m.PostConfigureService))
            postConfigureService(new(services,configuration));
    }

    /// <summary>
    /// 读到列表中备用
    /// </summary>
    private static IServiceCollection LoadModules(IServiceCollection services, Type moduleType)
    {
        var module = (Activator.CreateInstance(moduleType) as LoafModule)!;
        Modules.Add(module);

        if (moduleType.IsDefined(typeof(DependsOnAttribute), false))
        {
            var dependsAttribute = moduleType.GetCustomAttribute<DependsOnAttribute>()!;
            foreach (var dependModuleType in dependsAttribute.ModuleTypes)
            {
                LoadModules(services, dependModuleType);
            }
        }
        return services;
    }
}