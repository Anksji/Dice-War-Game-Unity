using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChestManager : MonoBehaviour {
    public static ChestManager _instance;

    int currentProgression, maxProgression;
    [SerializeField] Image fillChestImage;
    [SerializeField] TMP_Text progressionText;

    public Animator chestAnimator;
    public GameObject coins;

    private void Awake() {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start() {
        currentProgression = PlayerPrefs.GetInt("CHEST", 0);
        maxProgression = 100;
    }

    public void AddProgression(int n) {
        currentProgression += n;

        SetChest();
    }

    void SetChest() {
        fillChestImage.fillAmount = (float)currentProgression / maxProgression;
        progressionText.text = (int)(fillChestImage.fillAmount * 100) + "%";


        if (currentProgression >= maxProgression) {
            currentProgression = 0;
            int gotChestCoint = Random.Range(899, 2000);
            DiceCurrencyManager._instance.AddCoins(gotChestCoint);

            StartCoroutine(ChestOpening());
        }

        PlayerPrefs.SetInt("CHEST", currentProgression);
    }

    IEnumerator ChestOpening() {
        progressionText.gameObject.SetActive(false);

        yield return new WaitForSeconds(2.1f);
        chestAnimator.Play("ChestOpening");

        yield return new WaitForSeconds(3.4f);

        coins.SetActive(true);
    }
}
