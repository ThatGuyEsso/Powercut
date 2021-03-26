using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class PointerManager : MonoBehaviour
{

    public static PointerManager instance;
    [SerializeField] private Image gfx;
    [SerializeField] private  Sprite crossHair;
    [SerializeField] private Sprite pointer;
    [SerializeField] private Camera activeCamera;
    [SerializeField] private Vector2 pointerOffset;
    [SerializeField] private Vector2 crossHairOffset;
    [SerializeField] private Vector2 currentOffset = Vector2.zero;
    private void Awake()
    {
        if (instance == false)
        {
            instance = this;
            Cursor.visible = false;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void LateUpdate()
    {
     
        gfx.rectTransform.position =Mouse.current.position.ReadValue()+ currentOffset;
    }


    public void SetActiveCamera(Camera cam)
    {
        activeCamera = cam;
    }


    public void SwitchToPointer()
    {
        currentOffset = pointerOffset;
        gfx.sprite = pointer;
    }
    public void SwitchToCrossHair()
    {
        currentOffset = crossHairOffset;
        gfx.sprite = crossHair;
    }

    public void SetCursorVisibility(bool isVisble)
    {
        gfx.gameObject.SetActive(isVisble);
    }
}
