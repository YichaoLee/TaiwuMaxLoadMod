using System;
using System.Collections.Generic;
using HarmonyLib;
using Config;
using TaiwuModdingLib.Core.Plugin;
using System.Linq;
using GameData.Domains;
using GameData.Domains.Character;
using GameData.Domains.Taiwu;
using System.Net.Http;
using UnityEngine;
using GameData.Utilities;

namespace TaiwuMaxLoadMod_Backend
{
    [PluginConfig("TaiwuMaxLoadMod", "Lich", "1.0.0")]
    public class TaiwuMaxLoadMod : TaiwuRemakeHarmonyPlugin
    {
        public static int I_MaxInventory = 0;
        public static int I_MaxResourceCount = 0;
        public static int I_MaxWarehouseLoad = 0;

        private bool Enable(string key)
        {
            bool enable = false;
            DomainManager.Mod.GetSetting(ModIdStr, key, ref enable);
            return enable;
        }

        public override void OnModSettingUpdate()
        {
            this.HarmonyInstance.UnpatchSelf();
            AdaptableLog.Info("TaiwuMaxLoadMod OnModSettingUpdate");
            DomainManager.Mod.GetSetting(ModIdStr, "I_MaxInventory", ref I_MaxInventory);
            AdaptableLog.Info(string.Format("TaiwuMaxLoadMod GetSetting I_MaxInventory {0}", I_MaxInventory));
            DomainManager.Mod.GetSetting(ModIdStr, "I_MaxResourceCount", ref I_MaxResourceCount);
            AdaptableLog.Info(string.Format("TaiwuMaxLoadMod GetSetting I_MaxResourceCount {0}", I_MaxResourceCount));
            DomainManager.Mod.GetSetting(ModIdStr, "I_MaxWarehouseLoad", ref I_MaxWarehouseLoad);
            AdaptableLog.Info(string.Format("TaiwuMaxLoadMod GetSetting I_MaxWarehouseLoad {0}", I_MaxWarehouseLoad));

            if (I_MaxInventory > 0) HarmonyInstance.PatchAll(typeof(MaxInventory));
            if (I_MaxResourceCount > 0) HarmonyInstance.PatchAll(typeof(MaxResourceCount));
            if (I_MaxWarehouseLoad > 0) HarmonyInstance.PatchAll(typeof(MaxWarehouseLoad));

        }
        public override void Initialize()
        {
            this.OnModSettingUpdate();
        }
        //人物负重
        [HarmonyPatch(typeof(GameData.Domains.Character.Character), "GetMaxInventoryLoad")]
        public class MaxInventory
        {
            public static void Postfix(GameData.Domains.Character.Character __instance, ref int __result)
            {
                if (__instance.GetId() == DomainManager.Taiwu.GetTaiwuCharId())
                {
                    __result += I_MaxInventory * 1000;
                }
            }
        }
        //资源上限
        [HarmonyPatch(typeof(TaiwuDomain), "CalcMaterialResourceMaxCount")]
        public class MaxResourceCount
        {
            public static void Postfix(ref int __result)
            {
                __result += I_MaxResourceCount;
            }
        }

        //仓库上限
        [HarmonyPatch(typeof(TaiwuDomain), "CalcWarehouseMaxLoad")]
        public class MaxWarehouseLoad
        {
            public static void Postfix(ref int __result)
            {
                __result += I_MaxWarehouseLoad * 1000;
            }
        }

    }
}