using System;
using UnityEngine;
using UnityEngine.UI;

namespace ArcanesGroupFrames
{
    internal static class RaidFramesOptionsUI
    {
        private const float PanelWidth = 390f;
        private const float PanelHeight = 540f;
        private const float Margin = 12f;
        private const float RowHeight = 32f;
        private const float SliderRowHeight = 48f;
        private const float Gap = 7f;

        private static GameObject _canvasObject;
        private static GameObject _panelObject;
        private static RectTransform _panelRect;

        private static Text _lockText;
        private static Text _hpPercentText;
        private static Text _rawHealthText;
        private static Text _classColorText;

        private static Text _frameWidthLabel;
        private static Text _frameHeightLabel;
        private static Text _memberSpacingLabel;
        private static Text _groupSpacingLabel;

        private static Slider _frameWidthSlider;
        private static Slider _frameHeightSlider;
        private static Slider _memberSpacingSlider;
        private static Slider _groupSpacingSlider;

        internal static void Toggle()
        {
            EnsureCreated();

            if (_panelObject == null)
            {
                return;
            }

            bool show = !_panelObject.activeSelf;
            _panelObject.SetActive(show);

            if (show)
            {
                RefreshAllControls();
            }
        }

        private static void EnsureCreated()
        {
            if (_canvasObject != null && _panelObject != null)
            {
                return;
            }

            CreateCanvas();
            CreatePanel();
        }

        private static void CreateCanvas()
        {
            _canvasObject =
                new GameObject("ArcanesGroupFrames_OptionsCanvas");

            UnityEngine.Object.DontDestroyOnLoad(_canvasObject);

            Canvas canvas = _canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 700;

            CanvasScaler scaler = _canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;

            _canvasObject.AddComponent<GraphicRaycaster>();
        }

        private static void CreatePanel()
        {
            _panelObject = new GameObject("RaidFramesOptionsPanel");
            _panelObject.transform.SetParent(_canvasObject.transform, false);

            _panelRect = _panelObject.AddComponent<RectTransform>();
            _panelRect.anchorMin = new Vector2(0.5f, 0.5f);
            _panelRect.anchorMax = new Vector2(0.5f, 0.5f);
            _panelRect.pivot = new Vector2(0.5f, 0.5f);
            _panelRect.anchoredPosition = Vector2.zero;
            _panelRect.sizeDelta = new Vector2(PanelWidth, PanelHeight);

            Image background = _panelObject.AddComponent<Image>();
            background.color = new Color(0.02f, 0.035f, 0.045f, 0.985f);

            float y = -Margin;

            CreateTitleRow(ref y);
            CreateSectionLabel("General", ref y);
            _lockText = CreateToggleRow(ref y, ToggleLock);
            _hpPercentText = CreateToggleRow(ref y, ToggleHpPercent);
            _rawHealthText = CreateToggleRow(ref y, ToggleRawHp);
            _classColorText = CreateToggleRow(ref y, ToggleClassColors);

            CreateSectionLabel("Layout", ref y);
            CreateSliderRow(
                "Frame Width",
                90f,
                240f,
                ref y,
                out _frameWidthLabel,
                out _frameWidthSlider,
                value =>
                {
                    RaidFramesSettings.SetFrameWidth(value);
                    RaidFramesUI.SettingsChanged();
                    RefreshAllControls();
                });

            CreateSliderRow(
                "Frame Height",
                24f,
                70f,
                ref y,
                out _frameHeightLabel,
                out _frameHeightSlider,
                value =>
                {
                    RaidFramesSettings.SetFrameHeight(value);
                    RaidFramesUI.SettingsChanged();
                    RefreshAllControls();
                });

            CreateSliderRow(
                "Member Spacing",
                0f,
                10f,
                ref y,
                out _memberSpacingLabel,
                out _memberSpacingSlider,
                value =>
                {
                    RaidFramesSettings.SetMemberSpacing(value);
                    RaidFramesUI.SettingsChanged();
                    RefreshAllControls();
                });

            CreateSliderRow(
                "Group Spacing",
                0f,
                25f,
                ref y,
                out _groupSpacingLabel,
                out _groupSpacingSlider,
                value =>
                {
                    RaidFramesSettings.SetGroupSpacing(value);
                    RaidFramesUI.SettingsChanged();
                    RefreshAllControls();
                });

            CreateBottomButtons(ref y);

            _panelObject.SetActive(false);
        }

