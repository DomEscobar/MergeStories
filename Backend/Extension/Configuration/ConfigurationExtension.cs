using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WebApi.Extensions.Configuration
{
  public static class ConfigurationExtension
  {
    /// <summary>
    /// The connection string
    /// </summary>
    /// <param name="config"></param>
    /// <returns></returns>
    public static string ConnectionString(this IConfiguration config)
    {
      return config.GetValue<string>("ConnectionString");
    }

    /// <summary>
    /// If no connection is set we fall back to in memory. This returns true if that is the case.
    /// </summary>
    /// <param name="config"></param>
    /// <returns></returns>
    public static bool IsInMemoryFallback(this IConfiguration config)
    {
      return !config.MicrosoftSql().Exists() && string.IsNullOrEmpty(config.ConnectionString());
    }

    /// <summary>
    /// Returns the in memory configuration <see cref="IConfigurationSection"/>
    /// </summary>
    /// <param name="config"></param>
    /// <returns></returns>
    public static IConfigurationSection InMemory(this IConfiguration config)
    {
      return config.GetSection("InMemory");
    }

    /// <summary>
    /// Return the Microsoft SQL-Server <see cref="IConfigurationSection"/>
    /// </summary>
    /// <param name="config"></param>
    /// <returns></returns>
    public static IConfigurationSection MicrosoftSql(this IConfiguration config)
    {
      return config.GetSection("MicrosoftSqlServer");
    }

    /// <summary>
    /// Returns the enabled value
    /// </summary>
    /// <param name="section"></param>
    /// <returns></returns>
    public static bool IsEnabled(this IConfigurationSection section)
    {
      return section.GetValue<bool>("Enabled");
    }

    /// <summary>
    /// Returns the server value
    /// </summary>
    /// <param name="section"></param>
    /// <returns></returns>
    public static string Server(this IConfigurationSection section)
    {
      return section.GetValue<string>("Server");
    }

    /// <summary>
    /// Returns the database value
    /// </summary>
    /// <param name="section"></param>
    /// <returns></returns>
    public static string Database(this IConfigurationSection section)
    {
      return section.GetValue<string>("Database");
    }

    /// <summary>
    /// Returns the user value
    /// </summary>
    /// <param name="section"></param>
    /// <returns></returns>
    public static string User(this IConfigurationSection section)
    {
      return section.GetValue<string>("User");
    }

    /// <summary>
    /// Returns the password value
    /// </summary>
    /// <param name="section"></param>
    /// <returns></returns>
    public static string Password(this IConfigurationSection section)
    {
      return section.GetValue<string>("Password");
    }

    /// <summary>
    /// Returns the integrated security value
    /// </summary>
    /// <param name="section"></param>
    /// <returns></returns>
    public static bool IsIntegratedSecurity(this IConfigurationSection section)
    {
      return section.GetValue<bool>("IntegratedSecurity");
    }

    /// <summary>
    /// Determines if passwords should be verified or not
    /// </summary>
    /// <param name="config"></param>
    /// <returns></returns>
    public static bool IsBypassPasswords(this IConfiguration config)
    {
      return config.GetValue<bool>("BypassPasswords");
    }

    /// <summary>
    /// Returns the know proxy networks
    /// </summary>
    /// <param name="config"></param>
    /// <returns></returns>
    public static IEnumerable<IPNetwork> GetProxyNetworks(this IConfiguration config)
    {
      string proxies = config.GetValue<string>("proxynetworks");
      if (string.IsNullOrEmpty(proxies))
      {
        return Enumerable.Empty<IPNetwork>();
      }

      return proxies.Split(';').Select(s =>
      {
        string[] split = s.Split('/');
        int prefix = 8;
        if (split.Length > 1)
        {
          prefix = int.Parse(split[1]);
        }

        return new IPNetwork(IPAddress.Parse(split[0]), prefix);
      });
    }
  }
}
