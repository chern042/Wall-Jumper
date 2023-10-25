using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ads : MonoBehaviour
{

    bool loadBanner = false;
    bool initComplete = false;

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
        string appKey = "1c2457a4d";
#else
        string appKey = "unexpected_platform";
#endif


        Debug.Log("************TEST SUITE ENABLED");

       //IronSource.Agent.setMetaData("is_test_suite", "enable");



        Debug.Log("************VALIDATING INTEGRATION");
        IronSource.Agent.validateIntegration();

        Debug.Log("************UNITY VERSION: " + IronSource.unityVersion());
        // SDK init
        Debug.Log("************INITIALIZING AD AGENT "+appKey);
        IronSource.Agent.init(appKey);


        Debug.Log("************LOADING BANNER");
        //IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);

        IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent;
        IronSourceBannerEvents.onAdLoadedEvent += BannerOnAdLoadedEvent;
        IronSourceBannerEvents.onAdLoadFailedEvent += BannerOnAdLoadFailedEvent;
        IronSourceBannerEvents.onAdScreenPresentedEvent += BannerOnAdScreenPresentedEvent;




    }

    private void BannerOnAdScreenPresentedEvent(IronSourceAdInfo info)
    {
        Debug.Log("**********Banner Ad Presented: " + info.ToString());
    }

    private void BannerOnAdLoadedEvent(IronSourceAdInfo info)
    {
        Debug.Log("**********Banner Load Ad Succeeded: " + info.ToString());
        IronSource.Agent.displayBanner();

    }

    private void BannerOnAdLoadFailedEvent(IronSourceError error)
    {
        Debug.Log("**********Banner Load Ad Failed: " + error.ToString());
    }

    private void SdkInitializationCompletedEvent()
    {
        Debug.Log("************INITIALIZATION COMPLETE");

        Debug.Log("************LOADING BANNER");

        initComplete = true;

        //IronSource.Agent.launchTestSuite();
    }

    void OnApplicationPause(bool isPaused)
    {
        IronSource.Agent.onApplicationPause(isPaused);
    }



    // Update is called once per frame
    void Update()
    {

        if (GameEndMenu.gameEnded == true && loadBanner == false && initComplete == true)
        {
            IronSource.Agent.getPlacementInfo("Game_Over");
            IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);
            loadBanner = true;


        }
        if(GameEndMenu.gameEnded == false && loadBanner == true && initComplete == true)
        {
            IronSource.Agent.destroyBanner();
            loadBanner = false;
        }

    }

}
