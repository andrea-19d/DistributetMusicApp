using System.Text;
using CacheManager.Core;
using CacheManager.Core.Internal;
using Newtonsoft.Json;

namespace ApiGateway.Api.Extensions;

public sealed class NewtonsoftJsonCacheSerializer : ICacheSerializer
{
    private static readonly JsonSerializerSettings Settings = new()
    {
        // Keeps things simple + stable
        TypeNameHandling = TypeNameHandling.None,
        NullValueHandling = NullValueHandling.Ignore
    };

    // Envelope used to reliably round-trip CacheItem
    private sealed class CacheItemEnvelope
    {
        public string? Key { get; set; }
        public object? Value { get; set; }
        public ExpirationMode ExpirationMode { get; set; }
        public TimeSpan ExpirationTimeout { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime LastAccessedUtc { get; set; }
    }

    public byte[] Serialize<T>(T value)
    {
        if (value is null) return Array.Empty<byte>();

        var json = JsonConvert.SerializeObject(value, Settings);
        return Encoding.UTF8.GetBytes(json);
    }

    public object Deserialize(byte[] data, Type target)
    {
        if (data is null || data.Length == 0)
        {
            // return default instance for value types, null for ref types
            return target.IsValueType ? Activator.CreateInstance(target)! : null!;
        }

        var json = Encoding.UTF8.GetString(data);
        return JsonConvert.DeserializeObject(json, target, Settings)!;
    }

    public byte[] SerializeCacheItem<T>(CacheItem<T> value)
    {
        if (value is null) return Array.Empty<byte>();

        var envelope = new CacheItemEnvelope
        {
            Key = value.Key,
            Value = value.Value,
            ExpirationMode = value.ExpirationMode,
            ExpirationTimeout = value.ExpirationTimeout,
            CreatedUtc = value.CreatedUtc,
            LastAccessedUtc = value.LastAccessedUtc
        };

        var json = JsonConvert.SerializeObject(envelope, Settings);
        return Encoding.UTF8.GetBytes(json);
    }

    public CacheItem<T> DeserializeCacheItem<T>(byte[] value, Type valueType)
    {
        if (value is null || value.Length == 0)
        {
            // empty cache item (won't really happen normally)
            return new CacheItem<T>(string.Empty, default!);
        }

        var json = Encoding.UTF8.GetString(value);
        var envelope = JsonConvert.DeserializeObject<CacheItemEnvelope>(json, Settings)
                       ?? new CacheItemEnvelope();

        // Convert stored Value to T (valueType is provided by CacheManager)
        T typedValue;
        if (envelope.Value is null)
        {
            typedValue = default!;
        }
        else if (envelope.Value is T t)
        {
            typedValue = t;
        }
        else
        {
            // re-serialize to force correct target type
            var vJson = JsonConvert.SerializeObject(envelope.Value, Settings);
            typedValue = (T)JsonConvert.DeserializeObject(vJson, valueType, Settings)!;
        }

        var key = envelope.Key ?? string.Empty;

        // Create CacheItem with expiration info.
        // CacheItem<T> has multiple ctors across versions; this one is common:
        var item = new CacheItem<T>(
            key,
            typedValue,
            envelope.ExpirationMode,
            envelope.ExpirationTimeout
        );

        // We cannot set CreatedUtc / LastAccessedUtc directly (read-only),
        // but Redis handle doesn't require them for correctness.

        return item;
    }
}
