# CTRL+HEART — Master Design
*The complete, current source of truth. Supersedes conflicting sections of earlier drafts where noted. Compiled from the original canonical PRD plus every design decision made since.*

---

## PART 0 — HOW TO READ THIS DOCUMENT

This bible has two layers, and they are never mixed:
- **STABLE** — locked, foundational, not expected to change without a deliberate decision.
- **SUPERSEDED** — content from the original canonical PRD that has since been replaced. Kept visible so nobody rebuilds it by accident, marked clearly wherever it appears.

Where the original 52-section canonical PRD and later decisions conflict, **later decisions win** and the original section is marked SUPERSEDED with a pointer to its replacement.

---

## PART 1 — THE GAME [STABLE]

### 1.1 One-line pitch
CTRL+HEART is a real-time dating game where the player controls the emotional/internal forces inside a nervous person's mind while they attempt to have a normal first date. The top half of the screen shows the date; the bottom half shows the internal system. The date never pauses while the player manages the internal world.

### 1.2 Core player fantasy
The player is not the character. The player is the character's internal control center. The core feeling to protect at all times: *"I need to keep this person functioning long enough to have a normal conversation."* Social situations create internal problems; the player manages limited internal resources in real time; the resulting internal state determines what the character says and how the date reacts.

### 1.3 The central chain
```
Social Situation → Internal Crisis → Emotional Control → Internal State → Human Behaviour → Date Reaction
```
Everything in this document exists to serve this one chain. Any new feature should be checked against it: does it strengthen this loop, or dilute it?

### 1.4 Genre identity
Dating Simulation × Real-Time Internal Command. Not a conventional large-scale RTS — the internal map is small, readable, and tightly bound to the social situation. The game should feel like a genre collision, not an RTS bolted onto a visual novel.

### 1.5 Non-negotiable design rule
**The player never chooses dialogue.** Dialogue is the automatic readout of whatever internal state the player's management produced — never a menu, never player-selected. This was explicitly considered and explicitly rejected (see Part 6, Resolved Design Gaps, decision log). Every dialogue/answer/reaction system in this document exists downstream of this rule.

### 1.6 Tone
Funny, warm, chaotic, socially recognizable, mildly absurd. The internal world can be exaggerated and silly; the date itself should stay believable enough that the player recognizes the situations. The contrast between the two is where most of the comedy comes from.

**Known tonal risk (flagged, not yet resolved):** "Meltdown," a countdown timer, and failure states framed around losing composure could read as an anxiety-attack simulation rather than comedy depending on execution. Worth a deliberate tone pass rather than leaving this to accident.

---

## PART 2 — THE INTERNAL WORLD [STABLE]

### 2.1 Screen structure
Single persistent screen, split top/bottom, both halves visible at all times, neither becomes a separate mode:
- **Top:** the date — protagonist, date, dialogue/actions/reactions, response timer.
- **Bottom:** the internal world — the five-node map, the four emotion units, the four resource bars.

### 2.2 The four player-controlled units (emotions)
The player commands exactly these four. No other unit types exist (no cell types, no worker classes, no biological armies) — biology is represented through nodes and systems, not additional units.

| Emotion | Role | Strongest at | Overuse risk |
|---|---|---|---|
| **Calm** | Stabilization | Reducing node pressure, recovering damaged systems, stabilizing Heart | Relatively slow to act |
| **Anxiety** | Emergency response | Fastest unit; reaching threats quickly, intercepting Stress Events | Increases emotional pressure, makes character more visibly nervous |
| **Confidence** | Control & communication | Taking contested nodes, strengthening Voice, enabling assertive responses | Can read as arrogant |
| **Attraction** | Social amplification | Amplifying positive interactions, strengthening Heart/Body during good moments | Can produce oversharing, premature intensity |

### 2.3 The five nodes
Fixed, small, connected map — no pathfinding, no freeform terrain:
```
              BRAIN
             /     \
        VOICE       HEART
           |          |
          BODY       LUNGS
```
| Node | Governs | When damaged/overwhelmed |
|---|---|---|
| **Brain** | Thinking, Focus generation, memory-as-a-function-of-Brain | Focus generation falls; stronger dialogue unavailable; forgetting/repetition possible |
| **Lungs** | Oxygen production | Oxygen regen falls; sustained internal activity becomes harder |
| **Heart** | Emotional pressure, Composure | Faster heartbeat, nervousness, Composure pressure, stronger physical reactions |
| **Voice** | Communication | Strong dialogue unavailable, hesitant delivery, stuttering |
| **Body** | Visible physical symptoms | Sweating, fidgeting, shaking, visible nervousness — **now mechanically load-bearing, not cosmetic** (see Part 5, §5.7) |

