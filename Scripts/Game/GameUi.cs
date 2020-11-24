using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameUi : MonoBehaviour {
    public static GameUi _instance;

    [SerializeField] GameObject victoryPanel, loosePanel;

    [SerializeField] TMP_Text levelText;

    private void Awake() {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start() {
        SetSizes(cards[1]);
    }


    #region Human Class Cards
    public HumanCard[] cards;
    [SerializeField] Vector2 size;

    public void SetSizes(HumanCard hc) {
        foreach (HumanCard h in cards) {
            if (h != hc)
                h.GetComponent<RectTransform>().sizeDelta = size * 0.7f;
            else h.GetComponent<RectTransform>().sizeDelta = size;
        }
    }

    #endregion

    public void ShowVictory() {
        Invoke("ShowVictoryPanel", 0.25f);
    }

    public void ShowGameOver() {
        Invoke("ShowLoosePanel", 0.25f);
    }

    void ShowVictoryPanel() {
        victoryPanel.SetActive(true);
    }

    void ShowLoosePanel() {
        loosePanel.SetActive(true);
    }

    public void RestartScene(int n) {
        SceneManager.LoadScene(n);
    }

    public void InitLevel(int lvl) {
        levelText.text = "Level " + lvl;
    }

    public IEnumerator FadeOutObject(GameObject g, float time) {
        TMP_Text[] texts = g.GetComponentsInChildren<TMP_Text>();
        Image[] images = g.GetComponentsInChildren<Image>();

        float t = 0;
        while (t < time) {
            t += Time.deltaTime;

            foreach (Image img in images)
                img.color = Color.Lerp(img.color, new Color(0, 0, 0, 0), t / time);

            foreach (TMP_Text txt in texts)
                txt.color = Color.Lerp(txt.color, new Color(0, 0, 0, 0), t / time);

            yield return null;
        }

        g.SetActive(false);

        foreach (Image img in images)
            img.color = Color.white;

        foreach (TMP_Text txt in texts)
            txt.color = Color.white;
    }
}
