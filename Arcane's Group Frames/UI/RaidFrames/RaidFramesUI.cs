using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace ArcanesGroupFrames
{
    internal static class RaidFramesUI
    {
        // ============================================================
        // CONSTANTS
        // ============================================================

        private const int RaidGroupCount =
            3;


        private const float GroupHeaderHeight =
            22f;


        private const int MaximumMembersPerGroup =
            6;


        // ============================================================
        // ROOT
        // ============================================================

        private static GameObject _canvasObject;

        private static GameObject _panelObject;

        private static RectTransform _panelRect;


        // ============================================================
        // GROUPS
        // ============================================================

        private static readonly GameObject[]
            GroupObjects =
                new GameObject[RaidGroupCount + 1];


        private static readonly GameObject[]
            GroupHeaders =
                new GameObject[RaidGroupCount + 1];


        private static readonly RectTransform[]
            GroupContent =
                new RectTransform[RaidGroupCount + 1];


        // ============================================================
        // MEMBER FRAMES
        // ============================================================

        private static readonly List<RaidMemberFrame>
            MemberFrames =
                new List<RaidMemberFrame>();


        // ============================================================
        // STATE
        // ============================================================

        private static bool _lastRaidActive;


        private static string _lastRosterSignature =
            "";


        private static float _rosterRefreshTimer;

        private static float _healthRefreshTimer;


        private static Vector2 _savedPosition =
            Vector2.zero;


        // ============================================================
        // COLORS
        // ============================================================

        private static readonly Color GroupHeaderColor =
            new Color(
                0.055f,
                0.135f,
                0.16f,
                0.98f);


        private static readonly Color EditBackgroundColor =
            new Color(
                0.01f,
                0.02f,
                0.025f,
                0.42f);


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


            ApplyLockState();


            if (_lastRaidActive)
            {
                RebuildRoster();
            }


            Plugin.LogInfo(
                "Custom Raid Frames initialized.");
        }


        // ============================================================
        // CANVAS
        // ============================================================

        private static void CreateCanvas()
        {
            _canvasObject =
                new GameObject(
                    "ArcanesGroupFrames_RaidFramesCanvas");


            Object.DontDestroyOnLoad(
                _canvasObject);


            Canvas canvas =
                _canvasObject
                    .AddComponent<Canvas>();


            canvas.renderMode =
                RenderMode.ScreenSpaceOverlay;


            canvas.sortingOrder =
                480;


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
                    "RaidFramesPanel");


            _panelObject.transform.SetParent(
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


            _savedPosition =
                RaidFramesSettings.GetPosition();


            _panelRect.anchoredPosition =
                _savedPosition;


            BuildGroupContainers();
        }


        // ============================================================
        // GROUP CONTAINERS
        // ============================================================

        private static void BuildGroupContainers()
        {
            float width =
                RaidFramesSettings.FrameWidth;


            float groupHeight =
                GroupHeaderHeight +
                (MaximumMembersPerGroup *
                 RaidFramesSettings.FrameHeight) +
                ((MaximumMembersPerGroup - 1) *
                 RaidFramesSettings.MemberSpacing);


            float panelWidth =
                (RaidGroupCount * width) +
                ((RaidGroupCount - 1) *
                 RaidFramesSettings.GroupSpacing);


            _panelRect.sizeDelta =
                new Vector2(
                    panelWidth,
                    groupHeight);


            for (int group = 1;
                 group <= RaidGroupCount;
                 group++)
            {
                float x =
                    (group - 1) *
                    (width +
                     RaidFramesSettings.GroupSpacing);


                GameObject groupObject =
                    new GameObject(
                        $"RaidGroup_{group}");


                groupObject.transform.SetParent(
                    _panelRect,
                    false);


                RectTransform groupRect =
                    groupObject
                        .AddComponent<RectTransform>();


                groupRect.anchorMin =
                    new Vector2(
                        0f,
                        1f);


                groupRect.anchorMax =
                    new Vector2(
                        0f,
                        1f);


                groupRect.pivot =
                    new Vector2(
                        0f,
                        1f);


                groupRect.anchoredPosition =
                    new Vector2(
                        x,
                        0f);


                groupRect.sizeDelta =
                    new Vector2(
                        width,
                        groupHeight);


                Image editBackground =
                    groupObject
                        .AddComponent<Image>();


                editBackground.color =
                    EditBackgroundColor;


                GroupObjects[group] =
                    groupObject;


                CreateGroupHeader(
                    group,
                    groupRect);


                CreateMemberContainer(
                    group,
                    groupRect);
            }
        }


        // ============================================================
        // GROUP HEADER
        // ============================================================

        private static void CreateGroupHeader(
            int group,
            RectTransform parent)
        {
            GameObject header =
                new GameObject(
                    $"Group{group}_Header");


            header.transform.SetParent(
                parent,
                false);


            RectTransform rect =
                header.AddComponent<RectTransform>();


            rect.anchorMin =
                new Vector2(
                    0f,
                    1f);


            rect.anchorMax =
                new Vector2(
                    1f,
                    1f);


            rect.pivot =
                new Vector2(
                    0.5f,
                    1f);


            rect.anchoredPosition =
                Vector2.zero;


            rect.sizeDelta =
                new Vector2(
                    0f,
                    GroupHeaderHeight);


            Image image =
                header.AddComponent<Image>();


            image.color =
                GroupHeaderColor;


            Text text =
                CreateText(
                    header.transform,
                    $"GROUP {group}",
                    11,
                    TextAnchor.MiddleLeft);


            RectTransform textRect =
                text.rectTransform;


            textRect.offsetMin =
                new Vector2(
                    5f,
                    0f);


            textRect.offsetMax =
                new Vector2(
                    -3f,
                    0f);


            RaidFramesDragHandler drag =
                header.AddComponent<RaidFramesDragHandler>();


            drag.Initialize(
                _panelRect);


            GroupHeaders[group] =
                header;
        }


        // ============================================================
        // MEMBER CONTAINER
        // ============================================================

        private static void CreateMemberContainer(
            int group,
            RectTransform parent)
        {
            GameObject obj =
                new GameObject(
                    "Members");


            obj.transform.SetParent(
                parent,
                false);


            RectTransform rect =
                obj.AddComponent<RectTransform>();


            rect.anchorMin =
                new Vector2(
                    0f,
                    1f);


            rect.anchorMax =
                new Vector2(
                    1f,
                    1f);


            rect.pivot =
                new Vector2(
                    0.5f,
                    1f);


            rect.anchoredPosition =
                new Vector2(
                    0f,
                    -GroupHeaderHeight);


            rect.sizeDelta =
                new Vector2(
                    0f,
                    0f);


            VerticalLayoutGroup layout =
                obj.AddComponent<VerticalLayoutGroup>();


            layout.spacing =
                RaidFramesSettings.MemberSpacing;


            layout.childAlignment =
                TextAnchor.UpperCenter;


            layout.childControlWidth =
                false;


            layout.childControlHeight =
                false;


            layout.childForceExpandWidth =
                false;


            layout.childForceExpandHeight =
                false;


            GroupContent[group] =
                rect;
        }


        // ============================================================
        // LOCK STATE
        // ============================================================

        internal static void SetLocked(
            bool locked)
        {
            RaidFramesSettings.SetLocked(
                locked);


            ApplyLockState();
        }


        internal static void ToggleLocked()
        {
            SetLocked(
                !RaidFramesSettings.Locked);
        }


        private static void ApplyLockState()
        {
            bool unlocked =
                !RaidFramesSettings.Locked;


            for (int group = 1;
                 group <= RaidGroupCount;
                 group++)
            {
                if (GroupHeaders[group] != null)
                {
                    GroupHeaders[group]
                        .SetActive(
                            unlocked);
                }


                if (GroupObjects[group] != null)
                {
                    Image background =
                        GroupObjects[group]
                            .GetComponent<Image>();


                    if (background != null)
                    {
                        background.enabled =
                            unlocked;
                    }
                }


                if (GroupContent[group] != null)
                {
                    GroupContent[group]
                        .anchoredPosition =
                            new Vector2(
                                0f,
                                unlocked
                                    ? -GroupHeaderHeight
                                    : 0f);
                }
            }
        }


        // ============================================================
        // SETTINGS CHANGED
        // ============================================================

        internal static void SettingsChanged()
        {
            RecreateUi();
        }


        private static void RecreateUi()
        {
            ClearMemberFrames();


            _savedPosition =
                RaidFramesSettings.GetPosition();


            if (_panelRect != null)
            {
                _panelRect.anchoredPosition =
                    _savedPosition;
            }


            for (int group = 1;
                 group <= RaidGroupCount;
                 group++)
            {
                if (GroupObjects[group] != null)
                {
                    Object.Destroy(
                        GroupObjects[group]);
                }


                GroupObjects[group] =
                    null;


                GroupHeaders[group] =
                    null;


                GroupContent[group] =
                    null;
            }


            BuildGroupContainers();


            ApplyLockState();


            RebuildRoster();
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


            if (raidActive !=
                _lastRaidActive)
            {
                _lastRaidActive =
                    raidActive;


                SetVisible(
                    raidActive);


                if (raidActive)
                {
                    RebuildRoster();
                }
                else
                {
                    ClearMemberFrames();

                    _lastRosterSignature =
                        "";
                }
            }


            if (!raidActive)
            {
                return;
            }


            _healthRefreshTimer -=
                Time.unscaledDeltaTime;


            if (_healthRefreshTimer <= 0f)
            {
                _healthRefreshTimer =
                    0.10f;


                RefreshMemberFrames();
            }


            _rosterRefreshTimer -=
                Time.unscaledDeltaTime;


            if (_rosterRefreshTimer <= 0f)
            {
                _rosterRefreshTimer =
                    0.50f;


                string signature =
                    BuildRosterSignature();


                if (signature !=
                    _lastRosterSignature)
                {
                    RebuildRoster();
                }
            }
        }


        // ============================================================
        // ROSTER
        // ============================================================

        private static void RebuildRoster()
        {
            ClearMemberFrames();


            if (!GameData.RaidActive ||
                GameData.RaidManager == null)
            {
                return;
            }


            // --------------------------------------------------------
            // PLAYER -> GROUP 1
            // --------------------------------------------------------

            if (GameData.PlayerStats != null &&
                GroupContent[1] != null)
            {
                MemberFrames.Add(
                    new RaidMemberFrame(
                        GroupContent[1]));
            }


            // --------------------------------------------------------
            // RAID MEMBERS
            // --------------------------------------------------------

            List<RaidMemberSlot> slots =
                new List<RaidMemberSlot>();


            foreach (
                RaidMemberSlot slot
                in GameData.RaidManager.Raiders)
            {
                if (slot == null)
                {
                    continue;
                }


                if (slot.GroupNumber < 1 ||
                    slot.GroupNumber > RaidGroupCount)
                {
                    continue;
                }


                if (slot.AssignedSimTracking == null &&
                    slot.AssignedAvatar == null)
                {
                    continue;
                }


                slots.Add(
                    slot);
            }


            slots.Sort(
                (a, b) =>
                    a.SlotIndex.CompareTo(
                        b.SlotIndex));


            foreach (
                RaidMemberSlot slot
                in slots)
            {
                if (slot.AssignedAvatar != null &&
                    GameData.PlayerStats != null &&
                    slot.AssignedAvatar.MyStats ==
                    GameData.PlayerStats)
                {
                    continue;
                }


                RectTransform parent =
                    GroupContent[
                        slot.GroupNumber];


                if (parent == null)
                {
                    continue;
                }


                MemberFrames.Add(
                    new RaidMemberFrame(
                        parent,
                        slot));
            }


            _lastRosterSignature =
                BuildRosterSignature();


            RefreshMemberFrames();
        }


        // ============================================================
        // SIGNATURE
        // ============================================================

        private static string BuildRosterSignature()
        {
            if (!GameData.RaidActive ||
                GameData.RaidManager == null)
            {
                return "";
            }


            StringBuilder builder =
                new StringBuilder();


            foreach (
                RaidMemberSlot slot
                in GameData.RaidManager.Raiders)
            {
                if (slot == null)
                {
                    continue;
                }


                builder.Append(
                    slot.SlotIndex);


                builder.Append(
                    ':');


                builder.Append(
                    slot.GroupNumber);


                builder.Append(
                    ':');


                if (slot.AssignedSimTracking != null)
                {
                    builder.Append(
                        slot.AssignedSimTracking
                            .SimName);
                }


                builder.Append(
                    ';');
            }


            return builder.ToString();
        }


        // ============================================================
        // MEMBER REFRESH
        // ============================================================

        private static void RefreshMemberFrames()
        {
            for (int i = 0;
                 i < MemberFrames.Count;
                 i++)
            {
                MemberFrames[i]
                    ?.Refresh();
            }
        }


        private static void ClearMemberFrames()
        {
            for (int i = 0;
                 i < MemberFrames.Count;
                 i++)
            {
                MemberFrames[i]
                    ?.Destroy();
            }


            MemberFrames.Clear();
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


            RaidFramesSettings
                .SetPosition(
                    _savedPosition);


            Plugin.LogInfo(
                $"Raid Frames position: " +
                $"X={_savedPosition.x:F1}, " +
                $"Y={_savedPosition.y:F1}");
        }


        // ============================================================
        // TEXT FACTORY
        // ============================================================

        private static Text CreateText(
            Transform parent,
            string value,
            int fontSize,
            TextAnchor alignment)
        {
            GameObject obj =
                new GameObject(
                    "Text");


            obj.transform.SetParent(
                parent,
                false);


            RectTransform rect =
                obj.AddComponent<RectTransform>();


            rect.anchorMin =
                Vector2.zero;


            rect.anchorMax =
                Vector2.one;


            rect.offsetMin =
                Vector2.zero;


            rect.offsetMax =
                Vector2.zero;


            Text text =
                obj.AddComponent<Text>();


            text.font =
                Resources.GetBuiltinResource<Font>(
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
        // SHUTDOWN
        // ============================================================

        internal static void Shutdown()
        {
            ClearMemberFrames();


            if (_canvasObject != null)
            {
                Object.Destroy(
                    _canvasObject);
            }


            _canvasObject =
                null;


            _panelObject =
                null;


            _panelRect =
                null;


            _lastRaidActive =
                false;


            _lastRosterSignature =
                "";
        }
    }
}