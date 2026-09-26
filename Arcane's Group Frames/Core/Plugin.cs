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


            // --------------------------------------------------------
            // EXISTING NATIVE-UI TOGGLE
            //
            // Temporary while the custom replacement is still under
            // construction.
            // --------------------------------------------------------

            RaidManagerToggleButton
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


            Logging.LogInfo(
                $"{PluginName} v{PluginVersion} initialized.");
        }


        // ============================================================
        // UPDATE
        // ============================================================

        private void Update()
        {
            // --------------------------------------------------------
            // TEMPORARY NATIVE RAID UI TOGGLE
            // --------------------------------------------------------

            RaidManagerToggleButton
                .UpdateVisibility();


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


                // ----------------------------------------------------
                // RAID FRAMES
                // ----------------------------------------------------

                RaidFramesUI
                    .Shutdown();


                // ----------------------------------------------------
                // RAID CONTROL PANEL
                // ----------------------------------------------------

                RaidControlPanel
                    .Shutdown();


                // ----------------------------------------------------
                // TEMPORARY TOGGLE BUTTON
                // ----------------------------------------------------

                RaidManagerToggleButton
                    .Shutdown();


                // ----------------------------------------------------
                // RESTORE NATIVE RAID MANAGER
                // ----------------------------------------------------

                RaidManagerVisibility
                    .Reset();
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