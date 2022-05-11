using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;

using static StorybrewCommon.Assets;

namespace StorybrewScripts
{
    public class Overlay : StoryboardObjectGenerator
    {
        int t; void setTime(int T) => t = T;

        public override void Generate()
        {
            var solid1 = GetLayer(LAYER_SOLID1).CreateSprite(PIXEL);
            var solid2 = GetLayer(LAYER_SOLID2).CreateSprite(PIXEL);

            // Intro //

            // Verse //
            setTime         (11007);
            solid1.ScaleVec (t - BEAT4, 854, 480);
            solid1.Fade     (OsbEasing.In, t - BEAT4, t, 0, 1);

            // Verse (break) //
            setTime         (33076);
            solid1.Fade     (OsbEasing.OutExpo, t, t + BEAT4, 1, .05);

            // Prechorus //
            setTime         (44110);
            solid1.Fade     (t, 0);
            solid2.ScaleVec (t - BEAT4*2, 854, 480);
            solid2.Additive (t - BEAT4*2);
            solid2.Fade     (OsbEasing.In, t - BEAT4*2, t - BEAT, 0, 1);
            solid2.Fade     (OsbEasing.Out, t, t + BEAT4, 1, 0);

            // Chorus //
            setTime         (55145);
            solid2.Fade     (OsbEasing.In, t - BEAT4*2, t - BEAT, 0, 1);
            solid2.Fade     (OsbEasing.OutExpo, t - BEAT, t, 1, 0);
            solid2.Fade     (OsbEasing.Out, t, t + BEAT4*3, 1, 0);
            solid2.Fade     (OsbEasing.In, 66179 - BEAT4, 66179, 0, .1);
            solid2.Fade     (OsbEasing.Out, 66179, 66179 + BEAT4, .5, 0);

            // Chorus (no kiai) //
            setTime         (77214);
            solid2.Fade     (OsbEasing.In, t - BEAT4, t, 0, .3);
            solid2.Fade     (OsbEasing.Out, t, t + BEAT4*3, 1, 0);
            solid2.Fade     (OsbEasing.In, 88248 - BEAT4/2, 88248, 0, .1);
            solid2.Fade     (OsbEasing.Out, 88248, 88248 + BEAT4, .5, 0);

            // Verse // 
            setTime         (99283);
            solid2.Fade     (t, 0);
            solid1.Fade     (t, 1);
            solid2.Fade     (OsbEasing.In, t - BEAT4, t, 0, 1);

            // Verse (break) //
            setTime         (121352);
            solid1.Fade     (OsbEasing.OutExpo, t, t + BEAT4, 1, .05);

            // Prechorus //
            setTime         (132386);
            solid1.Fade     (t, 0);
            solid2.Fade     (OsbEasing.In, t - BEAT4*2, t - BEAT, 0, 1);
            solid2.Fade     (OsbEasing.Out, t, t + BEAT4, 1, 0);

            // Chorus //
            setTime         (143421);
            solid2.Fade     (OsbEasing.In, t - BEAT4*2, t - BEAT, 0, 1);
            solid2.Fade     (OsbEasing.OutExpo, t - BEAT, t, 1, 0);
            solid2.Fade     (OsbEasing.Out, t, t + BEAT4*3, 1, 0);
            solid2.Fade     (OsbEasing.In, 154455 - BEAT4, 154455, 0, .1);
            solid2.Fade     (OsbEasing.Out, 154455, 154455 + BEAT4, .5, 0);

            // Chorus- (no kiai) //
            setTime         (165490);
            solid2.Fade     (OsbEasing.In, t - BEAT4, t, 0, .3);
            solid2.Fade     (OsbEasing.Out, t, t + BEAT4*3, 1, 0);
            solid2.Fade     (OsbEasing.In, 176524 - BEAT4/2, 176524, 0, .1);
            solid2.Fade     (OsbEasing.Out, 176524, 176524 + BEAT4, .5, 0);

            // Outro //
            setTime         (187559);
            solid2.Fade     (OsbEasing.In, t - BEAT4, t, 0, .5);
            solid2.Fade     (OsbEasing.Out, t, t + BEAT4*3, 1, 0);
        }
    }
}