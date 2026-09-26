# CTRL+HEART — Content Merge Spec & AI Prompt
*Converts the two authored content docs (Player Replies v2 Graded, Girl Reactions 400 v1) into structured data Track A can load directly into ScriptableObjects. Use this doc to brief whatever AI/tool performs the merge — the prompt at the bottom is ready to paste in as-is, with the two source files attached.*

---

## 1. Known discrepancy — read before running this

The **Master Bible's §5.5 (Reaction Layer)** specifies 160 shared reactions (one per question × post-answer-tier, reused across whichever answer lands in that tier). The **actual authored content you have** — Girl Reactions 400 v1 — is one unique reaction **per answer**, i.e. 400 reactions, matching the later "Option A" decision made mid-project (every answer gets its own dedicated reaction, no sharing). **The 400-reaction version is what's real and what this merge spec builds against.** If anyone references the Bible's 160-reaction number later, it's stale on this specific point — flag it rather than silently reconciling in either direction.

---

## 2. Target Data Schema

Two linked tables, keyed so Track A's engine can look up an answer and its reaction in one pass.

### `Answer` record
```
{
  slotIndex:        int (1–10)
  tier:              int (1–4, matches Connection tier at time question fired)
  state:             enum — one of: Calm, Anxiety, Confidence, Attraction,
                     CalmAnxiety, CalmConfidence, CalmAttraction,
                     AnxietyConfidence, AnxietyAttraction, ConfidenceAttraction,
                     FrozenBlank
  answerText:        string (the player-character line, verbatim from source —
                     includes any *stage direction* asterisked text as part of
                     the same field; do not strip it)
  isWordless:        bool (true if answerText is ONLY a stage direction with
                     no spoken line — derive by checking whether the text
                     contains actual quoted dialogue or is purely descriptive)
  connectionDelta:   int (−20 to +20, from source "Delta" column)
  band:              enum — HighFit | Reasonable | Poor | ActivelyWrong | FrozenBlank
                     (from source "Band" column; FrozenBlank rows use this
                     literal band value, not one of the other four)
}
```

### `Reaction` record
```
{
  slotIndex:         int (1–10)
  tier:              int (1–4)
  state:             enum (same 11 values as Answer.state — reactions are
                     keyed 1:1 to a specific Answer record, not shared)
  reactionText:      string (the girl's reaction, verbatim from source —
                     includes stage direction and quoted dialogue together
                     as authored, do not split them into separate fields
                     unless Track A's engine specifically requires it)
  linkedAnswerDelta: int (the SAME delta value as the paired Answer record —
                     included here only for validation/cross-check during
                     import, not a second independent number)
}
```

### Linking rule
Every `Answer` record and its corresponding `Reaction` record share the same **(slotIndex, tier, state)** triple — that triple is the join key. **Note:** the Reactions source file does not repeat slotIndex/tier explicitly per row — it's implied by which `## T#` heading under which `# SLOT #` heading the row falls under. The merge process must carry that heading context down into every row parsed beneath it.