        private static void CreateTitleRow(ref float y)
        {
            GameObject row = CreateRow("TitleRow", y, 34f);
            y -= 34f + Gap;

            Text title = CreateText(row.transform, "Arcane's Group Frames", 17, TextAnchor.MiddleLeft);
            title.rectTransform.offsetMin = new Vector2(8f, 0f);
            title.rectTransform.offsetMax = new Vector2(-70f, 0f);

            CreateButton(
                row.transform,
                "Close",
                new Vector2(-62f, 3f),
                new Vector2(58f, 28f),
                Toggle,
                true);
        }

        private static void CreateSectionLabel(string text, ref float y)
        {
            GameObject row = CreateRow("Section_" + text, y, 22f);
            y -= 22f + 3f;

            Text label = CreateText(row.transform, text, 12, TextAnchor.MiddleLeft);
            label.color = new Color(0.72f, 0.88f, 0.92f, 1f);
            label.rectTransform.offsetMin = new Vector2(4f, 0f);
            label.rectTransform.offsetMax = Vector2.zero;
        }

        private static Text CreateToggleRow(ref float y, Action action)
        {
            GameObject row = CreateRow("ToggleRow", y, RowHeight);
            y -= RowHeight + Gap;

            Image image = row.AddComponent<Image>();
            image.color = new Color(0.055f, 0.135f, 0.16f, 1f);

            Button button = row.AddComponent<Button>();
            button.targetGraphic = image;
            button.onClick.AddListener(() => action());

            Text text = CreateText(row.transform, "", 12, TextAnchor.MiddleCenter);
            text.raycastTarget = false;
            return text;
        }

        private static void CreateSliderRow(
            string title,
            float min,
            float max,
            ref float y,
            out Text label,
            out Slider slider,
            Action<float> changed)
        {
            GameObject row = CreateRow(title + "Row", y, SliderRowHeight);
            y -= SliderRowHeight + Gap;

            GameObject labelObject = new GameObject("Label");
            labelObject.transform.SetParent(row.transform, false);
            RectTransform labelRect = labelObject.AddComponent<RectTransform>();
            labelRect.anchorMin = new Vector2(0f, 1f);
            labelRect.anchorMax = new Vector2(1f, 1f);
            labelRect.pivot = new Vector2(0.5f, 1f);
            labelRect.anchoredPosition = new Vector2(0f, -1f);
            labelRect.sizeDelta = new Vector2(-8f, 18f);

            label = labelObject.AddComponent<Text>();
            label.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            label.fontSize = 11;
            label.alignment = TextAnchor.MiddleLeft;
            label.color = Color.white;
            label.raycastTarget = false;

            GameObject sliderObject = new GameObject("Slider");
            sliderObject.transform.SetParent(row.transform, false);
            RectTransform sliderRect = sliderObject.AddComponent<RectTransform>();
            sliderRect.anchorMin = new Vector2(0f, 0f);
            sliderRect.anchorMax = new Vector2(1f, 0f);
            sliderRect.pivot = new Vector2(0.5f, 0f);
            sliderRect.anchoredPosition = new Vector2(0f, 4f);
            sliderRect.sizeDelta = new Vector2(-12f, 20f);

            slider = sliderObject.AddComponent<Slider>();
            slider.minValue = min;
            slider.maxValue = max;
            slider.wholeNumbers = true;

            CreateSliderGraphics(sliderObject, slider);
            slider.onValueChanged.AddListener(value => changed(value));
        }

