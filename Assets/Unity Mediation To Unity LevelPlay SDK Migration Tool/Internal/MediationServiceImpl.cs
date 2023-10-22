using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Unity.Services.Mediation
{
    internal class MediationServiceImpl : IMediationService
    {
        internal MediationServiceImpl() { }

        public IInterstitialAd CreateInterstitialAd(string adUnitId)
        {
            return new InterstitialAd(adUnitId);
        }

        public IRewardedAd CreateRewardedAd(string adUnitId)
        {
            return new RewardedAd(adUnitId);
        }

        public IBannerAd CreateBannerAd(string adUnitId, BannerAdSize size, BannerAdAnchor anchor = BannerAdAnchor.Default, Vector2 positionOffset = new Vector2())
        {
            return new BannerAd(adUnitId, size, anchor, positionOffset);
        }

        public string SdkVersion => IronSource.pluginVersion();

        public IDataPrivacy DataPrivacy => null;

        public IImpressionEventPublisher ImpressionEventPublisher => null;
    }
}
