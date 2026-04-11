# Knapster / XKnapster – Copilot Instructions

## 1. Project & Architecture Overview

- `Knapster` and `XKnapster` are **Vintage Story mods** built on top of the shared **Gantry** framework.
- The solution you are assisting with is rooted at `C:\dev\VintageStory\Mods\_Released\Knapster` and contains:
  - `src\Knapster`: The main Knapster mod (multiple "Easy" features and a Mod Menu UI).
  - `src\XKnapster`: A compatibility/extension mod for `XSkills` and other mods.
  - `..\..\..\Gantry`: The Gantry framework providing hosting, DI, networking, IO, Harmony patching, GUI abstractions, and EasyX infrastructure.

### 1.1 Host / Entry Points

- Each mod has a **minimal entry point** wired through Gantry:
  - `src\Knapster\Program.cs`: `internal sealed class Program() : G.ExHost<Program>("knapster");`
  - `src\XKnapster\Program.cs`: same pattern with its own mod id.
- Do **not** add logic directly in `Program`. All behaviour should be implemented via **ModSystems**, settings, patches, and services.

### 1.2 ModSystems and Sides

- Gantry abstracts Vintage Story `ModSystem` into **sided base classes**:
  - `ServerModSystem<T>` – server-only systems.
  - `ClientModSystem<T>` – client-only systems.
  - `UniversalModSystem<T>` – loaded on both sides.
- Sides and execution are further controlled via attributes from Gantry:
  - `[[Universal]]` – both client and server.
  - `[[ServerSide]]` – server-only methods/fields.
  - `[[ClientSide]]` – client-only methods/fields.
- When adding new systems or behaviour:
  - Prefer deriving from the appropriate Gantry base class.
  - Use the sided attributes consistently to avoid side-mismatch bugs.

### 1.3 EasyX Feature Model

- Most gameplay features in Knapster follow a common **EasyX pattern** built on Gantry’s EasyX services:
  - Server systems inherit `EasyXServerSystemBase<TSystem, TServerSettings, TClientSettings>`.
  - Server settings derive from `FeatureSettings<TSettings>` and implement `IEasyXServerSettings`.
  - Client settings implement `IEasyXClientSettings`.
- Existing EasyX features include (non-exhaustive):
  - `EasyHarvesting` (scythe speed), `EasyKnapping`, `EasyClayForming`, `EasySmithing`,
    `EasyPanning`, `EasyTilling`, `EasyPressing`, `EasyMixingBowl`, `EasyDoughForming`.
- Each EasyX feature generally has:
  - `Settings` (server + client) under `Features\<FeatureName>\Settings`.
  - `Systems` containing a server system (EasyXServerSystemBase) and optionally GUI tabs.
  - `Patches` defining Harmony patches that call into Gantry command handlers.
  - Optional `Commands` classes (Brighter commands) used by patches or systems.

### 1.4 Access Control & Modes

- Every EasyX server settings class implements **standard access control**:
  - `AccessMode Mode { get; set; }` – `Disabled`, `Enabled`, `Whitelist`, `Blacklist`.
  - `List<Player> Whitelist { get; set; }`.
  - `List<Player> Blacklist { get; set; }`.
- `EasyXServerSystemBase` wires the chat commands and whitelist/blacklist operations for each feature.
- When adding or modifying EasyX features:
  - Keep `Mode`, `Whitelist`, and `Blacklist` consistent.
  - Do not bypass `IsEnabledFor(...)` checks when adding new player-specific behaviour.

### 1.5 Configuration & Settings Storage

- Gantry provides a hierarchical settings system:
  - `FeatureSettings<TSettings>` in `Gantry.Services.IO.Configuration.Abstractions` is the base for feature settings.
  - Knapster’s feature settings classes live under `src\Knapster\Features\<Feature>\Settings`.
- Key behaviours:
  - `FeatureSettings<T>.UpdateSettings(newSettings)` updates properties via reflection (honouring `[DoNotPatch]`).
  - EasyX server systems expose a `Settings` property that is scoped by `ModFileScope` (World/Global/etc.).
- **Do not** introduce custom settings persistence mechanisms. Always use Gantry’s settings services and `FeatureSettings<T>`.

### 1.6 Networking & Packets

- EasyX and Mod Menu rely on Gantry’s networking helpers:
  - `EasyXServerSystemBase` creates a server network channel and registers packets for server and client settings, plus `SettingsSavedPacket`.
  - `KnapsterAdminModMenu` (`UniversalModSystem`) uses `ModMenuSettingsPacket` to pass scope and trigger the GUI.
- New features should:
  - Re-use the existing EasyX network pattern where possible.
  - Keep packets as small, serialisable DTOs (`[ProtoContract]`, `[ProtoMember]`).

### 1.7 Harmony Patching via Gantry

