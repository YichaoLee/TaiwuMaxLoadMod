using System;
using System.Collections.Generic;
using HarmonyLib;
using Config;

using TaiwuModdingLib.Core.Plugin;

namespace TaiwuMaxLoadMod_Frontend
{
    [PluginConfig("TaiwuMaxLoadMod", "Lich", "1.0.0")]
    public class TaiwuMaxLoadMod_Frontend : TaiwuRemakeHarmonyPlugin
    {
        private bool Enable(string key)
        {
            bool enable = false;
            ModManager.GetSetting(this.ModIdStr, key, ref enable);
            return enable;
        }
        public override void OnModSettingUpdate()
        {
            this.HarmonyInstance.UnpatchSelf();
        }
        public override void Initialize()
        {
            this.OnModSettingUpdate();
        }

    }
}