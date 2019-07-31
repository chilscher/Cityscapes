using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberButton: Tile {

    [HideInInspector]
    public int value;
    private GameManager gameManager;

    public void initialize(int value, GameManager gm) {
        this.value = value;
        gameManager = gm;
        addNumberToTile(value);
    }


    private void OnMouseDown() {
        gameManager.switchNumber(value);
        gameManager.showNumberButtonClicked(this);
    }


    public new void addNumberToTile(int num) {
        if (shownNumberObject != null) {
            Destroy(shownNumberObject);
        }
        GameObject relevantPrefab = null;
        switch (num) {
            case 1:
                relevantPrefab = num1Prefab;
                break;
            case 2:
                relevantPrefab = num2Prefab;
                break;
            case 3:
                relevantPrefab = num3Prefab;
                break;
            case 4:
                relevantPrefab = num4Prefab;
                break;
            case 5:
                relevantPrefab = num5Prefab;
                break;
            case 6:
                relevantPrefab = num6Prefab;
                break;
            case 7:
                relevantPrefab = num7Prefab;
                break;
        }
        if (relevantPrefab != null) {
            shownNumberObject = Instantiate(relevantPrefab);
            shownNumberObject.transform.position = this.transform.position;
            shownNumberObject.transform.parent = this.transform;
            shownNumberObject.transform.localScale *= transform.localScale.x;

            Vector3 pos = shownNumberObject.transform.localPosition;
            pos.z = -0.01f;
            shownNumberObject.transform.localPosition = pos;

        }
    }

}
