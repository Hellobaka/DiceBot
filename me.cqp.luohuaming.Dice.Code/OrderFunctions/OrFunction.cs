using me.cqp.luohuaming.Dice.PublicInfos;
using me.cqp.luohuaming.Dice.Sdk.Cqp;
using me.cqp.luohuaming.Dice.Sdk.Cqp.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace me.cqp.luohuaming.Dice.Code.OrderFunctions
{
    public class OrFunction : IOrderModel
    {
        public bool ImplementFlag { get; set; } = true;

        public string GetOrderStr() => "";

        public bool Judge(string destStr) => (destStr.Contains("还是") || destStr.ToLower().Contains("or"));

        private bool Cycling { get; set; } = false;

        private List<OrObject> OrObjects { get; set; } = new List<OrObject>();

        public FunctionResult Progress(CQGroupMessageEventArgs e)
        {
            if (e.Message.Text.Contains(CQApi.CQCode_At(MainSave.QQ).ToString()) is false)
            {
                return new FunctionResult();
            }
            FunctionResult result = new FunctionResult
            {
                Result = true,
                SendFlag = true,
            };
            var sendText = HandleOr(e.Message, e.FromGroup);
            if (sendText == null)
            {
                result.Result = false;
                result.SendFlag = false;
                return result;
            }
            result.SendObject.Add(sendText);
            return result;
        }

        public FunctionResult Progress(CQPrivateMessageEventArgs e)
        {
            FunctionResult result = new FunctionResult
            {
                Result = true,
                SendFlag = true,
            };
            var sendText = HandleOr(e.Message, e.FromQQ);
            if (sendText == null)
            {
                result.Result = false;
                result.SendFlag = false;
                return result;
            }
            result.SendObject.Add(sendText);
            return result;
        }

        public SendText HandleOr(string msg, long id)
        {
            SendText sendText = new SendText
            {
                SendID = id,
                Quote = true
            };
            msg = msg.Replace(CQApi.CQCode_At(MainSave.QQ).ToString(), "").Replace("?", "").Replace("？", "").Replace("呢", "").Replace("我是", "");
            msg = msg.Replace("你", "@@").Replace("我", "你").Replace("@@", "我");
            string[] args = msg.Split(new string[] { "还是", "or", "OR", "Or" }, StringSplitOptions.RemoveEmptyEntries);
            if (args.Length == 0)
            {
                return null;
            }
            string md5 = CommonHelper.ComputeMD5(string.Join("", args));
            StartCycling();
            string result;
            if (OrObjects.Any(x => x.MsgHash == md5))
            {
                var or = OrObjects.First(x => x.MsgHash == md5);
                if (or != null)
                {
                    or.EmphasizeCount++;
                    result = or.Result;
                    result += new string('！', Math.Min(or.EmphasizeCount, 3));
                }
                else
                {
                    return null;
                }
            }
            else
            {
                double randomResult = MainSave.Random.NextDouble();
                int index = (int)(randomResult * args.Length);
                index = Math.Min(index, args.Length);
                result = args[index];
                OrObjects.Add(new OrObject
                {
                    Msg = args,
                    MsgHash = md5,
                    Result = result,
                    Timeout = 0
                });
                // result += $"random={randomResult}";
            }
            result = result.Trim();
            sendText.MsgToSend.Add(result);
            return sendText;
        }

        private void StartCycling()
        {
            if (Cycling)
            {
                return;
            }
            Cycling = true;
            new Thread(() =>
            {
                while (true)
                {
                    for (int i = 0; i < OrObjects.Count; i++)
                    {
                        var item = OrObjects[i];
                        item.Timeout++;
                        if (item.Timeout >= 3 * 60 * 60)
                        {
                            OrObjects.Remove(item);
                            i--;
                        }
                    }
                    Thread.Sleep(1000);
                }
            }).Start(); ;
        }

        private class OrObject
        {
            public string[] Msg { get; set; }

            public int Timeout { get; set; }

            public string MsgHash { get; set; } = "";

            public string Result { get; set; } = "";

            public int EmphasizeCount { get; set; }
        }
    }
}
