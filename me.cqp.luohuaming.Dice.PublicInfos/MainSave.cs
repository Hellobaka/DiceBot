using me.cqp.luohuaming.Dice.Sdk.Cqp;
using System;
using System.Collections.Generic;
using System.IO;

namespace me.cqp.luohuaming.Dice.PublicInfos
{
    public static class MainSave
    {
        /// <summary>
        /// 保存各种事件的数组
        /// </summary>
        public static List<IOrderModel> Instances { get; set; } = new List<IOrderModel>();
        public static CQLog CQLog { get; set; }
        public static CQApi CQApi { get; set; }
        public static string AppDirectory { get; set; }
        public static string ImageDirectory { get; set; }
        public static Random Random { get; set; } = new Random();
    }
}
