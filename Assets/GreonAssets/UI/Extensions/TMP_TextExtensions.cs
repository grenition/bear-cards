using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GreonAssets.UI.Extensions
{
    public static class TMP_TextExtensions
    {
        private static Dictionary<TMP_Text, Coroutine> activeCoroutines = new Dictionary<TMP_Text, Coroutine>();
        
        public static void TypewriterEffect(this TMP_Text textComponent, string fullText, float duration)
        {
            if (textComponent == null) return;
            
            if (activeCoroutines.TryGetValue(textComponent, out Coroutine existingCoroutine))
            {
                textComponent.StopCoroutine(existingCoroutine);
                activeCoroutines.Remove(textComponent);
            }

            Coroutine newCoroutine = textComponent.StartCoroutine(TypewriterCoroutine(textComponent, fullText, duration));
            activeCoroutines[textComponent] = newCoroutine;
        }

        private static IEnumerator TypewriterCoroutine(TMP_Text textComponent, string fullText, float duration)
        {
            textComponent.text = string.Empty;
            int totalChars = fullText.Length;
            if (totalChars == 0)
            {
                yield break;
            }

            float interval = duration / totalChars;

            for (int i = 0; i <= totalChars; i++)
            {
                textComponent.text = fullText.Substring(0, i);
                yield return new WaitForSeconds(interval);
            }

            activeCoroutines.Remove(textComponent);
        }
    }
}