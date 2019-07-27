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
            }
        }
    }

    private void toggleRedHint(int num) {
        if (redHints.Contains(num)) {
            redHints.Remove(num);
        }
        else {
            redHints.Add(num);
        }
        //printRedHints();
        //show red hints on screen
    }

    private void toggleGreenHint(int num) {
        if (greenHints.Contains(num)) {
            greenHints.Remove(num);
        }
        else {
            greenHints.Add(num);
        }
        //printGreenHints();
        //show green hints on screen
    }

    private void printRedHints() {
        string s = "Red Hints: ";
        foreach(int i in redHints) {
            s += i + " ";
        }
        print(s);
    }
}
