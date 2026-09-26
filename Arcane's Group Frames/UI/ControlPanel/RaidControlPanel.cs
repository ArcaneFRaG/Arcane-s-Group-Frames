using System;
using UnityEngine;
using UnityEngine.UI;

namespace ArcanesGroupFrames
{
    internal static class RaidControlPanel
    {
        // ============================================================
        // CONSTANTS
        // ============================================================

        private const int RaidGroupCount =
            3;


        private const float PanelWidth =
            382f;

        private const float PanelHeight =
            250f;

        private const float HeaderHeight =
            24f;

        private const float ButtonHeight =
            25f;

        private const float Gap =
            3f;

        private const float Margin =
            5f;


        // ============================================================
        // ROOT OBJECTS
        // ============================================================

        private static GameObject _canvasObject;

        private static GameObject _panelObject;

        private static RectTransform _panelRect;


        // ============================================================
        // POPUPS
        // ============================================================

        private static GameObject _dpsPopup;

        private static GameObject _spreadPopup;


        private static Slider _dpsSlider;

        private static Slider _spreadSlider;


        private static Text _dpsValueText;

        private static Text _spreadValueText;


        // ============================================================
        // GROUP TARGET TEXT
        //
        // Index 0 unused.
        // ============================================================

        private static readonly Text[]
            GroupTargetTexts =
                new Text[RaidGroupCount + 1];


        private static readonly string[]
            LastTargetNames =
                new string[RaidGroupCount + 1];


        // ============================================================
        // STATE
        // ============================================================

        private static bool _lastRaidActive;


        // ============================================================
        // POSITION
        // ============================================================

        private static Vector2 _savedPosition =
            new Vector2(
                8f,
                -8f);


        // ============================================================
        // COLORS
        // ============================================================

        private static readonly Color PanelColor =
            new Color(
                0.018f,
                0.035f,
                0.045f,
                0.97f);


        private static readonly Color HeaderColor =
            new Color(
                0.025f,
                0.075f,
                0.09f,
                1f);


        private static readonly Color ButtonColor =
            new Color(
                0.055f,
                0.135f,
                0.16f,
                1f);


        private static readonly Color GroupColor =
            new Color(
                0.07f,
                0.17f,
                0.20f,
                1f);


        private static readonly Color TargetColor =
            new Color(
                0.035f,
                0.09f,
                0.105f,
                1f);


        private static readonly Color PopupColor =
            new Color(
                0.025f,
                0.055f,
                0.07f,
                0.99f);


        private static readonly Color SliderTrackColor =
            new Color(
                0.12f,
                0.18f,
                0.20f,
                1f);


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

            CreatePanel();


            _lastRaidActive =
                GameData.RaidActive;


            SetVisible(
                _lastRaidActive);


            RefreshTargetLabels(
                true);


}


        // ============================================================
        // CANVAS
        // ============================================================

