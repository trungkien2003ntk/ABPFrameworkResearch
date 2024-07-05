﻿using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyDemo.Bookstore.Data;
using Volo.Abp.DependencyInjection;

namespace MyDemo.Bookstore.EntityFrameworkCore;

public class EntityFrameworkCoreBookstoreDbSchemaMigrator
    : IBookstoreDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreBookstoreDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolve the BookstoreDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<BookstoreDbContext>()
            .Database
            .MigrateAsync();
    }
}
