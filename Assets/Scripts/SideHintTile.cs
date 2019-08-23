using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideHintTile : Tile {

    private int hintValue;
    [HideInInspector]
    public PuzzleTile[] row;
    private SpriteRenderer background;
    private SpriteRenderer arrow;
    private SpriteRenderer number;
    private SpriteRenderer redBorder;

    public Sprite[] incorrectNumberSprites;
    public Sprite[] correctNumberSprites;

    private Sprite incorrectSprite;
    private Sprite correctSprite;

    public void initialize(int hintValue) {
        this.hintValue = hintValue;
        background = transform.GetChild(0).GetComponent<SpriteRenderer>();
        arrow = transform.GetChild(1).GetComponent<SpriteRenderer>();
        number = transform.GetChild(2).GetComponent<SpriteRenderer>();
        redBorder = number.transform.GetChild(0).GetComponent<SpriteRenderer>();
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
        List<int> usedValues = new List<int>();
        foreach (PuzzleTile t in row) {
            if (t.shownNumber == 0) {
                return false;
            }
            if (usedValues.Contains(t.shownNumber)) {
                return false;
            }
            usedValues.Add(t.shownNumber);
        }
        return (numBuildingsCurrentlyVisible() == hintValue);
    }

    public void addHint() {
        addNumberToTile(hintValue);
    }
    

    public void addNumberToTile(int num) {
        incorrectSprite = incorrectNumberSprites[num - 1];
        correctSprite = correctNumberSprites[num - 1];
        number.sprite = incorrectSprite;
    }

    public void setSpriteToAppropriateColor() {
        if (StaticVariables.changeHintColorOnCorrectRows) {
            if (numBuildingsCurrentlyVisible() == hintValue) {
                number.sprite = correctSprite;
            }
            else {
                number.sprite = incorrectSprite;
            }
        }
        else {
            number.sprite = incorrectSprite;
        }
    }

    public void rotateHint(int amt, float tileSize) {
        background.transform.Rotate(new Vector3(0, 0, amt));
        arrow.transform.Rotate(new Vector3(0, 0, amt));
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

    public void addRedBorder() {
        redBorder.gameObject.SetActive(true);
    }

    public void removeRedBorder() {
        redBorder.gameObject.SetActive(false);
    }

}
