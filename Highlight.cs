using OpenTK;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.Util;
using System;
using System.Collections.Generic;

using static StorybrewCommon.Assets;

namespace StorybrewScripts
{
    public class Highlight : StoryboardObjectGenerator
    {
        [Configurable] public double HighlightSize   = 3.5;
        [Configurable] public bool   HorizontalStrip = false;

        OsbSpritePool pool;
        double size;

        public override void Generate()
        {
            size = HighlightSize/Beatmap.CircleSize;

            using (pool = new OsbSpritePool(GetLayer(LAYER_HIGHLIGHT2), GLOW, OsbOrigin.Centre, (s, a, b) => s.Additive(a)))
            {
                Glow(44110,  55145,  0f);
                Glow(55145,  77214,  .2f);
                Glow(77214,  99283,  .1f);
                Glow(132386, 143421,  0f);
                Glow(143421, 165490, .2f);
                Glow(165490, 187559, .1f);
            }

            using (pool = new OsbSpritePool(GetLayer(LAYER_HIGHLIGHT2), RING, OsbOrigin.Centre, (s, a, b) => s.Additive(a)))
            {
                foreach (var i in new List<int>{
                    44110, 55145, 66179, 77214, 132386, 143421, 154455, 165490, 187559})
                    Ring(i, true);

                foreach (var i in new List<int>{
                    88248, 60662, 71697, 148938, 159973, 176524})
                    Ring(i);
            }

            using (pool = new OsbSpritePool(GetLayer(LAYER_HIGHLIGHT1), PIXEL, OsbOrigin.Centre, (s, a, b) => s.Additive(a)))
            {
                for (var i=45490; i<46869; i+=BEAT)
                    Strip(i-10, i+10, false);

                for (var i=48248; i<49628; i+=BEAT)
                    Strip(i-10, i+10, false);

                for (var i=51007; i<52214; i+=BEAT)
                    Strip(i-10, i+10, false);

                Strip(52386, 53766);
                Strip(57904, 60748);
                Strip(63421, 66266);
                Strip(68938, 71783);
                Strip(74455, 75835);

                for (var i=133766; i<135145; i+=BEAT)
                    Strip(i-10, i+10, false);

                for (var i=136524; i<137904; i+=BEAT)
                    Strip(i-10, i+10, false);

                for (var i=139283; i<140490; i+=BEAT)
                    Strip(i-10, i+10, false);

                Strip(140662, 142041);
                Strip(146179, 149024);
                Strip(151697, 154541);
                Strip(157214, 160059);
                Strip(162731, 164110);
            }
        }

        void Glow(int startTime, int endTime, float brightness)
        {
            foreach (var circle in Beatmap.HitObjects)
            {
                var slider = circle is OsuSlider;
                var cStart = circle.StartTime;
                var cEnd   = circle.EndTime;

                if (cStart < startTime - 5 || endTime - 5 <= cStart) continue;

                var s = pool.Get(cStart, cEnd + BEAT4 / (slider ? 2 : 1));
                s.Move  (cStart, circle.Position);
                s.Scale (cStart, cStart + BEAT4/2, size, size*1.2);

                var bright = (slider ? .6 : .4) + brightness;
                if (slider) {
                    if (s.OpacityAt(cStart) == 0) s.Fade(cStart, bright);
                    s.Fade(OsbEasing.Out, cEnd, cEnd + (BEAT*2), bright, 0);
                } else {
                    s.Fade(OsbEasing.Out, cStart, cStart + (BEAT*2), bright, 0);
                }

                if (!s.ColorAt(cStart).Equals(circle.Color))
                    s.Color(cStart, circle.Color);

                if (slider)
                {
                    var step  = BEAT/8;
                    var start = cStart;
                    while (true)
                    {
                        var end  = start + step;
                        var done = cEnd - end < 5;
                        if (done) end = cEnd;

                        s.Move(start, end, s.PositionAt(start), circle.PositionAtTime(end));

                        if (done) break;
                        start += step;
                    }
                }
            }
        }

        void Ring(int time, bool big = false)
        {
            foreach (var circle in Beatmap.HitObjects)
            {
                if (circle.StartTime < time - 5 || time + 5 <= circle.StartTime) continue;

                var s = pool.Get(circle.StartTime, circle.StartTime + BEAT4*4);
                s.Move  (circle.StartTime, circle.Position);
                s.Scale (big ? OsbEasing.Out : OsbEasing.OutCubic, circle.StartTime, circle.StartTime +	(big ? BEAT4*5 : BEAT4*4), .2, (big ? 3 : 2));
                s.Fade  (OsbEasing.OutExpo, circle.StartTime, circle.StartTime + (big ? BEAT4*6 : BEAT4*4), 1, 0);
            }
        }

        void Strip(int startTime, int endTime, bool kiai = true)
        {
            var lastPos  = new Vector2(240, 320);
            var lastDir  = 0d;
            var lastTime = (double)startTime;
            var scale    = kiai ? 40 : 60;
            var fade     = kiai ? .4f : .3f;
            foreach (var circle in Beatmap.HitObjects)
            {
                var slider = circle is OsuSlider;
                var cStart = circle.StartTime;
                var cEnd   = circle.EndTime;

                if (cStart < startTime - 5 || endTime - 5 <= cStart) continue;

                var angle = 0d;

                if (kiai) {
                    angle =
                        Math.Sqrt(Math.Pow(lastPos.X - circle.Position.X, 2) + Math.Pow(lastPos.Y - circle.Position.Y, 2)) > 10 ?
                        Random(-0.3f, 0.3f) + Math.PI/2 :
                        lastDir - 0.1;
                } else {
                    angle =
                        Math.Atan2(circle.Position.Y - lastPos.Y, circle.Position.X - lastPos.X) + Math.PI/2;
                }

                var sprite = pool.Get(cStart, cStart + BEAT4);
                sprite.Move     (cStart, circle.Position);
                sprite.Rotate   (cStart, angle);
                sprite.ScaleVec (OsbEasing.OutQuint, cStart, cStart + BEAT4, 1400, scale * size, 1400, 0);
                sprite.Fade     (OsbEasing.OutExpo, cStart, cStart + BEAT4, fade, .05);

                scale    = Math.Min(60,  scale + 5);
                fade     = Math.Min(.8f, fade + .04f);
                lastPos  = circle.Position;
                lastDir  = angle;
                lastTime = cStart;
            }
        }
    }
}
