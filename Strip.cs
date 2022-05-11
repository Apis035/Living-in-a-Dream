using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using System;

using static StorybrewCommon.Assets;

namespace StorybrewScripts
{
    public class Strip : StoryboardObjectGenerator
    {
        public override void Generate()
        {
            var color1 = new Color4(.95f, .95f, .95f, 1f);
            var color2 = new Color4(.08f, .08f, .08f, 1f);

            Action<int, int, int> strip = (startTime, changeTime, endTime) =>
            {
                for (var x=-77; x<854; x+=30) {
                    var s = GetLayer(LAYER_STRIP).CreateSprite(PIXEL, OsbOrigin.TopLeft, new Vector2(x, 0));
                    s.ScaleVec  (startTime, 10, 480);
                    s.Rotate    (startTime, .15);
                    s.Color     (startTime, color1);
                    s.Color     (OsbEasing.OutExpo, changeTime, changeTime + BEAT4, color1, color2);
                    s.Fade      (startTime, startTime + BEAT4*2, 0, 1);
                    s.Fade      (endTime - BEAT4*2, endTime, 1, 0);
                    s.StartLoopGroup(startTime, (endTime - startTime) / (BEAT4*2));
                        s.MoveX     (0, BEAT4*2, s.InitialPosition.X, s.InitialPosition.X - 60);
                        s.EndGroup  ();
                }
            };

            strip(11007, 33076, 44110);
            strip(102041, 121352, 132386);
        }
    }
}
