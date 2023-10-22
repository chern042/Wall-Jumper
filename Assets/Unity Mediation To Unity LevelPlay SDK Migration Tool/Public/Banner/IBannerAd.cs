using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Unity.Services.Mediation
{
    /// <summary>
    /// Interface of an Banner Ad.
    /// </summary>
    public interface IBannerAd : IDisposable
    {
        /// <summary>
        /// Event to be triggered by the adapter when an Ad is loaded.
        /// </summary>
        [Obsolete("Listen to IronSourceBannerEvents.onAdLoadedEvent to receive LevelPlay banner load events.")]
        event EventHandler OnLoaded;

        /// <summary>
        /// Event to be triggered by the adapter when an Ad fails to load.
        /// </summary>
        [Obsolete("Listen to IronSourceBannerEvents.onAdLoadFailedEvent to receive LevelPlay banner failed load events.")]
        event EventHandler<LoadErrorEventArgs> OnFailedLoad;

        /// <summary>
        /// Event to be triggered by the adapter when the user clicks on the Ad.
        /// </summary>
        [Obsolete("Listen to IronSourceBannerEvents.onAdClickedEvent to receive LevelPlay banner click events.")]
        event EventHandler OnClicked;

        /// <summary>
        /// Event to be triggered by the adapter when the Ad refreshes
        /// </summary>
        [Obsolete("LevelPlay does not support ad refresh events.")]
        event EventHandler<LoadErrorEventArgs> OnRefreshed;

        /// <summary>
        /// Get the current state of the ad.
        /// </summary>
        [Obsolete("LevelPlay does not support the AdState check in your integration. This is an approximation of the banner ad state.")]
        AdState AdState { get; }

        /// <summary>
        /// Get the ad unit id set during construction.
        /// </summary>
        string AdUnitId { get; }

        /// <summary>
        /// Get the banner size set during construction.
        /// </summary>
        BannerAdSize Size { get; }

        /// <summary>
        /// Sets the position of the banner ad.
        /// </summary>
        /// <param name="anchor">Anchor on which the banner position is based</param>
        /// <param name="positionOffset">The X, Y coordinates offsets, relative to the anchor point</param>
        [Obsolete("You cannot reposition the banner. Consider destroying the banner (Ironsource.Agent.destroyBanner();) and loading a new one.")]
        void SetPosition(BannerAdAnchor anchor, Vector2 positionOffset = new Vector2());

        /// <summary>
        /// Loads the banner ad, and displays it when ready
        /// </summary>
        /// <returns>LoadAsync Task</returns>
        [Obsolete("Replace LoadAsync() with IronSource.Agent.loadBanner(size, position)")]
        Task LoadAsync();
    }
}
