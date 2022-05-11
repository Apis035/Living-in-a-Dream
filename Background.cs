using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using System;

using static StorybrewCommon.Assets;

namespace StorybrewScripts
{
    public class Background : StoryboardObjectGenerator
    {
        [Configurable] public double SidelightIntensity = 0.5;

        int t; void setTime(int T) => t = T;

        public override void Generate()
        {
            var blur   = GetLayer(LAYER_BG).CreateSprite(BLURBG);
            var girl   = GetLayer(LAYER_GIRL).CreateSprite(GIRL,       OsbOrigin.Centre,       new Vector2(370, 252));
            var flare  = GetLayer(LAYER_LIGHT).CreateSprite(FLARE);
            var bLight = GetLayer(LAYER_LIGHT).CreateSprite(BOTTOMLIGHT);
            var kLight = GetLayer(LAYER_LIGHT).CreateSprite(SIDELIGHT, OsbOrigin.CentreLeft,   new Vector2(-107, 220));
            var sLight = GetLayer(LAYER_LIGHT).CreateSprite(SIDELIGHT, OsbOrigin.CentreRight,  new Vector2(747, 220));
            var ltb1   = GetLayer(LAYER_LETTERBOX).CreateSprite(PIXEL, OsbOrigin.TopCentre,    new Vector2(320, -50));
            var ltb2   = GetLayer(LAYER_LETTERBOX).CreateSprite(PIXEL, OsbOrigin.BottomCentre, new Vector2(320, 530));

            var SI = SidelightIntensity;

            // Intro //
            setTime         (-27);
            ltb1.ScaleVec   (t, 854, 90);
            ltb2.ScaleVec   (t, 854, 90);
            ltb1.Color      (t, Color4.Black);
            ltb2.Color      (t, Color4.Black);
            ltb1.Fade       (t, 1);
            ltb2.Fade       (t, 1);
            blur.Scale      (t, 480.0f / 1080.0f);
            blur.Fade       (t, t + BEAT4*4, 0, .8);

            // Verse //
            setTime         (11007);
            blur.Fade       (t, 0);

            // Prechorus //
            setTime         (44110);
            blur.Fade       (t, .8);
            flare.Additive  (t);
            flare.Fade      (t, t + BEAT4*4, .4, 0);

            // Chorus //
            Action letterbox = () =>
            {
                ltb1.ScaleVec   (OsbEasing.InExpo, t - BEAT4, t - BEAT, 854, 90, 950, 290);
                ltb2.ScaleVec   (OsbEasing.InExpo, t - BEAT4, t - BEAT, 854, 90, 950, 290);
                ltb1.Rotate     (OsbEasing.InExpo, t - BEAT4, t - BEAT, 0, -.2);
                ltb2.Rotate     (OsbEasing.InExpo, t - BEAT4, t - BEAT, 0, -.2);
                ltb1.Fade       (OsbEasing.In, t - BEAT4, t - BEAT, 1, 0);
                ltb2.Fade       (OsbEasing.In, t - BEAT4, t - BEAT, 1, 0);
            };

            Action repeatSidelight = () =>
            {
                kLight.StartLoopGroup   (t, 4*4 - 2);
                    kLight.Fade         (OsbEasing.Out, 0, BEAT*2, SI, 0);
                    kLight.Fade         (OsbEasing.Out, BEAT*2 + BEAT/2, BEAT4, SI, 0);
                    kLight.EndGroup     ();

                sLight.StartLoopGroup   (t + BEAT, 4*4 - 2);
                    sLight.Fade         (OsbEasing.Out, 0, BEAT*2, SI, 0);
                    sLight.Fade         (OsbEasing.Out, BEAT*2, BEAT4, SI, 0);
                    sLight.EndGroup     ();
            };

            setTime         (55145);
            letterbox       ();
            blur.Fade       (t - BEAT, 0);
            flare.Fade      (t - BEAT4*4, t - BEAT, 0, .4);
            flare.Fade      (t - BEAT, 0);
            kLight.ScaleVec (t, 14, 8);
            kLight.Additive (t);
            kLight.Color    (t, Color4.MidnightBlue);
            sLight.ScaleVec (t + BEAT, 14, 8);
            sLight.FlipH    (t + BEAT);
            sLight.Additive (t + BEAT);
            sLight.Color    (t + BEAT, Color4.DarkRed);
            repeatSidelight ();

            kLight.Fade     (OsbEasing.Out, 74455, 75145, SI, 0);
            blur.Fade       (t, .8);
            flare.Fade      (t, .3);
            bLight.ScaleVec (t, 854, 1);

            bLight.StartLoopGroup   (t, 2);
                bLight.Fade         (OsbEasing.InOutSine, 0, BEAT4 * 4, .4, .1);
                bLight.Fade         (OsbEasing.InOutSine, BEAT4 * 4, BEAT4 * 8, .1, .4);
                bLight.EndGroup     ();

            Action<int, int> girlFade = (time, duration) => girl.Fade(time, time + BEAT4*duration, 0, .6);

            girl.Scale      (t, 480.0f / 1080.0f);
            girlFade        (t, 6);
            girlFade        (66179, 2);

            girl.StartLoopGroup     (t, 4);
                girl.MoveY          (OsbEasing.InOutSine, 0, BEAT4*4, 245, 260);
                girl.MoveY          (OsbEasing.InOutSine, BEAT4*4, BEAT4*8, 260, 245);
                girl.EndGroup       ();

            girl.StartLoopGroup     (t, 5);
                girl.Rotate         (OsbEasing.InOutSine, 0, BEAT4*3, -.01, .01);
                girl.Rotate         (OsbEasing.InOutSine, BEAT4*3, BEAT4*6, .01, -.01);
                girl.EndGroup       ();

            // Chorus (no kiai) //
            setTime         (77214);
            bLight.Fade     (t, 0);
            flare.Fade      (t, .1);
            girlFade        (t, 6);
            repeatSidelight ();
            kLight.Fade     (OsbEasing.Out, 96524, 97214, SI, 0);

            // Verse // 
            setTime         (99283);
            flare.Fade      (t, 0);
            blur.Fade       (t, 0);
            girl.Fade       (t, 0);
            ltb1.ScaleVec   (t, 854, 90);
            ltb2.ScaleVec   (t, 854, 90);
            ltb1.Rotate     (t, 0);
            ltb2.Rotate     (t, 0);
            ltb1.Fade       (OsbEasing.In, t, t + BEAT4*3, 0, 1);
            ltb2.Fade       (OsbEasing.In, t, t + BEAT4*3, 0, 1);

            // Verse (break) //

            // Prechorus //
            setTime         (132386);
            blur.Fade       (t, .8);
            flare.Fade      (t, t + BEAT4*4, .4, 0);

            // Chorus //
            setTime         (143421);
            letterbox       ();
            blur.Fade       (t - BEAT, 0);
            flare.Fade      (t - BEAT4*4, t - BEAT, 0, .4);
            flare.Fade      (t - BEAT, 0);

            kLight.StartLoopGroup   (t, 4*2 - 1);
                kLight.Fade         (OsbEasing.Out, 0, BEAT + BEAT/2, SI, 0);
                kLight.Fade         (OsbEasing.Out, BEAT + BEAT/2, BEAT4 - BEAT/2, SI, 0);
                kLight.Fade         (OsbEasing.Out, BEAT4, BEAT4 + BEAT/2, SI, SI/2);
                kLight.Fade         (OsbEasing.Out, BEAT4 + BEAT/2, BEAT4 + BEAT*2 - BEAT/2, SI, SI/4);
                kLight.Fade         (OsbEasing.Out, BEAT4 + BEAT*2 - BEAT/2, BEAT4*2 - BEAT/2, SI, 0);
                kLight.Fade         (BEAT4*2, 0);
                kLight.EndGroup     ();

            kLight.Fade     (OsbEasing.Out, 162731, 163421, SI, 0);

            sLight.StartLoopGroup   (t + BEAT, 4*4 - 2);
                sLight.Fade         (OsbEasing.Out, 0, BEAT*2, SI, 0);
                sLight.Fade         (OsbEasing.Out, BEAT*2, BEAT4, SI, 0);
                sLight.EndGroup     ();

            blur.Fade       (t, .8);
            flare.Fade      (t, .3);
            girlFade        (t, 6);
            girlFade        (154455, 2);

            girl.StartLoopGroup     (t, 6);
                girl.MoveY          (OsbEasing.InOutSine, 0, BEAT4*4, 245, 260);
                girl.MoveY          (OsbEasing.InOutSine, BEAT4*4, BEAT4*8, 260, 245);
                girl.EndGroup       ();
            girl.StartLoopGroup     (t, 8);
                girl.Rotate         (OsbEasing.InOutSine, 0, BEAT4*3, -.01, .01);
                girl.Rotate         (OsbEasing.InOutSine, BEAT4*3, BEAT4*6, .01, -.01);
                girl.EndGroup       ();

            // Chorus- (no kiai) //
            setTime         (165490);
            flare.Fade      (t, .1);
            girlFade        (t, 6);
            repeatSidelight ();
            kLight.Fade     (OsbEasing.Out, 184800, 185490, SI, 0);

            // Outro //
            setTime         (187559);
            flare.Fade      (t, 0);
            girl.Fade       (187559, 209628, .5, 0);
            blur.Fade       (OsbEasing.Out, 202731, 209628, .8, 0);
        }
    }
}