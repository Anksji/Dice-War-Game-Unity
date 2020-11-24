using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootEffect : MonoBehaviour {
    public float speed;

    public GameObject impactEffect;

    [HideInInspector] public Vector3 endPos;

    private IEnumerator Start() {
        yield return new WaitForSeconds(3f);

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        if (Vector3.Distance(transform.position, endPos) < 0.1f) {
            if (impactEffect != null) {
                var g = Instantiate(impactEffect);
                g.transform.position = transform.position + Vector3.up * 0.25f;
            }

            gameObject.SetActive(false);

            return;
        }

        float step = speed * Time.deltaTime;

        HandlePosition(step);
        HandleRotation(step);
    }

    void HandlePosition(float step) {
        transform.position = Vector3.MoveTowards(transform.position, endPos + Vector3.up * 1, step);
    }

    void HandleRotation(float step) {
        Vector3 dir = endPos - transform.position;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, dir, step, 0.0f);

        // Calculate a rotation a step closer to the target and applies rotation to this object
        transform.rotation = Quaternion.LookRotation(newDirection);
    }
}
