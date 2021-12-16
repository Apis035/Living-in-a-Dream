﻿using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Animations;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using System;

namespace StorybrewScripts
{
    public class Spectrum : StoryboardObjectGenerator
    {
        public Vector2 Position    = new Vector2(200, 240);
        public float   Width       = 800;
        public int     BeatDivisor = 8;
        public int     BarCount    = 120;
        public Vector2 Scale       = new Vector2(1, 40);
        public int     LogScale    = 300;
        public double  PowerScale  = 1.2;
        public double  Tolerance   = 2;

        public override void Generate()
        {
            int startTime = -27;
            int EndTime   = 213766;
            var bitmap    = GetMapsetBitmap("sb/s.png");
            var layer     = GetLayer("Spectrum");
            var barWidth  = Width*2 / BarCount;

            //Initialize spectrum keyframes
            var heightKeyframes = new KeyframedValue<float>[BarCount];
            for (var i=0; i<BarCount; i++)
                heightKeyframes[i] = new KeyframedValue<float>(null);

            //Get spectrum data
            var timestep  = Beatmap.GetTimingPointAt(startTime).BeatDuration / BeatDivisor;
            for (var time = (double)startTime; time<EndTime; time+=timestep)
            {
                var fft = GetFft(time + (timestep * 0.2), BarCount, null, OsbEasing.InExpo);
                for (var i=0; i<BarCount; i++)
                {
                    var height = (float)Math.Pow((float)Math.Log(1 + fft[i] * LogScale), PowerScale) * Scale.Y / bitmap.Height;
                    if (height < 0.5f) height = 0.5f;

                    heightKeyframes[i].Add(time, height);
                }
            }

            //Draw only 1/3 part of total specrum
            var totalWidth = 0f;
            for (var i=0; i<BarCount/3; i++)
                totalWidth += barWidth;

            for (var i=0; i<BarCount/3; i++)
            {
                var keyframes = heightKeyframes[i];
                keyframes.Simplify1dKeyframes(Tolerance, h => h);

                var sprite = layer.CreateSprite("sb/s.png", OsbOrigin.Centre, new Vector2(325 - totalWidth/2 + i * barWidth, Position.Y));
                sprite.CommandSplitThreshold = 100;

                var scaleX = (float)Math.Floor(Scale.X * barWidth / (bitmap.Width - 2) * 10) / 10.0f;
                keyframes.ForEachPair(
                    (start, end) =>
                    {
                        sprite.ScaleVec(start.Time, end.Time, scaleX, start.Value, scaleX, end.Value);
                    },
                    0.05f, s => (float)Math.Round(s, 1)
                );
                sprite.ScaleVec(startTime, scaleX, 0.5f);
                sprite.Fade(-27, 5490, 0, 1);
                sprite.Color(OsbEasing.In, 9628, 11007, Color4.White, Color4.Black);
                sprite.Fade(54800, 0.5);
                sprite.Fade(55145, 1);
                sprite.Color(OsbEasing.OutExpo, 33076, 34455, Color4.Black, Color4.White);
                sprite.Color(OsbEasing.In, 99283, 103421, Color4.White, Color4.Black);
                sprite.Color(OsbEasing.OutExpo, 121352, 122731, Color4.Black, Color4.White);
                sprite.Fade(143076, 0.5);
                sprite.Fade(143421, 1);
                sprite.Fade(209628, 213766, 1, 0);
            }
        }
    }
}