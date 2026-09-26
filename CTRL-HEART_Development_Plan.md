# CTRL+HEART — Development Plan
*5 days, 2 people. Day 5 reserved for date-character animation/voice/reaction-wiring. Days 1–4 = 4 dev-days for everything else. This plan assumes the Master Design Bible as locked scope — if that scope changes, this plan's day-by-day allocation must be revisited, not silently stretched.*

---

## READ THIS FIRST — the honest scope note

The system this plan builds now includes **610 pieces of authored prose content** (40 questions + 410 answers + 160 reactions), a Social Event Profile engine (multi-node targeting, Stress-vs-Push branching), 5-tier Connection logic, drag-and-drop RTS input, and 5 distinct endings. This is larger than the original 4-day estimate this project started with. It is being treated as achievable specifically because:
1. Content authoring is AI-assisted-draft + single human review pass (~1hr estimated for the bulk of it), not from-scratch writing.
2. Almost everything in the Social Event/tier/mixture system is a **data-tagging or lookup-table problem**, not new code per instance — the underlying engines are built once and fed data repeatedly.
3. Assets are being produced continuously alongside systems (not deferred to one day), per the asset-integration revision made earlier in planning.

If Day 3's integration checkpoint shows this isn't holding, the correct response is cutting slot count or mixture-state count (see "If You're Behind" at the end of this doc), not silently dropping quality-review on the 610 content pieces — bad content is worse than less content.

---

## TEAM SPLIT

**Track A (Internal/Systems):** GameManager, ResourceManager (Oxygen/Focus/Composure), EmotionManager (4 units + mixture detection), MindMapManager (5 nodes), Social Event Profile engine, drag-and-drop input handling, Connection-tier logic, Body-override logic, Composure/Meltdown path.

**Track B (Date/Presentation/Content):** DateManager, DialogueManager (question/answer/reaction selection), UI (both halves), all 610 pieces of dialogue content, 5 ending screens, Social Event Profile *data* (tagging, not the engine — engine is Track A).

Both tracks build against a shared data contract locked on Day 1 morning. This is the single highest-leverage moment in the whole plan — a contract mismatch found on Day 3 is the failure mode that kills a 4-day sprint.

---

## DAY 1 — FOUNDATIONS, CONTRACT LOCK, VISUAL LANGUAGE

### Together — first 2 hours (non-negotiable, blocks everything else)

