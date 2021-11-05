using System;

namespace RedisClient {
    class Program {
        static void Main (string[] args) {
            string para = args[0];
            // RedisHelper redisHelper = new RedisHelper ();
            // // UserModel user = new UserModel();
            // // redisHelper.Test(user, 1);
            // if (para == "pub") {
            //     string text = Console.ReadLine ();
            //     redisHelper.WriteStream (text);
            // } else if (para == "sub") {
            //     redisHelper.ReadStream ();
            // }
            CSRedisHelper redisHelper = new CSRedisHelper ();
            // UserModel user = new UserModel();
            // redisHelper.Test(user, 1);
            if (para == "pub") {
                Console.WriteLine ("请输入key");
                string key = Console.ReadLine ();
                Console.WriteLine ("请输入value");
                string value = Console.ReadLine ();
                redisHelper.pub (key, value);
            } else if (para == "sub") {
                redisHelper.sub ();
            }

        }
    }
}