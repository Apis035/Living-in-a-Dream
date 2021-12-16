using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;

namespace StorybrewScripts
{
    public class Main : StoryboardObjectGenerator
    {
        public override void Generate()
        {
		    GetLayer("Disable map BG").CreateSprite(Beatmap.BackgroundPath).Fade(0,0);

            var scale   = 480f / 1080f;
            var beat    = Beatmap.GetTimingPointAt(0).BeatDuration;
            var beat4   = beat*4;
            int time;

            var solid   = GetLayer("White BG").CreateSprite("sb/p.png");
            var overlay = GetLayer("White overlay").CreateSprite("sb/p.png");
            var blur    = GetLayer("Blurred BG").CreateSprite("sb/b.png");
            var girl    = GetLayer("Girl").CreateSprite("sb/g.png", OsbOrigin.Centre, new Vector2(370, 252));
            var flare   = GetLayer("Lightning").CreateSprite("sb/f.png");
            var light   = GetLayer("Lightning").CreateSprite("sb/l.png");
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
            solid.Fade(time, 0);
            flare.Scale(time, scale);
            flare.Additive(time);
            flare.Fade(time, time + beat4*4, 1, 0);
            overlay.ScaleVec(time - beat4*2, 854, 480);
            overlay.Additive(time - beat4*2);
            overlay.Fade(OsbEasing.In, time - beat4*2, time - beat, 0, 1);
            overlay.Fade(OsbEasing.Out, time, time + beat4, 1, 0);
            blur.Fade(time, 1);

            // Chorus //
            time = 55145;
            ltb1.Fade(time, 0);
            ltb2.Fade(time, 0);
            blur.Fade(time - beat, 0);
            flare.Fade(time - beat4*4, time - beat, 0, 1);
            flare.Fade(time - beat, 0);
            overlay.Fade(OsbEasing.In, time - beat4*2, time - beat, 0, 1);

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
            overlay.Fade(OsbEasing.In, time - beat4, time, 0, .3);
            overlay.Fade(OsbEasing.Out, time, time + beat4*3, 1, 0);
            overlay.Fade(OsbEasing.In, 88248 - beat4/2, 88248, 0, .1);
            overlay.Fade(OsbEasing.Out, 88248, 88248 + beat4, .5, 0);
            flare.Fade(time, .5);
            girl.Fade(time, .5);

            // Verse // 
            time = 99283;
            overlay.Fade(OsbEasing.In, time - beat4, time, 0, 1);
            overlay.Fade(time, 0);
            flare.Fade(time, 0);
            blur.Fade(time, 0);
            girl.Fade(time, 0);
            ltb1.Fade(OsbEasing.In, time, time + beat4*3, 0, 1);
            ltb2.Fade(OsbEasing.In, time, time + beat4*3, 0, 1);
            solid.Fade(time, 1);

            // Verse-break //
            time = 121352;
            solid.Fade(OsbEasing.OutExpo, time, time + beat4, 1, .05);

            // Prechorus //
            time = 132386;
            solid.Fade(time, 0);
            flare.Fade(time, time + beat4*4, 1, 0);
            overlay.Fade(OsbEasing.In, time - beat4*2, time - beat, 0, 1);
            overlay.Fade(OsbEasing.Out, time, time + beat4, 1, 0);
            blur.Fade(time, 1);

            // Chorus //
            time = 143421;
            ltb1.Fade(time, 0);
            ltb2.Fade(time, 0);
            blur.Fade(time - beat, 0);
            flare.Fade(time - beat4*4, time - beat, 0, 1);
            flare.Fade(time - beat, 0);
            overlay.Fade(OsbEasing.In, time - beat4*2, time - beat, 0, 1);

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
                girl.MoveY(OsbEasing.InOutSine, 0, beat4*4, 245, 260);
                girl.MoveY(OsbEasing.InOutSine, beat4*4, beat4*8, 260, 245);
                girl.EndGroup();
            girl.StartLoopGroup(time, 8);
                girl.Rotate(OsbEasing.InOutSine, 0, beat4*3, -.01, .01);
                girl.Rotate(OsbEasing.InOutSine, beat4*3, beat4*6, .01, -.01);
                girl.EndGroup();

            // Chorus-no kiai //
            time = 165490;
            light.Fade(time, 0);
            overlay.Fade(OsbEasing.In, time - beat4, time, 0, .3);
            overlay.Fade(OsbEasing.Out, time, time + beat4*3, 1, 0);
            overlay.Fade(OsbEasing.In, 176524 - beat4/2, 176524, 0, .1);
            overlay.Fade(OsbEasing.Out, 176524, 176524 + beat4, .5, 0);
            flare.Fade(time, .5);
            girl.Fade(time, .5);

            // Outro //
            time = 187559;
            overlay.Fade(OsbEasing.In, time - beat4, time, 0, .5);
            overlay.Fade(OsbEasing.Out, time, time + beat4*3, 1, 0);
            flare.Fade(time, 0);
            girl.Fade(187559, 206869, .5, 0);
            blur.Fade(OsbEasing.Out, 202731, 209628, 1, 0);
        }
    }
}
