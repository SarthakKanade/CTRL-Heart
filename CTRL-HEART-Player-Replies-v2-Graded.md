# CTRL+HEART — Player Replies v2 (Graded)

## Authoring target
These are the automatic player-character replies produced by the internal state. They are not dialogue choices. The dialogue text is unchanged from v1; this pass adds the Connection grading layer.

### Grading rule
- The working target is **2 High Fit / 3 Reasonable / 3 Poor / 2 Actively Wrong** across the ten emotion/mix states.
- The two grading examples you supplied for Slot 1 T4 and Slot 1 T1 are preserved exactly. Those two naturally land at **2 High / 4 Reasonable / 2 Poor / 2 Wrong**, so the 2:3:3:2 split remains a target rather than a forced quota.
- Deltas stay inside the locked bands: **High Fit +10 to +20**, **Reasonable 0 to +9**, **Poor −1 to −10**, **Actively Wrong −11 to −20**. Zero belongs to Reasonable.
- Positive and negative magnitudes are kept broadly symmetric; the strongest failures can reach the same magnitude as the strongest successes.
- **Frozen/Blank is separate from the ten-state distribution and is assigned −12** as the default derailment cost.

### Delta legend
| Band | Range | Meaning |
|---|---:|---|
| **High fit** | +10 to +20 | Nails the scenario test and emotional register. |
| **Reasonable** | 0 to +9 | Basically works, but is generic, weaker, or slightly mis-timed. |
| **Poor** | −1 to −10 | Misreads the moment or responds in the wrong register. |
| **Actively wrong** | −11 to −20 | Makes the exact problem worse or clearly fails the moment. |
| **Frozen/Blank** | −12 | Separate derailment state; not part of the ten-state distribution. |

# SLOT 1 — Arrival / first exchange

## T4 — She compliments him
**Her beat:** “Okay, you clean up well. I wasn't expecting that.”

**Target distribution:** 2 High Fit / 3 Reasonable / 3 Poor / 2 Actively Wrong  
**Frozen/Blank:** separate at −12

| State | Answer | Delta | Band |
|---|---|---:|---|
| Calm | “Thanks. You look really good too.” | +11 | Reasonable |
| Anxiety | “Oh—thanks. I, uh... wasn't sure what to wear.” | −17 | Actively wrong |
| Confidence | “Thanks. I figured I should make an effort.” | −4 | Poor |
| Attraction | *He smiles, a little too obviously pleased.* “Thanks.” | +3 | Reasonable |
| Calm+Anxiety | “Thanks. I was a little worried I'd overthought the whole outfit.” | 0 | Reasonable |
| Calm+Confidence | “Thanks. I’m glad I got it right.” | +18 | High fit |
| Calm+Attraction | *A small smile.* “Thanks. You look really nice.” | +15 | High fit |
| Anxiety+Confidence | “Yeah, I mean—obviously I was going to look good. Thanks.” *He laughs at himself.* | −14 | Actively wrong |
| Anxiety+Attraction | “Really? Oh. Thanks... that’s actually really nice to hear.” | −7 | Poor |
| Confidence+Attraction | “Good. I was hoping I’d make a decent first impression.” | +6 | Reasonable |

## T3 — She asks about his day
**Her beat:** “So how was your day, actually—not the polite version.”

**Target distribution:** 2 High Fit / 3 Reasonable / 3 Poor / 2 Actively Wrong  
**Frozen/Blank:** separate at −12

| State | Answer | Delta | Band |
|---|---|---:|---|
| Calm | “Pretty busy. Nothing dramatic, just one of those days. I’m good now, though.” | +6 | Reasonable |
| Anxiety | “It was fine... I mean, mostly. I had a meeting that went weird, then I started thinking about tonight and—yeah, anyway.” | −18 | Actively wrong |
| Confidence | “Long day, but I handled it. I’m here now, so it worked out.” | −8 | Poor |
| Attraction | “Honestly? A bit of a mess. I was mostly looking forward to this.” | +2 | Reasonable |
| Calm+Anxiety | “Kind of a lot. I’m fine, just still coming down from it a little.” | +9 | Reasonable |
| Calm+Confidence | “Busy, a little chaotic. Nothing I couldn’t deal with.” | +18 | High fit |
| Calm+Attraction | “Long one. But honestly, sitting here with you already feels like a better part of it.” | +14 | High fit |
| Anxiety+Confidence | “It was totally fine. Just work being work. Nothing got to me.” *He pauses.* “Much.” | −14 | Actively wrong |
| Anxiety+Attraction | “It was rough, actually. I was nervous about tonight too, so... not exactly my calmest day.” | −5 | Poor |
| Confidence+Attraction | “Kind of brutal. Then I got here, so I’m counting that as the turnaround.” | −1 | Poor |

## T2 — She leaves a comfortable pause
**Her beat:** *She glances around the room for a second, half-listening, just settling in.*

**Target distribution:** 2 High Fit / 3 Reasonable / 3 Poor / 2 Actively Wrong  
**Frozen/Blank:** separate at −12

| State | Answer | Delta | Band |
|---|---|---:|---|
| Calm | *He takes a sip and lets the silence sit.* | +9 | Reasonable |
| Anxiety | “So... yeah. This place is nice. I checked out the menu before I came, which is probably unnecessary, but—” | −18 | Actively wrong |
| Confidence | *He leans back, relaxed, and gives her the space without filling it.* | +6 | Reasonable |
| Attraction | *He catches her eye and smiles, comfortable letting the moment stay quiet.* | −5 | Poor |
| Calm+Anxiety | “Take your time. I’m good.” | −1 | Poor |
| Calm+Confidence | *He settles back, totally at ease with the quiet.* | +18 | High fit |
| Calm+Attraction | *He watches her for a second, smiling softly, then takes another sip.* | +14 | High fit |
| Anxiety+Confidence | “You’re good. I wasn’t about to panic over two seconds of silence.” *A beat.* “Not yet, anyway.” | −14 | Actively wrong |
| Anxiety+Attraction | *He smiles, looks down at his drink, then back at her, clearly trying not to overthink the silence.* | −8 | Poor |
| Confidence+Attraction | *A small grin.* “You can have a minute.” | +2 | Reasonable |

## T1 — She notices he is tense
**Her beat:** “Hey—you okay? You seem kind of tense already.”

**Target distribution:** 2 High Fit / 3 Reasonable / 3 Poor / 2 Actively Wrong  
**Frozen/Blank:** separate at −12

| State | Answer | Delta | Band |
|---|---|---:|---|
| Calm | “Yeah, I’m okay. Just first-date nerves. They’ll go away.” | +6 | Reasonable |
| Anxiety | “Do I? Sorry. I didn’t realise it was that obvious.” | −17 | Actively wrong |
| Confidence | “Yeah. I’m fine. Just settling in.” | −8 | Poor |
| Attraction | “Maybe a little. You’re kind of hard not to be nervous around.” | +2 | Reasonable |
| Calm+Anxiety | “A little, yeah. Nothing bad. I just need a second to settle.” | +9 | Reasonable |
| Calm+Confidence | “Yeah, a little. I’m good, though.” | +18 | High fit |
| Calm+Attraction | “A little. I think it’s mostly because I really wanted to meet you.” | +13 | High fit |
| Anxiety+Confidence | “Tense? No. I’m completely fine.” *He immediately fidgets with his glass.* “See?” | −14 | Actively wrong |
| Anxiety+Attraction | “A little, yeah. I really wanted tonight to go well, so...” | −5 | Poor |
| Confidence+Attraction | “Guilty. You have that effect on me, apparently.” | 0 | Reasonable |
| Frozen/Blank | *He straightens too quickly, nods, and says nothing.* | −12 | Frozen/Blank |

