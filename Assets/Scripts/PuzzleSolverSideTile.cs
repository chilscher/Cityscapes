//for Cityscapes, copyright Cole Hilscher 2024

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSolverSideTile {
    //an object used by PuzzleSolver to store the hint for a specific row in the puzzle
    //also contains pointers to all of the PuzzleSolverTiles in that row, and the PuzzleSolverSideTile on the opposite end of that row
    //all of these functions are used by PuzzleSolver to determine what numbers can be filled-in or crossed-out in the puzzle itself
    public int hint;
    public PuzzleSolverSideTile oppositeHint;
    public PuzzleSolverTile[] row; //the row or column that the hint tile is looking at. has length of size

    public bool HasATileInRowBeenPopulated() {
        //returns false if the entire row is empty
        //specifically meaning no tile in the row has an absolutely-correct determined value
        foreach (PuzzleSolverTile t in row) {
            if (t.populated)
                return true;
        }
        return false;
    }

    public bool IsAdjacentTileEmpty() {
        //returns true if the first PuzzleSolverTile in the row does not yet have a solution value
        return !row[0].populated;
    }

    public bool IsHintNextToContiguousUnpopulatedArea() {
        //returns true if the only open spaces in the row are in a single chunk next to the hint
        // _ _ # _   would be false      _ _ # #     would be true      _ _ _ # would be true
        if (IsAdjacentTileEmpty()) {
            bool hasNumBeenFound = false;
            foreach (PuzzleSolverTile t in row) {
                if (t.populated)
                    hasNumBeenFound = true;
                else {
                    if (hasNumBeenFound)
                        return false;
                }
            }
            return true;
        }
        return false;
    }

    public List<int> GetUnusedNumbers() {
        //generates a list of numbers not yet used by PuzzleSolverTiles in this row
        int size = row.Length;
        List<int> unusedNumbers = new List<int>();
        for (int i = 0; i< size; i++)
            unusedNumbers.Add(i + 1);
        foreach (PuzzleSolverTile t in row) {
            if (t.populated)
                unusedNumbers.Remove(t.value);
        }
        return unusedNumbers;
    }

    public int NumBuildingsCurrentlyVisible() {
        //gets the number of buildings currently visible from the perspective of the PuzzleSovlerSideTile
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

    public int GetHighestNumInRow() {
        //returns the highest number in the row's PuzzleSolverTiles
        int highest = 0;
        foreach (PuzzleSolverTile t in row) {
            if (t.value > highest)
                highest = t.value;
        }
        return highest;
    }

    public bool IsHighestValueInRow() {
        //returns true if the row contains the highest number possible
        //ex: if the puzzle size is 5, returns true if the row contains a 5-story building in it somewhere
        int highestVal = row.Length;
        bool c = false;
        foreach (PuzzleSolverTile t in row) {
            if (t.value == highestVal)
                c = true;
        }
        return c;
    }

    public int LowestBuildingCurrentlyVisible() {
        //finds the first populated building in the row - that one has to be the lowest visible, per the rules of the game
        foreach (PuzzleSolverTile t in row) {
            if (t.populated)
                return t.value;
        }
        return 0;
    }

    public bool IsAnyUnusedNumHigherThanLowestVisible() {
        //returns true if there is an unused number in the row that is higher than the lowest visible number
        int b = LowestBuildingCurrentlyVisible();
        List<int> h = GetUnusedNumbers();
        foreach( int i in h) {
            if (i > b)
                return true;
        }
        return false;
    }
}
