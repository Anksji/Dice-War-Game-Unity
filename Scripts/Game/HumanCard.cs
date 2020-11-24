using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HumanCard : MonoBehaviour {
    public Human humanPrefab;

    // Start is called before the first frame update
    void Start() {

    }

    public void OnClick() {
        GameUi._instance.SetSizes(this);

        DiceGameManager._instance.myPlayer.humanPrefab = humanPrefab;
    }
}
