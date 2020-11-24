using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour {
    public PlayerBase player;

    [SerializeField] Renderer renderer;
    [HideInInspector] public Rigidbody rb;

    public bool isThrown;

    bool isSpawned;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        renderer.material.color = player.color;
        Debug.Log("dice color is " + player.color);
    }

    // Update is called once per frame
    void Update() {
        if (isSpawned) return;

        if (isThrown) {
            if (rb.angularVelocity.magnitude < 0.02f && rb.velocity.magnitude < 0.02f) {
                isSpawned = true;
                player.SpawnHumans(DiceNumber.GetNumber(this), transform.position);

                Destroy(gameObject, 0.5f);
            }
        }
    }

    private Vector3 velocityBeforePhysicsUpdate;
    void FixedUpdate() {
        velocityBeforePhysicsUpdate = rb.velocity;
    }

    public void Shoot(Vector3 force, float forceMultiplier, float rotationMultiplier, bool isEnemy = false) {
        force.z = force.y;
        if (isEnemy) force.z *= -1;

        force *= forceMultiplier;

        force = DiceGameManager._instance.ClampDragForce(force);

        player.currentDice.rb.AddForce(force);
        player.currentDice.rb.angularVelocity = rotationMultiplier * new Vector3(Random.Range(force.x * 1f, force.x * 1.5f), Random.Range(force.y * 0.75f, force.y * 1.25f), force.z);

        player.DiceThrown();
    }

    private void OnCollisionEnter(Collision collision) {
        Human h = collision.gameObject.GetComponent<Human>();
        if (h != null) {
            if (h.player != player) {
                h.Die();

                rb.velocity = velocityBeforePhysicsUpdate;
            }
        }
    }
}