### 2.4 Emotional dominance & mixtures
A node can be influenced by more than one emotion at once; whichever dominates shapes both the node's behavior and (per Part 5) the dialogue that results.
- If one emotion dominates a node's influence too long/too strongly, that emotion's personality bleeds into the character (Anxiety-dominance → hesitation/self-doubt; Confidence-dominance → certainty/bravado; Attraction-dominance → flirting/oversharing; Calm-dominance → restraint/possible distance).
- **Mixture detection rule (locked):** two emotions count as a named "mixture state" when both are within 15 percentage points of each other's influence on the primary target node, AND both exceed 25% influence individually. Otherwise, the single highest-influence emotion is the pure-dominant state. (Tunable constant — adjust 15/25 during playtesting, rule itself is fixed.)
- **10 total answer-selecting states:** the 4 core emotions + 6 named mixtures (final mixture list to be confirmed during content authoring — see Part 6 Open Items).

### 2.5 The four resources

| Resource | Type | Generated by | At 0... |
|---|---|---|---|
| **Oxygen** | Operational | Lungs | NOT instant failure — internal units become less efficient, recovery slows, Composure suffers if shortage continues |
| **Focus** | Cognitive | Brain | NOT instant failure — restricts eligible answers to the 4 core-emotion set only, mixture-state answers become unavailable (see Part 5 §5.4) |
| **Composure** | Global stability, 0–100 | Heart pressure, successful/failed interactions | **0 = Meltdown.** A real, separate failure state (see Part 5 §5.8) |
| **Connection** | Relationship score, 0–100 | Accumulated consequence of answers | **0 = Date Collapse**, date ends immediately |

Composure bands: 75–100 Stable · 50–74 Nervous · 25–49 Unstable · 1–24 Panic · 0 Meltdown.
Heart Rate is not a separate resource — it's the visible expression of current Composure, purely presentational.

### 2.6 Resource relationship discipline
Each resource answers one, and only one, question — they must never overlap in what they represent:
- Oxygen: "Can I keep the internal system operating?"
- Focus: "Can the character think clearly?"
- Composure: "How close are we to losing control?"
- Connection: "How well is the date going?"

### 2.7 Tempo (how fast each system moves)
Oxygen: slow/continuous · Focus: medium/tactical · Composure: medium/event-driven · Connection: slow/accumulated · Social Events: fast · Emotion units: fast · Dialogue timer: very fast.

---

## PART 3 — THE DATE STRUCTURE [STABLE — REPLACES ORIGINAL PHASE SYSTEM]

### 3.1 SUPERSEDED: original 8-phase structure
The original canonical PRD's §25 (Phases 1–8: Arrival → Small Talk → Comfort → Awkwardness → Personal Conversation → Emotional Climax → Final Question → Resolution) is **retired**. Explicit decision: *"no older phases, new phases 10."*

### 3.2 CURRENT: 10-round/slot structure
The date is now structured as **10 sequential question/scenario rounds ("slots")**, replacing phases entirely. Each slot is a self-contained unit carrying its own question set, Social Event Profile, and answer/reaction content (see Part 5). There is no separate phase-grouping layer above the 10 slots — narrative escalation (small talk → vulnerability) must be authored into the *content* of slots 1 through 10 directly, not enforced by a phase system.

**Open item (unresolved):** whether any of the 10 slots are wordless scenarios (Social Event only, no spoken question) rather than full questions. If so, the exact count of "spoken questions" vs "scenario-only beats" among the 10 is not yet fixed.

### 3.3 Run length
Target: roughly 10–15 minutes per complete run (carried over from original PRD, not yet re-validated against the new 10-slot structure — worth confirming once a vertical slice exists).

### 3.4 Endings
Determined by final Connection value at the end of slot 10, checked once (not slot-specific logic):
- 70–100 → SECOND DATE
- 40–69 → MAYBE
- 1–39 → AWKWARD ENDING
- 0 → DATE FAILED (Date Collapse — can occur at any slot, not just slot 10)
- **Meltdown ending** (Composure = 0) — a fifth, distinct ending screen, separate from the four Connection-based outcomes above (see Part 5 §5.8).

