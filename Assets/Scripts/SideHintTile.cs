using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideHintTile : Tile {
    
    private int hintValue;
    

    public void initialize(Vector2 position, float tileSize, int hintValue, Transform parent) {
        setValues(position, tileSize, parent);
        this.hintValue = hintValue;
        addNumberToTile(hintValue);
    }

}
