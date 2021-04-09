using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DamageNumber : MonoBehaviour
{
    private TextMeshPro text;
    [Header("Animation Rates")]

    [SerializeField]
    private float scaleUpRate;
    [SerializeField]
    private float scaledownRate;
    [SerializeField]
    private float fadeRate;
    [SerializeField]
    private float scaleMultipler;
    private float scale;

    [Header("Range Settings")]
    [SerializeField]
    private float minTimeBeforeShrink;
    [SerializeField]
    private float maxTimeBeforeShrink;
    [SerializeField]
    private float minTravelDistance;
    [SerializeField]
    private float maxTravelDistance;
    [SerializeField]
    private float minSpeed;
    [SerializeField]
    private float maxSize;
    [SerializeField]
    private float maxSpeed;
    [Header("Appearance Settings")]

   
    private float speed;
    private float traveldDistance;
    private float timeBeforeShrink;
    private Vector3 travelDir;
    [SerializeField]
    private Gradient colorGradient;
    //State settings
    bool finalSize;
    bool shouldShrink;
    public void Init()
    {
        text = gameObject.GetComponent<TextMeshPro>();//Cache text
 
    
        timeBeforeShrink = Random.Range(minTimeBeforeShrink, maxTimeBeforeShrink);
        finalSize = false;
        traveldDistance = Random.Range(minTravelDistance, maxTravelDistance);
        speed = Random.Range(minSpeed, maxSpeed);
    }

    public void Update()
    {
        //
        //transform.position += travelDir * speed*Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, transform.position+(travelDir * traveldDistance), speed);

        if (finalSize == false)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(scale * scaleMultipler, scale * scaleMultipler, scale * scaleMultipler), (scale / scale * scaleMultipler)*scaleUpRate);
            if (transform.localScale == new Vector3(scale * scaleMultipler, scale * scaleMultipler, scale * scaleMultipler))
            {
                finalSize = true;
            }
        }


        if (finalSize == true)
        {
            if (timeBeforeShrink <= 0f)
            {
                shouldShrink = true;
            }
            else
            {
                timeBeforeShrink -= Time.deltaTime;
            }
        }


        if (shouldShrink)
        {



            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0, 0, 0), (scale / scale * scaleMultipler) * scaledownRate);

            text.color = Vector4.Lerp(text.color, new Vector4(text.color.r, text.color.g, text.color.b, 0f), fadeRate);
            if (text.color.a <= 0.05f)
            {
                ObjectPoolManager.Recycle(gameObject);
            }
        }
    }

    public void OnEnable()
    {
        timeBeforeShrink = Random.Range(minTimeBeforeShrink, maxTimeBeforeShrink);
        finalSize = false;
        shouldShrink = false;
        traveldDistance = Random.Range(minTravelDistance, maxTravelDistance);
        speed = Random.Range(minSpeed, maxSpeed);
    }

    private void DisplayDamage(float damageAmount)
    {
        text.text = Mathf.FloorToInt(damageAmount).ToString();
    }


    public void SetTextValues(float damageAmount,float targetMaxHealth, Vector3 dir)
    {
        float percentageDmg = damageAmount / targetMaxHealth;
        scale = percentageDmg;

        if (scale > maxSize) scale = maxSize;
        float gradient = percentageDmg;
        transform.localScale = new Vector3(scale, scale, scale);
        text.color = colorGradient.Evaluate(gradient);
        travelDir = dir;
        DisplayDamage(damageAmount);
    }
    public void SetTextValuesAtScale(float damageAmount, float targetMaxHealth, Vector3 dir,float healthScale)
    {
        float percentageDmg = damageAmount / targetMaxHealth;
        scale = percentageDmg* healthScale;

        if (scale > maxSize) scale = maxSize;
        float gradient = percentageDmg;
        transform.localScale = new Vector3(scale, scale, scale);
        text.color = colorGradient.Evaluate(gradient);
        travelDir = dir;
        DisplayDamage(damageAmount);
    }
}
