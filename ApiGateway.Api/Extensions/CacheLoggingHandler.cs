using System.Diagnostics;
using Microsoft.Extensions.Options;

namespace ApiGateway.Api.Extensions;

public sealed class CacheLoggingOptions
{
    /// <summary>
    /// If no explicit cache headers exist, responses faster than this
    /// are treated as likely cache hits.
    /// </summary>
    public int LikelyHitThresholdMs { get; set; } = 10;

    /// <summary>
    /// Header to check for cache status (e.g., "X-Cache", "CF-Cache-Status").
    /// </summary>
    public string CacheHeaderName { get; set; } = "X-Cache";
}

public sealed class CacheLoggingHandler : DelegatingHandler
{
    private readonly ILogger<CacheLoggingHandler> _logger;
    private readonly CacheLoggingOptions _options;

    public CacheLoggingHandler(
        ILogger<CacheLoggingHandler> logger,
        IOptions<CacheLoggingOptions>? options = null)
    {
        _logger = logger;
        _options = options?.Value ?? new CacheLoggingOptions();
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var sw = Stopwatch.StartNew();

        try
        {
            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            sw.Stop();

            var cacheStatus = GetCacheStatus(response, sw.ElapsedMilliseconds);

            _logger.LogInformation(
                "Request: {Method} {Url} | Status: {StatusCode} | Cache: {CacheStatus} | Time: {ElapsedMs}ms",
                request.Method.Method,
                request.RequestUri?.ToString() ?? "(null)",
                (int)response.StatusCode,
                cacheStatus,
                sw.ElapsedMilliseconds);

            return response;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            sw.Stop();
            _logger.LogWarning(
                "Request canceled: {Method} {Url} | Time: {ElapsedMs}ms",
                request.Method.Method,
                request.RequestUri?.ToString() ?? "(null)",
                sw.ElapsedMilliseconds);
            throw;
        }
        catch (Exception ex)
        {
            sw.Stop();
            _logger.LogError(
                ex,
                "Request failed: {Method} {Url} | Time: {ElapsedMs}ms",
                request.Method.Method,
                request.RequestUri?.ToString() ?? "(null)",
                sw.ElapsedMilliseconds);
            throw;
        }
    }

    private string GetCacheStatus(HttpResponseMessage response, long elapsedMs)
    {
        if (response.Headers.TryGetValues(_options.CacheHeaderName, out var values))
        {
            using var enumerator = values.GetEnumerator();
            if (enumerator.MoveNext())
            {
                return enumerator.Current ?? "UNKNOWN";
            }
            return "UNKNOWN";
        }

        // fallback heuristic
        return elapsedMs <= _options.LikelyHitThresholdMs ? "LIKELY_HIT" : "MISS";
    }
}
