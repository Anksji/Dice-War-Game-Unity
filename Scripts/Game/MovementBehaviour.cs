using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBehaviour : MonoBehaviour {
    [HideInInspector] public Stats stats;

    [Space(15)]
    [Header("Movement")]
    [SerializeField] float rotationSpeed = 10f;
    public Transform target;


    public bool canMove = true;

    float slowTimer;
    float movementModifier = 1;

    Rigidbody rb;

    private void Start() {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        if (target == null || !canMove || DiceGameManager._instance.isPaused) {
            return;
        }

        SetDirection();
        Move();

        if (slowTimer <= 0)
            movementModifier = 1;
    }

    public void AddSlow(int slowAmount, float time = 1.5f) {
        movementModifier = 1 * (1 - slowAmount / 100f);
        slowTimer = time;
    }

    /// <summary>
    /// Moves the player towards the target in FixedUpdate
    /// </summary>
    private void Move() {
        Vector3 moveDir = transform.forward;
        moveDir *= stats.movementSpeed * movementModifier;

        rb.velocity = new Vector3(moveDir.x, rb.velocity.y, moveDir.z);
    }

    /// <summary>
    /// Rotates the player towards the target smoothly
    /// </summary>
    void SetDirection() {
        var offset = 0f;
        Vector2 direction = new Vector2(target.transform.position.x, target.transform.position.z) - new Vector2(transform.position.x, transform.position.z);
        direction.Normalize();
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.AngleAxis(angle + offset, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }
}
