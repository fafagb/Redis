using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
namespace RedisClient
{
    public class RedisManager
    {
        private static string Constring = RedisConfig.Config();
        private static readonly object locker = new object();
        private static ConnectionMultiplexer instance;
        private static readonly Dictionary<string, ConnectionMultiplexer> Concache = new Dictionary<string, ConnectionMultiplexer>();

        /// <summary>
        /// 单例模式获取redis连接实例
        /// </summary>
        public static ConnectionMultiplexer Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        if (instance == null)
                            instance = GetManager();
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// 从缓存中获取
        /// </summary>
        /// <param name="constr"></param>
        /// <returns></returns>
        public static ConnectionMultiplexer GetConForMap(string constr)
        {
            if (!Concache.ContainsKey(constr))
                Concache[constr] = GetManager(constr);
            return Concache[constr];
        }

        private static ConnectionMultiplexer GetManager(string constr = null)
        {
            constr = constr ?? Constring;
            var connect = ConnectionMultiplexer.Connect(constr);

            #region 注册事件
            connect.ConnectionFailed += MuxerConnectionFailed;
            connect.ConnectionRestored += MuxerConnectionRestored;
            connect.ErrorMessage += MuxerErrorMessage;
            connect.ConfigurationChanged += MuxerConfigurationChanged;
            connect.HashSlotMoved += MuxerHashSlotMoved;
            connect.InternalError += MuxerInternalError;
            #endregion

            return connect;
        }
        #region Redis事件
        /// <summary>
        /// 内部异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerInternalError(object sender, InternalErrorEventArgs e)
        {
            
            Console.WriteLine("内部异常：" + e.Exception.Message);
        }

        /// <summary>
        /// 集群更改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerHashSlotMoved(object sender, HashSlotMovedEventArgs e)
        {
            Console.WriteLine("新集群：" + e.NewEndPoint + "旧集群：" + e.OldEndPoint);
        }

        /// <summary>
        /// 配置更改事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerConfigurationChanged(object sender, EndPointEventArgs e)
        {
            Console.WriteLine("配置更改：" + e.EndPoint);
        }

        /// <summary>
        /// 错误事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerErrorMessage(object sender, RedisErrorEventArgs e)
        {
            Console.WriteLine("异常信息：" + e.Message);
        }

        /// <summary>
        /// 重连错误事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerConnectionRestored(object sender, ConnectionFailedEventArgs e)
        {
            Console.WriteLine("重连错误" + e.EndPoint);
        }

        /// <summary>
        /// 连接失败事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerConnectionFailed(object sender, ConnectionFailedEventArgs e)
        {
            Console.WriteLine("连接异常" + e.EndPoint + "，类型为" + e.FailureType + (e.Exception == null ? "" : ("，异常信息是" + e.Exception.Message)));
        }
        #endregion
    }
}

