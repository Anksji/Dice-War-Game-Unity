using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerBase : MonoBehaviour {
    public TrajectoryVisualizer trajectory;
    public Color color;

    public float timeToRefillDice = 8f;
    [SerializeField] protected Image diceFillImage;
    [HideInInspector] public Dice currentDice;
    [SerializeField] protected Transform diceSpawn;

    public bool isAi;

    [HideInInspector]
    public List<Human> humans;

    public Human humanPrefab;

    public Transform castle;
    [HideInInspector] public int health;
    [SerializeField] TMP_Text healthText;

    protected void Init() {
        trajectory.SetColor(color);
        diceFillImage.fillAmount = 0.95f;

        humans = new List<Human>();

        //Setting up initial Health
        AddHealth(50);
    }

    protected virtual void Update() {
        if (DiceGameManager._instance.isPaused) return;

        diceFillImage.fillAmount += Time.deltaTime / GetTimeToRefill();

        if (diceFillImage.fillAmount >= 0.99f && currentDice == null) {
            diceFillImage.fillAmount = 0;
            StartCoroutine(SpawnNewDice());
        }
    }

    public void DiceThrown() {
        currentDice.isThrown = true;
        currentDice = null;
    }

    IEnumerator SpawnNewDice() {
        yield return new WaitForSeconds(0.5f);

        currentDice = Instantiate(DiceGameManager._instance.dicePrefab, diceSpawn);
        currentDice.transform.localPosition = Vector3.zero;
        currentDice.player = this;

        Debug.Log("dice color spawn new dice");
    }

    public void SpawnHumans(int n, Vector3 position, Human humanToClone = null) {
        if (humans.Count > 200) return;

        if (isAi) { //Randomly changing the human class if it is an ai
            humanPrefab = GameUi._instance.cards[Random.Range(0, 3)].humanPrefab;
        }

        for (int i = 0; i < n; i++) {
            Human h;
            if (humanToClone == null)
                h = Instantiate(humanPrefab);
            else h = Instantiate(humanToClone);

            Vector2 pos = Random.insideUnitCircle * (0.75f + n / 2.3f);
            h.transform.position = new Vector3(position.x + pos.x, position.y, position.z + pos.y);

            h.Init(this);

            humans.Add(h);
        }

        if (humanToClone == null)
            DiceGameManager._instance.InstatiateTMPText(position + Vector3.up * 2f, color, "+" + n);
    }

    float GetTimeToRefill() {
        if (isAi) return timeToRefillDice;

        return timeToRefillDice * (1 - UpgradeShop._instance.diceLevel / 100f);
    }

    public void AddHealth(int n) {
        health += n;

        if (health <= 0) {
            DiceGameManager._instance.GameOver(isAi);
        } else
            healthText.text = health + "";
    }
}
