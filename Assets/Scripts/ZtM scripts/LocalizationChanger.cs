using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LocalizationChanger : MonoBehaviour
{
    [SerializeField] private int LocaleID;

    private void Awake()
    {
        LocaleID = PlayerPrefs.GetInt("locale", LocalizationSettings.AvailableLocales.Locales.IndexOf(LocalizationSettings.SelectedLocale));
        if(LocaleID < 0)
        {
            LocaleID = 1;
        }

        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[LocaleID];
    }

    public void SetLocalization(int index)
    {
        LocaleID = index;
        PlayerPrefs.SetInt("locale", LocaleID);
        PlayerPrefs.Save();

        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[LocaleID];
    }
}
