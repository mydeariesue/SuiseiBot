namespace com.cbgan.SuiseiBot.Code.Resource.TypeEnum.CmdType
{
    internal enum PCRGuildCmdType : int
    {
        /// <summary>
        /// 建会
        /// </summary>
        CreateGuild = 1,
        /// <summary>
        /// 入会
        /// </summary>
        JoinGuild = 2,
        /// <summary>
        /// 查看成员
        /// </summary>
        ListMember = 3,
        /// <summary>
        /// 退会
        /// </summary>
        QuitGuild = 4,
        /// <summary>
        /// 清空成员
        /// </summary>
        QuitAll = 5,
        /// <summary>
        /// 一键入会
        /// </summary>
        JoinAll = 6,
        /// <summary>
        /// 会战开始指令
        /// </summary>
        BattleStart = 101,
        /// <summary>
        /// 会战开始指令
        /// </summary>
        UpdateBoss = 201
    }
}