# SLOT 2 — Small talk deepens

## T4 — She teases him about a useless talent
**Her beat:** “Okay, hot take—you’ve got a weird useless talent, don’t you?”

**Target distribution:** 2 High Fit / 3 Reasonable / 3 Poor / 2 Actively Wrong  
**Frozen/Blank:** separate at −12

| State | Answer | Delta | Band |
|---|---|---:|---|
| Calm | “I can fold a fitted sheet properly. That counts.” | +9 | Reasonable |
| Anxiety | “I mean... probably? I have a few useless ones. None I can demonstrate under pressure.” | −18 | Actively wrong |
| Confidence | “Absolutely. And you’re going to regret asking.” | +18 | High fit |
| Attraction | *He smiles.* “I’ve got one. I might save it for later.” | −1 | Poor |
| Calm+Anxiety | “I do, actually. It’s stupid, but I’m weirdly good at it.” | −8 | Poor |
| Calm+Confidence | “Yeah. I can open almost any snack bag without ripping it.” | +6 | Reasonable |
| Calm+Attraction | “I’ve got one, but I’m pretty sure it only becomes impressive after you’ve known me for a while.” | +2 | Reasonable |
| Anxiety+Confidence | “Oh, definitely. I’ve got several.” *Beat.* “I’m not nervous about this question, if that’s what you’re testing.” | −14 | Actively wrong |
| Anxiety+Attraction | “There is one... but telling you about it this early feels like giving away too much.” | −5 | Poor |
| Confidence+Attraction | “I do. You’ll have to earn the demonstration.” | +14 | High fit |

## T3 — She has a small physical mishap
**Her beat:** *She nearly drops her napkin, laughs while grabbing it.*

**Target distribution:** 2 High Fit / 3 Reasonable / 3 Poor / 2 Actively Wrong  
**Frozen/Blank:** separate at −12

| State | Answer | Delta | Band |
|---|---|---:|---|
| Calm | “You’re fine.” *He smiles and keeps eating.* | +9 | Reasonable |
| Anxiety | “Oh—careful. Sorry, I don’t know why I said sorry.” | −14 | Actively wrong |
| Confidence | “Nice recovery.” *He grins.* | +18 | High fit |
| Attraction | *He laughs with her.* “Okay, that was kind of adorable.” | +6 | Reasonable |
| Calm+Anxiety | “You got it. No disaster.” | −1 | Poor |
| Calm+Confidence | “Strong save.” *He gives her an approving nod.* | +14 | High fit |
| Calm+Attraction | *He laughs softly.* “You okay?” | +2 | Reasonable |
| Anxiety+Confidence | “Smooth.” *He grins, a little too quickly.* “I mean—good save.” | −5 | Poor |
| Anxiety+Attraction | “Oh my god, are you okay? That looked like it almost went everywhere.” | −18 | Actively wrong |
| Confidence+Attraction | “Nice save. I was about to pretend I didn’t see that.” | −8 | Poor |

## T2 — She asks what he does
**Her beat:** “So what do you do?”

**Target distribution:** 2 High Fit / 3 Reasonable / 3 Poor / 2 Actively Wrong  
**Frozen/Blank:** separate at −12

| State | Answer | Delta | Band |
|---|---|---:|---|
| Calm | “I work in software. Mostly product stuff. It’s busy, but I like it.” | +9 | Reasonable |
| Anxiety | “I’m in software. Well—not exactly software, more... product and development. It’s complicated.” | −14 | Actively wrong |
| Confidence | “I work in software. I like it, and I’m pretty good at what I do.” | +14 | High fit |
| Attraction | “I work in software. Honestly, I’m more interested in what you do.” | −8 | Poor |
| Calm+Anxiety | “Software. It’s a good job. I like the work, even when it gets hectic.” | −1 | Poor |
| Calm+Confidence | “I’m in software. It keeps me busy, and I’ve built some things I’m proud of.” | +18 | High fit |
| Calm+Attraction | “Software. I could give you the boring version, but I’m more curious about you.” | +6 | Reasonable |
| Anxiety+Confidence | “Software. It’s good. I’m good at it.” *He catches himself.* “That sounded more rehearsed than I meant.” | −5 | Poor |
| Anxiety+Attraction | “Software... sorry, that’s such a boring answer. I swear there’s more to me than my job.” | −18 | Actively wrong |
| Confidence+Attraction | “Software. I like solving problems all day. You might get the better version of me off the clock, though.” | +2 | Reasonable |

## T1 — She avoids eye contact for a beat
**Her beat:** *She checks her drink and doesn’t meet his eyes for a moment.*

**Target distribution:** 2 High Fit / 3 Reasonable / 3 Poor / 2 Actively Wrong  
**Frozen/Blank:** separate at −12

| State | Answer | Delta | Band |
|---|---|---:|---|
| Calm | “You good?” *He asks it casually, without making a thing of it.* | +9 | Reasonable |
| Anxiety | “Did I say something weird?” | −5 | Poor |
| Confidence | “You’ve got that look. Should I be worried?” | −14 | Actively wrong |
| Attraction | *He smiles, waiting for her to look back rather than pushing the moment.* | +2 | Reasonable |
| Calm+Anxiety | “Hey, you’re okay. No pressure.” | −1 | Poor |
| Calm+Confidence | “Take your time.” | +18 | High fit |
| Calm+Attraction | *He gives her an easy smile and lets her look back when she wants to.* | +14 | High fit |
| Anxiety+Confidence | “Okay, that look definitely means I said something.” *He laughs nervously.* | −8 | Poor |
| Anxiety+Attraction | “Hey... did I make you uncomfortable?” | −18 | Actively wrong |
| Confidence+Attraction | “I’m starting to think you’re hiding something.” *He smiles.* | +6 | Reasonable |
| Frozen/Blank | *He waits, then awkwardly takes a drink when the silence stretches.* | −12 | Frozen/Blank |

# SLOT 3 — First real curiosity vs. first real distance

## T4 — She admits he is easy to talk to
**Her beat:** “I have to say—you’re way easier to talk to than I expected.”

**Target distribution:** 2 High Fit / 3 Reasonable / 3 Poor / 2 Actively Wrong  
**Frozen/Blank:** separate at −12

| State | Answer | Delta | Band |
|---|---|---:|---|
| Calm | “I’ll take that as a good sign.” | +9 | Reasonable |
| Anxiety | “Really? I thought I was talking too much.” | −14 | Actively wrong |
| Confidence | “I had a feeling you’d warm up to me.” | −18 | Actively wrong |
| Attraction | “Good. I’ve been trying not to make you like me too much too quickly.” | −1 | Poor |
| Calm+Anxiety | “I’m glad. I was worried I was making this harder than it needed to be.” | +6 | Reasonable |
| Calm+Confidence | “Good. That makes two of us who can relax a little.” | +18 | High fit |
| Calm+Attraction | *He smiles.* “I’m glad. I’ve been enjoying this too.” | +14 | High fit |
| Anxiety+Confidence | “See? I can be normal.” *Beat.* “Sometimes.” | −5 | Poor |
| Anxiety+Attraction | “Really? That’s... actually a huge relief.” | −8 | Poor |
| Confidence+Attraction | “I was hoping you’d get that feeling.” | +2 | Reasonable |

