using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    [HideInInspector]
    public Vector2 position;
    [HideInInspector]
    public float tileSize;
    public GameObject num1Prefab;
    public GameObject num2Prefab;
    public GameObject num3Prefab;
    public GameObject num4Prefab;
    public GameObject num5Prefab;
    public GameObject num6Prefab;
    public GameObject num7Prefab;
    [HideInInspector]
    public GameObject shownNumberObject;
    [HideInInspector]
    public float tileScaleFactor;

    public void setValues(Vector2 position, float tileSize, Transform parent) {
        this.position = position;
        this.tileSize = tileSize;
        this.transform.parent = parent;
        this.tileScaleFactor = tileSize;
        transform.position = position;
        transform.localScale *= tileScaleFactor;
    }


    public void addNumberToTile(int num) {
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
            shownNumberObject.transform.localScale *= tileScaleFactor;

            Vector3 pos = shownNumberObject.transform.position;
            pos.z -= 1;
            shownNumberObject.transform.position = pos;
        }
    }

}
