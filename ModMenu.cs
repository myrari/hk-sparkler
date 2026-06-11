using Modding;
using Modding.Menu;
using Modding.Menu.Config;
using System;
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
            SimpleLogger logger = new("hk-sparkler");

            void goBack(MenuSelectable selectable) => UIManager.instance.UIGoToDynamicMenu(modListMenu);
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
                        c.AddTextPanel("SparklerPairingCodeLabel", new RelVector2(new Vector2(960f, 64f)), new TextPanelConfig
                        {
                            Text = "Input Pairing Code:",
                            Font = TextPanelConfig.TextFont.TrajanBold,
                            Size = 48,
                            Anchor = TextAnchor.MiddleCenter,
                        })
                        .AddTextInputPanel("SparklerPairingCodeInput", new RelVector2(new Vector2(960f, 64f)), new TextInputPanelConfig
                        {
                            Text = "...",
                            Size = 32,
                            Anchor = TextAnchor.MiddleCenter,
                        }, out var pairingCodeInput)
                        .AddTextPanel("SparklerPairingResponseLabel", new RelVector2(new Vector2(960f, 64f)), new TextPanelConfig
                        {
                            Text = "",
                            Font = TextPanelConfig.TextFont.TrajanRegular,
                            Size = 40,
                            Anchor = TextAnchor.MiddleCenter,
                        }, out var responseLabel)
                        .AddMenuButton(
                            "SparklerPairingButton",
                            new MenuButtonConfig
                            {
                                Label = "Pair",
                                CancelAction = goBack,
                                Proceed = false,
                                SubmitAction = _ =>
                                {
                                    responseLabel.StartCoroutine(SparklerAPI.Pair(HK_Sparkler.Instance.HttpClient, pairingCodeInput.text, res =>
                                    {
                                        if (res.error != null)
                                        {
                                            logger.LogError("failed to pair! " + res.error);

                                            responseLabel.text = "Error: " + res.error;
                                        }
                                        else if (res.secret != null)
                                        {
                                            logger.Log("got secret: " + res.secret);

                                            responseLabel.text = "Succesfully paired!";

                                            HK_Sparkler.Instance.Secret = res.secret;
                                        }
                                        else
                                        {
                                            logger.LogError("pairing got no error, but no secret!");

                                            responseLabel.text = "Pairing failed!";
                                        }
                                    }));
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
                            SubmitAction = (Action<MenuSelectable>)goBack,
                            Style = MenuButtonStyle.VanillaStyle,
                            Proceed = true,
                        }
                    )
                );
        }
    }
}