---

## PART 4 — SOCIAL EVENTS & STRESS [STABLE]

### 4.1 Social Event types (original)
From the original canonical PRD, still valid as the underlying Stress Event vocabulary:
- **Panic** (targets Brain) — reduces Focus generation, increases Anxiety pressure
- **Heart Flutter** (targets Heart) — increases Composure pressure, accelerates heartbeat
- **Awkward Silence** (targets Voice) — reduces communication quality
- **Overthinking** (targets Brain) — interferes with Focus, may cause memory/context mistakes
- **Sweat Surge** (targets Body) — visible physical embarrassment

### 4.2 NEW: Direct Emotion Push
A necessary addition beyond the original list. Not every Social Event should be a threat — a romantic or comfortable beat should be able to **directly raise** a named emotion (e.g., raise Attraction, raise Calm) rather than only ever relieving pressure. Required so that positive scenario beats don't mechanically read as "problems," which the original 5-type list (all negative) would otherwise force.

### 4.3 Social Event Profile (the RTS trigger system)
Every one of the 10 slots carries its own authored profile:
```
SocialEventProfile {
  target(s):   one or more of Brain / Voice / Heart / Body / Lungs
  effectType:  STRESS_EVENT (one of the 5 types in §4.1)
                 — or —
               DIRECT_EMOTION_PUSH (raises a named emotion directly)
  magnitude:   authored strength, tunable per slot
}
```
- Multiple nodes may be targeted simultaneously by one event — this is how late-run "multiple issues competing for attention" (original PRD §38) is actually implemented.
- **Node targeting across the 10 slots is randomly selected**, with the explicit design intent that question-authoring will naturally balance coverage across all 5 nodes rather than enforcing a hard "each node targeted at least once" rule.
- **Timing/intensity scaling across the run** (does magnitude increase toward slot 10?) — **not yet resolved.** Flagged for a tuning pass once a vertical slice exists.

### 4.4 No event overlap
Explicit decision: **no overlapping Social Events within or across rounds.** Each slot resolves fully (event → RTS response → answer → reaction) before the next slot's event fires. However, **RTS resource state is NOT reset between slots** — Oxygen, Focus, Composure, node health, and current dominant-emotion leanings all carry forward continuously from one slot into the next. Recovery happens only through the existing passive regen mechanics (§2.5) and successful-interaction bonuses (original PRD §31), never through an artificial full reset at slot boundaries.

---

## PART 5 — THE DIALOGUE & REACTION SYSTEM [STABLE]

*This section governs how questions are asked, how answers are chosen, and how the date reacts. It is the most-revised part of the design — see Part 6 for the decision history that produced it.*

### 5.1 The three independent variables
Never mixed, never allowed to influence each other outside these defined paths:
- **What disrupts the RTS layer** → each slot's authored Social Event Profile (Part 4.3).
- **What the player says** → driven only by RTS/emotion state at the moment of resolution. Connection never touches this text.
- **What happens next** → which question fires, and how the date reacts — both driven by Connection.

### 5.2 Core loop (authoritative version)
```
1. SOCIAL EVENT FIRES
   Slot's Social Event Profile → target node(s), effect type
   (Stress Event or Direct Emotion Push), magnitude

2. QUESTION FIRES (if this slot has a spoken question)
   Current Connection tier (5 tiers) → selects THIS slot's
   question variant authored for THIS tier
   → Tier 0 → no question; date ends immediately

3. PLAYER RESPONDS VIA DRAG-AND-DROP RTS
   Player drags emotion units onto nodes in real time,
   working against the Social Event before the response
   window (timer) expires

4. RESOLUTION (always = timer expiry, never early/manual)
   → Read dominant state on the PRIMARY target node
     (first-listed node in the Profile) — this determines
     the answer, even if secondary nodes were also targeted
   → If Focus is at/near 0: eligible answers are restricted
     to the 4 core emotions only (mixture-state answers
     unavailable)
   → If Body health is critically low: OVERRIDES the above —
     selects the Frozen/Blank answer regardless of what was
     dominant elsewhere (see §5.7)
   → If no state is dominant (tie, or nothing sent) at
     expiry: selects the Frozen/Blank answer (see §5.6)
   → Otherwise: selects the matching one of 10 pre-written
     emotion-state answers for this question
   → The selected answer carries its own pre-authored
     Connection delta, applied now

5. REACTION FIRES
   NEW post-answer Connection tier + WHICH QUESTION this was
   → selects one of this question's 4 pre-written reactions
     (one per live tier) — shared across whichever answer
     landed in that tier, not unique per answer
   → Reaction is tagged with one of ~20 shared animation clips
     (by tone/archetype, not 1-to-1 with the line)

6. LOOP
   Advance to next slot with new Connection/Composure/
   resource state carried forward (§4.4)
```