**Step 1: Lock data contracts.** Define as C# interfaces/ScriptableObject schemas, in this order:
- `Node` — id, current health, dict of {emotion: influence%} for tracking dominance
- `EmotionState` — enum of the 4 core emotions + 6 mixture slots (mixture names TBD — see Open Items, use placeholder names for now, e.g. Mixture_1 through Mixture_6, so schema work isn't blocked by content decisions)
- `SocialEventProfile` — target node(s) [list, not single], effectType [enum: StressEvent | DirectEmotionPush], stressEventSubtype [enum: Panic/HeartFlutter/AwkwardSilence/Overthinking/SweatSurge — only relevant if effectType=StressEvent], pushTargetEmotion [only relevant if effectType=DirectEmotionPush], magnitude [float]
- `Question` — slotIndex (1–10), connectionTier (1–4), text, linked SocialEventProfile
- `Answer` — parent question ref, emotionState (one of 10, or Frozen/Blank), text, connectionDelta
- `Reaction` — parent question ref, resultingTier (1–4), text, animationClipTag
- `ResourceState` — Oxygen, Focus, Composure, Connection (all floats, defined min/max, defined regen rates as tunable fields not hardcoded)

**Step 2: Lock scope numbers as written-down facts, not memory.** Both people should leave this meeting with the same numbers on paper: 10 slots, 4 question-tiers/slot (40 questions), 10 answer-states/question + Frozen/Blank (410 answers), 4 reaction-tiers/question (160 reactions), ~20 animation clips, 5 endings. Any deviation from these numbers later requires both people to explicitly re-agree, not one person quietly authoring more or less.

**Step 3: Lock visual language.** Flat-color/shape-based style (fastest to produce, matches the "stylized command center" direction). Fixed color per emotion (used identically everywhere: node fill, unit color, bars). Simple geometric/single-line icons for the 5 nodes. Explicit visual distinction between Stress Event and Direct Emotion Push on the map (closes Open Item #4 from the Bible — recommend: warm/red pulse for Stress Events, cool/gold glow for Direct Emotion Push, or similar — pick something and move on, this doesn't need debate, just consistency).

### Track A — rest of Day 1
- Unity project setup, folder structure, version control.
- `Node`/`ResourceState` data model in code.
- Oxygen/Focus passive regen math as pure, unit-testable functions (no UI yet) — Lungs-health → Oxygen-regen-rate, Brain-health → Focus-regen-rate.
- **Asset task:** produce the 5 node icons + 4 core-emotion colors/shapes in the locked style. These barely change even after Day 5 polish — safe to make near-final now.

### Track B — rest of Day 1
- Unity UI skeleton: top half (date portrait placeholder, dialogue text box, response timer bar) + bottom half (node layout, resource bars) — static but styled, not gray boxes.
- `Question`/`Answer`/`Reaction`/`SocialEventProfile` ScriptableObjects — schema-complete, empty of content.
- **Asset task:** single café background, single neutral-pose date character sprite, in the locked style.

### End of Day 1 checkpoint
Both people can show a working data schema and a styled (if static) UI. No gameplay yet — this is intentional; Day 1 is the cheapest day to catch a contract mismatch.

---

## DAY 2 — CORE ENGINES, IN ISOLATION

### Track A
- **Drag-and-drop input**: emotion units are draggable from a control tray onto nodes; register drop-target, apply influence.
- Dominance tracking per node; **mixture detection** implemented exactly per the locked rule (both emotions within 15pts of each other AND both >25% individual influence = mixture; else highest = pure state). This is a pure function — write it once, unit-test it with fake inputs before wiring to UI.
- **Social Event Profile engine**: given a Profile (target node(s), effectType, magnitude), apply the correct effect — Stress Event subtype behavior OR Direct Emotion Push behavior, across one or multiple target nodes.
- Debug test harness: buttons to fire arbitrary Social Event Profiles, sliders to fake resource states, watch nodes/resources respond. Playable without any date content — this is Track A's proof the engine works.
- **Asset task:** wire node icons into the live debug UI with health-bar fill states (glance-readability, per Bible §2.3/Part 8 design principle).

### Track B
- Dialogue selection logic: given a **fake/hardcoded** ResourceState + emotion state + Connection tier, correctly select (a) the right question for that tier, (b) the right answer for that emotion state, (c) the right reaction for that resulting tier. This proves the *lookup logic* works before it's fed real data.
- Response Timer UI + expiry-triggers-resolution logic (ties to Bible §5.2 step 4 — resolution is ALWAYS timer expiry, build it that way from the start, don't build an early-confirm path you'll have to rip out).
- **Author Slot 1's full content**: 4 questions (one per tier) + up to 40 answers (10 states × 4 questions — wait, correct count: Slot 1 has exactly 4 questions total across its 4 tiers, and *each* question needs its own 10 answers, so Slot 1 alone = 4 questions + 40 answers). Use this as the proof-of-authoring-pace test: how long did 4 questions + 40 answers actually take, with AI-draft + review? This number tells you whether the Day 2–4 authoring budget for the remaining 36 questions + 360 answers is realistic — check this explicitly at end of Day 2, don't assume.
- **Asset task:** UI chrome — dialogue box, timer visual, resource bars — in locked style.

### End of Day 2 checkpoint
Track A: engine reacts correctly to manual/debug triggers, mixture detection works, glance-readable. Track B: correct question/answer/reaction selected from fake state; Slot 1 fully authored; **authoring-pace number known and sanity-checked against remaining volume.**

---

## DAY 3 — INTEGRATION + PLAYTEST (the most important day in this plan)

### Together — morning
Wire Track B's selection logic to read REAL state from Track A's engine (replace fake state with live ResourceState/EmotionState/dominance values). This is where contract mismatches surface — budget the full morning, not a quick task.

### Together — early afternoon
**Playtest Slot 1 end-to-end, 8–10 times back to back.** This is the mandatory fun-check: does managing one Social Event under drag-and-drop time pressure, producing an automatic reactive answer, actually feel good? If this loop isn't fun here, no amount of additional slots/content fixes it — stop and fix feel (timer length, drag responsiveness, regen rates, how forgiving Composure is) before writing more content.

### Split — remaining afternoon
**Track A:**
- Extend engine to handle Slot 1's carried-forward state correctly feeding into Slot 2 (confirms Bible §4.4 — no reset between slots — actually works, not just designed).
- Build Body-override logic (§5.7 — critically-low Body forces Frozen/Blank regardless of primary-node dominance) and Focus-gating logic (§5.4 — low Focus restricts to 4 core states).
- Composure tracking → Meltdown trigger (separate from Connection=0 Date Collapse).

**Track B:**
- Author Slots 2–3's full content (2 more slots × [4 questions + 40 answers] = 8 questions + 80 answers), using Day 2's pace number to gauge whether this is on schedule.
- Wire the Frozen/Blank answer-state (10 total across the game — author 3 of them alongside Slots 1–3 now).

### End of Day 3 checkpoint
Slots 1–3 fully playable with real engine state, continuous carry-forward confirmed working, Body-override and Focus-gating both implemented and testable. This is your protected minimum viable deliverable if Day 4 runs short.

---

## DAY 4 — CONTENT SCALE-UP TO ALL 10 SLOTS + ENDINGS

### Track A
- Finish wiring all remaining mixture states if any were deferred.
- Implement the 5-ending trigger logic: 4 Connection-tier endings (checked once, at end of Slot 10) + Meltdown ending (checked continuously, can fire at any slot).
- Randomized node-targeting confirmation across the 10 Social Event Profiles — verify (by eye, from Track B's authored profiles) that coverage across all 5 nodes looks reasonably balanced; flag to Track B if it isn't (authoring-discipline check, not an engine rule).

### Track B
- Author remaining Slots 4–10 (7 slots × [4 questions + 40 answers] = 28 questions + 280 answers) plus their reactions.
- Author all 160 reactions across all 10 slots (if not done incrementally already — recommend authoring a slot's reactions immediately after its questions/answers, not as a separate late pass).
- Author the remaining 7 Frozen/Blank answers.
- Write all 5 ending screens (plain text is fine — visuals come Day 5 if time allows, but Meltdown and the 4 Connection endings all need to exist as text NOW).

### Together — last 1–2 hours
**Full run-through, start to finish, both people, together.** Last chance to catch pacing/balance issues before Day 5 locks visuals to the date character. Fix numbers (regen rates, Connection-delta magnitudes, Composure thresholds) — do not start new features this late.

### End of Day 4 checkpoint
The entire game is completable, start to finish, all 10 slots, all 5 endings reachable, ugly but functionally whole. This is the actual "full scope, nothing cut" deliverable.

---

## DAY 5 — DATE CHARACTER: ANIMATION, VOICE, REACTION WIRING

Reserved as specified. Everything else (node/UI assets, backgrounds, ending screens, animation-clip pool for the internal side) is already done by end of Day 4 under this plan — Day 5 carries only the date character.

**Priority order (if time runs short, cut from the bottom, not the top):**
1. Expression swaps tied to reactions (neutral/nervous/laughing/confused/surprised) linked to the reaction-selection system from Part 5.5 — this is what makes the date feel responsive, the single highest-value item.
2. Voice: recommend short vocal reaction stingers (laugh, gasp, surprised sound) rather than full voiced dialogue for all 410 answers/160 reactions — full voice-over at this content volume is a real scope risk on top of everything else; flag this as a live decision, don't assume it's included.
3. Idle animation/breathing loop, additional expression states beyond the core 5, if time remains.

---

## IF YOU'RE BEHIND — where to cut, in order

If Day 3's checkpoint shows the authoring pace from Day 2 won't cover all 10 slots by end of Day 4, cut **slot count first**, not content quality per slot:
1. Cut from 10 slots to 7–8 (keep Slot 1's proven content, keep the final/ending slot, trim from the middle where narrative escalation matters least).
2. If still behind, cut from 6 mixture states to 3–4 (this reduces answers-per-question from 10 to 7–8, a smaller cut than reducing slots, but touches every question).
3. Do NOT cut the review pass on authored content to save time — unreviewed AI-drafted dialogue at this volume risks tonal inconsistency across 400+ lines, which is a worse failure than having fewer, well-tuned lines.

---

## OPEN ITEMS THAT AFFECT THIS PLAN DIRECTLY

Carried from the Master Bible, Part 7 — resolve before or during Day 1/2, since they block specific tasks:
- Final 6 mixture-state names/definitions — blocks final answer-content authoring (placeholder names can unblock schema work on Day 1, per Day 1 Step 1 above, but real definitions are needed before Day 2's Slot 1 authoring).
- Wordless-scenario slots (how many of the 10, if any) — blocks exact question count and Track B's Day 2 authoring-pace estimate.
- Social Event intensity scaling across the run — affects Track A's Day 3 tuning pass; can be deferred to a flat/non-scaling baseline for now and revisited if time allows.