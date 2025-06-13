Another World Games - Slot Game Logic
Unity-Based Box Matching Puzzle Game

Overview
This is a Unity-based slot/box matching puzzle game where players place boxes into slots and organize the items inside them. The game features automatic item matching logic, box completion checks, and level progression.

Core Systems
1. Manager System
   SlotManager → Handles all slot operations (placement, validation, events)

GameStateManager → Controls game states (playing, level complete, game over)

LevelManager → Generates levels, spawns boxes, and enforces constraints

2. Slot System
   BoxSlot → Target slots where boxes are placed

ItemSlot → Temporary slots for holding items during matching

SlotBase<T> → Generic base class for all slots

3. Box System
   Box → Main box class (logic & data)

BoxMovement → Controls animations (DOTween-powered jumps)

BoxVisual → Manages sprites/UI

BoxItemHolder → Stores and manages internal items

4. Operation Logic
   SlotOperationsHandler → Executes item transfers & matching

SlotOperationScheduler → Manages operation timing (coroutine-based)

SlotGameStateValidator → Checks win/loss conditions

Game Flow
Level Start
LevelManager spawns boxes with:

Random colors

6 items per box (max 2 identical items, max 4 unique types)

If distribution fails after 10 attempts → Scene reloads

Box Placement
Player taps a box → BoxMovement plays DOJump animation to slot

On successful placement → OnPlaced event triggers automatic operations

Automatic Operations (Coroutine-Based)
Move distinct items from boxes → ItemSlots

Move matching items from ItemSlots back to their correct boxes

Destroy completed boxes with "QuitGame" animation

Loop continues until no more valid moves remain

Game End Conditions
✅ All boxes completed → Level Complete
❌ All BoxSlots filled but boxes remain → Game Over

Technical Notes
Uses DOTween for smooth box movement animations

Coroutines drive the operation scheduler for sequenced logic

Event-Driven architecture with OnPlaced, OnBoxCompleted events