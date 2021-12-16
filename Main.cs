using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.Util;
using System;
using System.Collections.Generic;

namespace StorybrewScripts
{
    public class Main : StoryboardObjectGenerator
    {
        OsbSpritePool pool;
        float scale;
        double beat, beat4;
        public override void Generate()
        {
		    GetLayer("Disable map BG").CreateSprite(Beatmap.BackgroundPath).Fade(0,0);

            scale = 480f / 1080f;
            beat  = Beatmap.GetTimingPointAt(0).BeatDuration;
            beat4 = beat*4;

            Background();
            Highlight();
        }

        void Background()
        {
            int time;

            var solid   = GetLayer("White BG").CreateSprite("sb/p.png");
            var overlay = GetLayer("White overlay").CreateSprite("sb/p.png");
            var blur    = GetLayer("Blurred BG").CreateSprite("sb/b.jpg");
            var girl    = GetLayer("Girl").CreateSprite("sb/g.png", OsbOrigin.Centre, new Vector2(370, 252));
            var flare   = GetLayer("Lightning").CreateSprite("sb/f.jpg");
            var light   = GetLayer("Lightning").CreateSprite("sb/l.png");
            var klight  = GetLayer("Lightning").CreateSprite("sb/e.png", OsbOrigin.CentreLeft, new Vector2(-107, 220));
            var slight  = GetLayer("Lightning").CreateSprite("sb/e.png", OsbOrigin.CentreRight, new Vector2(747, 220));
            var ltb1    = GetLayer("Letterbox").CreateSprite("sb/p.png", OsbOrigin.TopCentre, new Vector2(320, 0));
            var ltb2    = GetLayer("Letterbox").CreateSprite("sb/p.png", OsbOrigin.BottomCentre, new Vector2(320, 480));

            // Intro //
            time = -27;
            ltb1.ScaleVec(time, 854, 40);
            ltb1.Color(time, Color4.Black);
            ltb1.Fade(time, 1);
            ltb2.ScaleVec(time, 854, 40);
            ltb2.Color(time, Color4.Black);
            ltb2.Fade(time, 1);
            blur.Scale(time, scale);
            blur.Fade(time, time + beat4*4, 0, 1);

            // Verse //
            time = 11007;
            blur.Fade(time, 0);
            solid.ScaleVec(time - beat4, 854, 480);
            solid.Fade(OsbEasing.In, time - beat4, time, 0, 1);

            // Verse-break //
            time = 33076;
            solid.Fade(OsbEasing.OutExpo, time, time + beat4, 1, .05);

            // Prechorus //
            time = 44110;
            blur.Fade(time, 1);
            solid.Fade(time, 0);
            flare.Additive(time);
            flare.Fade(time, time + beat4*4, 1, 0);
            overlay.ScaleVec(time - beat4*2, 854, 480);
            overlay.Additive(time - beat4*2);
            overlay.Fade(OsbEasing.In, time - beat4*2, time - beat, 0, 1);
            overlay.Fade(OsbEasing.Out, time, time + beat4, 1, 0);

            // Chorus //
            time = 55145;
            ltb1.Fade(time, 0);
            ltb2.Fade(time, 0);
            blur.Fade(time - beat, 0);
            flare.Fade(time - beat4*4, time - beat, 0, 1);
            flare.Fade(time - beat, 0);
            overlay.Fade(OsbEasing.In, time - beat4*2, time - beat, 0, 1);

            Action repeatSidelight = () =>
            {
                klight.StartLoopGroup(time, 4*4-2);
                    klight.Fade(OsbEasing.Out, 0, beat*2, .5, 0);
                    klight.Fade(OsbEasing.Out, beat*2+beat/2, beat4, .5, 0);
                    klight.EndGroup();

                slight.StartLoopGroup(time + beat, 4*4-2);
                    slight.Fade(OsbEasing.Out, 0, beat*2, .5, 0);
                    slight.Fade(OsbEasing.Out, beat*2, beat4, .5, 0);
                    slight.EndGroup();
            };

            klight.ScaleVec(time, 10, 8);
            klight.Additive(time);
            klight.Color(time, Color4.MidnightBlue);
            slight.ScaleVec(time + beat, 10, 8);
            slight.FlipH(time + beat);
            slight.Additive(time + beat);
            slight.Color(time + beat, Color4.DarkRed);

            repeatSidelight();
            klight.Fade(OsbEasing.Out, 74455, 75145, .5, 0);

            overlay.Fade(OsbEasing.OutExpo, time - beat, time, 1, 0);
            overlay.Fade(OsbEasing.Out, time, time + beat4*3, 1, 0);
            overlay.Fade(OsbEasing.In, 66179 - beat4, 66179, 0, .1);
            overlay.Fade(OsbEasing.Out, 66179, 66179 + beat4, .5, 0);
            blur.Fade(time, 1);
            flare.Fade(time, 1);
            light.ScaleVec(time, 854, 1);
            light.StartLoopGroup(time, 2);
                light.Fade(OsbEasing.InOutSine, 0, beat*4*4, .8, .1);
                light.Fade(OsbEasing.InOutSine, beat*4*4, beat*4*8, .1, .8);
                light.EndGroup();
            girl.Scale(time, scale);
            girl.Fade(OsbEasing.Out, time, time + beat4, .5, .8);
            girl.StartLoopGroup(time, 4);
                girl.MoveY(OsbEasing.InOutSine, 0, beat4*4, 250, 260);
                girl.MoveY(OsbEasing.InOutSine, beat4*4, beat4*8, 260, 250);
                girl.EndGroup();
            girl.StartLoopGroup(time, 5);
                girl.Rotate(OsbEasing.InOutSine, 0, beat4*3, -.01, .01);
                girl.Rotate(OsbEasing.InOutSine, beat4*3, beat4*6, .01, -.01);
                girl.EndGroup();

            // Chorus-no kiai //
            time = 77214;
            light.Fade(time, 0);
            flare.Fade(time, .5);
            girl.Fade(time, .7);
            overlay.Fade(OsbEasing.In, time - beat4, time, 0, .3);
            overlay.Fade(OsbEasing.Out, time, time + beat4*3, 1, 0);
            overlay.Fade(OsbEasing.In, 88248 - beat4/2, 88248, 0, .1);
            overlay.Fade(OsbEasing.Out, 88248, 88248 + beat4, .5, 0);

            repeatSidelight();
            klight.Fade(OsbEasing.Out, 96524, 97214, .5, 0);

            // Verse // 
            time = 99283;
            overlay.Fade(time, 0);
            flare.Fade(time, 0);
            blur.Fade(time, 0);
            girl.Fade(time, 0);
            solid.Fade(time, 1);
            overlay.Fade(OsbEasing.In, time - beat4, time, 0, 1);
            ltb1.Fade(OsbEasing.In, time, time + beat4*3, 0, 1);
            ltb2.Fade(OsbEasing.In, time, time + beat4*3, 0, 1);

            // Verse-break //
            time = 121352;
            solid.Fade(OsbEasing.OutExpo, time, time + beat4, 1, .05);

            // Prechorus //
            time = 132386;
            blur.Fade(time, 1);
            solid.Fade(time, 0);
            flare.Fade(time, time + beat4*4, 1, 0);
            overlay.Fade(OsbEasing.In, time - beat4*2, time - beat, 0, 1);
            overlay.Fade(OsbEasing.Out, time, time + beat4, 1, 0);

            // Chorus //
            time = 143421;
            ltb1.Fade(time, 0);
            ltb2.Fade(time, 0);
            blur.Fade(time - beat, 0);
            flare.Fade(time - beat4*4, time - beat, 0, 1);
            flare.Fade(time - beat, 0);
            overlay.Fade(OsbEasing.In, time - beat4*2, time - beat, 0, 1);

            klight.StartLoopGroup(time, 4*2-1);
                klight.Fade(OsbEasing.Out, 0, beat+beat/2, .5, 0);
                klight.Fade(OsbEasing.Out, beat+beat/2, beat4-beat/2, .5, 0);
                klight.Fade(OsbEasing.Out, beat4, beat4+beat/2, .5, .1);
                klight.Fade(OsbEasing.Out, beat4+beat/2, beat4+beat*2-beat/2, .5, .05);
                klight.Fade(OsbEasing.Out, beat4+beat*2-beat/2, beat4*2-beat/2, .5, 0);
                klight.Fade(beat4*2, 0);
                klight.EndGroup();
            klight.Fade(OsbEasing.Out, 162731, 163421, .5, 0);

            slight.StartLoopGroup(time + beat, 4*4-2);
                slight.Fade(OsbEasing.Out, 0, beat*2, .5, 0);
                slight.Fade(OsbEasing.Out, beat*2, beat4, .5, 0);
                slight.EndGroup();

            overlay.Fade(OsbEasing.OutExpo, time - beat, time, 1, 0);
            overlay.Fade(OsbEasing.Out, time, time + beat4*3, 1, 0);
            overlay.Fade(OsbEasing.In, 154455 - beat4, 154455, 0, .1);
            overlay.Fade(OsbEasing.Out, 154455, 154455 + beat4, .5, 0);
            blur.Fade(time, 1);
            flare.Fade(time, 1);
            light.StartLoopGroup(time, 2);
                light.Fade(OsbEasing.InOutSine, 0, beat*4*4, .8, .1);
                light.Fade(OsbEasing.InOutSine, beat*4*4, beat*4*8, .1, .8);
                light.EndGroup();
            girl.Fade(OsbEasing.Out, time, time + beat4, .5, .8);
            girl.StartLoopGroup(time, 6);
                girl.MoveY(OsbEasing.InOutSine, 0, beat4*4, 250, 260);
                girl.MoveY(OsbEasing.InOutSine, beat4*4, beat4*8, 260, 250);
                girl.EndGroup();
            girl.StartLoopGroup(time, 8);
                girl.Rotate(OsbEasing.InOutSine, 0, beat4*3, -.01, .01);
                girl.Rotate(OsbEasing.InOutSine, beat4*3, beat4*6, .01, -.01);
                girl.EndGroup();

            // Chorus-no kiai //
            time = 165490;
            light.Fade(time, 0);
            flare.Fade(time, .5);
            girl.Fade(time, .7);
            overlay.Fade(OsbEasing.In, time - beat4, time, 0, .3);
            overlay.Fade(OsbEasing.Out, time, time + beat4*3, 1, 0);
            overlay.Fade(OsbEasing.In, 176524 - beat4/2, 176524, 0, .1);
            overlay.Fade(OsbEasing.Out, 176524, 176524 + beat4, .5, 0);

            repeatSidelight();
            klight.Fade(OsbEasing.Out, 184800, 185490, .5, 0);

            // Outro //
            time = 187559;
            flare.Fade(time, 0);
            overlay.Fade(OsbEasing.In, time - beat4, time, 0, .5);
            overlay.Fade(OsbEasing.Out, time, time + beat4*3, 1, 0);
            girl.Fade(187559, 206869, .6, 0);
            blur.Fade(OsbEasing.Out, 202731, 209628, 1, 0);
        }
        void Highlight()
        {
            var size = Beatmap.CircleSize/4;

            Action<int, int> glow = (startTime, endTime) => 
            {
                foreach (var circle in Beatmap.HitObjects)
                {
                    var slider = circle is OsuSlider;
                    var cStart = circle.StartTime;
                    var cEnd   = circle.EndTime;
                    
                    if (cStart < startTime - 5 || endTime - 5 <= cStart) continue;

                    var sprite = pool.Get(cStart, cEnd + beat4*2);
                    sprite.Move (cStart, circle.Position);
                    sprite.Scale(cStart, cStart + beat4, size, size*1.2);
                    sprite.Color(cStart, circle.Color);
                    sprite.Fade (OsbEasing.Out, cStart, cStart + beat4 *
                        (slider ? 2 : 1),
                        slider ? .4 : .2, 0);

                    if (slider)
                    {
                        var step = Beatmap.GetTimingPointAt((int)cStart).BeatDuration / 8;
                        var start = cStart;
                        while (true)
                        {
                            var end = start + step;
                            var complete = cEnd - end < 5;
                            if (complete) end = cEnd;

                            var startPosition = sprite.PositionAt(start);
                            sprite.Move(start, end, startPosition, circle.PositionAtTime(end));

                            if (complete) break;
                            start += step;
                        }
                    }
                }
            };

            Action<int, bool> ring = (time, big) =>
            {
                foreach (var circle in Beatmap.HitObjects)
                {
                    if (circle.StartTime < time - 5 || time + 5 <= circle.StartTime) continue;

                    var sprite = pool.Get(circle.StartTime, circle.StartTime + beat4*4);
					sprite.Move (circle.StartTime, circle.Position);
					sprite.Scale(big ? OsbEasing.Out : OsbEasing.OutCubic, circle.StartTime, circle.StartTime + (big ? beat4*5 : beat4*4), .2, (big ? 3 : 2));
					sprite.Fade (OsbEasing.OutExpo, circle.StartTime, circle.StartTime + (big ? beat4*6 : beat4*4), 1, 0);
                }
            };

            using (pool = new OsbSpritePool(GetLayer("Highlight"), "sb/h.png", OsbOrigin.Centre, (s, a, b) => s.Additive(a)))
            {
                glow(44110, 99283);
                glow(132386, 187559);
            }

            using (pool = new OsbSpritePool(GetLayer("Highlight"), "sb/r.png", OsbOrigin.Centre, (s, a, b) => s.Additive(a)))
            {
                foreach (var i in new List<int>{
                    44110, 55145, 66179, 77214, 132386, 143421, 154455, 165490, 187559})
                    ring(i, true);
                foreach (var i in new List<int>{
                    60662, 71697, 88248, 148938, 159973, 176524})
                    ring(i, false);
            }
        }
    }
}
