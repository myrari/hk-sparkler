using Modding;
using Modding.Menu;
using Modding.Menu.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace HK_Sparkler
{
    public static class ModMenu
    {
        public struct TextInputPanelConfig
        {
            public string Text;
            public int Size;
            public TextAnchor Anchor;
        }

        public static ContentArea AddTextInputPanel(
            this ContentArea content,
            string name,
            RelVector2 size,
            TextInputPanelConfig config,
            out InputField inputField
        )
        {
            content.AddStaticPanel(name, size, out var go);

            var text = go.AddComponent<Text>();
            text.fontSize = config.Size;
            text.font = MenuResources.TrajanRegular;
            text.alignment = config.Anchor;
            text.supportRichText = true;

            inputField = go.AddComponent<InputField>();
            inputField.text = config.Text;
            inputField.textComponent = text;

            content.NavGraph.AddNavigationNode(inputField);

            return content;
        }

        public static MenuBuilder CreateMenuScreen(MenuScreen modListMenu)
        {
            SimpleLogger logger = new SimpleLogger("hk-sparkler");

            Action<MenuSelectable> goBack = selectable => UIManager.instance.UIGoToDynamicMenu(modListMenu);
            return new MenuBuilder(UIManager.instance.UICanvas.gameObject, "Sparkler")
                .CreateTitle("Sparkler", MenuTitleStyle.vanillaStyle)
                .CreateContentPane(
                    RectTransformData.FromSizeAndPos(
                        new RelVector2(new Vector2(1920f, 903f)),
                        new AnchoredPosition(
                            new Vector2(0.5f, 0.5f),
                            new Vector2(0.5f, 0.5f),
                            new Vector2(0f, -60f)
                        )
                    )
                )
                .CreateControlPane(RectTransformData.FromSizeAndPos(
                    new RelVector2(new Vector2(1920f, 259f)),
                    new AnchoredPosition(
                        new Vector2(0.5f, 0.5f),
                        new Vector2(0.5f, 0.5f),
                        new Vector2(0f, -502f)
                    )
                ))
                .SetDefaultNavGraph(new GridNavGraph(1))
                .AddContent(
                    RegularGridLayout.CreateVerticalLayout(105f),
                    c =>
                    {
                        c.AddTextPanel("SparklerPairingCodeLabel", new RelVector2(new Vector2(960, 64f)), new TextPanelConfig
                        {
                            Text = "Input Pairing Code:",
                            Font = TextPanelConfig.TextFont.TrajanBold,
                            Size = 48,
                            Anchor = TextAnchor.MiddleCenter,
                        })
                        .AddTextInputPanel("SparklerPairingCodeInput", new RelVector2(new Vector2(960, 64f)), new TextInputPanelConfig
                        {
                            Text = "...",
                            Size = 32,
                            Anchor = TextAnchor.MiddleCenter,
                        }, out var pairingCodeInput)
                        .AddMenuButton(
                            "SparklerPairingButton",
                            new MenuButtonConfig
                            {
                                Label = "Pair",
                                CancelAction = goBack,
                                Proceed = false,
                                SubmitAction = _ =>
                                {
                                    logger.Log("pairing code: " + pairingCodeInput.text);
                                }
                            }
                        );
                    }
                )
                .AddControls(
                    new SingleContentLayout(
                        new AnchoredPosition(
                            new Vector2(0.5f, 0.5f),
                            new Vector2(0.5f, 0.5f),
                            new Vector2(0f, -64f)
                        )
                    ),
                    c => c.AddMenuButton(
                        "BackButton",
                        new MenuButtonConfig
                        {
                            Label = "Back",
                            CancelAction = goBack,
                            SubmitAction = goBack,
                            Style = MenuButtonStyle.VanillaStyle,
                            Proceed = true,
                        }
                    )
                );
        }
    }
}
