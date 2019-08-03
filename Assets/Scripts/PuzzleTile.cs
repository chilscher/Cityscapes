using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleTile : Tile {
    
    private int solution;
    private int maxValue;
    [HideInInspector]
    public int shownNumber = 0;
    [HideInInspector]
    public bool canClickTile = true;
    [HideInInspector]
    public GameManager gameManager;

    [HideInInspector]
    public List<int> redHints = new List<int>();
    [HideInInspector]
    public List<int> greenHints = new List<int>();
    private List<GameObject> redHintObjects = new List<GameObject>();
    private List<GameObject> greenHintObjects = new List<GameObject>();

    public Sprite emptyTileSprite;
    public Sprite fullTileSprite;

    public Sprite building1Sprite;
    public Sprite building2Sprite;
    public Sprite building3Sprite;
    public Sprite building4Sprite;
    public Sprite building5Sprite;
    public Sprite building6Sprite;
    public Sprite building7Sprite;

    public Sprite number1Sprite;
    public Sprite number2Sprite;
    public Sprite number3Sprite;
    public Sprite number4Sprite;
    public Sprite number5Sprite;
    public Sprite number6Sprite;
    public Sprite number7Sprite;
    /*
    public Sprite blue1Sprite;
    public Sprite blue2Sprite;
    public Sprite blue3Sprite;
    public Sprite blue4Sprite;
    public Sprite blue5Sprite;
    public Sprite blue6Sprite;
    public Sprite blue7Sprite;
    */

    public Sprite red1Sprite;
    public Sprite red2Sprite;
    public Sprite red3Sprite;
    public Sprite red4Sprite;
    public Sprite red5Sprite;
    public Sprite red6Sprite;
    public Sprite red7Sprite;

    public Sprite green1Sprite;
    public Sprite green2Sprite;
    public Sprite green3Sprite;
    public Sprite green4Sprite;
    public Sprite green5Sprite;
    public Sprite green6Sprite;
    public Sprite green7Sprite;


    private SpriteRenderer background;
    private SpriteRenderer building;
    private SpriteRenderer number;

    public void initialize(int solution, Transform parent, int maxValue, GameManager gameManager) {
        this.solution = solution;
        this.maxValue = maxValue;
        this.gameManager = gameManager;
        
        background = transform.GetChild(0).GetComponent<SpriteRenderer>();
        building = transform.GetChild(1).GetComponent<SpriteRenderer>();
        number = transform.GetChild(2).GetComponent<SpriteRenderer>();

        // pick a random tile rotation direction
        int[] directions = new int[] { 0, 90, 180, 270 };
        //System.Random rnd = new System.Random();
        int r = StaticVariables.rand.Next(4);
        background.transform.Rotate(new Vector3(0,0,directions[r]));
        building.transform.Rotate(new Vector3(0, 0, directions[r]));
        
    }

    public void clicked() {
        if (gameManager.clickTileAction == "Cycle Through"){
            rotateToNextNumber();
        }
        else if(gameManager.clickTileAction == "Apply Selected") {
            int selectedNumber = gameManager.selectedNumber;
            toggleNumber(selectedNumber);
        }
        else if (gameManager.clickTileAction == "Toggle Red Hint") {
            int selectedNumber = gameManager.selectedNumber;
            toggleRedHint(selectedNumber);
        }
        else if (gameManager.clickTileAction == "Toggle Green Hint") {
            int selectedNumber = gameManager.selectedNumber;
            toggleGreenHint(selectedNumber);
        }
        else {
            print(gameManager.clickTileAction + " is not a valid action!");
        }
    }

    private void OnMouseDown() {
        if (canClickTile) {
            clicked();
        }
        
    }

    public bool checkIfNumIsCorrect() {
        return (shownNumber == solution);
    }

    private void removeNumberFromTile() {
        if (shownNumberObject != null) {
            Destroy(shownNumberObject);
        }
        shownNumberObject = null;
        shownNumber = 0;
        
        background.sprite = emptyTileSprite;
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

    private void toggleNumber(int num) {
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

    private void toggleRedHint(int num) {
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

    private void toggleGreenHint(int num) {
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

            switch (value) {
                case 1:
                    s.sprite = red1Sprite;
                    s.enabled = true;
                    break;
                case 2:
                    s.sprite = red2Sprite;
                    s.enabled = true;
                    break;
                case 3:
                    s.sprite = red3Sprite;
                    s.enabled = true;
                    break;
                case 4:
                    s.sprite = red4Sprite;
                    s.enabled = true;
                    break;
                case 5:
                    s.sprite = red5Sprite;
                    s.enabled = true;
                    break;
                case 6:
                    s.sprite = red6Sprite;
                    s.enabled = true;
                    break;
                case 7:
                    s.sprite = red7Sprite;
                    s.enabled = true;
                    break;
            }
        }

        int startPos = 8 - greenHints.Count;

        for (int i = 0; i < greenHints.Count; i++) {
            int value = greenHints[i];
            SpriteRenderer s = hintsList.GetChild(startPos + i).GetComponent<SpriteRenderer>();
            s.enabled = true;
            switch (value) {
                case 1:
                    s.sprite = green1Sprite;
                    break;
                case 2:
                    s.sprite = green2Sprite;
                    break;
                case 3:
                    s.sprite = green3Sprite;
                    break;
                case 4:
                    s.sprite = green4Sprite;
                    break;
                case 5:
                    s.sprite = green5Sprite;
                    break;
                case 6:
                    s.sprite = green6Sprite;
                    break;
                case 7:
                    s.sprite = green7Sprite;
                    break;
            }
        }
    }

    private void clearColorHints() {
        redHints = new List<int>();
        greenHints = new List<int>();
        showColoredHints();
    }


    public new void addNumberToTile(int num) {
        if (shownNumberObject != null) {
            Destroy(shownNumberObject);
        }


        switch (num) {
            case 1:
                building.sprite = building1Sprite;
                number.sprite = number1Sprite;
                //number.sprite = blue1Sprite;
                break;
            case 2:
                building.sprite = building2Sprite;
                number.sprite = number2Sprite;
                //number.sprite = blue2Sprite;
                break;
            case 3:
                building.sprite = building3Sprite;
                number.sprite = number3Sprite;
                //number.sprite = blue3Sprite;
                break;
            case 4:
                building.sprite = building4Sprite;
                number.sprite = number4Sprite;
                //number.sprite = blue4Sprite;
                break;
            case 5:
                building.sprite = building5Sprite;
                number.sprite = number5Sprite;
                //number.sprite = blue5Sprite;
                break;
            case 6:
                building.sprite = building6Sprite;
                number.sprite = number6Sprite;
                //number.sprite = blue6Sprite;
                break;
            case 7:
                building.sprite = building7Sprite;
                number.sprite = number7Sprite;
                //number.sprite = blue7Sprite;
                break;
        }

        if (num == 0) {
            building.enabled = false;
            number.enabled = false;
        }
        else {
            background.sprite = fullTileSprite;

        }

        if (num != 0) {
            background.sprite = fullTileSprite;
            building.enabled = true;
            number.enabled = true;
        }
    }
    /*
    public void highlightNumberIfMatch(int num) {
        if (num == shownNumber) {
            switch (shownNumber) {
                case 1:
                    number.sprite = blue1Sprite;
                    break;
                case 2:
                    number.sprite = blue2Sprite;
                    break;
                case 3:
                    number.sprite = blue3Sprite;
                    break;
                case 4:
                    number.sprite = blue4Sprite;
                    break;
                case 5:
                    number.sprite = blue5Sprite;
                    break;
                case 6:
                    number.sprite = blue6Sprite;
                    break;
                case 7:
                    number.sprite = blue7Sprite;
                    break;
            }
        }
        else {

            switch (shownNumber) {
                case 1:
                    number.sprite = number1Sprite;
                    break;
                case 2:
                    number.sprite = number2Sprite;
                    break;
                case 3:
                    number.sprite = number3Sprite;
                    break;
                case 4:
                    number.sprite = number4Sprite;
                    break;
                case 5:
                    number.sprite = number5Sprite;
                    break;
                case 6:
                    number.sprite = number6Sprite;
                    break;
                case 7:
                    number.sprite = number7Sprite;
                    break;
            }
        }
    }
    */



}
