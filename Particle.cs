using OpenTK;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.Util;
using System;

using static StorybrewCommon.Assets;

namespace StorybrewScripts
{
    public class Particle : StoryboardObjectGenerator
    {

        OsbSpritePool pool;
        public override void Generate()
        {
            Action<int, int, int> spark = (startTime, endTime, step) =>
            {
                for (var i=startTime; i<endTime; i+=step)
                {
                    var angle    = Random(0, Math.PI*2);
                    var duration = 800 + Random(4000);
                    var distance = Random(300, 500);
                    var position = new Vector2(
                        (float)(320 + Math.Cos(angle) * distance),
                        (float)(240 + Math.Sin(angle) * distance)
                    );

                    var sprite = pool.Get(i, i + duration);
                    sprite.Move (OsbEasing.OutSine, i, i + duration, new Vector2(320, 240), position);
                    sprite.Scale(OsbEasing.Out, i, i + duration, Random(.5f, .8f), 0);
                    sprite.Fade (OsbEasing.InExpo, i, i + 800, 0, 1);
                }
            };

            Action<int, int, int, int> flyUp = (startTime, endTime, step, dur) =>
            {
                for (var i=startTime; i<endTime; i+=step)
                {
                    var duration = dur + Random(dur/2);
                    var startPos = new Vector2(-97 + Random(844), Random(200, 480));
                    var endPos   = new Vector2(
                        startPos.X + Random(-100, 100),
                        startPos.Y - Random(80, 380));

                    var sprite = pool.Get(i, i + duration);
                    sprite.Move (OsbEasing.InOutSine, i, i + duration, startPos, endPos);
                    sprite.Scale(OsbEasing.Out, i, i + duration, Random(.5f, 1f), 0);
                    sprite.Fade (OsbEasing.In, i, i + 1000, 0, 1);
                }
            };

            using (pool = new OsbSpritePool(GetLayer(LAYER_PARTICLE), PARTICLE, OsbOrigin.Centre))
            {
                spark(56524, 77214, 15);
                spark(77214, 99283, 30);

                spark(144800, 165490, 15);
                spark(165490, 187559, 30);

                flyUp(33076, 44110, 80, 5000);
                flyUp(44110, 46869, 40, 3000);
                flyUp(46869, 49628, 30, 2500);
                flyUp(49628, 52386, 20, 2000);
                flyUp(52386, 53766, 20, 1500);

                flyUp(121352, 132386, 80, 5000);
                flyUp(132386, 135145, 40, 3000);
                flyUp(135145, 137904, 30, 2500);
                flyUp(137904, 140662, 20, 2000);
                flyUp(140662, 142041, 20, 1500);
            }
        }
    }
}
