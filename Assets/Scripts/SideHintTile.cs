using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideHintTile : MonoBehaviour {

    private Vector2 position;
    private float tileSize;
    private int hintValue;

    public GameObject num1Prefab;
    public GameObject num2Prefab;
    public GameObject num3Prefab;
    public GameObject num4Prefab;
    public GameObject num5Prefab;
    public GameObject num6Prefab;
    public GameObject num7Prefab;
    private GameObject shownNumber;

    public void initialize(Vector2 position, float tileSize, int hintValue, Transform parent) {
        this.position = position;
        this.tileSize = tileSize;
        this.hintValue = hintValue;
        this.transform.parent = parent;

        transform.position = position;
        transform.localScale *= tileSize;
        addNumberToTile(hintValue);
    }

    public void clicked() {
        //print(hintValue);
    }

    private void OnMouseDown() {
        clicked();
    }


    private void addNumberToTile(int num) {
        if (shownNumber != null) {
            Destroy(shownNumber);
        }
        GameObject relevantPrefab = num1Prefab;
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
        shownNumber = Instantiate(relevantPrefab);
        shownNumber.transform.position = this.transform.position;
        shownNumber.transform.parent = this.transform;

        Vector3 pos = shownNumber.transform.position;
        pos.z -= 1;
        shownNumber.transform.position = pos;
    }

}
