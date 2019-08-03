using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideHintTile : Tile {

    private int hintValue;
    public PuzzleTile[] row;
    private SpriteRenderer background;
    private SpriteRenderer number;

    public Sprite number1Sprite;
    public Sprite number2Sprite;
    public Sprite number3Sprite;
    public Sprite number4Sprite;
    public Sprite number5Sprite;
    public Sprite number6Sprite;
    public Sprite number7Sprite;

    public Sprite green1Sprite;
    public Sprite green2Sprite;
    public Sprite green3Sprite;
    public Sprite green4Sprite;
    public Sprite green5Sprite;
    public Sprite green6Sprite;
    public Sprite green7Sprite;

    private Sprite normalSprite;
    private Sprite greenSprite;

    public void initialize(int hintValue) {
        this.hintValue = hintValue;
        background = transform.GetChild(0).GetComponent<SpriteRenderer>();
        number = transform.GetChild(1).GetComponent<SpriteRenderer>();
        addNumberToTile(hintValue);
    }

    public int numBuildingsCurrentlyVisible() {
        int count = 0;
        int highest = 0;
        foreach (PuzzleTile t in row) {
            if (t.shownNumber != 0) {
                if (t.shownNumber > highest) {
                    highest = t.shownNumber;
                    count++;
                }
            }
        }
        return count;
    }

    public bool isRowValid() {
        foreach (PuzzleTile t in row) {
            if (t.shownNumber == 0) {
                return false;
            }
        }
        return (numBuildingsCurrentlyVisible() == hintValue);
    }

    public void addHint() {
        addNumberToTile(hintValue);
    }
    

    public new void addNumberToTile(int num) {
        switch (num) {
            case 1:
                normalSprite = number1Sprite;
                greenSprite = green1Sprite;
                break;
            case 2:
                normalSprite = number2Sprite;
                greenSprite = green2Sprite;
                break;
            case 3:
                normalSprite = number3Sprite;
                greenSprite = green3Sprite;
                break;
            case 4:
                normalSprite = number4Sprite;
                greenSprite = green4Sprite;
                break;
            case 5:
                normalSprite = number5Sprite;
                greenSprite = green5Sprite;
                break;
            case 6:
                normalSprite = number6Sprite;
                greenSprite = green6Sprite;
                break;
            case 7:
                normalSprite = number7Sprite;
                greenSprite = green7Sprite;
                break;
        }
        number.sprite = normalSprite;
    }

    public void setSpriteToAppropriateColor() {
        if(numBuildingsCurrentlyVisible() == hintValue) {
            number.sprite = greenSprite;
        }
        else {
            number.sprite = normalSprite;
        }
    }

    public void rotateHint(int amt, float tileSize) {
        background.transform.Rotate(new Vector3(0, 0, amt));
        float numberMoveAmt = tileSize / 4;
        if (amt == 0) {
            Vector3 pos = number.transform.position;
            pos.y += numberMoveAmt;
            number.transform.position = pos;
        }
        else if(amt == 180) {
            Vector3 pos = number.transform.position;
            pos.y -= numberMoveAmt;
            number.transform.position = pos;
        }
        else if (amt == 90) {
            Vector3 pos = number.transform.position;
            pos.x -= numberMoveAmt;
            number.transform.position = pos;
        }
        else if (amt == 270) {
            Vector3 pos = number.transform.position;
            pos.x += numberMoveAmt;
            number.transform.position = pos;
        }
    }

}
