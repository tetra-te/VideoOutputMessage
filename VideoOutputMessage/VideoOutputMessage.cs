using YukkuriMovieMaker.Plugin;
using HarmonyLib;

namespace VideoOutputMessage
{
    public class VideoOutputMessage : IPlugin
    {
        public string Name => "動画出力メッセージ";

        public VideoOutputMessage()
        {
            var harmony = new Harmony("VideoOutputMessage");

            var original = AccessTools.Method("YukkuriMovieMaker.VideoFileWriter.VideoFileWriter:CreateVolumeAdjustMessage");
            var transpiler = typeof(Patch).GetMethod("Transpiler");

            harmony.Patch(original, transpiler: new HarmonyMethod(transpiler));
        }
    }
}
