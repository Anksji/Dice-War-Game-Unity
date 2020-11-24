using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour {
    MeshRenderer renderer;

    [SerializeField] Color myColor, enemyColor;

    public bool isChangingColor;

    private void Start() {
        renderer = GetComponent<MeshRenderer>();
    }

    private void OnCollisionEnter(Collision collision) {
        if (!isChangingColor) return;

        Human h = collision.gameObject.GetComponent<Human>();

        if (h != null) {
            if (h.player.isAi) {
                renderer.material.color = enemyColor;
            } else {
                renderer.material.color = myColor;
            }
        }
    }
}
