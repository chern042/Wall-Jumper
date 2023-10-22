using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ads : MonoBehaviour
{

    bool loadBanner = false;

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_ANDROID
        string appKey = "1c2457a4d";
#elif UNITY_IPHONE
        string appKey = "1c245b3d5";
#elif UNITY_IOS
        string appKey = "1c245b3d5";
#elif UNITY_EDITOR
        string appKey = "1c245b3d5";
#else
        string appKey = "unexpected_platform";
#endif

        IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent;


        IronSource.Agent.setMetaData("is_test_suite", "enable");



        Debug.Log("unity-script: IronSource.Agent.validateIntegration");
        IronSource.Agent.validateIntegration();

        Debug.Log("unity-script: unity version" + IronSource.unityVersion());

        // SDK init
        Debug.Log("unity-script: IronSource.Agent.init");
        IronSource.Agent.init(appKey);
        IronSource.Agent.loadBanner(IronSourceBannerSize.LARGE, IronSourceBannerPosition.BOTTOM);





    }

private void SdkInitializationCompletedEvent()
    {
        Debug.Log("TESSSST");
    IronSource.Agent.launchTestSuite();
    }

    // Update is called once per frame
    void Update()
    {

        if (GameEndMenu.gameEnded == true && loadBanner == false)
        {
            IronSource.Agent.displayBanner();


            loadBanner = true;

        }

    }

}
