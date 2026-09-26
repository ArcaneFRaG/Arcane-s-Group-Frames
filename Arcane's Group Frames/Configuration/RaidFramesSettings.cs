using UnityEngine;

namespace ArcanesGroupFrames
{
    internal static class RaidFramesSettings
    {
        private const string Prefix =
            "ArcanesGroupFrames.";

        internal const float DefaultFrameWidth = 145f;
        internal const float DefaultFrameHeight = 38f;
        internal const float DefaultMemberSpacing = 2f;
        internal const float DefaultGroupSpacing = 5f;
        internal const float DefaultPositionX = 8f;
        internal const float DefaultPositionY = -320f;

        internal static bool Locked { get; private set; } = true;
        internal static bool ShowHpPercent { get; private set; } = true;
        internal static bool ShowRawHealth { get; private set; } = false;
        internal static bool UseClassColors { get; private set; } = true;

        internal static float FrameWidth { get; private set; } = DefaultFrameWidth;
        internal static float FrameHeight { get; private set; } = DefaultFrameHeight;
        internal static float MemberSpacing { get; private set; } = DefaultMemberSpacing;
        internal static float GroupSpacing { get; private set; } = DefaultGroupSpacing;
        internal static float PositionX { get; private set; } = DefaultPositionX;
        internal static float PositionY { get; private set; } = DefaultPositionY;

        internal static void Load()
        {
            Locked = PlayerPrefs.GetInt(Prefix + "Locked", 1) != 0;
            ShowHpPercent = PlayerPrefs.GetInt(Prefix + "ShowHpPercent", 1) != 0;
            ShowRawHealth = PlayerPrefs.GetInt(Prefix + "ShowRawHealth", 0) != 0;
            UseClassColors = PlayerPrefs.GetInt(Prefix + "UseClassColors", 1) != 0;

            FrameWidth = PlayerPrefs.GetFloat(Prefix + "FrameWidth", DefaultFrameWidth);
            FrameHeight = PlayerPrefs.GetFloat(Prefix + "FrameHeight", DefaultFrameHeight);
            MemberSpacing = PlayerPrefs.GetFloat(Prefix + "MemberSpacing", DefaultMemberSpacing);
            GroupSpacing = PlayerPrefs.GetFloat(Prefix + "GroupSpacing", DefaultGroupSpacing);
            PositionX = PlayerPrefs.GetFloat(Prefix + "PositionX", DefaultPositionX);
            PositionY = PlayerPrefs.GetFloat(Prefix + "PositionY", DefaultPositionY);

            ClampValues();
        }

        internal static void SetLocked(bool value)
        {
            Locked = value;
            PlayerPrefs.SetInt(Prefix + "Locked", value ? 1 : 0);
            Save();
        }

        internal static void SetShowHpPercent(bool value)
        {
            ShowHpPercent = value;
            PlayerPrefs.SetInt(Prefix + "ShowHpPercent", value ? 1 : 0);
            Save();
        }

        internal static void SetShowRawHealth(bool value)
        {
            ShowRawHealth = value;
            PlayerPrefs.SetInt(Prefix + "ShowRawHealth", value ? 1 : 0);
            Save();
        }

        internal static void SetUseClassColors(bool value)
        {
            UseClassColors = value;
            PlayerPrefs.SetInt(Prefix + "UseClassColors", value ? 1 : 0);
            Save();
        }

        internal static void SetFrameWidth(float value)
        {
            FrameWidth = Mathf.Clamp(value, 90f, 240f);
            PlayerPrefs.SetFloat(Prefix + "FrameWidth", FrameWidth);
            Save();
        }

        internal static void SetFrameHeight(float value)
        {
            FrameHeight = Mathf.Clamp(value, 24f, 70f);
            PlayerPrefs.SetFloat(Prefix + "FrameHeight", FrameHeight);
            Save();
        }

        internal static void SetMemberSpacing(float value)
        {
            MemberSpacing = Mathf.Clamp(value, 0f, 10f);
            PlayerPrefs.SetFloat(Prefix + "MemberSpacing", MemberSpacing);
            Save();
        }

        internal static void SetGroupSpacing(float value)
        {
            GroupSpacing = Mathf.Clamp(value, 0f, 25f);
            PlayerPrefs.SetFloat(Prefix + "GroupSpacing", GroupSpacing);
            Save();
        }

        internal static Vector2 GetPosition()
        {
            return new Vector2(PositionX, PositionY);
        }

        internal static void SetPosition(Vector2 position)
        {
            PositionX = position.x;
            PositionY = position.y;
            PlayerPrefs.SetFloat(Prefix + "PositionX", PositionX);
            PlayerPrefs.SetFloat(Prefix + "PositionY", PositionY);
            Save();
        }

        internal static void ResetDefaults()
        {
            Locked = true;
            ShowHpPercent = true;
            ShowRawHealth = false;
            UseClassColors = true;
            FrameWidth = DefaultFrameWidth;
            FrameHeight = DefaultFrameHeight;
            MemberSpacing = DefaultMemberSpacing;
            GroupSpacing = DefaultGroupSpacing;
            PositionX = DefaultPositionX;
            PositionY = DefaultPositionY;

            PlayerPrefs.SetInt(Prefix + "Locked", 1);
            PlayerPrefs.SetInt(Prefix + "ShowHpPercent", 1);
            PlayerPrefs.SetInt(Prefix + "ShowRawHealth", 0);
            PlayerPrefs.SetInt(Prefix + "UseClassColors", 1);
            PlayerPrefs.SetFloat(Prefix + "FrameWidth", FrameWidth);
            PlayerPrefs.SetFloat(Prefix + "FrameHeight", FrameHeight);
            PlayerPrefs.SetFloat(Prefix + "MemberSpacing", MemberSpacing);
            PlayerPrefs.SetFloat(Prefix + "GroupSpacing", GroupSpacing);
            PlayerPrefs.SetFloat(Prefix + "PositionX", PositionX);
            PlayerPrefs.SetFloat(Prefix + "PositionY", PositionY);
            Save();
        }

        private static void ClampValues()
        {
            FrameWidth = Mathf.Clamp(FrameWidth, 90f, 240f);
            FrameHeight = Mathf.Clamp(FrameHeight, 24f, 70f);
            MemberSpacing = Mathf.Clamp(MemberSpacing, 0f, 10f);
            GroupSpacing = Mathf.Clamp(GroupSpacing, 0f, 25f);
        }

        private static void Save()
        {
            PlayerPrefs.Save();
        }
    }
}
