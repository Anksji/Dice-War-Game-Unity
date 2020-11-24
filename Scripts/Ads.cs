using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class Ads : MonoBehaviour {

    // Use this for initialization
    void Start() {
#if UNITY_IOS
        Advertisement.Initialize("4156438");
#elif UNITY_ANDROID
        Advertisement.Initialize("4208579");
#endif

    }

    public float timeToShowAds = 999;
    float timer;

    // Update is called once per frame
    void Update() {
        if (timer > timeToShowAds) {
            timer = 0;
            Advertisement.Show();
        }
    }
}