- Harmony patches in this project are always created via **Gantry’s Harmony abstractions**:
  - Use `HarmonyUniversalPatchAttribute`, `HarmonyClientPatchAttribute`, `HarmonyServerPatchAttribute`, etc.
  - Avoid using raw `HarmonyPatch` attributes directly unless there is a strong reason and an existing pattern for it.
- Patches live under `Features\<Feature>\Patches` and typically:
  - Decorate a static class with `[HarmonyUniversalPatch]` (or sided equivalent).
  - Provide `[HarmonyPrefix]`, `[HarmonyPostfix]`, and/or `[HarmonyTranspiler]` methods.
  - Delegate feature logic to Brighter commands or settings queries, rather than embedding complex logic directly in the patch.

### 1.8 Commands & Brighter Integration

- Some gameplay logic is encapsulated as **commands** (Brighter-style) under `Features\<Feature>\Commands`.
- Harmony patches often call `G.CommandProcessor.Handle(new SomeCommand(args))` and act on the result.
- When adding new commands:
  - Follow existing command class patterns (simple, immutable input via ctor, result as properties).
  - Prefer commands for non-trivial operations that may be re-used (GUI + patches + systems).

### 1.9 GUI, Mod Menu, and Tabs

- Knapster provides a **Mod Menu** (`KnapsterAdminModMenu`) to configure features at runtime:
  - `KnapsterAdminModMenu` is a `UniversalModSystem` that:
    - Adds `/knapster gui` on the server for admins.
    - On client, maintains `TabFactories` (list of `Func<ComposableGuiTab>`) and opens `ModMenuDialogue`.
  - `ModMenuDialogue` is a `GenericDialogue` that composes a vertical tab layout:
    - A `GeneralSettingsGuiTab` plus feature-specific tabs.
- Feature GUI tabs (e.g. `KnappingGuiTab`, `SmithingGuiTab`) usually inherit `EasyXGuiTab<TSettings>` and:
  - Compose controls for feature settings (sliders, switches, labels).
  - Use translation helpers and specific string IDs per feature.
- When adding new configurable features with UI:
  - Create a new `ComposableGuiTab` (usually `EasyXGuiTab<T>`-based) under the feature’s `Systems` namespace.
  - Register a tab factory into `KnapsterAdminModMenu.TabFactories` from the appropriate mod system.

### 1.10 Translations & Domain Language

- Translations use Gantry helpers like `Core.Lang.Translate(...)` and `G.T(domain, code, args)`.
- Each feature and UI area appears to have its own **translation domain** (e.g. `"ModMenu"`, `"ModMenu.GeneralSettings"`, `"EasyX"`, feature names like `"Knapping"`, `"Smithing"`).
- When introducing new text:
  - Prefer reusing these domains and naming patterns.
  - Keep keys consistent with existing usage (e.g. `lblX.Text`, `lblX.HoverText`, `btnY.Text`, etc.).

> TODO (for user + future updates): Add explicit rules for XML documentation style, localisation culture, and string formatting once specified by the user.

## 2. EasyX Feature Patterns

### 2.1 Standard EasyX Feature Shape

- A typical EasyX feature in Knapster follows this structure under `src\Knapster\Features\<FeatureName>`:
  - `Settings\*ServerSettings.cs` – server configuration, `FeatureSettings<T>` + `IEasyXServerSettings`.
  - `Settings\*ClientSettings.cs` – client configuration, `IEasyXClientSettings`.
  - `Systems\*Server.cs` – main server-side EasyX system, inherits `EasyXServerSystemBase<...>`.
  - `Systems\*Client.cs` – optional client-side system (GUI wiring, behaviours).
  - `Systems\*GuiTab.cs` – optional Mod Menu tab for this feature.
  - `Patches\*.cs` – Harmony patches applying the feature’s behaviour.
  - `Commands\*.cs` – Brighter command(s) providing query/update logic used by patches and UI.
- When creating a new EasyX feature, mirror one of the existing ones (e.g. `EasyKnapping`, `EasySmithing`) and follow the same folder/naming conventions.

### 2.2 Server Settings Pattern (`*ServerSettings`)

- All EasyX server settings classes:
  - Are declared under `Knapster.Features.<Feature>.Settings`.
  - Are annotated with `[JsonObject]` and `[ProtoContract]`.
  - Derive from `FeatureSettings<TSettings>` and implement `IEasyXServerSettings`.
  - Use `[ProtoMember(n)]` in sequential order, starting at 1, without gaps unless there is a strong reason.
- They **always** include the EasyX access control contract:
  - `AccessMode Mode { get; set; }` with:
    - `[ProtoMember(1)]`
    - `[DefaultValue(AccessMode.Enabled)]`
    - `Mode` initialised to `AccessMode.Enabled`.
    - `[JsonConverter(typeof(StringEnumConverter))]` so JSON stores the enum as a string.
  - `List<Player> Whitelist { get; set; } = [];` with `[ProtoMember(2)]`.
  - `List<Player> Blacklist { get; set; } = [];` with `[ProtoMember(3)]`.
