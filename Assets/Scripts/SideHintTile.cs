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

    public Sprite[] whiteSprites;

    private Color correctColor;
    private Color incorrectColor;

    public void initialize(int hintValue) {
        this.hintValue = hintValue;
        background = transform.GetChild(0).GetComponent<SpriteRenderer>();
        arrow = transform.GetChild(1).GetComponent<SpriteRenderer>();
        number = transform.GetChild(2).GetComponent<SpriteRenderer>();
        redBorder = transform.GetChild(3).GetComponent<SpriteRenderer>();

        setNumberColors();
        addNumberToTile(hintValue);

        Skin tempSkin = StaticVariables.skin;
        if (StaticVariables.isTutorial) { tempSkin = StaticVariables.allSkins[0]; }
        background.GetComponent<SpriteRenderer>().color = InterfaceFunctions.getColorFromString(tempSkin.streetColor);
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

    public void setNumberColors() {
        if (!StaticVariables.isTutorial) {

            ColorUtility.TryParseHtmlString(StaticVariables.skin.citizenColor, out incorrectColor);
            ColorUtility.TryParseHtmlString(StaticVariables.skin.satisfiedCitizenColor, out correctColor);
        }
        else {
            ColorUtility.TryParseHtmlString(StaticVariables.mintHex, out incorrectColor);
            ColorUtility.TryParseHtmlString(StaticVariables.greenHex, out correctColor);
        }
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
        number.sprite = whiteSprites[num - 1];
        number.color = incorrectColor;

    }

    public void setAppropriateColor() {
        number.color = incorrectColor;
        if (StaticVariables.changeResidentColorOnCorrectRows && !StaticVariables.isTutorial) {
            if ((numBuildingsCurrentlyVisible() == hintValue) && (row[0].shownNumber != 0)) {
                number.color = correctColor;
            }
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
            redBorder.transform.position = pos;
        }
        else if(amt == 180) {
            Vector3 pos = number.transform.position;
            pos.y -= numberMoveAmt;
            number.transform.position = pos;
            redBorder.transform.position = pos;
        }
        else if (amt == 90) {
            Vector3 pos = number.transform.position;
            pos.x -= numberMoveAmt;
            number.transform.position = pos;
            redBorder.transform.position = pos;
        }
        else if (amt == 270) {
            Vector3 pos = number.transform.position;
            pos.x += numberMoveAmt;
            number.transform.position = pos;
            redBorder.transform.position = pos;
        }
    }

    public void addRedBorder() {
        redBorder.gameObject.SetActive(true);
    }

    public void removeRedBorder() {
        redBorder.gameObject.SetActive(false);
    }
    

}
