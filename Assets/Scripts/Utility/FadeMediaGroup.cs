using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class FadeMediaGroup : MonoBehaviour
{
    [Header("Media Elements")]
    [SerializeField] private List<Image> images;
    [Tooltip("Elements that have specific oppacity values")] [SerializeField] private List<Image> limitedImages; 
    [SerializeField] private List<TextMeshProUGUI> texts;

    [Header("Fade Settings")]
    [SerializeField] private float fadeInRate, fadeOutRate;
    [SerializeField] private float maxLimitedOppacity;

    public delegate void FadeCompleteDelgate(GameObject go);
    public event FadeCompleteDelgate OnFadeComplete;

    public void BeginFadeIn()
    {
        StopAllCoroutines();
        StartCoroutine(FadeIn());
    }

    public void BeginFadeOut()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut());
    }
    private IEnumerator FadeIn()
    {
        //set oppacity to zero to initialise fade in
        float oppacity = 0f;
        foreach (Image image in images)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, oppacity);
        }
        foreach (Image limited in limitedImages)
        {
            limited.color = new Color(limited.color.r, limited.color.g, limited.color.b, oppacity);
        }
        foreach (TextMeshProUGUI txt in texts)
        {
            txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, oppacity);
        }


        //start fading in elements 
        while (oppacity < 1f)
        {
            oppacity += 1f / 100f;
            foreach (Image image in images)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, oppacity);
            }
            foreach (TextMeshProUGUI txt in texts)
            {
                txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, oppacity);
            }

            //ensure background oppacity does not exceed given max
            if (oppacity <= maxLimitedOppacity)
            {
                foreach (Image limited in limitedImages)
                {
                    limited.color = new Color(limited.color.r, limited.color.g, limited.color.b, oppacity);
                }
            }

            yield return new WaitForSeconds(fadeInRate);
        }

        //at the end reset each value
        foreach (Image image in images)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
        }
        foreach (Image limited in limitedImages)
        {
            limited.color = new Color(limited.color.r, limited.color.g, limited.color.b, maxLimitedOppacity);
        }
        foreach (TextMeshProUGUI txt in texts)
        {
            txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 1f);
        }

        //call complete
        OnFadeComplete?.Invoke(gameObject);


    }
    public IEnumerator FadeOut()
    {
        //set oppacity to one to initialise fade in
        float oppacity = 1f;
    
 
        //start fading in tutorial prompts
        while (oppacity > 0f)
        {
            //Simultaneously fade in image elements
            oppacity -= 1f / 100f;
          
            foreach (Image image in images)
            {
                if (oppacity <= image.color.a)//Only decrease alpha of image
                 image.color = new Color(image.color.r, image.color.g, image.color.b, oppacity);
            }
            foreach (TextMeshProUGUI txt in texts)
            {
                if (oppacity <= txt.color.a)//Only decrease alpha of image
                    txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, oppacity);
            }

            //ensure background oppacity does not exceed given max
            if (oppacity <= maxLimitedOppacity)
            {
                foreach (Image limited in limitedImages)
                {
                    limited.color = new Color(limited.color.r, limited.color.g, limited.color.b, oppacity);
                }
            }

            yield return new WaitForSeconds(fadeOutRate);
        }

        //at the end reset each value
        foreach (Image image in images)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
        }
        foreach (Image limited in limitedImages)
        {
            limited.color = new Color(limited.color.r, limited.color.g, limited.color.b, 0f);
        }
        foreach (TextMeshProUGUI txt in texts)
        {
            txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 0f);
        }
        //call complete
        OnFadeComplete?.Invoke(gameObject);

    }


}
