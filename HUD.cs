using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Subtitles;
using System;

using static StorybrewCommon.Assets;

namespace StorybrewScripts
{
    public class HUD : StoryboardObjectGenerator
    {
        int t; void setTime(int T) => t = T;

        public override void Generate()
        {
            var line = GetLayer(LAYER_GUI).CreateSprite(PIXEL, OsbOrigin.CentreLeft, new Vector2(320 - 150, 400));
            var dot  = GetLayer(LAYER_GUI).CreateSprite(PARTICLE, OsbOrigin.Centre, new Vector2(320, 400));

            setTime         (-27);
            line.ScaleVec   (OsbEasing.OutSine, t, t + BEAT4*4, 0, 1, 300, 1);
            line.Fade       (t, t + BEAT4*2, 0, .7);
            line.Color      (t, Color4.White);
            dot.Color       (t, Color4.White);

            setTime         (195835);
            line.MoveX      (OsbEasing.InSine, t, t + BEAT4*2, 320 - 150, 320);
            line.ScaleVec   (OsbEasing.InSine, t, t + BEAT4*2, 300, 1, 0, 1);
            //line.Fade       (OsbEasing.Out, t + BEAT4*2, t + BEAT4*3, .7, 0);

            var white = true;

            Action<int, int> progress = (start, end) =>
            {
                end -= 1;
                dot.Fade    (start, start + BEAT*2, 0, 1);
                dot.MoveX   (start, end, 320 - 150, 320 + 150);
                dot.Fade    (end - BEAT*2, end, 1, 0);
            };

            Action<int> swapColor = (time) => 
            {
                if (white) {
                    line.Color(OsbEasing.Out, time, time + BEAT4, Color4.White, Color4.Black);
                    dot.Color(OsbEasing.Out, time, time + BEAT4, Color4.White, Color4.Black);
                } else {
                    line.Color(OsbEasing.Out, time, time + BEAT4, Color4.Black, Color4.White);
                    dot.Color(OsbEasing.Out, time, time + BEAT4, Color4.Black, Color4.White);
                }
                white = !white;
            };

            Action<int> forceBlack = (time) =>
            {
                line.Color(OsbEasing.Out, time, time + BEAT4, Color4.Black, Color4.White);
                dot.Color(OsbEasing.Out, time, time + BEAT4, Color4.Black, Color4.White);
            };

            var font = LoadFont(SB_PATH, new FontDescription() {
                FontPath = FONT1,
                FontSize = 18,
                Color    = Color4.White
            });

            var s = GetLayer(LAYER_GUI).CreateSprite(
                        font.GetTexture("Intense part incoming...").Path, OsbOrigin.TopCentre, new Vector2(320, 408));

            Action<int> intense = (time) =>
            {
                if (s.ScaleAt(time).X != .5) s.Scale(time, .5);
                s.Fade(time, time + BEAT*2, 0, .8);
                s.Fade(time + BEAT4*3, time + BEAT4*3 + BEAT*2, .8, 0);
            };

            progress    (-27, 11007);
            swapColor   (11007);
            progress    (11007, 33076);
            swapColor   (33076);
            progress    (33076, 44110);
            swapColor   (41352);
            swapColor   (44110);
            progress    (44110, 55145);
            intense     (48248);
            swapColor   (52386);
            swapColor   (54455);
            progress    (55145, 77214);
            progress    (77214, 99283);
            forceBlack  (77214);
            swapColor   (99283);
            progress    (99283, 121352);
            swapColor   (121352);
            progress    (121352, 132386);
            swapColor   (129628);
            swapColor   (132386);
            progress    (132386, 143421);
            intense     (136524);
            swapColor   (140662);
            swapColor   (142731);
            progress    (143421, 165490);
            progress    (165490, 187559);
            forceBlack  (165490);
        }
    }
}
