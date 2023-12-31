﻿using Core.Application.Pipelines.Caching;
using Core.Application.Pipelines.Logging;
using Core.Application.Pipelines.Transaction;
using Core.Application.Pipelines.Validation;
using Core.Application.Rules;
using Core.CrossCuttingConcerns.Serilog;
using Core.CrossCuttingConcerns.Serilog.Logger;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddSubClassesOfType(Assembly.GetExecutingAssembly(), typeof(BaseBusinessRules));

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

                configuration.AddOpenBehavior(typeof(RequestValidationBehavior<,>)); //çalışırken gelip buraya bakacak

                configuration.AddOpenBehavior(typeof(TransactionScopeBehavior<,>)); //çalışırken gelip buraya bakacak
                configuration.AddOpenBehavior(typeof(CachingBehavior<,>)); //çalışırken gelip buraya bakacak
                configuration.AddOpenBehavior(typeof(CacheRemovingBehavior<,>)); //çalışırken gelip buraya bakacak
                configuration.AddOpenBehavior(typeof(LoggingBehavior<,>)); //çalışırken gelip buraya bakacak
            });
            services.AddSingleton<LoggerServiceBase, MsSqlLogger>();
            return services;
        }
        //Ayağa kalkınca direkt burayı çalıştır ki kurallara göre hareket etsin
        public static IServiceCollection AddSubClassesOfType(
     this IServiceCollection services,
     Assembly assembly,
     Type type,
     Func<IServiceCollection, Type, IServiceCollection>? addWithLifeCycle = null
 )
        {
            var types = assembly.GetTypes().Where(t => t.IsSubclassOf(type) && type != t).ToList();
            foreach (var item in types)
                if (addWithLifeCycle == null)
                    services.AddScoped(item);

                else
                    addWithLifeCycle(services, type);
            return services;
        }
    }
}