### Frozen/Blank handling
Frozen/Blank appears as a **single row per slot** in the Player Replies file (10 total, one per slot — confirm this against the source; in the sample shown it appears explicitly at Slot 1 T1 and Slot 10 T1, and per the doc's own "Authoring count" section there are 10 total across the whole file, one per slot). It is NOT tier-specific in the same way the other 10 states are — verify during merge whether each slot's Frozen/Blank row is tagged to a specific tier or is meant to apply slot-wide across all 4 tiers, since the two source files may not make this fully explicit. **Flag this specific ambiguity in the merge output rather than guessing a resolution.**

---

## 3. Field-by-field source mapping

| Target field | Source file | Source location |
|---|---|---|
| `slotIndex` | Both | Parsed from `# SLOT N — ...` heading |
| `tier` | Both | Parsed from `## T# — ...` heading (T4→tier 4, T3→tier 3, T2→tier 2, T1→tier 1) |
| `state` | Both | Leftmost table column, "State" — normalize `Calm+Anxiety` → `CalmAnxiety` (strip the `+`, no spaces) for the enum |
| `answerText` | Player Replies v2 Graded | "Answer" column |
| `connectionDelta` | Player Replies v2 Graded | "Delta" column (also cross-check against Reactions file's "Player Delta" column — **these must match exactly**; any row where they don't match is a data error to flag, not silently resolve) |
| `band` | Player Replies v2 Graded | "Band" column |
| `reactionText` | Girl Reactions 400 v1 | "Girl reaction" column |
| `isWordless` | Derived | Player Replies v2 Graded — true if `answerText` contains no straight-quoted dialogue (i.e., is entirely `*asterisked stage direction*`) |

---

## 4. Validation checks the merge process must run

1. **Row count check:** confirm exactly 400 Answer records (excluding Frozen/Blank) and exactly 400 Reaction records exist after parsing, matching the source docs' own stated "Authoring count" (400 emotion-state answers + 10 Frozen/Blank = 410 total answer lines).
2. **Delta cross-match:** for every (slotIndex, tier, state) triple, the delta value in the Answers source must equal the "Player Delta" value in the Reactions source. Report any mismatch by slot/tier/state rather than averaging or picking one value.
3. **Missing pair check:** every Answer record must have a matching Reaction record with the same (slotIndex, tier, state) key, and vice versa. Report any orphan on either side.
4. **Band-range check:** confirm every delta actually falls inside the band its "Band" column claims (High Fit +10 to +20, Reasonable 0 to +9, Poor −1 to −10, Actively Wrong −11 to −20) — flag any row where the stated band and the numeric delta disagree.
5. **State enum completeness:** confirm all 10 non-Frozen states appear exactly once per (slotIndex, tier) — i.e., no slot/tier is missing a state or has a duplicate.

---

## 5. The actual prompt (paste this to the merge AI, with both source files attached)

```
You are converting two authored content documents for a Unity game into
structured JSON data for ScriptableObject import. Do not rewrite, rephrase,
or "improve" any dialogue text — this is a literal parse-and-restructure
task, not a content-editing task.

INPUTS:
1. "CTRL-HEART-Player-Replies-v2-Graded.md" — contains player-character
   answer lines, organized by Slot (1-10) and Tier (T4/T3/T2/T1), one table
   per slot/tier with 10 emotion-state rows plus occasional Frozen/Blank rows.
2. "CTRL-HEART-Girl-Reactions-400-v1.md" — contains the girl's reaction to
   each of those same 400 answers, organized identically by Slot/Tier, with
   a "Player Delta" column that should exactly match the Delta column in
   file 1 for the same row.

TASK:
Produce a single JSON array of merged records, one per (slot, tier, state)
combination, with this exact shape:

{
  "slotIndex": <int 1-10>,
  "tier": <int 1-4>,
  "state": "<one of: Calm, Anxiety, Confidence, Attraction, CalmAnxiety,
            CalmConfidence, CalmAttraction, AnxietyConfidence,
            AnxietyAttraction, ConfidenceAttraction, FrozenBlank>",
  "answerText": "<verbatim from file 1's Answer column, including any
                 *asterisked stage direction* exactly as written>",
  "isWordless": <true if answerText contains no straight-quoted dialogue,
                false otherwise>,
  "connectionDelta": <int, from file 1's Delta column>,
  "band": "<one of: HighFit, Reasonable, Poor, ActivelyWrong, FrozenBlank>",
  "reactionText": "<verbatim from file 2's Girl reaction column for the
                   matching slot/tier/state>"
}

RULES:
- Do not alter any dialogue text. Preserve exact wording, punctuation, and
  stage-direction asterisks from both source files.
- Normalize state names by removing "+" and spaces (e.g. "Calm+Anxiety"
  becomes "CalmAnxiety") but do not otherwise change how a state is
  identified.
- Carry the Slot and Tier context from each section heading down into
  every row parsed beneath it — the source tables do not repeat this
  per-row.
- For every row, cross-check that file 1's Delta value and file 2's
  Player Delta value are identical for the same slot/tier/state. If they
  ever disagree, do NOT silently pick one — instead add an "error" field
  to that record noting the mismatch and both values, and still include
  the record with file 1's delta as the primary value.
- After producing the full array, output a short validation summary
  stating: total record count, whether it equals 410 (400 + 10 Frozen/
  Blank), any delta mismatches found, any slot/tier missing a state, and
  any band-vs-delta range disagreement (per the bands: HighFit +10 to
  +20, Reasonable 0 to +9, Poor -1 to -10, ActivelyWrong -11 to -20).
- If Frozen/Blank rows are not clearly tagged to a specific tier in
  either source file, do not guess — flag this explicitly in the
  validation summary and include the Frozen/Blank record with tier set
  to null, noting it needs manual resolution.

Output the JSON array first, then the validation summary as plain text
below it.
```

---

## 6. What to do with the output

Once the merge AI returns the JSON + validation summary:
1. **Read the validation summary first.** If it reports any delta mismatches, missing states, or band disagreements, those need a manual fix in the source `.md` files before re-running — don't let Track A import data with known inconsistencies.
2. Hand the clean JSON to Track A to write an import script that populates the `Answer`/`Reaction` ScriptableObjects per the Master Bible's §5.3-§5.5 data contract (the Dev Plan's Day 1 schema lock).
3. Re-run this same prompt against Slots 2-10 content in the same two source files — the prompt is written to handle the whole document, so if both files already contain all 10 slots (they appear to, based on the truncated view), one run should cover everything at once.
