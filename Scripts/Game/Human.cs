using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour {
    [HideInInspector] public PlayerBase player;
    [SerializeField] Renderer renderer;

    public Stats stats;

    DiceShooter shooter;
    MovementBehaviour movement;

    [Space(15)]
    int currentHealth;
    bool isDead;

    bool isGoingToBase;

    public Animator animator;

    [HideInInspector] public float lifeTime;


    // Start is called before the first frame update
    void Start() {
        shooter = GetComponent<DiceShooter>();
        movement = GetComponent<MovementBehaviour>();

        shooter.stats = stats;
        movement.stats = stats;

        currentHealth = stats.health;

        lifeTime = 0;

        InvokeRepeating("GetTarget", Random.Range(0.05f, 0.8f), 0.5f);
    }

    /// <summary>
    /// Initializes the human
    /// </summary>
    /// <param name="p"></param>
    public void Init(PlayerBase p) {
        player = p;
        renderer.material.color = player.color;
    }

    // Update is called once per frame
    void Update() {
        if (isDead || DiceGameManager._instance.isPaused) return;

        lifeTime += Time.deltaTime;

        if (shooter.isShooting) {
            movement.canMove = false;
        } else {
            movement.canMove = true;

            if (movement.target != null) {
                animator.Play("Run");

                if (isGoingToBase && Vector3.Distance(transform.position, movement.target.position) < 1.25f) {
                    Die();
                    DiceGameManager._instance.GetEnemy(player).AddHealth(-1);
                }
            } else {
                animator.Play("Idle");
            }
        }
    }

    /// <summary>
    /// Sets the target cube of the Human
    /// </summary>
    void GetTarget() {
        if (isDead || DiceGameManager._instance.isPaused) return;

        Human closestHuman = GetClosestHuman(Mathf.Infinity);
        Transform enemyCastle = DiceGameManager._instance.GetEnemy(player).castle;


        if (closestHuman != null) {
            if (Vector3.Distance(transform.position, closestHuman.transform.position) > Vector3.Distance(transform.position, enemyCastle.position)) {
                SetEnemyCastleAsTarget(enemyCastle);
                return;
            }

            movement.target = closestHuman.transform;
            shooter.target = closestHuman;

            isGoingToBase = false;
        } else {
            SetEnemyCastleAsTarget(enemyCastle);
        }
    }

    void SetEnemyCastleAsTarget(Transform enemyCastle) {
        shooter.target = null;

        isGoingToBase = true;
        movement.target = enemyCastle;
    }

    /// <summary>
    /// Returns the closest Enemy human if it is closer than maxDistance
    /// </summary>
    /// <param name="maxDistance"></param>
    /// <returns></returns>
    Human GetClosestHuman(float maxDistance) {
        Human closestHuman = null;
        float closestDistance = Mathf.Infinity;

        List<Human> humans = DiceGameManager._instance.GetEnemy(player).humans;

        foreach (Human h in humans) {
            float d = Vector3.Distance(transform.position, h.transform.position);
            if (d < closestDistance && d < maxDistance) {
                closestDistance = d;
                closestHuman = h;
            }
        }

        return closestHuman;
    }


    /// <summary>
    /// Dying related method
    /// </summary>
    public void Die() {
        if (isDead) return;

        isDead = true;
        animator.Play("Die");

        player.humans.Remove(this);

        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;

        Destroy(gameObject, 1.4f);
    }

    private void OnCollisionEnter(Collision collision) {
        //Colliding with enemy dice instantly kills the human
        Dice dice = collision.gameObject.GetComponent<Dice>();
        if (dice != null && dice.player != player) {
            Die();

            DiceGameManager._instance.InstatiateTMPText(transform.position + Vector3.up * 0.5f, player.color, "-1");
        }
    }

    /// <summary>
    /// Taking damage, dies if no more health
    /// </summary>
    /// <param name="dmg"></param>
    /// <param name="damageFrom"></param>
    public void TakeDamage(int dmg, Human damageFrom) {
        currentHealth -= dmg;

        if (currentHealth <= 0) {
            Die();
        }
    }
}
