using System.IO;
using System.Windows;
using YukkuriMovieMaker.Commons;
using YukkuriMovieMaker.Plugin;

namespace VideoOutputMessage
{
    public class VideoOutputMessage : IPlugin
    {
        public string Name => "動画出力メッセージ";

        public VideoOutputMessage()
        {
            var ymm4PluginDirectory = AppDirectories.PluginDirectory;

            var harmonyFolder = Path.Combine(ymm4PluginDirectory, "Harmony");
            var kerningFolder = Path.Combine(ymm4PluginDirectory, "SimpleKerningEffect");
            var ratioOutlineFolder = Path.Combine(ymm4PluginDirectory, "RatioOutlineEffect");
            var videoOutputMessageFolder = Path.Combine(ymm4PluginDirectory, "VideoOutputMessage");

            var harmonyYmmeDllName = "lib.har.ymmelib";

            // 0Harmony.ymmedllのパス候補
            var kerningHarmonyYmmeDll = Path.Combine(kerningFolder, harmonyYmmeDllName);
            var ratioOutlineHarmonyYmmeDll = Path.Combine(ratioOutlineFolder, harmonyYmmeDllName);

            var harmonyDllName = "0Harmony.dll";

            // 0Harmony.dllのパス候補
            var harmonyDll = Path.Combine(harmonyFolder, harmonyDllName);
            var kerningHarmonyDll = Path.Combine(kerningFolder, harmonyDllName);
            var ratioOutlineHarmonyDll = Path.Combine(ratioOutlineFolder, harmonyDllName);
            var videoOutputMessageHarmonyDll = Path.Combine(videoOutputMessageFolder, harmonyDllName);

            List<string> pluginToUpdate = [];

            if (File.Exists(kerningHarmonyDll) && (!File.Exists(kerningHarmonyYmmeDll)))
                pluginToUpdate.Add("簡易カーニング");
            if (File.Exists(ratioOutlineHarmonyDll) && (!File.Exists(ratioOutlineHarmonyYmmeDll)))
                pluginToUpdate.Add("比率縁取り");

            List<string> fileToDelte = [];

            if (File.Exists(harmonyDll))
                fileToDelte.Add(harmonyDll);
            if (File.Exists(kerningHarmonyDll))
                fileToDelte.Add(kerningHarmonyDll);
            if (File.Exists(ratioOutlineHarmonyDll))
                fileToDelte.Add(ratioOutlineHarmonyDll);
            if (File.Exists(videoOutputMessageHarmonyDll))
                fileToDelte.Add(videoOutputMessageHarmonyDll);

            var message = "";

            for (int i = 0; i < pluginToUpdate.Count; i++)
            {
                if (i == 0)
                    message += "以下のプラグインはアップデートが必要です。\r\n最新版をダウンロードしてインストールしてください。\r\n";

                message += "・" + pluginToUpdate[i] + "\r\n";

                if (i == pluginToUpdate.Count - 1)
                    message += "\r\n";
            }

            for (int i = 0; i < fileToDelte.Count; i++)
            {
                if (i == 0)
                    message += "YMM4を終了して以下のファイルを削除してください。\r\n削除しないとYMM4が起動しなくなることがあります。\r\n";

                message += "・" + fileToDelte[i] + "\r\n";
            }

            if (message != "")
                MessageBox.Show(message, "動画出力メッセージプラグイン");


            var harmony = Activator.CreateInstance(HRef.Harmony, ["VideoOutputMessage"]);

            var original = HRef.AccessToolsMethod.Invoke(null, ["YukkuriMovieMaker.VideoFileWriter.VideoFileWriter:GetRemainingTimeText", null, null]);
            var transpiler = typeof(Patch).GetMethod("Transpiler");
            var transpilerH = Activator.CreateInstance(HRef.HarmonyMethod, [transpiler]);

            HRef.HarmonyPatch.Invoke(harmony, [original, null, null, transpilerH, null]);
        }
    }
}
