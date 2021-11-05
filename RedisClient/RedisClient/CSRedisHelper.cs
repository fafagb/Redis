using System;
using System.Linq;
using System.Threading;

namespace RedisClient {
    public class CSRedisHelper {
        private static string _connstr = "192.168.3.41:6380";
        private static string _keyStream = "stream1";
        private static string _nameGrourp = "group1";
        private static string _nameConsumer = "consumer1";

        public void pub (string key, string value) {

            var csRedis = new CSRedis.CSRedisClient (_connstr);
            csRedis.XAdd (_keyStream, "*", (key, value));

            try {
                var arr = csRedis.Exists ("123");
                if (!arr) {
                    csRedis.XGroupCreate (_keyStream, _nameGrourp);
                }
                // csRedis.XGroupCreate (_keyStream, _nameGrourp);//第一次创建，加存在判断
            } catch (Exception ex){

            }
        }

        public void sub () {

            var csRedis = new CSRedis.CSRedisClient (_connstr);

            (string key, (string id, string[] items) [] data) [] product;
            (string Pid, string Platform, string Time) data = (null, null, null);

            while (true) {
                try {

                    product = csRedis.XReadGroup (_nameGrourp, _nameConsumer, 1, 10000, (_keyStream, ">"));
                    if (product?.Length > 0 == true && product[0].data?.Length > 0 == true) {
                        Console.WriteLine ($"message-id:{product.FirstOrDefault().data.FirstOrDefault().id}");

                        product.FirstOrDefault ().data.FirstOrDefault ().items.ToList ().ForEach (value => {
                            Console.WriteLine ($"    {value}");
                        });

                        //csRedis.XAck(_keyStream, _nameGrourp, product[0].data[0].id);
                    }
                } catch (Exception) {
                    //throw;
                }

            }

        }

    }

}