        private static void CreateBottomButtons(ref float y)
        {
            GameObject row = CreateRow("BottomButtons", y, 32f);
            y -= 32f;

            CreateButton(
                row.transform,
                "Reset Defaults",
                new Vector2(0f, 0f),
                new Vector2(145f, 32f),
                () =>
                {
                    RaidFramesSettings.ResetDefaults();
                    RaidFramesUI.SettingsChanged();
                    RefreshAllControls();
                },
                false);

            CreateButton(
                row.transform,
                "Close",
                new Vector2(153f, 0f),
                new Vector2(90f, 32f),
                Toggle,
                false);
        }

        private static GameObject CreateRow(string name, float y, float height)
        {
            GameObject row = new GameObject(name);
            row.transform.SetParent(_panelObject.transform, false);

            RectTransform rect = row.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(1f, 1f);
            rect.pivot = new Vector2(0.5f, 1f);
            rect.anchoredPosition = new Vector2(0f, y);
            rect.sizeDelta = new Vector2(-(Margin * 2f), height);
            return row;
        }

        private static Button CreateButton(
            Transform parent,
            string label,
            Vector2 anchoredPosition,
            Vector2 size,
            Action action,
            bool anchorRight)
        {
            GameObject obj = new GameObject(label + "Button");
            obj.transform.SetParent(parent, false);

            RectTransform rect = obj.AddComponent<RectTransform>();
            if (anchorRight)
            {
                rect.anchorMin = new Vector2(1f, 0.5f);
                rect.anchorMax = new Vector2(1f, 0.5f);
                rect.pivot = new Vector2(1f, 0.5f);
                rect.anchoredPosition = anchoredPosition;
            }
            else
            {
                rect.anchorMin = new Vector2(0f, 0.5f);
                rect.anchorMax = new Vector2(0f, 0.5f);
                rect.pivot = new Vector2(0f, 0.5f);
                rect.anchoredPosition = anchoredPosition;
            }
            rect.sizeDelta = size;

            Image image = obj.AddComponent<Image>();
            image.color = new Color(0.055f, 0.135f, 0.16f, 1f);

            Button button = obj.AddComponent<Button>();
            button.targetGraphic = image;
            if (action != null)
            {
                button.onClick.AddListener(() => action());
            }

            Text text = CreateText(obj.transform, label, 11, TextAnchor.MiddleCenter);
            text.raycastTarget = false;
            return button;
        }

        private static Text CreateText(
            Transform parent,
            string value,
            int fontSize,
            TextAnchor anchor)
        {
            GameObject obj = new GameObject("Text");
            obj.transform.SetParent(parent, false);

            RectTransform rect = obj.AddComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            Text text = obj.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.fontSize = fontSize;
            text.alignment = anchor;
            text.color = Color.white;
            text.text = value;
            text.raycastTarget = false;
            return text;
        }

        private static void CreateSliderGraphics(GameObject sliderObject, Slider slider)
        {
            GameObject track = new GameObject("Track");
            track.transform.SetParent(sliderObject.transform, false);
            RectTransform trackRect = track.AddComponent<RectTransform>();
            trackRect.anchorMin = new Vector2(0f, 0.5f);
            trackRect.anchorMax = new Vector2(1f, 0.5f);
            trackRect.sizeDelta = new Vector2(-10f, 4f);
            Image trackImage = track.AddComponent<Image>();
            trackImage.color = new Color(0.10f, 0.15f, 0.17f, 1f);

            GameObject handleArea = new GameObject("Handle Area");
            handleArea.transform.SetParent(sliderObject.transform, false);
            RectTransform areaRect = handleArea.AddComponent<RectTransform>();
            areaRect.anchorMin = Vector2.zero;
            areaRect.anchorMax = Vector2.one;
            areaRect.offsetMin = new Vector2(6f, 0f);
            areaRect.offsetMax = new Vector2(-6f, 0f);

            GameObject handle = new GameObject("Handle");
            handle.transform.SetParent(handleArea.transform, false);
            RectTransform handleRect = handle.AddComponent<RectTransform>();
            handleRect.anchorMin = new Vector2(0f, 0.5f);
            handleRect.anchorMax = new Vector2(0f, 0.5f);
            handleRect.pivot = new Vector2(0.5f, 0.5f);
            handleRect.sizeDelta = new Vector2(9f, 14f);

            Image handleImage = handle.AddComponent<Image>();
            handleImage.color = Color.white;

            slider.targetGraphic = handleImage;
            slider.handleRect = handleRect;
            slider.fillRect = null;
        }

