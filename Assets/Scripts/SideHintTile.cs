using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideHintTile : Tile {
    
    private int hintValue;
    public PuzzleTile[] row;

    public void initialize(Vector2 position, float tileSize, int hintValue, Transform parent) {
        setValues(position, tileSize, parent);
        this.hintValue = hintValue;
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
        
        foreach(PuzzleTile t in row) {
            if (t.shownNumber == 0) {
                return false;
            }
        }
        return (numBuildingsCurrentlyVisible() == hintValue);
    }
}
