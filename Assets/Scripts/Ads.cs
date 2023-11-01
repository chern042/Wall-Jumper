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
    bool bannerLoaded;
    bool bannerLoading;
    bool adsInitComplete;


    void Awake()
    {
        Debug.Log("**********************CALLING AWAKE()****************************");
       // PlayerPrefs.SetString("adsInitialized", "false");
        bannerLoaded = false;
       // bannerLoading = false;
       // IronSource.Agent.getAdvertiserId();


    }
    void Start()
    {
        Debug.Log("**********************CALLING START()****************************");
        /*
        if (PlayerPrefs.GetString("adsInitialized") == "true")
        {
            adsInitComplete = true;
        }
        else
        {
            adsInitComplete = false;
        }


        if (adsInitComplete == false)
         {*/
            Debug.Log("************TEST SUITE ENABLED");



            Debug.Log("************VALIDATING INTEGRATION");
            IronSource.Agent.validateIntegration();

            Debug.Log("************UNITY VERSION: " + IronSource.unityVersion());
            // SDK init
            Debug.Log("************INITIALIZING AD AGENT " + appKey);



            IronSource.Agent.init(appKey);


            IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent;
            IronSourceBannerEvents.onAdLoadedEvent += BannerOnAdLoadedEvent;
            IronSourceBannerEvents.onAdLoadFailedEvent += BannerOnAdLoadFailedEvent;
            IronSourceBannerEvents.onAdScreenPresentedEvent += BannerOnAdScreenPresentedEvent;
            IronSourceBannerEvents.onAdScreenDismissedEvent += BannerOnAdScreenDismissedEvent;
            IronSourceBannerEvents.onAdLeftApplicationEvent += BannerOnAdLeftApplicationEvent;

        // }


    }

    private void BannerOnAdLeftApplicationEvent(IronSourceAdInfo info)
    {
        Debug.Log("**********PLAYER LEFT APP AD LEFT EVENT: " + info.ToString());

    }

    private void BannerOnAdScreenDismissedEvent(IronSourceAdInfo info)
    {
        Debug.Log("**********BANNER ON AD SCREEN DISMISSED EVENT: " + info.ToString());

        bannerLoaded = false;
    }

    private void BannerOnAdScreenPresentedEvent(IronSourceAdInfo info)
    {
        Debug.Log("**********Banner Ad Screen Presented: " + info.ToString());
    }

    private void BannerOnAdLoadedEvent(IronSourceAdInfo info)
    {
        Debug.Log("**********Banner Load Ad Succeeded: " + info.ToString());
        bannerLoaded = true;
        //bannerLoading = false;
        //IronSource.Agent.displayBanner();
        Debug.Log("**********Hiding Banner***********");
        IronSource.Agent.hideBanner();
    }

    private void BannerOnAdLoadFailedEvent(IronSourceError error)
    {
        Debug.Log("**********Banner Load Ad Failed: " + error.ToString());

        //bannerLoading = false;
       //bannerLoaded = false;

        //if (!bannerLoaded && !bannerLoading && GameEndMenu.gameEnded == true)
        //{
         //   bannerLoading = true;
       IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM, "Game_Over");

        //}
    }



    private void SdkInitializationCompletedEvent()
    {
        IronSource.Agent.getAdvertiserId();

        Debug.Log("************SDK INITIALIZATION COMPLETE");

        Debug.Log("************LOADING BANNER");
#if UNITY_ANDROID
        IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM, "Jump_Man_Android");
#elif UNITY_IOS
        IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM, "Jump_Man_iOS");
#endif
        //adsInitComplete = true;
        //PlayerPrefs.SetString("adsInitialized", "true");
    }

    void OnApplicationPause(bool isPaused)
   {
        Debug.Log("**********ON APPLICATION PAUSE EVENT CALLED***************");

        // if (!isPaused)
        // {
        //     IronSource.Agent.displayBanner();
        // }
    }


    private void OnApplicationQuit()
    {
        Debug.Log("**********ON APPLICATION QUIT EVENT CALLED***************");

        //PlayerPrefs.SetString("adsInitialized", "false");
        IronSource.Agent.destroyBanner();

    }


    // Update is called once per frame
    void Update()
    {

        if(GameEndMenu.gameEnded == true && bannerLoaded == true)
        {
            Debug.Log("************DISPLAYING BANNER*********");
            IronSource.Agent.displayBanner();
        }else if(GameEndMenu.gameEnded == false && bannerLoaded == true)
        {
            Debug.Log("************HIDING BANNER************");
            IronSource.Agent.hideBanner();
        }











        /*        if (GameEndMenu.gameEnded == true && adsInitComplete == true && !bannerLoading ) 
                {

        #if UNITY_ANDROID

                    Debug.Log("************LOADING BANNER");
                    IronSource.Agent.getPlacementInfo("Jump_Man_Android");
                    if(!bannerLoaded){
                        bannerLoading = true;
                        IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM, "Jump_Man_Android");
                    }else{
                        IronSource.Agent.displayBanner();
                    }

        #elif UNITY_IOS

                    Debug.Log("************LOADING BANNER");
                    IronSource.Agent.getPlacementInfo("Jump_Man_iOS");
                    if (!bannerLoaded)
                    {
                        bannerLoading = true;
                        IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM, "Jump_Man_iOS");
                    }
                    else
                    {
                        IronSource.Agent.displayBanner();
                    }
        #endif

                }
                else if(GameEndMenu.gameEnded == false ) // && PlayerPrefs.GetString("adsInitializationComplete", "false") == "true")
                {
                    IronSource.Agent.hideBanner();

                }*/


    }

}
