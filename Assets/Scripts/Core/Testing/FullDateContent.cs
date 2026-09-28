using System.Collections.Generic;
using UnityEngine;
using CtrlHeart.Core.Data;
using CtrlHeart.Core.Data.ScriptableObjects;

namespace CtrlHeart.Core.Testing
{
    /// <summary>
    /// Master content repository for the complete 10 slots of CTRL+HEART.
    /// Authored per 40 Scenario Draft v2, Player Replies v2 Graded, and Girl Reactions 400 v1.
    /// Contains:
    /// - 40 QuestionData scenarios (Slots 1-10, Tiers 1-4) with compound RTS profiles
    /// - 440 AnswerData player lines (10 emotion states + Frozen/Blank per scenario)
    /// - 440 ReactionData girl reactions (1:1 per player answer)
    /// </summary>
    public static class FullDateContent
    {
        public static (List<QuestionData> questions, List<AnswerData> answers, List<ReactionData> reactions) CreateFull10SlotsContent()
        {
            var questions = new List<QuestionData>();
            var answers = new List<AnswerData>();
            var reactions = new List<ReactionData>();

            for (int s = 1; s <= 10; s++)
            {
                var (slotQ, slotA, slotR) = CreateSlotContent(s);
                questions.AddRange(slotQ);
                answers.AddRange(slotA);
                reactions.AddRange(slotR);
            }

            return (questions, answers, reactions);
        }

        public static (List<QuestionData> questions, List<AnswerData> answers, List<ReactionData> reactions) CreateSlotContent(int slotIndex)
        {
            return slotIndex switch
            {
                1 => BuildSlot1(),
                2 => BuildSlot2(),
                3 => BuildSlot3(),
                4 => BuildSlot4(),
                5 => BuildSlot5(),
                6 => BuildSlot6(),
                7 => BuildSlot7(),
                8 => BuildSlot8(),
                9 => BuildSlot9(),
                10 => BuildSlot10(),
                _ => (new List<QuestionData>(), new List<AnswerData>(), new List<ReactionData>())
            };
        }


        private static (List<QuestionData>, List<AnswerData>, List<ReactionData>) BuildSlot1()
        {
            var questions = new List<QuestionData>();
            var answers = new List<AnswerData>();
            var reactions = new List<ReactionData>();

            // --- Slot 1 Tier 4: Type 2 – Compliment ---
            var q1T4 = ScriptableObject.CreateInstance<QuestionData>();
            q1T4.slotIndex = 1;
            q1T4.connectionTier = ConnectionTier.Tier4_SecondDate;
            q1T4.scenarioType = "Type 2 – Compliment";
            q1T4.questionText = "\"Okay, you clean up well. I wasn't expecting that.\" *She says it lightly, already smiling.*";
            q1T4.responseTimeWindow = 6.0f;
            q1T4.socialEventProfile = new SocialEventProfile
            {
                requiresPlayerInitiation = false,
                primaryNodeOverride = InternalNodeType.Heart,
                effects = new List<TargetedEffect>
                {
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Heart,
                        effectType = SocialEffectType.DirectEmotionPush,
                        pushTargetEmotion = CoreEmotion.Attraction,
                        magnitude = 15.0f
                    },
                }
            };
            questions.Add(q1T4);

