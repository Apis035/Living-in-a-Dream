using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Subtitles;

using static StorybrewCommon.Assets;

namespace StorybrewScripts
{
    public class Credit : StoryboardObjectGenerator
    {
        int t; void setTime(int T) => t = T;

        FontGenerator font;
        string mapper;

        public override void Generate()
        {
            font = LoadFont(FONT_PATH, new FontDescription() {
                FontPath = FONT2,
                Color    = Color4.White,
                FontSize = 34
            });

            setTime (188938);
            drawText(t, t + BEAT4*3, "Quinn Karter ft. Natalie Major", true);
            drawText(t, t + BEAT4*3, "Living in a Dream (Feint Remix)");

            switch (Beatmap.Name)
            {
                case "Hard": case "Insane": case "Star":
                    mapper = "Asphyxia";
                    break;

                case "kolik's Easy":
                    mapper = "koleiker";
                    break;

                case "riffy's Normal":
                    mapper = "riffy";
                    break;

                case "Kyshiro's Extra":
                    mapper = "Kyshiro";
                    break;
            }

            setTime(193076);
            drawText(t, t + BEAT4*2, "Beatmap by", true);
            drawText(t, t + BEAT4*2, mapper);

            setTime(195835);
            drawText(t, t + BEAT4*2, "Storyboard by", true);
            drawText(t, t + BEAT4*2, "Apis035 & -Ady");
        }

        void drawText(int startTime, int endTime, string text, bool up = false)
        {
            var p1 = new Vector2(320, 360f + (up ? 6 : -6));
            var p2 = new Vector2(p1.X, p1.Y + (up ? -20 : 20));

            var s  = GetLayer(text == mapper ? LAYER_MAPPER : LAYER_CREDIT)
                    .CreateSprite(font.GetTexture(text).Path, OsbOrigin.Centre, p1);

            s.Scale (startTime, up ? .3f : .5f);
            s.MoveY (OsbEasing.OutElasticHalf,  startTime, endTime - 1000, p1.Y, p2.Y);
            s.MoveY (OsbEasing.InExpo,   endTime - 1000, endTime, p2.Y, p1.Y);
            s.Fade  (OsbEasing.OutCubic, startTime, startTime + 1000, 0, 1);
            s.Fade  (OsbEasing.InCubic,  endTime - 1000, endTime, 1, 0);
        }
    }
}
