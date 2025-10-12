using System.Reflection.Emit;
using HarmonyLib;
using VideoOutputMessage.Settings;

namespace VideoOutputMessage
{
    public class Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var message = VideoOutputSettings.Default.Message;
            var showStartupTime = VideoOutputSettings.Default.ShowStartupTime;

            if (!string.IsNullOrEmpty(message) || showStartupTime)
            {
                message = "\r\n" + message;
            }

            foreach (var instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Ret)
                {
                    if (showStartupTime)
                    {
                        yield return new CodeInstruction(OpCodes.Ldstr, message);
                        yield return new CodeInstruction(OpCodes.Call, typeof(GetTime).GetMethod("GetStartupTime"));
                        yield return new CodeInstruction(OpCodes.Call, typeof(string).GetMethod("Concat", [typeof(string), typeof(string), typeof(string)]));
                    }
                    else
                    {
                        yield return new CodeInstruction(OpCodes.Ldstr, message);
                        yield return new CodeInstruction(OpCodes.Call, typeof(string).GetMethod("Concat", [typeof(string), typeof(string)]));
                    }
                }
                
                yield return instruction;
            }
        }
    }
}
