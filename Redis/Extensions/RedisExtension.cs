﻿using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace Redis.Extensions
{
    public static class RedisExtension
    {
        public static HashEntry[] ToHashEntries(this object obj)
        {
            PropertyInfo[] properties = obj.GetType().GetProperties();
            return properties
                .Where(x => x.GetValue(obj) != null)
                .Select(property =>
                {
                    object propertyValue = property.GetValue(obj);
                    string hashValue;

                    if (propertyValue is IEnumerable<object>)
                        hashValue = JsonConvert.SerializeObject(propertyValue);
                    else
                        hashValue = propertyValue.ToString();

                    return new HashEntry(property.Name, hashValue);
                }).ToArray();
        }

        public static T FromHashEntries<T>(HashEntry[] hashEntries)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            var obj = Activator.CreateInstance(typeof(T));
            foreach (var property in properties)
            {
                HashEntry entry = hashEntries.FirstOrDefault(g => g.Name.ToString().Equals(property.Name));
                if (entry.Equals(new HashEntry())) continue;
                property.SetValue(obj, Convert.ChangeType(entry.Value.ToString(), property.PropertyType));
            }
            return (T)obj;
        }
    }
}
