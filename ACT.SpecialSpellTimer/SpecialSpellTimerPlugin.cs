﻿namespace ACT.SpecialSpellTimer
{
    using System;
    using System.Windows.Forms;

    using ACT.SpecialSpellTimer.Properties;
    using Advanced_Combat_Tracker;

    /// <summary>
    /// SpecialSpellTimer Plugin
    /// </summary>
    public class SpecialSpellTimerPlugin : IActPluginV1
    {
        /// <summary>
        /// 設定パネル
        /// </summary>
        public static ConfigPanel ConfigPanel { get; private set; }

        /// <summary>
        /// プラグインステータス表示ラベル
        /// </summary>
        private Label PluginStatusLabel
        {
            get;
            set;
        }

        /// <summary>
        /// 初期化する
        /// </summary>
        /// <param name="pluginScreenSpace">Pluginタブ</param>
        /// <param name="pluginStatusText">Pluginステータスラベル</param>
        void IActPluginV1.InitPlugin(
            TabPage pluginScreenSpace,
            Label pluginStatusText)
        {
            try
            {
                pluginScreenSpace.Text = "SpecialSpellTimer(スペスペ)";
                this.PluginStatusLabel = pluginStatusText;

                // アップデートを確認する
                this.Update();

                // 設定Panelを追加する
                ConfigPanel = new ConfigPanel();
                ConfigPanel.Dock = DockStyle.Fill;
                pluginScreenSpace.Controls.Add(ConfigPanel);

                // 本体を開始する
                SpellTimerCore.Default.Begin();

                this.PluginStatusLabel.Text = "Plugin Started";
            }
            catch (Exception ex)
            {
                ActGlobals.oFormActMain.WriteExceptionLog(
                    ex,
                    "ACT.SpecialSpellTimer プラグインの初期化で例外が発生しました。");

                this.PluginStatusLabel.Text = "Plugin Initialize Error";
            }
        }

        /// <summary>
        /// 後片付けをする
        /// </summary>
        void IActPluginV1.DeInitPlugin()
        {
            try
            {
                SpellTimerCore.Default.End();
                this.PluginStatusLabel.Text = "Plugin Exited";
            }
            catch (Exception ex)
            {
                ActGlobals.oFormActMain.WriteExceptionLog(
                    ex,
                    "ACT.SpecialSpellTimer プラグインの終了で例外が発生しました。");

                this.PluginStatusLabel.Text = "Plugin Exited Error";
            }
        }

        /// <summary>
        /// アップデートを行う
        /// </summary>
        private void Update()
        {
            if ((DateTime.Now - Settings.Default.LastUpdateDateTime).TotalHours >= 6d)
            {
                var message = UpdateChecker.Update();
                if (!string.IsNullOrWhiteSpace(message))
                {
                    ActGlobals.oFormActMain.WriteExceptionLog(
                        new Exception(),
                        message);
                }

                Settings.Default.LastUpdateDateTime = DateTime.Now;
                Settings.Default.Save();
            }
        }
    }
}
