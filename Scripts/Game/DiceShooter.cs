using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceShooter : MonoBehaviour {
    [HideInInspector] public Human target;

    [Header("General")]
    public ShootEffect shootEffect;
    public bool attachToTransform;


    [HideInInspector] public Stats stats;
    private float atkCountdown = 0f;

    public Transform firePoint;

    [HideInInspector]
    public bool isShooting;

    Human human;

    ObjectPool pool;

    private void Start() {
        human = GetComponent<Human>();

        if (shootEffect != null) {
            pool = gameObject.AddComponent<ObjectPool>();
            pool.InitPool(shootEffect.gameObject, 0, attachToTransform ? firePoint : null);
        }
    }

    // Update is called once per frame
    void Update() {
        if (target == null || stats.range < Vector3.Distance(target.transform.position, transform.position) || DiceGameManager._instance.isPaused) {
            isShooting = false;
            return;
        }

        SetDirection();

        if (atkCountdown <= 0f) {
            Shoot();

            atkCountdown = stats.atkSpeed;
            isShooting = true;
        }

        atkCountdown -= Time.deltaTime;

    }

    void Shoot() {
        if (human.animator != null)
            human.animator.Play("Fight");

        if (shootEffect != null) {
            var s = pool.GetObjectFromPool().GetComponent<ShootEffect>();
            s.endPos = target.transform.position;

            s.transform.position = firePoint.position;
            s.gameObject.SetActive(true);
        }

        if (stats.splashRadius > 0) {
            DoSplashDamage();
        } else {
            target.TakeDamage(stats.damage, human);
        }
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
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 90 * Time.deltaTime);
    }

    void DoSplashDamage() {
        Collider[] hitColliders = Physics.OverlapSphere(target.transform.position, stats.splashRadius);
        foreach (var hitCollider in hitColliders) {
            Human h = hitCollider.GetComponent<Human>();

            if (h != null)
                h.TakeDamage(stats.damage, human);
        }
    }
}
