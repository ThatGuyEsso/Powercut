using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GadgetDisplay : MonoBehaviour
{
    [Header("Postioning Settings")]
    public float taskYOffset;
    public float taskXOffset;
    public Sprite flashBangIcon;

    [Header("Display Settings")]
    public GameObject gadgetTemplatePrefab;
    private List<GadgetTemplate> gadgets = new List<GadgetTemplate>();


    private RectTransform rt;
    private void Awake()
    {
        //Cache referencs
        rt = gameObject.GetComponent<RectTransform>();

    }


    public void GenerateNewGadgetTemplate(GadgetTypes type, int amount)
    {
        GadgetTemplate newTemplate = Instantiate(gadgetTemplatePrefab, transform.position, Quaternion.identity).GetComponent<GadgetTemplate>();
        Debug.Log(newTemplate);
        newTemplate.transform.parent = rt.transform;
        RectTransform templateRect = newTemplate.gameObject.GetComponent<RectTransform>();
        int positionOffset = gadgets.Count - 1;
        if (templateRect == true)
        {

            Vector2 targetPos = rt.transform.position;


            templateRect.anchoredPosition = new Vector2(taskXOffset*positionOffset, taskYOffset);

        }
        Sprite icon = GetRespectiveIcon(type);

        newTemplate.SetUpTemplate(icon, amount,type);
        gadgets.Add(newTemplate);
    }

    private Sprite GetRespectiveIcon(GadgetTypes type)
    {
        switch (type)
        {
            case GadgetTypes.FlashBang:

                return flashBangIcon;


            default:
                return flashBangIcon;
        }
    }

    public void DecreaseRespectiveGadgetCounter(GadgetTypes type)
    {
        foreach(GadgetTemplate gadget in gadgets)
        {
            if (gadget.type == type) gadget.DecrementCounter();
        }
    }
}
