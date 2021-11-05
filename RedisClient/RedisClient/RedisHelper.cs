using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace RedisClient {
    public class RedisHelper {

        ConnectionMultiplexer _conn = RedisManager.Instance; //初始化

        ////public static IEnumerable<T> Where<T>(this IEnumerable<T> items,Func<T,bool> func)
        ////{
        ////    List<T> data = new List<T>();
        ////    foreach (var item in items)
        ////    {
        ////        if (func(item)) {
        ////            data.Add(item);
        ////        }
        ////    }
        ////    return data;
        ////}

        public void Test<T> (T t, long id) {
            var obj = t.GetType ();

            var database = _conn.GetDatabase (0);
            UserModel user = new UserModel ();
            user.Id = id;
            user.UserName = "名称1";
            user.Pwd = "123";
            user.NickName = "昵称1";
            string json = JsonConvert.SerializeObject (user); //序列化
            database.HashSet ("user", id, json);

            //获取Model
            string hashcang = database.HashGet ("user", id);
            user = JsonConvert.DeserializeObject<UserModel> (hashcang); //反序列化

            //获取List
            RedisValue[] values = database.HashValues ("user"); //获取所有value
            IList<UserModel> demolist = new List<UserModel> ();
            foreach (var item in values) {
                UserModel hashmodel = JsonConvert.DeserializeObject<UserModel> (item);
                demolist.Add (hashmodel);
            }
        }

        public void WriteStream (string text) {
            var database = _conn.GetDatabase (0);
            var messageId = database.StreamAdd ("event_stream", "foo_name", text);

            Console.WriteLine ($"消息id为{messageId}");
        }

        public void ReadStream () {

            var database = _conn.GetDatabase (0);
            while (true) {
                var messages = database.StreamRead ("event_stream", "0-0");
            
  Console.WriteLine ($"接受消息id为{messages[0].Id}接受数据为{messages[0].Values[0].Value}");
                
              
            }

        }

    }
}