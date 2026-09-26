# Arcane's Group Frames

Lunaris UI replacement for Erenshor group and raid frames.

## Source layout
- `Core/` - plugin lifecycle.
- `Configuration/` - persistent frame settings.
- `Styling/` - shared visual constants such as class colors.
- `UI/RaidFrames/` - raid member-frame rendering and movement.
- `UI/ControlPanel/` - raid command panel.
- `UI/Options/` - configuration UI.
- `UI/Native/` - native Raid Manager visibility/toggle integration.
- `Native/` - native control proxy helpers.
- `Raid/` - raid command/state helpers.
- `Docs/` - project scope and roadmap.

See `Docs/PROJECT_SCOPE.md` for current and planned functionality.


## Unit-frame interactions
- Left click a custom unit frame to target that unit.
- Right click a SimPlayer frame to open native inspection.
- Mouseover casting redirects Heal and Beneficial player spells to the hovered custom unit frame without changing the selected target.
