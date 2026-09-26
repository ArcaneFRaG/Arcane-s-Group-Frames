using UnityEngine;
using UnityEngine.UI;

namespace ArcanesGroupFrames
{
    internal sealed class RaidMemberFrame
    {
        // ============================================================
        // DATA
        // ============================================================

        private readonly RaidMemberSlot _slot;

        private readonly bool _player;


        // ============================================================
        // UI
        // ============================================================

        private readonly GameObject _root;

        private readonly RectTransform _rootRect;

        private readonly Image _background;

        private readonly Image _healthFill;

        private readonly Text _nameText;

        private readonly Text _hpPercentText;

        private readonly Text _rawHealthText;


        // ============================================================
        // CONSTRUCTOR - RAID MEMBER
        // ============================================================

        internal RaidMemberFrame(
            RectTransform parent,
            RaidMemberSlot slot)
        {
            _slot =
                slot;


            _player =
                false;


            _root =
                CreateRoot(
                    parent);


            _rootRect =
                _root.GetComponent<RectTransform>();


            _background =
                CreateBackground();


            _healthFill =
                CreateHealthFill();


            _nameText =
                CreateNameText();


            _hpPercentText =
                CreateHpPercentText();


            _rawHealthText =
                CreateRawHealthText();


            Refresh();
        }


        // ============================================================
        // CONSTRUCTOR - PLAYER
        // ============================================================

        internal RaidMemberFrame(
            RectTransform parent)
        {
            _slot =
                null;


            _player =
                true;


            _root =
                CreateRoot(
                    parent);


            _rootRect =
                _root.GetComponent<RectTransform>();


            _background =
                CreateBackground();


            _healthFill =
                CreateHealthFill();


            _nameText =
                CreateNameText();


            _hpPercentText =
                CreateHpPercentText();


            _rawHealthText =
                CreateRawHealthText();


            Refresh();
        }


        // ============================================================
        // ROOT
        // ============================================================

        private GameObject CreateRoot(
            RectTransform parent)
        {
            GameObject root =
                new GameObject(
                    _player
                        ? "RaidMember_Player"
                        : "RaidMember");


            root.transform
                .SetParent(
                    parent,
                    false);


            RectTransform rect =
                root.AddComponent<RectTransform>();


            rect.sizeDelta =
                new Vector2(
                    RaidFramesSettings.FrameWidth,
                    RaidFramesSettings.FrameHeight);


            LayoutElement layout =
                root.AddComponent<LayoutElement>();


            layout.preferredWidth =
                RaidFramesSettings.FrameWidth;


            layout.minWidth =
                RaidFramesSettings.FrameWidth;


            layout.preferredHeight =
                RaidFramesSettings.FrameHeight;


            layout.minHeight =
                RaidFramesSettings.FrameHeight;


            layout.flexibleWidth =
                0f;


            layout.flexibleHeight =
                0f;


            // Thin outer border / missing-health background.
            Image rootImage =
                root.AddComponent<Image>();


            rootImage.color =
                new Color(
                    0.025f,
                    0.03f,
                    0.035f,
                    0.98f);


            return root;
        }


        // ============================================================
        // BACKGROUND
        // ============================================================

        private Image CreateBackground()
        {
            GameObject obj =
                new GameObject(
                    "Background");


            obj.transform.SetParent(
                _root.transform,
                false);


            RectTransform rect =
                obj.AddComponent<RectTransform>();


            rect.anchorMin =
                Vector2.zero;


            rect.anchorMax =
                Vector2.one;


            rect.offsetMin =
                new Vector2(
                    1f,
                    1f);


            rect.offsetMax =
                new Vector2(
                    -1f,
                    -1f);


            Image image =
                obj.AddComponent<Image>();


            image.color =
                new Color(
                    0.07f,
                    0.07f,
                    0.075f,
                    1f);


            image.raycastTarget =
                false;


            return image;
        }


        // ============================================================
        // HEALTH FILL
        // ============================================================

