using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PuzzleTile : Tile {
    
    public int solution;
    private int maxValue;
    [HideInInspector]
    public int shownNumber = 0;
    [HideInInspector]
    public GameManager gameManager;

    [HideInInspector]
    public List<int> noteGroup1 = new List<int>();
    [HideInInspector]
    public List<int> noteGroup2 = new List<int>();

    public Sprite emptyTileSprite;
    public Sprite[] buildingSprites;

    private Sprite[] usableBuildingSprites;


    private SpriteRenderer background;
    private SpriteRenderer road;
    private SpriteRenderer building;
    private SpriteRenderer number;
    private SpriteRenderer redBorder;

    public Sprite[] whiteSprites;
    private Color numberColor;
    private Color note1Color;
    private Color note2Color;

    public void initialize(int solution, Transform parent, int maxValue, GameManager gameManager) {
        this.solution = solution;
        this.maxValue = maxValue;
        this.gameManager = gameManager;

        background = transform.GetChild(0).GetComponent<SpriteRenderer>();
        road = transform.GetChild(1).GetComponent<SpriteRenderer>();
        building = transform.GetChild(2).GetComponent<SpriteRenderer>();
        number = transform.GetChild(3).GetComponent<SpriteRenderer>();
        redBorder = transform.GetChild(4).GetComponent<SpriteRenderer>();

        // pick a random tile rotation direction
        int[] directions = new int[] { 0, 90, 180, 270 };
        int r = StaticVariables.rand.Next(4);
        building.transform.Rotate(new Vector3(0, 0, directions[r]));
        setNumberColors();
        createUsableBuildingSprites();
    }

    public void setNumberColors() {
        ColorUtility.TryParseHtmlString(StaticVariables.whiteHex, out numberColor);
        ColorUtility.TryParseHtmlString(StaticVariables.redHex, out note1Color);
        ColorUtility.TryParseHtmlString(StaticVariables.greenHex, out note2Color);
    }

    public void clicked() {
        /*
        if (gameManager.clickTileAction == "Cycle Through"){
            rotateToNextNumber();
        }
        */
        if(gameManager.clickTileAction == "Apply Selected") {
            int selectedNumber = gameManager.selectedNumber;
            toggleNumber(selectedNumber);
            gameManager.addToPuzzleHistory();
        }
        else if (gameManager.clickTileAction == "Toggle Red Hint") {
            int selectedNumber = gameManager.selectedNumber;
            toggleNote1(selectedNumber);
            gameManager.addToPuzzleHistory();
        }
        else if (gameManager.clickTileAction == "Toggle Green Hint") {
            int selectedNumber = gameManager.selectedNumber;
            toggleNote2(selectedNumber);
            gameManager.addToPuzzleHistory();
        }
        else {
            print(gameManager.clickTileAction + " is not a valid action!");
        }
    }

    private void OnMouseDown() {
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

    public bool checkIfNumIsCorrect() {
        return (shownNumber == solution);
    }

    private void removeNumberFromTile() {
        shownNumber = 0;
        
        building.enabled = false;
        number.enabled = false;
        
    }

    private void rotateToNextNumber() {
        if (shownNumber == maxValue) {
            removeNumberFromTile();
        }
        else {
            shownNumber += 1;
            addNumberToTile(shownNumber);
        }
    }

    public void showSolutionOnTile() {
        shownNumber = solution;
        addNumberToTile(solution);
    }

    public void toggleNumber(int num) {
        if (num == 0) {
            removeNumberFromTile();
        }
        else {
            if (num == shownNumber) {
                removeNumberFromTile();
            }
            else {
                shownNumber = num;
                addNumberToTile(shownNumber);
                clearColorHints();
            }
        }
    }

    public void toggleNote1(int num) {
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
                    print("you can only have 4 hints of each color!");
                    noteGroup1.Remove(num);
                }
                showColoredHints();
            }
        }
    }

    public void toggleNote2(int num) {
        if (num != 0) {
            if (shownNumber == 0) { //you cant add a hint to a tile that already has a number on it
                if (noteGroup2.Contains(num)) {
                    noteGroup2.Remove(num);
                }
                else {
                    noteGroup2.Add(num);
                }
                if (noteGroup2.Count > 4) {
                    print("you can only have 4 hints of each color!");
                    noteGroup2.Remove(num);
                }
                showColoredHints();
            }
        }
    }
    

    private void showColoredHints() {
        noteGroup1.Sort();
        noteGroup2.Sort();

        Transform hintsList = transform.GetChild(5);

        for (int i = 0; i<hintsList.transform.childCount; i++) {
            SpriteRenderer s = hintsList.GetChild(i).GetComponent<SpriteRenderer>();
            s.enabled = false;
        }

        for (int i = 0; i < noteGroup1.Count; i++) {
            int value = noteGroup1[i];
            SpriteRenderer s = hintsList.GetChild(i).GetComponent<SpriteRenderer>();
            s.sprite = whiteSprites[value - 1];
            s.color = note1Color;
            s.enabled = true;
        }

        int startPos = 8 - noteGroup2.Count;

        for (int i = 0; i < noteGroup2.Count; i++) {
            int value = noteGroup2[i];
            SpriteRenderer s = hintsList.GetChild(startPos + i).GetComponent<SpriteRenderer>();
            s.sprite = whiteSprites[value - 1];
            s.color = note2Color;
            s.enabled = true;
        }
    }

    public void clearColorHints() {
        noteGroup1 = new List<int>();
        noteGroup2 = new List<int>();
        showColoredHints();
    }


    public void addNumberToTile(int num) {

        if (num == 0) {
            building.enabled = false;
            number.enabled = false;
        }

        if (num != 0) {
            building.sprite = usableBuildingSprites[num - 1];
            number.sprite = whiteSprites[num - 1];
            number.color = numberColor;

            building.enabled = true;
            number.enabled = true;
        }
    }

    private void createUsableBuildingSprites() {
        //does not allow for cities of size 7!
        usableBuildingSprites = new Sprite[maxValue];
        int[] fillOrder = new int[6] { 1, 3, 6, 4, 5, 2 };
        int[] x = new int[maxValue];
        for (int i = 0; i<maxValue; i++) {
            x[i] = fillOrder[i];
        }
        Array.Sort(x) ;

        for(int i = 0; i<maxValue; i++) {
            int y = x[i];
            Sprite s = buildingSprites[y - 1];
            usableBuildingSprites[i] = s;
        }
    }

    public void addRedBorder() {
        redBorder.gameObject.SetActive(true);
    }

    public void removeRedBorder() {
        redBorder.gameObject.SetActive(false);
    }

}
