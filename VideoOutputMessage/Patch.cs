using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Windows;
using HarmonyLib;

namespace VideoOutputMessage
{
    public class Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var assemblyPath = Assembly.GetExecutingAssembly().Location;
            var directory = Path.GetDirectoryName(assemblyPath);
            var messagePath = Path.Combine(directory, "Message.txt");
            var showStartupTimePath = Path.Combine(directory, "ShowStartupTime.txt");
            
            var message = "";

            try
            {
                message = File.ReadAllText(messagePath);
            }
            catch
            {
                MessageBox.Show("メッセージを読み込めませんでした", "動画出力メッセージプラグイン");
            }

            var showStartupTimeText = "1";

            try
            {
                showStartupTimeText = File.ReadAllText(showStartupTimePath);
            }
            catch
            {
                MessageBox.Show("起動経過時間の設定を読み込めませんでした", "動画出力メッセージプラグイン");
            }

            bool showStartupTime;

            if (showStartupTimeText == "0")
            {
                showStartupTime = false;
            }
            else if(showStartupTimeText == "1")
            {
                showStartupTime = true;
            }
            else
            {
                showStartupTime = true;
                MessageBox.Show("起動経過時間の設定が間違っています\r\n半角の0または1のみが使用可能です", "動画出力メッセージプラグイン");
            }

            if (message != "" || showStartupTime)
            {
                message = $"\r\n{message}";
            }

            var code = new List<CodeInstruction>(instructions);

            for (int i = code.Count - 1; i >= 0; i--)
            {
                if (code[i].opcode == OpCodes.Ret)
                {
                    if (showStartupTime)
                    {
                        code.Insert(i, new CodeInstruction(OpCodes.Ldstr, message));
                        code.Insert(i + 1, new CodeInstruction(OpCodes.Call, typeof(GetTime).GetMethod("GetStartupTime")));
                        code.Insert(i + 2, new CodeInstruction(OpCodes.Call, typeof(string).GetMethod("Concat", [typeof(string), typeof(string), typeof(string)])));
                    }
                    else
                    {
                        code.Insert(i, new CodeInstruction(OpCodes.Ldstr, message));
                        code.Insert(i + 1, new CodeInstruction(OpCodes.Call, typeof(string).GetMethod("Concat", [typeof(string), typeof(string)])));
                    }
                }
            }

            return code;
        }
    }
}
