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
    }

    

}
