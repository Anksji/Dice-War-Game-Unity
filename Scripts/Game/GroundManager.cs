using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundManager : MonoBehaviour {
    public static GroundManager _instance;

    public Ground groundPrefab;

    public int sizeX, sizeY;
    public float scale;

    public Ground[,] grounds;

    private void Awake() {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start() {
        grounds = new Ground[sizeX, sizeY];

        for (int i = -sizeX / 2; i < sizeX / 2; i++) {
            for (int j = -sizeY / 2; j < sizeY / 2; j++) {
                var g = Instantiate(groundPrefab, transform);

                g.transform.localPosition = new Vector3(i * scale, 0, j * scale);
                g.transform.localScale = Vector3.one * scale;

                grounds[i + sizeX / 2, j + sizeY / 2] = g;
            }
        }

        CombineToBiggerCubes();
    }

    void CombineToBiggerCubes() {
        int n = Random.Range(15, 20);

        for (int i = 0; i < n; i++) {
            int size = Random.Range(2, 4);

            int times = 0;
            while (times < 200) {
                int x = Random.Range(0, sizeX);
                int y = Random.Range(0, sizeY);

                bool isGood = true;

                for (int k = x; k < x + size; k++) {
                    for (int l = y; l < y + size; l++) {
                        if (k >= sizeX || l >= sizeY || grounds[k, l] == null) isGood = false;
                    }
                }

                if (isGood) {
                    CombineCubes(x, y, size);

                    break;
                }

                times++;
            }
        }
    }

    void CombineCubes(int x, int y, int size) {
        for (int k = x; k < x + size; k++) {
            for (int l = y; l < y + size; l++) {
                Destroy(grounds[k, l].gameObject);
                grounds[k, l] = null;
            }
        }

        var g = Instantiate(groundPrefab, transform);

        g.transform.localPosition = new Vector3((x - sizeX / 2) * scale + (size - 1) * scale / 2f, 0, (y - sizeY / 2) * scale + (size - 1) * scale / 2f);
        g.transform.localScale = new Vector3(scale * size, scale, scale * size);
        g.transform.localEulerAngles = Vector3.zero;

        StartCoroutine(Delay(g.transform, x, y, size));
    }

    IEnumerator Delay(Transform t, int x, int y, int size) {
        yield return new WaitForSecondsRealtime(0.2f);

        t.localPosition = new Vector3((x - sizeX / 2) * scale + (size - 1) * scale / 2f, 0, (y - sizeY / 2) * scale + (size - 1) * scale / 2f);
        t.localEulerAngles = Vector3.zero;
    }
}