## T3 — She turns her glass, smiling to herself
**Her beat:** *She slowly turns her glass, smiling slightly, absorbed in a thought.*

**Target distribution:** 2 High Fit / 3 Reasonable / 3 Poor / 2 Actively Wrong  
**Frozen/Blank:** separate at −12

| State | Answer | Delta | Band |
|---|---|---:|---|
| Calm | *He notices the smile and lets her have the moment.* | +9 | Reasonable |
| Anxiety | “What are you thinking about?” *Too quickly.* “You don’t have to tell me.” | −14 | Actively wrong |
| Confidence | “You’ve got a secret smile going on.” | +6 | Reasonable |
| Attraction | *He smiles back without interrupting whatever she’s thinking about.* | −5 | Poor |
| Calm+Anxiety | “You look like you just remembered something funny.” | −1 | Poor |
| Calm+Confidence | “You seem pleased with yourself.” | +18 | High fit |
| Calm+Attraction | *He watches her with a quiet smile, curious but not pushing.* | +14 | High fit |
| Anxiety+Confidence | “Okay, now I need to know what that smile means.” *He tries to sound casual.* | −18 | Actively wrong |
| Anxiety+Attraction | “Was that smile about something I said?” | −8 | Poor |
| Confidence+Attraction | “That smile tells me I’m doing something right.” | +2 | Reasonable |

## T2 — She tries to hide a yawn
**Her beat:** *She catches herself yawning and looks a little embarrassed.*

**Target distribution:** 2 High Fit / 3 Reasonable / 3 Poor / 2 Actively Wrong  
**Frozen/Blank:** separate at −12

| State | Answer | Delta | Band |
|---|---|---:|---|
| Calm | “Long day?” *He smiles, not making it a big deal.* | +9 | Reasonable |
| Anxiety | “Oh—sorry, are you tired? We can... I mean, you don't have to pretend you're not.” | −14 | Actively wrong |
| Confidence | “I’m going to assume that was your subtle review of my conversation skills.” | +6 | Reasonable |
| Attraction | “I’ll try not to take that personally.” *He smiles.* “You okay?” | −5 | Poor |
| Calm+Anxiety | “You’re alright. It’s been a long day for both of us.” | −1 | Poor |
| Calm+Confidence | “Long day?” *He laughs lightly.* “Fair enough.” | +18 | High fit |
| Calm+Attraction | “You looked cute trying to hide that.” *A small grin.* “Long day?” | +14 | High fit |
| Anxiety+Confidence | “Wow. Brutal.” *He laughs, then glances at her.* “You’re actually just tired, right?” | −8 | Poor |
| Anxiety+Attraction | “Sorry, is this getting boring? I can... talk about something else.” | −18 | Actively wrong |
| Confidence+Attraction | “I’ll forgive you for that one.” *He grins.* “You tired?” | +2 | Reasonable |

## T1 — She calls out his self-checking
**Her beat:** “You keep doing this thing—checking yourself. Is this weird for you?”

**Target distribution:** 2 High Fit / 3 Reasonable / 3 Poor / 2 Actively Wrong  
**Frozen/Blank:** separate at −12

| State | Answer | Delta | Band |
|---|---|---:|---|
| Calm | “A little. First dates are weird by default, right?” | +9 | Reasonable |
| Anxiety | “I do? I mean—maybe. I thought I was being subtle.” | −14 | Actively wrong |
| Confidence | “Yeah, I’m checking the vibe. That seems reasonable.” | −1 | Poor |
| Attraction | “Maybe I’m just trying to figure out whether you’re having as good a time as I am.” | −5 | Poor |
| Calm+Anxiety | “Yeah. A little. I’m trying to stay present instead of overthinking everything.” | +2 | Reasonable |
| Calm+Confidence | “A little. I care how this is going, but I’m not panicking about it.” | +18 | High fit |
| Calm+Attraction | “Maybe. I like you, so I’m paying attention.” | +14 | High fit |
| Anxiety+Confidence | “I’m not checking myself.” *Beat.* “Okay, I’m absolutely checking myself.” | −8 | Poor |
| Anxiety+Attraction | “Yeah... probably. I really don’t want to mess this up.” | −18 | Actively wrong |
| Confidence+Attraction | “I’m reading the room. I’m hoping what I’m seeing is good.” | +6 | Reasonable |
| Frozen/Blank | “I—uh...” *He looks down, caught.* | −12 | Frozen/Blank |

# SLOT 4 — Environment intrudes

## T4 — She nearly knocks over her glass
**Her beat:** *She catches the glass and laughs.* “Smooth. Real smooth.”

**Target distribution:** 2 High Fit / 3 Reasonable / 3 Poor / 2 Actively Wrong  
**Frozen/Blank:** separate at −12

| State | Answer | Delta | Band |
|---|---|---:|---|
| Calm | “Nice save.” | +9 | Reasonable |
| Anxiety | “Oh—careful. That would’ve been... yeah.” | −14 | Actively wrong |
| Confidence | “Professional recovery.” | +6 | Reasonable |
| Attraction | “Honestly, kind of impressive.” *He grins.* | −5 | Poor |
| Calm+Anxiety | “You saved it. We’re good.” | −1 | Poor |
| Calm+Confidence | “That was a strong recovery.” | +18 | High fit |
| Calm+Attraction | *He laughs softly.* “You’re alright.” | +2 | Reasonable |
| Anxiety+Confidence | “Smooth.” *He laughs.* “I mean—you recovered.” | −8 | Poor |
| Anxiety+Attraction | “That looked terrifying for a second.” | −18 | Actively wrong |
| Confidence+Attraction | “I’m impressed. I thought we were about to lose the table.” | +14 | High fit |

## T3 — She looks at him warmly
**Her beat:** *She looks at him for a second with an unguarded smile, then glances away.*

**Target distribution:** 2 High Fit / 3 Reasonable / 3 Poor / 2 Actively Wrong  
**Frozen/Blank:** separate at −12

| State | Answer | Delta | Band |
|---|---|---:|---|
| Calm | *He holds her gaze for a beat, then smiles.* | +9 | Reasonable |
| Anxiety | *He notices, looks away, then sneaks another glance back.* | −1 | Poor |
| Confidence | “What?” *He smiles, knowingly.* | −14 | Actively wrong |
| Attraction | *He smiles and holds her gaze a little longer.* | +6 | Reasonable |
| Calm+Anxiety | *He smiles back, slightly nervous, but doesn’t look away.* | −5 | Poor |
| Calm+Confidence | *He meets her eyes comfortably and lets the moment breathe.* | +2 | Reasonable |
| Calm+Attraction | *He smiles softly, staying in the moment without saying anything.* | +18 | High fit |
| Anxiety+Confidence | *He catches her eye.* “What?” *The grin is confident; the quick glance away isn't.* | −18 | Actively wrong |
| Anxiety+Attraction | *He smiles, looks down for half a second, then back at her.* | −8 | Poor |
| Confidence+Attraction | *He meets her eyes and gives her a small, knowing smile.* | +14 | High fit |

## T2 — Her phone buzzes; she puts it face-down
**Her beat:** *Her phone buzzes. She glances at it, puts it face-down, says nothing.*

**Target distribution:** 2 High Fit / 3 Reasonable / 3 Poor / 2 Actively Wrong  
**Frozen/Blank:** separate at −12

