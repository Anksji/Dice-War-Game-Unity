using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class UnityAdsManager : MonoBehaviour, IUnityAdsListener
{
    // Start is called before the first frame update
    //[SerializeField] private Admob RewardVideoadmob;
    //[SerializeField] private Admob Interstitaladmob;
    void Start()
    {
        //Interstitaladmob.OnInitComplete += () => Interstitaladmob.RequestInterstitialAd();

        //RewardVideoadmob.OnRewardAdWatched += OnRewardAdWatchedHandle;
        //RewardVideoadmob.RequestRewardAd();
        Advertisement.Initialize(PublicCostStrings.gameId, PublicCostStrings.testMode);
        StartCoroutine(showBannerWhenInitialized());
    }

    //private void OnRewardAdWatchedHandle(GoogleMobileAds.Api.Reward reward)
    //{
    //    //ad is watched

    //}


    IEnumerator showBannerWhenInitialized()
    {
        while (!Advertisement.isInitialized)
        {
            yield return new WaitForSeconds(0.5f);
        }
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Advertisement.Banner.Show(PublicCostStrings.bannerAd);
    }

    public void showInterstitialAd()
    {
        // Check if UnityAds ready before calling Show method:
        if (Advertisement.IsReady())
        {
            Advertisement.Show(PublicCostStrings.interstialFullScreenAd);
            // Replace mySurfacingId with the ID of the placements you wish to display as shown in your Unity Dashboard.
        }
        else
        {
            //Interstitaladmob.ShowInterstitialAd();
            Debug.Log("Interstitial ad not ready at the moment! Please try again later!");
        }
    }
    public void showRewardedVideo()
    {
        // Check if UnityAds ready before calling Show method:
        if (Advertisement.IsReady(PublicCostStrings.rewardedVideoAd))
        {
            Advertisement.Show(PublicCostStrings.rewardedVideoAd);
        }
        else
        {
            //RewardVideoadmob.ShowRewardAd();
            Debug.Log("Rewarded video is not ready at the moment! Please try again later!");
        }
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsDidFinish(string surfacingId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            // Reward the user for watching the ad to completion.

        }
        else if (showResult == ShowResult.Skipped)
        {
            // Do not reward the user for skipping the ad.
        }
        else if (showResult == ShowResult.Failed)
        {
            Debug.LogWarning("The ad did not finish due to an error.");
        }
    }

    public void OnUnityAdsReady(string surfacingId)
    {
        // If the ready Ad Unit or legacy Placement is rewarded, show the ad:
        if (surfacingId == PublicCostStrings.rewardedVideoAd)
        {
            // Optional actions to take when theAd Unit or legacy Placement becomes ready (for example, enable the rewarded ads button)
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        Debug.Log("video ads is message " + message);
        // Log the error.
    }

    public void OnUnityAdsDidStart(string surfacingId)
    {
        // Optional actions to take when the end-users triggers an ad.
    }

    // When the object that subscribes to ad events is destroyed, remove the listener:
    public void OnDestroy()
    {
        Advertisement.RemoveListener(this);
    }

}