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
    public List<int> redHints = new List<int>();
    [HideInInspector]
    public List<int> greenHints = new List<int>();
    private List<GameObject> redHintObjects = new List<GameObject>();
    private List<GameObject> greenHintObjects = new List<GameObject>();

    public Sprite emptyTileSprite;
    public Sprite[] numberSprites;
    public Sprite[] buildingSprites;
    public Sprite[] redSprites;
    public Sprite[] greenSprites;

    private Sprite[] usableBuildingSprites;


    private SpriteRenderer background;
    private SpriteRenderer building;
    private SpriteRenderer number;
    private SpriteRenderer redBorder;

    public void initialize(int solution, Transform parent, int maxValue, GameManager gameManager) {
        this.solution = solution;
        this.maxValue = maxValue;
        this.gameManager = gameManager;
        
        background = transform.GetChild(0).GetComponent<SpriteRenderer>();
        building = transform.GetChild(1).GetComponent<SpriteRenderer>();
        number = transform.GetChild(2).GetComponent<SpriteRenderer>();
        redBorder = transform.GetChild(4).GetComponent<SpriteRenderer>();

        // pick a random tile rotation direction
        int[] directions = new int[] { 0, 90, 180, 270 };
        //System.Random rnd = new System.Random();
        int r = StaticVariables.rand.Next(4);
        //background.transform.Rotate(new Vector3(0,0,directions[r]));
        building.transform.Rotate(new Vector3(0, 0, directions[r]));

        createUsableBuildingSprites();
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
            toggleRedHint(selectedNumber);
            gameManager.addToPuzzleHistory();
        }
        else if (gameManager.clickTileAction == "Toggle Green Hint") {
            int selectedNumber = gameManager.selectedNumber;
            toggleGreenHint(selectedNumber);
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

    public void toggleRedHint(int num) {
        if (num != 0) {
            if (shownNumber == 0) { //you cant add a hint to a tile that already has a number on it

                if (redHints.Contains(num)) {
                    redHints.Remove(num);
                }
                else {
                    redHints.Add(num);
                }
                //for space reasons, there is a limit on the number of hints you can add
                if (redHints.Count > 4) {
                    print("you can only have 4 hints of each color!");
                    redHints.Remove(num);
                }
                showColoredHints();
            }
        }
    }

    public void toggleGreenHint(int num) {
        if (num != 0) {
            if (shownNumber == 0) { //you cant add a hint to a tile that already has a number on it
                if (greenHints.Contains(num)) {
                    greenHints.Remove(num);
                }
                else {
                    greenHints.Add(num);
                }
                if (greenHints.Count > 4) {
                    print("you can only have 4 hints of each color!");
                    greenHints.Remove(num);
                }
                showColoredHints();
            }
        }
    }
    

    private void showColoredHints() {
        redHints.Sort();
        greenHints.Sort();

        Transform hintsList = transform.GetChild(3);

        for (int i = 0; i<hintsList.transform.childCount; i++) {
            SpriteRenderer s = hintsList.GetChild(i).GetComponent<SpriteRenderer>();
            s.enabled = false;
        }

        for (int i = 0; i < redHints.Count; i++) {
            int value = redHints[i];
            SpriteRenderer s = hintsList.GetChild(i).GetComponent<SpriteRenderer>();

            s.sprite = redSprites[value - 1];
            s.enabled = true;
        }

        int startPos = 8 - greenHints.Count;

        for (int i = 0; i < greenHints.Count; i++) {
            int value = greenHints[i];
            SpriteRenderer s = hintsList.GetChild(startPos + i).GetComponent<SpriteRenderer>();
            s.sprite = greenSprites[value - 1];
            s.enabled = true;
        }
    }

    public void clearColorHints() {
        redHints = new List<int>();
        greenHints = new List<int>();
        showColoredHints();
    }


    public void addNumberToTile(int num) {

        if (num == 0) {
            building.enabled = false;
            number.enabled = false;
        }

        if (num != 0) {
            building.sprite = usableBuildingSprites[num - 1];
            number.sprite = numberSprites[num - 1];

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
