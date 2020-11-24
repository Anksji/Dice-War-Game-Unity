using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiceOfflineEarning : MonoBehaviour {

    public static DiceOfflineEarning _instance;


    private void Awake() {
        _instance = this;
    }

    // Use this for initialization
    void Start() {
        StartCoroutine(GetDay());
    }

    public TMP_Text offlineIncomeText;
    public GameObject offlineWindow;
    int coinsToAdd;

    public TimeSpan allTimeSinceFirstTime;
    /// <summary>
    /// Getting offline reward
    /// </summary>
    /// <returns></returns>
    private IEnumerator GetDay() {
        //WWW www = new WWW("http://worldclockapi.com/api/json/est/now");

        //DateTime currentDateTime = DateTime.Parse(www.text.Replace('/', ' '));
        DateTime currentDateTime = DateTime.Now;


        if (!PlayerPrefs.HasKey("FIRST_LOGIN_TIME")) {
            PlayerPrefs.SetString("FIRST_LOGIN_TIME", currentDateTime.ToString());
        }

        if (!PlayerPrefs.HasKey("OLD_LOGIN_TIME")) {
            PlayerPrefs.SetString("OLD_LOGIN_TIME", currentDateTime.ToString());
            yield break;
        }


        DateTime oldDate = DateTime.Parse(PlayerPrefs.GetString("OLD_LOGIN_TIME"));
        DateTime firstDate = DateTime.Parse(PlayerPrefs.GetString("FIRST_LOGIN_TIME"));

        TimeSpan sub = currentDateTime - oldDate;
        allTimeSinceFirstTime = currentDateTime - firstDate;



        if (sub.TotalMinutes < 5) yield break;

        yield return new WaitForSeconds(0.2f);

        offlineWindow.SetActive(true);

        coinsToAdd = (int)(Mathf.Min((float)sub.TotalMinutes, 12 * 60) * UpgradeShop._instance.offlineValue);
        DiceCurrencyManager._instance.AddCoins(coinsToAdd);

        offlineIncomeText.text = DiceCurrencyManager.GetSuffix((long)coinsToAdd);
        Debug.Log("OFFLINE REWARD " + (int)sub.TotalMinutes);

        PlayerPrefs.SetString("OLD_LOGIN_TIME", currentDateTime.ToString());
    }

    
    public void DoubleEarnings(){
        Invoke("addDoubleCoins", 5f);
    }

    private void addDoubleCoins()
    {
        DiceCurrencyManager._instance.AddCoins(coinsToAdd);
    }

    
    public void DelayedDeactivation() {
        StartCoroutine(DelayCoroutine());
    }

    IEnumerator DelayCoroutine() {
        yield return new WaitForSeconds(1.2f);

        offlineWindow.SetActive(false);
    }
}
