//for Cityscapes, copyright Cole Hilscher 2020

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PuzzleTile : Tile {
    //a Puzzle Tile object which holds the notes and building for one specific square of the Cityscapes Puzzle
    //the PuzzleTile script is attached to a PuzzleTilePrefab object. The PuzzleGenerator makes an instance of the PuzzleTilePrefab and provides it with data
    public int solution; //the correct answer for this tile. However, it is important to note that the puzzle may be solved with a different number on this tile, and still be a valid solution
    private int maxValue; //the highest value this tile can hold, specifically being the size of the puzzle
    [HideInInspector]
    public int shownNumber = 0; //the number that is displayed on the tile to the player
    [HideInInspector]
    public GameManager gameManager;

    [HideInInspector]
    public List<int> noteGroup1 = new List<int>(); //all of the note1s on this tile
    [HideInInspector]
    public List<int> noteGroup2 = new List<int>(); //all of the note2s on this tile

    public Sprite emptyTileSprite;

    //the different visual aspects that make up the PuzzleTile
    private Image road;
    private Image building;
    private Image number;
    private Image redBorder;

    public Sprite[] whiteSprites; //the numbers in a basic white/black color scheme, so the image can apply its own color.
    private Color numberColor;
    private Color note1Color;
    private Color note2Color;
    private Color highlightBuildingColor;

    private bool randomRotation = false; //if this is true, each tile will be generated with a random rotation (90, 180, 270, 0 deg) rotation applied to it for visual variety
    private bool scaleBuildingSizes = true; //if this is true, building sizes are scaled dynamically based on the number placed in the tile. Then the skin only needs to apply one building design
    private float minBuildingScale = 0.5f; //the smallest size that a building will be relative to the highest size. So if this is 0.5, then a #1 building will be half the dimensions of a #3 building on a small city (where the max building size is 3)
   
    public bool isPermanentBuilding = false; //if true, the tile's building size cannot be overwritten or removed.

    //stuff used to denote permanent buildings
    public Color permanentBuildingColor;
    public GameObject permanentDarkenedBackground;
    public Color permanentNumberColor;
    public void initialize(int solution, Transform parent, int maxValue, GameManager gameManager) {
        //create the PuzzleTile object, called by PuzzleGenerator
        this.solution = solution;
        this.maxValue = maxValue;
        this.gameManager = gameManager;

        road = transform.GetChild(2).GetComponent<Image>();
        building = transform.GetChild(3).GetComponent<Image>();
        number = transform.GetChild(4).GetComponent<Image>();
        redBorder = transform.GetChild(5).GetComponent<Image>();

        // pick a random tile rotation direction
        if (randomRotation) {
            int[] directions = new int[] { 0, 90, 180, 270 };
            int r = StaticVariables.rand.Next(4);
            building.transform.Rotate(new Vector3(0, 0, directions[r]));
        }
        //set colors and sprites from the current skin
        setNumberColors();
        building.sprite = gameManager.skin.buildingSprite;
        road.color = InterfaceFunctions.getColorFromString(gameManager.skin.streetColor);
    }

    public void setNumberColors() {
        //sets some color variables based on the current skin
        ColorUtility.TryParseHtmlString(StaticVariables.whiteHex, out numberColor);
        ColorUtility.TryParseHtmlString(gameManager.skin.note1Color, out note1Color);
        ColorUtility.TryParseHtmlString(gameManager.skin.note2Color, out note2Color);
        ColorUtility.TryParseHtmlString(gameManager.skin.highlightBuildingColor, out highlightBuildingColor);
    }

    public void clicked() {
        //when the PuzzleTile is clicked, do whatever the GameManger.clickTileAction says to do
        if (isPermanentBuilding){
            //do nothing, cannot edit starting tiles
        }
        else if(gameManager.clickTileAction == "Apply Selected") {
            int selectedNumber = gameManager.selectedNumber;
            toggleNumber(selectedNumber);
            gameManager.addToPuzzleHistory();
        }
        else if (gameManager.clickTileAction == "Toggle Note 1") {
            int selectedNumber = gameManager.selectedNumber;
            toggleNote1(selectedNumber);
            gameManager.addToPuzzleHistory();
        }
        else if (gameManager.clickTileAction == "Toggle Note 2") {
            int selectedNumber = gameManager.selectedNumber;
            toggleNote2(selectedNumber);
            gameManager.addToPuzzleHistory();
        }
        else if(gameManager.clickTileAction == "Clear Tile") {
            if (doesTileContainAnything()) {
                clearColoredNotes();
                removeNumberFromTile();
                gameManager.addToPuzzleHistory();
            }
        }
        else {
            print(gameManager.clickTileAction + " is not a valid action!");
        }
    }

    private void OnMouseDown() {
        //when the player clicks this tile, process the click, unless this is part of the tutorial, in which case process the click selectively
        if (gameManager.canClick) {
            if (!StaticVariables.isTutorial) {
                clicked();
            }
            else {
                if (gameManager.tutorialManager.canPlayerClickTile(this)){
                    clicked();
                    gameManager.tutorialManager.clickedTile(this);
                }
            }
        }
        
    }

    public void removeNumberFromTile() {
        //removes the building number from the PuzzleTile
        if (scaleBuildingSizes && shownNumber != 0) {
            float scale1 = ((float)shownNumber - 1) / (maxValue - 1);
            float scale2 = scale1 * minBuildingScale;
            float scale3 = scale2 + minBuildingScale;
            building.transform.localScale /= (scale3);
        }
        shownNumber = 0;
        building.enabled = false;
        number.enabled = false;
    }

    public void toggleNumber(int num) {
        //apply the selected number to the tile
        //if the number is already on this tile, then remove it
        //if there are any notes on the tile, clear them
        if (num == 0) {
            removeNumberFromTile();
        }
        else {
            if (num == shownNumber) {
                removeNumberFromTile();
            }
            else {
                removeNumberFromTile();
                shownNumber = num;
                addNumberToTile(shownNumber);
                clearColoredNotes();
            }
        }
    }

    public void toggleNote1(int num) {
        //add/remove a note1 for the provided number to this tile
        //there is a limit of 4 of each type of note per tile
        if (num != 0) {
            if (shownNumber == 0) { //you cant add a hint to a tile that already has a number on it

                if (noteGroup1.Contains(num)) {
                    noteGroup1.Remove(num);
                }
                else {
                    noteGroup1.Add(num);
                }
                //for space reasons, there is a limit on the number of hints you can add
                if (noteGroup1.Count > 4) {
                    //print("you can only have 4 hints of each color!");
                    noteGroup1.Remove(num);
                }
                showColoredNotes();
            }
        }
    }

    public void toggleNote2(int num) {
        //add/remove a note2 for the provided number to this tile
        //there is a limit of 4 of each type of note per tile
        if (num != 0) {
            if (shownNumber == 0) { //you cant add a hint to a tile that already has a number on it
                if (noteGroup2.Contains(num)) {
                    noteGroup2.Remove(num);
                }
                else {
                    noteGroup2.Add(num);
                }
                if (noteGroup2.Count > 4) {
                    //print("you can only have 4 hints of each color!");
                    noteGroup2.Remove(num);
                }
                showColoredNotes();
            }
        }
    }
    

    private void showColoredNotes() {
        //shows the colored notes, in the appropriate position, of the appropriate color, on this tile
        //note1s start at the top-left of the tile and fill in to the right, to a max of 4 note1s
        //note2s start at the bottom-right of the tile and fill in to the left, to a max of 4 note2s
        noteGroup1.Sort();
        noteGroup2.Sort();

        Transform hintsList = transform.GetChild(5);

        for (int i = 0; i<hintsList.transform.childCount; i++) {
            Image s = hintsList.GetChild(i).GetComponent<Image>();
            s.enabled = false;
        }

        for (int i = 0; i < noteGroup1.Count; i++) {
            int value = noteGroup1[i];
            Image s = hintsList.GetChild(i).GetComponent<Image>();
            s.sprite = whiteSprites[value - 1];
            s.color = note1Color;
            s.enabled = true;
        }

        int startPos = 8 - noteGroup2.Count;

        for (int i = 0; i < noteGroup2.Count; i++) {
            int value = noteGroup2[i];
            Image s = hintsList.GetChild(startPos + i).GetComponent<Image>();
            s.sprite = whiteSprites[value - 1];
            s.color = note2Color;
            s.enabled = true;
        }
    }

    public void clearColoredNotes() {
        //remove all notes from this tile
        noteGroup1 = new List<int>();
        noteGroup2 = new List<int>();
        showColoredNotes();
    }

    public void addPermanentBuildingToTile(int num){
        shownNumber = num;
        addNumberToTile(num);
        isPermanentBuilding = true;
        building.color = permanentBuildingColor;
        permanentDarkenedBackground.SetActive(true);
    }

    public void addNumberToTile(int num) {
        //sets the number on this tile to be num. Also displays the building and sets it to the appropriate scale based on the puzzle size
        if (num == 0) {
            building.enabled = false;
            number.enabled = false;
        }

        if (num != 0) {
            if (scaleBuildingSizes) {
                building.transform.localScale = new Vector3(1,1,1); //reset the building size, so when it is redrawn the building size does not exponentially diminish
                float scale1 = ((float)num - 1) / (maxValue - 1);
                float scale2 = scale1 * minBuildingScale;
                float scale3 = scale2 + minBuildingScale;
                building.transform.localScale *= (scale3);
            }
            number.sprite = whiteSprites[num - 1];
            number.color = numberColor;

            building.enabled = true;
            number.enabled = true;

            highlightIfBuildingNumber(gameManager.selectedNumber);
        }
    }

    public void addRedBorder() {
        //add a red border around this tile, used in the tutorial
        redBorder.gameObject.SetActive(true);
    }

    public void removeRedBorder() {
        //removes the red border from this tile, used in the tutorial
        redBorder.gameObject.SetActive(false);
    }

    public bool doesTileContainColoredNote(int colorNum, int noteNum) {
        //returns true if this tile contains a specific note type of a provided color
        //for example, one might ask if this tile contains a RED 3, and this function will provide a boolean answer
        List<int> noteGroup = noteGroup1;
        if (colorNum == 2) {
            noteGroup = noteGroup2;
        }
        foreach (int n in noteGroup) {
            if (noteNum == n) {
                return true;
            }
        }
        return false;
    }

    public bool doesTileContainAnything() {
        //returns false if the tile is completely empty
        bool doesIt = false;
        if (shownNumber != 0) { doesIt = true; }
        if (noteGroup1.Count > 0) { doesIt = true; }
        if (noteGroup2.Count > 0) { doesIt = true; }
        return doesIt;
    }

    public void highlightIfBuildingNumber(int num) {
        //changes the color of the building on this tile to be the highlight color, if that is equal to the provided num
        //buildings do not change color during the tutorial
        if (!StaticVariables.isTutorial && StaticVariables.includeHighlightBuildings) {
            if (shownNumber == num) {
                number.color = highlightBuildingColor;
            }
            else {
                number.color = numberColor;
                if (isPermanentBuilding)
                    number.color = permanentNumberColor;
            }
        }
    }

    public void unhighlightBuildingNumber() {
        //un-highlights the building color on this tile
        number.color = numberColor;
        if (isPermanentBuilding)
            number.color = permanentNumberColor;
    }

}
