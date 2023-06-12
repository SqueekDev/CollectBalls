using UnityEngine;
using Agava.YandexGames;
using Lean.Localization;

public class Localization : MonoBehaviour
{
    private const string EnglishCode = "en";
    private const string RussianCode = "ru";
    private const string TurkishCode = "tr";
    private const string English = "English";
    private const string Russian = "Russian";
    private const string Turkish = "Turkish";

    private void OnEnable()
    {
        string language = YandexGamesSdk.Environment.i18n.lang;

        if (language != null)
        {
            if (language == EnglishCode)
                LeanLocalization.SetCurrentLanguageAll(English);
            else if (language == RussianCode)
                LeanLocalization.SetCurrentLanguageAll(Russian);
            else if (language == TurkishCode)
                LeanLocalization.SetCurrentLanguageAll(Turkish);
            else
                LeanLocalization.SetCurrentLanguageAll(English);
        }
        else
        {
            LeanLocalization.SetCurrentLanguageAll(English);
        }

        LeanLocalization.UpdateTranslations();
    }
}
