using Modding;
using System;
using System.Collections.Generic;
using System.Net.Http;
using UnityEngine;

namespace HK_Sparkler
{
    public class HK_Sparkler : Mod, ICustomMenuMod
    {
        internal static HK_Sparkler Instance;

        public HttpClient HttpClient;

        public string Secret;

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
            if (Instance == null)
            {
                Log("Initializing");

                HttpClient = new HttpClient
                {
                    BaseAddress = new Uri("https://sparkler.myrari.net"),
                };

                Secret = null;

                Instance = this;

                Log("Initialized");
            } else
            {
                Log("Already initialized!");
            }
        }
    }
}