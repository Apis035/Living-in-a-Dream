using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.Util;
using StorybrewCommon.Subtitles;
using System;
using System.Collections.Generic;

namespace StorybrewScripts
{
	public class Main : StoryboardObjectGenerator
	{
		[Configurable] public bool ReduceLight = false;
		OsbSpritePool pool;
		float scale;
		int beat, beat4;
		FontGenerator font;

		public override void Generate()
		{
			GetLayer("Disable Map BG").CreateSprite(Beatmap.BackgroundPath).Fade(0,0);

			scale = 480f / 1080f;
			beat  = (int)Beatmap.GetTimingPointAt(0).BeatDuration;
			beat4 = beat*4;

			font = LoadFont("sb/font",
				new FontDescription() { FontPath = "Consolas", Color = Color4.White, FontSize = 34});

			Background();
			Highlight();
			Particle();
			Credits();
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
			var ltb1    = GetLayer("Letterbox").CreateSprite("sb/p.png", OsbOrigin.TopCentre, new Vector2(320, -50));
			var ltb2    = GetLayer("Letterbox").CreateSprite("sb/p.png", OsbOrigin.BottomCentre, new Vector2(320, 530));

			// Intro //
			time = -27;
			ltb1.ScaleVec(time, 854, 90);
			ltb2.ScaleVec(time, 854, 90);
			ltb1.Color(time, Color4.Black);
			ltb2.Color(time, Color4.Black);
			ltb1.Fade(time, 1);
			ltb2.Fade(time, 1);
			blur.Scale(time, scale);
			blur.Fade(time, time + beat4*4, 0, ReduceLight ? .8 : 1);

			// Verse //
			time = 11007;
			blur.Fade(time, 0);
			solid.ScaleVec(time - beat4, 854, 480);
			solid.Fade(OsbEasing.In, time - beat4, time, 0, 1);

			// Verse (break) //
			time = 33076;
			solid.Fade(OsbEasing.OutExpo, time, time + beat4, 1, .05);

			// Prechorus //
			time = 44110;
			blur.Fade(time, ReduceLight ? .8 : 1);
			solid.Fade(time, 0);
			flare.Additive(time);
			flare.Fade(time, time + beat4*4, ReduceLight ? .4 : .8, 0);
			overlay.ScaleVec(time - beat4*2, 854, 480);
			overlay.Additive(time - beat4*2);
			overlay.Fade(OsbEasing.In, time - beat4*2, time - beat, 0, 1);
			overlay.Fade(OsbEasing.Out, time, time + beat4, 1, 0);

			// Chorus //
			time = 55145;

			Action letterboxIn = () =>
			{
				ltb1.ScaleVec(OsbEasing.InExpo, time - beat4, time - beat, 854, 90, 950, 290);
				ltb2.ScaleVec(OsbEasing.InExpo, time - beat4, time - beat, 854, 90, 950, 290);
				ltb1.Rotate(OsbEasing.InExpo, time - beat4, time - beat, 0, -.2);
				ltb2.Rotate(OsbEasing.InExpo, time - beat4, time - beat, 0, -.2);
				ltb1.Fade(time - beat4, time - beat, 1, 0);
				ltb2.Fade(time - beat4, time - beat, 1, 0);
			};

			letterboxIn();
			blur.Fade(time - beat, 0);
			flare.Fade(time - beat4*4, time - beat, 0, ReduceLight ? .4 : .8);
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

			klight.ScaleVec(time, 14, 8);
			klight.Additive(time);
			klight.Color(time, Color4.MidnightBlue);
			slight.ScaleVec(time + beat, 14, 8);
			slight.FlipH(time + beat);
			slight.Additive(time + beat);
			slight.Color(time + beat, Color4.DarkRed);

			repeatSidelight();
			klight.Fade(OsbEasing.Out, 74455, 75145, .5, 0);

			overlay.Fade(OsbEasing.OutExpo, time - beat, time, 1, 0);
			overlay.Fade(OsbEasing.Out, time, time + beat4*3, 1, 0);
			overlay.Fade(OsbEasing.In, 66179 - beat4, 66179, 0, .1);
			overlay.Fade(OsbEasing.Out, 66179, 66179 + beat4, .5, 0);
			blur.Fade(time, ReduceLight ? .8 : 1);
			flare.Fade(time, ReduceLight ? .3 : .8);
			light.ScaleVec(time, 854, 1);
			light.StartLoopGroup(time, 2);
				light.Fade(OsbEasing.InOutSine, 0, beat*4*4, ReduceLight ? .4 : .8, .1);
				light.Fade(OsbEasing.InOutSine, beat*4*4, beat*4*8, .1, ReduceLight ? .4 : .8);
				light.EndGroup();
			girl.Scale(time, scale);
			girl.Fade(time, .6);
			girl.StartLoopGroup(time, 4);
				girl.MoveY(OsbEasing.InOutSine, 0, beat4*4, 245, 260);
				girl.MoveY(OsbEasing.InOutSine, beat4*4, beat4*8, 260, 245);
				girl.EndGroup();
			girl.StartLoopGroup(time, 5);
				girl.Rotate(OsbEasing.InOutSine, 0, beat4*3, -.01, .01);
				girl.Rotate(OsbEasing.InOutSine, beat4*3, beat4*6, .01, -.01);
				girl.EndGroup();

			// Chorus (no kiai) //
			time = 77214;
			light.Fade(time, 0);
			flare.Fade(time, ReduceLight ? .1 : .5);
			girl.Fade(time, .5);
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
			ltb1.ScaleVec(time, 854, 90);
			ltb2.ScaleVec(time, 854, 90);
			ltb1.Rotate(time, 0);
			ltb2.Rotate(time, 0);
			ltb1.Fade(OsbEasing.In, time, time + beat4*3, 0, 1);
			ltb2.Fade(OsbEasing.In, time, time + beat4*3, 0, 1);

			// Verse (break) //
			time = 121352;
			solid.Fade(OsbEasing.OutExpo, time, time + beat4, 1, .05);

			// Prechorus //
			time = 132386;
			blur.Fade(time, ReduceLight ? .8 : 1);
			solid.Fade(time, 0);
			flare.Fade(time, time + beat4*4, ReduceLight ? .4 : .8, 0);
			overlay.Fade(OsbEasing.In, time - beat4*2, time - beat, 0, 1);
			overlay.Fade(OsbEasing.Out, time, time + beat4, 1, 0);

			// Chorus //
			time = 143421;

			letterboxIn();
			blur.Fade(time - beat, 0);
			flare.Fade(time - beat4*4, time - beat, 0, ReduceLight ? .4 : .8);
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
			blur.Fade(time, ReduceLight ? .8 : 1);
			flare.Fade(time, ReduceLight ? .3 : .8);
			light.StartLoopGroup(time, 2);
				light.Fade(OsbEasing.InOutSine, 0, beat*4*4, ReduceLight ? .4 : .8, .1);
				light.Fade(OsbEasing.InOutSine, beat*4*4, beat*4*8, .1, ReduceLight ? .4 : .8);
				light.EndGroup();
			girl.Fade(time, .6);
			girl.StartLoopGroup(time, 6);
				girl.MoveY(OsbEasing.InOutSine, 0, beat4*4, 245, 260);
				girl.MoveY(OsbEasing.InOutSine, beat4*4, beat4*8, 260, 245);
				girl.EndGroup();
			girl.StartLoopGroup(time, 8);
				girl.Rotate(OsbEasing.InOutSine, 0, beat4*3, -.01, .01);
				girl.Rotate(OsbEasing.InOutSine, beat4*3, beat4*6, .01, -.01);
				girl.EndGroup();

			// Chorus- (no kiai) //
			time = 165490;
			light.Fade(time, 0);
			flare.Fade(time, ReduceLight ? .1 : .5);
			girl.Fade(time, .5);
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
			girl.Fade(187559, 206869, .5, 0);
			blur.Fade(OsbEasing.Out, 202731, 209628, ReduceLight ? .8 : 1, 0);
		}

