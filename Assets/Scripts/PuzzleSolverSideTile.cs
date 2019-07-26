using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSolverSideTile {

    public int hint;
    public PuzzleSolverSideTile oppositeHint;
    public PuzzleSolverTile[] row; //the row or column that the hint tile is looking at. has length of size
    //public PuzzleSolverTile adjacentTile;

    public bool hasATileInRowBeenPopulated() {
        foreach (PuzzleSolverTile t in row) {
            if (t.populated) {
                //print("good");
                return true;
            }
        }
        return false;
    }

    public bool isAdjacentTileEmpty() {
        return (!row[0].populated);
    }

    public bool isHintNextToContiguousUnpopulatedArea() {
        //returns true if the only open spaces in the row are in a single chunk next to the hint
        // _ _ # _   would be false      _ _ # #     would be true      _ _ _ # would be true
        if (isAdjacentTileEmpty()) {
            bool hasNumBeenFound = false;
            foreach (PuzzleSolverTile t in row) {
                if (t.populated) {
                    hasNumBeenFound = true;
                }
                else {
                    if (hasNumBeenFound) {
                        return false;
                    }
                }
            }
            return true;
        }
        return false;
    }

    public List<int> getUnusedNumbers() {
        int size = row.Length;
        List<int> unusedNumbers = new List<int>();
        for (int i = 0; i< size; i++) {
            unusedNumbers.Add(i + 1);
        }

        foreach (PuzzleSolverTile t in row) {
            if (t.populated) {
                unusedNumbers.Remove(t.value);
            }
        }
        return unusedNumbers;
    }

    public int numBuildingsCurrentlyVisible() {
        int count = 0;
        int highest = 0;
        foreach (PuzzleSolverTile t in row) {
            if (t.populated) {
                if(t.value > highest) {
                    highest = t.value;
                    count++;
                }
            }
        }
        return count;
    }

    public int getHighestNumInRow() {
        int highest = 0;
        foreach (PuzzleSolverTile t in row) {
            if (t.value > highest) {
                highest = t.value;
            }
        }
        return highest;
    }

    public bool isHighestValueInRow() {
        int highestVal = row.Length;
        bool c = false;
        foreach (PuzzleSolverTile t in row) {
            if (t.value == highestVal) {
                c = true;
            }
        }
        return c;
    }

    public int lowestBuildingCurrentlyVisible() {
        foreach (PuzzleSolverTile t in row) {
            if (t.populated) {
                return t.value;
            }
        }
        return 0;
    }

    public bool isAnyUnusedNumHigherThanLowestVisible() {
        int b = lowestBuildingCurrentlyVisible();
        List<int> h = getUnusedNumbers();
        foreach( int i in h) {
            if (i > b) {
                return true;
            }
        }
        return false;
    }

}
