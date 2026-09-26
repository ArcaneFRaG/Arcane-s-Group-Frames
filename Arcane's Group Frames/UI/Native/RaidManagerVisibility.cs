using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ArcanesGroupFrames
{
    internal static class RaidManagerVisibility
    {
        private static RaidManager _raidManager;

        private static GameObject _memberListRoot;

        private static CanvasGroup _memberListCanvasGroup;


        internal static bool IsVisible
        {
            get;
            private set;
        } = true;


        // ============================================================
        // VISIBILITY
        // ============================================================

        internal static void SetVisible(
            bool visible)
        {
            IsVisible =
                visible;


            if (!TryResolve())
            {
                Plugin.LogWarning(
                    "Could not resolve the native raid-member list.");

                return;
            }


            _memberListCanvasGroup.alpha =
                visible
                    ? 1f
                    : 0f;

            _memberListCanvasGroup.interactable =
                visible;

            _memberListCanvasGroup.blocksRaycasts =
                visible;


            Plugin.LogInfo(
                "Native raid-member list visibility: " +
                (visible
                    ? "Visible"
                    : "Hidden"));
        }


        internal static void Toggle()
        {
            SetVisible(
                !IsVisible);
        }


        internal static void Refresh()
        {
            _raidManager =
                null;

            _memberListRoot =
                null;

            _memberListCanvasGroup =
                null;


            SetVisible(
                IsVisible);
        }


        // ============================================================
        // RESOLVE NATIVE MEMBER LIST
        // ============================================================

        private static bool TryResolve()
        {
            if (_raidManager != null &&
                _memberListRoot != null &&
                _memberListCanvasGroup != null)
            {
                return true;
            }


            _raidManager =
                Object.FindObjectOfType<RaidManager>(
                    true);

            if (_raidManager == null)
            {
                Plugin.LogWarning(
                    "Could not find RaidManager.");

                return false;
            }


            RaidMemberSlot[] slots =
                _raidManager
                    .GetComponentsInChildren<RaidMemberSlot>(
                        true);


            if (slots == null ||
                slots.Length == 0)
            {
                Plugin.LogWarning(
                    "RaidManager contains no RaidMemberSlot components.");

                return false;
            }


            Transform commonParent =
                FindCommonParent(
                    slots);


            if (commonParent == null)
            {
                Plugin.LogWarning(
                    "Could not determine common parent for raid-member slots.");

                return false;
            }


            _memberListRoot =
                commonParent.gameObject;


            Plugin.LogInfo(
                "Raid-member list root resolved as: " +
                GetHierarchyPath(
                    commonParent));


            _memberListCanvasGroup =
                _memberListRoot
                    .GetComponent<CanvasGroup>();


            if (_memberListCanvasGroup == null)
            {
                _memberListCanvasGroup =
                    _memberListRoot
                        .AddComponent<CanvasGroup>();


                Plugin.LogInfo(
                    "Added CanvasGroup to raid-member list root.");
            }


            return true;
        }


        // ============================================================
        // COMMON PARENT
        // ============================================================

        private static Transform FindCommonParent(
            RaidMemberSlot[] slots)
        {
            if (slots == null ||
                slots.Length == 0)
            {
                return null;
            }


            List<Transform> firstPath =
                BuildParentPath(
                    slots[0].transform);


            for (int i = 0;
                 i < firstPath.Count;
                 i++)
            {
                Transform candidate =
                    firstPath[i];


                bool containsAll =
                    true;


                for (int s = 1;
                     s < slots.Length;
                     s++)
                {
                    if (!IsDescendantOf(
                        slots[s].transform,
                        candidate))
                    {
                        containsAll =
                            false;

                        break;
                    }
                }


                if (containsAll)
                {
                    return candidate;
                }
            }


            return null;
        }


        private static List<Transform> BuildParentPath(
            Transform child)
        {
            List<Transform> result =
                new List<Transform>();


            Transform current =
                child;


            while (current != null)
            {
                result.Add(
                    current);

                current =
                    current.parent;
            }


            return result;
        }


        private static bool IsDescendantOf(
            Transform child,
            Transform possibleParent)
        {
            Transform current =
                child;


            while (current != null)
            {
                if (current ==
                    possibleParent)
                {
                    return true;
                }

                current =
                    current.parent;
            }


            return false;
        }


        // ============================================================
        // DIAGNOSTICS
        // ============================================================

        private static string GetHierarchyPath(
            Transform transform)
        {
            if (transform == null)
            {
                return "<null>";
            }


            string path =
                transform.name;


            Transform current =
                transform.parent;


            while (current != null)
            {
                path =
                    current.name +
                    "/" +
                    path;

                current =
                    current.parent;
            }


            return path;
        }


        // ============================================================
        // RESET
        // ============================================================

        internal static void Reset()
        {
            if (_memberListCanvasGroup != null)
            {
                _memberListCanvasGroup.alpha =
                    1f;

                _memberListCanvasGroup.interactable =
                    true;

                _memberListCanvasGroup.blocksRaycasts =
                    true;
            }


            _raidManager =
                null;

            _memberListRoot =
                null;

            _memberListCanvasGroup =
                null;


            IsVisible =
                true;
        }
    }
}