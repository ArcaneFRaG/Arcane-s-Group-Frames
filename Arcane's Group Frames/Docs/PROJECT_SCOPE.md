# Arcane's Group Frames - Project Scope

## Current raid-frame direction
- Replace Erenshor's raid frame presentation with compact WoW-style unit frames.
- Exactly three raid groups.
- Group headers are visible only while frames are unlocked/editable and hidden during normal locked gameplay.
- HP percentage is displayed in the bottom-right of each unit frame.
- Native raid state remains authoritative.
- Raid Control Panel remains a separate movable UI surface.

## Current customization
- Lock/unlock raid frames.
- Frame width and height.
- Member spacing.
- Group spacing.
- HP percentage visibility.
- Raw HP visibility.
- Class-color mode.
- Persist raid-frame screen position.

## Planned customization
- Resize behavior / drag-resize affordances.
- Selectable health-bar texture.
- Static bar-color mode and custom static color.
- Configurable missing-health/background-frame color.
- Selectable font.
- Configurable name font size.
- Configurable HP percentage font size.
- Configurable raw-HP font size.
- Persist every user-facing option across reloads/restarts.

## Group-frame replacement scope
- Replace the native normal-party/group frames with the same custom frame system.
- Normal party mode is a maximum of one custom group.
- Preserve the functional native group-command buttons even when the native member-frame presentation is replaced.
- Keep native group state/logic authoritative.

## Unit-frame interaction scope
- Left-click a member frame to target that unit, primarily for healing.
- Right-click a SimPlayer member frame to open native SimPlayer inspection.
- Add mouseover casting support so spells can be cast at the unit beneath the cursor.
- Investigate configurable click-casting bindings for direct healing/support casts from member frames.
- Ensure targeting/casting interactions do not conflict with unlocked/edit-mode dragging.

## Raid roster editing
- Drag and drop raider frames between Groups 1-3.
- Changes must update Erenshor's actual native raid-group assignment, not only the custom visual order.

## Later combat-frame polish
- Buff/debuff indicators.
- Aggro/threat highlight.
- Target highlight.
- Dead/offline presentation.
- Mana/resource bars.
- Role indicators where useful.
