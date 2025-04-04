using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Size_canvas : MonoBehaviour
{
    public float resoX = 1920;
    public float resoY = 1080;

    private CanvasScaler can;
    // Start is called before the first frame update
    void Start()
    {
        can = GetComponent<CanvasScaler>();
        SetInfo();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetInfo()
    {
        resoX = (float)Screen.currentResolution.width;
        resoY = (float)Screen.currentResolution.height;
        can.referenceResolution = new Vector2(resoX, resoY);
    }
}
