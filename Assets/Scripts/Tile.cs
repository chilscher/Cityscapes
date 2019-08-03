using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    [HideInInspector]
    public Vector3 position;
    [HideInInspector]
    public float tileSize;
    [HideInInspector]
    public GameObject shownNumberObject;
    [HideInInspector]
    public float tileScaleFactor;


    public void setValues(Vector2 position, float tileSize, Transform parent) {
        this.position = position;
        this.tileSize = tileSize;
        this.transform.SetParent(parent);
        this.tileScaleFactor = tileSize;
        transform.position = position;

        transform.localScale *= tileScaleFactor;

        Vector3 pos = transform.localPosition;
        pos.z = 0;
        transform.localPosition = pos;

        
        
    }
}
