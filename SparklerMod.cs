using Modding;
using Modding.Menu;
using Modding.Menu.Config;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UObject = UnityEngine.Object;

namespace HK_Sparkler
{
    public class HK_Sparkler : Mod, ITogglableMod, ICustomMenuMod
    {
        internal static HK_Sparkler Instance;

        public HK_Sparkler() : base("hk-sparkler")
        {
            Instance = this;
        }

        public bool ToggleButtonInsideMenu => true;

        public MenuScreen GetMenuScreen(MenuScreen modListMenu, ModToggleDelegates? toggleDelegates)
        {
            return ModMenu.CreateMenuScreen(modListMenu).Build();
        }

        public override string GetVersion() => "0.2.0";

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            Log("Initializing");

            Instance = this;

            Log("Initialized");
        }

        public void Unload() { }
    }
}