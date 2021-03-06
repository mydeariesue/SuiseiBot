using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using com.cbgan.SuiseiBot.Code.Resource.TypeEnum;
using Native.Sdk.Cqp;
using Native.Sdk.Cqp.Enum;
using Native.Sdk.Cqp.Model;

namespace com.cbgan.SuiseiBot.Code.Tool
{
    internal static class Utils
    {
        #region CQCode处理

        /// <summary>
        /// 根据CQCodes获取AT的QQ号列表
        /// </summary>
        /// <param name="codeList">CQCodes 列表</param>
        /// <param name="status">状态\n（-1说明存在不合法的QQ号）</param>
        /// <returns>存放QQ号的列表</returns>
        public static List<long> GetAtList(List<CQCode> codeList, out int status)
        {
            List<long> ret = new List<long>();
            status = 0;
            foreach (CQCode code in codeList) //检查每一个AT
            {
                if (code.Function.Equals(CQFunction.At)            &&
                    code.Items.ContainsKey("qq")                   &&
                    long.TryParse(code.Items["qq"], out long qqid) &&
                    qqid > QQ.MinValue)
                {
                    ret.Add(qqid);
                }
                else
                {
                    //有操作的QQ号非法
                    status = -1;
                }
            }

            return ret;
        }

        #endregion

        #region 时间戳转换工具

        /// <summary>
        /// 检查参数数组长度
        /// </summary>
        /// <param name="args">指令数组</param>
        /// <param name="len">至少需要的参数个数</param>
        /// <param name="QQgroup">（可选，不给的话就不发送错误信息）\n报错信息要发送到的QQ群对象</param>
        /// <param name="fromQQid">（可选，但QQgroup给了的话本参数必填）\n要通知的人的QQ Id</param>
        /// <returns>长度合法性\n-1不符合 0符合 1超出</returns>
        public static LenType CheckForLength(string[] args, int len, Group QQgroup = null, long fromQQid = 0)
        {
            if (args.Length >= len + 1)
            {
                if (args.Length == len + 1) return LenType.Legitimate;
                else return LenType.Extra;
            }
            else
            {
                QQgroup?.SendGroupMessage(CQApi.CQCode_At(fromQQid), " 命令参数不全，请补充。");

                return LenType.Illegal;
            }
        }

        /// <summary>
        /// 获取当前时间戳
        /// 时间戳单位(毫秒)
        /// </summary>
        public static long GetNowTimeStamp() 
            => (long)(DateTime.Now - new DateTime(1970, 1, 1, 8, 0, 0, 0)).TotalSeconds;

        /// <summary>
        /// 获取今天零点的时间戳
        /// 时间戳单位(毫秒)
        /// </summary>
        public static long GetTodayStamp() 
            => (long)(DateTime.Today - new DateTime(1970, 1, 1, 8, 0, 0, 0)).TotalSeconds;

        /// <summary>
        /// 将long类型时间戳转换为DateTime
        /// 时间戳单位(毫秒)
        /// </summary>
        public static DateTime TimeStampToDateTime(long TimeStamp)
            => new System.DateTime(1970, 1, 1, 8, 0, 0, 0).AddSeconds(TimeStamp);

        /// <summary>
        /// 将DateTime转换为long时间戳
        /// 时间戳单位(毫秒)
        /// </summary>
        public static long DateTimeToTimeStamp(DateTime dateTime)
            => (long)(dateTime - new DateTime(1970, 1, 1, 8, 0, 0, 0)).TotalSeconds;
        #endregion

        #region 群成员处理

        /// <summary>
        /// 获取群成员的名片，没有则获取昵称
        /// </summary>
        public static string getNick(GroupMemberInfo input)
        {
            return input.Card == "" ? input.Nick : input.Card;
        }

        #endregion

        #region 字符串处理

        /// <summary>
        /// 获取字符串在QQ上显示的长度（用于PadQQ函数）
        /// </summary>
        /// <param name="input">要计算长度的字符串</param>
        /// <param name="len">至少需要的参数个数</param>
        /// <returns>长度（不要问为啥是Double，0.5个字符真的存在）</returns>
        public static double getQQStrLength(string input)
        {
            double strLength = 0;
            foreach (char i in input)
            {
                if (Char.IsLetter(i))
                {
                    strLength += 2.5;
                }
                else if (Char.IsNumber(i))
                {
                    strLength += 2;
                }
                else if (Char.IsSymbol(i))
                {
                    strLength += 2;
                }
                else
                {
                    strLength += 3;
                }
            }

            return strLength;
        }

        /// <summary>
        /// 对字符串进行PadRight，但符合QQ上的对齐标准
        /// </summary>
        /// <param name="input">要补齐的字符串</param>
        /// <param name="padNums">补齐的长度（请使用getQQStrLength进行计算）</param>
        /// <param name="paddingChar">用来对齐的字符（强烈建议用默认的空格，其他字符请手动计算后用String类原生的PadRight进行操作）</param>
        /// <returns>补齐长度后的字符串</returns>
        public static string PadRightQQ(string input, double padNums, char paddingChar = ' ')
        {
            StringBuilder sb = new StringBuilder();

            int toPadNum = int.Parse(Math.Floor(padNums - getQQStrLength(input)).ToString());
            if (toPadNum <= 0)
            {
                return input;
            }
            else
            {
                sb.Append(input);
                for (int i = 0; i < toPadNum; i++)
                {
                    sb.Append(paddingChar);
                }

                return sb.ToString();
            }
        }

        #endregion
    }
}