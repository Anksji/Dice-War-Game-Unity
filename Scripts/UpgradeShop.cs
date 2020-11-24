using TMPro;
using UnityEngine;

public class UpgradeShop : MonoBehaviour {

    public static UpgradeShop _instance;

    private void Awake() {
        _instance = this;

    }

    public void Init() {
        diceLevel = PlayerPrefs.GetInt("UPGRADE_" + "HUMANS", 0);
        SetDice();

        offlineLevel = PlayerPrefs.GetInt("UPGRADE_" + "OFFLINE", 0);
        SetOffline();
    }

    public void HideUi() {
        StartCoroutine(GameUi._instance.FadeOutObject(shop, 2f));
    }

    public GameObject shop;

    #region Human Number
    [Header("Starting Human Num")]
    public TMP_Text diceLevelText, dicePriceText;
    public TMP_Text diceText;
    public int diceLevel;
    public int dicePrice;
    public int diceValue;

    void SetDice() {
        diceLevelText.text = "Lv " + diceLevel;

        dicePrice = 31 + (int)Mathf.Pow(5f, (diceLevel / 1.8f) + 2.0f);
        dicePriceText.text = DiceCurrencyManager.GetSuffix(dicePrice);

        diceValue = diceLevel + 1;
        diceText.text = "-" + (diceValue * 1) + "%";

        DiceGameManager._instance.myPlayer.timeToRefillDice = 8 * (1 - diceValue * 0.01f);
    }

    public void BuyDice() {
        if (dicePrice <= DiceCurrencyManager._instance.coins) {
            DiceCurrencyManager._instance.AddCoins(-dicePrice);
            diceLevel++;

            SetDice();

            PlayerPrefs.SetInt("UPGRADE_" + "HUMANS", diceLevel);
        } else {
        }
    }
    #endregion


    #region Offline Earning
    [Header("Offline Earning")]
    public TMP_Text offlineLevelText, offlinePriceText;
    public TMP_Text offlineText;
    public int offlineLevel;
    public int offlinePrice;
    public float offlineValue;

    void SetOffline() {
        offlineLevelText.text = "Lv " + offlineLevel;

        offlinePrice = 31 + (int)Mathf.Pow(5f, (offlineLevel / 1.8f) + 2.0f);
        offlinePriceText.text = DiceCurrencyManager.GetSuffix(offlinePrice);

        offlineValue = 1 + offlineLevel * 0.8f;
        offlineText.text = "+" + (offlineLevel * 10) + "%";
    }

    public void BuyOffline() {
        if (offlinePrice <= DiceCurrencyManager._instance.coins) {
            DiceCurrencyManager._instance.AddCoins(-offlinePrice);
            offlineLevel++;

            SetOffline();

            PlayerPrefs.SetInt("UPGRADE_" + "OFFLINE", offlineLevel);
        } else {
        }
    }
    #endregion

}