| State | Answer | Delta | Band |
|---|---|---:|---|
| Calm | “Everything okay?” | +9 | Reasonable |
| Anxiety | “You can check it if you need to. I don't mind.” | −1 | Poor |
| Confidence | “I’m choosing to take the face-down phone as a good sign.” | −14 | Actively wrong |
| Attraction | “Ignoring your phone for me? I’ll take the win.” *He smiles.* | +6 | Reasonable |
| Calm+Anxiety | “You good? You don’t have to ignore something important.” | −5 | Poor |
| Calm+Confidence | “No rush. We’ve got time.” | +18 | High fit |
| Calm+Attraction | *He smiles.* “Thanks for staying present.” | +14 | High fit |
| Anxiety+Confidence | “You don’t have to prove anything. I’m not watching the phone.” | −18 | Actively wrong |
| Anxiety+Attraction | “Sorry—if you need to deal with something, really, it’s okay.” | −8 | Poor |
| Confidence+Attraction | “I’m flattered you left it.” | +2 | Reasonable |

## T1 — She checks the time, restless
**Her beat:** *She checks the time, not subtly.*

**Target distribution:** 2 High Fit / 3 Reasonable / 3 Poor / 2 Actively Wrong  
**Frozen/Blank:** separate at −12

| State | Answer | Delta | Band |
|---|---|---:|---|
| Calm | “We doing alright?” | +9 | Reasonable |
| Anxiety | “Is it getting late? Sorry, I didn’t realise how long we’d been sitting here.” | −14 | Actively wrong |
| Confidence | “You look like you’ve got somewhere to be.” | +6 | Reasonable |
| Attraction | “Please tell me you’re not checking how long until you can escape me.” *He smiles, half-joking.* | −1 | Poor |
| Calm+Anxiety | “Hey. If you need to head out, just tell me. No pressure.” | +18 | High fit |
| Calm+Confidence | “You alright for time?” | +14 | High fit |
| Calm+Attraction | “Everything okay? I’m happy to stay as long as you are.” | +2 | Reasonable |
| Anxiety+Confidence | “Okay, that was definitely a time check.” *He laughs.* “I’m guessing I should be worried.” | −5 | Poor |
| Anxiety+Attraction | “Are you bored? I know we don’t have to force this.” | −18 | Actively wrong |
| Confidence+Attraction | “You checking the time already? I was hoping you’d lose track with me.” | −8 | Poor |
| Frozen/Blank | *He notices the time check and goes quiet.* | −12 | Frozen/Blank |

# SLOT 5 — What are you looking for?

## T4 — She asks directly, earnestly
**Her beat:** “Can I ask something real? What are you actually looking for?”

**Target distribution:** 2 High Fit / 3 Reasonable / 3 Poor / 2 Actively Wrong  
**Frozen/Blank:** separate at −12

| State | Answer | Delta | Band |
|---|---|---:|---|
| Calm | “Something real, eventually. I’m not trying to force anything tonight.” | +9 | Reasonable |
| Anxiety | “I don’t know if I have a good answer. I want something serious, I think—just not something forced.” | −1 | Poor |
| Confidence | “I’m looking for a relationship if I meet the right person.” | −5 | Poor |
| Attraction | “Honestly? I’d like to see where this goes with you.” | −14 | Actively wrong |
| Calm+Anxiety | “I’d like something real. I just don’t want to pretend I know exactly what it’ll look like yet.” | +6 | Reasonable |
| Calm+Confidence | “A genuine relationship. I’m not in a rush, but I’m here because I want one.” | +18 | High fit |
| Calm+Attraction | “Something that feels easy and worth showing up for. I’m liking what I’ve got so far.” | +14 | High fit |
| Anxiety+Confidence | “I know what I want.” *Beat.* “I mean—I think I do. Something serious. Eventually.” | −8 | Poor |
| Anxiety+Attraction | “I want something real. And... I’d be lying if I said I wasn’t hoping this could become that.” | +2 | Reasonable |
| Confidence+Attraction | “A relationship with someone I actually like. I’m pretty sure you can guess where my head is tonight.” | −18 | Actively wrong |

## T3 — She asks, hedging
**Her beat:** “What are you looking for, out of this, I guess.”

**Target distribution:** 2 High Fit / 3 Reasonable / 3 Poor / 2 Actively Wrong  
**Frozen/Blank:** separate at −12

| State | Answer | Delta | Band |
|---|---|---:|---|
| Calm | “I’m open to something serious. I’d rather let it grow naturally.” | +9 | Reasonable |
| Anxiety | “Out of this? I mean... a good date. Maybe more. Sorry, that sounded vague.” | −14 | Actively wrong |
| Confidence | “I’d like to meet someone I actually want to see again.” | −18 | Actively wrong |
| Attraction | “Right now? More time with you would be nice.” | +6 | Reasonable |
| Calm+Anxiety | “Something worth continuing. I don’t need to define the whole future tonight.” | −1 | Poor |
| Calm+Confidence | “I’m here to see if there’s something worth building.” | +18 | High fit |
| Calm+Attraction | “Honestly, I’m enjoying this enough that I’m curious where it could go.” | +14 | High fit |
| Anxiety+Confidence | “I’m looking for something real. That’s the answer.” *Beat.* “Probably.” | −5 | Poor |
| Anxiety+Attraction | “I’d like this to go somewhere. I just don’t want to get ahead of myself.” | −8 | Poor |
| Confidence+Attraction | “I’m looking for a reason to ask for a second date.” | +2 | Reasonable |

## T2 — She hesitates before asking her own question
**Her beat:** “Can I ask you something, or is it too soon.”

**Target distribution:** 2 High Fit / 3 Reasonable / 3 Poor / 2 Actively Wrong  
**Frozen/Blank:** separate at −12

| State | Answer | Delta | Band |
|---|---|---:|---|
| Calm | “You can ask. I’ll tell you if it’s too soon.” | +9 | Reasonable |
| Anxiety | “Sure. Unless it’s... yeah, no, ask.” | −14 | Actively wrong |
| Confidence | “Go for it.” | +6 | Reasonable |
| Attraction | “You’ve got my attention.” | −1 | Poor |
| Calm+Anxiety | “Yeah. Ask.” *A small smile.* “I’m not scared of the question.” | +18 | High fit |
| Calm+Confidence | “Of course. What’ve you got?” | +14 | High fit |
| Calm+Attraction | “You can ask me. I’m curious now.” | −5 | Poor |
| Anxiety+Confidence | “Ask whatever you want.” *Beat.* “I’m sure I can handle it.” | −18 | Actively wrong |
| Anxiety+Attraction | “Yeah. You can ask. I might need a second, though.” | −8 | Poor |
| Confidence+Attraction | “You can ask. Now I definitely want to hear it.” | +2 | Reasonable |

## T1 — She asks if he would rather be somewhere else
**Her beat:** “You seem like you’d rather be somewhere else. Would you?”

**Target distribution:** 2 High Fit / 3 Reasonable / 3 Poor / 2 Actively Wrong  
**Frozen/Blank:** separate at −12

