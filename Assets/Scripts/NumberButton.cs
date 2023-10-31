//for Cityscapes, copyright Cole Hilscher 2020

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberButton: Tile {
    //one of the buttons that the player can click
    //clicking the button selects the number from GameManager
    [HideInInspector]
    public int value;
    public Sprite[] numberSprites;
    private GameManager gameManager;
    private Image number;

    public void initialize(int value, GameManager gm) {
        this.value = value;
        gameManager = gm;
        number = transform.GetChild(1).GetComponent<Image>();
        addNumberToTile(value);
    }
    
    private void OnMouseDown() {
        //click the button, set the GameManager number, easy peasy
        if (gameManager.canClick) {
            gameManager.switchNumber(value);
        }
    }
    
    public void addNumberToTile(int num) {
        //adds the sprite for num to the tile, called when the puzzle visuals are being configured
        number.sprite = numberSprites[num];
    }

}
