using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberButton: Tile {

    [HideInInspector]
    public int value;
    public Sprite[] numberSprites;
    private GameManager gameManager;
    private SpriteRenderer number;

    public void initialize(int value, GameManager gm) {
        this.value = value;
        gameManager = gm;
        number = transform.GetChild(1).GetComponent<SpriteRenderer>();
        addNumberToTile(value);
    }


    private void OnMouseDown() {
        if (gameManager.canClick) {
            gameManager.switchNumber(value);
            gameManager.showNumberButtonClicked(this);

        }
    }


    public void addNumberToTile(int num) {
        number.sprite = numberSprites[num];
    }

}