        private Image CreateHealthFill()
        {
            GameObject obj =
                new GameObject(
                    "HealthFill");


            obj.transform.SetParent(
                _root.transform,
                false);


            RectTransform rect =
                obj.AddComponent<RectTransform>();


            rect.anchorMin =
                new Vector2(
                    0f,
                    0f);


            rect.anchorMax =
                new Vector2(
                    1f,
                    1f);


            rect.pivot =
                new Vector2(
                    0f,
                    0.5f);


            rect.offsetMin =
                new Vector2(
                    2f,
                    2f);


            rect.offsetMax =
                new Vector2(
                    -2f,
                    -2f);


            Image image =
                obj.AddComponent<Image>();


            image.raycastTarget =
                false;


            return image;
        }


        // ============================================================
        // NAME
        // ============================================================

        private Text CreateNameText()
        {
            GameObject obj =
                new GameObject(
                    "Name");


            obj.transform.SetParent(
                _root.transform,
                false);


            RectTransform rect =
                obj.AddComponent<RectTransform>();


            rect.anchorMin =
                Vector2.zero;


            rect.anchorMax =
                Vector2.one;


            rect.offsetMin =
                new Vector2(
                    5f,
                    4f);


            rect.offsetMax =
                new Vector2(
                    -5f,
                    -3f);


            Text text =
                obj.AddComponent<Text>();


            text.font =
                Resources.GetBuiltinResource<Font>(
                    "Arial.ttf");


            text.fontSize =
                12;


            text.alignment =
                TextAnchor.UpperLeft;


            text.color =
                Color.white;


            text.raycastTarget =
                false;


            AddOutline(
                obj);


            return text;
        }


        // ============================================================
        // HP PERCENT
        // ============================================================

        private Text CreateHpPercentText()
        {
            GameObject obj =
                new GameObject(
                    "HpPercent");


            obj.transform.SetParent(
                _root.transform,
                false);


            RectTransform rect =
                obj.AddComponent<RectTransform>();


            rect.anchorMin =
                new Vector2(
                    0.45f,
                    0f);


            rect.anchorMax =
                new Vector2(
                    1f,
                    0.55f);


            rect.offsetMin =
                Vector2.zero;


            rect.offsetMax =
                new Vector2(
                    -5f,
                    0f);


            Text text =
                obj.AddComponent<Text>();


            text.font =
                Resources.GetBuiltinResource<Font>(
                    "Arial.ttf");


            text.fontSize =
                11;


            text.alignment =
                TextAnchor.LowerRight;


            text.color =
                Color.white;


            text.raycastTarget =
                false;


            AddOutline(
                obj);


            return text;
        }


        // ============================================================
        // RAW HP
        // ============================================================

        private Text CreateRawHealthText()
        {
            GameObject obj =
                new GameObject(
                    "RawHealth");


            obj.transform.SetParent(
                _root.transform,
                false);


            RectTransform rect =
                obj.AddComponent<RectTransform>();


            rect.anchorMin =
                new Vector2(
                    0f,
                    0f);


            rect.anchorMax =
                new Vector2(
                    0.70f,
                    0.55f);


            rect.offsetMin =
                new Vector2(
                    5f,
                    0f);


            rect.offsetMax =
                Vector2.zero;


            Text text =
                obj.AddComponent<Text>();


            text.font =
                Resources.GetBuiltinResource<Font>(
                    "Arial.ttf");


            text.fontSize =
                10;


            text.alignment =
                TextAnchor.LowerLeft;


            text.color =
                new Color(
                    0.90f,
                    0.90f,
                    0.90f,
                    1f);


            text.raycastTarget =
                false;


            AddOutline(
                obj);


            return text;
        }


        // ============================================================
        // REFRESH
        // ============================================================

        internal void Refresh()
        {
            Stats stats =
                GetStats();


            _nameText.text =
                GetName();


            if (stats == null)
            {
                SetHealth(
                    0,
                    1);


                return;
            }


            SetHealth(
                stats.CurrentHP,
                stats.CurrentMaxHP);
        }


        // ============================================================
        // HEALTH
        // ============================================================

