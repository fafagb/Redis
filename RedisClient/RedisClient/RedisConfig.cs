using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace RedisClient
{
    public sealed class RedisConfig
    {
      //  public static readonly string config = "m.domainhtecache.save.redis.group.hex.com:6379";//ConfigurationManager.AppSettings["RedisConfig"];
        public static readonly string config = "192.168.3.41:6380";
      //  public static readonly string config = "192.168.244.132:6379";//ConfigurationManager.AppSettings["RedisConfig"];
        public static readonly string redisKey = ConfigurationManager.AppSettings["RedisKey"] ?? "";
        public static string Config()
        {
            return config;
        }
        public static string Key()
        {
            return redisKey;
        }
    }
}
