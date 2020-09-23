using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPScript : MonoBehaviour
{
    public GameObject thisObject;

    private ObjectStatus status;

    private Image hPBarImage;

    private Color Color;

    private new Camera camera;

    private void OnEnable()
    {
        transform.position = Vector3.one * 50000;
        Color = Color.green;
    }
    public void HPBarInit()
    {
        transform.position = Vector3.one * 50000;
        Color = Color.green;
    }

    private void Start()
    {
        camera = Camera.main;

        status = thisObject.GetComponent<ObjectStatus>();

        hPBarImage = GetComponent<Image>();
    }


    // Update is called once per frame
    void Update()
    {
        transform.position = camera.WorldToScreenPoint(thisObject.transform.position);

        hPBarImage.fillAmount = status.CurHP / (float)status.MaxHP;
        
        if (hPBarImage.fillAmount > 0.5f)
        {
            Color.r = 2f - hPBarImage.fillAmount * 2f;
        }
        else
        {
            Color.r = 1;
            Color.g = 2f * hPBarImage.fillAmount;
        }

        hPBarImage.color = Color;
    }
}
