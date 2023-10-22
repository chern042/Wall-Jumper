using System;
using System.Threading.Tasks;

namespace Unity.Services.Mediation
{
    /// <summary>
    /// Interface of an Interstitial Ad.
    /// </summary>
    public interface IInterstitialAd : IDisposable
    {
        /// <summary>
        /// Event to be triggered by the adapter when an Ad is loaded.
        /// </summary>
        [Obsolete("Listen to IronSourceInterstitialEvents.onAdReadyEvent to receive LevelPlay banner load events.")]
        event EventHandler OnLoaded;

        /// <summary>
        /// Event to be triggered by the adapter when an Ad fails to load.
        /// </summary>
        [Obsolete("Listen to IronSourceInterstitialEvents.onAdLoadFailedEvent to receive LevelPlay banner failed load events.")]
        event EventHandler<LoadErrorEventArgs> OnFailedLoad;

        /// <summary>
        /// Event to be triggered by the adapter when an Ad is started.
        /// </summary>
        [Obsolete("Listen to IronSourceInterstitialEvents.onAdShowSucceededEvent to receive LevelPlay banner show events.")]
        event EventHandler OnShowed;

        /// <summary>
        /// Event to be triggered by the adapter when the user clicks on the Ad.
        /// </summary>
        [Obsolete("Listen to IronSourceInterstitialEvents.onAdClickedEvent to receive LevelPlay banner ad click events.")]
        event EventHandler OnClicked;

        /// <summary>
        /// Event to be triggered by the adapter when the Ad is closed.
        /// </summary>
        [Obsolete("Listen to IronSourceInterstitialEvents.onAdClosedEvent to receive LevelPlay banner close events.")]
        event EventHandler OnClosed;

        /// <summary>
        /// Event to be triggered by the adapter when the Ad has an error.
        /// </summary>
        [Obsolete("Listen to IronSourceInterstitialEvents.onAdShowFailedEvent to receive LevelPlay banner failed show events.")]
        event EventHandler<ShowErrorEventArgs> OnFailedShow;

        /// <summary>
        /// Get the current state of the ad.
        /// </summary>
        [Obsolete("Replace with IronSource.Agent.isInterstitialReady() to check availability")]
        AdState AdState { get; }

        /// <summary>
        /// Get the ad unit id set during construction.
        /// </summary>
        string AdUnitId { get; }

        /// <summary>
        /// Method to tell the Mediation SDK to load an Ad.
        /// </summary>
        /// <returns>LoadAsync Task</returns>
        /// <exception cref="Unity.Services.Mediation.LoadFailedException">Thrown when the ad failed to load</exception>
        [Obsolete("Replace LoadAsync() with IronSource.Agent.loadInterstitial() ")]
        Task LoadAsync();

        /// <summary>
        /// Method to tell the Mediation SDK to show the loaded Ad.
        /// </summary>
        /// <param name="showOptions">Optional, allows setting optional parameters for showing an interstitial ad.</param>
        /// <returns>ShowAsync Task</returns>
        /// <exception cref="Unity.Services.Mediation.ShowFailedException">Thrown when the ad failed to show</exception>
        [Obsolete("Replace ShowAsync() with IronSource.Agent.showInterstitial() ")]
        Task ShowAsync(InterstitialAdShowOptions showOptions = null);
    }

    /// <summary>
    /// Contains optional parameters for showing an interstitial ad.
    /// </summary>
    [Obsolete("LevelPlay does not support InterstitialAdShowOptions")]
    public class InterstitialAdShowOptions
    {
        /// <summary>
        /// If set to true, the ad will automatically load another ad after it is done showing, so it is not necessary
        /// to call LoadAsync again. This will occur when the OnClosed or OnFailedShow event is triggered.
        /// </summary>
        public bool AutoReload { get; set; } = false;
    }
}