        private void SetHealth(
            int current,
            int maximum)
        {
            if (maximum <= 0)
            {
                maximum =
                    1;
            }


            current =
                Mathf.Clamp(
                    current,
                    0,
                    maximum);


            float percentage =
                Mathf.Clamp01(
                    (float)current /
                    maximum);


            RectTransform fillRect =
                _healthFill.rectTransform;


            fillRect.anchorMax =
                new Vector2(
                    percentage,
                    1f);


            fillRect.offsetMax =
                new Vector2(
                    -2f,
                    -2f);


            // --------------------------------------------------------
            // HEALTH TEXT
            // --------------------------------------------------------

            _hpPercentText.gameObject.SetActive(
                RaidFramesSettings.ShowHpPercent);


            _hpPercentText.text =
                Mathf.RoundToInt(
                    percentage * 100f) +
                "%";


            _rawHealthText.gameObject.SetActive(
                RaidFramesSettings.ShowRawHealth);


            _rawHealthText.text =
                $"{current} / {maximum}";


            // --------------------------------------------------------
            // COLORS
            // --------------------------------------------------------

            if (current <= 0)
            {
                _healthFill.color =
                    new Color(
                        0.18f,
                        0.18f,
                        0.18f,
                        0.95f);


                _nameText.color =
                    new Color(
                        0.65f,
                        0.65f,
                        0.65f,
                        1f);


                _hpPercentText.text =
                    "DEAD";


                return;
            }


            _nameText.color =
                Color.white;


            _healthFill.color =
                GetHealthColor();
        }


        // ============================================================
        // CLASS COLOR
        // ============================================================

        private Color GetHealthColor()
        {
            if (!RaidFramesSettings.UseClassColors)
            {
                return RaidClassColors.DefaultColor;
            }


            Stats stats =
                GetStats();


            if (stats == null ||
                stats.CharacterClass == null)
            {
                return RaidClassColors.DefaultColor;
            }


            return RaidClassColors.GetColor(
                stats.CharacterClass.ClassName);
        }


        // ============================================================
        // DATA
        // ============================================================

        private Stats GetStats()
        {
            if (_player)
            {
                return GameData.PlayerStats;
            }


            if (_slot == null)
            {
                return null;
            }


            if (_slot.AssignedAvatar != null &&
                _slot.AssignedAvatar.MyStats != null)
            {
                return _slot
                    .AssignedAvatar
                    .MyStats;
            }


            if (_slot.AssignedSimTracking != null &&
                _slot.AssignedSimTracking.MyStats != null)
            {
                return _slot
                    .AssignedSimTracking
                    .MyStats;
            }


            return null;
        }


        private string GetName()
        {
            if (_player)
            {
                if (GameData.PlayerStats != null &&
                    !string.IsNullOrWhiteSpace(
                        GameData.PlayerStats.MyName))
                {
                    return GameData
                        .PlayerStats
                        .MyName;
                }


                return "Player";
            }


            if (_slot == null)
            {
                return "Empty";
            }


            if (_slot.AssignedSimTracking != null &&
                !string.IsNullOrWhiteSpace(
                    _slot.AssignedSimTracking.SimName))
            {
                return _slot
                    .AssignedSimTracking
                    .SimName;
            }


            if (_slot.AssignedAvatar != null &&
                _slot.AssignedAvatar.MyStats != null &&
                !string.IsNullOrWhiteSpace(
                    _slot.AssignedAvatar.MyStats.MyName))
            {
                return _slot
                    .AssignedAvatar
                    .MyStats
                    .MyName;
            }


            return "Unknown";
        }


        // ============================================================
        // OUTLINE
        // ============================================================

        private static void AddOutline(
            GameObject obj)
        {
            Outline outline =
                obj.AddComponent<Outline>();


            outline.effectColor =
                new Color(
                    0f,
                    0f,
                    0f,
                    0.85f);


            outline.effectDistance =
                new Vector2(
                    1f,
                    -1f);
        }


        // ============================================================
        // DESTROY
        // ============================================================

        internal void Destroy()
        {
            if (_root != null)
            {
                Object.Destroy(
                    _root);
            }
        }
    }
}