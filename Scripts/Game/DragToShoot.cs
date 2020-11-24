using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragToShoot : MonoBehaviour {
    public PlayerBase player;

    [SerializeField] float forceMultiplier = 2;
    [SerializeField] float rotationMultiplier = 1f;

    private Vector3 mousePressDownPos;
    private Vector3 mouseReleasePos;

    // Start is called before the first frame update
    void Start() {
        if (player != DiceGameManager._instance.myPlayer) enabled = false;
    }

    private void OnMouseDown() {
        mousePressDownPos = Input.mousePosition;
    }

    private void OnMouseDrag() {
        if (player.currentDice == null) return;

        Vector3 forceInit = Input.mousePosition - mousePressDownPos;
        Vector3 forceV = new Vector3(forceInit.x, forceInit.y, forceInit.y) * forceMultiplier;

        player.trajectory.UpdateTrajectory(forceV, player.currentDice.rb, player.currentDice.transform.position);
    }

    private void OnMouseUp() {
        if (player.currentDice == null) return;

        player.trajectory.HideLine();

        mouseReleasePos = Input.mousePosition;

        if (Vector2.Distance(mousePressDownPos, mouseReleasePos) > 100)
            player.currentDice.Shoot(mouseReleasePos - mousePressDownPos, forceMultiplier, rotationMultiplier);
    }
}
