using HarmonyLib;
using System.IO;
using System.Reflection.Emit;
using System.Windows;
using VideoOutputMessage.Settings;
using YukkuriMovieMaker.Commons;

namespace VideoOutputMessage
{
    public class Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var message = VideoOutputSettings.Default.Message;
            bool showStartupTime = VideoOutputSettings.Default.ShowStartupTime;

            if (!string.IsNullOrEmpty(message) || showStartupTime)
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
