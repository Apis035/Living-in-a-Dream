using System.IO;
using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Subtitles;
using System;

using static StorybrewCommon.Assets;

namespace StorybrewScripts
{
    public class Lyric : StoryboardObjectGenerator
    {
        [Configurable] public int PositionY = 360;
        [Configurable] public int FadeTime  = 100;

        public override void Generate()
        {
            var lyricFile = $"{ProjectPath}/assetlibrary/lyrics_for_living_in_a_dream_w_probox.txt";
		    AddDependency(lyricFile);

            var font = LoadFont(FONT_PATH,
                new FontDescription() {
                    FontPath = FONT2,
                    FontSize = 36,
                    Color    = Color4.White
                }
            );

            foreach (var line in File.ReadAllLines(lyricFile)) {
                int o;
                var arg = line.Split(new[]{' '}, 2);
                if (!String.IsNullOrEmpty(arg[0]) && int.TryParse(arg[0], out o))
                {
                    var startTime = int.Parse(arg[0]);
                    var endTime   = startTime;
                    var text      = arg[1];
                    var width     = 0f;
                    var letterX   = 0f;
                    var inDelay   = 0;
                    var outDelay  = 0;

                    foreach (var letter in text) {
                        switch (letter) {
                            case '_': endTime += BEAT;     break;
                            case ',': endTime += BEAT/4*3; break;
                            case '-': endTime += BEAT/2;   break;
                            case '/': endTime += BEAT/4;   break;
                            default: width += font.GetTexture(letter.ToString()).BaseWidth * .5f; break;
                        }
                    }

                    foreach (var letter in text) {
                        if (letter != '_' && letter != ',' && letter != '-' && letter != '/') {
                            var texture = font.GetTexture(letter.ToString());

                            if (!texture.IsEmpty) {
                                var position = new Vector2(320 - width/2 +  letterX, PositionY) + texture.OffsetFor(OsbOrigin.Centre) * .5f;

                                var s = GetLayer(LAYER_LYRIC).CreateSprite(texture.Path, OsbOrigin.Centre, position);
                                s.Scale (startTime, .5f);

                                if (startTime + inDelay - FadeTime > startTime + outDelay*4 + 20) {
                                    s.MoveY (startTime + outDelay, startTime + outDelay + 20, position.Y, position.Y - 6);
                                    s.Fade  (startTime + outDelay, startTime + outDelay*4 + 20, 0, .2);
                                }
                                s.MoveY (OsbEasing.OutCirc, startTime + inDelay - FadeTime, startTime + inDelay + FadeTime*4, position.Y - 6, position.Y);
                                s.Fade  (startTime + inDelay - FadeTime, startTime + inDelay, .2, 1);
                                s.MoveY (OsbEasing.OutCubic, endTime - FadeTime, endTime, position.Y, position.Y + 4);
                                s.Fade  (OsbEasing.OutCubic, endTime - FadeTime, endTime, 1, 0);

                                filter(startTime, endTime, s);
                                //if (black) s.Color(startTime + inDelay, Color4.Black);
                            }

                            letterX  += texture.BaseWidth * .5f;
                            outDelay += 6;
                        } else {
                            switch (letter) {
                                case '_': inDelay += BEAT;     break;
                                case ',': inDelay += BEAT/4*3; break;
                                case '-': inDelay += BEAT/2;   break;
                                case '/': inDelay += BEAT/4;   break;
                            }
                        }
                    }
                }
            }
        }

        void filter(int start, int end, OsbSprite s)
        {
            if ((start >= 11007 && end <= 31352) || (start >= 99283 && end <= 120317)) {
                s.Color(start, Color4.Black);
            }

            if (start == 32386 || start == 120662) {
                start += BEAT*2;
                s.Color(OsbEasing.OutExpo, start, start + BEAT4, Color4.Black, Color4.White);
            }

            if (start == 43421 || start == 131697) {
                s.Color(start, Color4.Black);
                start += BEAT*2;
                s.Color(OsbEasing.OutExpo, start, start + BEAT4, Color4.Black, Color4.White);
            }

            if (start == 53421 || start == 141697) {
                s.Color(start, start + BEAT*2, Color4.White, Color4.Black);
            }

            if (start == 77214 || start == 165490) {
                s.Color(OsbEasing.Out, start, start + BEAT4, Color4.Black, Color4.White);
            }

            if (start == 97904) {
                s.Color(start, start + BEAT4, Color4.White, Color4.Black);
            }
        }
    }
}