		void Highlight()
		{
			var size = 3.5/Beatmap.CircleSize;

			Action<int, int, float> glow = (startTime, endTime, brightness) => 
			{
				foreach (var circle in Beatmap.HitObjects)
				{
					var slider = circle is OsuSlider;
					var cStart = circle.StartTime;
					var cEnd   = circle.EndTime;

					if (cStart < startTime - 5 || endTime - 5 <= cStart) continue;

					var sprite = pool.Get(cStart, cEnd + beat4 / (slider ? 2 : 1));
					sprite.Move(cStart, circle.Position);
					sprite.Scale(cStart, cStart + beat4/2, size, size*1.2);
					sprite.Fade(OsbEasing.Out, cStart, cStart + beat4 /
						(slider ? 2 : 1),
						(slider ? .4 : .2) + brightness, 0);

					if (!sprite.ColorAt(cStart).Equals(circle.Color)) // reduce color command dupe to save filesize
						sprite.Color(cStart, circle.Color);

					if (slider)
					{
						var step  = beat / 8;
						var start = cStart;
						while (true)
						{
							var end  = start + step;
							var done = cEnd - end < 5;
							if (done) end = cEnd;

							sprite.Move(start, end, sprite.PositionAt(start), circle.PositionAtTime(end));

							if (done) break;
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
					sprite.Move(circle.StartTime, circle.Position);
					sprite.Scale(big ? OsbEasing.Out : OsbEasing.OutCubic, circle.StartTime, circle.StartTime +	(big ? beat4*5 : beat4*4), .2, (big ? 3 : 2));
					sprite.Fade(OsbEasing.OutExpo, circle.StartTime, circle.StartTime +	(big ? beat4*6 : beat4*4), 1, 0);
				}
			};

			Action<int, int, bool> strip = (startTime, endTime, noDelay) =>
			{
				var lastPos  = new Vector2(240, 320);
				var lastDir  = 0d;
				var lastTime = (double)startTime;
				var scale    = noDelay ? 70 : 40;
				var fade     = noDelay ? .2f : .02f;
				foreach (var circle in Beatmap.HitObjects)
				{
					var slider = circle is OsuSlider;
					var cStart = circle.StartTime;
					var cEnd   = circle.EndTime;

					if (cStart < startTime - 5 || endTime - 5 <= cStart) continue;

					var angle  = Math.Sqrt(
						// change direction only slightly a bit if its a circle stack
						Math.Pow(lastPos.X - circle.Position.X, 2) + Math.Pow(lastPos.Y - circle.Position.Y, 2)) > 10 ?
						Math.Atan2(circle.Position.Y - lastPos.Y, circle.Position.X - lastPos.X) + Math.PI/2 :
						lastDir - 0.1;

					if (cStart - lastTime < 100 && scale > 40) scale -= 10;

					var sprite = pool.Get(cStart, cStart + beat4);
					sprite.Move(cStart, circle.Position);
					sprite.Rotate(cStart, angle);
					sprite.ScaleVec(OsbEasing.OutQuint, cStart, cStart + beat4, 1400, scale * size, 1400, 0);
					sprite.Fade(OsbEasing.OutExpo, cStart, cStart + beat4, fade, .05);

					if (scale < 70) scale += 10;
					if (fade < .3f) fade  += .04f;
					lastPos  = circle.Position;
					lastDir  = angle;
					lastTime = cStart;
				}
			};

			using (pool = new OsbSpritePool(GetLayer("Front Highlight"), "sb/h.png", OsbOrigin.Centre, (s, a, b) => s.Additive(a)))
			{
				glow(44110, 55145, 0f);
				glow(55145, 77214, .3f);
				glow(77214, 99283, .1f);
				glow(132386, 143421, 0f);
				glow(143421, 165490, .3f);
				glow(165490, 187559, .1f);
			}

			using (pool = new OsbSpritePool(GetLayer("Front Highlight"), "sb/r.png", OsbOrigin.Centre, (s, a, b) => s.Additive(a)))
			{
				foreach (var i in new List<int>{
					44110, 55145, 66179, 77214, 132386, 143421, 154455, 165490, 187559})
					ring(i, true);
				foreach (var i in new List<int>{
					88248, 176524})
					ring(i, false);
			}

			using (pool = new OsbSpritePool(GetLayer("Back Highlight"), "sb/p.png", OsbOrigin.Centre, (s, a, b) => s.Additive(a)))
			{
				for (var i=45490; i<46869; i+=beat)
					strip(i-10, i+10, true);
				for (var i=48248; i<49628; i+=beat)
					strip(i-10, i+10, true);
				for (var i=51007; i<52386; i+=beat)
					strip(i-10, i+10, true);
				strip(52386, 53766, false);
				strip(57904, 60748, false);
				strip(63421, 66266, false);
				strip(68938, 71783, false);
				strip(74455, 75835, false);

				for (var i=133766; i<135145; i+=beat)
					strip(i-10, i+10, true);
				for (var i=136524; i<137904; i+=beat)
					strip(i-10, i+10, true);
				for (var i=139283; i<140662; i+=beat)
					strip(i-10, i+10, true);
				strip(140662, 142041, false);
				strip(146179, 149024, false);
				strip(151697, 154541, false);
				strip(157214, 160059, false);
				strip(162731, 164110, false);
			}
		}

		void Particle()
		{
			Action<int, int, int> spark = (startTime, endTime, step) =>
			{
				for (var i=startTime; i<endTime; i+=step)
				{
					var angle    = Random(0, Math.PI*2);
					var duration = 1500 + Random(5000);
					var distance = Random(300, 500);
					var position = new Vector2(
						(float)(320 + Math.Cos(angle) * distance),
						(float)(240 + Math.Sin(angle) * distance)
					);

					var sprite = pool.Get(i, i + duration);
					sprite.Move(OsbEasing.OutSine, i, i + duration, new Vector2(320, 240), position);
					sprite.Scale(OsbEasing.Out, i, i + duration, .5, 0);
					sprite.Fade(OsbEasing.InExpo, i, i + 800, 0, 1);
				}
			};

			Action<int, int, int, int> flyUp = (startTime, endTime, step, dur) =>
			{
				for (var i=startTime; i<endTime; i+=step)
				{
					var duration = dur + Random(dur/2);
					var startPos = new Vector2(-97 + Random(844), 450);
					var endPos   = new Vector2(
						startPos.X + Random(-100, 100),
						startPos.Y - Random(300, 480));

					var sprite = pool.Get(i, i + duration);
					sprite.Move(OsbEasing.InOutSine, i, i + duration, startPos, endPos);
					sprite.Scale(OsbEasing.Out, i, i + duration, Random(10) < 4 ? .5 : 1, 0);
					sprite.Fade(OsbEasing.In, i, i + 1000, 0, 1);
				}
			};

			using (pool = new OsbSpritePool(GetLayer("Particle"), "sb/a.png", OsbOrigin.Centre))
			{
				spark(56524, 77214, 15);
				spark(77214, 99283, 30);

				spark(144800, 165490, 15);
				spark(165490, 187559, 30);

				flyUp(33076, 44110, 120, 5000);
				flyUp(44110, 46869, 40, 3000);
				flyUp(46869, 49628, 30, 2500);
				flyUp(49628, 52386, 20, 2000);
				flyUp(52386, 53766, 20, 1500);

				flyUp(121352, 132386, 120, 5000);
				flyUp(132386, 135145, 40, 3000);
				flyUp(135145, 137904, 30, 2500);
				flyUp(137904, 140662, 20, 2000);
				flyUp(140662, 142041, 20, 1500);
			}
		}

		void Credits()
		{
			var mapper = "peppy";

			Action<int, int, bool, string, bool> drawText = (startTime, endTime, upPos, text, mapperName) =>
			{
				var width = 0f;
				var scale = upPos ? .3f : .5f;
				foreach (var letter in text) width += font.GetTexture(letter.ToString()).BaseWidth * scale;

				var x = 313f - width/2;
				var y = upPos ? 324f : 350f;
				var delay = 0;

				foreach (var letter in text)
				{
					var texture = font.GetTexture(letter.ToString());

					if (!texture.IsEmpty)
					{
						var layer = text == mapper ? "Mapper" : "Credits";

						var pos = new Vector2(x, y) + texture.OffsetFor(OsbOrigin.Centre);
						var sprite = GetLayer(layer).CreateSprite(texture.Path, OsbOrigin.Centre, pos);
						sprite.Scale(startTime, scale);
						sprite.MoveY(OsbEasing.OutCirc, startTime, startTime + 1000, pos.Y + (upPos ? 12 : -12), pos.Y);
						sprite.MoveY(OsbEasing.InCirc, endTime - 1000, endTime, pos.Y, pos.Y + (upPos ? 12 : -12));
						sprite.Fade(OsbEasing.Out, startTime, startTime + 1000, 0, 1);
						sprite.Fade(OsbEasing.In, endTime - 1000, endTime, 1, 0);
					}
					x += texture.BaseWidth * scale;
					delay += 10;
				}
			};

			var line = GetLayer("Credits").CreateSprite("sb/p.png", OsbOrigin.Centre, new Vector2(320, 363));
			line.Fade(0, .7);
			line.Fade(9000, 10500, .7, 0);
			line.ScaleVec(OsbEasing.InOutSine, 0, 4000, 0, 1, 340, 1);
			line.ScaleVec(OsbEasing.InOutSine, 6000, 10500, 340, 1, 0, 1);

			drawText(1000, 4000, true, "Quinn Karter ft. Natalie Major", false);
			drawText(1000, 4000, false, "Living in a Dream (Feint Remix)", false);

			switch (Beatmap.Name)
			{
				case "Normal": case "Hard": case "Insane": case "Star":
					mapper = "Asphyxia";
					break;

				case "kolik's Easy":
					mapper = "koleiker";
					break;

				case "Kyshiro's Extra":
					mapper = "Kyshiro";
					break;
			}

			drawText(4000, 7000, true, "Beatmap by", false);
			drawText(4000, 7000, false, mapper, true);

			drawText(7000, 10000, true, "Storyboard by", false);
			drawText(7000, 10000, false, "Apis035 & -Ady", true);

		}
	}
}
