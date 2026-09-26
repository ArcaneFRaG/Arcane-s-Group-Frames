using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ArcanesGroupFrames
{
    internal enum ClickCastBindingSlot
    {
        LeftClick,
        RightClick,
        MiddleClick,
        ShiftLeft,
        ShiftRight,
        CtrlLeft,
        CtrlRight,
        AltLeft,
        AltRight
    }

    internal static class ClickCastingManager
    {
        internal const string ActionNone = "ACTION:NONE";
        internal const string ActionTarget = "ACTION:TARGET";
        internal const string ActionInspect = "ACTION:INSPECT";
        private const string SpellPrefix = "SPELL:";

        internal static string GetBinding(
            ClickCastBindingSlot slot,
            bool partyContext = false)
        {
            if (partyContext)
            {
                switch (slot)
                {
                    case ClickCastBindingSlot.LeftClick: return PartyFramesSettings.ClickLeft;
                    case ClickCastBindingSlot.RightClick: return PartyFramesSettings.ClickRight;
                    case ClickCastBindingSlot.MiddleClick: return PartyFramesSettings.ClickMiddle;
                    case ClickCastBindingSlot.ShiftLeft: return PartyFramesSettings.ClickShiftLeft;
                    case ClickCastBindingSlot.ShiftRight: return PartyFramesSettings.ClickShiftRight;
                    case ClickCastBindingSlot.CtrlLeft: return PartyFramesSettings.ClickCtrlLeft;
                    case ClickCastBindingSlot.CtrlRight: return PartyFramesSettings.ClickCtrlRight;
                    case ClickCastBindingSlot.AltLeft: return PartyFramesSettings.ClickAltLeft;
                    case ClickCastBindingSlot.AltRight: return PartyFramesSettings.ClickAltRight;
                    default: return ActionNone;
                }
            }

            switch (slot)
            {
                case ClickCastBindingSlot.LeftClick: return RaidFramesSettings.ClickLeft;
                case ClickCastBindingSlot.RightClick: return RaidFramesSettings.ClickRight;
                case ClickCastBindingSlot.MiddleClick: return RaidFramesSettings.ClickMiddle;
                case ClickCastBindingSlot.ShiftLeft: return RaidFramesSettings.ClickShiftLeft;
                case ClickCastBindingSlot.ShiftRight: return RaidFramesSettings.ClickShiftRight;
                case ClickCastBindingSlot.CtrlLeft: return RaidFramesSettings.ClickCtrlLeft;
                case ClickCastBindingSlot.CtrlRight: return RaidFramesSettings.ClickCtrlRight;
                case ClickCastBindingSlot.AltLeft: return RaidFramesSettings.ClickAltLeft;
                case ClickCastBindingSlot.AltRight: return RaidFramesSettings.ClickAltRight;
                default: return ActionNone;
            }
        }

        internal static void SetBinding(
            ClickCastBindingSlot slot,
            string value,
            bool partyContext = false)
        {
            if (partyContext)
            {
                PartyFramesSettings.SetClickBinding(slot, value);
                return;
            }

            RaidFramesSettings.SetClickBinding(slot, value);
        }

        internal static string MakeSpellBinding(Spell spell)
        {
            return spell == null
                ? ActionNone
                : SpellPrefix + spell.Id;
        }

        internal static Spell ResolveSpell(string binding)
        {
            if (string.IsNullOrEmpty(binding) ||
                !binding.StartsWith(SpellPrefix, StringComparison.Ordinal))
            {
                return null;
            }

            return HealingSpellCatalog.FindById(
                binding.Substring(SpellPrefix.Length));
        }

        internal static string GetBindingDisplayName(string binding)
        {
            if (string.IsNullOrEmpty(binding) || binding == ActionNone)
            {
                return "None";
            }

            if (binding == ActionTarget)
            {
                return "Target";
            }

            if (binding == ActionInspect)
            {
                return "Inspect";
            }

            Spell spell = ResolveSpell(binding);

            return spell != null
                ? spell.SpellName
                : "Missing Spell";
        }

        internal static ClickCastBindingSlot ResolveSlot(
            PointerEventData.InputButton button)
        {
            bool shift =
                Input.GetKey(KeyCode.LeftShift) ||
                Input.GetKey(KeyCode.RightShift);

            bool ctrl =
                Input.GetKey(KeyCode.LeftControl) ||
                Input.GetKey(KeyCode.RightControl);

            bool alt =
                Input.GetKey(KeyCode.LeftAlt) ||
                Input.GetKey(KeyCode.RightAlt);

            if (shift)
            {
                return button == PointerEventData.InputButton.Right
                    ? ClickCastBindingSlot.ShiftRight
                    : ClickCastBindingSlot.ShiftLeft;
            }

            if (ctrl)
            {
                return button == PointerEventData.InputButton.Right
                    ? ClickCastBindingSlot.CtrlRight
                    : ClickCastBindingSlot.CtrlLeft;
            }

            if (alt)
            {
                return button == PointerEventData.InputButton.Right
                    ? ClickCastBindingSlot.AltRight
                    : ClickCastBindingSlot.AltLeft;
            }

            if (button == PointerEventData.InputButton.Right)
            {
                return ClickCastBindingSlot.RightClick;
            }

            if (button == PointerEventData.InputButton.Middle)
            {
                return ClickCastBindingSlot.MiddleClick;
            }

            return ClickCastBindingSlot.LeftClick;
        }

        internal static bool TryCastBoundSpell(
            string binding,
            Character target)
        {
            Spell spell = ResolveSpell(binding);

            if (spell == null ||
                target == null ||
                target.MyStats == null ||
                GameData.PlayerControl == null)
            {
                return false;
            }

            CastSpell caster =
                GameData.PlayerControl.GetComponent<CastSpell>();

            if (caster == null)
            {
                return false;
            }

            bool started = caster.StartSpell(
                spell,
                target.MyStats);

            if (started)
            {
}

            return started;
        }

        private static string GetCharacterName(Character character)
        {
            if (character == null)
            {
                return "<unknown>";
            }

            if (character.MyStats != null &&
                !string.IsNullOrEmpty(character.MyStats.MyName))
            {
                return character.MyStats.MyName;
            }

            return character.transform != null
                ? character.transform.name
                : "<unknown>";
        }
    }
}
