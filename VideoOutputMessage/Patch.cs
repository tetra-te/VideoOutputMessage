using System.IO;
using System.Reflection.Emit;
using System.Windows;
using YukkuriMovieMaker.Commons;
using HarmonyLib;

namespace VideoOutputMessage
{
    public class Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var pluginFolder = Path.Combine(AppDirectories.PluginDirectory, "VideoOutputMessage");
            var messagePath = Path.Combine(pluginFolder, "Message.txt");
            var showStartupTimePath = Path.Combine(pluginFolder, "ShowStartupTime.txt");

            if (!File.Exists(messagePath))
            {
                using (var writer = new StreamWriter(messagePath))
                {
                    writer.Write("動画編集お疲れ様！");
                }
            }

            if (!File.Exists(showStartupTimePath))
            {
                using (var writer = new StreamWriter(showStartupTimePath))
                {
                    writer.Write("1");
                }
            }

            var message = "";

            try
            {
                message = File.ReadAllText(messagePath);
            }
            catch
            {
                MessageBox.Show($"メッセージを読み込めませんでした\r\n以下のファイルが読み込めませんでした\r\n{messagePath}", "動画出力メッセージプラグイン");
            }

            var showStartupTimeText = "1";

            try
            {
                showStartupTimeText = File.ReadAllText(showStartupTimePath);
            }
            catch
            {
                MessageBox.Show($"起動経過時間表示の設定を読み込めませんでした\r\n以下のファイルが読み込めませんでした\r\n{showStartupTimePath}", "動画出力メッセージプラグイン");
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
                MessageBox.Show($"起動経過時間の設定が間違っています\r\n{showStartupTimePath}には半角数字の0または1のみを書いてください", "動画出力メッセージプラグイン");
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
