namespace StorybrewScripts {
    public class DisableBG : StorybrewCommon.Scripting.StoryboardObjectGenerator {
        public override void Generate() => GetLayer("Disable background").CreateSprite(Beatmap.BackgroundPath).Fade(0,0);}}
