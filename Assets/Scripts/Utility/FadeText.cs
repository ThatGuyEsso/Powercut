using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FadeText : MonoBehaviour
{
    private List<TextMeshProUGUI> UITexts = new List<TextMeshProUGUI>();


    public event TextFadeComplete OnTextFadeEnd;
    public delegate void TextFadeComplete();
    public void BeginTextFadeIn(float fadeTime, float fadeMagnitude)
    {
        StartCoroutine(TextFadeIn(fadeTime, fadeMagnitude));
    }
    public void BeginTextFadeOut(float fadeTime, float fadeMagnitude)
    {
        StartCoroutine(TextFadeOut(fadeTime, fadeMagnitude));
    }


    private IEnumerator TextFadeIn(float fadeTime, float fadeMagnitude)
    {
        if (UITexts.Count > 0)
        {

            float oppacity = 0;
            foreach(TextMeshProUGUI text in UITexts)
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, oppacity);
            }


            while (oppacity < 1f)
            {
                oppacity += fadeMagnitude;
           
                foreach (TextMeshProUGUI text in UITexts)
                {
                    if (oppacity >= 0.95) oppacity = 1f;
                    text.color = new Color(text.color.r, text.color.g, text.color.b, oppacity);
                }
           
                yield return new WaitForSeconds(fadeTime);
    
            }
        }

        OnTextFadeEnd?.Invoke();
    }

    private IEnumerator TextFadeOut(float fadeTime, float fadeMagnitude)
    {
        if (UITexts.Count > 0)
        {

            float oppacity = 1f;
            foreach (TextMeshProUGUI text in UITexts)
            {
                if (text.isActiveAndEnabled)
                {

                    text.color = new Color(text.color.r, text.color.g, text.color.b, oppacity);
                }
            }


            while (oppacity > 0f)
            {
                oppacity -= fadeMagnitude;
          
                foreach (TextMeshProUGUI text in UITexts)
                {
                    if (text.isActiveAndEnabled)
                    {

                        if (oppacity <= 0.05) oppacity = 0f;
                        text.color = new Color(text.color.r, text.color.g, text.color.b, oppacity);
                    }
                }
     
                yield return new WaitForSeconds(fadeTime);

            }
        }

        OnTextFadeEnd?.Invoke();
    }

    public void AddScreenElements(List<TextMeshProUGUI> list)
    {
        UITexts = list;
    }

    public void ClearList() { UITexts.Clear(); }

}
