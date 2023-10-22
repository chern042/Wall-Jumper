using System;
using System.Threading.Tasks;

namespace Unity.Services.Mediation
{
    /// <summary>
    /// Class to be instantiated in order to show a Rewarded Ad.
    /// </summary>
    public sealed class RewardedAd : IRewardedAd
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
        /// Event to be triggered by the adapter when a Rewarded Ad is shown.
        /// </summary>
        public event EventHandler OnShowed;

        /// <summary>
        /// Event to be triggered by the adapter when the user clicks on the RewardedAd.
        /// </summary>
        public event EventHandler OnClicked;

        /// <summary>
        /// Event to be triggered by the adapter when the RewardedAd is closed.
        /// </summary>
        public event EventHandler OnClosed;

        /// <summary>
        /// Event to be triggered by the adapter when the RewardedAd has an error.
        /// </summary>
        public event EventHandler<ShowErrorEventArgs> OnFailedShow;

        /// <summary>
        /// Event to be triggered by the adapter when a reward needs to be issued.
        /// </summary>
        public event EventHandler<RewardEventArgs> OnUserRewarded;

        AdState levelPlayState;

        /// <summary>
        ///<value>Gets the state of the <c>RewardedAd</c>.</value>
        /// </summary>
        public AdState AdState => levelPlayState;

        /// <summary>
        /// <value>Gets the id of the ad unit.</value>
        /// </summary>
        public string AdUnitId => IronSourceAdUnits.REWARDED_VIDEO;

        TaskCompletionSource<object> m_ShowCompletionSource;
        bool m_IsShowing;

        /// <summary>
        /// Constructor for managing a specific Rewarded Ad.
        /// </summary>
        /// <param name="adUnitId">Unique Id for the Ad you want to show.</param>
        [Obsolete("Make sure to initialize with IronSource.Agent.init(appKey) first.\nYou do not need to create and load a rewarded instance; LevelPlay uses a static rewarded class, which means you only need to call IronSource.Agent.showRewardedVideo().\nMake sure to register the appropriate callbacks, which are common to all rewarded videos.\nFor more information, see https://developers.is.com/ironsource-mobile/unity/rewarded-video-integration-unity/")]
        public RewardedAd(string adUnitId)
        {
            InitializeCallbacks();
            levelPlayState = IronSource.Agent.isRewardedVideoAvailable() ? AdState.Loaded : AdState.Unloaded;
        }

        void InitializeCallbacks()
        {
            IronSourceRewardedVideoEvents.onAdAvailableEvent += OnLoadedBridge;
            IronSourceRewardedVideoEvents.onAdUnavailableEvent += OnAdUnavailableBridge;
            IronSourceRewardedVideoEvents.onAdReadyEvent += OnLoadedBridge;
            IronSourceRewardedVideoEvents.onAdLoadFailedEvent += OnFailedLoadBridge;
            IronSourceRewardedVideoEvents.onAdOpenedEvent += OnShowedBridge;
            IronSourceRewardedVideoEvents.onAdClickedEvent += OnClickedBridge;
            IronSourceRewardedVideoEvents.onAdClosedEvent += OnClosedBridge;
            IronSourceRewardedVideoEvents.onAdShowFailedEvent += OnFailedShowBridge;
            IronSourceRewardedVideoEvents.onAdRewardedEvent += OnUserRewardedBridge;
        }

        void OnAdUnavailableBridge()
        {
            levelPlayState = AdState.Unloaded;
        }

        void OnLoadedBridge(IronSourceAdInfo adInfo)
        {
            levelPlayState = AdState.Loaded;
            this.OnLoaded?.Invoke(null, EventArgs.Empty);
        }

        void OnFailedLoadBridge(IronSourceError adError)
        {
            this.OnFailedLoad?.Invoke(null, (LoadErrorEventArgs)EventArgs.Empty);
        }