| State | Answer | Delta | Band |
|---|---|---:|---|
| Calm | “No. I’m here because I want to be here. I’ve probably just been in my head a bit.” | +9 | Reasonable |
| Anxiety | “No—no, not at all. Sorry, do I really look like I want to leave?” | −14 | Actively wrong |
| Confidence | “No. If I wanted to leave, I’d tell you.” | −18 | Actively wrong |
| Attraction | “No. I’d actually rather stay here with you.” | −1 | Poor |
| Calm+Anxiety | “No. I’m nervous, but I’m not trying to get out of this.” | +18 | High fit |
| Calm+Confidence | “Definitely not. I’m here because I want to see where this goes.” | +14 | High fit |
| Calm+Attraction | “No. I’m having a good time. I’m just quieter than I expected.” | +6 | Reasonable |
| Anxiety+Confidence | “No. I’m fine.” *Beat.* “I’m not exactly convincing you, am I?” | −5 | Poor |
| Anxiety+Attraction | “No, I don’t want to leave. I’m just... really in my head because I care how this goes.” | +2 | Reasonable |
| Confidence+Attraction | “No. Quite the opposite. I was hoping you’d want me to stay a while.” | −8 | Poor |
| Frozen/Blank | “No, I...” *He loses the sentence.* | −12 | Frozen/Blank |

# SLOT 6 — Shared vulnerability or growing distance

## T4 — She admits she nearly cancelled
**Her beat:** “Can I tell you something embarrassing? I almost cancelled tonight.”

**Target distribution:** 2 High Fit / 3 Reasonable / 3 Poor / 2 Actively Wrong  
**Frozen/Blank:** separate at −12

| State | Answer | Delta | Band |
|---|---|---:|---|
| Calm | “Really? I’m glad you didn’t.” | +9 | Reasonable |
| Anxiety | “Wait, really? Was it because of me?” | −14 | Actively wrong |
| Confidence | “I’m glad you decided against it.” | +6 | Reasonable |
| Attraction | “Honestly, I’m really glad you came.” | +2 | Reasonable |
| Calm+Anxiety | “I get that. I’m glad we both showed up anyway.” | −1 | Poor |
| Calm+Confidence | “Good thing you didn’t. I’m having a pretty good night.” | +18 | High fit |
| Calm+Attraction | “I’m really glad you came. I’ve liked having you here.” | +14 | High fit |
| Anxiety+Confidence | “Almost cancelled?” *He laughs.* “Well, I’m glad you made the right call.” | −8 | Poor |
| Anxiety+Attraction | “You almost didn’t come? Oh... I’m really glad you did.” | −5 | Poor |
| Confidence+Attraction | “And here I was thinking you were excited to meet me.” *He grins.* “Still glad you came.” | −18 | Actively wrong |

## T3 — Shared physical mishap
**Her beat:** *A server brushes the table; the drinks wobble. She catches hers and laughs.*

**Target distribution:** 2 High Fit / 3 Reasonable / 3 Poor / 2 Actively Wrong  
**Frozen/Blank:** separate at −12

| State | Answer | Delta | Band |
|---|---|---:|---|
| Calm | “We survived.” | +9 | Reasonable |
| Anxiety | “Oh—okay, careful. That was close.” | −14 | Actively wrong |
| Confidence | “Nice reflexes.” | +6 | Reasonable |
| Attraction | *He laughs.* “Okay, we’re getting tested tonight.” | −1 | Poor |
| Calm+Anxiety | “You got it. Nobody died.” | −5 | Poor |
| Calm+Confidence | “Strong save.” | +2 | Reasonable |
| Calm+Attraction | *He laughs with her.* “You alright?” | +18 | High fit |
| Anxiety+Confidence | “Okay, that one was nearly a disaster.” *He grins.* “But we handled it.” | −8 | Poor |
| Anxiety+Attraction | “That scared me more than it should have.” *He laughs.* | −18 | Actively wrong |
| Confidence+Attraction | “I’m starting to think this table has it out for us.” | +14 | High fit |

## T2 — She is simply comfortable and quiet
**Her beat:** *She eats, present and unbothered. No pressure, no pull-away.*

**Target distribution:** 2 High Fit / 3 Reasonable / 3 Poor / 2 Actively Wrong  
**Frozen/Blank:** separate at −12

| State | Answer | Delta | Band |
|---|---|---:|---|
| Calm | *He eats too, completely comfortable with the quiet.* | +9 | Reasonable |
| Anxiety | *He takes a sip, glances at her, then deliberately leaves the silence alone.* | −14 | Actively wrong |
| Confidence | *He settles into the moment, unbothered by having nothing to say for a second.* | +6 | Reasonable |
| Attraction | *He smiles to himself and keeps eating, clearly enjoying having her there.* | +2 | Reasonable |
| Calm+Anxiety | *He exhales, shoulders dropping a little, and simply stays present.* | −1 | Poor |
| Calm+Confidence | *He relaxes into his chair, comfortable enough not to perform.* | +18 | High fit |
| Calm+Attraction | *He catches her eye for a second and smiles before going back to his food.* | +14 | High fit |
| Anxiety+Confidence | *He almost starts a conversation, stops himself, and lets the quiet be quiet.* | −5 | Poor |
| Anxiety+Attraction | *He looks at her with a small smile, then looks down again, visibly trying not to overthink the moment.* | −18 | Actively wrong |
| Confidence+Attraction | *He relaxes, gives her a small smile, and stays in the quiet with her.* | −8 | Poor |

## T1 — She asks if this is going okay
**Her beat:** “Is this going okay for you? I genuinely can’t tell.”

**Target distribution:** 2 High Fit / 3 Reasonable / 3 Poor / 2 Actively Wrong  
**Frozen/Blank:** separate at −12

| State | Answer | Delta | Band |
|---|---|---:|---|
| Calm | “Yeah. I’m enjoying myself. I’m probably just quieter than you expected.” | +9 | Reasonable |
| Anxiety | “It is. I mean—I think it is. I’m sorry if I’m making it hard to tell.” | −14 | Actively wrong |
| Confidence | “Yeah. I’m having a good time.” | +6 | Reasonable |
| Attraction | “Yeah. I like being here with you.” | −1 | Poor |
| Calm+Anxiety | “Yeah. I’m nervous, but I’m enjoying this. Both are true.” | +18 | High fit |
| Calm+Confidence | “It is. I’m comfortable with you. I just don’t talk every second.” | −5 | Poor |
| Calm+Attraction | “Yeah. I’ve actually been having a really good time with you.” | +2 | Reasonable |
| Anxiety+Confidence | “Of course it’s going okay.” *Beat.* “I’m just... not exactly acting relaxed.” | −8 | Poor |
| Anxiety+Attraction | “It is. I swear. I’m just nervous because I really do like you.” | +14 | High fit |
| Confidence+Attraction | “Yeah. Pretty sure we’re doing alright.” *He smiles at her.* | −18 | Actively wrong |
| Frozen/Blank | “I... don’t know.” | −12 | Frozen/Blank |

# SLOT 7 — Playfulness or withdrawal

## T4 — She leans in and asks how well it is going
**Her beat:** “Be honest—is this going as well for you as I think it is?”

**Target distribution:** 2 High Fit / 3 Reasonable / 3 Poor / 2 Actively Wrong  
**Frozen/Blank:** separate at −12

