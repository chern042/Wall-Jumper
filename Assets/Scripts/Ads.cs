using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ads : MonoBehaviour
{
#if UNITY_ANDROID
        string appKey = "1c2457a4d";
#elif UNITY_IOS
    string appKey = "1c245b3d5";
#elif UNITY_EDITOR
        string appKey = "1c2457a4d";
#else
        string appKey = "unexpected_platform";
#endif
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    static void OnBeforeSplashScreen()
    {

        Debug.Log("**********************BEFORE SPLASH SCREEN/FIRST SCENE LOAD****************************");
        PlayerPrefs.SetString("bannerHidden", "false");
        PlayerPrefs.SetString("bannerLoaded", "false");
        PlayerPrefs.SetString("sdkInitComplete", "false");
    }


    //void Awake()
    //{
    //    Debug.Log("**********************CALLING AWAKE() AND INITIALIZING AD EVENTS****************************");
    //    IronSource.Agent.getAdvertiserId();
    //}
    void Start()
    {

        if (PlayerPrefs.GetString("sdkInitComplete") == "false")
        {


            // Debug.Log("************TEST SUITE ENABLED");
            //IronSource.Agent.setMetaData("is_test_suite", "enable");



            Debug.Log("************VALIDATING INTEGRATION");
            IronSource.Agent.validateIntegration();

            Debug.Log("************UNITY VERSION: " + IronSource.unityVersion());
            // SDK init
            Debug.Log("************SDK INITIALIZATION BEGIN FOR AD AGENT " + appKey);



            IronSource.Agent.init(appKey);


            IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent;
            IronSourceBannerEvents.onAdLoadedEvent += BannerOnAdLoadedEvent;
            IronSourceBannerEvents.onAdLoadFailedEvent += BannerOnAdLoadFailedEvent;
            //IronSourceBannerEvents.onAdScreenPresentedEvent += BannerOnAdScreenPresentedEvent;
            //IronSourceBannerEvents.onAdScreenDismissedEvent += BannerOnAdScreenDismissedEvent;
            //IronSourceBannerEvents.onAdLeftApplicationEvent += BannerOnAdLeftApplicationEvent;
        }

    }
    
    //private void BannerOnAdLeftApplicationEvent(IronSourceAdInfo info)
    //{
    //    Debug.Log("**********PLAYER LEFT APP AD LEFT EVENT: " + info.ToString());

    //}

    //private void BannerOnAdScreenDismissedEvent(IronSourceAdInfo info)
    //{
    //    Debug.Log("**********BANNER ON AD SCREEN DISMISSED EVENT: " + info.ToString());

    //}

    //private void BannerOnAdScreenPresentedEvent(IronSourceAdInfo info)
    //{
    //    Debug.Log("**********Banner Ad Screen Presented: " + info.ToString());
    //}

    private void BannerOnAdLoadedEvent(IronSourceAdInfo info)
    {
       // Debug.Log("**********Banner Load Ad Succeeded: " + info.ToString());
        PlayerPrefs.SetString("bannerLoaded", "true");

       // Debug.Log("**********Hiding Banner***********");
        PlayerPrefs.SetString("bannerHidden", "true");

        IronSource.Agent.hideBanner();
    }

    private void BannerOnAdLoadFailedEvent(IronSourceError error)
    {
        //Debug.Log("**********Banner Load Ad Failed: " + error.ToString());
       IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM, "Game_Over");
        PlayerPrefs.SetString("bannerLoaded","false");
    }



     private void SdkInitializationCompletedEvent()
    {
        //IronSource.Agent.launchTestSuite();
        //Debug.Log("************SDK INITIALIZATION COMPLETE");

        //IronSource.Agent.getAdvertiserId();
        PlayerPrefs.SetString("sdkInitComplete", "true");
        //Debug.Log("************LOADING BANNER SDK INIT");
#if UNITY_ANDROID
        IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM, "Jump_Man_Android");
#elif UNITY_IOS
        IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM, "Jump_Man_iOS");
#endif

    }




    private void OnApplicationQuit()
    {
       // Debug.Log("**********ON APPLICATION QUIT EVENT CALLED***************");
        IronSource.Agent.destroyBanner();

    }


    // Update is called once per frame
    void Update()
    {

        if (GameEndMenu.gameEnded == true && PlayerPrefs.GetString("sdkInitComplete") == "true" && PlayerPrefs.GetString("bannerLoaded") == "true" && PlayerPrefs.GetString("bannerHidden") == "true")
        {
           // Debug.Log("************DISPLAYING BANNER*********");
            IronSource.Agent.displayBanner();
            PlayerPrefs.SetString("bannerHidden", "false");
        }
        else if(GameEndMenu.gameEnded == false && PlayerPrefs.GetString("sdkInitComplete") == "true" && PlayerPrefs.GetString("bannerLoaded") == "true" && PlayerPrefs.GetString("bannerHidden") == "false")
        {
            //Debug.Log("************HIDING BANNER************");
            IronSource.Agent.hideBanner();
            PlayerPrefs.SetString("bannerHidden", "true");
        }

    }

}
