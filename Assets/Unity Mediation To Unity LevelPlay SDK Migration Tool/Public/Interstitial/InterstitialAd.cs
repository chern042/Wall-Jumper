using System;
using System.Threading.Tasks;

namespace Unity.Services.Mediation
{
    /// <summary>
    /// Class to be instantiated in order to show an Interstitial Ad.
    /// </summary>
    public sealed class InterstitialAd : IInterstitialAd
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
        /// Event to be triggered by the adapter when an Ad is started.
        /// </summary>
        public event EventHandler OnShowed;

        /// <summary>
        /// Event to be triggered by the adapter when the user clicks on the Ad.
        /// </summary>
        public event EventHandler OnClicked;

        /// <summary>
        /// Event to be triggered by the adapter when the Ad is closed.
        /// </summary>
        public event EventHandler OnClosed;

        /// <summary>
        /// Event to be triggered by the adapter when the Ad has an error.
        /// </summary>
        public event EventHandler<ShowErrorEventArgs> OnFailedShow;

        AdState levelPlayState;

        /// <summary>
        /// Get the current state of the ad.
        /// </summary>
        public AdState AdState => levelPlayState;

        /// <summary>
        /// Get the ad unit id set during construction.
        /// </summary>
        public string AdUnitId => IronSourceAdUnits.INTERSTITIAL;

        TaskCompletionSource<object> m_LoadCompletionSource;
        TaskCompletionSource<object> m_ShowCompletionSource;
        bool m_IsLoading;
        bool m_IsShowing;

        /// <summary>
        /// Constructor for managing a specific Interstitial Ad.
        /// </summary>
        /// <param name="adUnitId">Unique Id for the Ad you want to show.</param>
        [Obsolete("Make sure to initialize with IronSource.Agent.init(appKey) first.\nYou do not need to create interstitial instance; LevelPlay uses a static interstitial class, which means you only need to call IronSource.Agent.loadInterstitial() and IronSource.Agent.showInterstitial().\nMake sure to register the appropriate callbacks, which are common to all interstitials.\nFor more information, see https://developers.is.com/ironsource-mobile/unity/interstitial-integration-unity/")]
        public InterstitialAd(string adUnitId)
        {
            InitializeCallbacks();
            levelPlayState = AdState.Unloaded;
        }

        void InitializeCallbacks()
        {
            IronSourceInterstitialEvents.onAdReadyEvent += OnLoadedBridge;
            IronSourceInterstitialEvents.onAdLoadFailedEvent += OnFailedLoadBridge;
            IronSourceInterstitialEvents.onAdShowSucceededEvent += OnShowedBridge;
            IronSourceInterstitialEvents.onAdClickedEvent += OnClickedBridge;
            IronSourceInterstitialEvents.onAdClosedEvent += OnClosedBridge;
            IronSourceInterstitialEvents.onAdShowFailedEvent += OnFailedShowBridge;
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
            this.OnFailedLoad?.Invoke(null, (LoadErrorEventArgs)EventArgs.Empty);
        }

        void OnShowedBridge(IronSourceAdInfo adInfo)
        {
            m_ShowCompletionSource.TrySetResult(null);
            TearDownAsyncShow();
            this.OnShowed?.Invoke(null, EventArgs.Empty);
        }

        void OnClickedBridge(IronSourceAdInfo adInfo)
        {
            this.OnClicked?.Invoke(null, EventArgs.Empty);
        }

        void OnClosedBridge(IronSourceAdInfo adInfo)
        {
            levelPlayState = AdState.Unloaded;
            this.OnClosed?.Invoke(null, EventArgs.Empty);
        }

        void OnFailedShowBridge(IronSourceError adError, IronSourceAdInfo adInfo)
        {
            levelPlayState = IronSource.Agent.isInterstitialReady() ? AdState.Loaded : AdState.Unloaded;
            m_ShowCompletionSource.TrySetException(new ShowFailedException(ShowError.Unknown, adError.getDescription()));
            TearDownAsyncShow();
            this.OnFailedShow?.Invoke(null, (ShowErrorEventArgs)EventArgs.Empty);
        }

        /// <summary>
        /// Method to tell the Mediation SDK to load an Ad.
        /// </summary>
        /// <returns>LoadAsync Task</returns>
        /// <exception cref="Unity.Services.Mediation.LoadFailedException">Thrown when the ad failed to load</exception>
        [Obsolete("Replace LoadAsync() with IronSource.Agent.loadInterstitial() ")]
        public Task LoadAsync()
        {
            if (!m_IsLoading)
            {
                SetupAsyncLoad();
                IronSource.Agent.loadInterstitial();
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
        /// Method to tell the Mediation SDK to show the loaded Ad.
        /// </summary>
        /// <param name="showOptions">Optional, allows setting optional parameters for showing an interstitial ad.</param>
        /// <returns>ShowAsync Task</returns>
        /// <exception cref="Unity.Services.Mediation.ShowFailedException">Thrown when the ad failed to show</exception>
        [Obsolete("Replace ShowAsync() with IronSource.Agent.showInterstitial() ")]
        public Task ShowAsync(InterstitialAdShowOptions showOptions = null)
        {
            if (!m_IsShowing)
            {
                SetupAsyncShow();
                IronSource.Agent.showInterstitial();
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
            IronSourceInterstitialEvents.onAdReadyEvent -= OnLoadedBridge;
            IronSourceInterstitialEvents.onAdLoadFailedEvent -= OnFailedLoadBridge;
            IronSourceInterstitialEvents.onAdShowSucceededEvent -= OnShowedBridge;
            IronSourceInterstitialEvents.onAdClickedEvent -= OnClickedBridge;
            IronSourceInterstitialEvents.onAdClosedEvent -= OnClosedBridge;
            IronSourceInterstitialEvents.onAdShowFailedEvent -= OnFailedShowBridge;
        }
    }
}
