using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILanguageText : MonoBehaviour
{
    public string dataKey;
    [TextArea(1, 10)]
    public string defaultText;
    private string languageKey;
    private void Update()
    {
        if (languageKey != RPGLanguageManager.CurrentLanguageKey)
        {
            var textComponent = GetComponent<Text>();
            if (textComponent != null)
            {
                var text = "";
                if (RPGLanguageManager.Texts.TryGetValue(dataKey, out text))
                    textComponent.text = text;
                else
                    textComponent.text = defaultText;
            }
            languageKey = RPGLanguageManager.CurrentLanguageKey;
        }
    }

    void OnValidate()
    {
        var textComponent = GetComponent<Text>();
        textComponent.text = defaultText;
    }
}
