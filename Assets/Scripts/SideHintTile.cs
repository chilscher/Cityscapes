//for Cityscapes, copyright Cole Hilscher 2020

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideHintTile : Tile {
    //contains information for the Side Hints, also known as Residents, which are the clues on the side of the puzzle
    //the SideHintTile script is attached to a SideHintTilePrefab object. The PuzzleGenerator makes an instance of the SideHintTilePrefab and provides it with data

    private int hintValue;
    [HideInInspector]
    public PuzzleTile[] row; //all of the Puzzle Tiles that this SideHintTile's hint references (this SideHintTile is the number at the end of a row of buildings)

    //some visual elements of the SideHintTile Prefab
    private SpriteRenderer background;
    private SpriteRenderer arrow;
    private SpriteRenderer number;
    private SpriteRenderer redBorder;

    //the number sprites
    public Sprite[] whiteSprites;

    //the colors that the numbers in the SideHintTiles can be
    private Color correctColor;
    private Color incorrectColor;

    public void initialize(int hintValue) {
        //creates the sideHintTile. Here goes all of the code that defines the private variables used later
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
        //iterates through the PuzzleTiles that this SideHintTile looks upon, and determines how many buildings are currently visible
        //note: does NOT reference the solution, but instead whatever buildings are actually there
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
        //sets the colors that the SideHintTile number can be, based off of the current skin.
        //the tutorial uses its own colors, which are the ones used in the basic skin
        if (!StaticVariables.isTutorial) {

            ColorUtility.TryParseHtmlString(StaticVariables.skin.citizenColor, out incorrectColor);
            ColorUtility.TryParseHtmlString(StaticVariables.skin.satisfiedCitizenColor, out correctColor);
        }
        else {
            ColorUtility.TryParseHtmlString(InterfaceFunctions.getDefaultSkin().citizenColor, out incorrectColor);
            ColorUtility.TryParseHtmlString(InterfaceFunctions.getDefaultSkin().satisfiedCitizenColor, out correctColor);
        }
    }

    public bool isRowValid() {
        //checks if this SideHintTile's building requirement is satisfied. Part of the PuzzleGenerator function to check if the player has won
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
        //shows the number on the tile
        addNumberToTile(hintValue);
    }
    

    public void addNumberToTile(int num) {
        //shows the number on the tile
        number.sprite = whiteSprites[num - 1];
        number.color = incorrectColor;

    }

    public void setAppropriateColor() {
        //colors the SideHintTile number based on if its building criterion is satisfied
        //does nothing if the relevant upgrade is not toggled, or if this is the tutorial
        number.color = incorrectColor;
        if (StaticVariables.changeResidentColorOnCorrectRows && !StaticVariables.isTutorial) {
            if ((numBuildingsCurrentlyVisible() == hintValue) && (row[0].shownNumber != 0)) {
                number.color = correctColor;
            }
        }
    }

    public void rotateHint(int amt, float tileSize) {
        //rotate the arrow and puzzle border to face the interior of the puzzle
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
        //add a border around the Side Hint Tile number. Used in the tutorial
        redBorder.gameObject.SetActive(true);
    }

    public void removeRedBorder() {
        //removes the border around the SideHintTile number. Used in the tutorial
        redBorder.gameObject.SetActive(false);
    }
}