- Additional feature-specific properties:
  - Are appended starting from `[ProtoMember(4)]` upwards.
  - Use `[DefaultValue(...)]` to declare defaults that match both runtime initial values and documentation.
  - Use `[ChatCommand(...)]` on properties that should be adjustable via chat for that feature.
    - `Name` should be short and clear (e.g. `"speed"`, `"voxels"`, `"cost"`, `"instant"`).
    - `Aliases` are short letters (`"s"`, `"v"`, `"c"`, `"i"`) matching README/command docs.
    - Use `MinValue`, `MaxValue`, and `DefaultValue` to constrain and document valid ranges.

> When adding a new configuration option to an EasyX feature, prefer adding it as a property (with `[ProtoMember]`, `[DefaultValue]`, and `[ChatCommand]`) on the corresponding `*ServerSettings` class rather than inventing a new mechanism.

### 2.3 Client Settings Pattern (`*ClientSettings`)

- EasyX client settings classes:
  - Are declared under `Knapster.Features.<Feature>.Settings`.
  - Are annotated with `[JsonObject]` and `[ProtoContract]`.
  - Implement `IEasyXClientSettings` (no base class).
- They always include at least:
  - `bool Enabled { get; set; }` with `[ProtoMember(1)]`, default `false`.
- They mirror only the **client-relevant subset** of server settings:
  - Example: for `EasyHarvesting`, `SpeedMultiplier` exists both server-side and client-side, both `[ProtoMember(2)]`.
  - Do not copy server admin-only details (e.g. `Mode`, `Whitelist`, `Blacklist`) into client settings.
- `EasyXServerSystemBase` maps server settings to client settings via `GeneratePacketPerPlayer` using Gantry’s mapping extensions.

> When you add a new server setting that affects client behaviour, consider whether it should also exist on the client settings class and be populated via the EasyX mapping.

### 2.4 Server System Pattern (`*Server`)

- Every EasyX server system:
  - Lives under `Knapster.Features.<Feature>.Systems`.
  - Inherits `EasyXServerSystemBase<TSystem, TServerSettings, TClientSettings>`.
  - Is typically `sealed` unless there is a strong reason not to.
  - Overrides only what it needs; most rely entirely on base wiring.
- Structure example:
  - `public sealed class EasyHarvestingServer : EasyXServerSystemBase<EasyHarvestingServer, EasyHarvestingServerSettings, EasyHarvestingClientSettings>`
  - Defines:
    - `protected override string SubCommandName => "Harvesting";`
- Some features also override `ShouldLoad(ICoreAPI api)` to enforce **mod dependencies**:
  - E.g. `EasyDoughFormingServer` checks `artofcooking` and `coreofarts` mods.
  - E.g. `EasyMixingBowlServer` checks `aculinaryartillery`.
- Override points:
  - `SubCommandName`:
    - Must match the chat subcommand and translation domain usage (e.g. `"Harvesting"`, `"Knapping"`, `"Smithing"`).
  - `FeatureSpecificCommands(IChatCommand subCommand, CommandArgumentParsers parsers)`:
    - For adding extra subcommands or feature-specific chat options.
  - `ShouldLoad(ICoreAPI api)`:
    - For feature gating based on mod presence or other conditions.

> When creating a new EasyX feature, always:
> - Choose a `SubCommandName` that is singular, PascalCase, and consistent with existing features.
> - Ensure translations and README command documentation use this same name.

### 2.5 Client System Pattern (`*Client`)

- Many features have a dedicated client system that:
  - Lives under `Knapster.Features.<Feature>.Systems`.
  - Usually inherits a Gantry EasyX client base (e.g. `EasyXClientSystemBase<...>` or similar – follow existing patterns).
  - Listens to client settings packets and manages GUI elements/behaviours.
- The client system is responsible for:
  - Updating in-game behaviour based on `IEasyXClientSettings` received over the network.
  - Optionally registering GUI controls, overlays, or other visuals related to the feature.

> When changing or adding client behaviour for an EasyX feature, attach it to the existing `*Client` system rather than adding ad-hoc logic elsewhere.

### 2.6 GUI Tabs for EasyX Features

