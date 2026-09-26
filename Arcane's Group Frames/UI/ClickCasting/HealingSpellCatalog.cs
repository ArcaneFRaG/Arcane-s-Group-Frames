using System;
using System.Collections.Generic;
using System.Linq;

namespace ArcanesGroupFrames
{
    internal static class HealingSpellCatalog
    {
        internal static List<Spell> GetAllHealingSpells()
        {
            List<Spell> result = new List<Spell>();

            if (GameData.SpellDatabase == null ||
                GameData.SpellDatabase.SpellDatabase == null)
            {
                return result;
            }

            foreach (Spell spell in GameData.SpellDatabase.SpellDatabase)
            {
                if (!IsHealingSpell(spell))
                {
                    continue;
                }

                result.Add(spell);
            }

            return result
                .OrderBy(GetClassSortName)
                .ThenBy(spell => spell.RequiredLevel)
                .ThenBy(spell => spell.SpellName)
                .ToList();
        }

        internal static List<Spell> GetPlayerHealingSpells()
        {
            List<Spell> all = GetAllHealingSpells();

            if (GameData.PlayerStats == null ||
                GameData.PlayerStats.CharacterClass == null)
            {
                return all;
            }

            Class playerClass = GameData.PlayerStats.CharacterClass;
            int playerLevel = GameData.PlayerStats.Level;

            return all
                .Where(spell =>
                    spell != null &&
                    spell.RequiredLevel <= playerLevel &&
                    spell.UsedBy != null &&
                    spell.UsedBy.Contains(playerClass))
                .ToList();
        }

        internal static Spell FindById(string id)
        {
            if (string.IsNullOrEmpty(id) ||
                GameData.SpellDatabase == null ||
                GameData.SpellDatabase.SpellDatabase == null)
            {
                return null;
            }

            foreach (Spell spell in GameData.SpellDatabase.SpellDatabase)
            {
                if (spell != null &&
                    string.Equals(spell.Id, id, StringComparison.Ordinal))
                {
                    return spell;
                }
            }

            return null;
        }

        internal static bool IsHealingSpell(Spell spell)
        {
            if (spell == null)
            {
                return false;
            }

            // Native Erenshor healing spells are categorized as Heal.
            // This includes direct heals and HoTs whose healing is carried
            // by StatusEffectToApply.TargetHealing.
            return spell.Type == Spell.SpellType.Heal;
        }

        internal static string GetDisplayName(Spell spell)
        {
            if (spell == null)
            {
                return "None";
            }

            string className = GetClassSortName(spell);
            string prefix = string.IsNullOrEmpty(className)
                ? string.Empty
                : className + " - ";

            return prefix + spell.SpellName + " (Lv " + spell.RequiredLevel + ")";
        }

        internal static void DumpAllHealingSpellsToLog()
        {
            List<Spell> spells = GetAllHealingSpells();

foreach (Spell spell in spells)
            {
}
        }

        private static bool IsHot(Spell spell)
        {
            return spell != null &&
                   spell.StatusEffectToApply != null &&
                   spell.StatusEffectToApply.TargetHealing > 0 &&
                   spell.StatusEffectToApply.SpellDurationInTicks > 0;
        }

        private static string GetClassSortName(Spell spell)
        {
            if (spell == null || spell.UsedBy == null || spell.UsedBy.Count == 0)
            {
                return string.Empty;
            }

            Class first = spell.UsedBy[0];

            if (first == null)
            {
                return string.Empty;
            }

            if (!string.IsNullOrEmpty(first.DisplayName))
            {
                return first.DisplayName;
            }

            return first.ClassName ?? string.Empty;
        }
    }
}
