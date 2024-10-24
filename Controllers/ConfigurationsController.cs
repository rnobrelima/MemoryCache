using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace MemoryCache.Controllers;

[ApiController]
[Route("[controller]")]
public class ConfigurationsController : ControllerBase
{
    private const string CacheKeyConfigurations = "cacheKeyConfiguracoes";
    private static readonly Dictionary<string, bool>? Configurations = new()
    {
        { "Config1", true },
        { "Config2", false },
    };
    
    private readonly IMemoryCache _memoryCache;

    public ConfigurationsController(ILogger<ConfigurationsController> logger, IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    [HttpGet(Name = "GetConfigurations")]
    public Dictionary<string, bool>? Get()
    {
        if (_memoryCache.TryGetValue(CacheKeyConfigurations, out Dictionary<string, bool>? configuration))
        {
            return configuration;
        }
        
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(30)) // Cache duration
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(60)); // Total lifetime

        // Save data in cache
        _memoryCache.Set(CacheKeyConfigurations, Configurations, cacheEntryOptions);
        return Configurations;
    }
}