- For features that appear in the Mod Menu:
  - There is a `*GuiTab` class in `Systems` that inherits `EasyXGuiTab<TSettings>`.
  - Constructor pattern:
    - `public SmithingGuiTab(ICoreGantryAPI core, EasySmithingServerSettings settings) : EasyXGuiTab<EasySmithingServerSettings>(core, "Smithing", settings) { }`
  - `ComposeFeatureSettings(GuiComposer composer, ElementBounds left, ElementBounds right)`:
    - Adds per-feature UI controls (sliders, switches, labels).
    - Uses feature-specific translation helpers:
      - `F("lblX.Text")` / `F("lblX.HoverText")` for labels/hover text.
      - `P("intSomething.Unit", value)` for pluralisation-aware units.
  - `RefreshValues(GuiComposer composer)`:
    - Reads values from `Settings`.
    - Pushes them into controls (e.g. `SetValues(...)` on sliders, `SetValue(...)` on switches).
- Event handlers (`OnXChanged`) always:
  - Update `Settings.<Property>`.
  - Call `RefreshValues(Composer)` to keep UI and settings synchronised.
  - Return `true` for slider callbacks to show the value change is accepted.

> When you add a new EasyX setting intended for admin configuration, update both:
> - The server settings class (property with `[ChatCommand]`).
> - The corresponding `*GuiTab` to expose the property in the Mod Menu.

### 2.7 Harmony Patches for EasyX Features

- Every EasyX feature with runtime behaviour has a `Patches` class or classes:
  - Declared under `Knapster.Features.<Feature>.Patches`.
  - Decorated with `[HarmonyUniversalPatch]` (unless feature is strictly side-specific).
  - Uses Gantry Harmony annotations with target types/method names of Vintage Story game classes.
- Patches use:
  - `HarmonyPrefix` for pre-execution changes.
  - `HarmonyPostfix` for post-execution changes.
  - `HarmonyTranspiler` for IL-level modifications when needed.
- They **do not** hard-code settings directly; instead they:
  - Call Gantry’s command pipeline:
    - Example: `G.CommandProcessor.Handle(new GetHarvestingSpeedMultiplierCommand(byEntity)).SpeedMultiplier;`
  - Or otherwise query `Settings` via well-defined commands or services.

> When extending or fixing an EasyX behaviour:
> - Prefer adding/updating a Brighter command (e.g. `Get<Feature>SettingsCommand`) and calling it from the patch.
> - Keep Harmony code thin and focused on wiring, not business logic.

### 2.8 Chat Commands for EasyX Features

- EasyX server systems auto-wire core commands through `EasyXServerSystemBase`:
  - Base commands:
    - `/knapster <subCommandName> mode ...`
    - `/knapster <subCommandName> whitelist[...]`
    - `/knapster <subCommandName> blacklist[...]`
  - Plus automatically generated commands for each `[ChatCommand]`-annotated property in `TServerSettings`.
- Extra feature-specific commands:
  - Implemented by overriding `FeatureSpecificCommands(...)` in the EasyX server system if needed.
  - Use Gantry’s chat command parsers and translation helpers.

> When documenting new EasyX commands, follow the style in `docs\README.md`:
> - Group by feature.
> - Explicitly list allowed values and ranges (min/max).
> - Keep naming consistent with `[ChatCommand]` attributes and Mod Menu labels.

## 3. Adding a New EasyX Feature – Playbook

This section is a **step-by-step guide** for adding a new EasyX feature to Knapster. When asked to "add a new EasyX feature", follow these steps and mirror existing features closely (e.g. `EasyKnapping`, `EasySmithing`, `EasyHarvesting`).

### 3.1 Choose a Feature Name and Scope

1. Pick a **PascalCase** feature name that fits the existing pattern, e.g. `EasyChiseling` or `EasySawing`.
2. Decide what the feature changes in gameplay (e.g. speed multiplier, instant completion, voxel batch size).
3. Confirm whether it should:
   - Be universally available when enabled, but toggleable via access mode/whitelists/blacklists.
   - Have both chat commands **and** a Mod Menu tab, or chat commands only.

### 3.2 Create Folder Structure

Under `src\Knapster\Features`, create a new folder `Easy<Feature>` with subfolders mirroring other features:

- `Settings` – for `Easy<Feature>ServerSettings` and `Easy<Feature>ClientSettings`.
- `Systems` – for `Easy<Feature>Server`, `Easy<Feature>Client`, and optional `*GuiTab`.
- `Patches` – for Harmony patches that implement behaviour.
- `Commands` – for any Brighter command(s) the patches or systems will use.

Follow the exact casing and structure used by existing features.

### 3.3 Define Server Settings

1. In `Settings\Easy<Feature>ServerSettings.cs`, create a class:

   - Namespace: `Knapster.Features.Easy<Feature>.Settings`.
   - Attributes: `[JsonObject]`, `[ProtoContract]`.
   - Base/interface: `FeatureSettings<Easy<Feature>ServerSettings>, IEasyXServerSettings`.

2. Add the standard access control properties:

   - `Mode` with `[ProtoMember(1)]`, `[DefaultValue(AccessMode.Enabled)]`, and the JSON enum converter.
   - `Whitelist` with `[ProtoMember(2)]`.
   - `Blacklist` with `[ProtoMember(3)]`.