        private static void CreateCanvas()
        {
            _canvasObject =
                new GameObject(
                    "ArcanesGroupFrames_RaidControlsCanvas");


            UnityEngine.Object
                .DontDestroyOnLoad(
                    _canvasObject);


            Canvas canvas =
                _canvasObject
                    .AddComponent<Canvas>();


            canvas.renderMode =
                RenderMode.ScreenSpaceOverlay;


            canvas.sortingOrder =
                490;


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
        // PANEL
        // ============================================================

        private static void CreatePanel()
        {
            _panelObject =
                new GameObject(
                    "RaidControlPanel");


            _panelObject
                .transform
                .SetParent(
                    _canvasObject.transform,
                    false);


            _panelRect =
                _panelObject
                    .AddComponent<RectTransform>();


            _panelRect.anchorMin =
                new Vector2(
                    0f,
                    1f);


            _panelRect.anchorMax =
                new Vector2(
                    0f,
                    1f);


            _panelRect.pivot =
                new Vector2(
                    0f,
                    1f);


            _panelRect.anchoredPosition =
                _savedPosition;


            _panelRect.sizeDelta =
                new Vector2(
                    PanelWidth,
                    PanelHeight);


            Image background =
                _panelObject
                    .AddComponent<Image>();


            background.color =
                PanelColor;


            CreateHeader();

            CreateGlobalRows();

            CreateGroupRows();
        }


        // ============================================================
        // HEADER
        // ============================================================

        private static void CreateHeader()
        {
            GameObject header =
                CreateBox(
                    "Header",
                    0f,
                    0f,
                    PanelWidth,
                    HeaderHeight,
                    HeaderColor);


            // --------------------------------------------------------
            // TITLE
            // --------------------------------------------------------

            CreateText(
                header.transform,
                "Raid Controls",
                14,
                TextAnchor.MiddleLeft,
                7f);


            // --------------------------------------------------------
            // OPTIONS BUTTON
            // --------------------------------------------------------

            GameObject optionsObject =
                new GameObject(
                    "OptionsButton");


            optionsObject.transform
                .SetParent(
                    header.transform,
                    false);


            RectTransform optionsRect =
                optionsObject
                    .AddComponent<RectTransform>();


            optionsRect.anchorMin =
                new Vector2(
                    1f,
                    0f);


            optionsRect.anchorMax =
                new Vector2(
                    1f,
                    1f);


            optionsRect.pivot =
                new Vector2(
                    1f,
                    0.5f);


            optionsRect.anchoredPosition =
                new Vector2(
                    -3f,
                    0f);


            optionsRect.sizeDelta =
                new Vector2(
                    58f,
                    -4f);


            Image optionsImage =
                optionsObject
                    .AddComponent<Image>();


            optionsImage.color =
                ButtonColor;


            Button optionsButton =
                optionsObject
                    .AddComponent<Button>();


            optionsButton.targetGraphic =
                optionsImage;


            optionsButton.onClick.AddListener(
                RaidFramesOptionsUI.Toggle);


            CreateText(
                optionsObject.transform,
                "Options",
                11,
                TextAnchor.MiddleCenter,
                0f);


            // --------------------------------------------------------
            // PANEL DRAGGING
            // --------------------------------------------------------

            RaidControlPanelDragHandler drag =
                header
                    .AddComponent<RaidControlPanelDragHandler>();


            drag.Initialize(
                _panelRect);
        }


        // ============================================================
        // GLOBAL ROWS
        // ============================================================

        private static void CreateGlobalRows()
        {
            float y =
                -(HeaderHeight + Gap);


            const float width =
                72f;


            // ========================================================
            // ROW 1
            // ========================================================

            CreateButton(
                "Tanks Engage",
                Margin,
                y,
                width,
                ButtonHeight,
                NativeRaidControlProxy.TanksEngage);


            CreateButton(
                "All Attack",
                Margin + (width + Gap),
                y,
                width,
                ButtonHeight,
                RaidGroupCommands.AllAttack);


            CreateButton(
                "All Pull",
                Margin + ((width + Gap) * 2f),
                y,
                width,
                ButtonHeight,
                RaidGroupCommands.AllPull);


            Button dpsButton =
                CreateButton(
                    "Raid DPS",
                    Margin + ((width + Gap) * 3f),
                    y,
                    width,
                    ButtonHeight,
                    ToggleDpsPopup);


            Button spreadButton =
                CreateButton(
                    "Spread",
                    Margin + ((width + Gap) * 4f),
                    y,
                    width,
                    ButtonHeight,
                    ToggleSpreadPopup);


            // ========================================================
            // SLIDER POPUPS
            // ========================================================

            _dpsPopup =
                CreateSliderPopup(
                    "RaidDpsPopup",
                    dpsButton.GetComponent<RectTransform>(),
                    "Raid DPS",
                    0f,
                    100f,
                    100f,
                    true,
                    true,
                    OnDpsChanged,
                    out _dpsSlider,
                    out _dpsValueText);


            _spreadPopup =
                CreateSliderPopup(
                    "RaidSpreadPopup",
                    spreadButton.GetComponent<RectTransform>(),
                    "Spread",
                    0f,
                    20f,
                    0f,
                    true,
                    false,
                    OnSpreadChanged,
                    out _spreadSlider,
                    out _spreadValueText);


            _dpsPopup.SetActive(
                false);


            _spreadPopup.SetActive(
                false);


            // ========================================================
            // ROW 2
            // ========================================================

            y -=
                ButtonHeight +
                Gap;


            CreateButton(
                "Burn Target",
                Margin,
                y,
                width,
                ButtonHeight,
                NativeRaidControlProxy.BurnTarget);


            CreateButton(
                "Invite Raider",
                Margin + (width + Gap),
                y,
                width,
                ButtonHeight,
                NativeRaidControlProxy.InviteRaider);


            CreateButton(
                "Save",
                Margin + ((width + Gap) * 2f),
                y,
                width,
                ButtonHeight,
                NativeRaidControlProxy.SaveRaid);


            CreateButton(
                "Load",
                Margin + ((width + Gap) * 3f),
                y,
                width,
                ButtonHeight,
                NativeRaidControlProxy.LoadRaid);


            CreateButton(
                "Loot Dist.",
                Margin + ((width + Gap) * 4f),
                y,
                width,
                ButtonHeight,
                NativeRaidControlProxy.LootDistribution);
        }


        // ============================================================
        // GROUP ROWS
        // ============================================================

        private static void CreateGroupRows()
        {
            float y =
                -(HeaderHeight +
                  Gap +
                  ButtonHeight +
                  Gap +
                  ButtonHeight +
                  10f);


            for (int group = 1;
                 group <= RaidGroupCount;
                 group++)
            {
                CreateGroupBlock(
                    group,
                    y);


                y -=
                    54f;
            }
        }


        // ============================================================
        // GROUP BLOCK
        // ============================================================

        private static void CreateGroupBlock(
            int group,
            float y)
        {
            const float labelWidth =
                48f;

            const float assignWidth =
                95f;

            const float commandWidth =
                72f;


            // ========================================================
            // TOP ROW
            // ========================================================

            GameObject label =
                CreateBox(
                    $"Group{group}",
                    Margin,
                    y,
                    labelWidth,
                    ButtonHeight,
                    GroupColor);


            CreateText(
                label.transform,
                $"GRP {group}",
                13,
                TextAnchor.MiddleCenter,
                0f);


            float x =
                Margin +
                labelWidth +
                Gap;


            // --------------------------------------------------------
            // ASSIGN TARGET
            // --------------------------------------------------------

            CreateButton(
                "Assign Targ.",
                x,
                y,
                assignWidth,
                ButtonHeight,
                () =>
                {
                    RaidGroupCommands
                        .AssignTarget(
                            group);


                    RefreshTargetLabel(
                        group,
                        true);
                });


            x +=
                assignWidth +
                Gap;


            // --------------------------------------------------------
            // ATTACK
            // --------------------------------------------------------

            CreateButton(
                "Attack",
                x,
                y,
                commandWidth,
                ButtonHeight,
                () =>
                    RaidGroupCommands
                        .Attack(
                            group));


            x +=
                commandWidth +
                Gap;


            // --------------------------------------------------------
            // PULL
            // --------------------------------------------------------

            CreateButton(
                "Pull",
                x,
                y,
                commandWidth,
                ButtonHeight,
                () =>
                    RaidGroupCommands
                        .Pull(
                            group));


            // ========================================================
            // SECOND ROW
            // ========================================================

            float row2 =
                y -
                ButtonHeight -
                Gap;


            float secondX =
                Margin +
                labelWidth +
                Gap;


            // --------------------------------------------------------
            // FOLLOW
            // --------------------------------------------------------

            CreateButton(
                "Follow",
                secondX,
                row2,
                assignWidth,
                ButtonHeight,
                () =>
                    RaidGroupCommands
                        .Follow(
                            group));


            secondX +=
                assignWidth +
                Gap;


            // --------------------------------------------------------
            // HERE
            // --------------------------------------------------------

            CreateButton(
                "Here!",
                secondX,
                row2,
                commandWidth,
                ButtonHeight,
                () =>
                    RaidGroupCommands
                        .Here(
                            group));


            secondX +=
                commandWidth +
                Gap;


            // ========================================================
            // ASSIGNED TARGET DISPLAY
            // ========================================================

            float remainingWidth =
                PanelWidth -
                Margin -
                secondX;


            GameObject targetBox =
                CreateBox(
                    $"Group{group}_Target",
                    secondX,
                    row2,
                    remainingWidth,
                    ButtonHeight,
                    TargetColor);


            Text targetText =
                CreateText(
                    targetBox.transform,
                    "Target: None",
                    11,
                    TextAnchor.MiddleLeft,
                    5f);


            targetText.horizontalOverflow =
                HorizontalWrapMode.Wrap;


            targetText.verticalOverflow =
                VerticalWrapMode.Truncate;


            GroupTargetTexts[group] =
                targetText;


            LastTargetNames[group] =
                null;
        }


        // ============================================================
        // TARGET LABEL REFRESH
        // ============================================================

        private static void RefreshTargetLabels(
            bool force)
        {
            for (int group = 1;
                 group <= RaidGroupCount;
                 group++)
            {
                RefreshTargetLabel(
                    group,
                    force);
            }
        }


        private static void RefreshTargetLabel(
            int group,
            bool force)
        {
            if (group < 1 ||
                group > RaidGroupCount)
            {
                return;
            }


            Text targetText =
                GroupTargetTexts[group];


            if (targetText == null)
            {
                return;
            }


            string name =
                RaidGroupCommands
                    .GetAssignedTargetName(
                        group);


            if (!force &&
                LastTargetNames[group] ==
                name)
            {
                return;
            }


            LastTargetNames[group] =
                name;


            targetText.text =
                "Target: " +
                name;
        }


        // ============================================================
        // DPS POPUP
        // ============================================================

        private static void ToggleDpsPopup()
        {
            if (_dpsPopup == null)
            {
                return;
            }


            bool show =
                !_dpsPopup.activeSelf;


            ClosePopups();


            if (show)
            {
                _dpsPopup.SetActive(
                    true);


                _dpsPopup.transform
                    .SetAsLastSibling();
            }
        }


        private static void OnDpsChanged(
            float value)
        {
            value =
                Mathf.Round(
                    value);


            if (_dpsValueText != null)
            {
                _dpsValueText.text =
                    $"{value:F0}%";
            }


            RaidGroupCommands
                .SetRaidDps(
                    value);
        }


        // ============================================================
        // SPREAD POPUP
        // ============================================================

        private static void ToggleSpreadPopup()
        {
            if (_spreadPopup == null)
            {
                return;
            }


            bool show =
                !_spreadPopup.activeSelf;


            ClosePopups();


            if (show)
            {
                _spreadPopup.SetActive(
                    true);


                _spreadPopup.transform
                    .SetAsLastSibling();
            }
        }


        private static void OnSpreadChanged(
            float value)
        {
            value =
                Mathf.Round(
                    value);


            if (_spreadValueText != null)
            {
                _spreadValueText.text =
                    value.ToString(
                        "F0");
            }


            RaidGroupCommands
                .SetRaidSpread(
                    value);
        }


        // ============================================================
        // CLOSE POPUPS
        // ============================================================

        private static void ClosePopups()
        {
            if (_dpsPopup != null)
            {
                _dpsPopup.SetActive(
                    false);
            }


            if (_spreadPopup != null)
            {
                _spreadPopup.SetActive(
                    false);
            }
        }


        // ============================================================
        // SLIDER POPUP FACTORY
        // ============================================================

        private static GameObject CreateSliderPopup(
            string name,
            RectTransform sourceButton,
            string title,
            float min,
            float max,
            float initialValue,
            bool wholeNumbers,
            bool displayPercent,
            Action<float> changed,
            out Slider slider,
            out Text valueText)
        {
            // ========================================================
            // ROOT
            // ========================================================

            GameObject popup =
                new GameObject(
                    name);


            popup.transform
                .SetParent(
                    _panelRect,
                    false);


            RectTransform popupRect =
                popup
                    .AddComponent<RectTransform>();


            popupRect.anchorMin =
                new Vector2(
                    0f,
                    1f);


            popupRect.anchorMax =
                new Vector2(
                    0f,
                    1f);


            popupRect.pivot =
                new Vector2(
                    0f,
                    1f);


            Vector2 sourcePosition =
                sourceButton
                    .anchoredPosition;


            popupRect.anchoredPosition =
                new Vector2(
                    sourcePosition.x,
                    sourcePosition.y -
                    ButtonHeight -
                    2f);


            popupRect.sizeDelta =
                new Vector2(
                    135f,
                    48f);


            Image popupImage =
                popup
                    .AddComponent<Image>();


            popupImage.color =
                PopupColor;


            // ========================================================
            // TITLE
            // ========================================================

            GameObject titleObject =
                new GameObject(
                    "Title");


            titleObject.transform
                .SetParent(
                    popup.transform,
                    false);


            RectTransform titleRect =
                titleObject
                    .AddComponent<RectTransform>();


            titleRect.anchorMin =
                new Vector2(
                    0f,
                    1f);


            titleRect.anchorMax =
                new Vector2(
                    1f,
                    1f);


            titleRect.pivot =
                new Vector2(
                    0.5f,
                    1f);


            titleRect.anchoredPosition =
                Vector2.zero;


            titleRect.sizeDelta =
                new Vector2(
                    0f,
                    18f);


            titleRect.offsetMin =
                new Vector2(
                    5f,
                    titleRect.offsetMin.y);


            titleRect.offsetMax =
                new Vector2(
                    -42f,
                    titleRect.offsetMax.y);


            Text titleText =
                titleObject
                    .AddComponent<Text>();


            titleText.font =
                Resources
                    .GetBuiltinResource<Font>(
                        "Arial.ttf");


            titleText.fontSize =
                11;


            titleText.alignment =
                TextAnchor.MiddleLeft;


            titleText.color =
                Color.white;


            titleText.text =
                title;


            titleText.raycastTarget =
                false;


            // ========================================================
            // VALUE
            // ========================================================

            GameObject valueObject =
                new GameObject(
                    "Value");


            valueObject.transform
                .SetParent(
                    popup.transform,
                    false);


            RectTransform valueRect =
                valueObject
                    .AddComponent<RectTransform>();


            valueRect.anchorMin =
                new Vector2(
                    1f,
                    1f);


            valueRect.anchorMax =
                new Vector2(
                    1f,
                    1f);


            valueRect.pivot =
                new Vector2(
                    1f,
                    1f);


            valueRect.anchoredPosition =
                new Vector2(
                    -5f,
                    0f);


            valueRect.sizeDelta =
                new Vector2(
                    38f,
                    18f);


            valueText =
                valueObject
                    .AddComponent<Text>();


            valueText.font =
                Resources
                    .GetBuiltinResource<Font>(
                        "Arial.ttf");


            valueText.fontSize =
                11;


            valueText.alignment =
                TextAnchor.MiddleRight;


            valueText.color =
                Color.white;


            valueText.text =
                displayPercent
                    ? $"{initialValue:F0}%"
                    : initialValue.ToString(
                        "F0");


            valueText.raycastTarget =
                false;


            // ========================================================
            // SLIDER ROOT
            // ========================================================

            GameObject sliderObject =
                new GameObject(
                    "Slider");


            sliderObject.transform
                .SetParent(
                    popup.transform,
                    false);


            RectTransform sliderRect =
                sliderObject
                    .AddComponent<RectTransform>();


            sliderRect.anchorMin =
                new Vector2(
                    0f,
                    0f);


            sliderRect.anchorMax =
                new Vector2(
                    1f,
                    0f);


            sliderRect.pivot =
                new Vector2(
                    0.5f,
                    0f);


            sliderRect.anchoredPosition =
                new Vector2(
                    0f,
                    6f);


            sliderRect.sizeDelta =
                new Vector2(
                    -12f,
                    20f);


            slider =
                sliderObject
                    .AddComponent<Slider>();


            slider.direction =
                Slider.Direction.LeftToRight;


            slider.minValue =
                min;


            slider.maxValue =
                max;


            slider.wholeNumbers =
                wholeNumbers;


            // ========================================================
            // TRACK
            // ========================================================

            GameObject trackObject =
                new GameObject(
                    "Track");


            trackObject.transform
                .SetParent(
                    sliderObject.transform,
                    false);


            RectTransform trackRect =
                trackObject
                    .AddComponent<RectTransform>();


            trackRect.anchorMin =
                new Vector2(
                    0f,
                    0.5f);


            trackRect.anchorMax =
                new Vector2(
                    1f,
                    0.5f);


            trackRect.pivot =
                new Vector2(
                    0.5f,
                    0.5f);


            trackRect.anchoredPosition =
                Vector2.zero;


            trackRect.sizeDelta =
                new Vector2(
                    -8f,
                    4f);


            Image trackImage =
                trackObject
                    .AddComponent<Image>();


            trackImage.color =
                SliderTrackColor;


            // ========================================================
            // HANDLE AREA
            // ========================================================

            GameObject handleAreaObject =
                new GameObject(
                    "Handle Slide Area");


            handleAreaObject.transform
                .SetParent(
                    sliderObject.transform,
                    false);


            RectTransform handleArea =
                handleAreaObject
                    .AddComponent<RectTransform>();


            handleArea.anchorMin =
                Vector2.zero;


            handleArea.anchorMax =
                Vector2.one;


            handleArea.offsetMin =
                new Vector2(
                    6f,
                    0f);


            handleArea.offsetMax =
                new Vector2(
                    -6f,
                    0f);


            // ========================================================
            // HANDLE
            // ========================================================

            GameObject handleObject =
                new GameObject(
                    "Handle");


            handleObject.transform
                .SetParent(
                    handleAreaObject.transform,
                    false);


            RectTransform handleRect =
                handleObject
                    .AddComponent<RectTransform>();


            handleRect.anchorMin =
                new Vector2(
                    0f,
                    0.5f);


            handleRect.anchorMax =
                new Vector2(
                    0f,
                    0.5f);


            handleRect.pivot =
                new Vector2(
                    0.5f,
                    0.5f);


            handleRect.anchoredPosition =
                Vector2.zero;


            // --------------------------------------------------------
            // SMALLER HANDLE
            // --------------------------------------------------------

            handleRect.sizeDelta =
                new Vector2(
                    10f,
                    14f);


            Image handleImage =
                handleObject
                    .AddComponent<Image>();


            handleImage.color =
                Color.white;


            // ========================================================
            // SLIDER HOOKUP
            // ========================================================

            slider.targetGraphic =
                handleImage;


            slider.handleRect =
                handleRect;


            // --------------------------------------------------------
            // Deliberately no fill rect.
            //
            // This prevents the previous giant cyan rectangle bug.
            // --------------------------------------------------------

            slider.fillRect =
                null;


            slider.SetValueWithoutNotify(
                initialValue);


            slider.onValueChanged
                .AddListener(
                    value =>
                    {
                        changed?.Invoke(
                            value);
                    });


            return popup;
        }


        // ============================================================
        // BUTTON FACTORY
        // ============================================================

        private static Button CreateButton(
            string label,
            float x,
            float y,
            float width,
            float height,
            Action action)
        {
            GameObject obj =
                CreateBox(
                    "Button_" +
                    label.Replace(
                        " ",
                        "_"),
                    x,
                    y,
                    width,
                    height,
                    ButtonColor);


            Button button =
                obj.AddComponent<Button>();


            button.targetGraphic =
                obj.GetComponent<Image>();


            if (action != null)
            {
                button.onClick
                    .AddListener(
                        () =>
                            action());
            }


            CreateText(
                obj.transform,
                label,
                12,
                TextAnchor.MiddleCenter,
                0f);


            return button;
        }


        // ============================================================
        // BOX FACTORY
        // ============================================================

        private static GameObject CreateBox(
            string name,
            float x,
            float y,
            float width,
            float height,
            Color color)
        {
            GameObject obj =
                new GameObject(
                    name);


            obj.transform
                .SetParent(
                    _panelRect,
                    false);


            RectTransform rect =
                obj.AddComponent<RectTransform>();


            rect.anchorMin =
                new Vector2(
                    0f,
                    1f);


            rect.anchorMax =
                new Vector2(
                    0f,
                    1f);


            rect.pivot =
                new Vector2(
                    0f,
                    1f);


            rect.anchoredPosition =
                new Vector2(
                    x,
                    y);


            rect.sizeDelta =
                new Vector2(
                    width,
                    height);


            Image image =
                obj.AddComponent<Image>();


            image.color =
                color;


            return obj;
        }


        // ============================================================
        // TEXT FACTORY
        // ============================================================

        private static Text CreateText(
            Transform parent,
            string value,
            int fontSize,
            TextAnchor alignment,
            float leftPadding)
        {
            GameObject obj =
                new GameObject(
                    "Text");


            obj.transform
                .SetParent(
                    parent,
                    false);


            RectTransform rect =
                obj.AddComponent<RectTransform>();


            rect.anchorMin =
                Vector2.zero;


            rect.anchorMax =
                Vector2.one;


            rect.offsetMin =
                new Vector2(
                    leftPadding,
                    0f);


            rect.offsetMax =
                new Vector2(
                    -3f,
                    0f);


            Text text =
                obj.AddComponent<Text>();


            text.font =
                Resources
                    .GetBuiltinResource<Font>(
                        "Arial.ttf");


            text.fontSize =
                fontSize;


            text.alignment =
                alignment;


            text.color =
                Color.white;


            text.raycastTarget =
                false;


            text.text =
                value;


            return text;
        }


        // ============================================================
        // UPDATE
        // ============================================================

        internal static void Update()
        {
            if (_panelObject == null)
            {
                return;
            }


            bool raidActive =
                GameData.RaidActive;


            // ========================================================
            // RAID START / END
            // ========================================================

            if (raidActive !=
                _lastRaidActive)
            {
                _lastRaidActive =
                    raidActive;


                SetVisible(
                    raidActive);


                if (!raidActive)
                {
                    ClosePopups();


                    RaidGroupCommands
                        .Reset();


                    RefreshTargetLabels(
                        true);
                }
            }


            // ========================================================
            // LIVE TARGET DISPLAY
            // ========================================================

            if (raidActive)
            {
                RefreshTargetLabels(
                    false);
            }
        }


        // ============================================================
        // VISIBILITY
        // ============================================================

        private static void SetVisible(
            bool visible)
        {
            if (_panelObject != null)
            {
                _panelObject.SetActive(
                    visible);
            }
        }


        // ============================================================
        // POSITION
        // ============================================================

        internal static void StoreCurrentPosition()
        {
            if (_panelRect == null)
            {
                return;
            }


            _savedPosition =
                _panelRect
                    .anchoredPosition;


}


        // ============================================================
        // SHUTDOWN
        // ============================================================

        internal static void Shutdown()
        {
            if (_canvasObject != null)
            {
                UnityEngine.Object
                    .Destroy(
                        _canvasObject);
            }


            RaidGroupCommands
                .Reset();


            for (int group = 1;
                 group <= RaidGroupCount;
                 group++)
            {
                GroupTargetTexts[group] =
                    null;


                LastTargetNames[group] =
                    null;
            }


            _canvasObject =
                null;


            _panelObject =
                null;


            _panelRect =
                null;


            _dpsPopup =
                null;


            _spreadPopup =
                null;


            _dpsSlider =
                null;


            _spreadSlider =
                null;


            _dpsValueText =
                null;


            _spreadValueText =
                null;


            _lastRaidActive =
                false;
        }
    }
}