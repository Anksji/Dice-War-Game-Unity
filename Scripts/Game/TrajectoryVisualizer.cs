using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryVisualizer : MonoBehaviour {
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private int lineSegmentCount = 30;

    private List<Vector3> linePoints;

    [SerializeField] Transform spriteTransform;

    // Start is called before the first frame update
    void Start() {
        linePoints = new List<Vector3>();
    }

    public void UpdateTrajectory(Vector3 force, Rigidbody rb, Vector3 start) {
        force = DiceGameManager._instance.ClampDragForce(force);

        Vector3 velocity = force / rb.mass * Time.fixedDeltaTime;
        float flightDurration = 2 * velocity.y / Physics.gravity.y;
        float stepTime = flightDurration / lineSegmentCount;

        linePoints.Clear();

        for (int i = 0; i < lineSegmentCount; i++) {
            float stepTimePassed = stepTime * i;

            Vector3 movement = new Vector3(
                velocity.x * stepTimePassed,
                velocity.y * stepTimePassed - 0.5f * stepTimePassed * stepTimePassed * Physics.gravity.y,
                velocity.z * stepTimePassed
            );

            linePoints.Add(-movement + start);
        }

        spriteTransform.position = new Vector3(linePoints[lineSegmentCount - 1].x, 1.51f, linePoints[lineSegmentCount - 1].z);

        lineRenderer.positionCount = lineSegmentCount;
        lineRenderer.SetPositions(linePoints.ToArray());

        spriteTransform.gameObject.SetActive(true);
    }

    public void HideLine() {
        lineRenderer.positionCount = 0;
        spriteTransform.gameObject.SetActive(false);
    }

    public void SetColor(Color c) {
        lineRenderer.startColor = lineRenderer.endColor = c;
    }
}