### 5.3 Question layer
- 10 slots total. Each slot has **4 question variants**, one per live Connection tier (Tier 0 has no question — date ends).
- Each tier's question is authored as a genuinely distinct psychological read of how the date is currently feeling — not a reskinned soft/neutral/harsh version of the same question.
- **Total: 40 questions** (10 slots × 4 tiers).
- Each question is authored together with its Social Event Profile as one unit.

### 5.4 Answer layer
- Each question has one answer per emotion/mixture state: **4 core + 6 named mixtures = 10 states.**
- **Total: 400 answers** (40 questions × 10 states).
- Each answer carries its own Connection delta (§5.2 step 4) — this is where the player's specific emotional choice still matters mechanically, even where the resulting reaction (§5.5) doesn't vary per-answer.
- Answers are authored once, displayed verbatim, never modified or wrapped at runtime.

### 5.5 Reaction layer
- Reaction depends on (which question) × (post-answer Connection tier) — **not** on which specific emotion-answer fired.
- **Total: 160 reactions** (40 questions × 4 live post-answer tiers).
- Two different emotion-answers to the same question, if their deltas land Connection in the same resulting tier, share the identical reaction — reaction reflects *how well the moment landed*, not *which emotion led the response* (that influence is already expressed through the answer text and its delta).
- Each reaction is tagged with one of ~20 shared animation clips.

### 5.6 The Frozen/Blank state
An 11th answer-state per question (not one of the 10 emotion-driven states), firing when:
- No emotion is dominant at resolution (tie, or nothing sent), OR
- Body health is critically low (overrides normal selection — see §5.7)
Represents genuine blankness/derailment — the character has nothing, an authentic silence or stumble, distinct from any of the 10 emotion-flavored answers.
**Adds 10 lines total** (1 per question) — total answer count is therefore **410**, not 400.

### 5.7 Body's mechanical role
Body is not cosmetic. Critically low Body health can hijack answer selection regardless of what the player was managing on the question's primary target node, forcing the Frozen/Blank response. This preserves the original canonical PRD's claim (§6) that visible physical symptoms "can influence the date's reaction," using the same content already committed to §5.6 rather than requiring new authored lines.

### 5.8 Composure / Meltdown (the second failure path)
Composure reaching 0 is a distinct failure state from Connection reaching 0, and was previously unaddressed in this system. Resolution:
- Meltdown does **not** get its own 160-line reaction set — it reuses the tonal register of the existing Tier-1 (1–39 Connection) reaction pool, since "losing control" and "the date going badly" are adjacent emotional territory.
- What IS new: **one dedicated Meltdown ending screen**, distinct from the four Connection-based endings (§3.4).

### 5.9 Total authoring volume (final, locked)

| Content type | Count | Notes |
|---|---|---|
| Social Event Profiles | 40 | Data tags (target/type/magnitude), not prose |
| Questions | 40 | 10 slots × 4 live tiers |
| Answers | 410 | 400 emotion-state answers + 10 Frozen/Blank |
| Connection Deltas | 410 | Data values attached to each answer |
| Reactions | 160 | 40 questions × 4 post-answer tiers |
| Reaction Animation Clips | ~20 | Shared pool, reused across all 160 reactions |
| Endings | 5 | 4 Connection-tier endings + 1 Meltdown ending |

**Total authored prose: 40 + 410 + 160 = 610 pieces.** Confirmed viable given AI-assisted drafting + a single human review/edit pass (~1hr estimated for the answer set). Social Event Profiles and Connection deltas are lightweight data, not separate writing.

