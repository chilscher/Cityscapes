using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleTile : Tile {
    
    private int solution;
    private int maxValue;
    private int shownNumber = 0;
    [HideInInspector]
    public bool canClickTile = true;


    public void initialize(Vector2 position, float tileSize, int solution, Transform parent, int maxValue) {
        setValues(position, tileSize, parent);
        this.solution = solution;
        this.maxValue = maxValue;        
    }

    public void clicked() {
        rotateToNextNumber();
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

}