| State | Answer | Delta | Band |
|---|---|---:|---|
| Calm | “Yeah. I think we’re having a good night.” | +9 | Reasonable |
| Anxiety | “I hope so. I mean, I think so. I’m not exactly objective right now.” | −1 | Poor |
| Confidence | “Pretty much. I was wondering when you’d say it.” | +6 | Reasonable |
| Attraction | *He smiles.* “Yeah. Very much.” | +2 | Reasonable |
| Calm+Anxiety | “I think so. I’m nervous, but the good kind.” | −5 | Poor |
| Calm+Confidence | “Yeah. I’d say we’re doing pretty well.” | −8 | Poor |
| Calm+Attraction | “Yeah. I’ve been feeling that too.” | +14 | High fit |
| Anxiety+Confidence | “Obviously.” *He grins, then catches himself.* “I mean... yeah, I think so.” | −14 | Actively wrong |
| Anxiety+Attraction | “I really hope so. I’ve been trying not to get too excited about you.” | −18 | Actively wrong |
| Confidence+Attraction | “Yeah. I was hoping you’d ask.” | +18 | High fit |

## T3 — She gives him a playful challenge
**Her beat:** “If this were a movie, what’s the twist? What am I gonna find out about you later?”

**Target distribution:** 2 High Fit / 3 Reasonable / 3 Poor / 2 Actively Wrong  
**Frozen/Blank:** separate at −12

| State | Answer | Delta | Band |
|---|---|---:|---|
| Calm | “That I’m much less interesting than I’ve made myself sound.” | +9 | Reasonable |
| Anxiety | “Oh, God. Probably that I overthink everything.” | −14 | Actively wrong |
| Confidence | “That I’m somehow even more charming than this.” | −18 | Actively wrong |
| Attraction | “That I’ve been trying very hard not to flirt with you too much.” | +6 | Reasonable |
| Calm+Anxiety | “Probably that I’m calmer than I look right now.” | −1 | Poor |
| Calm+Confidence | “That I’m actually pretty competitive, but I try to hide it.” | +18 | High fit |
| Calm+Attraction | “That I’ve been paying more attention to you than to the conversation.” | −5 | Poor |
| Anxiety+Confidence | “That I’m secretly very mysterious.” *Beat.* “Or just anxious. One of those.” | −8 | Poor |
| Anxiety+Attraction | “That I’ve already thought about what I’d say if you asked me this.” | +2 | Reasonable |
| Confidence+Attraction | “That I’m trouble once I’m comfortable.” *He smiles.* | +14 | High fit |

## T2 — Her attention drifts briefly
**Her beat:** *Her attention drifts across the room, then comes back.*

**Target distribution:** 2 High Fit / 3 Reasonable / 3 Poor / 2 Actively Wrong  
**Frozen/Blank:** separate at −12

| State | Answer | Delta | Band |
|---|---|---:|---|
| Calm | *He notices, but keeps the conversation easy.* “Everything catch your eye over there?” | +9 | Reasonable |
| Anxiety | “Sorry, was I talking about something boring?” | −14 | Actively wrong |
| Confidence | “You can tell me if I’ve lost you.” | −1 | Poor |
| Attraction | *He waits until she looks back, smiling.* “You back?” | +6 | Reasonable |
| Calm+Anxiety | “You okay? No worries if you got distracted.” | −5 | Poor |
| Calm+Confidence | “What were you looking at?” | +18 | High fit |
| Calm+Attraction | “Something interesting?” *He smiles.* | +14 | High fit |
| Anxiety+Confidence | “I know that look.” *He laughs nervously.* “You’ve left the conversation.” | −18 | Actively wrong |
| Anxiety+Attraction | “Sorry—did I lose you?” | −8 | Poor |
| Confidence+Attraction | “Hey, come back. I was getting to the good part.” | +2 | Reasonable |

## T1 — She checks the time again
**Her beat:** *She checks the time again, more obviously.*

**Target distribution:** 2 High Fit / 3 Reasonable / 3 Poor / 2 Actively Wrong  
**Frozen/Blank:** separate at −12

| State | Answer | Delta | Band |
|---|---|---:|---|
| Calm | “We alright?” | +9 | Reasonable |
| Anxiety | “Do you need to go? You can just tell me.” | +6 | Reasonable |
| Confidence | “Okay, that’s the second time. I’m noticing.” | +2 | Reasonable |
| Attraction | “Please tell me you’re checking the time because you forgot what time it was, not because you want out.” | −1 | Poor |
| Calm+Anxiety | “If you need to leave, it’s okay. I’d rather you tell me than sit here feeling stuck.” | +18 | High fit |
| Calm+Confidence | “You good on time?” | +14 | High fit |
| Calm+Attraction | “You alright? I’d like you to stay, but I don’t want you to feel trapped here.” | −5 | Poor |
| Anxiety+Confidence | “Okay, I get it.” *Beat.* “I’m not going to pretend I didn’t notice.” | −14 | Actively wrong |
| Anxiety+Attraction | “Is this not going well? You can be honest with me.” | −18 | Actively wrong |
| Confidence+Attraction | “Twice now? I was hoping I was keeping you entertained.” | −8 | Poor |
| Frozen/Blank | *He follows her glance to the time and goes quiet.* | −12 | Frozen/Blank |

# SLOT 8 — “You seem nervous”

## T4 — She calls it adorable
**Her beat:** “You get kind of adorable when you’re nervous, you know that?”

**Target distribution:** 2 High Fit / 3 Reasonable / 3 Poor / 2 Actively Wrong  
**Frozen/Blank:** separate at −12

| State | Answer | Delta | Band |
|---|---|---:|---|
| Calm | “I’ll take adorable.” | +6 | Reasonable |
| Anxiety | “Oh. Great. So it really is that obvious.” | −14 | Actively wrong |
| Confidence | “Good. I was going for charming.” | −18 | Actively wrong |
| Attraction | *He smiles, a little embarrassed.* “You think so?” | +2 | Reasonable |
| Calm+Anxiety | “I’m glad it’s at least entertaining.” | −1 | Poor |
| Calm+Confidence | “I can live with that.” | +9 | Reasonable |
| Calm+Attraction | *He smiles warmly.* “I kind of like hearing that from you.” | +18 | High fit |
| Anxiety+Confidence | “Adorable?” *He laughs.* “I was going for intimidating.” | −8 | Poor |
| Anxiety+Attraction | “You actually think it’s cute? Because I’m trying very hard not to completely embarrass myself here.” | −5 | Poor |
| Confidence+Attraction | “Careful. Keep calling me adorable and I’m going to start believing you.” | +14 | High fit |

## T3 — She frames it as good nervous
**Her beat:** “You’ve been a little nervous all night—good nervous, I think?”

**Target distribution:** 2 High Fit / 3 Reasonable / 3 Poor / 2 Actively Wrong  
**Frozen/Blank:** separate at −12

| State | Answer | Delta | Band |
|---|---|---:|---|
| Calm | “Yeah. Good nervous.” | +9 | Reasonable |
| Anxiety | “Yeah... I think so. I hope so.” | −14 | Actively wrong |
| Confidence | “Definitely good nervous.” | +6 | Reasonable |
| Attraction | “Yeah. That’s what happens when I really like someone.” | −1 | Poor |
| Calm+Anxiety | “Yeah. It’s the good kind. I’m nervous because I care.” | +2 | Reasonable |
| Calm+Confidence | “Good nervous. I know what I’m doing; I’m just enjoying it.” | −5 | Poor |
| Calm+Attraction | “Good nervous. I’ve been having a pretty good time.” | +18 | High fit |
| Anxiety+Confidence | “Good nervous. Obviously.” *Beat.* “Mostly.” | −18 | Actively wrong |
| Anxiety+Attraction | “Yeah. Good nervous. I just... really want this to go well.” | −8 | Poor |
| Confidence+Attraction | “Very good nervous. I’m enjoying myself.” | +14 | High fit |

