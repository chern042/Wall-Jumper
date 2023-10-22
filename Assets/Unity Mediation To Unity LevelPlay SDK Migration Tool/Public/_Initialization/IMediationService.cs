using System;
using UnityEngine;

namespace Unity.Services.Mediation
{
    /// <summary>
    /// The interface for Mediation features.
    /// </summary>
    public interface IMediationService
    {
        /// <summary>
        /// The Interstitial ads creator function.
        /// </summary>
        /// <param name="adUnitId"> The Ad Unit Id for the ad unit you wish to show. </param>
        /// <returns> A new Interstitial Ad instance. </returns>
        [Obsolete("Make sure to initialize with IronSource.Agent.init(appKey) first.\nYou do not need to create interstitial instance; LevelPlay uses a static interstitial class, which means you only need to call IronSource.Agent.loadInterstitial() and IronSource.Agent.showInterstitial().\nMake sure to register the appropriate callbacks, which are common to all interstitials.\nFor more information, see https://developers.is.com/ironsource-mobile/unity/interstitial-integration-unity/")]
        IInterstitialAd CreateInterstitialAd(string adUnitId);

        /// <summary>
        /// The Rewarded ads creator function.
        /// </summary>
        /// <param name="adUnitId"> The Ad Unit Id for the ad unit you wish to show. </param>
        /// <returns> A new Rewarded Ad instance. </returns>
        [Obsolete("Make sure to initialize with IronSource.Agent.init(appKey) first.\nYou do not need to create and load a rewarded instance; LevelPlay uses a static rewarded class, which means you only need to call IronSource.Agent.showRewardedVideo().\nMake sure to register the appropriate callbacks, which are common to all rewarded videos.\nFor more information, see https://developers.is.com/ironsource-mobile/unity/rewarded-video-integration-unity/")]
        IRewardedAd CreateRewardedAd(string adUnitId);

        /// <summary>
        /// The Banner ads creator function.
        /// </summary>
        /// <param name="adUnitId">Unique Id for the Ad you want to show.</param>
        /// <param name="size">Size of banner set to be constructed.</param>
        /// <param name="anchor">Anchor on which the banner position is based</param>
        /// <param name="positionOffset">The X, Y coordinates offsets, relative to the anchor point</param>
        /// <returns> A new Banner Ad instance. </returns>
        [Obsolete("Make sure to initialize with IronSource.Agent.init(appKey) first.\nYou do not need to create a banner instance; LevelPlay uses a static banner class, which means you only need to call IronSource.Agent.loadBanner(bannerSize, bannerPos).\nMake sure to register the appropriate callbacks, which are common to all banners.\nFor more information, see https://developers.is.com/ironsource-mobile/unity/banner-integration-unity/")]
        IBannerAd CreateBannerAd(string adUnitId, BannerAdSize size, BannerAdAnchor anchor = BannerAdAnchor.Default, Vector2 positionOffset = new Vector2());

        /// <summary>
        /// Access the Data Privacy API, to register the user's consent status.
        /// </summary>
        [Obsolete("You do not need to call DataPrivacy instance. LevelPlay uses its static agent to manage regulation advanced settings.\nFor more information, see https://developers.is.com/ironsource-mobile/unity/regulation-advanced-settings/")]
        IDataPrivacy DataPrivacy { get; }

        /// <summary>
        /// Access the Impression Event Publisher API, to receive events when impression events are fired from ad objects.
        /// </summary>
        [Obsolete("Call IronSourceBannerEvents, IronSourceInterstitialEvents, or IronSourceRewardedVideoEvents to receive ad events.")]
        IImpressionEventPublisher ImpressionEventPublisher { get; }

        /// <summary>
        /// Native Mediation SDK version this mediation service is operating upon.
        /// </summary>
        string SdkVersion => IronSource.pluginVersion();
    }
}
