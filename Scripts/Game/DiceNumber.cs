using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceNumber {
    public static int GetNumber(Dice dice) {
        int n = 0;

        if (Vector3.Cross(Vector3.up, dice.transform.right).magnitude < 0.5f) {
            if (Vector3.Dot(Vector3.up, dice.transform.right) > 0) {
                n = 5;
            } else {
                n = 6;
            }
        } else if (Vector3.Cross(Vector3.up, dice.transform.up).magnitude < 0.5f) {
            if (Vector3.Dot(Vector3.up, dice.transform.up) > 0) {
                n = 3;
            } else {
                n = 1;
            }
        } else if (Vector3.Cross(Vector3.up, dice.transform.forward).magnitude < 0.5f) {
            if (Vector3.Dot(Vector3.up, dice.transform.forward) > 0) {
                n = 2;
            } else {
                n = 4;
            }
        }

        return n;
    }
}
