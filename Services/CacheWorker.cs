﻿using CourseManagement.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace CourseManagement.Services
{
    public class CacheWorker : BackgroundService
    {
        private readonly TimeSpan _updateInterval = TimeSpan.FromHours(3);

        private readonly ILogger<CacheWorker> _logger;

        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly IMemoryCache _cache;

        private bool _isCacheInitialized = false;

        public CacheWorker(ILogger<CacheWorker> logger, IMemoryCache cache, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _cache = cache;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await AuthorService._cacheSignal.WaitAsync();
            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (IServiceScope scope = _serviceScopeFactory.CreateScope())
                    {
                        AppDbContext _context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                        Author[]? authors = (from a in _context.Authors select a).ToArray();
                        if (authors is { Length: > 0 })
                        {
                            _cache.Set("AllAuthors", authors);
                            _logger.LogInformation("Cache updated with {Count:#,#} authors.", authors.Length);
                        }
                        else
                        {
                            _logger.LogWarning("Unable to fetch photos to update cache.");
                        }
                    }
                }
                finally
                {
                    if (!_isCacheInitialized)
                    {
                        AuthorService._cacheSignal.Release();
                        _isCacheInitialized = true;
                    }
                }

                try
                {
                    _logger.LogInformation("Will attempt to update the cache in {Hours} hours from now.", _updateInterval.Hours);
                    await Task.Delay(_updateInterval, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    _logger.LogWarning("Cancellation acknowledged: shutting down.");
                    break;
                }
            }
        }
    }
}
