using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YukkuriMovieMaker.Plugin;

namespace VideoOutputMessage.Settings
{
    internal class VideoOutputSettings : SettingsBase<VideoOutputSettings>
    {
        public override SettingsCategory Category => SettingsCategory.None;
        public override string Name => "動画出力メッセージ";

        public override bool HasSettingView => true;
        public override object? SettingView => new VideoOutputMessageSettingsView();

        public string Message { get => message; set => Set(ref message, value); }
        private string message = "動画編集お疲れ様！";

        public bool ShowStartupTime { get => showStartupTime; set => Set(ref showStartupTime, value); }
        private bool showStartupTime = true;

        public override void Initialize()
        {
        }
    }
}