3. Add feature-specific properties starting from `[ProtoMember(4)]`:

   - Use `float`, `int`, or `bool` as appropriate.
   - Decorate with `[DefaultValue(...)]` to match intended defaults.
   - Add `[ChatCommand("name", Aliases = [...], MinValue = ..., MaxValue = ..., DefaultValue = ...)]` as needed.

4. Keep XML summaries short and aligned with README wording (you will define XML rules later, but maintain consistency with existing summaries).

### 3.4 Define Client Settings

1. In `Settings\Easy<Feature>ClientSettings.cs`, create a class:

   - Namespace: `Knapster.Features.Easy<Feature>.Settings`.
   - Attributes: `[JsonObject]`, `[ProtoContract]`.
   - Interface: `IEasyXClientSettings`.

2. Add the standard fields:

   - `Enabled` with `[ProtoMember(1)]`, default `false`.

3. Add client-relevant copies of the server settings (e.g. multipliers) with matching `[ProtoMember]` indices where appropriate.

> Be careful that the client settings class only contains what the client needs to know for behaviour/GUI; do not mirror admin-only fields.

### 3.5 Implement the Server System

1. In `Systems\Easy<Feature>Server.cs`, create:

   - Namespace: `Knapster.Features.Easy<Feature>.Systems`.
   - Class: usually `public sealed class Easy<Feature>Server : EasyXServerSystemBase<Easy<Feature>Server, Easy<Feature>ServerSettings, Easy<Feature>ClientSettings>`.

2. Implement the required override:

   - `protected override string SubCommandName => "<FeatureName>";`
     - This is the SubCommand used in `/knapster <subCommandName> ...`.
     - It should match the feature name used in translations and README (e.g. `"Harvesting"`, `"Smithing"`).

3. If the feature depends on other mods, override `ShouldLoad(ICoreAPI api)` and use `api.ModLoader.IsModEnabled(...)` or `AreAllModsLoaded(...)` as shown in `EasyDoughFormingServer` and `EasyMixingBowlServer`.

4. If you need extra chat commands beyond the auto-generated ones from `[ChatCommand]`:

   - Override `FeatureSpecificCommands(IChatCommand subCommand, CommandArgumentParsers parsers)`.
   - Use Gantry extensions for argument parsers and translations.

### 3.6 Implement the Client System (If Needed)

1. In `Systems\Easy<Feature>Client.cs`, create a client system matching existing EasyX client patterns:

   - Namespace: `Knapster.Features.Easy<Feature>.Systems`.
   - Base class: follow existing EasyX client base (e.g. `EasyXClientSystemBase<...>`).
   - Responsibilities: subscribe to client settings packets, update behaviour and local state.

2. Register any per-client behaviour (e.g. GUI hooks, animations) in this system, not in random static helpers.

> When in doubt, mirror `EasyKnappingClient`, `EasySmithingClient`, or another similar feature.

### 3.7 Create Harmony Patches for Behaviour

1. In `Patches\Easy<Feature>UniversalPatches.cs` (or similar name), define a class:

   - Namespace: `Knapster.Features.Easy<Feature>.Patches`.
   - Attributes: `[HarmonyUniversalPatch]` on the class.

2. Add targeted patch methods for the Vintage Story classes/methods you want to influence:

   - Use Gantry Harmony attributes to point to the target type and method (e.g. `typeof(ItemScythe), nameof(ItemScythe.OnHeldAttackStop)`).
   - Implement `HarmonyPrefix`, `HarmonyPostfix`, or `HarmonyTranspiler` as minimal glue.

3. For any logic that requires reading settings or complex calculations:

   - Create a command under `Commands` (e.g. `Get<Feature>SettingsCommand`).
   - In the patch, call `G.CommandProcessor.Handle(new Get<Feature>SettingsCommand(args))` and use the returned data.

> Keep IL manipulation in transpilers as small and well-scoped as possible; prefer direct method calls with clear names.

### 3.8 Add Commands (If Needed)

1. Under `Commands`, add a command class such as `Get<Feature>SettingsCommand` or something more specific.
2. The command should:

   - Encapsulate inputs via constructor parameters.
   - Expose result properties with descriptive names (`SpeedMultiplier`, `IsEnabled`, etc.).
   - Follow existing Brighter command patterns in the project.

3. Harmony patches and/or systems should use commands rather than duplicating logic.

### 3.9 Wire the Feature into the Mod Menu (Optional but Recommended)

If the feature should appear in the Mod Menu:

