using me.cqp.luohuaming.Dice.PublicInfos;
using me.cqp.luohuaming.Dice.Sdk.Cqp.EventArgs;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace me.cqp.luohuaming.Dice.Code.OrderFunctions
{
    public class DiceFunction : IOrderModel
    {
        public bool ImplementFlag { get; set; } = true;

        public string GetOrderStr() => "";

        public bool Judge(string destStr)
        {
            destStr = destStr.Trim().ToLower().Replace("。", ".");
            return Regex.IsMatch(destStr, "^r[\\d\\.]*$") || Regex.IsMatch(destStr, "^[\\d]+d[\\d\\.]+$");
        }

        public FunctionResult Progress(CQGroupMessageEventArgs e)
        {
            FunctionResult result = new FunctionResult
            {
                Result = true,
                SendFlag = true,
            };
            var sendText = HandleDice(e.Message, e.FromGroup);
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
            var sendText = HandleDice(e.Message, e.FromQQ);
            if (sendText == null)
            {
                result.Result = false;
                result.SendFlag = false;
                return result;
            }
            result.SendObject.Add(sendText);
            return result;
        }

        public SendText HandleDice(string msg, long id)
        {
            SendText sendText = new SendText
            {
                SendID = id,
                Quote = true
            };
            msg = msg.Trim().ToLower().Replace("。", ".");
            if (!Regex.IsMatch(msg, "^r[\\d\\.]*$") && !Regex.IsMatch(msg, "^[\\d]+d[\\d\\.]+$"))
            {
                return null;
            }
            bool doubleFlag = false;
            int diceCount = 0;
            double min = 1, max = 6;
            List<double> diceResult = new List<double>();
            List<string> result = new List<string>();
            if (msg.StartsWith("r"))
            {
                diceCount = 1;
                string digtalStr = "";
                for (int i = 1; i < msg.Length; i++)
                {
                    if (char.IsDigit(msg[i]) || (!digtalStr.Contains(".") && msg[i] == '.'))
                    {
                        digtalStr += msg[i].ToString();
                    }
                    else
                    {
                        break;
                    }
                }
                if (!string.IsNullOrEmpty(digtalStr) && !double.TryParse(digtalStr, out max))
                {
                    MainSave.CQLog.Info("R-Dice", $"Invalid Input: {msg}");
                    return null;
                }
                doubleFlag = digtalStr.Contains(".");
            }
            else
            {
                string digtalStr = "";
                for (int i = 0; i < msg.Length; i++)
                {
                    if (char.IsDigit(msg[i]) || (!digtalStr.Contains(".") && msg[i] == '.'))
                    {
                        digtalStr += msg[i].ToString();
                    }
                    else if (msg[i] == 'd' && diceCount == 0)
                    {
                        if (string.IsNullOrEmpty(digtalStr))
                        {
                            MainSave.CQLog.Info("D-Dice", "Invalid DiceCount");
                            return null;
                        }
                        diceCount = (int)double.Parse(digtalStr);
                        digtalStr = "";
                    }
                    else
                    {
                        MainSave.CQLog.Info("D-Dice", $"Invalid Input: {msg}");
                        return null;
                    }
                }
                if (!string.IsNullOrEmpty(digtalStr) && !double.TryParse(digtalStr, out max))
                {
                    MainSave.CQLog.Info("D-Dice", $"Invalid Input: {msg}");
                    return null;
                }
                doubleFlag = digtalStr.Contains(".");
            }
            if (doubleFlag)
            {
                for (int i = 0; i < diceCount; i++)
                {
                    double r = (MainSave.Random.NextDouble() * (max - min)) + min;
                    diceResult.Add(r);
                    result.Add(r.ToString("0.0000"));
                }
            }
            else
            {
                for (int i = 0; i < diceCount; i++)
                {
                    int r = MainSave.Random.Next((int)min, (int)max + 1);
                    diceResult.Add(r);
                    result.Add(r.ToString());
                }
            }
            if (result.Count == 0)
            {
                MainSave.CQLog.Info("Dice", $"Invalid Input: {msg}");
                return null;
            }
            else
            {
                string sum = "";
                if (diceCount > 2)
                {
                    sum = $"\nSum: {diceResult.Sum():f4} Avg: {diceResult.Average():f4} Min: {diceResult.Min():f4} Max: {diceResult.Max():f4}";
                }
                sendText.MsgToSend.Add($"{string.Join(",", result)}{sum}");
            }
            return sendText;
        }
    }
}
