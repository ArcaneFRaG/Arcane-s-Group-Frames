using HarmonyLib;
using Lunaris;

namespace ArcanesGroupFrames
{
    [LunarisPlugin(
        PluginName,
        PluginVersion,
        "Arcane",
        "Custom group and raid frame replacement for Erenshor.")]
    [LunarisPermission(
        LunarisPermission.None)]
    public sealed class Plugin : LunarisPlugin
    {
        // ============================================================
        // PLUGIN INFORMATION
        // ============================================================

        internal const string PluginName =
            "Arcane's Group Frames";

        internal const string PluginVersion =
            "0.1.0";


        // ============================================================
        // INSTANCE
        // ============================================================

        internal static Plugin Instance
        {
            get;
            private set;
        }


        private Harmony _harmony;


        // ============================================================
        // STARTUP
        // ============================================================

        private void Awake()
        {
            Instance =
                this;


            // --------------------------------------------------------
            // LOAD PERSISTENT SETTINGS FIRST
            // --------------------------------------------------------

            RaidFramesSettings
                .Load();

            PartyFramesSettings
                .Load();


            // --------------------------------------------------------
            // HARMONY PATCHES
            //
            // Used by the mouseover casting target override.
            // --------------------------------------------------------

            _harmony =
                new Harmony(
                    "arcanes.groupframes");


            _harmony.PatchAll(
                typeof(Plugin).Assembly);


            // --------------------------------------------------------
            // NATIVE RAID UI VISIBILITY
            //
            // The native raid-member list is automatically hidden while
            // a raid is active and restored when the raid ends.
            // --------------------------------------------------------

            RaidManagerVisibility
                .Initialize();


            // --------------------------------------------------------
            // RAID CONTROL PANEL
            // --------------------------------------------------------

            RaidControlPanel
                .Initialize();


            // --------------------------------------------------------
            // CUSTOM RAID FRAMES
            // --------------------------------------------------------

            RaidFramesUI
                .Initialize();


            // --------------------------------------------------------
            // CUSTOM NORMAL PARTY FRAMES
            // --------------------------------------------------------

            PartyFramesUI
                .Initialize();

            GroupOptionsButton
                .Initialize();


            Logging.LogInfo(
                $"{PluginName} v{PluginVersion} initialized.");
        }


        // ============================================================
        // UPDATE
        // ============================================================

        private void Update()
        {
            // --------------------------------------------------------
            // AUTOMATIC NATIVE RAID UI VISIBILITY
            // --------------------------------------------------------

            RaidManagerVisibility
                .UpdateAutomatic();


            // --------------------------------------------------------
            // CUSTOM RAID CONTROLS
            // --------------------------------------------------------

            RaidControlPanel
                .Update();


            // --------------------------------------------------------
            // CUSTOM RAID FRAMES
            // --------------------------------------------------------

            RaidFramesUI
                .Update();


            // --------------------------------------------------------
            // CUSTOM NORMAL PARTY FRAMES
            // --------------------------------------------------------

            PartyFramesUI
                .Update();

            GroupOptionsButton
                .Update();
        }


        // ============================================================
        // SHUTDOWN
        // ============================================================

        private void OnDestroy()
        {
            try
            {
                // ----------------------------------------------------
                // OPTIONS UI
                // ----------------------------------------------------

                RaidFramesOptionsUI
                    .Shutdown();

                GroupFramesOptionsUI
                    .Shutdown();


                // ----------------------------------------------------
                // RAID FRAMES
                // ----------------------------------------------------

                UnitFrameMouseoverState
                    .Reset();


                GroupOptionsButton
                    .Shutdown();

                PartyFramesUI
                    .Shutdown();


                NativeGroupFramesVisibility
                    .Reset();


                RaidFramesUI
                    .Shutdown();


                // ----------------------------------------------------
                // RAID CONTROL PANEL
                // ----------------------------------------------------

                RaidControlPanel
                    .Shutdown();


                // ----------------------------------------------------
                // RESTORE NATIVE RAID MANAGER
                // ----------------------------------------------------

                RaidManagerVisibility
                    .Reset();


                // ----------------------------------------------------
                // REMOVE OUR HARMONY PATCHES
                // ----------------------------------------------------

                if (_harmony != null)
                {
                    _harmony.UnpatchSelf();
                    _harmony = null;
                }
            }
            finally
            {
                Instance =
                    null;
            }
        }


        // ============================================================
        // LOGGING HELPERS
        // ============================================================

        internal static void LogInfo(
            string message)
        {
            Instance?
                .Logging?
                .LogInfo(
                    message);
        }


        internal static void LogWarning(
            string message)
        {
            Instance?
                .Logging?
                .LogWarning(
                    message);
        }


        internal static void LogError(
            string message)
        {
            Instance?
                .Logging?
                .LogError(
                    message);
        }
    }
}