using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using com.cbgan.SuiseiBot.Code.Tool.Log;
using SharpYaml.Serialization;

namespace com.cbgan.SuiseiBot.Code.IO.Config
{
    internal class ConfigIO
    {
        #region 属性
        private string Path { set; get; }
        public ConfigClass LoadedConfig { private set; get; }
        #endregion

        #region 构造函数
        /// <summary>
        /// ConfigIO构造函数，默认构造时加载本地配置文件
        /// </summary>
        /// <param name="loginQQ"></param>
        /// <param name="initConfig"></param>
        public ConfigIO(long loginQQ, bool initConfig = true)
        {
            this.Path = IOUtils.GetConfigPath(loginQQ.ToString());
            //执行一次加载
            if (initConfig) ConfigFileInit();
        }
        #endregion

        #region 公有方法
        /// <summary>
        /// 加载配置文件
        /// </summary>
        public bool LoadConfig()
        {
            try
            {
                Serializer       serializer = new Serializer();
                using TextReader reader     = File.OpenText(Path);
                LoadedConfig = serializer.Deserialize<ConfigClass>(reader);
                return true;
            }
            catch (Exception e)
            {
                ConsoleLog.Error("ConfigIO ERROR", ConsoleLog.ErrorLogBuilder(e));
                return false;
            }
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 初始化配置文件
        /// </summary>
        /// <returns>ConfigClass</returns>
        private ConfigClass getInitConfigClass()
        {
            return new ConfigClass
            {
                LogLevel              = LogLevel.Info,
                ModuleSwitch = new Module
                {
                    Bili_Subscription = false,
                    Debug             = false,
                    HaveFun           = true,
                    PCR_GuildManager  = false,
                    PCR_GuildRank     = false,
                    PCR_Subscription  = false,
                    Suisei            = false,
                    Setu              = false
                },
                SubscriptionConfig = new BiliSubscription
                {
                    FlashTime = 3600,
                    GroupsConfig = new List<GroupSubscription>
                    {
                        new GroupSubscription()
                        {
                            GroupId          = new List<long>(),
                            PCR_Subscription = false,
                            SubscriptionId   = new List<long>()
                        }
                    }
                }
            };
        }
        /// <summary>
        /// 初始化配置文件并返回当前配置文件内容
        /// </summary>
        private void ConfigFileInit()
        {
            try
            {
                //当读取到文件时直接返回
                if (File.Exists(Path) && LoadConfig())
                {
                    ConsoleLog.Debug("ConfigIO", "读取配置文件");
                    return;
                }
                //没读取到文件时创建新的文件
                ConsoleLog.Warning("ConfigIO", "未找到配置文件");
                ConsoleLog.Info("ConfigIO", "创建新的配置文件");
                Serializer       serializer = new Serializer(new SerializerSettings { });
                ConfigClass      config     = getInitConfigClass();
                string           configText = serializer.Serialize(config);
                using TextWriter writer     = File.CreateText(Path);
                writer.Write(configText);
                LoadedConfig = config;
            }
            catch (Exception e)
            {
                ConsoleLog.Fatal("ConfigIO ERROR", ConsoleLog.ErrorLogBuilder(e));
                Thread.Sleep(5000);
                Environment.Exit(-1);
            }
        }
        #endregion
    }
}
