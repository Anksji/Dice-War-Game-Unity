using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Cloner : MonoBehaviour {
    [SerializeField] bool isEnemyCloner;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] TMP_Text text;

    [SerializeField] int value;

    List<Human> alreadyCloned;

    // Start is called before the first frame update
    void Start() {
        if (isEnemyCloner) {
            sr.color = DiceGameManager._instance.enemyPlayer.color;
        } else {
            sr.color = DiceGameManager._instance.myPlayer.color;
        }

        alreadyCloned = new List<Human>();

        text.text = "x" + value;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Human")) {
            Human h = other.gameObject.GetComponent<Human>();

            if (h.player.isAi != isEnemyCloner) return;
            if (alreadyCloned.Contains(h) || h.lifeTime < 0.8f) return;

            alreadyCloned.Add(h);

            h.player.SpawnHumans(value - 1, h.transform.position, h);
        }
    }
}