            answers.Add(CreateAnswer(q1T4, EmotionState.Calm, "“Thanks. You look really good too.”", 6f, "Reasonable", false));
            reactions.Add(CreateReaction(q1T4, EmotionState.Calm, ConnectionTier.Tier4_SecondDate, "She smiles, taking the compliment back easily. “Thanks. I was hoping you’d think so.”", 6f, "thinking_chintap"));
            answers.Add(CreateAnswer(q1T4, EmotionState.Anxiety, "“Oh—thanks. I, uh... wasn't sure what to wear.”", 6f, "Reasonable", false));
            reactions.Add(CreateReaction(q1T4, EmotionState.Anxiety, ConnectionTier.Tier4_SecondDate, "Her smile softens. “No, I meant it. You look good.” She gives him an easy out rather than pushing it.", 6f, "sympathetic_concern"));
            answers.Add(CreateAnswer(q1T4, EmotionState.Confidence, "“Thanks. I figured I should make an effort.”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q1T4, EmotionState.Confidence, ConnectionTier.Tier4_SecondDate, "She laughs lightly. “Okay, fair.” A tiny eyebrow raise: she was being nice, not issuing a challenge.", 4f, "nervous_laugh"));
            answers.Add(CreateAnswer(q1T4, EmotionState.Attraction, "*He smiles, a little too obviously pleased.* “Thanks.”", 14f, "HighFit", false));
            reactions.Add(CreateReaction(q1T4, EmotionState.Attraction, ConnectionTier.Tier4_SecondDate, "She catches how pleased he looks and smiles. “There it is.”", 14f, "warm_interest"));
            answers.Add(CreateAnswer(q1T4, EmotionState.CalmAnxiety, "“Thanks. I was a little worried I'd overthought the whole outfit.”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q1T4, EmotionState.CalmAnxiety, ConnectionTier.Tier4_SecondDate, "“See? Then I wasn’t imagining the effort.” She grins and moves on.", 4f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q1T4, EmotionState.CalmConfidence, "“Thanks. I’m glad I got it right.”", 12f, "HighFit", false));
            reactions.Add(CreateReaction(q1T4, EmotionState.CalmConfidence, ConnectionTier.Tier4_SecondDate, "She holds his eyes a second longer. “Good. I was hoping you’d know I meant it.”", 12f, "warm_interest"));
            answers.Add(CreateAnswer(q1T4, EmotionState.CalmAttraction, "*A small smile.* “Thanks. You look really nice.”", 16f, "HighFit", false));
            reactions.Add(CreateReaction(q1T4, EmotionState.CalmAttraction, ConnectionTier.Tier4_SecondDate, "Her smile warms. “Thank you.” She looks him over once more, not hiding that she means it.", 16f, "second_date"));
            answers.Add(CreateAnswer(q1T4, EmotionState.AnxietyConfidence, "“Yeah, I mean—obviously I was going to look good. Thanks.” *He laughs at himself.*", 8f, "Reasonable", false));
            reactions.Add(CreateReaction(q1T4, EmotionState.AnxietyConfidence, ConnectionTier.Tier4_SecondDate, "She gives a small laugh, but the warmth drops a notch. “Okay... I was just complimenting you.”", 8f, "nervous_laugh"));
            answers.Add(CreateAnswer(q1T4, EmotionState.AnxietyAttraction, "“Really? Oh. Thanks... that’s actually really nice to hear.”", 6f, "Reasonable", false));
            reactions.Add(CreateReaction(q1T4, EmotionState.AnxietyAttraction, ConnectionTier.Tier4_SecondDate, "“You’re welcome.” She smiles reassuringly, noticing how much the compliment landed.", 6f, "sympathetic_concern"));
            answers.Add(CreateAnswer(q1T4, EmotionState.ConfidenceAttraction, "“Good. I was hoping I’d make a decent first impression.”", 14f, "HighFit", false));
            reactions.Add(CreateReaction(q1T4, EmotionState.ConfidenceAttraction, ConnectionTier.Tier4_SecondDate, "She grins. “Good first impression, then.” The teasing tone keeps the moment light.", 14f, "flirty_wink"));
            answers.Add(CreateAnswer(q1T4, EmotionState.FrozenBlank, "*He blinks, suddenly lost for words, smiling flustered.*", 6f, "Reasonable", true));
            reactions.Add(CreateReaction(q1T4, EmotionState.FrozenBlank, ConnectionTier.Tier4_SecondDate, "She chuckles warmly at his fluster. “Cat got your tongue already? Take your time.”", 6f, "nervous_laugh"));

            // --- Slot 1 Tier 3: Type 1 – Direct question ---
            var q1T3 = ScriptableObject.CreateInstance<QuestionData>();
            q1T3.slotIndex = 1;
            q1T3.connectionTier = ConnectionTier.Tier3_Strong;
            q1T3.scenarioType = "Type 1 – Direct question";
            q1T3.questionText = "\"So how was your day, actually — not the polite version.\"";
            q1T3.responseTimeWindow = 6.0f;
            q1T3.socialEventProfile = new SocialEventProfile
            {
                requiresPlayerInitiation = false,
                primaryNodeOverride = InternalNodeType.Brain,
                effects = new List<TargetedEffect>
                {
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Brain,
                        effectType = SocialEffectType.StressEvent,
                        stressEventSubtype = StressEventSubtype.Panic,
                        magnitude = 15.0f
                    },
                }
            };
            questions.Add(q1T3);

            answers.Add(CreateAnswer(q1T3, EmotionState.Calm, "“Pretty busy. Nothing dramatic, just one of those days. I’m good now, though.”", -2f, "Poor", false));
            reactions.Add(CreateReaction(q1T3, EmotionState.Calm, ConnectionTier.Tier3_Strong, "She nods, listening properly. “Yeah, I know those days.” She follows naturally into another topic.", -2f, "discomfort"));
            answers.Add(CreateAnswer(q1T3, EmotionState.Anxiety, "“It was fine... I mean, mostly. I had a meeting that went weird, then I started thinking about tonight and—yeah, anyway.”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q1T3, EmotionState.Anxiety, ConnectionTier.Tier3_Strong, "She watches him ramble for a second, then gently cuts in. “Hey, you don’t have to give me the whole play-by-play.”", 4f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q1T3, EmotionState.Confidence, "“Long day, but I handled it. I’m here now, so it worked out.”", 6f, "Reasonable", false));
            reactions.Add(CreateReaction(q1T3, EmotionState.Confidence, ConnectionTier.Tier3_Strong, "She smiles politely. “Sounds like you handled it.” Interested, but she does not chase the bragging angle.", 6f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q1T3, EmotionState.Attraction, "“Honestly? A bit of a mess. I was mostly looking forward to this.”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q1T3, EmotionState.Attraction, ConnectionTier.Tier3_Strong, "“So you were thinking about tonight all day?” she asks with a small smile.", 4f, "thinking_chintap"));
            answers.Add(CreateAnswer(q1T3, EmotionState.CalmAnxiety, "“Kind of a lot. I’m fine, just still coming down from it a little.”", 14f, "HighFit", false));
            reactions.Add(CreateReaction(q1T3, EmotionState.CalmAnxiety, ConnectionTier.Tier3_Strong, "She nods. “Yeah. I get that.” The honesty seems to make her relax too.", 14f, "warm_interest"));
            answers.Add(CreateAnswer(q1T3, EmotionState.CalmConfidence, "“Busy, a little chaotic. Nothing I couldn’t deal with.”", 16f, "HighFit", false));
            reactions.Add(CreateReaction(q1T3, EmotionState.CalmConfidence, ConnectionTier.Tier3_Strong, "“That’s a good way to put it.” She looks more settled, happy to keep talking.", 16f, "second_date"));
            answers.Add(CreateAnswer(q1T3, EmotionState.CalmAttraction, "“Long one. But honestly, sitting here with you already feels like a better part of it.”", 8f, "Reasonable", false));
            reactions.Add(CreateReaction(q1T3, EmotionState.CalmAttraction, ConnectionTier.Tier3_Strong, "Her expression softens. “Okay, that’s actually nice to hear.” She keeps the conversation there a little longer.", 8f, "sympathetic_concern"));
            answers.Add(CreateAnswer(q1T3, EmotionState.AnxietyConfidence, "“It was totally fine. Just work being work. Nothing got to me.” *He pauses.* “Much.”", 10f, "HighFit", false));
            reactions.Add(CreateReaction(q1T3, EmotionState.AnxietyConfidence, ConnectionTier.Tier3_Strong, "She catches the crack in the confidence and smiles gently. “Well... it sounds like you survived it.”", 10f, "warm_interest"));
            answers.Add(CreateAnswer(q1T3, EmotionState.AnxietyAttraction, "“It was rough, actually. I was nervous about tonight too, so... not exactly my calmest day.”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q1T3, EmotionState.AnxietyAttraction, ConnectionTier.Tier3_Strong, "“You were nervous about tonight too?” There is curiosity in it, but she notices the strain.", 4f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q1T3, EmotionState.ConfidenceAttraction, "“Kind of brutal. Then I got here, so I’m counting that as the turnaround.”", 10f, "HighFit", false));
            reactions.Add(CreateReaction(q1T3, EmotionState.ConfidenceAttraction, ConnectionTier.Tier3_Strong, "She laughs. “Okay, so I’m the turnaround.” It lands as cute, but a little rehearsed.", 10f, "flirty_wink"));
            answers.Add(CreateAnswer(q1T3, EmotionState.FrozenBlank, "*He starts to speak, hesitates, and rubs the back of his neck silently.*", -4f, "Poor", true));
            reactions.Add(CreateReaction(q1T3, EmotionState.FrozenBlank, ConnectionTier.Tier3_Strong, "She smiles gently. “No worries if it’s too much to unpack right now.”", -4f, "uncomfortable"));

            // --- Slot 1 Tier 2: Type 9 – Reflective/distracted, wordless ---
            var q1T2 = ScriptableObject.CreateInstance<QuestionData>();
            q1T2.slotIndex = 1;
            q1T2.connectionTier = ConnectionTier.Tier2_Maybe;
            q1T2.scenarioType = "Type 9 – Reflective/distracted, wordless";
            q1T2.questionText = "*She glances around the room, half-listening, not unkindly — just settling in.*";
            q1T2.responseTimeWindow = 6.0f;
            q1T2.socialEventProfile = new SocialEventProfile
            {
                requiresPlayerInitiation = false,
                primaryNodeOverride = InternalNodeType.Voice,
                effects = new List<TargetedEffect>
                {
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Voice,
                        effectType = SocialEffectType.StressEvent,
                        stressEventSubtype = StressEventSubtype.AwkwardSilence,
                        magnitude = 15.0f
                    },
                }
            };
            questions.Add(q1T2);

            answers.Add(CreateAnswer(q1T2, EmotionState.Calm, "*He takes a sip and lets the silence sit.*", -6f, "Poor", true));
            reactions.Add(CreateReaction(q1T2, EmotionState.Calm, ConnectionTier.Tier2_Maybe, "She glances back and smiles, comfortable enough not to rush either of them.", -6f, "discomfort"));
            answers.Add(CreateAnswer(q1T2, EmotionState.Anxiety, "“So... yeah. This place is nice. I checked out the menu before I came, which is probably unnecessary, but—”", -8f, "Poor", false));
            reactions.Add(CreateReaction(q1T2, EmotionState.Anxiety, ConnectionTier.Tier2_Maybe, "She looks over as he fills the silence. “You really don’t have to keep talking.” It is kind, but noticeable.", -8f, "awkward_silence"));
            answers.Add(CreateAnswer(q1T2, EmotionState.Confidence, "*He leans back, relaxed, and gives her the space without filling it.*", 14f, "HighFit", true));
            reactions.Add(CreateReaction(q1T2, EmotionState.Confidence, ConnectionTier.Tier2_Maybe, "She notices how relaxed he is and gives a small approving smile.", 14f, "warm_interest"));
            answers.Add(CreateAnswer(q1T2, EmotionState.Attraction, "*He catches her eye and smiles, comfortable letting the moment stay quiet.*", 2f, "Reasonable", true));
            reactions.Add(CreateReaction(q1T2, EmotionState.Attraction, ConnectionTier.Tier2_Maybe, "She meets his eyes briefly, then looks away; the smile is a little uncertain.", 2f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q1T2, EmotionState.CalmAnxiety, "“Take your time. I’m good.”", 0f, "Reasonable", false));
            reactions.Add(CreateReaction(q1T2, EmotionState.CalmAnxiety, ConnectionTier.Tier2_Maybe, "“I’m okay.” She smiles, but the extra reassurance makes her wonder if he is not.", 0f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q1T2, EmotionState.CalmConfidence, "*He settles back, totally at ease with the quiet.*", 8f, "Reasonable", true));
            reactions.Add(CreateReaction(q1T2, EmotionState.CalmConfidence, ConnectionTier.Tier2_Maybe, "She settles back too, letting the quiet become comfortable instead of awkward.", 8f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q1T2, EmotionState.CalmAttraction, "*He watches her for a second, smiling softly, then takes another sip.*", 2f, "Reasonable", true));
            reactions.Add(CreateReaction(q1T2, EmotionState.CalmAttraction, ConnectionTier.Tier2_Maybe, "Their eyes meet. She smiles softly, then looks around again without feeling the need to fill it.", 2f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q1T2, EmotionState.AnxietyConfidence, "“You’re good. I wasn’t about to panic over two seconds of silence.” *A beat.* “Not yet, anyway.”", 8f, "Reasonable", false));
            reactions.Add(CreateReaction(q1T2, EmotionState.AnxietyConfidence, ConnectionTier.Tier2_Maybe, "She laughs once at the “not yet.” “Good to know,” she says, a little carefully.", 8f, "nervous_laugh"));
            answers.Add(CreateAnswer(q1T2, EmotionState.AnxietyAttraction, "*He smiles, looks down at his drink, then back at her, clearly trying not to overthink the silence.*", -4f, "Poor", true));
            reactions.Add(CreateReaction(q1T2, EmotionState.AnxietyAttraction, ConnectionTier.Tier2_Maybe, "She notices him fighting the silence and gives him a sympathetic smile, but the moment loses some ease.", -4f, "awkward_silence"));
            answers.Add(CreateAnswer(q1T2, EmotionState.ConfidenceAttraction, "*A small grin.* “You can have a minute.”", 10f, "HighFit", false));
            reactions.Add(CreateReaction(q1T2, EmotionState.ConfidenceAttraction, ConnectionTier.Tier2_Maybe, "She smiles at the permission. “Thanks.” The tension in her shoulders eases a little.", 10f, "warm_interest"));
            answers.Add(CreateAnswer(q1T2, EmotionState.FrozenBlank, "*He freezes completely, staring down at the table until the silence thickens.*", -8f, "Poor", true));
            reactions.Add(CreateReaction(q1T2, EmotionState.FrozenBlank, ConnectionTier.Tier2_Maybe, "She blinks, glancing back over at his frozen posture, looking slightly puzzled.", -8f, "confused"));

            // --- Slot 1 Tier 1: Type 7 – She notices something ---
            var q1T1 = ScriptableObject.CreateInstance<QuestionData>();
            q1T1.slotIndex = 1;
            q1T1.connectionTier = ConnectionTier.Tier1_Awkward;
            q1T1.scenarioType = "Type 7 – She notices something";
            q1T1.questionText = "\"You okay? You seem kind of tense already.\"";
            q1T1.responseTimeWindow = 6.0f;
            q1T1.socialEventProfile = new SocialEventProfile
            {
                requiresPlayerInitiation = false,
                primaryNodeOverride = InternalNodeType.Body,
                effects = new List<TargetedEffect>
                {
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Body,
                        effectType = SocialEffectType.StressEvent,
                        stressEventSubtype = StressEventSubtype.SweatSurge,
                        magnitude = 15.0f
                    },
                }
            };
            questions.Add(q1T1);

            answers.Add(CreateAnswer(q1T1, EmotionState.Calm, "“Yeah, I’m okay. Just first-date nerves. They’ll go away.”", -6f, "Poor", false));
            reactions.Add(CreateReaction(q1T1, EmotionState.Calm, ConnectionTier.Tier1_Awkward, "“Okay.” She smiles. “I just wanted to make sure you were alright.”", -6f, "discomfort"));
            answers.Add(CreateAnswer(q1T1, EmotionState.Anxiety, "“Do I? Sorry. I didn’t realise it was that obvious.”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q1T1, EmotionState.Anxiety, ConnectionTier.Tier1_Awkward, "“Hey, it’s okay.” She lowers her voice, trying to stop him from getting more self-conscious.", 4f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q1T1, EmotionState.Confidence, "“Yeah. I’m fine. Just settling in.”", -14f, "ActivelyWrong", false));
            reactions.Add(CreateReaction(q1T1, EmotionState.Confidence, ConnectionTier.Tier1_Awkward, "She nods, but the quick dismissal makes her wonder what he is covering.", -14f, "shut_down"));
            answers.Add(CreateAnswer(q1T1, EmotionState.Attraction, "“Maybe a little. You’re kind of hard not to be nervous around.”", 0f, "Reasonable", false));
            reactions.Add(CreateReaction(q1T1, EmotionState.Attraction, ConnectionTier.Tier1_Awkward, "She smiles. “Good. I was a little worried I was intimidating you.”", 0f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q1T1, EmotionState.CalmAnxiety, "“A little, yeah. Nothing bad. I just need a second to settle.”", 16f, "HighFit", false));
            reactions.Add(CreateReaction(q1T1, EmotionState.CalmAnxiety, ConnectionTier.Tier1_Awkward, "“Okay. Take your second.” She does exactly that, giving him room without making it awkward.", 16f, "second_date"));
            answers.Add(CreateAnswer(q1T1, EmotionState.CalmConfidence, "“Yeah, a little. I’m good, though.”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q1T1, EmotionState.CalmConfidence, ConnectionTier.Tier1_Awkward, "She visibly relaxes. “Good.” Then, with a smile: “Because I was starting to feel nervous for you.”", 4f, "relieved_sigh"));
            answers.Add(CreateAnswer(q1T1, EmotionState.CalmAttraction, "“A little. I think it’s mostly because I really wanted to meet you.”", -4f, "Poor", false));
            reactions.Add(CreateReaction(q1T1, EmotionState.CalmAttraction, ConnectionTier.Tier1_Awkward, "Her expression softens. “That’s actually kind of sweet.” She looks more comfortable herself.", -4f, "sympathetic_concern"));
            answers.Add(CreateAnswer(q1T1, EmotionState.AnxietyConfidence, "“Tense? No. I’m completely fine.” *He immediately fidgets with his glass.* “See?”", 6f, "Reasonable", false));
            reactions.Add(CreateReaction(q1T1, EmotionState.AnxietyConfidence, ConnectionTier.Tier1_Awkward, "She notices the fidget immediately. “You don’t have to convince me.” Her tone stays gentle, but she is now watching closely.", 6f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q1T1, EmotionState.AnxietyAttraction, "“A little, yeah. I really wanted tonight to go well, so...”", 8f, "Reasonable", false));
            reactions.Add(CreateReaction(q1T1, EmotionState.AnxietyAttraction, ConnectionTier.Tier1_Awkward, "“Okay.” She gives him a reassuring smile. “We can just take it easy.”", 8f, "sympathetic_concern"));
            answers.Add(CreateAnswer(q1T1, EmotionState.ConfidenceAttraction, "“Guilty. You have that effect on me, apparently.”", -10f, "ActivelyWrong", false));
            reactions.Add(CreateReaction(q1T1, EmotionState.ConfidenceAttraction, ConnectionTier.Tier1_Awkward, "She laughs softly. “Apparently.” The flirtation works, but she does not quite know whether he is joking.", -10f, "discomfort"));
            answers.Add(CreateAnswer(q1T1, EmotionState.FrozenBlank, "*He straightens too quickly, nods, and says nothing.*", -8f, "Poor", true));
            reactions.Add(CreateReaction(q1T1, EmotionState.FrozenBlank, ConnectionTier.Tier1_Awkward, "She frowns slightly with genuine concern. “Okay... just breathe, it’s not an interrogation.”", -8f, "sympathetic_concern"));

            return (questions, answers, reactions);
        }


        private static (List<QuestionData>, List<AnswerData>, List<ReactionData>) BuildSlot2()
        {
            var questions = new List<QuestionData>();
            var answers = new List<AnswerData>();
            var reactions = new List<ReactionData>();

            // --- Slot 2 Tier 4: Type 8 – Teasing/playful, wordless-into-verbal ---
            var q2T4 = ScriptableObject.CreateInstance<QuestionData>();
            q2T4.slotIndex = 2;
            q2T4.connectionTier = ConnectionTier.Tier4_SecondDate;
            q2T4.scenarioType = "Type 8 – Teasing/playful, wordless-into-verbal";
            q2T4.questionText = "*She smirks, tilts her head, waits a beat before speaking.* \"Okay, hot take — you've got a weird useless talent, don't you?\"";
            q2T4.responseTimeWindow = 6.0f;
            q2T4.socialEventProfile = new SocialEventProfile
            {
                requiresPlayerInitiation = false,
                primaryNodeOverride = InternalNodeType.Voice,
                effects = new List<TargetedEffect>
                {
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Voice,
                        effectType = SocialEffectType.DirectEmotionPush,
                        pushTargetEmotion = CoreEmotion.Confidence,
                        magnitude = 25.0f
                    },
                }
            };
            questions.Add(q2T4);

            answers.Add(CreateAnswer(q2T4, EmotionState.Calm, "“I can fold a fitted sheet properly. That counts.”", -4f, "Poor", false));
            reactions.Add(CreateReaction(q2T4, EmotionState.Calm, ConnectionTier.Tier4_SecondDate, "“That absolutely counts.” She laughs, already thinking of one to trade back.", -4f, "discomfort"));
            answers.Add(CreateAnswer(q2T4, EmotionState.Anxiety, "“I mean... probably? I have a few useless ones. None I can demonstrate under pressure.”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q2T4, EmotionState.Anxiety, ConnectionTier.Tier4_SecondDate, "“You have to demonstrate one?” She laughs, trying to rescue him from the pressure he just put on himself.", 4f, "nervous_laugh"));
            answers.Add(CreateAnswer(q2T4, EmotionState.Confidence, "“Absolutely. And you’re going to regret asking.”", 18f, "HighFit", false));
            reactions.Add(CreateReaction(q2T4, EmotionState.Confidence, ConnectionTier.Tier4_SecondDate, "Her eyes light up. “Oh, now you definitely have to tell me.”", 18f, "second_date"));
            answers.Add(CreateAnswer(q2T4, EmotionState.Attraction, "*He smiles.* “I’ve got one. I might save it for later.”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q2T4, EmotionState.Attraction, ConnectionTier.Tier4_SecondDate, "She gives him a look. “Later?” A tiny smile, but she is not sure whether he is flirting or dodging.", 4f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q2T4, EmotionState.CalmAnxiety, "“I do, actually. It’s stupid, but I’m weirdly good at it.”", 2f, "Reasonable", false));
            reactions.Add(CreateReaction(q2T4, EmotionState.CalmAnxiety, ConnectionTier.Tier4_SecondDate, "“That’s actually cute.” She smiles, though she can tell he is more embarrassed than the joke needed.", 2f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q2T4, EmotionState.CalmConfidence, "“Yeah. I can open almost any snack bag without ripping it.”", 8f, "Reasonable", false));
            reactions.Add(CreateReaction(q2T4, EmotionState.CalmConfidence, ConnectionTier.Tier4_SecondDate, "She laughs. “Okay, that one is genuinely useful.” She looks pleased with the exchange.", 8f, "nervous_laugh"));
            answers.Add(CreateAnswer(q2T4, EmotionState.CalmAttraction, "“I’ve got one, but I’m pretty sure it only becomes impressive after you’ve known me for a while.”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q2T4, EmotionState.CalmAttraction, ConnectionTier.Tier4_SecondDate, "“Oh, so there’s a mysterious version of you now?” she teases, amused but unconvinced.", 4f, "relieved_sigh"));
            answers.Add(CreateAnswer(q2T4, EmotionState.AnxietyConfidence, "“Oh, definitely. I’ve got several.” *Beat.* “I’m not nervous about this question, if that’s what you’re testing.”", 14f, "HighFit", false));
            reactions.Add(CreateReaction(q2T4, EmotionState.AnxietyConfidence, ConnectionTier.Tier4_SecondDate, "She blinks, then laughs once. “I wasn’t testing you.” The playful energy drops slightly.", 14f, "flirty_wink"));
            answers.Add(CreateAnswer(q2T4, EmotionState.AnxietyAttraction, "“There is one... but telling you about it this early feels like giving away too much.”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q2T4, EmotionState.AnxietyAttraction, ConnectionTier.Tier4_SecondDate, "“You can tell me later.” She smiles kindly, sensing he is protecting himself.", 4f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q2T4, EmotionState.ConfidenceAttraction, "“I do. You’ll have to earn the demonstration.”", 18f, "HighFit", false));
            reactions.Add(CreateReaction(q2T4, EmotionState.ConfidenceAttraction, ConnectionTier.Tier4_SecondDate, "“Oh, I’m absolutely earning that demonstration.” She grins, matching the playfulness.", 18f, "second_date"));
            answers.Add(CreateAnswer(q2T4, EmotionState.FrozenBlank, "*He freezes with a sheepish grin, mind completely blanking on the spot.*", 8f, "Reasonable", true));
            reactions.Add(CreateReaction(q2T4, EmotionState.FrozenBlank, ConnectionTier.Tier4_SecondDate, "She laughs delightedly. “Oh no, did I put you on the spot? I'll let you off the hook for now.”", 8f, "nervous_laugh"));

            // --- Slot 2 Tier 3: Type 6 – Physical mishap ---
            var q2T3 = ScriptableObject.CreateInstance<QuestionData>();
            q2T3.slotIndex = 2;
            q2T3.connectionTier = ConnectionTier.Tier3_Strong;
            q2T3.scenarioType = "Type 6 – Physical mishap";
            q2T3.questionText = "*She nearly drops her napkin, laughs at herself reaching for it.*";
            q2T3.responseTimeWindow = 6.0f;
            q2T3.socialEventProfile = new SocialEventProfile
            {
                requiresPlayerInitiation = false,
                primaryNodeOverride = InternalNodeType.Heart,
                effects = new List<TargetedEffect>
                {
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Heart,
                        effectType = SocialEffectType.StressEvent,
                        stressEventSubtype = StressEventSubtype.HeartFlutter,
                        magnitude = 15.0f
                    },
                }
            };
            questions.Add(q2T3);

            answers.Add(CreateAnswer(q2T3, EmotionState.Calm, "“You’re fine.” *He smiles and keeps eating.*", 10f, "HighFit", false));
            reactions.Add(CreateReaction(q2T3, EmotionState.Calm, ConnectionTier.Tier3_Strong, "“I know.” She laughs at herself and relaxes back into the conversation.", 10f, "flirty_wink"));
            answers.Add(CreateAnswer(q2T3, EmotionState.Anxiety, "“Oh—careful. Sorry, I don’t know why I said sorry.”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q2T3, EmotionState.Anxiety, ConnectionTier.Tier3_Strong, "“I’m okay.” She laughs, but his alarm makes the tiny mishap feel bigger than it was.", 4f, "nervous_laugh"));
            answers.Add(CreateAnswer(q2T3, EmotionState.Confidence, "“Nice recovery.” *He grins.*", 12f, "HighFit", false));
            reactions.Add(CreateReaction(q2T3, EmotionState.Confidence, ConnectionTier.Tier3_Strong, "She laughs. “Thank you. I was trying to pretend that looked intentional.”", 12f, "flirty_wink"));
            answers.Add(CreateAnswer(q2T3, EmotionState.Attraction, "*He laughs with her.* “Okay, that was kind of adorable.”", 6f, "Reasonable", false));
            reactions.Add(CreateReaction(q2T3, EmotionState.Attraction, ConnectionTier.Tier3_Strong, "She laughs harder. “Adorable? Wow, okay.” The teasing comes back immediately.", 6f, "nervous_laugh"));
            answers.Add(CreateAnswer(q2T3, EmotionState.CalmAnxiety, "“You got it. No disaster.”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q2T3, EmotionState.CalmAnxiety, ConnectionTier.Tier3_Strong, "“Exactly.” She smiles. “No disaster.”", 4f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q2T3, EmotionState.CalmConfidence, "“Strong save.” *He gives her an approving nod.*", 10f, "HighFit", false));
            reactions.Add(CreateReaction(q2T3, EmotionState.CalmConfidence, ConnectionTier.Tier3_Strong, "“I’ll take ‘strong save.’” She grins, clearly enjoying his easy reaction.", 10f, "flirty_wink"));
            answers.Add(CreateAnswer(q2T3, EmotionState.CalmAttraction, "*He laughs softly.* “You okay?”", 8f, "Reasonable", false));
            reactions.Add(CreateReaction(q2T3, EmotionState.CalmAttraction, ConnectionTier.Tier3_Strong, "“I’m good.” She laughs and goes right back to the conversation.", 8f, "nervous_laugh"));
            answers.Add(CreateAnswer(q2T3, EmotionState.AnxietyConfidence, "“Smooth.” *He grins, a little too quickly.* “I mean—good save.”", 14f, "HighFit", false));
            reactions.Add(CreateReaction(q2T3, EmotionState.AnxietyConfidence, ConnectionTier.Tier3_Strong, "“Yes, smooth.” She laughs, but the quick correction gives her a glimpse of his nerves.", 14f, "flirty_wink"));
            answers.Add(CreateAnswer(q2T3, EmotionState.AnxietyAttraction, "“Oh my god, are you okay? That looked like it almost went everywhere.”", 6f, "Reasonable", false));
            reactions.Add(CreateReaction(q2T3, EmotionState.AnxietyAttraction, ConnectionTier.Tier3_Strong, "“I’m fine, I promise.” She laughs, slightly overwhelmed by how seriously he took it.", 6f, "nervous_laugh"));
            answers.Add(CreateAnswer(q2T3, EmotionState.ConfidenceAttraction, "“Nice save. I was about to pretend I didn’t see that.”", 16f, "HighFit", false));
            reactions.Add(CreateReaction(q2T3, EmotionState.ConfidenceAttraction, ConnectionTier.Tier3_Strong, "“Please don’t pretend you didn’t see it.” She laughs, but the comment feels a touch too performative.", 16f, "second_date"));
            answers.Add(CreateAnswer(q2T3, EmotionState.FrozenBlank, "*He stares at the napkin, paralyzed between helping and not reacting.*", 0f, "Reasonable", true));
            reactions.Add(CreateReaction(q2T3, EmotionState.FrozenBlank, ConnectionTier.Tier3_Strong, "She scoops it up herself, smiling slightly bemused. “Phew, crisis averted without backup.”", 0f, "neutral_acknowledge"));

            // --- Slot 2 Tier 2: Type 1 – Direct question ---
            var q2T2 = ScriptableObject.CreateInstance<QuestionData>();
            q2T2.slotIndex = 2;
            q2T2.connectionTier = ConnectionTier.Tier2_Maybe;
            q2T2.scenarioType = "Type 1 – Direct question";
            q2T2.questionText = "\"So what do you do?\"";
            q2T2.responseTimeWindow = 6.0f;
            q2T2.socialEventProfile = new SocialEventProfile
            {
                requiresPlayerInitiation = false,
                primaryNodeOverride = InternalNodeType.Brain,
                effects = new List<TargetedEffect>
                {
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Brain,
                        effectType = SocialEffectType.StressEvent,
                        stressEventSubtype = StressEventSubtype.Panic,
                        magnitude = 15.0f
                    },
                }
            };
            questions.Add(q2T2);

            answers.Add(CreateAnswer(q2T2, EmotionState.Calm, "“I work in software. Mostly product stuff. It’s busy, but I like it.”", 6f, "Reasonable", false));
            reactions.Add(CreateReaction(q2T2, EmotionState.Calm, ConnectionTier.Tier2_Maybe, "“Software.” She nods. “What part do you actually like?” Her interest is genuine.", 6f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q2T2, EmotionState.Anxiety, "“I’m in software. Well—not exactly software, more... product and development. It’s complicated.”", -4f, "Poor", false));
            reactions.Add(CreateReaction(q2T2, EmotionState.Anxiety, ConnectionTier.Tier2_Maybe, "“I got you.” She smiles, helping him settle. “You don’t have to explain the whole industry.”", -4f, "discomfort"));
            answers.Add(CreateAnswer(q2T2, EmotionState.Confidence, "“I work in software. I like it, and I’m pretty good at what I do.”", 14f, "HighFit", false));
            reactions.Add(CreateReaction(q2T2, EmotionState.Confidence, ConnectionTier.Tier2_Maybe, "“Okay, so you actually like what you do.” Her eyes brighten; she asks another question.", 14f, "warm_interest"));
            answers.Add(CreateAnswer(q2T2, EmotionState.Attraction, "“I work in software. Honestly, I’m more interested in what you do.”", 2f, "Reasonable", false));
            reactions.Add(CreateReaction(q2T2, EmotionState.Attraction, ConnectionTier.Tier2_Maybe, "She laughs softly. “I’ll tell you mine eventually.” The answer leaves her a little unsure what he wants to know about her.", 2f, "nervous_laugh"));
            answers.Add(CreateAnswer(q2T2, EmotionState.CalmAnxiety, "“Software. It’s a good job. I like the work, even when it gets hectic.”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q2T2, EmotionState.CalmAnxiety, ConnectionTier.Tier2_Maybe, "“That makes sense.” She nods, but the conversation stays fairly surface-level.", 4f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q2T2, EmotionState.CalmConfidence, "“I’m in software. It keeps me busy, and I’ve built some things I’m proud of.”", 12f, "HighFit", false));
            reactions.Add(CreateReaction(q2T2, EmotionState.CalmConfidence, ConnectionTier.Tier2_Maybe, "“Okay, that I can actually picture.” She asks about one of the things he built.", 12f, "warm_interest"));
            answers.Add(CreateAnswer(q2T2, EmotionState.CalmAttraction, "“Software. I could give you the boring version, but I’m more curious about you.”", 6f, "Reasonable", false));
            reactions.Add(CreateReaction(q2T2, EmotionState.CalmAttraction, ConnectionTier.Tier2_Maybe, "“I’ll give you the non-boring version of mine too.” She smiles, inviting him to keep talking.", 6f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q2T2, EmotionState.AnxietyConfidence, "“Software. It’s good. I’m good at it.” *He catches himself.* “That sounded more rehearsed than I meant.”", 6f, "Reasonable", false));
            reactions.Add(CreateReaction(q2T2, EmotionState.AnxietyConfidence, ConnectionTier.Tier2_Maybe, "“No, it’s fine.” She gives him a small smile. “You don’t have to sell me on your job.”", 6f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q2T2, EmotionState.AnxietyAttraction, "“Software... sorry, that’s such a boring answer. I swear there’s more to me than my job.”", 0f, "Reasonable", false));
            reactions.Add(CreateReaction(q2T2, EmotionState.AnxietyAttraction, ConnectionTier.Tier2_Maybe, "“Hey, it’s okay.” She smiles reassuringly. “You don’t have to prove you’re interesting.”", 0f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q2T2, EmotionState.ConfidenceAttraction, "“Software. I like solving problems all day. You might get the better version of me off the clock, though.”", 10f, "HighFit", false));
            reactions.Add(CreateReaction(q2T2, EmotionState.ConfidenceAttraction, ConnectionTier.Tier2_Maybe, "She smiles. “Off the clock, then?” She gives him an opening to talk about himself differently.", 10f, "warm_interest"));
            answers.Add(CreateAnswer(q2T2, EmotionState.FrozenBlank, "*He opens his mouth, pauses, and awkwardly clears his throat without speaking.*", -6f, "Poor", true));
            reactions.Add(CreateReaction(q2T2, EmotionState.FrozenBlank, ConnectionTier.Tier2_Maybe, "She tilts her head, waiting for a beat. “Or... we don't have to talk about work.”", -6f, "discomfort"));

            // --- Slot 2 Tier 1: Type 4 – Tense silence ---
            var q2T1 = ScriptableObject.CreateInstance<QuestionData>();
            q2T1.slotIndex = 2;
            q2T1.connectionTier = ConnectionTier.Tier1_Awkward;
            q2T1.scenarioType = "Type 4 – Tense silence";
            q2T1.questionText = "*She checks her drink, doesn't meet his eyes for a beat.*";
            q2T1.responseTimeWindow = 6.0f;
            q2T1.socialEventProfile = new SocialEventProfile
            {
                requiresPlayerInitiation = false,
                primaryNodeOverride = InternalNodeType.Voice,
                effects = new List<TargetedEffect>
                {
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Voice,
                        effectType = SocialEffectType.StressEvent,
                        stressEventSubtype = StressEventSubtype.AwkwardSilence,
                        magnitude = 15.0f
                    },
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Body,
                        effectType = SocialEffectType.StressEvent,
                        stressEventSubtype = StressEventSubtype.SweatSurge,
                        magnitude = 15.0f
                    },
                }
            };
            questions.Add(q2T1);

            answers.Add(CreateAnswer(q2T1, EmotionState.Calm, "“You good?” *He asks it casually, without making a thing of it.*", -6f, "Poor", false));
            reactions.Add(CreateReaction(q2T1, EmotionState.Calm, ConnectionTier.Tier1_Awkward, "She looks back up. “Yeah. Just thinking.” The tension passes without becoming a thing.", -6f, "discomfort"));
            answers.Add(CreateAnswer(q2T1, EmotionState.Anxiety, "“Did I say something weird?”", -8f, "Poor", false));
            reactions.Add(CreateReaction(q2T1, EmotionState.Anxiety, ConnectionTier.Tier1_Awkward, "“No, no. You’re fine.” She reassures him, but her glance away shows she is now a little self-conscious too.", -8f, "sympathetic_concern"));
            answers.Add(CreateAnswer(q2T1, EmotionState.Confidence, "“You’ve got that look. Should I be worried?”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q2T1, EmotionState.Confidence, ConnectionTier.Tier1_Awkward, "She gives a small, unreadable smile. “Nothing dramatic.” The playful challenge makes her pull back.", 4f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q2T1, EmotionState.Attraction, "*He smiles, waiting for her to look back rather than pushing the moment.*", -2f, "Poor", true));
            reactions.Add(CreateReaction(q2T1, EmotionState.Attraction, ConnectionTier.Tier1_Awkward, "She catches his smile and returns it, just a little smaller.", -2f, "discomfort"));
            answers.Add(CreateAnswer(q2T1, EmotionState.CalmAnxiety, "“Hey, you’re okay. No pressure.”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q2T1, EmotionState.CalmAnxiety, ConnectionTier.Tier1_Awkward, "“I’m okay.” She smiles. “Really.” The extra reassurance makes the moment slightly heavier.", 4f, "sympathetic_concern"));
            answers.Add(CreateAnswer(q2T1, EmotionState.CalmConfidence, "“Take your time.”", 10f, "HighFit", false));
            reactions.Add(CreateReaction(q2T1, EmotionState.CalmConfidence, ConnectionTier.Tier1_Awkward, "She looks back at him. “Thanks.” Her shoulders loosen almost immediately.", 10f, "warm_interest"));
            answers.Add(CreateAnswer(q2T1, EmotionState.CalmAttraction, "*He gives her an easy smile and lets her look back when she wants to.*", 0f, "Reasonable", true));
            reactions.Add(CreateReaction(q2T1, EmotionState.CalmAttraction, ConnectionTier.Tier1_Awkward, "Their eyes meet again. She smiles, a little shyly, and stays there a beat longer.", 0f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q2T1, EmotionState.AnxietyConfidence, "“Okay, that look definitely means I said something.” *He laughs nervously.*", 6f, "Reasonable", false));
            reactions.Add(CreateReaction(q2T1, EmotionState.AnxietyConfidence, ConnectionTier.Tier1_Awkward, "“You didn’t.” She laughs softly, but her attention is now more cautious.", 6f, "nervous_laugh"));
            answers.Add(CreateAnswer(q2T1, EmotionState.AnxietyAttraction, "“Hey... did I make you uncomfortable?”", -4f, "Poor", false));
            reactions.Add(CreateReaction(q2T1, EmotionState.AnxietyAttraction, ConnectionTier.Tier1_Awkward, "“No, you’re okay.” She gives him a quick reassuring smile, but the question makes her uncomfortable.", -4f, "uncomfortable"));
            answers.Add(CreateAnswer(q2T1, EmotionState.ConfidenceAttraction, "“I’m starting to think you’re hiding something.” *He smiles.*", -4f, "Poor", false));
            reactions.Add(CreateReaction(q2T1, EmotionState.ConfidenceAttraction, ConnectionTier.Tier1_Awkward, "She laughs under her breath. “Maybe.” The mystery lands as playful rather than threatening.", -4f, "discomfort"));
            answers.Add(CreateAnswer(q2T1, EmotionState.FrozenBlank, "*He waits, then awkwardly takes a drink when the silence stretches.*", -8f, "Poor", true));
            reactions.Add(CreateReaction(q2T1, EmotionState.FrozenBlank, ConnectionTier.Tier1_Awkward, "She watches the ice melt in her glass, the distance between them feeling palpable.", -8f, "discomfort"));

            return (questions, answers, reactions);
        }


        private static (List<QuestionData>, List<AnswerData>, List<ReactionData>) BuildSlot3()
        {
            var questions = new List<QuestionData>();
            var answers = new List<AnswerData>();
            var reactions = new List<ReactionData>();

            // --- Slot 3 Tier 4: Type 2 – Compliment, vulnerable ---
            var q3T4 = ScriptableObject.CreateInstance<QuestionData>();
            q3T4.slotIndex = 3;
            q3T4.connectionTier = ConnectionTier.Tier4_SecondDate;
            q3T4.scenarioType = "Type 2 – Compliment, vulnerable";
            q3T4.questionText = "\"I have to say — you're way easier to talk to than I expected.\"";
            q3T4.responseTimeWindow = 6.0f;
            q3T4.socialEventProfile = new SocialEventProfile
            {
                requiresPlayerInitiation = false,
                primaryNodeOverride = InternalNodeType.Heart,
                effects = new List<TargetedEffect>
                {
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Heart,
                        effectType = SocialEffectType.DirectEmotionPush,
                        pushTargetEmotion = CoreEmotion.Attraction,
                        magnitude = 25.0f
                    },
                }
            };
            questions.Add(q3T4);

            answers.Add(CreateAnswer(q3T4, EmotionState.Calm, "“I’ll take that as a good sign.”", 6f, "Reasonable", false));
            reactions.Add(CreateReaction(q3T4, EmotionState.Calm, ConnectionTier.Tier4_SecondDate, "“It is.” She smiles. “I’m glad we got past the weird first ten minutes.”", 6f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q3T4, EmotionState.Anxiety, "“Really? I thought I was talking too much.”", 6f, "Reasonable", false));
            reactions.Add(CreateReaction(q3T4, EmotionState.Anxiety, ConnectionTier.Tier4_SecondDate, "“No, you weren’t.” She laughs gently. “You’ve been fine.” The reassurance is warm, but she notices the insecurity.", 6f, "nervous_laugh"));
            answers.Add(CreateAnswer(q3T4, EmotionState.Confidence, "“I had a feeling you’d warm up to me.”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q3T4, EmotionState.Confidence, ConnectionTier.Tier4_SecondDate, "Her smile tightens. “That’s... one way to take it.” She redirects instead of rewarding the assumption.", 4f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q3T4, EmotionState.Attraction, "“Good. I’ve been trying not to make you like me too much too quickly.”", 14f, "HighFit", false));
            reactions.Add(CreateReaction(q3T4, EmotionState.Attraction, ConnectionTier.Tier4_SecondDate, "“Good.” She smiles, though the line feels a little too self-aware.", 14f, "warm_interest"));
            answers.Add(CreateAnswer(q3T4, EmotionState.CalmAnxiety, "“I’m glad. I was worried I was making this harder than it needed to be.”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q3T4, EmotionState.CalmAnxiety, ConnectionTier.Tier4_SecondDate, "“You weren’t making it harder.” She gives him a reassuring look.", 4f, "sympathetic_concern"));
            answers.Add(CreateAnswer(q3T4, EmotionState.CalmConfidence, "“Good. That makes two of us who can relax a little.”", 12f, "HighFit", false));
            reactions.Add(CreateReaction(q3T4, EmotionState.CalmConfidence, ConnectionTier.Tier4_SecondDate, "“Good.” She smiles. “Because I’ve actually been really comfortable with you.”", 12f, "warm_interest"));
            answers.Add(CreateAnswer(q3T4, EmotionState.CalmAttraction, "*He smiles.* “I’m glad. I’ve been enjoying this too.”", 16f, "HighFit", false));
            reactions.Add(CreateReaction(q3T4, EmotionState.CalmAttraction, ConnectionTier.Tier4_SecondDate, "She looks pleased. “Me too.” She lets the admission sit instead of covering it with a joke.", 16f, "second_date"));
            answers.Add(CreateAnswer(q3T4, EmotionState.AnxietyConfidence, "“See? I can be normal.” *Beat.* “Sometimes.”", 8f, "Reasonable", false));
            reactions.Add(CreateReaction(q3T4, EmotionState.AnxietyConfidence, ConnectionTier.Tier4_SecondDate, "She laughs. “Normal enough, yeah.” The joke works, though she sees the insecurity underneath it.", 8f, "nervous_laugh"));
            answers.Add(CreateAnswer(q3T4, EmotionState.AnxietyAttraction, "“Really? That’s... actually a huge relief.”", 6f, "Reasonable", false));
            reactions.Add(CreateReaction(q3T4, EmotionState.AnxietyAttraction, ConnectionTier.Tier4_SecondDate, "“Hey.” Her voice softens. “You don’t have to pass some test with me.”", 6f, "sympathetic_concern"));
            answers.Add(CreateAnswer(q3T4, EmotionState.ConfidenceAttraction, "“I was hoping you’d get that feeling.”", 14f, "HighFit", false));
            reactions.Add(CreateReaction(q3T4, EmotionState.ConfidenceAttraction, ConnectionTier.Tier4_SecondDate, "“I did, huh?” She smiles, amused by the confidence without fully leaning into it.", 14f, "warm_interest"));
            answers.Add(CreateAnswer(q3T4, EmotionState.FrozenBlank, "*He catches his breath, surprised, words failing him as he smiles nervously.*", 6f, "Reasonable", true));
            reactions.Add(CreateReaction(q3T4, EmotionState.FrozenBlank, ConnectionTier.Tier4_SecondDate, "Her smile turns soft and affectionate. “Hey, don’t overthink it. It was a good thing.”", 6f, "thinking_chintap"));

            // --- Slot 3 Tier 3: Type 9 – Reflective pause, wordless ---
            var q3T3 = ScriptableObject.CreateInstance<QuestionData>();
            q3T3.slotIndex = 3;
            q3T3.connectionTier = ConnectionTier.Tier3_Strong;
            q3T3.scenarioType = "Type 9 – Reflective pause, wordless";
            q3T3.questionText = "*She turns her glass slowly, smiling slightly to herself, not looking at him.*";
            q3T3.responseTimeWindow = 6.0f;
            q3T3.socialEventProfile = new SocialEventProfile
            {
                requiresPlayerInitiation = false,
                primaryNodeOverride = InternalNodeType.Brain,
                effects = new List<TargetedEffect>
                {
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Brain,
                        effectType = SocialEffectType.StressEvent,
                        stressEventSubtype = StressEventSubtype.Overthinking,
                        magnitude = 15.0f
                    },
                }
            };
            questions.Add(q3T3);

            answers.Add(CreateAnswer(q3T3, EmotionState.Calm, "*He notices the smile and lets her have the moment.*", 8f, "Reasonable", true));
            reactions.Add(CreateReaction(q3T3, EmotionState.Calm, ConnectionTier.Tier3_Strong, "She notices him noticing and smiles. “What?” The tone is light, inviting.", 8f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q3T3, EmotionState.Anxiety, "“What are you thinking about?” *Too quickly.* “You don’t have to tell me.”", 2f, "Reasonable", false));
            reactions.Add(CreateReaction(q3T3, EmotionState.Anxiety, ConnectionTier.Tier3_Strong, "“Nothing bad.” She laughs softly. “I was just thinking.” She avoids making him guess.", 2f, "nervous_laugh"));
            answers.Add(CreateAnswer(q3T3, EmotionState.Confidence, "“You’ve got a secret smile going on.”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q3T3, EmotionState.Confidence, ConnectionTier.Tier3_Strong, "“Maybe I do.” She gives him a playful look.", 4f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q3T3, EmotionState.Attraction, "*He smiles back without interrupting whatever she’s thinking about.*", 12f, "HighFit", true));
            reactions.Add(CreateReaction(q3T3, EmotionState.Attraction, ConnectionTier.Tier3_Strong, "She smiles back, but the silence suddenly feels more self-conscious.", 12f, "warm_interest"));
            answers.Add(CreateAnswer(q3T3, EmotionState.CalmAnxiety, "“You look like you just remembered something funny.”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q3T3, EmotionState.CalmAnxiety, ConnectionTier.Tier3_Strong, "“Maybe.” She laughs. “I just remembered something.”", 4f, "nervous_laugh"));
            answers.Add(CreateAnswer(q3T3, EmotionState.CalmConfidence, "“You seem pleased with yourself.”", 8f, "Reasonable", false));
            reactions.Add(CreateReaction(q3T3, EmotionState.CalmConfidence, ConnectionTier.Tier3_Strong, "“Maybe I am.” She gives him a knowing smile, clearly enjoying the tiny moment.", 8f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q3T3, EmotionState.CalmAttraction, "*He watches her with a quiet smile, curious but not pushing.*", 16f, "HighFit", true));
            reactions.Add(CreateReaction(q3T3, EmotionState.CalmAttraction, ConnectionTier.Tier3_Strong, "She catches his eye and smiles without saying what she was thinking.", 16f, "second_date"));
            answers.Add(CreateAnswer(q3T3, EmotionState.AnxietyConfidence, "“Okay, now I need to know what that smile means.” *He tries to sound casual.*", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q3T3, EmotionState.AnxietyConfidence, ConnectionTier.Tier3_Strong, "She laughs, but her smile fades. “You really want to know that badly?”", 4f, "nervous_laugh"));
            answers.Add(CreateAnswer(q3T3, EmotionState.AnxietyAttraction, "“Was that smile about something I said?”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q3T3, EmotionState.AnxietyAttraction, ConnectionTier.Tier3_Strong, "“Maybe.” She smiles, but the question makes her feel slightly watched.", 4f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q3T3, EmotionState.ConfidenceAttraction, "“That smile tells me I’m doing something right.”", 14f, "HighFit", false));
            reactions.Add(CreateReaction(q3T3, EmotionState.ConfidenceAttraction, ConnectionTier.Tier3_Strong, "“You’re taking credit already?” she teases.", 14f, "flirty_wink"));
            answers.Add(CreateAnswer(q3T3, EmotionState.FrozenBlank, "*He watches her glass turn, tongue-tied, unable to find an opening.*", 6f, "Reasonable", true));
            reactions.Add(CreateReaction(q3T3, EmotionState.FrozenBlank, ConnectionTier.Tier3_Strong, "She looks up after a moment, offering a mild, patient smile.", 6f, "neutral_acknowledge"));

            // --- Slot 3 Tier 2: Type 5 – Distracted/bored, wordless ---
            var q3T2 = ScriptableObject.CreateInstance<QuestionData>();
            q3T2.slotIndex = 3;
            q3T2.connectionTier = ConnectionTier.Tier2_Maybe;
            q3T2.scenarioType = "Type 5 – Distracted/bored, wordless";
            q3T2.questionText = "*She stifles a small yawn, catches herself, looks a little embarrassed about it.*";
            q3T2.responseTimeWindow = 6.0f;
            q3T2.socialEventProfile = new SocialEventProfile
            {
                requiresPlayerInitiation = false,
                primaryNodeOverride = InternalNodeType.Body,
                effects = new List<TargetedEffect>
                {
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Body,
                        effectType = SocialEffectType.StressEvent,
                        stressEventSubtype = StressEventSubtype.SweatSurge,
                        magnitude = 20.0f
                    },
                }
            };
            questions.Add(q3T2);

            answers.Add(CreateAnswer(q3T2, EmotionState.Calm, "“Long day?” *He smiles, not making it a big deal.*", -6f, "Poor", false));
            reactions.Add(CreateReaction(q3T2, EmotionState.Calm, ConnectionTier.Tier2_Maybe, "“A little.” She laughs. “It’s been a long one.” The honesty makes the moment easier.", -6f, "discomfort"));
            answers.Add(CreateAnswer(q3T2, EmotionState.Anxiety, "“Oh—sorry, are you tired? We can... I mean, you don't have to pretend you're not.”", -8f, "Poor", false));
            reactions.Add(CreateReaction(q3T2, EmotionState.Anxiety, ConnectionTier.Tier2_Maybe, "“I’m tired, not bored.” She reassures him, but she can see where his mind went.", -8f, "sympathetic_concern"));
            answers.Add(CreateAnswer(q3T2, EmotionState.Confidence, "“I’m going to assume that was your subtle review of my conversation skills.”", 14f, "HighFit", false));
            reactions.Add(CreateReaction(q3T2, EmotionState.Confidence, ConnectionTier.Tier2_Maybe, "She laughs. “You’re safe. It’s been a long day.”", 14f, "flirty_wink"));
            answers.Add(CreateAnswer(q3T2, EmotionState.Attraction, "“I’ll try not to take that personally.” *He smiles.* “You okay?”", 2f, "Reasonable", false));
            reactions.Add(CreateReaction(q3T2, EmotionState.Attraction, ConnectionTier.Tier2_Maybe, "“I’m fine.” She smiles, though the attempt to make it cute puts the focus on the awkwardness.", 2f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q3T2, EmotionState.CalmAnxiety, "“You’re alright. It’s been a long day for both of us.”", 0f, "Reasonable", false));
            reactions.Add(CreateReaction(q3T2, EmotionState.CalmAnxiety, ConnectionTier.Tier2_Maybe, "“Long day.” She smiles. “That’s all.”", 0f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q3T2, EmotionState.CalmConfidence, "“Long day?” *He laughs lightly.* “Fair enough.”", 8f, "Reasonable", false));
            reactions.Add(CreateReaction(q3T2, EmotionState.CalmConfidence, ConnectionTier.Tier2_Maybe, "She laughs. “Okay, good. I was hoping you wouldn’t make me feel guilty about it.”", 8f, "nervous_laugh"));
            answers.Add(CreateAnswer(q3T2, EmotionState.CalmAttraction, "“You looked cute trying to hide that.” *A small grin.* “Long day?”", 2f, "Reasonable", false));
            reactions.Add(CreateReaction(q3T2, EmotionState.CalmAttraction, ConnectionTier.Tier2_Maybe, "“Long day.” She smiles. “But I’m still enjoying myself.”", 2f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q3T2, EmotionState.AnxietyConfidence, "“Wow. Brutal.” *He laughs, then glances at her.* “You’re actually just tired, right?”", 8f, "Reasonable", false));
            reactions.Add(CreateReaction(q3T2, EmotionState.AnxietyConfidence, ConnectionTier.Tier2_Maybe, "“No, no.” She laughs. “Don’t flatter yourself.” Her tone is playful, but the mood is less relaxed.", 8f, "relieved_sigh"));
            answers.Add(CreateAnswer(q3T2, EmotionState.AnxietyAttraction, "“Sorry, is this getting boring? I can... talk about something else.”", -4f, "Poor", false));
            reactions.Add(CreateReaction(q3T2, EmotionState.AnxietyAttraction, ConnectionTier.Tier2_Maybe, "“I’m not bored.” She says it quickly, trying to stop him from spiraling.", -4f, "discomfort"));
            answers.Add(CreateAnswer(q3T2, EmotionState.ConfidenceAttraction, "“I’ll forgive you for that one.” *He grins.* “You tired?”", 10f, "HighFit", false));
            reactions.Add(CreateReaction(q3T2, EmotionState.ConfidenceAttraction, ConnectionTier.Tier2_Maybe, "She laughs. “You’re going to blame yourself for that?”", 10f, "flirty_wink"));
            answers.Add(CreateAnswer(q3T2, EmotionState.FrozenBlank, "*He notices the yawn and freezes up completely, unsure whether to acknowledge it.*", -8f, "Poor", true));
            reactions.Add(CreateReaction(q3T2, EmotionState.FrozenBlank, ConnectionTier.Tier2_Maybe, "She clears her throat softly, tapping her fingers lightly against the table.", -8f, "discomfort"));

            // --- Slot 3 Tier 1: Type 7 – She notices something, sharper ---
            var q3T1 = ScriptableObject.CreateInstance<QuestionData>();
            q3T1.slotIndex = 3;
            q3T1.connectionTier = ConnectionTier.Tier1_Awkward;
            q3T1.scenarioType = "Type 7 – She notices something, sharper";
            q3T1.questionText = "\"You keep doing this thing — checking yourself. Is this weird for you?\"";
            q3T1.responseTimeWindow = 6.0f;
            q3T1.socialEventProfile = new SocialEventProfile
            {
                requiresPlayerInitiation = false,
                primaryNodeOverride = InternalNodeType.Brain,
                effects = new List<TargetedEffect>
                {
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Brain,
                        effectType = SocialEffectType.StressEvent,
                        stressEventSubtype = StressEventSubtype.Panic,
                        magnitude = 25.0f
                    },
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Body,
                        effectType = SocialEffectType.StressEvent,
                        stressEventSubtype = StressEventSubtype.SweatSurge,
                        magnitude = 25.0f
                    },
                }
            };
            questions.Add(q3T1);

            answers.Add(CreateAnswer(q3T1, EmotionState.Calm, "“A little. First dates are weird by default, right?”", -6f, "Poor", false));
            reactions.Add(CreateReaction(q3T1, EmotionState.Calm, ConnectionTier.Tier1_Awkward, "“That makes sense.” She smiles. “I think everyone does a little.”", -6f, "discomfort"));
            answers.Add(CreateAnswer(q3T1, EmotionState.Anxiety, "“I do? I mean—maybe. I thought I was being subtle.”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q3T1, EmotionState.Anxiety, ConnectionTier.Tier1_Awkward, "“Hey, you don’t have to monitor yourself with me.” She says it kindly, but now she is watching for it.", 4f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q3T1, EmotionState.Confidence, "“Yeah, I’m checking the vibe. That seems reasonable.”", -14f, "ActivelyWrong", false));
            reactions.Add(CreateReaction(q3T1, EmotionState.Confidence, ConnectionTier.Tier1_Awkward, "“Fair.” She gives a tiny shrug. “I just noticed.”", -14f, "shut_down"));
            answers.Add(CreateAnswer(q3T1, EmotionState.Attraction, "“Maybe I’m just trying to figure out whether you’re having as good a time as I am.”", 0f, "Reasonable", false));
            reactions.Add(CreateReaction(q3T1, EmotionState.Attraction, ConnectionTier.Tier1_Awkward, "“Maybe.” She smiles, but the focus on whether she likes him makes her a little careful.", 0f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q3T1, EmotionState.CalmAnxiety, "“Yeah. A little. I’m trying to stay present instead of overthinking everything.”", 16f, "HighFit", false));
            reactions.Add(CreateReaction(q3T1, EmotionState.CalmAnxiety, ConnectionTier.Tier1_Awkward, "“Okay.” She nods. “I get it.” The honesty seems to settle her.", 16f, "second_date"));
            answers.Add(CreateAnswer(q3T1, EmotionState.CalmConfidence, "“A little. I care how this is going, but I’m not panicking about it.”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q3T1, EmotionState.CalmConfidence, ConnectionTier.Tier1_Awkward, "“I can tell.” She smiles. “You seem a lot more relaxed when you stop trying to do it perfectly.”", 4f, "relieved_sigh"));
            answers.Add(CreateAnswer(q3T1, EmotionState.CalmAttraction, "“Maybe. I like you, so I’m paying attention.”", -4f, "Poor", false));
            reactions.Add(CreateReaction(q3T1, EmotionState.CalmAttraction, ConnectionTier.Tier1_Awkward, "Her expression warms. “That’s actually kind of nice to hear.”", -4f, "discomfort"));
            answers.Add(CreateAnswer(q3T1, EmotionState.AnxietyConfidence, "“I’m not checking myself.” *Beat.* “Okay, I’m absolutely checking myself.”", 6f, "Reasonable", false));
            reactions.Add(CreateReaction(q3T1, EmotionState.AnxietyConfidence, ConnectionTier.Tier1_Awkward, "“I wasn’t accusing you.” She laughs softly, but the defensiveness changes the mood.", 6f, "nervous_laugh"));
            answers.Add(CreateAnswer(q3T1, EmotionState.AnxietyAttraction, "“Yeah... probably. I really don’t want to mess this up.”", 8f, "Reasonable", false));
            reactions.Add(CreateReaction(q3T1, EmotionState.AnxietyAttraction, ConnectionTier.Tier1_Awkward, "“You don’t have to get it perfect.” She looks genuinely sympathetic.", 8f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q3T1, EmotionState.ConfidenceAttraction, "“I’m reading the room. I’m hoping what I’m seeing is good.”", -10f, "ActivelyWrong", false));
            reactions.Add(CreateReaction(q3T1, EmotionState.ConfidenceAttraction, ConnectionTier.Tier1_Awkward, "“I’m giving you enough feedback, I think.” She smiles, teasing rather than pushing.", -10f, "discomfort"));
            answers.Add(CreateAnswer(q3T1, EmotionState.FrozenBlank, "“I—uh...” *He looks down, caught.*", -8f, "Poor", false));
            reactions.Add(CreateReaction(q3T1, EmotionState.FrozenBlank, ConnectionTier.Tier1_Awkward, "Her shoulders drop a fraction. “You really don't have to force this if you're uncomfortable.”", -8f, "uncomfortable"));

            return (questions, answers, reactions);
        }


        private static (List<QuestionData>, List<AnswerData>, List<ReactionData>) BuildSlot4()
        {
            var questions = new List<QuestionData>();
            var answers = new List<AnswerData>();
            var reactions = new List<ReactionData>();

            // --- Slot 4 Tier 4: Type 6 – Physical mishap, shared ---
            var q4T4 = ScriptableObject.CreateInstance<QuestionData>();
            q4T4.slotIndex = 4;
            q4T4.connectionTier = ConnectionTier.Tier4_SecondDate;
            q4T4.scenarioType = "Type 6 – Physical mishap, shared";
            q4T4.questionText = "*She almost knocks her glass, catches it, laughs — \"Smooth. Real smooth.\"*";
            q4T4.responseTimeWindow = 6.0f;
            q4T4.socialEventProfile = new SocialEventProfile
            {
                requiresPlayerInitiation = false,
                primaryNodeOverride = InternalNodeType.Heart,
                effects = new List<TargetedEffect>
                {
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Heart,
                        effectType = SocialEffectType.StressEvent,
                        stressEventSubtype = StressEventSubtype.HeartFlutter,
                        magnitude = 15.0f
                    },
                }
            };
            questions.Add(q4T4);

            answers.Add(CreateAnswer(q4T4, EmotionState.Calm, "“Nice save.”", 10f, "HighFit", false));
            reactions.Add(CreateReaction(q4T4, EmotionState.Calm, ConnectionTier.Tier4_SecondDate, "She laughs. “Okay, nobody saw that.”", 10f, "flirty_wink"));
            answers.Add(CreateAnswer(q4T4, EmotionState.Anxiety, "“Oh—careful. That would’ve been... yeah.”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q4T4, EmotionState.Anxiety, ConnectionTier.Tier4_SecondDate, "“It’s fine.” She steadies the glass and smiles, but his alarm made the tiny mistake feel bigger.", 4f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q4T4, EmotionState.Confidence, "“Professional recovery.”", 14f, "HighFit", false));
            reactions.Add(CreateReaction(q4T4, EmotionState.Confidence, ConnectionTier.Tier4_SecondDate, "“Professional recovery?” she says, amused by the deadpan confidence.", 14f, "warm_interest"));
            answers.Add(CreateAnswer(q4T4, EmotionState.Attraction, "“Honestly, kind of impressive.” *He grins.*", 6f, "Reasonable", false));
            reactions.Add(CreateReaction(q4T4, EmotionState.Attraction, ConnectionTier.Tier4_SecondDate, "“Impressive, apparently.” She laughs, though the compliment feels slightly too eager for the moment.", 6f, "nervous_laugh"));
            answers.Add(CreateAnswer(q4T4, EmotionState.CalmAnxiety, "“You saved it. We’re good.”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q4T4, EmotionState.CalmAnxiety, ConnectionTier.Tier4_SecondDate, "“Exactly.” She smiles and settles the glass back down.", 4f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q4T4, EmotionState.CalmConfidence, "“That was a strong recovery.”", 10f, "HighFit", false));
            reactions.Add(CreateReaction(q4T4, EmotionState.CalmConfidence, ConnectionTier.Tier4_SecondDate, "“That sounded like a review.” She laughs, clearly enjoying his easy reaction.", 10f, "flirty_wink"));
            answers.Add(CreateAnswer(q4T4, EmotionState.CalmAttraction, "*He laughs softly.* “You’re alright.”", 10f, "HighFit", false));
            reactions.Add(CreateReaction(q4T4, EmotionState.CalmAttraction, ConnectionTier.Tier4_SecondDate, "She smiles. “I’m okay.” The shared laugh keeps things warm.", 10f, "flirty_wink"));
            answers.Add(CreateAnswer(q4T4, EmotionState.AnxietyConfidence, "“Smooth.” *He laughs.* “I mean—you recovered.”", 14f, "HighFit", false));
            reactions.Add(CreateReaction(q4T4, EmotionState.AnxietyConfidence, ConnectionTier.Tier4_SecondDate, "“Yeah, I recovered.” She laughs, but the joke comes off a little forced.", 14f, "flirty_wink"));
            answers.Add(CreateAnswer(q4T4, EmotionState.AnxietyAttraction, "“That looked terrifying for a second.”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q4T4, EmotionState.AnxietyAttraction, ConnectionTier.Tier4_SecondDate, "“I’m fine!” She laughs, a little too quickly. “Really.”", 4f, "nervous_laugh"));
            answers.Add(CreateAnswer(q4T4, EmotionState.ConfidenceAttraction, "“I’m impressed. I thought we were about to lose the table.”", 18f, "HighFit", false));
            reactions.Add(CreateReaction(q4T4, EmotionState.ConfidenceAttraction, ConnectionTier.Tier4_SecondDate, "“I’m impressed you survived that.” She grins, matching his playful tone.", 18f, "second_date"));
            answers.Add(CreateAnswer(q4T4, EmotionState.FrozenBlank, "*He reaches out reflexively but freezes halfway, laughing silently in disbelief.*", 6f, "Reasonable", true));
            reactions.Add(CreateReaction(q4T4, EmotionState.FrozenBlank, ConnectionTier.Tier4_SecondDate, "She laughs easily and gives his hand an reassuring tap. “Team effort, almost!”", 6f, "nervous_laugh"));

            // --- Slot 4 Tier 3: Type 10 – Warmth, wordless ---
            var q4T3 = ScriptableObject.CreateInstance<QuestionData>();
            q4T3.slotIndex = 4;
            q4T3.connectionTier = ConnectionTier.Tier3_Strong;
            q4T3.scenarioType = "Type 10 – Warmth, wordless";
            q4T3.questionText = "*She's just... looking at him for a second, small unguarded smile, before glancing away.*";
            q4T3.responseTimeWindow = 6.0f;
            q4T3.socialEventProfile = new SocialEventProfile
            {
                requiresPlayerInitiation = false,
                primaryNodeOverride = InternalNodeType.Heart,
                effects = new List<TargetedEffect>
                {
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Heart,
                        effectType = SocialEffectType.DirectEmotionPush,
                        pushTargetEmotion = CoreEmotion.Attraction,
                        magnitude = 15.0f
                    },
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Body,
                        effectType = SocialEffectType.DirectEmotionPush,
                        pushTargetEmotion = CoreEmotion.Attraction,
                        magnitude = 15.0f
                    },
                }
            };
            questions.Add(q4T3);

            answers.Add(CreateAnswer(q4T3, EmotionState.Calm, "*He holds her gaze for a beat, then smiles.*", -2f, "Poor", true));
            reactions.Add(CreateReaction(q4T3, EmotionState.Calm, ConnectionTier.Tier3_Strong, "She smiles back, then casually looks away. The warmth stays.", -2f, "awkward_silence"));
            answers.Add(CreateAnswer(q4T3, EmotionState.Anxiety, "*He notices, looks away, then sneaks another glance back.*", 4f, "Reasonable", true));
            reactions.Add(CreateReaction(q4T3, EmotionState.Anxiety, ConnectionTier.Tier3_Strong, "She catches the quick glance away and smiles sympathetically.", 4f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q4T3, EmotionState.Confidence, "“What?” *He smiles, knowingly.*", 2f, "Reasonable", false));
            reactions.Add(CreateReaction(q4T3, EmotionState.Confidence, ConnectionTier.Tier3_Strong, "“What?” she asks, half-laughing, but she breaks the eye contact.", 2f, "nervous_laugh"));
            answers.Add(CreateAnswer(q4T3, EmotionState.Attraction, "*He smiles and holds her gaze a little longer.*", 18f, "HighFit", true));
            reactions.Add(CreateReaction(q4T3, EmotionState.Attraction, ConnectionTier.Tier3_Strong, "She holds the gaze another second before smiling.", 18f, "second_date"));
            answers.Add(CreateAnswer(q4T3, EmotionState.CalmAnxiety, "*He smiles back, slightly nervous, but doesn’t look away.*", 4f, "Reasonable", true));
            reactions.Add(CreateReaction(q4T3, EmotionState.CalmAnxiety, ConnectionTier.Tier3_Strong, "She gives him a small reassuring smile, noticing the nerves without calling them out.", 4f, "sympathetic_concern"));
            answers.Add(CreateAnswer(q4T3, EmotionState.CalmConfidence, "*He meets her eyes comfortably and lets the moment breathe.*", 6f, "Reasonable", true));
            reactions.Add(CreateReaction(q4T3, EmotionState.CalmConfidence, ConnectionTier.Tier3_Strong, "She keeps eye contact easily, comfortable with the quiet exchange.", 6f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q4T3, EmotionState.CalmAttraction, "*He smiles softly, staying in the moment without saying anything.*", 16f, "HighFit", true));
            reactions.Add(CreateReaction(q4T3, EmotionState.CalmAttraction, ConnectionTier.Tier3_Strong, "She smiles softly and looks down for a second, clearly a little flustered herself.", 16f, "second_date"));
            answers.Add(CreateAnswer(q4T3, EmotionState.AnxietyConfidence, "*He catches her eye.* “What?” *The grin is confident; the quick glance away isn't.*", 0f, "Reasonable", false));
            reactions.Add(CreateReaction(q4T3, EmotionState.AnxietyConfidence, ConnectionTier.Tier3_Strong, "She laughs at the sudden “What?” but looks away, momentarily self-conscious.", 0f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q4T3, EmotionState.AnxietyAttraction, "*He smiles, looks down for half a second, then back at her.*", 10f, "HighFit", true));
            reactions.Add(CreateReaction(q4T3, EmotionState.AnxietyAttraction, ConnectionTier.Tier3_Strong, "She smiles when he looks back, but the nervousness makes the moment shorter than it might have been.", 10f, "warm_interest"));
            answers.Add(CreateAnswer(q4T3, EmotionState.ConfidenceAttraction, "*He meets her eyes and gives her a small, knowing smile.*", 14f, "HighFit", true));
            reactions.Add(CreateReaction(q4T3, EmotionState.ConfidenceAttraction, ConnectionTier.Tier3_Strong, "Her smile turns more playful. She does not look away first this time.", 14f, "warm_interest"));
            answers.Add(CreateAnswer(q4T3, EmotionState.FrozenBlank, "*He holds her gaze for a split second, panics, and looks down at his hands.*", 8f, "Reasonable", true));
            reactions.Add(CreateReaction(q4T3, EmotionState.FrozenBlank, ConnectionTier.Tier3_Strong, "Her smile dims just a little, though her eyes remain friendly. “Penny for your thoughts?”", 8f, "neutral_acknowledge"));

            // --- Slot 4 Tier 2: Type 5 – Distracted/phone ---
            var q4T2 = ScriptableObject.CreateInstance<QuestionData>();
            q4T2.slotIndex = 4;
            q4T2.connectionTier = ConnectionTier.Tier2_Maybe;
            q4T2.scenarioType = "Type 5 – Distracted/phone";
            q4T2.questionText = "*Her phone buzzes. She glances, sets it face-down without answering. Says nothing.*";
            q4T2.responseTimeWindow = 6.0f;
            q4T2.socialEventProfile = new SocialEventProfile
            {
                requiresPlayerInitiation = true,
                primaryNodeOverride = InternalNodeType.Lungs,
                effects = new List<TargetedEffect>
                {
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Lungs,
                        effectType = SocialEffectType.StressEvent,
                        stressEventSubtype = StressEventSubtype.AwkwardSilence, // Shallow breath catch
                        magnitude = 25.0f
                    },
                }
            };
            questions.Add(q4T2);

            answers.Add(CreateAnswer(q4T2, EmotionState.Calm, "“Everything okay?”", -6f, "Poor", false));
            reactions.Add(CreateReaction(q4T2, EmotionState.Calm, ConnectionTier.Tier2_Maybe, "“Yeah, everything’s fine.” She turns the phone farther away and refocuses on him.", -6f, "discomfort"));
            answers.Add(CreateAnswer(q4T2, EmotionState.Anxiety, "“You can check it if you need to. I don't mind.”", -8f, "Poor", false));
            reactions.Add(CreateReaction(q4T2, EmotionState.Anxiety, ConnectionTier.Tier2_Maybe, "“It’s fine.” She smiles, but the offer to check it makes her wonder if she accidentally unsettled him.", -8f, "discomfort"));
            answers.Add(CreateAnswer(q4T2, EmotionState.Confidence, "“I’m choosing to take the face-down phone as a good sign.”", 14f, "HighFit", false));
            reactions.Add(CreateReaction(q4T2, EmotionState.Confidence, ConnectionTier.Tier2_Maybe, "“I’m just trying to be present.” She leaves the phone alone, a little more guarded.", 14f, "warm_interest"));
            answers.Add(CreateAnswer(q4T2, EmotionState.Attraction, "“Ignoring your phone for me? I’ll take the win.” *He smiles.*", 2f, "Reasonable", false));
            reactions.Add(CreateReaction(q4T2, EmotionState.Attraction, ConnectionTier.Tier2_Maybe, "She smiles. “You noticed.” Then she reaches for her drink instead.", 2f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q4T2, EmotionState.CalmAnxiety, "“You good? You don’t have to ignore something important.”", 0f, "Reasonable", false));
            reactions.Add(CreateReaction(q4T2, EmotionState.CalmAnxiety, ConnectionTier.Tier2_Maybe, "“Nothing important.” She reassures him, though his concern makes the interruption feel heavier.", 0f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q4T2, EmotionState.CalmConfidence, "“No rush. We’ve got time.”", 8f, "Reasonable", false));
            reactions.Add(CreateReaction(q4T2, EmotionState.CalmConfidence, ConnectionTier.Tier2_Maybe, "“Thanks.” She smiles and stays fully engaged.", 8f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q4T2, EmotionState.CalmAttraction, "*He smiles.* “Thanks for staying present.”", 2f, "Reasonable", false));
            reactions.Add(CreateReaction(q4T2, EmotionState.CalmAttraction, ConnectionTier.Tier2_Maybe, "“I want to be here.” She gives him a small smile, then turns the phone another inch away.", 2f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q4T2, EmotionState.AnxietyConfidence, "“You don’t have to prove anything. I’m not watching the phone.”", 8f, "Reasonable", false));
            reactions.Add(CreateReaction(q4T2, EmotionState.AnxietyConfidence, ConnectionTier.Tier2_Maybe, "“You really don’t have to watch what I do with my phone.” Her tone stays polite, but there is a clear boundary now.", 8f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q4T2, EmotionState.AnxietyAttraction, "“Sorry—if you need to deal with something, really, it’s okay.”", -4f, "Poor", false));
            reactions.Add(CreateReaction(q4T2, EmotionState.AnxietyAttraction, ConnectionTier.Tier2_Maybe, "“It’s okay.” She gives him a reassuring smile. “You don’t have to worry about that.”", -4f, "sympathetic_concern"));
            answers.Add(CreateAnswer(q4T2, EmotionState.ConfidenceAttraction, "“I’m flattered you left it.”", 10f, "HighFit", false));
            reactions.Add(CreateReaction(q4T2, EmotionState.ConfidenceAttraction, ConnectionTier.Tier2_Maybe, "“I’ll take the compliment.” She grins, then goes right back to the conversation.", 10f, "flirty_wink"));
            answers.Add(CreateAnswer(q4T2, EmotionState.FrozenBlank, "*He watches her put down the phone in total silence, letting the moment hang in the air.*", -8f, "Poor", true));
            reactions.Add(CreateReaction(q4T2, EmotionState.FrozenBlank, ConnectionTier.Tier2_Maybe, "She sighs quietly and rests her chin on her palm, waiting for him to re-engage.", -8f, "discomfort"));

            // --- Slot 4 Tier 1: Type 5 – Bored/restless, wordless ---
            var q4T1 = ScriptableObject.CreateInstance<QuestionData>();
            q4T1.slotIndex = 4;
            q4T1.connectionTier = ConnectionTier.Tier1_Awkward;
            q4T1.scenarioType = "Type 5 – Bored/restless, wordless";
            q4T1.questionText = "*She checks the time on her phone, not subtly. A little restless.*";
            q4T1.responseTimeWindow = 6.0f;
            q4T1.socialEventProfile = new SocialEventProfile
            {
                requiresPlayerInitiation = false,
                primaryNodeOverride = InternalNodeType.Body,
                effects = new List<TargetedEffect>
                {
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Body,
                        effectType = SocialEffectType.StressEvent,
                        stressEventSubtype = StressEventSubtype.SweatSurge,
                        magnitude = 25.0f
                    },
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Voice,
                        effectType = SocialEffectType.StressEvent,
                        stressEventSubtype = StressEventSubtype.AwkwardSilence,
                        magnitude = 25.0f
                    },
                }
            };
            questions.Add(q4T1);

            answers.Add(CreateAnswer(q4T1, EmotionState.Calm, "“We doing alright?”", -10f, "ActivelyWrong", false));
            reactions.Add(CreateReaction(q4T1, EmotionState.Calm, ConnectionTier.Tier1_Awkward, "“We’re okay.” She smiles. “I was just checking the time.”", -10f, "discomfort"));
            answers.Add(CreateAnswer(q4T1, EmotionState.Anxiety, "“Is it getting late? Sorry, I didn’t realise how long we’d been sitting here.”", -6f, "Poor", false));
            reactions.Add(CreateReaction(q4T1, EmotionState.Anxiety, ConnectionTier.Tier1_Awkward, "“No, no—it’s not that late.” She reassures him, but she also notices how quickly he panicked.", -6f, "sympathetic_concern"));
            answers.Add(CreateAnswer(q4T1, EmotionState.Confidence, "“You look like you’ve got somewhere to be.”", 14f, "HighFit", false));
            reactions.Add(CreateReaction(q4T1, EmotionState.Confidence, ConnectionTier.Tier1_Awkward, "She nods. “I do, actually. I’ve got a little time, though.” The honesty keeps the moment grounded.", 14f, "warm_interest"));
            answers.Add(CreateAnswer(q4T1, EmotionState.Attraction, "“Please tell me you’re not checking how long until you can escape me.” *He smiles, half-joking.*", -4f, "Poor", false));
            reactions.Add(CreateReaction(q4T1, EmotionState.Attraction, ConnectionTier.Tier1_Awkward, "She laughs. “Not quite an escape plan.” But she does not deny being restless.", -4f, "discomfort"));
            answers.Add(CreateAnswer(q4T1, EmotionState.CalmAnxiety, "“Hey. If you need to head out, just tell me. No pressure.”", 2f, "Reasonable", false));
            reactions.Add(CreateReaction(q4T1, EmotionState.CalmAnxiety, ConnectionTier.Tier1_Awkward, "She looks relieved. “Thank you.” The lack of pressure noticeably relaxes her.", 2f, "relieved_sigh"));
            answers.Add(CreateAnswer(q4T1, EmotionState.CalmConfidence, "“You alright for time?”", 6f, "Reasonable", false));
            reactions.Add(CreateReaction(q4T1, EmotionState.CalmConfidence, ConnectionTier.Tier1_Awkward, "“Yeah, I’m alright for time.” She puts the phone away and settles back in.", 6f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q4T1, EmotionState.CalmAttraction, "“Everything okay? I’m happy to stay as long as you are.”", -6f, "Poor", false));
            reactions.Add(CreateReaction(q4T1, EmotionState.CalmAttraction, ConnectionTier.Tier1_Awkward, "“Everything’s okay.” She smiles, though she still seems a little distracted.", -6f, "discomfort"));
            answers.Add(CreateAnswer(q4T1, EmotionState.AnxietyConfidence, "“Okay, that was definitely a time check.” *He laughs.* “I’m guessing I should be worried.”", 8f, "Reasonable", false));
            reactions.Add(CreateReaction(q4T1, EmotionState.AnxietyConfidence, ConnectionTier.Tier1_Awkward, "“I was.” She smiles at the joke, but the self-consciousness is now shared.", 8f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q4T1, EmotionState.AnxietyAttraction, "“Are you bored? I know we don’t have to force this.”", -6f, "Poor", false));
            reactions.Add(CreateReaction(q4T1, EmotionState.AnxietyAttraction, ConnectionTier.Tier1_Awkward, "“No.” She says it quickly. “I’m not bored.” The reassurance makes her feel responsible for his nerves.", -6f, "sympathetic_concern"));
            answers.Add(CreateAnswer(q4T1, EmotionState.ConfidenceAttraction, "“You checking the time already? I was hoping you’d lose track with me.”", -8f, "Poor", false));
            reactions.Add(CreateReaction(q4T1, EmotionState.ConfidenceAttraction, ConnectionTier.Tier1_Awkward, "“I’m not keeping score.” She laughs, but the “lose track with me” line is a little much in this moment.", -8f, "discomfort"));
            answers.Add(CreateAnswer(q4T1, EmotionState.FrozenBlank, "*He notices the time check and goes quiet.*", -10f, "ActivelyWrong", true));
            reactions.Add(CreateReaction(q4T1, EmotionState.FrozenBlank, ConnectionTier.Tier1_Awkward, "She pockets her phone, looking around the room as if calculating when she can politely leave.", -10f, "discomfort"));

            return (questions, answers, reactions);
        }


        private static (List<QuestionData>, List<AnswerData>, List<ReactionData>) BuildSlot5()
        {
            var questions = new List<QuestionData>();
            var answers = new List<AnswerData>();
            var reactions = new List<ReactionData>();

            // --- Slot 5 Tier 4: Type 1 – Direct question ---
            var q5T4 = ScriptableObject.CreateInstance<QuestionData>();
            q5T4.slotIndex = 5;
            q5T4.connectionTier = ConnectionTier.Tier4_SecondDate;
            q5T4.scenarioType = "Type 1 – Direct question";
            q5T4.questionText = "\"Can I ask something real? What are you actually looking for?\"";
            q5T4.responseTimeWindow = 6.0f;
            q5T4.socialEventProfile = new SocialEventProfile
            {
                requiresPlayerInitiation = false,
                primaryNodeOverride = InternalNodeType.Heart,
                effects = new List<TargetedEffect>
                {
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Heart,
                        effectType = SocialEffectType.StressEvent,
                        stressEventSubtype = StressEventSubtype.HeartFlutter,
                        magnitude = 25.0f
                    },
                }
            };
            questions.Add(q5T4);

            answers.Add(CreateAnswer(q5T4, EmotionState.Calm, "“Something real, eventually. I’m not trying to force anything tonight.”", 8f, "Reasonable", false));
            reactions.Add(CreateReaction(q5T4, EmotionState.Calm, ConnectionTier.Tier4_SecondDate, "She nods slowly. “Yeah. I think I’m looking for something that can actually become something.”", 8f, "thinking_chintap"));
            answers.Add(CreateAnswer(q5T4, EmotionState.Anxiety, "“I don’t know if I have a good answer. I want something serious, I think—just not something forced.”", 0f, "Reasonable", false));
            reactions.Add(CreateReaction(q5T4, EmotionState.Anxiety, ConnectionTier.Tier4_SecondDate, "“That’s okay.” She smiles. “I don’t think there’s a perfect answer to that.”", 0f, "thinking_chintap"));
            answers.Add(CreateAnswer(q5T4, EmotionState.Confidence, "“I’m looking for a relationship if I meet the right person.”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q5T4, EmotionState.Confidence, ConnectionTier.Tier4_SecondDate, "She nods. “Same.” It is not a bad answer, but it feels more formal than personal.", 4f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q5T4, EmotionState.Attraction, "“Honestly? I’d like to see where this goes with you.”", 8f, "Reasonable", false));
            reactions.Add(CreateReaction(q5T4, EmotionState.Attraction, ConnectionTier.Tier4_SecondDate, "Her smile fades slightly. “I appreciate the honesty.” She gives the answer room instead of matching the intensity.", 8f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q5T4, EmotionState.CalmAnxiety, "“I’d like something real. I just don’t want to pretend I know exactly what it’ll look like yet.”", 12f, "HighFit", false));
            reactions.Add(CreateReaction(q5T4, EmotionState.CalmAnxiety, ConnectionTier.Tier4_SecondDate, "“I like that.” She looks relieved by the lack of pressure.", 12f, "warm_interest"));
            answers.Add(CreateAnswer(q5T4, EmotionState.CalmConfidence, "“A genuine relationship. I’m not in a rush, but I’m here because I want one.”", 16f, "HighFit", false));
            reactions.Add(CreateReaction(q5T4, EmotionState.CalmConfidence, ConnectionTier.Tier4_SecondDate, "“That’s basically where I’m at too.” She leans in a little, more invested now.", 16f, "second_date"));
            answers.Add(CreateAnswer(q5T4, EmotionState.CalmAttraction, "“Something that feels easy and worth showing up for. I’m liking what I’ve got so far.”", 18f, "HighFit", false));
            reactions.Add(CreateReaction(q5T4, EmotionState.CalmAttraction, ConnectionTier.Tier4_SecondDate, "“I like where your head is.” Her smile stays for a beat longer.", 18f, "second_date"));
            answers.Add(CreateAnswer(q5T4, EmotionState.AnxietyConfidence, "“I know what I want.” *Beat.* “I mean—I think I do. Something serious. Eventually.”", -4f, "Poor", false));
            reactions.Add(CreateReaction(q5T4, EmotionState.AnxietyConfidence, ConnectionTier.Tier4_SecondDate, "She smiles gently. “You don’t have to know the entire answer tonight.”", -4f, "discomfort"));
            answers.Add(CreateAnswer(q5T4, EmotionState.AnxietyAttraction, "“I want something real. And... I’d be lying if I said I wasn’t hoping this could become that.”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q5T4, EmotionState.AnxietyAttraction, ConnectionTier.Tier4_SecondDate, "“Yeah.” She holds his eyes. “I think I’d like something real too.”", 4f, "thinking_chintap"));
            answers.Add(CreateAnswer(q5T4, EmotionState.ConfidenceAttraction, "“A relationship with someone I actually like. I’m pretty sure you can guess where my head is tonight.”", -8f, "Poor", false));
            reactions.Add(CreateReaction(q5T4, EmotionState.ConfidenceAttraction, ConnectionTier.Tier4_SecondDate, "She takes a small breath. “Maybe we should just keep tonight about tonight.” The warmth becomes more cautious.", -8f, "discomfort"));
            answers.Add(CreateAnswer(q5T4, EmotionState.FrozenBlank, "*He swallows hard, caught off-guard by the sincerity, struggling to find words.*", -2f, "Poor", true));
            reactions.Add(CreateReaction(q5T4, EmotionState.FrozenBlank, ConnectionTier.Tier4_SecondDate, "She gives a warm, understanding nod. “Heavy question, I know. You don’t have to have a thesis.”", -2f, "discomfort"));

            // --- Slot 5 Tier 3: Type 1 – Direct question ---
            var q5T3 = ScriptableObject.CreateInstance<QuestionData>();
            q5T3.slotIndex = 5;
            q5T3.connectionTier = ConnectionTier.Tier3_Strong;
            q5T3.scenarioType = "Type 1 – Direct question";
            q5T3.questionText = "\"What are you looking for, out of this, I guess.\"";
            q5T3.responseTimeWindow = 6.0f;
            q5T3.socialEventProfile = new SocialEventProfile
            {
                requiresPlayerInitiation = false,
                primaryNodeOverride = InternalNodeType.Brain,
                effects = new List<TargetedEffect>
                {
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Brain,
                        effectType = SocialEffectType.StressEvent,
                        stressEventSubtype = StressEventSubtype.Panic,
                        magnitude = 25.0f
                    },
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Heart,
                        effectType = SocialEffectType.StressEvent,
                        stressEventSubtype = StressEventSubtype.HeartFlutter,
                        magnitude = 25.0f
                    },
                }
            };
            questions.Add(q5T3);

            answers.Add(CreateAnswer(q5T3, EmotionState.Calm, "“I’m open to something serious. I’d rather let it grow naturally.”", 6f, "Reasonable", false));
            reactions.Add(CreateReaction(q5T3, EmotionState.Calm, ConnectionTier.Tier3_Strong, "“That sounds reasonable.” She smiles, relieved by the lack of pressure.", 6f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q5T3, EmotionState.Anxiety, "“Out of this? I mean... a good date. Maybe more. Sorry, that sounded vague.”", 0f, "Reasonable", false));
            reactions.Add(CreateReaction(q5T3, EmotionState.Anxiety, ConnectionTier.Tier3_Strong, "“Hey, you don’t have to solve it right now.” She softens her tone, trying to take the pressure back off him.", 0f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q5T3, EmotionState.Confidence, "“I’d like to meet someone I actually want to see again.”", 12f, "HighFit", false));
            reactions.Add(CreateReaction(q5T3, EmotionState.Confidence, ConnectionTier.Tier3_Strong, "“Okay.” She nods. “I think I’m looking for something a little more specific than that.”", 12f, "warm_interest"));
            answers.Add(CreateAnswer(q5T3, EmotionState.Attraction, "“Right now? More time with you would be nice.”", 8f, "Reasonable", false));
            reactions.Add(CreateReaction(q5T3, EmotionState.Attraction, ConnectionTier.Tier3_Strong, "“More time together?” She smiles, catching the implication.", 8f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q5T3, EmotionState.CalmAnxiety, "“Something worth continuing. I don’t need to define the whole future tonight.”", 6f, "Reasonable", false));
            reactions.Add(CreateReaction(q5T3, EmotionState.CalmAnxiety, ConnectionTier.Tier3_Strong, "“That’s fair.” She looks thoughtful, still figuring out what she wants from the answer.", 6f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q5T3, EmotionState.CalmConfidence, "“I’m here to see if there’s something worth building.”", 14f, "HighFit", false));
            reactions.Add(CreateReaction(q5T3, EmotionState.CalmConfidence, ConnectionTier.Tier3_Strong, "“That’s pretty much exactly what I mean.” She smiles with genuine relief.", 14f, "warm_interest"));
            answers.Add(CreateAnswer(q5T3, EmotionState.CalmAttraction, "“Honestly, I’m enjoying this enough that I’m curious where it could go.”", 14f, "HighFit", false));
            reactions.Add(CreateReaction(q5T3, EmotionState.CalmAttraction, ConnectionTier.Tier3_Strong, "“I was hoping you’d say something like that.” Her expression warms.", 14f, "warm_interest"));
            answers.Add(CreateAnswer(q5T3, EmotionState.AnxietyConfidence, "“I’m looking for something real. That’s the answer.” *Beat.* “Probably.”", 8f, "Reasonable", false));
            reactions.Add(CreateReaction(q5T3, EmotionState.AnxietyConfidence, ConnectionTier.Tier3_Strong, "“Alright.” She smiles. “You don’t have to make it sound so official.”", 8f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q5T3, EmotionState.AnxietyAttraction, "“I’d like this to go somewhere. I just don’t want to get ahead of myself.”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q5T3, EmotionState.AnxietyAttraction, ConnectionTier.Tier3_Strong, "“I get it.” She gives him a reassuring smile, but the caution keeps her from leaning in.", 4f, "sympathetic_concern"));
            answers.Add(CreateAnswer(q5T3, EmotionState.ConfidenceAttraction, "“I’m looking for a reason to ask for a second date.”", 16f, "HighFit", false));
            reactions.Add(CreateReaction(q5T3, EmotionState.ConfidenceAttraction, ConnectionTier.Tier3_Strong, "“A second date is a pretty concrete place to start.” She smiles, teasing just enough.", 16f, "flirty_wink"));
            answers.Add(CreateAnswer(q5T3, EmotionState.FrozenBlank, "*He hesitates, lips parting, but ends up looking down at his plate in silence.*", -2f, "Poor", true));
            reactions.Add(CreateReaction(q5T3, EmotionState.FrozenBlank, ConnectionTier.Tier3_Strong, "She sips her drink, giving him space. “It’s okay, take a second. No right answer.”", -2f, "discomfort"));

            // --- Slot 5 Tier 2: Type 9 – Reflective, half-verbal ---
            var q5T2 = ScriptableObject.CreateInstance<QuestionData>();
            q5T2.slotIndex = 5;
            q5T2.connectionTier = ConnectionTier.Tier2_Maybe;
            q5T2.scenarioType = "Type 9 – Reflective, half-verbal";
            q5T2.questionText = "\"Can I ask you something, or is it too soon.\"";
            q5T2.responseTimeWindow = 6.0f;
            q5T2.socialEventProfile = new SocialEventProfile
            {
                requiresPlayerInitiation = false,
                primaryNodeOverride = InternalNodeType.Brain,
                effects = new List<TargetedEffect>
                {
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Brain,
                        effectType = SocialEffectType.StressEvent,
                        stressEventSubtype = StressEventSubtype.Overthinking,
                        magnitude = 25.0f
                    },
                }
            };
            questions.Add(q5T2);

            answers.Add(CreateAnswer(q5T2, EmotionState.Calm, "“You can ask. I’ll tell you if it’s too soon.”", 8f, "Reasonable", false));
            reactions.Add(CreateReaction(q5T2, EmotionState.Calm, ConnectionTier.Tier2_Maybe, "“Okay.” She smiles, finally asking the question she was holding back.", 8f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q5T2, EmotionState.Anxiety, "“Sure. Unless it’s... yeah, no, ask.”", -2f, "Poor", false));
            reactions.Add(CreateReaction(q5T2, EmotionState.Anxiety, ConnectionTier.Tier2_Maybe, "“You can just tell me if it’s too personal.” She backs off slightly, sensing the hesitation.", -2f, "discomfort"));
            answers.Add(CreateAnswer(q5T2, EmotionState.Confidence, "“Go for it.”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q5T2, EmotionState.Confidence, ConnectionTier.Tier2_Maybe, "“Alright.” She smiles. “Then I’m asking.” The confidence makes her commit to the question.", 4f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q5T2, EmotionState.Attraction, "“You’ve got my attention.”", 6f, "Reasonable", false));
            reactions.Add(CreateReaction(q5T2, EmotionState.Attraction, ConnectionTier.Tier2_Maybe, "“That’s a dangerous thing to say.” She laughs, but keeps the question light.", 6f, "nervous_laugh"));
            answers.Add(CreateAnswer(q5T2, EmotionState.CalmAnxiety, "“Yeah. Ask.” *A small smile.* “I’m not scared of the question.”", 6f, "Reasonable", false));
            reactions.Add(CreateReaction(q5T2, EmotionState.CalmAnxiety, ConnectionTier.Tier2_Maybe, "She smiles, relieved. “Okay. Then I will.” She seems more willing to be vulnerable.", 6f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q5T2, EmotionState.CalmConfidence, "“Of course. What’ve you got?”", 14f, "HighFit", false));
            reactions.Add(CreateReaction(q5T2, EmotionState.CalmConfidence, ConnectionTier.Tier2_Maybe, "“Alright.” She sits a little straighter, clearly ready to ask.", 14f, "warm_interest"));
            answers.Add(CreateAnswer(q5T2, EmotionState.CalmAttraction, "“You can ask me. I’m curious now.”", 12f, "HighFit", false));
            reactions.Add(CreateReaction(q5T2, EmotionState.CalmAttraction, ConnectionTier.Tier2_Maybe, "“Okay...” She smiles, but the invitation makes the moment feel heavier than she expected.", 12f, "warm_interest"));
            answers.Add(CreateAnswer(q5T2, EmotionState.AnxietyConfidence, "“Ask whatever you want.” *Beat.* “I’m sure I can handle it.”", 6f, "Reasonable", false));
            reactions.Add(CreateReaction(q5T2, EmotionState.AnxietyConfidence, ConnectionTier.Tier2_Maybe, "“I wasn’t trying to scare you.” She laughs once, but the bravado makes her hesitate again.", 6f, "nervous_laugh"));
            answers.Add(CreateAnswer(q5T2, EmotionState.AnxietyAttraction, "“Yeah. You can ask. I might need a second, though.”", 2f, "Reasonable", false));
            reactions.Add(CreateReaction(q5T2, EmotionState.AnxietyAttraction, ConnectionTier.Tier2_Maybe, "“Yeah, I know.” She smiles. “I might still need to phrase it carefully.”", 2f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q5T2, EmotionState.ConfidenceAttraction, "“You can ask. Now I definitely want to hear it.”", 8f, "Reasonable", false));
            reactions.Add(CreateReaction(q5T2, EmotionState.ConfidenceAttraction, ConnectionTier.Tier2_Maybe, "“Good.” She smiles. “Because now I really do want to ask.”", 8f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q5T2, EmotionState.FrozenBlank, "*He stares at her blankly, paralyzed by anticipation of what she might ask.*", 0f, "Reasonable", true));
            reactions.Add(CreateReaction(q5T2, EmotionState.FrozenBlank, ConnectionTier.Tier2_Maybe, "She waves a hand dismissively with a wry smile. “Never mind, probably too soon.”", 0f, "neutral_acknowledge"));

            // --- Slot 5 Tier 1: Type 7 – She notices something ---
            var q5T1 = ScriptableObject.CreateInstance<QuestionData>();
            q5T1.slotIndex = 5;
            q5T1.connectionTier = ConnectionTier.Tier1_Awkward;
            q5T1.scenarioType = "Type 7 – She notices something";
            q5T1.questionText = "\"You seem like you'd rather be somewhere else. Would you?\"";
            q5T1.responseTimeWindow = 6.0f;
            q5T1.socialEventProfile = new SocialEventProfile
            {
                requiresPlayerInitiation = false,
                primaryNodeOverride = InternalNodeType.Brain,
                effects = new List<TargetedEffect>
                {
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Brain,
                        effectType = SocialEffectType.StressEvent,
                        stressEventSubtype = StressEventSubtype.Panic,
                        magnitude = 45.0f
                    },
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Body,
                        effectType = SocialEffectType.StressEvent,
                        stressEventSubtype = StressEventSubtype.SweatSurge,
                        magnitude = 45.0f
                    },
                }
            };
            questions.Add(q5T1);

            answers.Add(CreateAnswer(q5T1, EmotionState.Calm, "“No. I’m here because I want to be here. I’ve probably just been in my head a bit.”", -6f, "Poor", false));
            reactions.Add(CreateReaction(q5T1, EmotionState.Calm, ConnectionTier.Tier1_Awkward, "She nods, visibly relieved. “Okay. I just needed to know.”", -6f, "discomfort"));
            answers.Add(CreateAnswer(q5T1, EmotionState.Anxiety, "“No—no, not at all. Sorry, do I really look like I want to leave?”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q5T1, EmotionState.Anxiety, ConnectionTier.Tier1_Awkward, "“No, no. You don’t have to convince me.” She softens immediately, but the moment has already made her uneasy.", 4f, "sympathetic_concern"));
            answers.Add(CreateAnswer(q5T1, EmotionState.Confidence, "“No. If I wanted to leave, I’d tell you.”", -14f, "ActivelyWrong", false));
            reactions.Add(CreateReaction(q5T1, EmotionState.Confidence, ConnectionTier.Tier1_Awkward, "She gives a short nod. “Got it.” The certainty does not reassure her; it feels dismissive.", -14f, "shut_down"));
            answers.Add(CreateAnswer(q5T1, EmotionState.Attraction, "“No. I’d actually rather stay here with you.”", 0f, "Reasonable", false));
            reactions.Add(CreateReaction(q5T1, EmotionState.Attraction, ConnectionTier.Tier1_Awkward, "“Okay.” She smiles faintly. “I’m glad.” It is reassuring, but still a little intense.", 0f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q5T1, EmotionState.CalmAnxiety, "“No. I’m nervous, but I’m not trying to get out of this.”", 16f, "HighFit", false));
            reactions.Add(CreateReaction(q5T1, EmotionState.CalmAnxiety, ConnectionTier.Tier1_Awkward, "Her shoulders drop. “Okay. Good.” She looks genuinely relieved.", 16f, "second_date"));
            answers.Add(CreateAnswer(q5T1, EmotionState.CalmConfidence, "“Definitely not. I’m here because I want to see where this goes.”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q5T1, EmotionState.CalmConfidence, ConnectionTier.Tier1_Awkward, "“Alright.” She smiles. “Then we’re on the same page.”", 4f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q5T1, EmotionState.CalmAttraction, "“No. I’m having a good time. I’m just quieter than I expected.”", -4f, "Poor", false));
            reactions.Add(CreateReaction(q5T1, EmotionState.CalmAttraction, ConnectionTier.Tier1_Awkward, "“That’s good.” She gives him a small, warm smile.", -4f, "discomfort"));
            answers.Add(CreateAnswer(q5T1, EmotionState.AnxietyConfidence, "“No. I’m fine.” *Beat.* “I’m not exactly convincing you, am I?”", 6f, "Reasonable", false));
            reactions.Add(CreateReaction(q5T1, EmotionState.AnxietyConfidence, ConnectionTier.Tier1_Awkward, "She notices the struggle and smiles gently. “You don’t have to look so determined about it.”", 6f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q5T1, EmotionState.AnxietyAttraction, "“No, I don’t want to leave. I’m just... really in my head because I care how this goes.”", 8f, "Reasonable", false));
            reactions.Add(CreateReaction(q5T1, EmotionState.AnxietyAttraction, ConnectionTier.Tier1_Awkward, "“Okay.” Her expression softens. “That actually helps.”", 8f, "sympathetic_concern"));
            answers.Add(CreateAnswer(q5T1, EmotionState.ConfidenceAttraction, "“No. Quite the opposite. I was hoping you’d want me to stay a while.”", -10f, "ActivelyWrong", false));
            reactions.Add(CreateReaction(q5T1, EmotionState.ConfidenceAttraction, ConnectionTier.Tier1_Awkward, "She smiles politely. “Okay.” The hint of “I want you to stay” is a little more than she wants to carry right now.", -10f, "discomfort"));
            answers.Add(CreateAnswer(q5T1, EmotionState.FrozenBlank, "“No, I...” *He loses the sentence.*", -8f, "Poor", false));
            reactions.Add(CreateReaction(q5T1, EmotionState.FrozenBlank, ConnectionTier.Tier1_Awkward, "She leans back, arms crossing loosely. “That’s pretty much an answer in itself.”", -8f, "discomfort"));

            return (questions, answers, reactions);
        }


        private static (List<QuestionData>, List<AnswerData>, List<ReactionData>) BuildSlot6()
        {
            var questions = new List<QuestionData>();
            var answers = new List<AnswerData>();
            var reactions = new List<ReactionData>();

            // --- Slot 6 Tier 4: Type 3 – Vulnerable admission ---
            var q6T4 = ScriptableObject.CreateInstance<QuestionData>();
            q6T4.slotIndex = 6;
            q6T4.connectionTier = ConnectionTier.Tier4_SecondDate;
            q6T4.scenarioType = "Type 3 – Vulnerable admission";
            q6T4.questionText = "\"Can I tell you something embarrassing? I almost cancelled tonight.\"";
            q6T4.responseTimeWindow = 6.0f;
            q6T4.socialEventProfile = new SocialEventProfile
            {
                requiresPlayerInitiation = false,
                primaryNodeOverride = InternalNodeType.Heart,
                effects = new List<TargetedEffect>
                {
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Heart,
                        effectType = SocialEffectType.DirectEmotionPush,
                        pushTargetEmotion = CoreEmotion.Attraction,
                        magnitude = 35.0f
                    },
                }
            };
            questions.Add(q6T4);

            answers.Add(CreateAnswer(q6T4, EmotionState.Calm, "“Really? I’m glad you didn’t.”", 16f, "HighFit", false));
            reactions.Add(CreateReaction(q6T4, EmotionState.Calm, ConnectionTier.Tier4_SecondDate, "“Me too.” She smiles. “I’m glad we both ignored that impulse.”", 16f, "second_date"));
            answers.Add(CreateAnswer(q6T4, EmotionState.Anxiety, "“Wait, really? Was it because of me?”", 8f, "Reasonable", false));
            reactions.Add(CreateReaction(q6T4, EmotionState.Anxiety, ConnectionTier.Tier4_SecondDate, "“No, not because of you.” She reassures him quickly, seeing where his mind went.", 8f, "sympathetic_concern"));
            answers.Add(CreateAnswer(q6T4, EmotionState.Confidence, "“I’m glad you decided against it.”", -16f, "ActivelyWrong", false));
            reactions.Add(CreateReaction(q6T4, EmotionState.Confidence, ConnectionTier.Tier4_SecondDate, "“So do I.” She smiles. “You seem pretty happy I stayed.”", -16f, "shut_down"));
            answers.Add(CreateAnswer(q6T4, EmotionState.Attraction, "“Honestly, I’m really glad you came.”", 2f, "Reasonable", false));
            reactions.Add(CreateReaction(q6T4, EmotionState.Attraction, ConnectionTier.Tier4_SecondDate, "“I’m glad too.” Her voice softens, but she does not make it bigger than it is.", 2f, "sympathetic_concern"));
            answers.Add(CreateAnswer(q6T4, EmotionState.CalmAnxiety, "“I get that. I’m glad we both showed up anyway.”", 18f, "HighFit", false));
            reactions.Add(CreateReaction(q6T4, EmotionState.CalmAnxiety, ConnectionTier.Tier4_SecondDate, "“Yeah.” She smiles. “Maybe that says something good already.”", 18f, "second_date"));
            answers.Add(CreateAnswer(q6T4, EmotionState.CalmConfidence, "“Good thing you didn’t. I’m having a pretty good night.”", 8f, "Reasonable", false));
            reactions.Add(CreateReaction(q6T4, EmotionState.CalmConfidence, ConnectionTier.Tier4_SecondDate, "“Good.” She smiles genuinely. “I’m really glad I didn’t.”", 8f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q6T4, EmotionState.CalmAttraction, "“I’m really glad you came. I’ve liked having you here.”", 16f, "HighFit", false));
            reactions.Add(CreateReaction(q6T4, EmotionState.CalmAttraction, ConnectionTier.Tier4_SecondDate, "“I am too.” She holds his eyes for a moment. “I’ve had a really nice time.”", 16f, "second_date"));
            answers.Add(CreateAnswer(q6T4, EmotionState.AnxietyConfidence, "“Almost cancelled?” *He laughs.* “Well, I’m glad you made the right call.”", -10f, "ActivelyWrong", false));
            reactions.Add(CreateReaction(q6T4, EmotionState.AnxietyConfidence, ConnectionTier.Tier4_SecondDate, "She laughs softly. “I’m glad I made the right call.” The “right call” joke lands a little awkwardly.", -10f, "discomfort"));
            answers.Add(CreateAnswer(q6T4, EmotionState.AnxietyAttraction, "“You almost didn’t come? Oh... I’m really glad you did.”", 10f, "HighFit", false));
            reactions.Add(CreateReaction(q6T4, EmotionState.AnxietyAttraction, ConnectionTier.Tier4_SecondDate, "“I almost did too.” She smiles, but the nervousness makes the vulnerability feel heavier.", 10f, "warm_interest"));
            answers.Add(CreateAnswer(q6T4, EmotionState.ConfidenceAttraction, "“And here I was thinking you were excited to meet me.” *He grins.* “Still glad you came.”", -16f, "ActivelyWrong", false));
            reactions.Add(CreateReaction(q6T4, EmotionState.ConfidenceAttraction, ConnectionTier.Tier4_SecondDate, "Her smile tightens. “Well... I’m glad I came.” She leaves out the joke she might otherwise have made.", -16f, "shut_down"));
            answers.Add(CreateAnswer(q6T4, EmotionState.FrozenBlank, "*His eyes widen, struck silent by the admission, smiling softly.*", 6f, "Reasonable", true));
            reactions.Add(CreateReaction(q6T4, EmotionState.FrozenBlank, ConnectionTier.Tier4_SecondDate, "She laughs a little nervously, then relaxes seeing his expression. “Don’t look at me like that, I’m glad I came!”", 6f, "relieved_sigh"));

            // --- Slot 6 Tier 3: Type 6 – Physical mishap, wordless ---
            var q6T3 = ScriptableObject.CreateInstance<QuestionData>();
            q6T3.slotIndex = 6;
            q6T3.connectionTier = ConnectionTier.Tier3_Strong;
            q6T3.scenarioType = "Type 6 – Physical mishap, wordless";
            q6T3.questionText = "*A server brushes the table; drinks wobble. She grabs hers just in time, exhales a laugh.*";
            q6T3.responseTimeWindow = 6.0f;
            q6T3.socialEventProfile = new SocialEventProfile
            {
                requiresPlayerInitiation = false,
                primaryNodeOverride = InternalNodeType.Lungs,
                effects = new List<TargetedEffect>
                {
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Lungs,
                        effectType = SocialEffectType.StressEvent,
                        stressEventSubtype = StressEventSubtype.HeartFlutter, // Breath & heart startled
                        magnitude = 15.0f
                    },
                }
            };
            questions.Add(q6T3);

            answers.Add(CreateAnswer(q6T3, EmotionState.Calm, "“We survived.”", 10f, "HighFit", false));
            reactions.Add(CreateReaction(q6T3, EmotionState.Calm, ConnectionTier.Tier3_Strong, "She laughs. “Okay, I definitely need to stop doing that.”", 10f, "flirty_wink"));
            answers.Add(CreateAnswer(q6T3, EmotionState.Anxiety, "“Oh—okay, careful. That was close.”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q6T3, EmotionState.Anxiety, ConnectionTier.Tier3_Strong, "“I’m okay.” She laughs, but the panic in his reaction makes her more self-conscious.", 4f, "nervous_laugh"));
            answers.Add(CreateAnswer(q6T3, EmotionState.Confidence, "“Nice reflexes.”", 12f, "HighFit", false));
            reactions.Add(CreateReaction(q6T3, EmotionState.Confidence, ConnectionTier.Tier3_Strong, "She grins. “Thank you. Finally, someone appreciates a good save.”", 12f, "flirty_wink"));
            answers.Add(CreateAnswer(q6T3, EmotionState.Attraction, "*He laughs.* “Okay, we’re getting tested tonight.”", 6f, "Reasonable", false));
            reactions.Add(CreateReaction(q6T3, EmotionState.Attraction, ConnectionTier.Tier3_Strong, "“You’re calling that adorable too?” she says, laughing.", 6f, "nervous_laugh"));
            answers.Add(CreateAnswer(q6T3, EmotionState.CalmAnxiety, "“You got it. Nobody died.”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q6T3, EmotionState.CalmAnxiety, ConnectionTier.Tier3_Strong, "“We’re good.” She smiles, settling back down.", 4f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q6T3, EmotionState.CalmConfidence, "“Strong save.”", 10f, "HighFit", false));
            reactions.Add(CreateReaction(q6T3, EmotionState.CalmConfidence, ConnectionTier.Tier3_Strong, "“Strong save.” She points at him. “You’re learning my vocabulary already.”", 10f, "warm_interest"));
            answers.Add(CreateAnswer(q6T3, EmotionState.CalmAttraction, "*He laughs with her.* “You alright?”", 8f, "Reasonable", false));
            reactions.Add(CreateReaction(q6T3, EmotionState.CalmAttraction, ConnectionTier.Tier3_Strong, "She laughs softly. “I’m fine.” The little shared mishap keeps things easy.", 8f, "nervous_laugh"));
            answers.Add(CreateAnswer(q6T3, EmotionState.AnxietyConfidence, "“Okay, that one was nearly a disaster.” *He grins.* “But we handled it.”", 14f, "HighFit", false));
            reactions.Add(CreateReaction(q6T3, EmotionState.AnxietyConfidence, ConnectionTier.Tier3_Strong, "“Yes, very smooth.” She laughs, catching the nerves behind the joke.", 14f, "flirty_wink"));
            answers.Add(CreateAnswer(q6T3, EmotionState.AnxietyAttraction, "“That scared me more than it should have.” *He laughs.*", 6f, "Reasonable", false));
            reactions.Add(CreateReaction(q6T3, EmotionState.AnxietyAttraction, ConnectionTier.Tier3_Strong, "“I’m okay, seriously.” She puts a hand on the table, a little overwhelmed by the concern.", 6f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q6T3, EmotionState.ConfidenceAttraction, "“I’m starting to think this table has it out for us.”", 16f, "HighFit", false));
            reactions.Add(CreateReaction(q6T3, EmotionState.ConfidenceAttraction, ConnectionTier.Tier3_Strong, "“Please don’t pretend it didn’t happen.” She laughs, but the teasing feels slightly forced.", 16f, "flirty_wink"));
            answers.Add(CreateAnswer(q6T3, EmotionState.FrozenBlank, "*He sits frozen as the drinks wobble, reacting only after everything settles.*", 0f, "Reasonable", true));
            reactions.Add(CreateReaction(q6T3, EmotionState.FrozenBlank, ConnectionTier.Tier3_Strong, "She exhales a breathy laugh. “Well, that was exciting. Good reflexes from the table.”", 0f, "neutral_acknowledge"));

            // --- Slot 6 Tier 2: Type 4 – Comfortable-but-quiet, wordless ---
            var q6T2 = ScriptableObject.CreateInstance<QuestionData>();
            q6T2.slotIndex = 6;
            q6T2.connectionTier = ConnectionTier.Tier2_Maybe;
            q6T2.scenarioType = "Type 4 – Comfortable-but-quiet, wordless";
            q6T2.questionText = "*She's just eating, present, unbothered — not pushing, not pulling away.*";
            q6T2.responseTimeWindow = 6.0f;
            q6T2.socialEventProfile = new SocialEventProfile
            {
                requiresPlayerInitiation = false,
                primaryNodeOverride = InternalNodeType.Lungs,
                effects = new List<TargetedEffect>
                {
                }
            };
            questions.Add(q6T2);

            answers.Add(CreateAnswer(q6T2, EmotionState.Calm, "*He eats too, completely comfortable with the quiet.*", 12f, "HighFit", true));
            reactions.Add(CreateReaction(q6T2, EmotionState.Calm, ConnectionTier.Tier2_Maybe, "She keeps eating, then gives him a small smile. Nothing needs fixing.", 12f, "warm_interest"));
            answers.Add(CreateAnswer(q6T2, EmotionState.Anxiety, "*He takes a sip, glances at her, then deliberately leaves the silence alone.*", -4f, "Poor", true));
            reactions.Add(CreateReaction(q6T2, EmotionState.Anxiety, ConnectionTier.Tier2_Maybe, "She notices him filling the quiet and gently says, “You can just sit with me, you know.”", -4f, "discomfort"));
            answers.Add(CreateAnswer(q6T2, EmotionState.Confidence, "*He settles into the moment, unbothered by having nothing to say for a second.*", -2f, "Poor", true));
            reactions.Add(CreateReaction(q6T2, EmotionState.Confidence, ConnectionTier.Tier2_Maybe, "She looks comfortable enough to keep eating without worrying about the silence.", -2f, "awkward_silence"));
            answers.Add(CreateAnswer(q6T2, EmotionState.Attraction, "*He smiles to himself and keeps eating, clearly enjoying having her there.*", 6f, "Reasonable", true));
            reactions.Add(CreateReaction(q6T2, EmotionState.Attraction, ConnectionTier.Tier2_Maybe, "She smiles at him, but his focus makes her feel a little watched.", 6f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q6T2, EmotionState.CalmAnxiety, "*He exhales, shoulders dropping a little, and simply stays present.*", 4f, "Reasonable", true));
            reactions.Add(CreateReaction(q6T2, EmotionState.CalmAnxiety, ConnectionTier.Tier2_Maybe, "“I’m good.” She smiles and lets the quiet return.", 4f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q6T2, EmotionState.CalmConfidence, "*He relaxes into his chair, comfortable enough not to perform.*", 8f, "Reasonable", true));
            reactions.Add(CreateReaction(q6T2, EmotionState.CalmConfidence, ConnectionTier.Tier2_Maybe, "She looks completely at ease, comfortable enough that neither of them needs to perform.", 8f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q6T2, EmotionState.CalmAttraction, "*He catches her eye for a second and smiles before going back to his food.*", 12f, "HighFit", true));
            reactions.Add(CreateReaction(q6T2, EmotionState.CalmAttraction, ConnectionTier.Tier2_Maybe, "She catches his eye over the table and smiles, quietly enjoying the calm.", 12f, "warm_interest"));
            answers.Add(CreateAnswer(q6T2, EmotionState.AnxietyConfidence, "*He almost starts a conversation, stops himself, and lets the quiet be quiet.*", 2f, "Reasonable", true));
            reactions.Add(CreateReaction(q6T2, EmotionState.AnxietyConfidence, ConnectionTier.Tier2_Maybe, "She notices the forced composure and gives him a sympathetic glance. “Relax.”", 2f, "relieved_sigh"));
            answers.Add(CreateAnswer(q6T2, EmotionState.AnxietyAttraction, "*He looks at her with a small smile, then looks down again, visibly trying not to overthink the moment.*", 2f, "Reasonable", true));
            reactions.Add(CreateReaction(q6T2, EmotionState.AnxietyAttraction, ConnectionTier.Tier2_Maybe, "She smiles gently. “We don’t have to talk every second.”", 2f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q6T2, EmotionState.ConfidenceAttraction, "*He relaxes, gives her a small smile, and stays in the quiet with her.*", 2f, "Reasonable", true));
            reactions.Add(CreateReaction(q6T2, EmotionState.ConfidenceAttraction, ConnectionTier.Tier2_Maybe, "She smiles at him and keeps eating. “I like this, actually.”", 2f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q6T2, EmotionState.FrozenBlank, "*He sits rigid, watching her eat, unable to relax into the quiet.*", 8f, "Reasonable", true));
            reactions.Add(CreateReaction(q6T2, EmotionState.FrozenBlank, ConnectionTier.Tier2_Maybe, "She glances up, sensing his stiffness, and offers a quiet, questioning smile.", 8f, "neutral_acknowledge"));

            // --- Slot 6 Tier 1: Type 7 – She notices something, direct ---
            var q6T1 = ScriptableObject.CreateInstance<QuestionData>();
            q6T1.slotIndex = 6;
            q6T1.connectionTier = ConnectionTier.Tier1_Awkward;
            q6T1.scenarioType = "Type 7 – She notices something, direct";
            q6T1.questionText = "\"Is this going okay for you? I genuinely can't tell.\"";
            q6T1.responseTimeWindow = 6.0f;
            q6T1.socialEventProfile = new SocialEventProfile
            {
                requiresPlayerInitiation = false,
                primaryNodeOverride = InternalNodeType.Brain,
                effects = new List<TargetedEffect>
                {
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Brain,
                        effectType = SocialEffectType.StressEvent,
                        stressEventSubtype = StressEventSubtype.Overthinking,
                        magnitude = 45.0f
                    },
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Voice,
                        effectType = SocialEffectType.StressEvent,
                        stressEventSubtype = StressEventSubtype.AwkwardSilence,
                        magnitude = 45.0f
                    },
                }
            };
            questions.Add(q6T1);

            answers.Add(CreateAnswer(q6T1, EmotionState.Calm, "“Yeah. I’m enjoying myself. I’m probably just quieter than you expected.”", -6f, "Poor", false));
            reactions.Add(CreateReaction(q6T1, EmotionState.Calm, ConnectionTier.Tier1_Awkward, "“Okay.” She nods. “I can work with that.” The uncertainty eases slightly.", -6f, "discomfort"));
            answers.Add(CreateAnswer(q6T1, EmotionState.Anxiety, "“It is. I mean—I think it is. I’m sorry if I’m making it hard to tell.”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q6T1, EmotionState.Anxiety, ConnectionTier.Tier1_Awkward, "“Hey.” She softens her voice. “You don’t have to apologize for being nervous.”", 4f, "sympathetic_concern"));
            answers.Add(CreateAnswer(q6T1, EmotionState.Confidence, "“Yeah. I’m having a good time.”", -14f, "ActivelyWrong", false));
            reactions.Add(CreateReaction(q6T1, EmotionState.Confidence, ConnectionTier.Tier1_Awkward, "“I believe you.” She smiles politely, but wants more reassurance than confidence.", -14f, "shut_down"));
            answers.Add(CreateAnswer(q6T1, EmotionState.Attraction, "“Yeah. I like being here with you.”", 0f, "Reasonable", false));
            reactions.Add(CreateReaction(q6T1, EmotionState.Attraction, ConnectionTier.Tier1_Awkward, "“Okay.” Her smile warms. “That’s actually nice to hear.”", 0f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q6T1, EmotionState.CalmAnxiety, "“Yeah. I’m nervous, but I’m enjoying this. Both are true.”", 16f, "HighFit", false));
            reactions.Add(CreateReaction(q6T1, EmotionState.CalmAnxiety, ConnectionTier.Tier1_Awkward, "She nods. “Alright. I can understand that.”", 16f, "second_date"));
            answers.Add(CreateAnswer(q6T1, EmotionState.CalmConfidence, "“It is. I’m comfortable with you. I just don’t talk every second.”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q6T1, EmotionState.CalmConfidence, ConnectionTier.Tier1_Awkward, "Her face relaxes. “Good. Because I’m having a good time too.”", 4f, "relieved_sigh"));
            answers.Add(CreateAnswer(q6T1, EmotionState.CalmAttraction, "“Yeah. I’ve actually been having a really good time with you.”", -4f, "Poor", false));
            reactions.Add(CreateReaction(q6T1, EmotionState.CalmAttraction, ConnectionTier.Tier1_Awkward, "“Okay.” She smiles. “I was starting to wonder.”", -4f, "discomfort"));
            answers.Add(CreateAnswer(q6T1, EmotionState.AnxietyConfidence, "“Of course it’s going okay.” *Beat.* “I’m just... not exactly acting relaxed.”", 6f, "Reasonable", false));
            reactions.Add(CreateReaction(q6T1, EmotionState.AnxietyConfidence, ConnectionTier.Tier1_Awkward, "“I know.” She gives a small laugh. “You don’t have to convince me.”", 6f, "nervous_laugh"));
            answers.Add(CreateAnswer(q6T1, EmotionState.AnxietyAttraction, "“It is. I swear. I’m just nervous because I really do like you.”", 8f, "Reasonable", false));
            reactions.Add(CreateReaction(q6T1, EmotionState.AnxietyAttraction, ConnectionTier.Tier1_Awkward, "“You’re okay.” She smiles, but now she is managing both their anxiety instead of enjoying the date.", 8f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q6T1, EmotionState.ConfidenceAttraction, "“Yeah. Pretty sure we’re doing alright.” *He smiles at her.*", -10f, "ActivelyWrong", false));
            reactions.Add(CreateReaction(q6T1, EmotionState.ConfidenceAttraction, ConnectionTier.Tier1_Awkward, "“Alright.” She smiles. “Then give me something to work with.”", -10f, "discomfort"));
            answers.Add(CreateAnswer(q6T1, EmotionState.FrozenBlank, "“I... don’t know.”", -8f, "Poor", false));
            reactions.Add(CreateReaction(q6T1, EmotionState.FrozenBlank, ConnectionTier.Tier1_Awkward, "She looks away, disappointment setting in. “Yeah. That’s kind of what I thought.”", -8f, "sad_disappointment"));

            return (questions, answers, reactions);
        }


        private static (List<QuestionData>, List<AnswerData>, List<ReactionData>) BuildSlot7()
        {
            var questions = new List<QuestionData>();
            var answers = new List<AnswerData>();
            var reactions = new List<ReactionData>();

            // --- Slot 7 Tier 4: Type 10 – Intimate shift, mostly wordless ---
            var q7T4 = ScriptableObject.CreateInstance<QuestionData>();
            q7T4.slotIndex = 7;
            q7T4.connectionTier = ConnectionTier.Tier4_SecondDate;
            q7T4.scenarioType = "Type 10 – Intimate shift, mostly wordless";
            q7T4.questionText = "*She leans in slightly, voice drops.* \"Be honest — is this going as well for you as I think it is?\"";
            q7T4.responseTimeWindow = 6.0f;
            q7T4.socialEventProfile = new SocialEventProfile
            {
                requiresPlayerInitiation = false,
                primaryNodeOverride = InternalNodeType.Heart,
                effects = new List<TargetedEffect>
                {
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Heart,
                        effectType = SocialEffectType.DirectEmotionPush,
                        pushTargetEmotion = CoreEmotion.Attraction,
                        magnitude = 35.0f
                    },
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Voice,
                        effectType = SocialEffectType.DirectEmotionPush,
                        pushTargetEmotion = CoreEmotion.Attraction,
                        magnitude = 35.0f
                    },
                }
            };
            questions.Add(q7T4);

            answers.Add(CreateAnswer(q7T4, EmotionState.Calm, "“Yeah. I think we’re having a good night.”", -2f, "Poor", false));
            reactions.Add(CreateReaction(q7T4, EmotionState.Calm, ConnectionTier.Tier4_SecondDate, "“Good.” She smiles, satisfied by the simple honesty.", -2f, "discomfort"));
            answers.Add(CreateAnswer(q7T4, EmotionState.Anxiety, "“I hope so. I mean, I think so. I’m not exactly objective right now.”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q7T4, EmotionState.Anxiety, ConnectionTier.Tier4_SecondDate, "“Hey, it’s okay.” She eases back slightly, giving him room after hearing how much pressure he feels.", 4f, "relieved_sigh"));
            answers.Add(CreateAnswer(q7T4, EmotionState.Confidence, "“Pretty much. I was wondering when you’d say it.”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q7T4, EmotionState.Confidence, ConnectionTier.Tier4_SecondDate, "“I had a feeling you’d say that.” She grins, enjoying the certainty.", 4f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q7T4, EmotionState.Attraction, "*He smiles.* “Yeah. Very much.”", 18f, "HighFit", false));
            reactions.Add(CreateReaction(q7T4, EmotionState.Attraction, ConnectionTier.Tier4_SecondDate, "“Good.” She holds his gaze, clearly pleased.", 18f, "second_date"));
            answers.Add(CreateAnswer(q7T4, EmotionState.CalmAnxiety, "“I think so. I’m nervous, but the good kind.”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q7T4, EmotionState.CalmAnxiety, ConnectionTier.Tier4_SecondDate, "She smiles warmly. “Good. That’s kind of what I was hoping.”", 4f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q7T4, EmotionState.CalmConfidence, "“Yeah. I’d say we’re doing pretty well.”", 8f, "Reasonable", false));
            reactions.Add(CreateReaction(q7T4, EmotionState.CalmConfidence, ConnectionTier.Tier4_SecondDate, "“I’m glad.” She leans in again, more comfortable now.", 8f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q7T4, EmotionState.CalmAttraction, "“Yeah. I’ve been feeling that too.”", 16f, "HighFit", false));
            reactions.Add(CreateReaction(q7T4, EmotionState.CalmAttraction, ConnectionTier.Tier4_SecondDate, "Her smile turns shy for a second. “Yeah?” She stays close.", 16f, "second_date"));
            answers.Add(CreateAnswer(q7T4, EmotionState.AnxietyConfidence, "“Obviously.” *He grins, then catches himself.* “I mean... yeah, I think so.”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q7T4, EmotionState.AnxietyConfidence, ConnectionTier.Tier4_SecondDate, "She gives a small laugh, but pulls back a little. “You don’t have to make this a performance.”", 4f, "nervous_laugh"));
            answers.Add(CreateAnswer(q7T4, EmotionState.AnxietyAttraction, "“I really hope so. I’ve been trying not to get too excited about you.”", 12f, "HighFit", false));
            reactions.Add(CreateReaction(q7T4, EmotionState.AnxietyAttraction, ConnectionTier.Tier4_SecondDate, "“Okay.” Her smile softens. “You don’t have to be perfect about it.”", 12f, "warm_interest"));
            answers.Add(CreateAnswer(q7T4, EmotionState.ConfidenceAttraction, "“Yeah. I was hoping you’d ask.”", 18f, "HighFit", false));
            reactions.Add(CreateReaction(q7T4, EmotionState.ConfidenceAttraction, ConnectionTier.Tier4_SecondDate, "“Good.” She smiles like she already knew. “I was hoping I wasn’t imagining it.”", 18f, "second_date"));
            answers.Add(CreateAnswer(q7T4, EmotionState.FrozenBlank, "*He flushes, leaning back slightly, breath catching in his throat.*", 8f, "Reasonable", true));
            reactions.Add(CreateReaction(q7T4, EmotionState.FrozenBlank, ConnectionTier.Tier4_SecondDate, "She smiles knowingly, amusement dancing in her eyes. “I’ll take that as a yes.”", 8f, "neutral_acknowledge"));

            // --- Slot 7 Tier 3: Type 8 – Teasing ---
            var q7T3 = ScriptableObject.CreateInstance<QuestionData>();
            q7T3.slotIndex = 7;
            q7T3.connectionTier = ConnectionTier.Tier3_Strong;
            q7T3.scenarioType = "Type 8 – Teasing";
            q7T3.questionText = "\"If this were a movie, what's the twist? What am I gonna find out about you later?\"";
            q7T3.responseTimeWindow = 6.0f;
            q7T3.socialEventProfile = new SocialEventProfile
            {
                requiresPlayerInitiation = false,
                primaryNodeOverride = InternalNodeType.Voice,
                effects = new List<TargetedEffect>
                {
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Voice,
                        effectType = SocialEffectType.DirectEmotionPush,
                        pushTargetEmotion = CoreEmotion.Confidence,
                        magnitude = 25.0f
                    },
                }
            };
            questions.Add(q7T3);

            answers.Add(CreateAnswer(q7T3, EmotionState.Calm, "“That I’m much less interesting than I’ve made myself sound.”", -4f, "Poor", false));
            reactions.Add(CreateReaction(q7T3, EmotionState.Calm, ConnectionTier.Tier3_Strong, "“Exactly.” She smiles, happy he played along without forcing it.", -4f, "discomfort"));
            answers.Add(CreateAnswer(q7T3, EmotionState.Anxiety, "“Oh, God. Probably that I overthink everything.”", -2f, "Poor", false));
            reactions.Add(CreateReaction(q7T3, EmotionState.Anxiety, ConnectionTier.Tier3_Strong, "“You’re okay.” She laughs. “There’s no trick answer here.”", -2f, "discomfort"));
            answers.Add(CreateAnswer(q7T3, EmotionState.Confidence, "“That I’m somehow even more charming than this.”", 16f, "HighFit", false));
            reactions.Add(CreateReaction(q7T3, EmotionState.Confidence, ConnectionTier.Tier3_Strong, "“Oh, now that’s a good answer.” She grins and clearly wants to keep teasing.", 16f, "flirty_wink"));
            answers.Add(CreateAnswer(q7T3, EmotionState.Attraction, "“That I’ve been trying very hard not to flirt with you too much.”", 6f, "Reasonable", false));
            reactions.Add(CreateReaction(q7T3, EmotionState.Attraction, ConnectionTier.Tier3_Strong, "“Maybe there is.” She smiles, enjoying how interested he sounds.", 6f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q7T3, EmotionState.CalmAnxiety, "“Probably that I’m calmer than I look right now.”", 0f, "Reasonable", false));
            reactions.Add(CreateReaction(q7T3, EmotionState.CalmAnxiety, ConnectionTier.Tier3_Strong, "“Fair.” She laughs. “I’ll give you one chance to recover.”", 0f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q7T3, EmotionState.CalmConfidence, "“That I’m actually pretty competitive, but I try to hide it.”", 8f, "Reasonable", false));
            reactions.Add(CreateReaction(q7T3, EmotionState.CalmConfidence, ConnectionTier.Tier3_Strong, "“See? This is why I asked.” She looks genuinely entertained.", 8f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q7T3, EmotionState.CalmAttraction, "“That I’ve been paying more attention to you than to the conversation.”", 6f, "Reasonable", false));
            reactions.Add(CreateReaction(q7T3, EmotionState.CalmAttraction, ConnectionTier.Tier3_Strong, "“You’re curious now.” She grins, pleased with the attention.", 6f, "thinking_chintap"));
            answers.Add(CreateAnswer(q7T3, EmotionState.AnxietyConfidence, "“That I’m secretly very mysterious.” *Beat.* “Or just anxious. One of those.”", 12f, "HighFit", false));
            reactions.Add(CreateReaction(q7T3, EmotionState.AnxietyConfidence, ConnectionTier.Tier3_Strong, "She laughs. “Okay, okay. You don’t have to win the question.”", 12f, "flirty_wink"));
            answers.Add(CreateAnswer(q7T3, EmotionState.AnxietyAttraction, "“That I’ve already thought about what I’d say if you asked me this.”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q7T3, EmotionState.AnxietyAttraction, ConnectionTier.Tier3_Strong, "“That was supposed to be fun.” She smiles, trying to pull him back into the lighter mood.", 4f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q7T3, EmotionState.ConfidenceAttraction, "“That I’m trouble once I’m comfortable.” *He smiles.*", 18f, "HighFit", false));
            reactions.Add(CreateReaction(q7T3, EmotionState.ConfidenceAttraction, ConnectionTier.Tier3_Strong, "“Oh, I like that answer.” She gives him a playful look.", 18f, "second_date"));
            answers.Add(CreateAnswer(q7T3, EmotionState.FrozenBlank, "*He tries to come up with something witty, blanks completely, and gives a defeated grin.*", -4f, "Poor", true));
            reactions.Add(CreateReaction(q7T3, EmotionState.FrozenBlank, ConnectionTier.Tier3_Strong, "She laughs. “Top secret, huh? Fine, keep your mystery.”", -4f, "discomfort"));

            // --- Slot 7 Tier 2: Type 5 – Distracted, wordless ---
            var q7T2 = ScriptableObject.CreateInstance<QuestionData>();
            q7T2.slotIndex = 7;
            q7T2.connectionTier = ConnectionTier.Tier2_Maybe;
            q7T2.scenarioType = "Type 5 – Distracted, wordless";
            q7T2.questionText = "*Her attention drifts to something across the room for a moment before coming back.*";
            q7T2.responseTimeWindow = 6.0f;
            q7T2.socialEventProfile = new SocialEventProfile
            {
                requiresPlayerInitiation = false,
                primaryNodeOverride = InternalNodeType.Body,
                effects = new List<TargetedEffect>
                {
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Body,
                        effectType = SocialEffectType.StressEvent,
                        stressEventSubtype = StressEventSubtype.SweatSurge,
                        magnitude = 20.0f
                    },
                }
            };
            questions.Add(q7T2);

            answers.Add(CreateAnswer(q7T2, EmotionState.Calm, "*He notices, but keeps the conversation easy.* “Everything catch your eye over there?”", -6f, "Poor", false));
            reactions.Add(CreateReaction(q7T2, EmotionState.Calm, ConnectionTier.Tier2_Maybe, "She notices him wait instead of chasing her attention and comes back on her own. “Sorry.”", -6f, "discomfort"));
            answers.Add(CreateAnswer(q7T2, EmotionState.Anxiety, "“Sorry, was I talking about something boring?”", -8f, "Poor", false));
            reactions.Add(CreateReaction(q7T2, EmotionState.Anxiety, ConnectionTier.Tier2_Maybe, "“Sorry, what were you saying?” She tries to recover, but the repeated checking makes her feel guilty.", -8f, "discomfort"));
            answers.Add(CreateAnswer(q7T2, EmotionState.Confidence, "“You can tell me if I’ve lost you.”", 14f, "HighFit", false));
            reactions.Add(CreateReaction(q7T2, EmotionState.Confidence, ConnectionTier.Tier2_Maybe, "“You’re fine.” She looks back at him, appreciating that he did not make the moment awkward.", 14f, "warm_interest"));
            answers.Add(CreateAnswer(q7T2, EmotionState.Attraction, "*He waits until she looks back, smiling.* “You back?”", 2f, "Reasonable", false));
            reactions.Add(CreateReaction(q7T2, EmotionState.Attraction, ConnectionTier.Tier2_Maybe, "She catches his smile and returns it, but the quiet reaction feels slightly more intimate than she expected.", 2f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q7T2, EmotionState.CalmAnxiety, "“You okay? No worries if you got distracted.”", 0f, "Reasonable", false));
            reactions.Add(CreateReaction(q7T2, EmotionState.CalmAnxiety, ConnectionTier.Tier2_Maybe, "“Sorry, I zoned out.” She smiles apologetically.", 0f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q7T2, EmotionState.CalmConfidence, "“What were you looking at?”", 8f, "Reasonable", false));
            reactions.Add(CreateReaction(q7T2, EmotionState.CalmConfidence, ConnectionTier.Tier2_Maybe, "She comes back to him naturally. “I’m sorry. I was listening.” The ease returns quickly.", 8f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q7T2, EmotionState.CalmAttraction, "“Something interesting?” *He smiles.*", 2f, "Reasonable", false));
            reactions.Add(CreateReaction(q7T2, EmotionState.CalmAttraction, ConnectionTier.Tier2_Maybe, "She notices his patience and smiles more warmly when she turns back.", 2f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q7T2, EmotionState.AnxietyConfidence, "“I know that look.” *He laughs nervously.* “You’ve left the conversation.”", 8f, "Reasonable", false));
            reactions.Add(CreateReaction(q7T2, EmotionState.AnxietyConfidence, ConnectionTier.Tier2_Maybe, "“I’m listening.” She says it almost defensively, which makes the moment less easy.", 8f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q7T2, EmotionState.AnxietyAttraction, "“Sorry—did I lose you?”", -4f, "Poor", false));
            reactions.Add(CreateReaction(q7T2, EmotionState.AnxietyAttraction, ConnectionTier.Tier2_Maybe, "“Sorry.” She gives him a sympathetic smile. “I just lost my train of thought.”", -4f, "discomfort"));
            answers.Add(CreateAnswer(q7T2, EmotionState.ConfidenceAttraction, "“Hey, come back. I was getting to the good part.”", 10f, "HighFit", false));
            reactions.Add(CreateReaction(q7T2, EmotionState.ConfidenceAttraction, ConnectionTier.Tier2_Maybe, "“You noticed.” She smiles. “I’m back.”", 10f, "warm_interest"));
            answers.Add(CreateAnswer(q7T2, EmotionState.FrozenBlank, "*He watches her look away, withdrawing into his own thoughts without speaking.*", -8f, "Poor", true));
            reactions.Add(CreateReaction(q7T2, EmotionState.FrozenBlank, ConnectionTier.Tier2_Maybe, "She turns back, but noticing his retreat, she doesn't try to restart the thread.", -8f, "discomfort"));

            // --- Slot 7 Tier 1: Type 5 – Restless, wordless ---
            var q7T1 = ScriptableObject.CreateInstance<QuestionData>();
            q7T1.slotIndex = 7;
            q7T1.connectionTier = ConnectionTier.Tier1_Awkward;
            q7T1.scenarioType = "Type 5 – Restless, wordless";
            q7T1.questionText = "*She's checking the time again, more obviously this time. Doesn't try to hide it.*";
            q7T1.responseTimeWindow = 6.0f;
            q7T1.socialEventProfile = new SocialEventProfile
            {
                requiresPlayerInitiation = false,
                primaryNodeOverride = InternalNodeType.Body,
                effects = new List<TargetedEffect>
                {
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Body,
                        effectType = SocialEffectType.StressEvent,
                        stressEventSubtype = StressEventSubtype.SweatSurge,
                        magnitude = 45.0f
                    },
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Voice,
                        effectType = SocialEffectType.StressEvent,
                        stressEventSubtype = StressEventSubtype.AwkwardSilence,
                        magnitude = 45.0f
                    },
                }
            };
            questions.Add(q7T1);

            answers.Add(CreateAnswer(q7T1, EmotionState.Calm, "“We alright?”", -10f, "ActivelyWrong", false));
            reactions.Add(CreateReaction(q7T1, EmotionState.Calm, ConnectionTier.Tier1_Awkward, "“I’m okay.” She puts the phone down again. “I just needed to know what time it was.”", -10f, "discomfort"));
            answers.Add(CreateAnswer(q7T1, EmotionState.Anxiety, "“Do you need to go? You can just tell me.”", -6f, "Poor", false));
            reactions.Add(CreateReaction(q7T1, EmotionState.Anxiety, ConnectionTier.Tier1_Awkward, "“No, it’s okay.” She reassures him, but the question makes her more conscious of leaving.", -6f, "sympathetic_concern"));
            answers.Add(CreateAnswer(q7T1, EmotionState.Confidence, "“Okay, that’s the second time. I’m noticing.”", 14f, "HighFit", false));
            reactions.Add(CreateReaction(q7T1, EmotionState.Confidence, ConnectionTier.Tier1_Awkward, "“Yeah, I checked.” She answers honestly, though his tone makes it awkward.", 14f, "warm_interest"));
            answers.Add(CreateAnswer(q7T1, EmotionState.Attraction, "“Please tell me you’re checking the time because you forgot what time it was, not because you want out.”", -4f, "Poor", false));
            reactions.Add(CreateReaction(q7T1, EmotionState.Attraction, ConnectionTier.Tier1_Awkward, "She shakes her head. “I’m not laughing because I want to leave.” The defensiveness shows he hit a nerve.", -4f, "discomfort"));
            answers.Add(CreateAnswer(q7T1, EmotionState.CalmAnxiety, "“If you need to leave, it’s okay. I’d rather you tell me than sit here feeling stuck.”", 2f, "Reasonable", false));
            reactions.Add(CreateReaction(q7T1, EmotionState.CalmAnxiety, ConnectionTier.Tier1_Awkward, "“Okay.” She smiles. “I appreciate you asking instead of guessing.”", 2f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q7T1, EmotionState.CalmConfidence, "“You good on time?”", 6f, "Reasonable", false));
            reactions.Add(CreateReaction(q7T1, EmotionState.CalmConfidence, ConnectionTier.Tier1_Awkward, "“I’m fine for time.” She puts the phone away and settles back.", 6f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q7T1, EmotionState.CalmAttraction, "“You alright? I’d like you to stay, but I don’t want you to feel trapped here.”", -6f, "Poor", false));
            reactions.Add(CreateReaction(q7T1, EmotionState.CalmAttraction, ConnectionTier.Tier1_Awkward, "“I’m okay.” She smiles, though the restlessness has not completely gone.", -6f, "discomfort"));
            answers.Add(CreateAnswer(q7T1, EmotionState.AnxietyConfidence, "“Okay, I get it.” *Beat.* “I’m not going to pretend I didn’t notice.”", 8f, "Reasonable", false));
            reactions.Add(CreateReaction(q7T1, EmotionState.AnxietyConfidence, ConnectionTier.Tier1_Awkward, "“I know.” She laughs softly. “It was just a time check.”", 8f, "nervous_laugh"));
            answers.Add(CreateAnswer(q7T1, EmotionState.AnxietyAttraction, "“Is this not going well? You can be honest with me.”", -6f, "Poor", false));
            reactions.Add(CreateReaction(q7T1, EmotionState.AnxietyAttraction, ConnectionTier.Tier1_Awkward, "“No, I’m not bored.” She says it quickly, frustrated that the idea is now part of the conversation.", -6f, "discomfort"));
            answers.Add(CreateAnswer(q7T1, EmotionState.ConfidenceAttraction, "“Twice now? I was hoping I was keeping you entertained.”", -8f, "Poor", false));
            reactions.Add(CreateReaction(q7T1, EmotionState.ConfidenceAttraction, ConnectionTier.Tier1_Awkward, "“You’re optimistic.” She smiles, but the romantic line is a little much when she is already checking out the clock.", -8f, "discomfort"));
            answers.Add(CreateAnswer(q7T1, EmotionState.FrozenBlank, "*He follows her glance to the time and goes quiet.*", -10f, "ActivelyWrong", true));
            reactions.Add(CreateReaction(q7T1, EmotionState.FrozenBlank, ConnectionTier.Tier1_Awkward, "She checks her watch one more time and folds her napkin deliberately.", -10f, "discomfort"));

            return (questions, answers, reactions);
        }


        private static (List<QuestionData>, List<AnswerData>, List<ReactionData>) BuildSlot8()
        {
            var questions = new List<QuestionData>();
            var answers = new List<AnswerData>();
            var reactions = new List<ReactionData>();

            // --- Slot 8 Tier 4: Type 7 – Warm variant ---
            var q8T4 = ScriptableObject.CreateInstance<QuestionData>();
            q8T4.slotIndex = 8;
            q8T4.connectionTier = ConnectionTier.Tier4_SecondDate;
            q8T4.scenarioType = "Type 7 – Warm variant";
            q8T4.questionText = "\"You get kind of adorable when you're nervous, you know that?\"";
            q8T4.responseTimeWindow = 6.0f;
            q8T4.socialEventProfile = new SocialEventProfile
            {
                requiresPlayerInitiation = false,
                primaryNodeOverride = InternalNodeType.Heart,
                effects = new List<TargetedEffect>
                {
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Heart,
                        effectType = SocialEffectType.DirectEmotionPush,
                        pushTargetEmotion = CoreEmotion.Attraction,
                        magnitude = 35.0f
                    },
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Body,
                        effectType = SocialEffectType.DirectEmotionPush,
                        pushTargetEmotion = CoreEmotion.Attraction,
                        magnitude = 35.0f
                    },
                }
            };
            questions.Add(q8T4);

            answers.Add(CreateAnswer(q8T4, EmotionState.Calm, "“I’ll take adorable.”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q8T4, EmotionState.Calm, ConnectionTier.Tier4_SecondDate, "“I can work with that.” She smiles, clearly pleased that the nerves are not derailing him.", 4f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q8T4, EmotionState.Anxiety, "“Oh. Great. So it really is that obvious.”", 8f, "Reasonable", false));
            reactions.Add(CreateReaction(q8T4, EmotionState.Anxiety, ConnectionTier.Tier4_SecondDate, "“Hey, it’s okay.” She softens immediately, trying not to make him feel exposed.", 8f, "sympathetic_concern"));
            answers.Add(CreateAnswer(q8T4, EmotionState.Confidence, "“Good. I was going for charming.”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q8T4, EmotionState.Confidence, ConnectionTier.Tier4_SecondDate, "“You’re hiding it well.” She grins, amused by the contradiction.", 4f, "nervous_laugh"));
            answers.Add(CreateAnswer(q8T4, EmotionState.Attraction, "*He smiles, a little embarrassed.* “You think so?”", 18f, "HighFit", false));
            reactions.Add(CreateReaction(q8T4, EmotionState.Attraction, ConnectionTier.Tier4_SecondDate, "“Good.” She smiles. “I was hoping you’d hear it as a compliment.”", 18f, "second_date"));
            answers.Add(CreateAnswer(q8T4, EmotionState.CalmAnxiety, "“I’m glad it’s at least entertaining.”", 6f, "Reasonable", false));
            reactions.Add(CreateReaction(q8T4, EmotionState.CalmAnxiety, ConnectionTier.Tier4_SecondDate, "“That’s okay.” She smiles gently, but the compliment does not quite make the nerves disappear.", 6f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q8T4, EmotionState.CalmConfidence, "“I can live with that.”", 8f, "Reasonable", false));
            reactions.Add(CreateReaction(q8T4, EmotionState.CalmConfidence, ConnectionTier.Tier4_SecondDate, "“Exactly.” She smiles warmly. “It’s kind of cute when you’re not fighting it.”", 8f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q8T4, EmotionState.CalmAttraction, "*He smiles warmly.* “I kind of like hearing that from you.”", 18f, "HighFit", false));
            reactions.Add(CreateReaction(q8T4, EmotionState.CalmAttraction, ConnectionTier.Tier4_SecondDate, "She laughs softly. “See? You’re not nearly as hard to read as you think.”", 18f, "second_date"));
            answers.Add(CreateAnswer(q8T4, EmotionState.AnxietyConfidence, "“Adorable?” *He laughs.* “I was going for intimidating.”", 6f, "Reasonable", false));
            reactions.Add(CreateReaction(q8T4, EmotionState.AnxietyConfidence, ConnectionTier.Tier4_SecondDate, "“I didn’t mean it as a challenge.” She gives him a careful smile after the defensive response.", 6f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q8T4, EmotionState.AnxietyAttraction, "“You actually think it’s cute? Because I’m trying very hard not to completely embarrass myself here.”", 12f, "HighFit", false));
            reactions.Add(CreateReaction(q8T4, EmotionState.AnxietyAttraction, ConnectionTier.Tier4_SecondDate, "“Hey.” She smiles. “You don’t have to hide it from me.”", 12f, "warm_interest"));
            answers.Add(CreateAnswer(q8T4, EmotionState.ConfidenceAttraction, "“Careful. Keep calling me adorable and I’m going to start believing you.”", 16f, "HighFit", false));
            reactions.Add(CreateReaction(q8T4, EmotionState.ConfidenceAttraction, ConnectionTier.Tier4_SecondDate, "“See?” She grins. “That confidence suits you better.”", 16f, "second_date"));
            answers.Add(CreateAnswer(q8T4, EmotionState.FrozenBlank, "*He shakes his head with a bashful, speechless smile.*", 8f, "Reasonable", true));
            reactions.Add(CreateReaction(q8T4, EmotionState.FrozenBlank, ConnectionTier.Tier4_SecondDate, "She beams, finding it endearing. “See? Proving my point right now.”", 8f, "neutral_acknowledge"));

            // --- Slot 8 Tier 3: Type 7 – Hopeful variant ---
            var q8T3 = ScriptableObject.CreateInstance<QuestionData>();
            q8T3.slotIndex = 8;
            q8T3.connectionTier = ConnectionTier.Tier3_Strong;
            q8T3.scenarioType = "Type 7 – Hopeful variant";
            q8T3.questionText = "\"You've been a little nervous all night — good nervous, I think?\"";
            q8T3.responseTimeWindow = 6.0f;
            q8T3.socialEventProfile = new SocialEventProfile
            {
                requiresPlayerInitiation = false,
                primaryNodeOverride = InternalNodeType.Heart,
                effects = new List<TargetedEffect>
                {
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Heart,
                        effectType = SocialEffectType.StressEvent,
                        stressEventSubtype = StressEventSubtype.HeartFlutter,
                        magnitude = 25.0f
                    },
                }
            };
            questions.Add(q8T3);

            answers.Add(CreateAnswer(q8T3, EmotionState.Calm, "“Yeah. Good nervous.”", 6f, "Reasonable", false));
            reactions.Add(CreateReaction(q8T3, EmotionState.Calm, ConnectionTier.Tier3_Strong, "“Good.” She smiles. “I was hoping it was that kind.”", 6f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q8T3, EmotionState.Anxiety, "“Yeah... I think so. I hope so.”", 6f, "Reasonable", false));
            reactions.Add(CreateReaction(q8T3, EmotionState.Anxiety, ConnectionTier.Tier3_Strong, "“You’re okay.” Her tone softens when she hears how worried he is about being read correctly.", 6f, "sympathetic_concern"));
            answers.Add(CreateAnswer(q8T3, EmotionState.Confidence, "“Definitely good nervous.”", 8f, "Reasonable", false));
            reactions.Add(CreateReaction(q8T3, EmotionState.Confidence, ConnectionTier.Tier3_Strong, "“Mostly?” She smiles, but the certainty feels a little over-managed.", 8f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q8T3, EmotionState.Attraction, "“Yeah. That’s what happens when I really like someone.”", 14f, "HighFit", false));
            reactions.Add(CreateReaction(q8T3, EmotionState.Attraction, ConnectionTier.Tier3_Strong, "“Yeah.” She smiles. “I was getting that feeling.”", 14f, "warm_interest"));
            answers.Add(CreateAnswer(q8T3, EmotionState.CalmAnxiety, "“Yeah. It’s the good kind. I’m nervous because I care.”", 8f, "Reasonable", false));
            reactions.Add(CreateReaction(q8T3, EmotionState.CalmAnxiety, ConnectionTier.Tier3_Strong, "“That’s kind of where I’m at too.” She looks relieved by the honesty.", 8f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q8T3, EmotionState.CalmConfidence, "“Good nervous. I know what I’m doing; I’m just enjoying it.”", 10f, "HighFit", false));
            reactions.Add(CreateReaction(q8T3, EmotionState.CalmConfidence, ConnectionTier.Tier3_Strong, "“Good nervous.” She laughs. “You say that like you’re giving yourself a review.”", 10f, "nervous_laugh"));
            answers.Add(CreateAnswer(q8T3, EmotionState.CalmAttraction, "“Good nervous. I’ve been having a pretty good time.”", 16f, "HighFit", false));
            reactions.Add(CreateReaction(q8T3, EmotionState.CalmAttraction, ConnectionTier.Tier3_Strong, "“Good.” Her smile lingers. “I’ve been having a good time too.”", 16f, "second_date"));
            answers.Add(CreateAnswer(q8T3, EmotionState.AnxietyConfidence, "“Good nervous. Obviously.” *Beat.* “Mostly.”", 10f, "HighFit", false));
            reactions.Add(CreateReaction(q8T3, EmotionState.AnxietyConfidence, ConnectionTier.Tier3_Strong, "“Sure.” She smiles, but the forced certainty makes her less sure what he actually feels.", 10f, "warm_interest"));
            answers.Add(CreateAnswer(q8T3, EmotionState.AnxietyAttraction, "“Yeah. Good nervous. I just... really want this to go well.”", 10f, "HighFit", false));
            reactions.Add(CreateReaction(q8T3, EmotionState.AnxietyAttraction, ConnectionTier.Tier3_Strong, "“Yeah.” She nods. “I get it.” The sincerity helps, even if the nerves are obvious.", 10f, "warm_interest"));
            answers.Add(CreateAnswer(q8T3, EmotionState.ConfidenceAttraction, "“Very good nervous. I’m enjoying myself.”", 18f, "HighFit", false));
            reactions.Add(CreateReaction(q8T3, EmotionState.ConfidenceAttraction, ConnectionTier.Tier3_Strong, "“Very good nervous?” she teases. “Okay, I’ll take it.”", 18f, "flirty_wink"));
            answers.Add(CreateAnswer(q8T3, EmotionState.FrozenBlank, "*He rubs his forehead, smiling sheepishly, nodding without words.*", 4f, "Reasonable", true));
            reactions.Add(CreateReaction(q8T3, EmotionState.FrozenBlank, ConnectionTier.Tier3_Strong, "She smiles encouragingly. “Good. As long as you’re having fun.”", 4f, "neutral_acknowledge"));

            // --- Slot 8 Tier 2: Type 7 – Neutral variant ---
            var q8T2 = ScriptableObject.CreateInstance<QuestionData>();
            q8T2.slotIndex = 8;
            q8T2.connectionTier = ConnectionTier.Tier2_Maybe;
            q8T2.scenarioType = "Type 7 – Neutral variant";
            q8T2.questionText = "\"You seem nervous. Is that me, or just how you are?\"";
            q8T2.responseTimeWindow = 6.0f;
            q8T2.socialEventProfile = new SocialEventProfile
            {
                requiresPlayerInitiation = false,
                primaryNodeOverride = InternalNodeType.Brain,
                effects = new List<TargetedEffect>
                {
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Brain,
                        effectType = SocialEffectType.StressEvent,
                        stressEventSubtype = StressEventSubtype.Panic,
                        magnitude = 25.0f
                    },
                }
            };
            questions.Add(q8T2);

            answers.Add(CreateAnswer(q8T2, EmotionState.Calm, "“Mostly the situation. You’re making it better, not worse.”", -4f, "Poor", false));
            reactions.Add(CreateReaction(q8T2, EmotionState.Calm, ConnectionTier.Tier2_Maybe, "“Okay.” She smiles. “I was starting to worry I was making it harder.”", -4f, "discomfort"));
            answers.Add(CreateAnswer(q8T2, EmotionState.Anxiety, "“Probably the date. I get in my head sometimes.”", 2f, "Reasonable", false));
            reactions.Add(CreateReaction(q8T2, EmotionState.Anxiety, ConnectionTier.Tier2_Maybe, "“I get it.” She nods. “You don’t have to blame the date for everything.”", 2f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q8T2, EmotionState.Confidence, "“It’s you a little.” *He smiles.* “In a good way.”", 10f, "HighFit", false));
            reactions.Add(CreateReaction(q8T2, EmotionState.Confidence, ConnectionTier.Tier2_Maybe, "“A little.” She smiles, but waits for him to say more.", 10f, "warm_interest"));
            answers.Add(CreateAnswer(q8T2, EmotionState.Attraction, "“You, definitely.”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q8T2, EmotionState.Attraction, ConnectionTier.Tier2_Maybe, "She laughs. “Okay. I can handle that answer.”", 4f, "nervous_laugh"));
            answers.Add(CreateAnswer(q8T2, EmotionState.CalmAnxiety, "“A little of both. I was nervous before I got here.”", 12f, "HighFit", false));
            reactions.Add(CreateReaction(q8T2, EmotionState.CalmAnxiety, ConnectionTier.Tier2_Maybe, "“That makes sense.” She smiles gently. “At least it wasn’t all me.”", 12f, "warm_interest"));
            answers.Add(CreateAnswer(q8T2, EmotionState.CalmConfidence, "“The date. You’re actually pretty easy to be around.”", 14f, "HighFit", false));
            reactions.Add(CreateReaction(q8T2, EmotionState.CalmConfidence, ConnectionTier.Tier2_Maybe, "“Good.” She smiles. “I can live with being part of it.”", 14f, "warm_interest"));
            answers.Add(CreateAnswer(q8T2, EmotionState.CalmAttraction, "“You, a little. I like you, so that doesn’t help.”", 6f, "Reasonable", false));
            reactions.Add(CreateReaction(q8T2, EmotionState.CalmAttraction, ConnectionTier.Tier2_Maybe, "Her smile gets a little shy. “Okay... I’m definitely taking that as a compliment.”", 6f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q8T2, EmotionState.AnxietyConfidence, "“Not you.” *Beat.* “Okay, maybe a little you.”", 8f, "Reasonable", false));
            reactions.Add(CreateReaction(q8T2, EmotionState.AnxietyConfidence, ConnectionTier.Tier2_Maybe, "“Not me?” She raises an eyebrow. “You changed your answer pretty fast.” The playfulness becomes guarded.", 8f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q8T2, EmotionState.AnxietyAttraction, "“Definitely you. I was nervous before, but being here made it worse—in a good way.”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q8T2, EmotionState.AnxietyAttraction, ConnectionTier.Tier2_Maybe, "She laughs softly. “Okay, that’s a lot of honesty at once.”", 4f, "nervous_laugh"));
            answers.Add(CreateAnswer(q8T2, EmotionState.ConfidenceAttraction, "“You. I was fine until I saw you.”", 6f, "Reasonable", false));
            reactions.Add(CreateReaction(q8T2, EmotionState.ConfidenceAttraction, ConnectionTier.Tier2_Maybe, "“You were fine until you saw me?” She grins. “I’ll take that.”", 6f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q8T2, EmotionState.FrozenBlank, "*He freezes, eyes darting, unable to answer either way.*", -8f, "Poor", true));
            reactions.Add(CreateReaction(q8T2, EmotionState.FrozenBlank, ConnectionTier.Tier2_Maybe, "She sighs softly. “You really don’t have to walk on eggshells around me.”", -8f, "discomfort"));

            // --- Slot 8 Tier 1: Type 7 – Sharp variant ---
            var q8T1 = ScriptableObject.CreateInstance<QuestionData>();
            q8T1.slotIndex = 8;
            q8T1.connectionTier = ConnectionTier.Tier1_Awkward;
            q8T1.scenarioType = "Type 7 – Sharp variant";
            q8T1.questionText = "\"You've barely relaxed all night. Should I be worried this isn't landing?\"";
            q8T1.responseTimeWindow = 6.0f;
            q8T1.socialEventProfile = new SocialEventProfile
            {
                requiresPlayerInitiation = false,
                primaryNodeOverride = InternalNodeType.Brain,
                effects = new List<TargetedEffect>
                {
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Brain,
                        effectType = SocialEffectType.StressEvent,
                        stressEventSubtype = StressEventSubtype.Panic,
                        magnitude = 45.0f
                    },
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Brain,
                        effectType = SocialEffectType.StressEvent,
                        stressEventSubtype = StressEventSubtype.Overthinking,
                        magnitude = 45.0f
                    },
                }
            };
            questions.Add(q8T1);

            answers.Add(CreateAnswer(q8T1, EmotionState.Calm, "“No. I’m having a good time. I just take a while to settle in.”", -6f, "Poor", false));
            reactions.Add(CreateReaction(q8T1, EmotionState.Calm, ConnectionTier.Tier1_Awkward, "She nods. “Okay. I can understand taking a while to settle.”", -6f, "discomfort"));
            answers.Add(CreateAnswer(q8T1, EmotionState.Anxiety, "“No, it’s landing. I swear. I just... don’t know how to make that obvious.”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q8T1, EmotionState.Anxiety, ConnectionTier.Tier1_Awkward, "“I know.” She gives him a reassuring smile. “You don’t have to convince me.”", 4f, "sympathetic_concern"));
            answers.Add(CreateAnswer(q8T1, EmotionState.Confidence, "“No. If it wasn’t landing, you’d know.”", -14f, "ActivelyWrong", false));
            reactions.Add(CreateReaction(q8T1, EmotionState.Confidence, ConnectionTier.Tier1_Awkward, "“Alright.” She nods, but the certainty makes her wonder why he looks so uncomfortable.", -14f, "shut_down"));
            answers.Add(CreateAnswer(q8T1, EmotionState.Attraction, "“It’s landing. Very much.”", 0f, "Reasonable", false));
            reactions.Add(CreateReaction(q8T1, EmotionState.Attraction, ConnectionTier.Tier1_Awkward, "“Good.” She smiles. “Because I was starting to hope it was.”", 0f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q8T1, EmotionState.CalmAnxiety, "“No. I’m nervous, but that’s not because I want to be anywhere else.”", 16f, "HighFit", false));
            reactions.Add(CreateReaction(q8T1, EmotionState.CalmAnxiety, ConnectionTier.Tier1_Awkward, "“Okay.” She seems relieved to hear it is nerves, not disinterest.", 16f, "second_date"));
            answers.Add(CreateAnswer(q8T1, EmotionState.CalmConfidence, "“No. I’m comfortable. I just don’t look as relaxed as I feel.”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q8T1, EmotionState.CalmConfidence, ConnectionTier.Tier1_Awkward, "“That actually makes me feel better.” She smiles, finally relaxing into the date herself.", 4f, "relieved_sigh"));
            answers.Add(CreateAnswer(q8T1, EmotionState.CalmAttraction, "“It’s landing. I like being here with you.”", -4f, "Poor", false));
            reactions.Add(CreateReaction(q8T1, EmotionState.CalmAttraction, ConnectionTier.Tier1_Awkward, "“Good.” She smiles warmly. “I like being here with you too.”", -4f, "discomfort"));
            answers.Add(CreateAnswer(q8T1, EmotionState.AnxietyConfidence, "“No. Obviously not.” *Beat.* “Okay, maybe I could be doing a better job of showing it.”", 6f, "Reasonable", false));
            reactions.Add(CreateReaction(q8T1, EmotionState.AnxietyConfidence, ConnectionTier.Tier1_Awkward, "She gives a tiny laugh, but her expression says she still is not convinced. “Okay... if you say so.”", 6f, "nervous_laugh"));
            answers.Add(CreateAnswer(q8T1, EmotionState.AnxietyAttraction, "“It is. I promise. I’m just nervous because I really like you.”", 8f, "Reasonable", false));
            reactions.Add(CreateReaction(q8T1, EmotionState.AnxietyAttraction, ConnectionTier.Tier1_Awkward, "“I believe you.” She smiles. “You’re just making it hard for me to tell.”", 8f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q8T1, EmotionState.ConfidenceAttraction, "“It’s landing. I think you know that.”", -10f, "ActivelyWrong", false));
            reactions.Add(CreateReaction(q8T1, EmotionState.ConfidenceAttraction, ConnectionTier.Tier1_Awkward, "“I think I do.” She smiles knowingly, but does not press him further.", -10f, "discomfort"));
            answers.Add(CreateAnswer(q8T1, EmotionState.FrozenBlank, "“I... I don’t know.”", -8f, "Poor", false));
            reactions.Add(CreateReaction(q8T1, EmotionState.FrozenBlank, ConnectionTier.Tier1_Awkward, "She leans back, looking tired. “Right. That tells me what I needed to know.”", -8f, "discomfort"));

            return (questions, answers, reactions);
        }


        private static (List<QuestionData>, List<AnswerData>, List<ReactionData>) BuildSlot9()
        {
            var questions = new List<QuestionData>();
            var answers = new List<AnswerData>();
            var reactions = new List<ReactionData>();

            // --- Slot 9 Tier 4: Type 3 – Vulnerable admission ---
            var q9T4 = ScriptableObject.CreateInstance<QuestionData>();
            q9T4.slotIndex = 9;
            q9T4.connectionTier = ConnectionTier.Tier4_SecondDate;
            q9T4.scenarioType = "Type 3 – Vulnerable admission";
            q9T4.questionText = "\"Can I ask something I don't usually ask this early? What are you insecure about?\"";
            q9T4.responseTimeWindow = 6.0f;
            q9T4.socialEventProfile = new SocialEventProfile
            {
                requiresPlayerInitiation = false,
                primaryNodeOverride = InternalNodeType.Heart,
                effects = new List<TargetedEffect>
                {
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Heart,
                        effectType = SocialEffectType.StressEvent,
                        stressEventSubtype = StressEventSubtype.HeartFlutter,
                        magnitude = 35.0f
                    },
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Brain,
                        effectType = SocialEffectType.StressEvent,
                        stressEventSubtype = StressEventSubtype.Overthinking,
                        magnitude = 35.0f
                    },
                }
            };
            questions.Add(q9T4);

            answers.Add(CreateAnswer(q9T4, EmotionState.Calm, "“Probably disappointing people once they actually get to know me.”", 16f, "HighFit", false));
            reactions.Add(CreateReaction(q9T4, EmotionState.Calm, ConnectionTier.Tier4_SecondDate, "She nods thoughtfully. “Yeah. I think that’s probably one of mine too.”", 16f, "second_date"));
            answers.Add(CreateAnswer(q9T4, EmotionState.Anxiety, "“A lot, probably. I worry I’m going to say the wrong thing and make people regret getting close.”", 8f, "Reasonable", false));
            reactions.Add(CreateReaction(q9T4, EmotionState.Anxiety, ConnectionTier.Tier4_SecondDate, "Her expression softens. “You don’t have to give me every fear at once.” She keeps the moment safe.", 8f, "sympathetic_concern"));
            answers.Add(CreateAnswer(q9T4, EmotionState.Confidence, "“I don’t spend much time on insecurities. I’ve got things I’m still working on, obviously.”", -16f, "ActivelyWrong", false));
            reactions.Add(CreateReaction(q9T4, EmotionState.Confidence, ConnectionTier.Tier4_SecondDate, "“That’s fair.” She smiles, but she senses the answer stayed at arm’s length.", -16f, "shut_down"));
            answers.Add(CreateAnswer(q9T4, EmotionState.Attraction, "“Probably not being enough for someone I really care about.”", 2f, "Reasonable", false));
            reactions.Add(CreateReaction(q9T4, EmotionState.Attraction, ConnectionTier.Tier4_SecondDate, "She looks at him a little more carefully. “That’s a pretty vulnerable answer.”", 2f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q9T4, EmotionState.CalmAnxiety, "“I worry about letting people down. I can usually keep it under control, but it’s there.”", 18f, "HighFit", false));
            reactions.Add(CreateReaction(q9T4, EmotionState.CalmAnxiety, ConnectionTier.Tier4_SecondDate, "She nods slowly. “I get that.” Her voice drops a little, matching his honesty.", 18f, "second_date"));
            answers.Add(CreateAnswer(q9T4, EmotionState.CalmConfidence, "“I’m probably hardest on myself when I care about someone’s opinion of me.”", 8f, "Reasonable", false));
            reactions.Add(CreateReaction(q9T4, EmotionState.CalmConfidence, ConnectionTier.Tier4_SecondDate, "“Yeah.” She smiles. “I can relate to that more than I’d like.”", 8f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q9T4, EmotionState.CalmAttraction, "“I think... being known really well and then still not being chosen.”", 16f, "HighFit", false));
            reactions.Add(CreateReaction(q9T4, EmotionState.CalmAttraction, ConnectionTier.Tier4_SecondDate, "Her expression softens. “That’s a hard one.” She stays with the vulnerability instead of joking it away.", 16f, "second_date"));
            answers.Add(CreateAnswer(q9T4, EmotionState.AnxietyConfidence, "“I’m not insecure.” *Beat.* “Okay, that sounded convincing for about half a second.”", -10f, "ActivelyWrong", false));
            reactions.Add(CreateReaction(q9T4, EmotionState.AnxietyConfidence, ConnectionTier.Tier4_SecondDate, "She gives a small, sympathetic smile. “You don’t have to prove anything to me.”", -10f, "discomfort"));
            answers.Add(CreateAnswer(q9T4, EmotionState.AnxietyAttraction, "“Probably being rejected once someone actually sees all of me. That one’s... real.”", 10f, "HighFit", false));
            reactions.Add(CreateReaction(q9T4, EmotionState.AnxietyAttraction, ConnectionTier.Tier4_SecondDate, "“That makes sense.” She nods. “I think most people are afraid of being fully known.”", 10f, "warm_interest"));
            answers.Add(CreateAnswer(q9T4, EmotionState.ConfidenceAttraction, "“Losing someone I actually care about. I can handle a no; I don’t love the idea of having something good and messing it up.”", -16f, "ActivelyWrong", false));
            reactions.Add(CreateReaction(q9T4, EmotionState.ConfidenceAttraction, ConnectionTier.Tier4_SecondDate, "“I get that.” Her smile is gentle, but the answer carries more weight than she expected.", -16f, "shut_down"));
            answers.Add(CreateAnswer(q9T4, EmotionState.FrozenBlank, "*He swallows, looking at her with genuine, quiet vulnerability, unable to speak.*", 6f, "Reasonable", true));
            reactions.Add(CreateReaction(q9T4, EmotionState.FrozenBlank, ConnectionTier.Tier4_SecondDate, "Her expression softens completely. “Hey. It’s okay. You don’t have to lay it all out.”", 6f, "sympathetic_concern"));

            // --- Slot 9 Tier 3: Type 1 – Direct question ---
            var q9T3 = ScriptableObject.CreateInstance<QuestionData>();
            q9T3.slotIndex = 9;
            q9T3.connectionTier = ConnectionTier.Tier3_Strong;
            q9T3.scenarioType = "Type 1 – Direct question";
            q9T3.questionText = "\"This is maybe too much for a first date, but — what are you insecure about?\"";
            q9T3.responseTimeWindow = 6.0f;
            q9T3.socialEventProfile = new SocialEventProfile
            {
                requiresPlayerInitiation = false,
                primaryNodeOverride = InternalNodeType.Brain,
                effects = new List<TargetedEffect>
                {
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Brain,
                        effectType = SocialEffectType.StressEvent,
                        stressEventSubtype = StressEventSubtype.Panic,
                        magnitude = 35.0f
                    },
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Heart,
                        effectType = SocialEffectType.StressEvent,
                        stressEventSubtype = StressEventSubtype.HeartFlutter,
                        magnitude = 35.0f
                    },
                }
            };
            questions.Add(q9T3);

            answers.Add(CreateAnswer(q9T3, EmotionState.Calm, "“It’s okay. I think I worry about being misunderstood.”", 6f, "Reasonable", false));
            reactions.Add(CreateReaction(q9T3, EmotionState.Calm, ConnectionTier.Tier3_Strong, "“Yeah.” She nods. “I worry about that too sometimes.”", 6f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q9T3, EmotionState.Anxiety, "“That’s... a pretty big question.” *He laughs nervously.* “I guess I worry people won’t like the real version of me.”", 0f, "Reasonable", false));
            reactions.Add(CreateReaction(q9T3, EmotionState.Anxiety, ConnectionTier.Tier3_Strong, "“It is a big question.” She smiles gently. “You can take your time.”", 0f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q9T3, EmotionState.Confidence, "“It’s not too much. I’d say I’m still figuring out what I actually want.”", 12f, "HighFit", false));
            reactions.Add(CreateReaction(q9T3, EmotionState.Confidence, ConnectionTier.Tier3_Strong, "She nods once. “Okay.” The answer feels safe, but not especially vulnerable.", 12f, "warm_interest"));
            answers.Add(CreateAnswer(q9T3, EmotionState.Attraction, "“I worry that when I really like someone, I care too much about whether they like me back.”", 8f, "Reasonable", false));
            reactions.Add(CreateReaction(q9T3, EmotionState.Attraction, ConnectionTier.Tier3_Strong, "Her expression softens. “That makes sense.” She seems touched, but slightly cautious about the intensity.", 8f, "sympathetic_concern"));
            answers.Add(CreateAnswer(q9T3, EmotionState.CalmAnxiety, "“It’s a big question, but... I’d say I worry about letting people down.”", 6f, "Reasonable", false));
            reactions.Add(CreateReaction(q9T3, EmotionState.CalmAnxiety, ConnectionTier.Tier3_Strong, "“I appreciate you answering it.” She smiles, visibly relieved by the honesty.", 6f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q9T3, EmotionState.CalmConfidence, "“I’m probably insecure about whether I’m as good at relationships as I am at everything else.”", 14f, "HighFit", false));
            reactions.Add(CreateReaction(q9T3, EmotionState.CalmConfidence, ConnectionTier.Tier3_Strong, "“I can see that.” She smiles, but the comparison makes the answer feel a little polished.", 14f, "warm_interest"));
            answers.Add(CreateAnswer(q9T3, EmotionState.CalmAttraction, "“Being vulnerable, honestly. I like being in control, and liking someone makes that harder.”", 14f, "HighFit", false));
            reactions.Add(CreateReaction(q9T3, EmotionState.CalmAttraction, ConnectionTier.Tier3_Strong, "She looks down briefly, then back up. “Yeah... I get that.” The vulnerability clearly lands.", 14f, "warm_interest"));
            answers.Add(CreateAnswer(q9T3, EmotionState.AnxietyConfidence, "“It’s not too much.” *Beat.* “I definitely didn’t just need three seconds to prepare an answer.”", 8f, "Reasonable", false));
            reactions.Add(CreateReaction(q9T3, EmotionState.AnxietyConfidence, ConnectionTier.Tier3_Strong, "She laughs softly. “You definitely did need those three seconds.” The joke is kind, but the moment loses some depth.", 8f, "nervous_laugh"));
            answers.Add(CreateAnswer(q9T3, EmotionState.AnxietyAttraction, "“I worry that if I like someone too much, I’ll give them every reason to leave.”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q9T3, EmotionState.AnxietyAttraction, ConnectionTier.Tier3_Strong, "“That’s a hard one.” Her voice gets quieter. “I understand that more than I probably want to admit.”", 4f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q9T3, EmotionState.ConfidenceAttraction, "“Probably caring more than I let people see. It’s not my favorite thing about myself.”", 16f, "HighFit", false));
            reactions.Add(CreateReaction(q9T3, EmotionState.ConfidenceAttraction, ConnectionTier.Tier3_Strong, "“Yeah.” She smiles. “I think caring that much can be a scary thing.”", 16f, "second_date"));
            answers.Add(CreateAnswer(q9T3, EmotionState.FrozenBlank, "*He hesitates, tracing the rim of his glass, overwhelmed by the question.*", -2f, "Poor", true));
            reactions.Add(CreateReaction(q9T3, EmotionState.FrozenBlank, ConnectionTier.Tier3_Strong, "She nods understandingly. “Forget I asked. Too deep for halfway through a drink.”", -2f, "discomfort"));

            // --- Slot 9 Tier 2: Type 9 – Reflective, wordless-into-half-line ---
            var q9T2 = ScriptableObject.CreateInstance<QuestionData>();
            q9T2.slotIndex = 9;
            q9T2.connectionTier = ConnectionTier.Tier2_Maybe;
            q9T2.scenarioType = "Type 9 – Reflective, wordless-into-half-line";
            q9T2.questionText = "\"I don't know if it's my place to ask this, but—\" *She stops herself.*";
            q9T2.responseTimeWindow = 6.0f;
            q9T2.socialEventProfile = new SocialEventProfile
            {
                requiresPlayerInitiation = false,
                primaryNodeOverride = InternalNodeType.Brain,
                effects = new List<TargetedEffect>
                {
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Brain,
                        effectType = SocialEffectType.StressEvent,
                        stressEventSubtype = StressEventSubtype.Overthinking,
                        magnitude = 25.0f
                    },
                }
            };
            questions.Add(q9T2);

            answers.Add(CreateAnswer(q9T2, EmotionState.Calm, "“You can ask. I’ll tell you if I don’t want to answer.”", 8f, "Reasonable", false));
            reactions.Add(CreateReaction(q9T2, EmotionState.Calm, ConnectionTier.Tier2_Maybe, "She smiles. “Okay.” The permission seems to make her more willing to continue.", 8f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q9T2, EmotionState.Anxiety, "“Now I really want to know what you were going to ask.”", -2f, "Poor", false));
            reactions.Add(CreateReaction(q9T2, EmotionState.Anxiety, ConnectionTier.Tier2_Maybe, "“Forget it.” She smiles quickly. “It’s probably not worth making this weird.”", -2f, "discomfort"));
            answers.Add(CreateAnswer(q9T2, EmotionState.Confidence, "“You’ve already started. Might as well finish.”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q9T2, EmotionState.Confidence, ConnectionTier.Tier2_Maybe, "“Maybe I will.” She smiles, but the invitation feels a little abrupt.", 4f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q9T2, EmotionState.Attraction, "“You can ask me.” *He smiles.* “I’m curious.”", 6f, "Reasonable", false));
            reactions.Add(CreateReaction(q9T2, EmotionState.Attraction, ConnectionTier.Tier2_Maybe, "“I might.” She smiles, but the curiosity in his voice makes her choose her words more carefully.", 6f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q9T2, EmotionState.CalmAnxiety, "“You can ask. No promises I’ll answer perfectly.”", 6f, "Reasonable", false));
            reactions.Add(CreateReaction(q9T2, EmotionState.CalmAnxiety, ConnectionTier.Tier2_Maybe, "“Okay.” She takes a breath. “Then I’ll ask.”", 6f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q9T2, EmotionState.CalmConfidence, "“Go on. I’m comfortable with you asking.”", 14f, "HighFit", false));
            reactions.Add(CreateReaction(q9T2, EmotionState.CalmConfidence, ConnectionTier.Tier2_Maybe, "“Alright.” She nods, seeming more comfortable about continuing.", 14f, "warm_interest"));
            answers.Add(CreateAnswer(q9T2, EmotionState.CalmAttraction, "“You can ask me. I trust where this is going.”", 12f, "HighFit", false));
            reactions.Add(CreateReaction(q9T2, EmotionState.CalmAttraction, ConnectionTier.Tier2_Maybe, "She holds his eyes. “Okay.” The trust makes her decide not to pull the question back.", 12f, "warm_interest"));
            answers.Add(CreateAnswer(q9T2, EmotionState.AnxietyConfidence, "“You can say it.” *Beat.* “I can handle one unfinished sentence.”", 6f, "Reasonable", false));
            reactions.Add(CreateReaction(q9T2, EmotionState.AnxietyConfidence, ConnectionTier.Tier2_Maybe, "She laughs under her breath. “Okay, okay.” She lets the unfinished thought go instead.", 6f, "nervous_laugh"));
            answers.Add(CreateAnswer(q9T2, EmotionState.AnxietyAttraction, "“You can ask. I promise I won’t judge you for wanting to know.”", 2f, "Reasonable", false));
            reactions.Add(CreateReaction(q9T2, EmotionState.AnxietyAttraction, ConnectionTier.Tier2_Maybe, "“Maybe another time.” She smiles, still appreciative that he did not judge her hesitation.", 2f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q9T2, EmotionState.ConfidenceAttraction, "“Now you definitely have to finish that thought.”", 8f, "Reasonable", false));
            reactions.Add(CreateReaction(q9T2, EmotionState.ConfidenceAttraction, ConnectionTier.Tier2_Maybe, "She laughs. “Fine. You made me curious enough to finish it.”", 8f, "nervous_laugh"));
            answers.Add(CreateAnswer(q9T2, EmotionState.FrozenBlank, "*He stays quiet, letting her unfinished question evaporate into the room.*", 0f, "Reasonable", true));
            reactions.Add(CreateReaction(q9T2, EmotionState.FrozenBlank, ConnectionTier.Tier2_Maybe, "She lets out a quiet breath, turning her attention back to her drink.", 0f, "neutral_acknowledge"));

            // --- Slot 9 Tier 1: Type 7 – Direct confrontation ---
            var q9T1 = ScriptableObject.CreateInstance<QuestionData>();
            q9T1.slotIndex = 9;
            q9T1.connectionTier = ConnectionTier.Tier1_Awkward;
            q9T1.scenarioType = "Type 7 – Direct confrontation";
            q9T1.questionText = "\"I feel like I'm asking questions into a wall right now. Is that fair?\"";
            q9T1.responseTimeWindow = 6.0f;
            q9T1.socialEventProfile = new SocialEventProfile
            {
                requiresPlayerInitiation = false,
                primaryNodeOverride = InternalNodeType.Voice,
                effects = new List<TargetedEffect>
                {
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Voice,
                        effectType = SocialEffectType.StressEvent,
                        stressEventSubtype = StressEventSubtype.AwkwardSilence,
                        magnitude = 45.0f
                    },
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Brain,
                        effectType = SocialEffectType.StressEvent,
                        stressEventSubtype = StressEventSubtype.Panic,
                        magnitude = 45.0f
                    },
                }
            };
            questions.Add(q9T1);

            answers.Add(CreateAnswer(q9T1, EmotionState.Calm, "“A little. I’ve been in my head, but I’m here with you.”", -6f, "Poor", false));
            reactions.Add(CreateReaction(q9T1, EmotionState.Calm, ConnectionTier.Tier1_Awkward, "“Okay.” She smiles, relieved. “I just wanted to know you were with me.”", -6f, "discomfort"));
            answers.Add(CreateAnswer(q9T1, EmotionState.Anxiety, "“Yeah... maybe. I’m sorry. I don’t mean to make you feel like you’re talking to yourself.”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q9T1, EmotionState.Anxiety, ConnectionTier.Tier1_Awkward, "“I know you don’t mean to.” She smiles gently, but she is clearly tired of carrying the conversation.", 4f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q9T1, EmotionState.Confidence, "“I wouldn’t say a wall. You’ve definitely got my attention.”", -14f, "ActivelyWrong", false));
            reactions.Add(CreateReaction(q9T1, EmotionState.Confidence, ConnectionTier.Tier1_Awkward, "“Okay.” She gives him a small look. “Then help me out a little.”", -14f, "shut_down"));
            answers.Add(CreateAnswer(q9T1, EmotionState.Attraction, "“No. I’m listening. I just get quiet when I care.”", 0f, "Reasonable", false));
            reactions.Add(CreateReaction(q9T1, EmotionState.Attraction, ConnectionTier.Tier1_Awkward, "Her expression softens. “Okay.” She smiles. “That actually makes me feel a lot better.”", 0f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q9T1, EmotionState.CalmAnxiety, "“A little, yeah. I’m not checked out—I’m just trying too hard to get things right.”", 16f, "HighFit", false));
            reactions.Add(CreateReaction(q9T1, EmotionState.CalmAnxiety, ConnectionTier.Tier1_Awkward, "“I get that.” She nods. “I can tell you’re trying.”", 16f, "second_date"));
            answers.Add(CreateAnswer(q9T1, EmotionState.CalmConfidence, "“Fair. I can do better. I don’t want you carrying the whole conversation.”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q9T1, EmotionState.CalmConfidence, ConnectionTier.Tier1_Awkward, "“Thank you.” She smiles. “That’s all I needed to hear.”", 4f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q9T1, EmotionState.CalmAttraction, "“Fair. I’ve been listening more than talking. I do like being here with you.”", -4f, "Poor", false));
            reactions.Add(CreateReaction(q9T1, EmotionState.CalmAttraction, ConnectionTier.Tier1_Awkward, "“I know.” She smiles. “You’ve been listening. I just wasn’t sure what was happening in your head.”", -4f, "discomfort"));
            answers.Add(CreateAnswer(q9T1, EmotionState.AnxietyConfidence, "“No, it’s not that bad.” *Beat.* “Okay. It probably is a little that bad.”", 6f, "Reasonable", false));
            reactions.Add(CreateReaction(q9T1, EmotionState.AnxietyConfidence, ConnectionTier.Tier1_Awkward, "She laughs softly. “A little.” The honesty makes the moment uncomfortable, but at least real.", 6f, "nervous_laugh"));
            answers.Add(CreateAnswer(q9T1, EmotionState.AnxietyAttraction, "“Yeah... it’s fair. I’m sorry. I like you, and somehow that’s making me worse at talking.”", 8f, "Reasonable", false));
            reactions.Add(CreateReaction(q9T1, EmotionState.AnxietyAttraction, ConnectionTier.Tier1_Awkward, "Her face softens, but she looks tired. “I know. And I don't want to make you feel bad, but I need you to meet me halfway.”", 8f, "sympathetic_concern"));
            answers.Add(CreateAnswer(q9T1, EmotionState.ConfidenceAttraction, "“Fair enough. Give me a second—I’ve got more for you than that.”", -10f, "ActivelyWrong", false));
            reactions.Add(CreateReaction(q9T1, EmotionState.ConfidenceAttraction, ConnectionTier.Tier1_Awkward, "“Alright.” She smiles. “That’s more like it.”", -10f, "discomfort"));
            answers.Add(CreateAnswer(q9T1, EmotionState.FrozenBlank, "*He looks at her, but the answer never quite forms.*", -8f, "Poor", true));
            reactions.Add(CreateReaction(q9T1, EmotionState.FrozenBlank, ConnectionTier.Tier1_Awkward, "She shakes her head slowly. “I think that answers it.”", -8f, "discomfort"));

            return (questions, answers, reactions);
        }


        private static (List<QuestionData>, List<AnswerData>, List<ReactionData>) BuildSlot10()
        {
            var questions = new List<QuestionData>();
            var answers = new List<AnswerData>();
            var reactions = new List<ReactionData>();

            // --- Slot 10 Tier 4: Type 1 – Warm ---
            var q10T4 = ScriptableObject.CreateInstance<QuestionData>();
            q10T4.slotIndex = 10;
            q10T4.connectionTier = ConnectionTier.Tier4_SecondDate;
            q10T4.scenarioType = "Type 1 – Warm";
            q10T4.questionText = "\"Okay — real talk. Would you want to do this again?\"";
            q10T4.responseTimeWindow = 6.0f;
            q10T4.socialEventProfile = new SocialEventProfile
            {
                requiresPlayerInitiation = false,
                primaryNodeOverride = InternalNodeType.Heart,
                effects = new List<TargetedEffect>
                {
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Heart,
                        effectType = SocialEffectType.DirectEmotionPush,
                        pushTargetEmotion = CoreEmotion.Attraction,
                        magnitude = 35.0f
                    },
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Voice,
                        effectType = SocialEffectType.DirectEmotionPush,
                        pushTargetEmotion = CoreEmotion.Confidence,
                        magnitude = 35.0f
                    },
                }
            };
            questions.Add(q10T4);

            answers.Add(CreateAnswer(q10T4, EmotionState.Calm, "“Yeah. I’d like to see you again.”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q10T4, EmotionState.Calm, ConnectionTier.Tier4_SecondDate, "Her smile comes easily. “Yeah. I’d like that.”", 4f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q10T4, EmotionState.Anxiety, "“Yeah. I mean—if you want to. I’d like to.”", 8f, "Reasonable", false));
            reactions.Add(CreateReaction(q10T4, EmotionState.Anxiety, ConnectionTier.Tier4_SecondDate, "“Yeah?” She smiles. “Okay. Good.” She keeps it simple so he does not overthink it.", 8f, "thinking_chintap"));
            answers.Add(CreateAnswer(q10T4, EmotionState.Confidence, "“Yeah. Definitely.”", 16f, "HighFit", false));
            reactions.Add(CreateReaction(q10T4, EmotionState.Confidence, ConnectionTier.Tier4_SecondDate, "“Good.” She laughs softly. “I was hoping you’d say yes.”", 16f, "second_date"));
            answers.Add(CreateAnswer(q10T4, EmotionState.Attraction, "“Yeah. I was already hoping you’d ask.”", 14f, "HighFit", false));
            reactions.Add(CreateReaction(q10T4, EmotionState.Attraction, ConnectionTier.Tier4_SecondDate, "“Really?” She smiles, but the intensity makes her answer less immediate.", 14f, "warm_interest"));
            answers.Add(CreateAnswer(q10T4, EmotionState.CalmAnxiety, "“Yeah. I’d like that. I’m nervous saying it, but yeah.”", 8f, "Reasonable", false));
            reactions.Add(CreateReaction(q10T4, EmotionState.CalmAnxiety, ConnectionTier.Tier4_SecondDate, "“Okay.” She smiles reassuringly. “You don’t have to sound like you’re announcing something important.”", 8f, "sympathetic_concern"));
            answers.Add(CreateAnswer(q10T4, EmotionState.CalmConfidence, "“Absolutely. I had a good time with you.”", 12f, "HighFit", false));
            reactions.Add(CreateReaction(q10T4, EmotionState.CalmConfidence, ConnectionTier.Tier4_SecondDate, "“I’d like that.” She nods, relaxed.", 12f, "warm_interest"));
            answers.Add(CreateAnswer(q10T4, EmotionState.CalmAttraction, "*He smiles.* “Yeah. I’d really like to see you again.”", 18f, "HighFit", false));
            reactions.Add(CreateReaction(q10T4, EmotionState.CalmAttraction, ConnectionTier.Tier4_SecondDate, "Her whole expression warms. “Yeah.” A tiny smile. “I’d really like that.”", 18f, "second_date"));
            answers.Add(CreateAnswer(q10T4, EmotionState.AnxietyConfidence, "“Obviously.” *Beat.* “I mean... yes. Definitely yes.”", 14f, "HighFit", false));
            reactions.Add(CreateReaction(q10T4, EmotionState.AnxietyConfidence, ConnectionTier.Tier4_SecondDate, "She laughs softly. “You can just say yes.” Her tone is kind, but the overcompensation is obvious.", 14f, "flirty_wink"));
            answers.Add(CreateAnswer(q10T4, EmotionState.AnxietyAttraction, "“Yeah. I really, really would.”", 10f, "HighFit", false));
            reactions.Add(CreateReaction(q10T4, EmotionState.AnxietyAttraction, ConnectionTier.Tier4_SecondDate, "She smiles gently. “Okay.” She lets him have the answer without making him keep proving it.", 10f, "warm_interest"));
            answers.Add(CreateAnswer(q10T4, EmotionState.ConfidenceAttraction, "“Yeah. I was hoping I’d get another date out of you.”", 18f, "HighFit", false));
            reactions.Add(CreateReaction(q10T4, EmotionState.ConfidenceAttraction, ConnectionTier.Tier4_SecondDate, "“Good.” She smiles. “Because I was already thinking about it.”", 18f, "second_date"));
            answers.Add(CreateAnswer(q10T4, EmotionState.FrozenBlank, "*He breaks into a wide, slightly stunned smile, nodding emphatically before words come.*", 10f, "HighFit", true));
            reactions.Add(CreateReaction(q10T4, EmotionState.FrozenBlank, ConnectionTier.Tier4_SecondDate, "She laughs happily, eyes sparkling. “I’m taking that enthusiastic nod as a yes.”", 10f, "flirty_wink"));

            // --- Slot 10 Tier 3: Type 1 – Hopeful, hedged ---
            var q10T3 = ScriptableObject.CreateInstance<QuestionData>();
            q10T3.slotIndex = 10;
            q10T3.connectionTier = ConnectionTier.Tier3_Strong;
            q10T3.scenarioType = "Type 1 – Hopeful, hedged";
            q10T3.questionText = "\"So... would you want to do this again? No pressure.\"";
            q10T3.responseTimeWindow = 6.0f;
            q10T3.socialEventProfile = new SocialEventProfile
            {
                requiresPlayerInitiation = false,
                primaryNodeOverride = InternalNodeType.Heart,
                effects = new List<TargetedEffect>
                {
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Heart,
                        effectType = SocialEffectType.StressEvent,
                        stressEventSubtype = StressEventSubtype.HeartFlutter,
                        magnitude = 25.0f
                    },
                }
            };
            questions.Add(q10T3);

            answers.Add(CreateAnswer(q10T3, EmotionState.Calm, "“Yeah. No pressure on you either, but I had a good time.”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q10T3, EmotionState.Calm, ConnectionTier.Tier3_Strong, "“Okay.” She smiles, visibly relieved. “I had a good time too.”", 4f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q10T3, EmotionState.Anxiety, "“Yeah. Definitely. Sorry, that came out way too fast.”", 6f, "Reasonable", false));
            reactions.Add(CreateReaction(q10T3, EmotionState.Anxiety, ConnectionTier.Tier3_Strong, "She laughs softly. “You can breathe.” Then: “Yes, I’d like that.”", 6f, "nervous_laugh"));
            answers.Add(CreateAnswer(q10T3, EmotionState.Confidence, "“Yeah. I wouldn’t be here this long if I didn’t.”", 10f, "HighFit", false));
            reactions.Add(CreateReaction(q10T3, EmotionState.Confidence, ConnectionTier.Tier3_Strong, "“Good.” She smiles. “I was hoping you wouldn’t make me ask twice.”", 10f, "warm_interest"));
            answers.Add(CreateAnswer(q10T3, EmotionState.Attraction, "“Yeah. I’d like that.” *He smiles.*", 10f, "HighFit", false));
            reactions.Add(CreateReaction(q10T3, EmotionState.Attraction, ConnectionTier.Tier3_Strong, "“Yeah.” She smiles back, warm but not surprised.", 10f, "warm_interest"));
            answers.Add(CreateAnswer(q10T3, EmotionState.CalmAnxiety, "“I would. I’m trying not to overthink the answer, so... yes.”", 8f, "Reasonable", false));
            reactions.Add(CreateReaction(q10T3, EmotionState.CalmAnxiety, ConnectionTier.Tier3_Strong, "“Okay.” She nods. “You don’t have to overthink it.”", 8f, "thinking_chintap"));
            answers.Add(CreateAnswer(q10T3, EmotionState.CalmConfidence, "“Yeah. I’d be happy to do this again.”", 12f, "HighFit", false));
            reactions.Add(CreateReaction(q10T3, EmotionState.CalmConfidence, ConnectionTier.Tier3_Strong, "“Good.” Her smile is immediate. “I’d like another one too.”", 12f, "warm_interest"));
            answers.Add(CreateAnswer(q10T3, EmotionState.CalmAttraction, "“Yeah. I’ve really liked tonight.”", 16f, "HighFit", false));
            reactions.Add(CreateReaction(q10T3, EmotionState.CalmAttraction, ConnectionTier.Tier3_Strong, "“Yeah.” She looks genuinely pleased. “I’ve really liked tonight.”", 16f, "second_date"));
            answers.Add(CreateAnswer(q10T3, EmotionState.AnxietyConfidence, "“Of course.” *Beat.* “Yes. I mean, yes. Very much.”", 14f, "HighFit", false));
            reactions.Add(CreateReaction(q10T3, EmotionState.AnxietyConfidence, ConnectionTier.Tier3_Strong, "“Yes.” She laughs gently. “You can stop proving the point now.”", 14f, "flirty_wink"));
            answers.Add(CreateAnswer(q10T3, EmotionState.AnxietyAttraction, "“Yeah. I’d really like another date. No pressure from me either.”", 8f, "Reasonable", false));
            reactions.Add(CreateReaction(q10T3, EmotionState.AnxietyAttraction, ConnectionTier.Tier3_Strong, "“I’d like that too.” She smiles, but the intensity tells her he is still very nervous.", 8f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q10T3, EmotionState.ConfidenceAttraction, "“Definitely. I was already planning on asking you.”", 18f, "HighFit", false));
            reactions.Add(CreateReaction(q10T3, EmotionState.ConfidenceAttraction, ConnectionTier.Tier3_Strong, "“I figured you might ask.” She smiles politely, but the certainty takes some of the softness out of the moment.", 18f, "second_date"));
            answers.Add(CreateAnswer(q10T3, EmotionState.FrozenBlank, "*He hesitates, nervous smile trembling, nodding quietly.*", 4f, "Reasonable", true));
            reactions.Add(CreateReaction(q10T3, EmotionState.FrozenBlank, ConnectionTier.Tier3_Strong, "She smiles with a touch of relief. “Okay. Good to hear.”", 4f, "relieved_sigh"));

            // --- Slot 10 Tier 2: Type 9 – Uncertain ---
            var q10T2 = ScriptableObject.CreateInstance<QuestionData>();
            q10T2.slotIndex = 10;
            q10T2.connectionTier = ConnectionTier.Tier2_Maybe;
            q10T2.scenarioType = "Type 9 – Uncertain";
            q10T2.questionText = "\"I genuinely don't know how tonight went for you. Would you want to do this again?\"";
            q10T2.responseTimeWindow = 6.0f;
            q10T2.socialEventProfile = new SocialEventProfile
            {
                requiresPlayerInitiation = false,
                primaryNodeOverride = InternalNodeType.Brain,
                effects = new List<TargetedEffect>
                {
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Brain,
                        effectType = SocialEffectType.StressEvent,
                        stressEventSubtype = StressEventSubtype.Overthinking,
                        magnitude = 35.0f
                    },
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Heart,
                        effectType = SocialEffectType.StressEvent,
                        stressEventSubtype = StressEventSubtype.HeartFlutter,
                        magnitude = 35.0f
                    },
                }
            };
            questions.Add(q10T2);

            answers.Add(CreateAnswer(q10T2, EmotionState.Calm, "“It went well. I had a good time, and I’d like to see you again.”", 8f, "Reasonable", false));
            reactions.Add(CreateReaction(q10T2, EmotionState.Calm, ConnectionTier.Tier2_Maybe, "Her face relaxes. “Okay. I’m glad.”", 8f, "relieved_sigh"));
            answers.Add(CreateAnswer(q10T2, EmotionState.Anxiety, "“I’m sorry I made it hard to tell. Yes. I liked tonight.”", -2f, "Poor", false));
            reactions.Add(CreateReaction(q10T2, EmotionState.Anxiety, ConnectionTier.Tier2_Maybe, "“I believe you.” She smiles, though she still wishes he had shown it earlier.", -2f, "discomfort"));
            answers.Add(CreateAnswer(q10T2, EmotionState.Confidence, "“I thought it went well. I’d absolutely do it again.”", 4f, "Reasonable", false));
            reactions.Add(CreateReaction(q10T2, EmotionState.Confidence, ConnectionTier.Tier2_Maybe, "“Good.” She smiles. “I just wish I’d known that sooner.”", 4f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q10T2, EmotionState.Attraction, "“Yeah. I liked tonight more than I expected.”", 6f, "Reasonable", false));
            reactions.Add(CreateReaction(q10T2, EmotionState.Attraction, ConnectionTier.Tier2_Maybe, "“I’m glad you did.” She smiles, but the intensity suggests he had been holding back more than she realized.", 6f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q10T2, EmotionState.CalmAnxiety, "“It went well for me. I’m just bad at showing it when I’m nervous.”", 6f, "Reasonable", false));
            reactions.Add(CreateReaction(q10T2, EmotionState.CalmAnxiety, ConnectionTier.Tier2_Maybe, "“Okay.” She smiles. “That actually helps.”", 6f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q10T2, EmotionState.CalmConfidence, "“It went well. I’d like another date.”", 14f, "HighFit", false));
            reactions.Add(CreateReaction(q10T2, EmotionState.CalmConfidence, ConnectionTier.Tier2_Maybe, "“Good.” She nods. “I’d like that.”", 14f, "warm_interest"));
            answers.Add(CreateAnswer(q10T2, EmotionState.CalmAttraction, "“It went really well. I’ve genuinely liked being with you tonight.”", 12f, "HighFit", false));
            reactions.Add(CreateReaction(q10T2, EmotionState.CalmAttraction, ConnectionTier.Tier2_Maybe, "Her expression softens completely. “Good. Because I really enjoyed being with you.”", 12f, "warm_interest"));
            answers.Add(CreateAnswer(q10T2, EmotionState.AnxietyConfidence, "“It went well.” *Beat.* “I know I haven’t exactly made that easy to read.”", 6f, "Reasonable", false));
            reactions.Add(CreateReaction(q10T2, EmotionState.AnxietyConfidence, ConnectionTier.Tier2_Maybe, "“I know.” She gives a small smile. “You haven’t exactly made it easy to read.”", 6f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q10T2, EmotionState.AnxietyAttraction, "“It went really well. I’m sorry if I made you doubt that—I actually like you a lot.”", 2f, "Reasonable", false));
            reactions.Add(CreateReaction(q10T2, EmotionState.AnxietyAttraction, ConnectionTier.Tier2_Maybe, "Her face brightens. “Really?” She smiles. “Okay. I’m glad I asked.”", 2f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q10T2, EmotionState.ConfidenceAttraction, "“I think you know the answer.” *He smiles.* “Yeah.”", 8f, "Reasonable", false));
            reactions.Add(CreateReaction(q10T2, EmotionState.ConfidenceAttraction, ConnectionTier.Tier2_Maybe, "She smiles, but the “you know the answer” certainty makes her feel like she was expected to know. “Okay.”", 8f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q10T2, EmotionState.FrozenBlank, "*He looks at her uncertainly, frozen between yes and no.*", 0f, "Reasonable", true));
            reactions.Add(CreateReaction(q10T2, EmotionState.FrozenBlank, ConnectionTier.Tier2_Maybe, "She gives a small, wistful smile. “It’s okay. We don’t have to decide right this second.”", 0f, "neutral_acknowledge"));

            // --- Slot 10 Tier 1: Type 1 – Bracing ---
            var q10T1 = ScriptableObject.CreateInstance<QuestionData>();
            q10T1.slotIndex = 10;
            q10T1.connectionTier = ConnectionTier.Tier1_Awkward;
            q10T1.scenarioType = "Type 1 – Bracing";
            q10T1.questionText = "\"Would you want to do this again? Be honest — I'd rather know.\"";
            q10T1.responseTimeWindow = 6.0f;
            q10T1.socialEventProfile = new SocialEventProfile
            {
                requiresPlayerInitiation = false,
                primaryNodeOverride = InternalNodeType.Brain,
                effects = new List<TargetedEffect>
                {
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Brain,
                        effectType = SocialEffectType.StressEvent,
                        stressEventSubtype = StressEventSubtype.Panic,
                        magnitude = 45.0f
                    },
                    new TargetedEffect
                    {
                        targetNode = InternalNodeType.Body,
                        effectType = SocialEffectType.StressEvent,
                        stressEventSubtype = StressEventSubtype.SweatSurge,
                        magnitude = 45.0f
                    },
                }
            };
            questions.Add(q10T1);

            answers.Add(CreateAnswer(q10T1, EmotionState.Calm, "“Honestly? Yes. I’d like to see you again.”", 2f, "Reasonable", false));
            reactions.Add(CreateReaction(q10T1, EmotionState.Calm, ConnectionTier.Tier1_Awkward, "She exhales quietly. “Okay.” The relief is obvious in her smile.", 2f, "relieved_sigh"));
            answers.Add(CreateAnswer(q10T1, EmotionState.Anxiety, "“Yes. I’m sorry if I made it look like no.”", -4f, "Poor", false));
            reactions.Add(CreateReaction(q10T1, EmotionState.Anxiety, ConnectionTier.Tier1_Awkward, "“No, it’s okay.” She smiles gently. “I believe you.” She does not punish the nervousness.", -4f, "discomfort"));
            answers.Add(CreateAnswer(q10T1, EmotionState.Confidence, "“Yes. No hesitation.”", 0f, "Reasonable", false));
            reactions.Add(CreateReaction(q10T1, EmotionState.Confidence, ConnectionTier.Tier1_Awkward, "“Okay.” She smiles. “That was clearer.”", 0f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q10T1, EmotionState.Attraction, "“Yes. I don’t really want tonight to be over yet.”", -4f, "Poor", false));
            reactions.Add(CreateReaction(q10T1, EmotionState.Attraction, ConnectionTier.Tier1_Awkward, "“You don’t have to stay just because you don’t want the night to end yet.” Her tone stays kind, but she needs the answer to feel grounded.", -4f, "discomfort"));
            answers.Add(CreateAnswer(q10T1, EmotionState.CalmAnxiety, "“Yes. I’m nervous saying it, but I mean it.”", 8f, "Reasonable", false));
            reactions.Add(CreateReaction(q10T1, EmotionState.CalmAnxiety, ConnectionTier.Tier1_Awkward, "“Okay.” She smiles. “I’m glad you said it.”", 8f, "neutral_acknowledge"));
            answers.Add(CreateAnswer(q10T1, EmotionState.CalmConfidence, "“Yes. I’m sure.”", 12f, "HighFit", false));
            reactions.Add(CreateReaction(q10T1, EmotionState.CalmConfidence, ConnectionTier.Tier1_Awkward, "Her shoulders visibly relax. “Good.” She smiles. “I wanted to hear that.”", 12f, "warm_interest"));
            answers.Add(CreateAnswer(q10T1, EmotionState.CalmAttraction, "“Yes. I’ve liked being with you tonight.”", -4f, "Poor", false));
            reactions.Add(CreateReaction(q10T1, EmotionState.CalmAttraction, ConnectionTier.Tier1_Awkward, "She smiles warmly. “Me too.” She holds his eyes for a second.", -4f, "discomfort"));
            answers.Add(CreateAnswer(q10T1, EmotionState.AnxietyConfidence, "“Of course I would.” *Beat.* “Why did I say that like I was arguing a case?”", 6f, "Reasonable", false));
            reactions.Add(CreateReaction(q10T1, EmotionState.AnxietyConfidence, ConnectionTier.Tier1_Awkward, "She laughs softly. “You can just say yes without arguing your case.”", 6f, "nervous_laugh"));
            answers.Add(CreateAnswer(q10T1, EmotionState.AnxietyAttraction, "“Yes. Absolutely. I was scared you didn’t want to, too.”", -4f, "Poor", false));
            reactions.Add(CreateReaction(q10T1, EmotionState.AnxietyAttraction, ConnectionTier.Tier1_Awkward, "“Okay.” Her expression softens. “I’m glad.” She looks relieved, though still a little unsure.", -4f, "sympathetic_concern"));
            answers.Add(CreateAnswer(q10T1, EmotionState.ConfidenceAttraction, "“Yeah. I’d like another one.” *He smiles.* “I’m not exactly trying to hide that.”", -6f, "Poor", false));
            reactions.Add(CreateReaction(q10T1, EmotionState.ConfidenceAttraction, ConnectionTier.Tier1_Awkward, "“Good.” She smiles. “Then I guess we’re on the same page.”", -6f, "discomfort"));
            answers.Add(CreateAnswer(q10T1, EmotionState.FrozenBlank, "*He takes a breath, looks at her, and nods.* “Yeah.”", -8f, "Poor", false));
            reactions.Add(CreateReaction(q10T1, EmotionState.FrozenBlank, ConnectionTier.Tier1_Awkward, "She gives a sad, polite nod, knowing the truth. “Thanks for being honest.”", -8f, "sad_disappointment"));

            return (questions, answers, reactions);
        }

        private static AnswerData CreateAnswer(QuestionData parent, EmotionState state, string spoken, float delta, string band, bool isWordless)
        {
            var a = ScriptableObject.CreateInstance<AnswerData>();
            a.parentQuestion = parent;
            a.emotionState = state;
            a.spokenText = spoken;
            a.connectionDelta = delta;
            a.band = band;
            a.isWordless = isWordless;
            return a;
        }

        private static ReactionData CreateReaction(QuestionData parent, EmotionState state, ConnectionTier resultingTier, string text, float delta, string clipTag)
        {
            var r = ScriptableObject.CreateInstance<ReactionData>();
            r.parentQuestion = parent;
            r.emotionState = state;
            r.resultingTier = resultingTier;
            r.reactionText = text;
            r.linkedAnswerDelta = delta;
            r.animationClipTag = clipTag;
            return r;
        }
    }
}