1. Create a `Systems\<Feature>GuiTab.cs` class inheriting `EasyXGuiTab<Easy<Feature>ServerSettings>`:

   - Constructor: `public <Feature>GuiTab(ICoreGantryAPI core, Easy<Feature>ServerSettings settings) : EasyXGuiTab<Easy<Feature>ServerSettings>(core, "<FeatureName>", settings) { }`.
   - Override `ComposeFeatureSettings` to add UI for your feature’s settings.
   - Override `RefreshValues` to sync UI from settings.

2. In the relevant ModSystem (or a central place), add a factory to `KnapsterAdminModMenu.TabFactories` to create this tab.

> Always reuse the translation key patterns (`lblX.Text`, `lblX.HoverText`, switches, sliders) from existing tabs.

### 3.10 Add Translations

1. Identify the translation domain to use:

   - For Mod Menu tabs, follow patterns like `"ModMenu.<FeatureName>"` or `"EasyX.<FeatureName>"`.
   - For general features, use `"EasyX"` plus the feature name, e.g. `"EasyX.EasyChiseling"`.

2. Add translation keys for any new text introduced by the feature:

   - Use `Core.Lang.Translate(...)` in patches, systems, and commands as needed.
   - Ensure keys are consistent with existing translation usages (e.g. `lblX.Text`, `btnY.Tooltip`).

## 4. Harmony Patching – Safety & Transpiler Guidelines

### 4.1 General Principles

- Prefer **small, explicit patches** over large, invasive ones.
- Keep Harmony classes **thin**:
  - Do not embed heavy logic directly in prefixes/postfixes/transpilers.
  - Delegate to commands/services (`G.CommandProcessor.Handle(...)`) or helper methods.
- Respect **sides**:
  - Use Gantry’s sided attributes/patch attributes correctly (`HarmonyUniversalPatch`, `HarmonyClientPatch`, `HarmonyServerPatch`).
  - Never assume a type or API is available on both client and server unless you’ve checked existing patterns.

> When in doubt, copy the structure of an existing Knapster patch (e.g. `EasyHarvestingUniversalPatches`) and adapt minimally.

### 4.2 Using Gantry Harmony Attributes (Not Raw HarmonyPatch)

- Always use Gantry’s attributes from `Gantry.Services.HarmonyPatches.Annotations`:
  - Class-level: `HarmonyUniversalPatchAttribute`, `HarmonyClientPatchAttribute`, `HarmonyServerPatchAttribute`, etc.
  - Method-level: same attributes with target types and methods.
- Avoid annotating with `HarmonyPatch` directly unless:
  - There is a clear, existing precedent in this codebase.
  - You need a specific overload that Gantry wrappers do not provide.

**Patterns:**
- Class-wide “universal” patch container:
  - `[HarmonyUniversalPatch] public sealed class EasyHarvestingUniversalPatches { ... }`
- Method-specific target:
  - `[HarmonyPrefix]`
  - `[HarmonyUniversalPatch(typeof(ItemScythe), nameof(ItemScythe.OnHeldAttackStop))]`
  - `public static void Prefix(ref float secondsPassed, EntityAgent byEntity) { ... }`

### 4.3 Safety Rules for Prefix/Postfix Patches

- **Prefix:**
  - Only modify `ref` parameters when necessary and intentional.
  - Avoid throwing exceptions; guard against `null`/invalid state defensively.
  - If you need to cancel the original method:
    - Use `bool`/`__runOriginal` patterns only where existing patches already do so.
    - Otherwise, prefer adjusting inputs/outputs, not skipping the method.

- **Postfix:**
  - Treat original return values as potentially `null` (or default) unless documented otherwise.
  - Do not assume a postfix will always run; other mods may throw earlier.

> Do not rely on specific call ordering with other mods’ patches. Write patches so they are safe even if they run first or last.

### 4.4 Safety Rules for Transpilers

Transpilers are powerful but fragile; keep them **surgical**.

- Only use a transpiler when:
  - You must alter control flow or inject extra operations **inside** an existing method.
  - A simple prefix/postfix cannot achieve the required behaviour.
- Work with **small, robust patterns**:
  - Search for a short, unique IL sequence (e.g. `ldc.r4 2f` followed by `clt`) as in `EasyHarvestingUniversalPatches`.
  - Add minimal extra instructions around that pattern; do not re-write the entire method.

**Do:**
- Convert to an array once (`var code = instructions.ToArray();`).
- Iterate with indices; keep a separate `result` list.
- Copy instructions unchanged, except when you hit your pattern.
- Verify bounds (e.g. `i < code.Length - 1`) before peeking `i + 1`.

**Don’t:**
- Don’t assume the IL will never change between Vintage Story versions.
- Don’t match extremely generic patterns that might appear many times.
- Don’t reorder instructions unless you fully understand the control flow and stack behaviour.

> If you cannot clearly explain what your transpiler does in 2–3 sentences and point to a small, unique IL pattern, it’s probably too complex for this codebase.

### 4.5 Working with Other Mods / Game Updates

