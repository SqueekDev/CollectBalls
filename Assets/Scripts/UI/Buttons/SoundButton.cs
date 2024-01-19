namespace UI
{
    public class SoundButton : MusicButton
    {
        private const string SoundKeyName = "Sound";

        protected override string GetKeyName()
        {
            return SoundKeyName;
        }
    }
}