using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Credits: https://forum.unity.com/threads/device-screen-rotation-event.118638/#post-4587073
public class Orientation : MonoBehaviour
{
    [SerializeField] private GridController gridController;
    private RectTransform rectTransform;
    private GameObject landscape;
    private GameObject portrait;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        landscape = transform.Find("landscape").gameObject;
        portrait = transform.Find("portrait").gameObject;
        SetOrientation();
    }
    void SetOrientation()
    {
        if(rectTransform == null) return;
        bool verticalOrientation = rectTransform.rect.width < rectTransform.rect.height ? true : false;
        portrait.SetActive(verticalOrientation);
        landscape.SetActive(!verticalOrientation);
    }
    void OnRectTransformDimensionsChange()
    {
        SetOrientation();
        gridController.ScaleCameraToGrid(); //Could set this up cleaner with an event call or something. It also gets called on exit right now which throws an exception.
    }
}