- Target methods from **core game types** (`ItemScythe`, GUI entities, etc.) may change across Vintage Story versions.
- Mitigation strategies:
  - Keep patches resilient:
    - If the IL pattern is not found, return the original `instructions` unchanged rather than throwing.
  - Log in debug/verbose modes if a pattern was not applied (using `G.Logger.VerboseDebug` or similar) when appropriate.

> Never hard-fail (throw) in a Harmony patch because the underlying IL changed; prefer a graceful no-op.

### 4.6 Reading Settings from Patches

- Patches should not read global/static config directly unless there is no alternative.
- Preferred pattern:
  - Use a Brighter command or a dedicated service:
    - Example: `SpeedMultiplier(byEntity)` in `EasyHarvestingUniversalPatches` delegates to:
      - `G.CommandProcessor.Handle(new GetHarvestingSpeedMultiplierCommand(byEntity)).SpeedMultiplier;`
- Benefits:
  - Centralises feature logic.
  - Keeps patches decoupled from settings implementation details.
  - Makes it easier to reuse logic in GUI, systems, and tests.

> When adding new behaviour, start from “add a command + call it from the patch” rather than “compute everything in the patch”.

### 4.7 Testing and Verifying Patches

- Before committing changes to patches:
  - Compile the solution; ensure no missing method/field references to Vintage Story API types.
  - Reason about stack behaviour:
    - For every transpiler modification, ensure the evaluation stack remains balanced.
  - Where possible, test in-game with:
    - Feature **enabled/disabled**.
    - Access modes (Enabled/Disabled/Whitelist/Blacklist) with different players.
- When patches depend on EasyX settings:
  - Verify that enabling/disabling the feature for a player actually changes behaviour as expected.
  - Ensure no exceptions are thrown when the feature is `Disabled` globally.

### 4.8 Common Pitfalls to Avoid

- **Side mismatch:**
  - Calling client-only APIs from server-only patches, or vice versa.
  - Fix: ensure the target method you patch is valid on the side you annotate (`HarmonyClientPatch`, `HarmonyServerPatch`, `HarmonyUniversalPatch` + check API usages).

- **Null assumptions:**
  - Assuming `byEntity`, `Slot`, `BlockEntity`, or GUI references are never `null`.
  - Fix: add `if (x == null) return;` guards in patches where appropriate.

- **Overwriting others’ behaviour:**
  - Completely replacing logic that other mods might expect.
  - Fix: prefer multiplicative/additive adjustments (e.g. multiplying time or speed) instead of wholesale replacement.

- **Transpiler overreach:**
  - Modifying too large a region, causing unexpected side effects when the method changes.
  - Fix: keep patterns as narrow and specific as possible.

---

> When you ask GitHub Copilot (this assistant) to add or adjust a Harmony patch in this project, it should:
> - Prefer prefix/postfix over transpilers where possible.
> - Use Gantry’s Harmony attributes, not raw `HarmonyPatch`, unless matching an existing pattern.
> - Keep patches small, safe, defensive, and rely on commands/settings/services for real logic.

## 5. Gantry ModHost, Dependency Injection, and Services

### 5.1 Hosting Model (`G.ExHost` and `ModHost`)

- Entry points (`Program` classes) call into Gantry’s host builder via:
  - `internal sealed class Program() : G.ExHost<Program>("knapster");`
- `G.ExHost<T>` sets up:
  - A `ModHost` with DI container (`IServiceCollection` / `IServiceProvider`).
  - Registration of Gantry core services (IO, Harmony, EasyX, Brighter, etc.).
  - Loading and lifetime management of all `ModSystem` derivatives in the mod’s assemblies.

> Do not bypass `G.ExHost` or manually construct your own service providers. All services should be registered into and resolved from Gantry’s DI.

### 5.2 Where to Register Services

There are **three main places** to hook into DI:

1. **ModSystem-level registration (sided)**
   - For server-only or client-only services specific to a ModSystem, implement the appropriate registrar interface and/or sided methods that Gantry exposes.
   - Example: `EasyXServerSystemBase<T>` implements `IServerServiceRegistrar`:
     - `public void ConfigureServerModServices(IServiceCollection services, ICoreGantryAPI gantry) { ... }`
   - Use these hooks when the service is **feature-specific** and only relevant when that system is loaded.

2. **EasyX feature registration**
   - `EasyXServerSystemBase<TSystem, TServerSettings, TClientSettings>` already registers feature settings:
     - `services.AddScopedFeatureSettings<TServerSettings>();`
   - Additional per-feature services (e.g. helpers for complex computations) can also be registered here if they are tightly coupled to the feature.