### 5.10 Input method
**Drag-and-drop.** Player drags emotion units from a control area onto target nodes on the internal map in real time during the response window.

---

## PART 6 — RESOLVED DESIGN GAPS (DECISION LOG)

Every gap identified and closed during design. Kept as a log so no one re-litigates a settled decision without knowing it was already made deliberately.

| # | Gap | Resolution |
|---|---|---|
| 1 | What triggers answer resolution? | Timer expiry only — never early/manual confirm. Null/tie state → Frozen/Blank (§5.6). |
| 2 | Multi-node Social Events — which node's dominance picks the answer? | Primary (first-listed) target node in the Social Event Profile decides; secondary targets still affect resources. |
| 3 | Oxygen/Focus never wired into dialogue | Low Focus restricts eligible answers to the 4 core emotions (no mixture states) — a filter, not new content. |
| 4 | Composure/Meltdown had no path in this system | Reuses Tier-1 reaction tone; gets one new dedicated ending screen. |
| 5 | Does anything reset between slots? | No artificial reset — continuous regen per existing resource mechanics (§2.5, original PRD §31). |
| 6 | Mixture-state detection threshold undefined | Both emotions within 15pts of each other AND both >25% influence = mixture; else single highest = pure state. |
| 7 | Replay determinism (same inputs → same outputs, always) | Deliberately out of scope — accepted as consistent with a single predetermined narrative run, not a roguelike. |
| 8 | Does the final slot need special-case ending logic? | No — slot 10 uses the identical 4-tier system; ending triggers on final Connection check only. |
| 9 | Relationship between old 8-phase structure and new 10-slot structure | Old phases fully retired. 10 slots are the only structure now (Part 3). |
| 10 | Are Social Event node-targets balanced across the 10 slots? | Randomly selected; balance is an authoring-discipline goal, not an enforced rule. |
| 11 | Can Social Events overlap within/across rounds? Does RTS state reset between rounds? | No overlap, ever. RTS state carries continuously across all 10 slots — no reset. |
| 12 | What is the actual input method for the RTS layer? | Drag-and-drop. |
| 13 | Does Body health affect dialogue, or is it cosmetic? | Mechanically load-bearing — can override answer selection when critically low (§5.7). |

### Major system-level pivots (for context, not re-litigation)
- **Rejected:** runtime AI-generated dialogue (latency inside the timer, non-determinism, un-tunability — see original discussion). All content is predetermined.
- **Rejected:** player-chosen dialogue (4-option menu) — would have replaced the core "dialogue as readout" mechanic with a conventional VN choice system. Explicitly declined; RTS state alone determines the answer.
- **Rejected, then partially reconsidered:** a fully layered/templated answer-assembly system (competence tiers × reusable tone wrappers × reusable intensity modifiers) — abandoned in favor of direct, fully-authored answers per question×emotion, once AI-assisted drafting made the larger volume achievable.
- **Rejected:** per-answer unique reactions (400 of them) — replaced with per-(question×tier) reactions (160) once it was recognized that reaction should reflect *outcome*, not *which specific emotion led the response* (that's already expressed in the answer and its delta).

---

## PART 7 — OPEN ITEMS (GENUINELY UNRESOLVED)

Stated plainly rather than papered over:

1. **Final list of the 6 named emotion mixtures** (beyond the 4 core emotions) — not yet chosen.
2. **Whether any of the 10 slots are wordless scenarios** rather than spoken questions, and if so how many.
3. **Social Event timing/intensity scaling** across the run — flat throughout, or escalating toward slot 10? Not yet decided.
4. **Whether Direct Emotion Push events need a distinct visual/audio language** on the internal map (separate from Stress Event visuals) so players can tell a threat from an opportunity at a glance.
5. **10–15 minute run-length target** was carried over from the old 8-phase structure and has not been re-validated against the new 10-slot structure.
6. **Tonal risk** (Meltdown/timer/failure states reading as anxiety-simulation rather than comedy) — flagged, not addressed by any mechanical decision yet.

---

## PART 8 — WHAT DELIBERATELY STAYS SIMPLE

Carried over from the original canonical PRD, still valid: no character creation, no inventory, no permanent progression/XP/skill trees, no multiple date locations, no multiple dates, no procedural generation, no large unit counts, no realistic biological simulation, no dozens of emotions. Depth comes from interaction between the existing systems, not from adding more systems.