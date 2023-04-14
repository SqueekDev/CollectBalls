using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundButton : MusicButton
{
    private const string _soundKeyName = "Sound";

    protected override string GetKeyName()
    {
        return _soundKeyName;
    }
}
