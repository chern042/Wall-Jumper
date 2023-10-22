using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ad : MonoBehaviour
{
    private RectTransform objectToScale;
    private float prevScreenSizeTest;
    // Start is called before the first frame update
    void Start()
    {
        objectToScale = GetComponent<RectTransform>();
        float aspectRatio = ((float)Screen.height / (float)Screen.width);
        if(aspectRatio >=1.6f)
         {
            float sizeDelta = (Screen.width*0.666f) / objectToScale.rect.width;
            objectToScale.transform.localScale = new Vector2(sizeDelta, sizeDelta);
            objectToScale.transform.position = new Vector3(objectToScale.transform.position.x, ((objectToScale.rect.height * sizeDelta) / 2) + 100f, objectToScale.transform.position.z);
            prevScreenSizeTest = Screen.width;
        }
        else
        {
            float sizeDelta = (Screen.width * 0.5f) / objectToScale.rect.width;
            objectToScale.transform.localScale = new Vector2(sizeDelta, sizeDelta);
            objectToScale.transform.position = new Vector3(objectToScale.transform.position.x, (float)Screen.width / 2f, objectToScale.transform.position.z);

            prevScreenSizeTest = Screen.width;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Screen.width != prevScreenSizeTest)
        {

            float aspectRatio = ((float)Screen.height / (float)Screen.width);

            Debug.Log("SCREEN RATIO (H/W): " + aspectRatio);
            Debug.Log("CHANGED SCREEN WIDTH FROM " + prevScreenSizeTest + " TO " + Screen.width);





            if (aspectRatio >= 1.6f)
            {
                float sizeDelta = (Screen.width * 0.666f) / objectToScale.rect.width;
                Debug.Log("CHANGED MENU SCALE FROM " + objectToScale.transform.localScale + " TO " + sizeDelta);
                Debug.Log("CHANGED MENU WIDTH FROM " + objectToScale.rect.width * objectToScale.transform.localScale + " TO " + objectToScale.rect.width * sizeDelta);
                objectToScale.transform.localScale = new Vector2(sizeDelta, sizeDelta);
                objectToScale.transform.position = new Vector3(objectToScale.transform.position.x, ((objectToScale.rect.height * sizeDelta) / 2) + 100f, objectToScale.transform.position.z);

            }
            else
            {
                float sizeDelta = (Screen.width * 0.5f) / objectToScale.rect.width;
                Debug.Log("CHANGED MENU SCALE FROM " + objectToScale.transform.localScale + " TO " + sizeDelta);
                Debug.Log("CHANGED MENU WIDTH FROM " + objectToScale.rect.width * objectToScale.transform.localScale + " TO " + objectToScale.rect.width * sizeDelta);
                objectToScale.transform.localScale = new Vector2(sizeDelta, sizeDelta);
                objectToScale.transform.position = new Vector3(objectToScale.transform.position.x, (float)Screen.width/2f, objectToScale.transform.position.z);
            }
            prevScreenSizeTest = Screen.width;


        }
    }
}