3. **Global Gantry/Host extensions (re-usable services)**
   - If a service is reusable across multiple features/mod systems, prefer registering it using Gantry’s hosting extension points (e.g. `GantryDependencyInjectionExtensions` or `HostExtensions`) rather than per-feature.
   - This is less common inside Knapster itself but exists in the Gantry repo for cross-mod services.

> Rule of thumb: 
> - Feature-specific, side-specific service → register in that feature’s `*Server` / `*Client` system or its DI hook.
> - Cross-feature, cross-system service → register once via a shared host/DI extension in Gantry.

### 5.3 Service Lifetimes and Patterns

Use lifetimes consistent with existing Gantry patterns:

- `Singleton`
  - For stateless or app-wide state objects that should exist once per side.
  - Example: utilities, shared caches that do not depend on per-world/per-player settings.

- `Scoped` (per “mod scope” / world context)
  - Used by Gantry for settings (`AddScopedFeatureSettings<T>()`).
  - Prefer for services that logically belong to a world/save or feature configuration.

- `Transient`
  - For lightweight, stateless helpers that are short-lived and created on demand.
  - Avoid if the service maintains state across calls.

> When adding new services, mirror the lifetime & registration style of closest existing examples in Gantry (e.g. how IO or Harmony services are registered).

### 5.4 How to Register New Services (Concrete Guidance)

When you need a new service for Knapster:

1. Decide the scope:
   - Does it only matter on the **server** side? Use a server system or server registrar.
   - Does it only matter on the **client** side? Use a client system or client registrar.
   - Do both sides need different implementations? Use Gantry’s sided DI (different registrations per side).

2. Choose a registration hook:
   - For an EasyX feature:
     - Add to `ConfigureServerModServices(IServiceCollection services, ICoreGantryAPI gantry)` in your `EasyXServerSystemBase` subclass if that is exposed, or add a dedicated DI extension that is called from the system.
   - For a general Knapster-wide service (used by multiple features):
     - Prefer a central `ModSystem` or a Gantry extension registration if such a pattern already exists.

3. Register using standard extensions:
   - `services.AddSingleton<IMyService, MyService>();`
   - `services.AddScoped<IMyScopedService, MyScopedService>();`
   - `services.AddTransient<IMyTransientService, MyTransientService>();`

4. Avoid static singletons for new behaviour; prefer DI.

### 5.5 How to Inject Services

Gantry uses **constructor injection** plus custom attributes to wire ModSystems and other components.

Key patterns:

- ModSystems
  - Often have parameterless constructors, and Gantry injects dependencies via attributes:
    - `[Inject]` on fields/properties, or
    - Sided/annotated constructors (`[SidedConstructor]`).
  - For new systems, prefer:
    - A constructor that takes required dependencies and is annotated according to side rules, or
    - `[Inject]` on properties/fields if matching existing patterns.

- Other components (GUI tabs, commands, helpers)
  - GUI tabs often receive an `ICoreGantryAPI` instance and relevant settings via their constructors:
    - `public SmithingGuiTab(ICoreGantryAPI core, EasySmithingServerSettings settings) { ... }`
  - Commands are typically **not resolved from DI** but constructed directly with domain arguments and call into `G.CommandProcessor`.

> When in doubt, pass `ICoreGantryAPI` and settings explicitly into new components (constructors), rather than resolving them via DI inside those components.

### 5.6 Accessing Settings via DI vs via `G.Settings`

- Gantry provides two main patterns for working with settings:
  - The **global static-style** `G.Settings` / `Core.Settings` accessors.
  - The **scoped DI** pattern via `AddScopedFeatureSettings<T>()` and injected settings consumers/services.
- Knapster EasyX features primarily use:
  - `Core.Settings.Global.Feature<ConfigurationSettings>()`.
  - `Core.Settings.For(Scope).Feature<TServerSettings>()`.
- When adding new code inside EasyX systems:
  - Prefer using the **existing `Settings` property** on the system rather than re-resolving via DI.
  - Avoid adding your own global static accessors; reuse `G.Settings`/`Core.Settings` patterns already in use.

### 5.7 Where NOT to Register/Resolve Services

- Do **not** register services in:
  - `Program` (entry point) – hosting/DI is managed by `G.ExHost`.
  - Harmony patches – patches should be pure static glue and use `G.CommandProcessor` or other existing entry points rather than constructing or resolving services directly.
- Do **not** resolve services via `new ServiceCollection().BuildServiceProvider()` inside feature code. This would bypass Gantry’s container and lead to duplicate instances/state.

---

> When you ask GitHub Copilot (this assistant) to add or change services in this project, it should:
> - Register new services using Gantry’s DI infrastructure, not ad-hoc static singletons.
> - Choose lifetimes consistent with similar existing services (mostly scoped for feature settings, singleton for reusable utilities).
> - Inject services via constructor or `[Inject]`-style patterns used elsewhere, **not** by building a new container or calling static locators except the established `G` / `Core` accessors.
