using UnityEngine;
using UnityEngine.UI;

namespace ArcanesGroupFrames
{
    internal static class RaidManagerToggleButton
    {
        private static GameObject _canvasObject;

        private static GameObject _buttonObject;


        private static RectTransform _buttonRect;

        private static Button _button;

        private static Text _buttonText;


        // ============================================================
        // RAID STATE
        // ============================================================

        private static bool _lastRaidActive;


        // ============================================================
        // DEFAULT POSITION
        // ============================================================

        private static Vector2 _savedPosition =
            new Vector2(
                -115f,
                -245f);


        // ============================================================
        // INITIALIZE
        // ============================================================

        internal static void Initialize()
        {
            if (_canvasObject != null)
            {
                return;
            }


            CreateCanvas();

            CreateButton();

            UpdateButtonText();


            _lastRaidActive =
                GameData.RaidActive;


            SetButtonVisible(
                _lastRaidActive);


            Plugin.LogInfo(
                "Raid Manager toggle button initialized.");
        }


        // ============================================================
        // CANVAS
        // ============================================================

        private static void CreateCanvas()
        {
            _canvasObject =
                new GameObject(
                    "ArcanesGroupFrames_Canvas");


            Object.DontDestroyOnLoad(
                _canvasObject);


            Canvas canvas =
                _canvasObject
                    .AddComponent<Canvas>();


            canvas.renderMode =
                RenderMode.ScreenSpaceOverlay;


            canvas.sortingOrder =
                500;


            CanvasScaler scaler =
                _canvasObject
                    .AddComponent<CanvasScaler>();


            scaler.uiScaleMode =
                CanvasScaler.ScaleMode
                    .ScaleWithScreenSize;


            scaler.referenceResolution =
                new Vector2(
                    1920f,
                    1080f);


            scaler.matchWidthOrHeight =
                0.5f;


            _canvasObject
                .AddComponent<GraphicRaycaster>();
        }


        // ============================================================
        // BUTTON
        // ============================================================

        private static void CreateButton()
        {
            _buttonObject =
                new GameObject(
                    "RaidManagerToggleButton");


            _buttonObject
                .transform
                .SetParent(
                    _canvasObject.transform,
                    false);


            _buttonRect =
                _buttonObject
                    .AddComponent<RectTransform>();


            // Top-right anchor so the default position sits
            // beneath the minimap.

            _buttonRect.anchorMin =
                new Vector2(
                    1f,
                    1f);

            _buttonRect.anchorMax =
                new Vector2(
                    1f,
                    1f);


            _buttonRect.pivot =
                new Vector2(
                    0.5f,
                    0.5f);


            _buttonRect.anchoredPosition =
                _savedPosition;


            _buttonRect.sizeDelta =
                new Vector2(
                    180f,
                    34f);


            Image background =
                _buttonObject
                    .AddComponent<Image>();


            background.color =
                new Color(
                    0.08f,
                    0.08f,
                    0.08f,
                    0.90f);


            _button =
                _buttonObject
                    .AddComponent<Button>();


            _button.targetGraphic =
                background;


            _button
                .onClick
                .AddListener(
                    OnButtonClicked);


            _buttonObject
                .AddComponent<RaidManagerToggleDragHandler>();


            CreateButtonText();
        }


        // ============================================================
        // TEXT
        // ============================================================

        private static void CreateButtonText()
        {
            GameObject textObject =
                new GameObject(
                    "Text");


            textObject
                .transform
                .SetParent(
                    _buttonObject.transform,
                    false);


            RectTransform rect =
                textObject
                    .AddComponent<RectTransform>();


            rect.anchorMin =
                Vector2.zero;

            rect.anchorMax =
                Vector2.one;


            rect.offsetMin =
                Vector2.zero;

            rect.offsetMax =
                Vector2.zero;


            _buttonText =
                textObject
                    .AddComponent<Text>();


            _buttonText.font =
                Resources
                    .GetBuiltinResource<Font>(
                        "Arial.ttf");


            _buttonText.fontSize =
                15;


            _buttonText.alignment =
                TextAnchor.MiddleCenter;


            _buttonText.color =
                Color.white;


            _buttonText.raycastTarget =
                false;
        }


        // ============================================================
        // RAID STATE
        // ============================================================

        internal static void UpdateVisibility()
        {
            if (_buttonObject == null)
            {
                return;
            }


            bool raidActive =
                GameData.RaidActive;


            if (raidActive ==
                _lastRaidActive)
            {
                return;
            }


            _lastRaidActive =
                raidActive;


            SetButtonVisible(
                raidActive);


            if (raidActive)
            {
                Plugin.LogInfo(
                    "Raid started. " +
                    "Raid Frames toggle button shown.");
            }
            else
            {
                Plugin.LogInfo(
                    "Raid ended. " +
                    "Raid Frames toggle button hidden.");


                RaidManagerVisibility
                    .SetVisible(
                        true);


                UpdateButtonText();
            }
        }


        // ============================================================
        // BUTTON VISIBILITY
        // ============================================================

        private static void SetButtonVisible(
            bool visible)
        {
            if (_buttonObject == null)
            {
                return;
            }


            if (_buttonObject.activeSelf ==
                visible)
            {
                return;
            }


            _buttonObject.SetActive(
                visible);
        }


        // ============================================================
        // CLICK
        // ============================================================

        private static void OnButtonClicked()
        {
            RaidManagerVisibility
                .Toggle();


            UpdateButtonText();
        }


        // ============================================================
        // POSITION
        // ============================================================

        internal static void StoreCurrentPosition()
        {
            if (_buttonRect == null)
            {
                return;
            }


            _savedPosition =
                _buttonRect
                    .anchoredPosition;


            Plugin.LogInfo(
                $"Raid Manager button position: " +
                $"X={_savedPosition.x:F1}, " +
                $"Y={_savedPosition.y:F1}");
        }


        // ============================================================
        // TEXT REFRESH
        // ============================================================

        internal static void UpdateButtonText()
        {
            if (_buttonText == null)
            {
                return;
            }


            _buttonText.text =
                RaidManagerVisibility.IsVisible
                    ? "Hide Raid Frames"
                    : "Show Raid Frames";
        }


        // ============================================================
        // SHUTDOWN
        // ============================================================

        internal static void Shutdown()
        {
            if (_canvasObject != null)
            {
                Object.Destroy(
                    _canvasObject);
            }


            _buttonObject =
                null;

            _buttonRect =
                null;

            _button =
                null;

            _buttonText =
                null;

            _canvasObject =
                null;


            _lastRaidActive =
                false;
        }
    }
}