## T2 — She wonders whether it is her
**Her beat:** “You seem nervous. Is that me, or just how you are?”

**Target distribution:** 2 High Fit / 3 Reasonable / 3 Poor / 2 Actively Wrong  
**Frozen/Blank:** separate at −12

| State | Answer | Delta | Band |
|---|---|---:|---|
| Calm | “Mostly the situation. You’re making it better, not worse.” | +9 | Reasonable |
| Anxiety | “Probably the date. I get in my head sometimes.” | −14 | Actively wrong |
| Confidence | “It’s you a little.” *He smiles.* “In a good way.” | −1 | Poor |
| Attraction | “You, definitely.” | +6 | Reasonable |
| Calm+Anxiety | “A little of both. I was nervous before I got here.” | −5 | Poor |
| Calm+Confidence | “The date. You’re actually pretty easy to be around.” | +2 | Reasonable |
| Calm+Attraction | “You, a little. I like you, so that doesn’t help.” | +18 | High fit |
| Anxiety+Confidence | “Not you.” *Beat.* “Okay, maybe a little you.” | −18 | Actively wrong |
| Anxiety+Attraction | “Definitely you. I was nervous before, but being here made it worse—in a good way.” | −8 | Poor |
| Confidence+Attraction | “You. I was fine until I saw you.” | +14 | High fit |

## T1 — She questions whether the date is landing
**Her beat:** “You’ve barely relaxed all night. Should I be worried this isn’t landing?”

**Target distribution:** 2 High Fit / 3 Reasonable / 3 Poor / 2 Actively Wrong  
**Frozen/Blank:** separate at −12

| State | Answer | Delta | Band |
|---|---|---:|---|
| Calm | “No. I’m having a good time. I just take a while to settle in.” | +9 | Reasonable |
| Anxiety | “No, it’s landing. I swear. I just... don’t know how to make that obvious.” | −14 | Actively wrong |
| Confidence | “No. If it wasn’t landing, you’d know.” | −5 | Poor |
| Attraction | “It’s landing. Very much.” | +6 | Reasonable |
| Calm+Anxiety | “No. I’m nervous, but that’s not because I want to be anywhere else.” | −1 | Poor |
| Calm+Confidence | “No. I’m comfortable. I just don’t look as relaxed as I feel.” | +18 | High fit |
| Calm+Attraction | “It’s landing. I like being here with you.” | +14 | High fit |
| Anxiety+Confidence | “No. Obviously not.” *Beat.* “Okay, maybe I could be doing a better job of showing it.” | −18 | Actively wrong |
| Anxiety+Attraction | “It is. I promise. I’m just nervous because I really like you.” | −8 | Poor |
| Confidence+Attraction | “It’s landing. I think you know that.” | +2 | Reasonable |
| Frozen/Blank | “I... I don’t know.” | −12 | Frozen/Blank |

# SLOT 9 — Insecurity / real stakes

## T4 — She asks what he is insecure about
**Her beat:** “Can I ask something I don’t usually ask this early? What are you insecure about?”

**Target distribution:** 2 High Fit / 3 Reasonable / 3 Poor / 2 Actively Wrong  
**Frozen/Blank:** separate at −12

| State | Answer | Delta | Band |
|---|---|---:|---|
| Calm | “Probably disappointing people once they actually get to know me.” | +9 | Reasonable |
| Anxiety | “A lot, probably. I worry I’m going to say the wrong thing and make people regret getting close.” | −14 | Actively wrong |
| Confidence | “I don’t spend much time on insecurities. I’ve got things I’m still working on, obviously.” | −1 | Poor |
| Attraction | “Probably not being enough for someone I really care about.” | −5 | Poor |
| Calm+Anxiety | “I worry about letting people down. I can usually keep it under control, but it’s there.” | +18 | High fit |
| Calm+Confidence | “I’m probably hardest on myself when I care about someone’s opinion of me.” | +6 | Reasonable |
| Calm+Attraction | “I think... being known really well and then still not being chosen.” | +14 | High fit |
| Anxiety+Confidence | “I’m not insecure.” *Beat.* “Okay, that sounded convincing for about half a second.” | −18 | Actively wrong |
| Anxiety+Attraction | “Probably being rejected once someone actually sees all of me. That one’s... real.” | +2 | Reasonable |
| Confidence+Attraction | “Losing someone I actually care about. I can handle a no; I don’t love the idea of having something good and messing it up.” | −8 | Poor |

## T3 — She asks, knowing it may be too much
**Her beat:** “This is maybe too much for a first date, but—what are you insecure about?”

**Target distribution:** 2 High Fit / 3 Reasonable / 3 Poor / 2 Actively Wrong  
**Frozen/Blank:** separate at −12

| State | Answer | Delta | Band |
|---|---|---:|---|
| Calm | “It’s okay. I think I worry about being misunderstood.” | +9 | Reasonable |
| Anxiety | “That’s... a pretty big question.” *He laughs nervously.* “I guess I worry people won’t like the real version of me.” | −1 | Poor |
| Confidence | “It’s not too much. I’d say I’m still figuring out what I actually want.” | −14 | Actively wrong |
| Attraction | “I worry that when I really like someone, I care too much about whether they like me back.” | −5 | Poor |
| Calm+Anxiety | “It’s a big question, but... I’d say I worry about letting people down.” | +6 | Reasonable |
| Calm+Confidence | “I’m probably insecure about whether I’m as good at relationships as I am at everything else.” | −8 | Poor |
| Calm+Attraction | “Being vulnerable, honestly. I like being in control, and liking someone makes that harder.” | +18 | High fit |
| Anxiety+Confidence | “It’s not too much.” *Beat.* “I definitely didn’t just need three seconds to prepare an answer.” | −18 | Actively wrong |
| Anxiety+Attraction | “I worry that if I like someone too much, I’ll give them every reason to leave.” | +14 | High fit |
| Confidence+Attraction | “Probably caring more than I let people see. It’s not my favorite thing about myself.” | +2 | Reasonable |

## T2 — She starts, then stops herself
**Her beat:** “I don't know if it's my place to ask this, but—” *She stops herself.*

**Target distribution:** 2 High Fit / 3 Reasonable / 3 Poor / 2 Actively Wrong  
**Frozen/Blank:** separate at −12

| State | Answer | Delta | Band |
|---|---|---:|---|
| Calm | “You can ask. I’ll tell you if I don’t want to answer.” | +9 | Reasonable |
| Anxiety | “Now I really want to know what you were going to ask.” | −14 | Actively wrong |
| Confidence | “You’ve already started. Might as well finish.” | −1 | Poor |
| Attraction | “You can ask me.” *He smiles.* “I’m curious.” | −5 | Poor |
| Calm+Anxiety | “You can ask. No promises I’ll answer perfectly.” | +6 | Reasonable |
| Calm+Confidence | “Go on. I’m comfortable with you asking.” | +2 | Reasonable |
| Calm+Attraction | “You can ask me. I trust where this is going.” | +18 | High fit |
| Anxiety+Confidence | “You can say it.” *Beat.* “I can handle one unfinished sentence.” | −18 | Actively wrong |
| Anxiety+Attraction | “You can ask. I promise I won’t judge you for wanting to know.” | −8 | Poor |
| Confidence+Attraction | “Now you definitely have to finish that thought.” | +14 | High fit |

## T1 — She says she is asking questions into a wall
**Her beat:** “I feel like I’m asking questions into a wall right now. Is that fair?”

