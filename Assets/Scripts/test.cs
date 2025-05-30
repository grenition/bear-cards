using UnityEngine;
using UnityEngine.Localization.Settings;

public class test : MonoBehaviour
{
    [SerializeField] private string key;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            print(LocalizationSettings.StringDatabase.GetLocalizedString(key));
        }
    }
}
