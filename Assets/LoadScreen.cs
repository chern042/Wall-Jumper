using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LoadScreen : MonoBehaviour
{
    [SerializeField] GameObject loadingPanel;
    public static bool gameLoaded;
    // Start is called before the first frame update
    void Start()
        
    {
        gameLoaded = false;
        //loadingPanel = GetComponent<GameObject>();
    }




    private void OnPreRender()
    {
        loadingPanel.SetActive(true);
       // gameLoaded = false;
    }
    private void OnRenderObject()
    {
        loadingPanel.SetActive(false);
        
        gameLoaded = true;
    }
}
