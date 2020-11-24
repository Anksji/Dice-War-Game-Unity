using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DiceGameManager : MonoBehaviour {
    public static DiceGameManager _instance;

    public Dice dicePrefab;
    public TMP_Text tmp_textPrefab;

    public PlayerBase myPlayer;
    public PlayerBase enemyPlayer;

    public GameObject pauseScreen;
    public LevelPrefab currentLevel;
    [SerializeField] LevelPrefab[] levelPrefabs;
    [SerializeField] GameObject[] groundTypes;
    public AudioSource audS;
    public TMP_Text coin_got;
    public TMP_Text lossgmcoin_got;

    public int levelNum;

    [HideInInspector] public bool isPaused = true;

    [SerializeField] GameObject tutorial;

    private void Awake() {
        _instance = this;
    }

    public void openGameOne()
    {
        Application.OpenURL(PublicCostStrings.carGameLink);

    }
    public void openGameTwo()
    {
        Application.OpenURL(PublicCostStrings.snakeGameLink);

    }



    // Start is called before the first frame update
    void Start() {
        levelNum = PlayerPrefs.GetInt("LEVEL_NUM", 0);
        //levelNum = 250;
        currentLevel = Instantiate(levelPrefabs[levelNum % levelPrefabs.Length]);
        Instantiate(groundTypes[levelNum % groundTypes.Length]);

        GameUi._instance.InitLevel(levelNum + 1);
        UpgradeShop._instance.Init();

        //Based on current level, we set up the values of the dice fill rate and shoot rate
        enemyPlayer.GetComponent<AiPlayer>().Setup();
    }

    public void StartGame() {
        isPaused = false;
        UpgradeShop._instance.HideUi();

        if (!PlayerPrefs.HasKey("TUTORIAL")) {
            PlayerPrefs.SetInt("TUTORIAL", 1);

            tutorial.SetActive(true);
        }
    }

    public void PauseGame()
    {
        pauseScreen.SetActive(true);
        audS.Pause();
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        pauseScreen.SetActive(false);
        audS.Play();
        Time.timeScale = 1;
    }

    public void GameOver(bool isWin) {
        if (isPaused) return;

        isPaused = true;
        int coinGot = Random.Range(10, 30);
        if (isWin) { //Victory
            coinGot = Random.Range(60, 200);
            coin_got.text = "" + coinGot;
            PlayerPrefs.SetInt("LEVEL_NUM", levelNum + 1);

            ChestManager._instance.AddProgression(Random.Range(18, 25));
            GameUi._instance.ShowVictory();
        } else { //Defeat
            coinGot = Random.Range(10, 50);
            lossgmcoin_got.text = "" + coinGot;
            GameUi._instance.ShowGameOver();
        }

        DiceCurrencyManager._instance.AddCoins(coinGot);
    }

    public void InstatiateTMPText(Vector3 pos, Color c, string txt) {
        var t = Instantiate(tmp_textPrefab);
        t.transform.position = pos;
        t.color = c;
        t.text = txt;

        StartCoroutine(TextAnim(t));
    }

    IEnumerator TextAnim(TMP_Text text) {
        Color startColor = text.color;
        Vector3 startPos = text.transform.position;

        float t = 0;
        while (t < 1) {
            t += Time.deltaTime * 0.75f;
            text.color = Color.Lerp(startColor, new Color(startColor.r, startColor.g, startColor.b, 0), t);
            text.transform.position = Vector3.Lerp(startPos, startPos + Vector3.up * 4, t);

            yield return null;
        }

        Destroy(text.gameObject);
    }

    public float maxZForce;
    public Vector3 ClampDragForce(Vector3 v) {
        v.z = Mathf.Clamp(v.z, -maxZForce, maxZForce);
        v.y = Mathf.Clamp(v.y, -maxZForce, maxZForce);

        return v;
    }

    public PlayerBase GetEnemy(PlayerBase p) {
        if (p == myPlayer)
            return enemyPlayer;
        else return myPlayer;
    }
}
