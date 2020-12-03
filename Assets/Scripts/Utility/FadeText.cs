using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FadeText : MonoBehaviour
{
    public TextMeshProUGUI[] UITexts;


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
                text.color = new Color(text.color.r, text.color.g, text.color.b, oppacity);
            }
            yield return new WaitForSeconds(fadeTime);
    
        }

        OnTextFadeEnd?.Invoke();
    }

    private IEnumerator TextFadeOut(float fadeTime, float fadeMagnitude)
    {
        float oppacity = 1f;
        foreach (TextMeshProUGUI text in UITexts)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, oppacity);
        }


        while (oppacity > 0f)
        {
            oppacity -= fadeMagnitude;
          
            foreach (TextMeshProUGUI text in UITexts)
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, oppacity);
            }
            yield return new WaitForSeconds(fadeTime);

        }

        OnTextFadeEnd?.Invoke();
    }
}
