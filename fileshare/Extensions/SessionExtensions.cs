using System;
using System.Text.Json;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace FileShare.Extensions
{
    public static class SessionExtensions
    {
        public static T Get<T>(this ISession session, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new Exception("Key provided is null or whitespace");
            string value = session.GetString(key);
            if (string.IsNullOrWhiteSpace(value))
                return default(T);
            T deserializedValue = JsonSerializer.Deserialize<T>(value);
            return deserializedValue;
        }

        public static void Set<T>(this ISession session, string key, T value)
        {
            if (value == null)
                throw new Exception("Value provided is null");
            if (string.IsNullOrWhiteSpace(key))
                throw new Exception("Key provided is null or whitespace");
            string serializedValue = JsonSerializer.Serialize(value);
            session.SetString(key, serializedValue);
        }
    }
}