        private static void ToggleLock()
        {
            RaidFramesUI.ToggleLocked();
            RefreshAllControls();
        }

        private static void ToggleHpPercent()
        {
            RaidFramesSettings.SetShowHpPercent(!RaidFramesSettings.ShowHpPercent);
            RefreshAllControls();
        }

        private static void ToggleRawHp()
        {
            RaidFramesSettings.SetShowRawHealth(!RaidFramesSettings.ShowRawHealth);
            RefreshAllControls();
        }

        private static void ToggleClassColors()
        {
            RaidFramesSettings.SetUseClassColors(!RaidFramesSettings.UseClassColors);
            RefreshAllControls();
        }

        private static void RefreshAllControls()
        {
            if (_lockText != null)
                _lockText.text = RaidFramesSettings.Locked ? "Frames: LOCKED" : "Frames: UNLOCKED";
            if (_hpPercentText != null)
                _hpPercentText.text = "HP %: " + (RaidFramesSettings.ShowHpPercent ? "ON" : "OFF");
            if (_rawHealthText != null)
                _rawHealthText.text = "Raw HP: " + (RaidFramesSettings.ShowRawHealth ? "ON" : "OFF");
            if (_classColorText != null)
                _classColorText.text = "Class Colors: " + (RaidFramesSettings.UseClassColors ? "ON" : "OFF");

            SetSlider(_frameWidthSlider, RaidFramesSettings.FrameWidth);
            SetSlider(_frameHeightSlider, RaidFramesSettings.FrameHeight);
            SetSlider(_memberSpacingSlider, RaidFramesSettings.MemberSpacing);
            SetSlider(_groupSpacingSlider, RaidFramesSettings.GroupSpacing);

            if (_frameWidthLabel != null) _frameWidthLabel.text = "Frame Width: " + RaidFramesSettings.FrameWidth.ToString("F0");
            if (_frameHeightLabel != null) _frameHeightLabel.text = "Frame Height: " + RaidFramesSettings.FrameHeight.ToString("F0");
            if (_memberSpacingLabel != null) _memberSpacingLabel.text = "Member Spacing: " + RaidFramesSettings.MemberSpacing.ToString("F0");
            if (_groupSpacingLabel != null) _groupSpacingLabel.text = "Group Spacing: " + RaidFramesSettings.GroupSpacing.ToString("F0");
        }

        private static void SetSlider(Slider slider, float value)
        {
            if (slider != null)
            {
                slider.SetValueWithoutNotify(value);
            }
        }

        internal static void Shutdown()
        {
            if (_canvasObject != null)
            {
                UnityEngine.Object.Destroy(_canvasObject);
            }

            _canvasObject = null;
            _panelObject = null;
            _panelRect = null;
            _lockText = null;
            _hpPercentText = null;
            _rawHealthText = null;
            _classColorText = null;
            _frameWidthLabel = null;
            _frameHeightLabel = null;
            _memberSpacingLabel = null;
            _groupSpacingLabel = null;
            _frameWidthSlider = null;
            _frameHeightSlider = null;
            _memberSpacingSlider = null;
            _groupSpacingSlider = null;
        }
    }
}
