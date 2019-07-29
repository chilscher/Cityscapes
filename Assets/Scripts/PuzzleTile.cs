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


    public GameObject red1Prefab;
    public GameObject red2Prefab;
    public GameObject red3Prefab;
    public GameObject red4Prefab;
    public GameObject red5Prefab;
    public GameObject red6Prefab;
    public GameObject red7Prefab;

    public GameObject green1Prefab;
    public GameObject green2Prefab;
    public GameObject green3Prefab;
    public GameObject green4Prefab;
    public GameObject green5Prefab;
    public GameObject green6Prefab;
    public GameObject green7Prefab;

    public void initialize(int solution, Transform parent, int maxValue, GameManager gameManager) {
        this.solution = solution;
        this.maxValue = maxValue;
        this.gameManager = gameManager;
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
        foreach (GameObject g in redHintObjects) {
            Destroy(g);
        }
        foreach (GameObject g in greenHintObjects) {
            Destroy(g);
        }
        for (int i = 0; i< redHints.Count; i++) {
            int value = redHints[i];

            GameObject relevantPrefab = null;
            switch (value) {
                case 1:
                    relevantPrefab = red1Prefab;
                    break;
                case 2:
                    relevantPrefab = red2Prefab;
                    break;
                case 3:
                    relevantPrefab = red3Prefab;
                    break;
                case 4:
                    relevantPrefab = red4Prefab;
                    break;
                case 5:
                    relevantPrefab = red5Prefab;
                    break;
                case 6:
                    relevantPrefab = red6Prefab;
                    break;
                case 7:
                    relevantPrefab = red7Prefab;
                    break;
            }
            if (relevantPrefab != null) {
                GameObject hint = Instantiate(relevantPrefab);
                hint.transform.position = this.transform.position;
                hint.transform.parent = this.transform;
                //hint.transform.localScale *= 0.4f * transform.localScale.x;
                hint.transform.localScale *= 0.4f;

                Vector3 pos = hint.transform.position;
                pos.z -= 1;
                //float totalTileSize = GetComponent<BoxCollider2D>().size.x * transform.localScale.x;
                float totalTileSize = GetComponent<BoxCollider2D>().size.x;
                pos.x -= (0.3f * totalTileSize);
                pos.x += (0.2f * totalTileSize * i);
                pos.y += (0.25f * totalTileSize);



                hint.transform.position = pos;

                redHintObjects.Add(hint);
            }
        }
        for (int i = 0; i < greenHints.Count; i++) {
            int value = greenHints[i];

            GameObject relevantPrefab = null;
            switch (value) {
                case 1:
                    relevantPrefab = green1Prefab;
                    break;
                case 2:
                    relevantPrefab = green2Prefab;
                    break;
                case 3:
                    relevantPrefab = green3Prefab;
                    break;
                case 4:
                    relevantPrefab = green4Prefab;
                    break;
                case 5:
                    relevantPrefab = green5Prefab;
                    break;
                case 6:
                    relevantPrefab = green6Prefab;
                    break;
                case 7:
                    relevantPrefab = green7Prefab;
                    break;
            }
            if (relevantPrefab != null) {
                GameObject hint = Instantiate(relevantPrefab);
                hint.transform.position = this.transform.position;
                hint.transform.parent = this.transform;
                //hint.transform.localScale *= 0.4f * transform.localScale.x;
                hint.transform.localScale *= 0.4f;

                Vector3 pos = hint.transform.position;
                pos.z -= 1;
                //float totalTileSize = GetComponent<BoxCollider2D>().size.x * transform.localScale.x;
                float totalTileSize = GetComponent<BoxCollider2D>().size.x;
                pos.x += ((0.5f * totalTileSize) - (0.2f * totalTileSize * greenHints.Count));
                pos.x += (0.2f * totalTileSize * i);
                pos.y -= (0.25f * totalTileSize);



                hint.transform.position = pos;

                greenHintObjects.Add(hint);
            }
        }
    }

    private void clearColorHints() {
        redHints = new List<int>();
        greenHints = new List<int>();
        showColoredHints();
    }
}
