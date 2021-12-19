using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
public class AdvertisementManager : Singleton<AdvertisementManager>
{
    private BannerView bannerView;
    private InterstitialAd front;

    // 배너 광고 생성
    private void InitBannerViewCreate()
    {
        // ID
        string adUnitId = "ca-app-pub-3940256099942544/6300978111";

        // Size Setting
        AdSize adSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);

        // BannerView Create
        bannerView = new BannerView(adUnitId, adSize, AdPosition.Bottom);
        AdRequest adRequest = new AdRequest.Builder().Build();
        bannerView.LoadAd(adRequest);
    }

    private void InitFrontCreate()
    {
#if UNITY_ANDROID
        // ID
        string adUnitId = "ca-app-pub-3940256099942544/6300978111";
#endif

        front = new InterstitialAd(adUnitId);
        front.OnAdClosed += OnAdClosed;

        AdRequest adRequest = new AdRequest.Builder().Build();
        front.LoadAd(adRequest);

        StartCoroutine(Coroutine_DelayFrontShow());
        IEnumerator Coroutine_DelayFrontShow()
        {
            while(!front.IsLoaded())
            {
                yield return new WaitForSeconds(0.2f);
            }
            front.Show();
        }
    }

    private void OnAdClosed(object sender, System.EventArgs e)
    {
        Debug.Log("전면광고 종료");
    }

    protected override void OnStart()
    {
        //InitBannerViewCreate();
        InitFrontCreate();
    }
}
