public class SoundButton : MusicButton
{
    private const string _soundKeyName = "Sound";

    protected override string GetKeyName()
    {
        return _soundKeyName;
    }
}
