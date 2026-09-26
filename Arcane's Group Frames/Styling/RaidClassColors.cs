using UnityEngine;

namespace ArcanesGroupFrames
{
    internal static class RaidClassColors
    {
        internal static Color GetColor(string className)
        {
            if (string.IsNullOrWhiteSpace(className))
            {
                return DefaultColor;
            }

            switch (className.Trim().ToLowerInvariant())
            {
                // Paladin deliberately keeps the Group Frames pink.
                case "paladin": return Paladin;
                case "arcanist": return Arcanist;
                case "duelist": return Duelist;
                case "druid": return Druid;
                case "stormcaller": return Stormcaller;
                case "reaver": return Reaver;
                case "blightcaller": return Blightcaller;
                case "necromancer": return Necromancer;
                case "monk": return Monk;
                default: return DefaultColor;
            }
        }

        internal static readonly Color Paladin =
            new Color(0.96f, 0.55f, 0.73f, 0.96f);

        // Exact Erenshor Combat Meter rc2 palette for the remaining classes.
        internal static readonly Color Arcanist =
            new Color(0.48f, 0.35f, 0.88f, 1f);
        internal static readonly Color Duelist =
            new Color(0.94f, 0.42f, 0.18f, 1f);
        internal static readonly Color Druid =
            new Color(0.30f, 0.76f, 0.34f, 1f);
        internal static readonly Color Stormcaller =
            new Color(0.24f, 0.62f, 0.94f, 1f);
        internal static readonly Color Reaver =
            new Color(0.82f, 0.20f, 0.24f, 1f);
        internal static readonly Color Blightcaller =
            new Color(0.55f, 0.22f, 0.72f, 1f);
        internal static readonly Color Necromancer =
            new Color(0.64f, 0.32f, 0.78f, 1f);
        internal static readonly Color Monk =
            new Color(0.78f, 0.62f, 0.31f, 1f);

        internal static readonly Color DefaultColor =
            new Color(0.18f, 0.52f, 0.22f, 1f);
    }
}
