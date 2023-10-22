using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Unity.Services.Mediation
{
    /// <summary>
    /// Class to be instantiated in order to show a Banner Ad.
    /// </summary>
    public sealed class BannerAd : IBannerAd
    {
        /// <summary>
        /// Event to be triggered by the adapter when an Ad is loaded.
        /// </summary>
        public event EventHandler OnLoaded;

        /// <summary>
        /// Event to be triggered by the adapter when an Ad fails to load.
        /// </summary>
        public event EventHandler<LoadErrorEventArgs> OnFailedLoad;

        /// <summary>
        /// Event to be triggered by the adapter when the user clicks on the Ad.
        /// </summary>
        public event EventHandler OnClicked;

        /// <summary>
        /// Event to be triggered by the adapter when the Ad refreshes
        /// </summary>
        public event EventHandler<LoadErrorEventArgs> OnRefreshed;

        AdState levelPlayState;

        /// <summary>
        /// Get the current state of the ad.
        /// </summary>
        public AdState AdState => levelPlayState;

        /// <summary>
        /// Get the ad unit id set during construction.
        /// </summary>
        public string AdUnitId => IronSourceAdUnits.BANNER;

        /// <summary>
        /// Get the banner size set during construction.
        /// </summary>
        public BannerAdSize Size => originalSize;

        BannerAdSize originalSize;
        IronSourceBannerSize bannerSize;
        IronSourceBannerPosition bannerPos;

        TaskCompletionSource<object> m_LoadCompletionSource;
        bool m_IsLoading;

        /// <summary>
        /// Constructor for managing a specific Banner Ad.
        /// </summary>
        /// <param name="adUnitId">Unique Id for the Ad you want to show.</param>
        /// <param name="size">Size of banner set to be constructed.</param>
        /// <param name="anchor">Anchor on which the banner position is based</param>
        /// <param name="positionOffset">The X, Y coordinates offsets, relative to the anchor point</param>
        [Obsolete("Make sure to initialize with IronSource.Agent.init(appKey) first.\nYou do not need to create a banner instance; LevelPlay uses a static banner class, which means you only need to call IronSource.Agent.loadBanner(bannerSize, bannerPos).\nMake sure to register the appropriate callbacks, which are common to all banners.\nFor more information, see https://developers.is.com/ironsource-mobile/unity/banner-integration-unity/")]
        public BannerAd(string adUnitId, BannerAdSize size, BannerAdAnchor anchor = BannerAdAnchor.Default, Vector2 positionOffset = new Vector2())
        {
            originalSize = size;

            if (size.Equals(BannerAdPredefinedSize.Banner.ToBannerAdSize()))
            {
                bannerSize = IronSourceBannerSize.BANNER;
                Debug.Log("Banner size changed from 350x50 to 320x50 to conform to LevelPlay's IronSourceBannersize.BANNER");
            }
            else if (size.Equals(BannerAdPredefinedSize.LargeBanner.ToBannerAdSize()))
            {
                bannerSize = IronSourceBannerSize.LARGE;
                Debug.Log("Banner size changed from 320x100 to 320x90 to conform to LevelPlay's IronSourceBannersize.LARGE");
            }
            else if (size.Equals(BannerAdPredefinedSize.MediumRectangle.ToBannerAdSize()))
            {
                bannerSize = IronSourceBannerSize.RECTANGLE;
                Debug.Log("Banner size remains 300x250 to conform to LevelPlay's IronSourceBannersize.RECTANGLE");
            }
            else if (size.Equals(BannerAdPredefinedSize.Leaderboard.ToBannerAdSize()))
            {
                bannerSize = new IronSourceBannerSize(BannerAdPredefinedSize.Leaderboard.ToBannerAdSize().DpWidth, BannerAdPredefinedSize.Leaderboard.ToBannerAdSize().DpHeight);
                Debug.Log("LevelPlay does not support the Leaderboard banner size in your integration. Please use a different banner size.");
            }
            else
            {
                bannerSize = new IronSourceBannerSize(size.DpWidth, size.DpHeight);
            }

            bannerPos = new IronSourceBannerPosition();
            Debug.Log("LevelPlay Banner only supports TOP or BOTTOM position");

            if (anchor == BannerAdAnchor.TopCenter || anchor == BannerAdAnchor.TopLeft || anchor == BannerAdAnchor.TopRight
                || anchor == BannerAdAnchor.Center || anchor == BannerAdAnchor.MiddleLeft || anchor == BannerAdAnchor.MiddleRight
                || anchor == BannerAdAnchor.None || anchor == BannerAdAnchor.Default)
            {
                bannerPos = IronSourceBannerPosition.TOP;
            }
            else if (anchor == BannerAdAnchor.BottomCenter || anchor == BannerAdAnchor.BottomLeft || anchor == BannerAdAnchor.BottomRight)
            {
                bannerPos = IronSourceBannerPosition.BOTTOM;
            }
            else
            {
                Debug.Log("Invalid banner anchor position");
            }

            InitializeCallbacks();
            levelPlayState = AdState.Unloaded;
        }

        /// <summary>
        /// Sets the position of a banner ad.
        /// </summary>
        /// <param name="anchor">Anchor on which the banner position is based</param>
        /// <param name="positionOffset">The X, Y coordinates offsets, relative to the anchor point</param>
        [Obsolete("You cannot reposition the banner. Consider destroying the banner (Ironsource.Agent.destroyBanner()) and loading a new one.")]
        public void SetPosition(BannerAdAnchor anchor, Vector2 positionOffset = new Vector2()) { }

        void InitializeCallbacks()
        {
            IronSourceBannerEvents.onAdLoadedEvent += OnLoadedBridge;
            IronSourceBannerEvents.onAdLoadFailedEvent += OnFailedLoadBridge;
            IronSourceBannerEvents.onAdClickedEvent += OnClickedBridge;
        }

        void OnLoadedBridge(IronSourceAdInfo adInfo)
        {
            m_LoadCompletionSource.TrySetResult(null);
            TearDownAsyncLoad();
            levelPlayState = AdState.Loaded;
            this.OnLoaded?.Invoke(null, EventArgs.Empty);
        }

        void OnFailedLoadBridge(IronSourceError adError)
        {
            m_LoadCompletionSource.SetException(new LoadFailedException(LoadError.Unknown, adError.getDescription()));
            TearDownAsyncLoad();
            levelPlayState = AdState.Unloaded;
            this.OnFailedLoad?.Invoke(null, (LoadErrorEventArgs)EventArgs.Empty);
        }

        void OnClickedBridge(IronSourceAdInfo adInfo)
        {
            this.OnClicked?.Invoke(null, EventArgs.Empty);
        }

        /// <summary>
        /// Method to tell the Mediation SDK to load an Ad.
        /// </summary>
        /// <returns>Async Load task</returns>
        /// <exception cref="Unity.Services.Mediation.LoadFailedException">Thrown when the ad failed to load</exception>
        [Obsolete("You do not need to create a banner instance; LevelPlay uses a static banner class, which means you only need to call IronSource.Agent.loadBanner(bannerSize, bannerPos).")]
        public Task LoadAsync()
        {
            if (!m_IsLoading)
            {
                SetupAsyncLoad();
                IronSource.Agent.loadBanner(bannerSize, bannerPos);
                levelPlayState = AdState.Loading;
            }

            return m_LoadCompletionSource?.Task ?? Task.CompletedTask;
        }

        void SetupAsyncLoad()
        {
            m_LoadCompletionSource = new TaskCompletionSource<object>();
            m_IsLoading = true;
        }

        void TearDownAsyncLoad()
        {
            m_IsLoading = false;
        }

        /// <summary>
        /// Dispose and release internal resources.
        /// </summary>
        public void Dispose()
        {
            IronSourceBannerEvents.onAdLoadedEvent -= OnLoadedBridge;
            IronSourceBannerEvents.onAdLoadFailedEvent -= OnFailedLoadBridge;
            IronSourceBannerEvents.onAdClickedEvent -= OnClickedBridge;
        }
    }
}
