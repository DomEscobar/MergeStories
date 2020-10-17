using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using PublicTimeAPI.Models;
using PublicTimeAPI.Repository;
using System;
using System.Linq;
using WebApi.Extensions.Configuration;

namespace WebApi.Extensions.DbContext
{
  public static class DbContextExtension
  {
    /// <summary>
    /// Configure the options to use the in-memory-database
    /// </summary>
    /// <param name="builder">Context builder</param>
    /// <returns>returns <cref="DbContextOptionsBuilder"></returns>
    public static DbContextOptionsBuilder ConfigureInMemoryDatabase(this DbContextOptionsBuilder builder)
    {
      builder
        .UseInMemoryDatabase(databaseName: "EPublic")
        .UseInternalServiceProvider(
        new ServiceCollection()
        .AddEntityFrameworkInMemoryDatabase()
        .BuildServiceProvider())
        .EnableSensitiveDataLogging();

      return builder;
    }

    /// <summary>
    /// Configure the options to use the <paramref name="connectionString"/>
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="connectionString"></param>
    /// <returns></returns>
    public static DbContextOptionsBuilder ConfigureSqlServer(this DbContextOptionsBuilder builder, string connectionString)
    {
      return builder.UseSqlServer(connectionString);
    }

    /// <summary>
    /// Configure the options to use the <see cref="IConfigurationSection"/>
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="section"></param>
    /// <returns></returns>
    public static DbContextOptionsBuilder ConfigureSqlServer(this DbContextOptionsBuilder builder, IConfigurationSection section)
    {

      var str = $"Data Source={section.Server()}; Initial Catalog={section.Database()};MultipleActiveResultSets=True;User id={section.User()}; PWD={section.Password()}; Integrated Security={section.IsIntegratedSecurity()}";


      return builder.ConfigureSqlServer(str);
    }

    /// <summary>
    /// Configure the options using the <see cref="IConfiguration"/>
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="config"></param>
    public static void Configure(this DbContextOptionsBuilder builder, IConfiguration config)
    {
      if (!string.IsNullOrEmpty(config.ConnectionString()))
      {
        builder.ConfigureSqlServer(config.ConnectionString());
      }
      else if (config.MicrosoftSql().Exists())
      {
        builder.UseMySql(config.MicrosoftSql().Value, mySqlOptions => mySqlOptions
                    .ServerVersion(new Version(10, 3, 21), ServerType.MySql));
      }
      else
      {
        builder.ConfigureInMemoryDatabase();
      }
    }

    public static bool AllMigrationsApplied(this ApplicationDbContext context)
    {
      var applied = context.GetService<IHistoryRepository>()
          .GetAppliedMigrations()
          .Select(m => m.MigrationId);

      var total = context.GetService<IMigrationsAssembly>()
          .Migrations
          .Select(m => m.Key);

      return !total.Except(applied).Any();
    }

    public static void EnsureSeedData(this ApplicationDbContext context, bool checkForAppliedMigrations)
    {
      if (checkForAppliedMigrations && !context.AllMigrationsApplied())
      {
        return;
      }
    }
  }
}
