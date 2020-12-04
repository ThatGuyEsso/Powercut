using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen instance;
    public GameObject cam;
    public GameObject loadingScreen;
    public GameObject loadingText;
    public float fadeRate;
    public Image screenImage;


    public event FadeCompleted OnFadeComplete;
    public delegate void FadeCompleted( );
    public event CurtainCall OnCurtainCallEnd;
    public delegate void CurtainCall();
    private void Awake()
    {

        if (instance == false)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
   
    }
        


    public void ToggleScreen( bool isOn)
    {
        cam.SetActive(isOn);
        loadingText.SetActive(isOn);
        loadingScreen.SetActive(isOn);
    }
    public void BeginFade(bool fadeIn)
    {
        if (fadeIn)
        {
            StartCoroutine(FadeIn());
        }
        else
        {
            StartCoroutine(FadeOut());
        }
    }
    private IEnumerator FadeIn()
    {
        float oppacity = 0;
        cam.SetActive(true);
        loadingScreen.SetActive(true);
        loadingText.SetActive(false);
        screenImage.color = new Color(screenImage.color.r, screenImage.color.g, screenImage.color.b, 0f);
        

        while (oppacity < 1f)
        {
            oppacity += 0.01f; 
            screenImage.color = new Color(screenImage.color.r, screenImage.color.g, screenImage.color.b, oppacity);
            yield return new WaitForSeconds(fadeRate);
        }
        loadingText.SetActive(true);
        OnFadeComplete?.Invoke();
    }

    private IEnumerator FadeOut()
    {
        float oppacity = 1;
        cam.SetActive(true);
        loadingScreen.SetActive(true);
        loadingText.SetActive(false);
        screenImage.color = new Color(screenImage.color.r, screenImage.color.g, screenImage.color.b, oppacity);


        while (oppacity > 0f)
        {
            oppacity -= 0.01f;
            screenImage.color = new Color(screenImage.color.r, screenImage.color.g, screenImage.color.b, oppacity);
            yield return new WaitForSeconds(fadeRate);
        }
        loadingScreen.SetActive(false);
        cam.SetActive(false);
        OnFadeComplete?.Invoke();
    }

    public void StartCurtainCall(float callTime, bool useUICam)
    {
        StartCoroutine(CallCurtain(callTime,  useUICam));
    }
    private IEnumerator CallCurtain(float callTime, bool useUICam)
    {
        float oppacity = 0;
        cam.SetActive(useUICam);
        loadingScreen.SetActive(true);
        loadingText.SetActive(false);
        screenImage.color = new Color(screenImage.color.r, screenImage.color.g, screenImage.color.b, oppacity);


        while (oppacity < 1f)
        {
            oppacity += 0.01f;

            screenImage.color = new Color(screenImage.color.r, screenImage.color.g, screenImage.color.b, oppacity);
            yield return new WaitForSeconds(callTime);
        }

        OnCurtainCallEnd?.Invoke();
    }
   
}