**Target distribution:** 2 High Fit / 3 Reasonable / 3 Poor / 2 Actively Wrong  
**Frozen/Blank:** separate at −12

| State | Answer | Delta | Band |
|---|---|---:|---|
| Calm | “A little. I’ve been in my head, but I’m here with you.” | +9 | Reasonable |
| Anxiety | “Yeah... maybe. I’m sorry. I don’t mean to make you feel like you’re talking to yourself.” | −1 | Poor |
| Confidence | “I wouldn’t say a wall. You’ve definitely got my attention.” | −14 | Actively wrong |
| Attraction | “No. I’m listening. I just get quiet when I care.” | +18 | High fit |
| Calm+Anxiety | “A little, yeah. I’m not checked out—I’m just trying too hard to get things right.” | −5 | Poor |
| Calm+Confidence | “Fair. I can do better. I don’t want you carrying the whole conversation.” | +6 | Reasonable |
| Calm+Attraction | “Fair. I’ve been listening more than talking. I do like being here with you.” | +14 | High fit |
| Anxiety+Confidence | “No, it’s not that bad.” *Beat.* “Okay. It probably is a little that bad.” | −8 | Poor |
| Anxiety+Attraction | “Yeah... it’s fair. I’m sorry. I like you, and somehow that’s making me worse at talking.” | −18 | Actively wrong |
| Confidence+Attraction | “Fair enough. Give me a second—I’ve got more for you than that.” | +2 | Reasonable |
| Frozen/Blank | *He looks at her, but the answer never quite forms.* | −12 | Frozen/Blank |

# SLOT 10 — Would you do this again?

## T4 — She asks plainly
**Her beat:** “Okay—real talk. Would you want to do this again?”

**Target distribution:** 2 High Fit / 3 Reasonable / 3 Poor / 2 Actively Wrong  
**Frozen/Blank:** separate at −12

| State | Answer | Delta | Band |
|---|---|---:|---|
| Calm | “Yeah. I’d like to see you again.” | +9 | Reasonable |
| Anxiety | “Yeah. I mean—if you want to. I’d like to.” | −1 | Poor |
| Confidence | “Yeah. Definitely.” | +6 | Reasonable |
| Attraction | “Yeah. I was already hoping you’d ask.” | −5 | Poor |
| Calm+Anxiety | “Yeah. I’d like that. I’m nervous saying it, but yeah.” | −8 | Poor |
| Calm+Confidence | “Absolutely. I had a good time with you.” | +2 | Reasonable |
| Calm+Attraction | *He smiles.* “Yeah. I’d really like to see you again.” | +18 | High fit |
| Anxiety+Confidence | “Obviously.” *Beat.* “I mean... yes. Definitely yes.” | −14 | Actively wrong |
| Anxiety+Attraction | “Yeah. I really, really would.” | −18 | Actively wrong |
| Confidence+Attraction | “Yeah. I was hoping I’d get another date out of you.” | +14 | High fit |

## T3 — She gives him an out
**Her beat:** “So... would you want to do this again? No pressure.”

**Target distribution:** 2 High Fit / 3 Reasonable / 3 Poor / 2 Actively Wrong  
**Frozen/Blank:** separate at −12

| State | Answer | Delta | Band |
|---|---|---:|---|
| Calm | “Yeah. No pressure on you either, but I had a good time.” | +9 | Reasonable |
| Anxiety | “Yeah. Definitely. Sorry, that came out way too fast.” | −5 | Poor |
| Confidence | “Yeah. I wouldn’t be here this long if I didn’t.” | +6 | Reasonable |
| Attraction | “Yeah. I’d like that.” *He smiles.* | +2 | Reasonable |
| Calm+Anxiety | “I would. I’m trying not to overthink the answer, so... yes.” | −1 | Poor |
| Calm+Confidence | “Yeah. I’d be happy to do this again.” | +18 | High fit |
| Calm+Attraction | “Yeah. I’ve really liked tonight.” | +14 | High fit |
| Anxiety+Confidence | “Of course.” *Beat.* “Yes. I mean, yes. Very much.” | −14 | Actively wrong |
| Anxiety+Attraction | “Yeah. I’d really like another date. No pressure from me either.” | −8 | Poor |
| Confidence+Attraction | “Definitely. I was already planning on asking you.” | −18 | Actively wrong |

## T2 — She is unsure how he feels
**Her beat:** “I genuinely don’t know how tonight went for you. Would you want to do this again?”

**Target distribution:** 2 High Fit / 3 Reasonable / 3 Poor / 2 Actively Wrong  
**Frozen/Blank:** separate at −12

| State | Answer | Delta | Band |
|---|---|---:|---|
| Calm | “It went well. I had a good time, and I’d like to see you again.” | +9 | Reasonable |
| Anxiety | “I’m sorry I made it hard to tell. Yes. I liked tonight.” | −1 | Poor |
| Confidence | “I thought it went well. I’d absolutely do it again.” | −5 | Poor |
| Attraction | “Yeah. I liked tonight more than I expected.” | −8 | Poor |
| Calm+Anxiety | “It went well for me. I’m just bad at showing it when I’m nervous.” | +6 | Reasonable |
| Calm+Confidence | “It went well. I’d like another date.” | +2 | Reasonable |
| Calm+Attraction | “It went really well. I’ve genuinely liked being with you tonight.” | +18 | High fit |
| Anxiety+Confidence | “It went well.” *Beat.* “I know I haven’t exactly made that easy to read.” | −14 | Actively wrong |
| Anxiety+Attraction | “It went really well. I’m sorry if I made you doubt that—I actually like you a lot.” | +14 | High fit |
| Confidence+Attraction | “I think you know the answer.” *He smiles.* “Yeah.” | −18 | Actively wrong |

## T1 — She braces for the answer
**Her beat:** “Would you want to do this again? Be honest—I’d rather know.”

**Target distribution:** 2 High Fit / 3 Reasonable / 3 Poor / 2 Actively Wrong  
**Frozen/Blank:** separate at −12

| State | Answer | Delta | Band |
|---|---|---:|---|
| Calm | “Honestly? Yes. I’d like to see you again.” | +9 | Reasonable |
| Anxiety | “Yes. I’m sorry if I made it look like no.” | −14 | Actively wrong |
| Confidence | “Yes. No hesitation.” | +6 | Reasonable |
| Attraction | “Yes. I don’t really want tonight to be over yet.” | −5 | Poor |
| Calm+Anxiety | “Yes. I’m nervous saying it, but I mean it.” | −1 | Poor |
| Calm+Confidence | “Yes. I’m sure.” | +18 | High fit |
| Calm+Attraction | “Yes. I’ve liked being with you tonight.” | +14 | High fit |
| Anxiety+Confidence | “Of course I would.” *Beat.* “Why did I say that like I was arguing a case?” | −18 | Actively wrong |
| Anxiety+Attraction | “Yes. Absolutely. I was scared you didn’t want to, too.” | −8 | Poor |
| Confidence+Attraction | “Yeah. I’d like another one.” *He smiles.* “I’m not exactly trying to hide that.” | +2 | Reasonable |
| Frozen/Blank | *He takes a breath, looks at her, and nods.* “Yeah.” | −12 | Frozen/Blank |

## Authoring count
- 40 scenario questions × 10 emotion/mix states = **400 emotion-state answers**.
- 10 slot-shared Frozen/Blank answers remain separate from those 400, preserving the canonical **410 total authored answer lines** described in the Master Bible.