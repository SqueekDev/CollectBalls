using UnityEngine;
using Agava.YandexGames;
using Lean.Localization;

public class Localization : MonoBehaviour
{
    private const string _englishCode = "en";
    private const string _russianCode = "en";
    private const string _turkishCode = "en";
    private const string _english = "English";
    private const string _russian = "Russian";
    private const string _turkish = "Turkish";

    private void OnEnable()
    {
        string language = YandexGamesSdk.Environment.i18n.lang;

        if (language != null)
        {
            if (language == _englishCode)
                LeanLocalization.SetCurrentLanguageAll(_english);
            else if (language == _russianCode)
                LeanLocalization.SetCurrentLanguageAll(_russian);
            else if (language == _turkishCode)
                LeanLocalization.SetCurrentLanguageAll(_turkish);
            else
                LeanLocalization.SetCurrentLanguageAll(_english);
        }
        else
            LeanLocalization.SetCurrentLanguageAll(_english);

        LeanLocalization.UpdateTranslations();
    }
}