        void OnShowedBridge(IronSourceAdInfo adInfo)
        {
            m_ShowCompletionSource.TrySetResult(null);
            TearDownAsyncShow();
            this.OnShowed?.Invoke(null, EventArgs.Empty);
        }

        void OnClickedBridge(IronSourcePlacement placement, IronSourceAdInfo adInfo)
        {
            this.OnClicked?.Invoke(null, EventArgs.Empty);
        }

        void OnClosedBridge(IronSourceAdInfo adInfo)
        {
            levelPlayState = IronSource.Agent.isRewardedVideoAvailable() ? AdState.Loaded : AdState.Unloaded;
            this.OnClosed?.Invoke(null, EventArgs.Empty);
        }

        void OnFailedShowBridge(IronSourceError adError, IronSourceAdInfo adInfo)
        {
            m_ShowCompletionSource.TrySetException(new ShowFailedException(ShowError.Unknown, adError.getDescription()));
            TearDownAsyncShow();
            this.OnFailedShow?.Invoke(null, (ShowErrorEventArgs)EventArgs.Empty);
        }

        void OnUserRewardedBridge(IronSourcePlacement placement, IronSourceAdInfo adInfo)
        {
            RewardEventArgs eventArgs = new RewardEventArgs(placement.getRewardName(), placement.getRewardAmount().ToString());
            this.OnUserRewarded?.Invoke(null, eventArgs);
        }

        /// <summary>
        /// Method to tell the Mediation SDK to load an Ad.
        /// </summary>
        /// <returns>LoadAsync Task</returns>
        /// <exception cref="Unity.Services.Mediation.LoadFailedException">Thrown when the ad failed to load</exception>
        [Obsolete("You do not need to load a rewarded ad; LevelPlay uses a static rewarded class, which means you only need to call IronSource.Agent.showRewardedVideo().")]
        public Task LoadAsync()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Method to tell the Mediation SDK to show the loaded Ad.
        /// </summary>
        /// <param name="showOptions">Optional, allows setting optional parameters for showing a rewarded ad.</param>
        /// <returns>ShowAsync Task</returns>
        /// <exception cref="Unity.Services.Mediation.ShowFailedException">Thrown when the ad failed to show</exception>
        [Obsolete("Replace ShowAsync() with IronSource.Agent.showRewardedVideo() ")]
        public Task ShowAsync(RewardedAdShowOptions showOptions = null)
        {
            if (!m_IsShowing)
            {
                SetupAsyncShow();
                IronSource.Agent.showRewardedVideo();
                levelPlayState = AdState.Showing;
            }

            return m_ShowCompletionSource?.Task ?? Task.CompletedTask;
        }

        void SetupAsyncShow()
        {
            m_ShowCompletionSource = new TaskCompletionSource<object>();
            m_IsShowing = true;
        }

        void TearDownAsyncShow()
        {
            m_IsShowing = false;
        }

        /// <summary>
        /// Dispose and release internal resources.
        /// </summary>
        public void Dispose()
        {
            IronSourceRewardedVideoEvents.onAdAvailableEvent -= OnLoadedBridge;
            IronSourceRewardedVideoEvents.onAdUnavailableEvent -= OnAdUnavailableBridge;
            IronSourceRewardedVideoEvents.onAdReadyEvent -= OnLoadedBridge;
            IronSourceRewardedVideoEvents.onAdLoadFailedEvent -= OnFailedLoadBridge;
            IronSourceRewardedVideoEvents.onAdOpenedEvent -= OnShowedBridge;
            IronSourceRewardedVideoEvents.onAdClickedEvent -= OnClickedBridge;
            IronSourceRewardedVideoEvents.onAdClosedEvent -= OnClosedBridge;
            IronSourceRewardedVideoEvents.onAdShowFailedEvent -= OnFailedShowBridge;
            IronSourceRewardedVideoEvents.onAdRewardedEvent -= OnUserRewardedBridge;
        }
    }
}
