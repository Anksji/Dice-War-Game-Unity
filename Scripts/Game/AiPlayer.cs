using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiPlayer : PlayerBase {

    float timeToShoot;
    bool isShooting;

    // Start is called before the first frame update
    void Start() {
        base.Init();

    }

    public void Setup() {
        timeToRefillDice = Mathf.Lerp(7f, 2.2f, DiceGameManager._instance.levelNum / 14f);
        timeToShoot = Mathf.Lerp(3.0f, 0.2f, DiceGameManager._instance.levelNum / 20f);
    }

    protected override void Update() {
        base.Update();

        if (!isShooting && currentDice != null) {
            StartCoroutine(ShootCoroutine());
        }
    }

    IEnumerator ShootCoroutine() {
        isShooting = true;

        yield return new WaitForSeconds(Random.Range(0.8f * timeToShoot, 1.2f * timeToShoot));

        float y = Random.Range(-460, -130);
        Vector3 forceV = new Vector3(Random.Range(-120, 120), -y, y) * 1.4f;
        Vector3 currentForce;

        float t = 0;
        float timeToAim = Random.Range(1f, 3f);

        while (t < timeToAim) {
            t += Time.deltaTime;

            currentForce = Vector3.Lerp(Vector3.zero, forceV, t / timeToAim);
            trajectory.UpdateTrajectory(currentForce, currentDice.rb, currentDice.transform.position);

            yield return null;
        }

        trajectory.HideLine();
        currentDice.Shoot(new Vector3(forceV.x, forceV.y, 0), 1f, 1, true);

        isShooting = false;
    }
}
