using Agava.YandexGames;
using Lean.Localization;
using UnityEngine;

namespace YandexSDK
{
    public class Localization : MonoBehaviour
    {
        private const string EnglishCode = "en";
        private const string RussianCode = "ru";
        private const string TurkishCode = "tr";
        private const string English = "English";
        private const string Russian = "Russian";
        private const string Turkish = "Turkish";

        private string _language;

        private void OnEnable()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            _language = YandexGamesSdk.Environment.i18n.lang;
#endif
            switch (_language)
            {
                case EnglishCode:
                    LeanLocalization.SetCurrentLanguageAll(English);
                    break;
                case RussianCode:
                    LeanLocalization.SetCurrentLanguageAll(Russian);
                    break;
                case TurkishCode:
                    LeanLocalization.SetCurrentLanguageAll(Turkish);
                    break;
                default:
                    LeanLocalization.SetCurrentLanguageAll(English);
                    break;
            }

            LeanLocalization.UpdateTranslations();
        }
    }
}