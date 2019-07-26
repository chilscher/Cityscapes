using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSolver{
    
    public PuzzleSolverSideTile[] topHints;
    public PuzzleSolverSideTile[] bottomHints;
    public PuzzleSolverSideTile[] leftHints;
    public PuzzleSolverSideTile[] rightHints;

    public int size;
    
    public PuzzleSolverTile[,] tilesArray;
    public PuzzleSolverTile[] tilesList;

    public PuzzleSolverSideTile[] hintsList;

    private int[,] rightAnswer;

    public void solvePuzzle(int[] topHints, int[] bottomHints, int[] leftHints, int[] rightHints, int[,] puzzle) {
        setUp(topHints, bottomHints, leftHints, rightHints, puzzle);
        //printHints();

        immediatePopulation();
        immediateProhibition();

        //printCore();

        bool cont = true;
        while (cont){
            int beforeCount = getNumTilesPopulated();
            iterativeProhibition();
            iterativePopulation();
            int afterCount = getNumTilesPopulated();
            if (beforeCount == afterCount) {
                cont = false;
            }
            //printCore();
        }

    }

    public bool isPuzzleValid(int[] topHints, int[] bottomHints, int[] leftHints, int[] rightHints, int[,] puzzle) {
        solvePuzzle(topHints, bottomHints, leftHints, rightHints, puzzle);
        if ((getNumEmptyTiles() <7)) {
        
        //if(!areCompletedTilesRight()) { 
        
            return true;
        }
        return false;
        
        //return true;
    }


    public int[,] getUniqueSolution(int[] topHints, int[] bottomHints, int[] leftHints, int[] rightHints, int[,] puzzle) {
        setUp(topHints, bottomHints, leftHints, rightHints, puzzle);
        immediatePopulation();
        immediateProhibition();
        bool cont = true;
        while (cont) {
            int beforeCount = getNumTilesPopulated();
            iterativeProhibition();
            iterativePopulation();
            int afterCount = getNumTilesPopulated();
            if (beforeCount == afterCount) {
                cont = false;
            }
        }

        int[,] s = new int[size, size];
        for (int i = 0; i<size; i++) {
            for (int j = 0; j<size; j++) {
                s[i, j] = tilesArray[i, j].value;
            }
        }

        return s;



    }

    private void setUp(int[] topHints, int[] bottomHints, int[] leftHints, int[] rightHints, int[,] puzzle) {
        size = topHints.Length;
        this.rightAnswer = puzzle;
        this.topHints = createHintTiles(topHints);
        this.bottomHints = createHintTiles(bottomHints);
        this.leftHints = createHintTiles(leftHints);
        this.rightHints = createHintTiles(rightHints);

        createEmptyPuzzle();
        createHintsList();
        addOppositeHints(this.topHints, this.bottomHints);
        addOppositeHints(this.leftHints, this.rightHints);
        addHintsToTiles();
        addTilesToHints();

    }

    private void createEmptyPuzzle() {
        tilesArray = new PuzzleSolverTile[size, size];
        tilesList = new PuzzleSolverTile[size * size];
        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++) {
                PuzzleSolverTile x = new PuzzleSolverTile();
                tilesArray[i, j] = x;
                tilesList[(i * size) + j] = x;
                x.xValue = j;
                x.yValue = i;
            }
        }
    }

    private PuzzleSolverSideTile[] createHintTiles(int[] values) {
        PuzzleSolverSideTile[] hintTiles = new PuzzleSolverSideTile[size];
        for (int i = 0; i < size; i++) {
            int value = values[i];
            hintTiles[i] = new PuzzleSolverSideTile();
            hintTiles[i].hint = value;
            hintTiles[i].row = new PuzzleSolverTile[size];
        }
        return hintTiles;
    }

    private void addOppositeHints(PuzzleSolverSideTile[] groupA, PuzzleSolverSideTile[] groupB) {
        for (int i = 0; i <size; i++) {
            PuzzleSolverSideTile a = groupA[i];
            PuzzleSolverSideTile b = groupB[i];
            a.oppositeHint = b;
            b.oppositeHint = a;
        }
    }

    private void createHintsList() {
        hintsList = new PuzzleSolverSideTile[size * 4];
        for (int i = 0; i <size; i++) {
            hintsList[i] = topHints[i];
            hintsList[size + i] = bottomHints[i];
            hintsList[size + size + i] = leftHints[i];
            hintsList[size + size + size + i] = rightHints[i];
        }
    }

    private void addHintsToTiles() {
        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++) {
                PuzzleSolverTile tile = tilesArray[i, j];
                tile.topHintTile = topHints[j];
                tile.bottomHintTile = bottomHints[j];
                tile.leftHintTile = leftHints[i];
                tile.rightHintTile = rightHints[i];
            }
        }
    }

    private void addTilesToHints() {
        for (int i = 0; i<size; i++) {
            for (int j = 0; j < size; j++) {
                topHints[j].row[i] = tilesArray[i, j];
                leftHints[i].row[j] = tilesArray[i, j];
            }

        }
        for (int i = 0; i<size; i++) {
            bottomHints[i].row = reverseList(topHints[i].row);
            rightHints[i].row = reverseList(leftHints[i].row);
        }
    }
    
    private void immediatePopulation() {
        foreach (PuzzleSolverSideTile h in hintsList) {
            //if hint is 1, the adjacent tile has to be the highest possible number
            if (h.hint == 1) { h.row[0].populate(size); }
            
            //if hint is 2 and the opposite hint is a 1, the adjacent tile has to be the second highest possible number
            if ((h.hint == 2) && h.oppositeHint.hint == 1) { h.row[0].populate(size - 1); }
            
            //if hint is the puzzle size, fill in all tiles in the row in ascending order
            if (h.hint == size) {
                for (int i = 0; i<size; i++) {
                    h.row[i].populate(i + 1);
                }
            }

            //if hint is between 1 and the max number (exclusive) and if the hint and the opposite hint add to max value + 1, the highest possible number has to be hint-value spaces into the current row
            if ((h.hint > 1) && (h.hint <size) && ((h.hint + h.oppositeHint.hint) == (size + 1))) {
                h.row[h.hint - 1].populate(size);
            }
        }
    }

    private void immediateProhibition() {
        foreach(PuzzleSolverSideTile h in hintsList) {
            //if hint is between 1 and the max number (exclusive), the tiles next to it cannot be high numbers
            if ((h.hint > 1) && (h.hint < size)) {
                int numTiles = h.hint - 1;
                for(int i=0; i<numTiles; i++) {
                    PuzzleSolverTile tile = h.row[i];
                    int prohibitTopX = numTiles - i;
                    for (int j = 0; j<prohibitTopX; j++) {
                        tile.prohibitValue(size - j);
                    }
                }
            }
        }
    }

    private void iterativeProhibition() {
        foreach (PuzzleSolverSideTile h in hintsList) {
            //go through every row, and add all used values as prohibited to other tiles
            List<int> containedValues = new List<int>();
            foreach (PuzzleSolverTile t in h.row) {
                if (t.value != 0) { containedValues.Add(t.value); }
            }
            foreach (PuzzleSolverTile t in h.row) {
                t.prohibitValues(containedValues);
            }
            
            //if hint is between 1 and size (exclusive) and the row is already populated, you can prohibit high values on tiles near the hint
            if ((h.hint > 1) && (h.hint < size) && h.hasATileInRowBeenPopulated() && (!h.row[0].populated)) {
                List<int> unusedNumbers = h.getUnusedNumbers();//assumes unusedNumbers is sorted
                List<int> unusedNumbersHighToLow = reverseList(unusedNumbers);
                int numVisible = h.numBuildingsCurrentlyVisible();
                int numYetToBeVisible = h.hint - numVisible;
                int tilesToProhibit = numYetToBeVisible - 1;
                for (int i = 0; i< tilesToProhibit; i++) {
                    PuzzleSolverTile t = h.row[i];
                    int prohibitHighestX = tilesToProhibit - i;

                    List<int> valuesToProhibit = new List<int>();
                    for (int j = 0; j < prohibitHighestX; j++) {
                        valuesToProhibit.Add(unusedNumbersHighToLow[j]);
                    }
                    t.prohibitValues(valuesToProhibit);
                }
            }
            //if hint is 2, the highest remaining number in the row can't be between the adjacent tile and the highest value
            if ((h.hint == 2) && (!h.row[0].populated) && (h.isHighestValueInRow())) {
                List<int> u = h.getUnusedNumbers();
                int highestUnusedVal = u[u.Count - 1];
                bool foundMax = false;
                foreach (PuzzleSolverTile t in h.row) {
                    if (t.value == size) {
                        foundMax = true;
                    }
                    if ((!foundMax) && (t != h.row[0])) {
                        t.prohibitValue(highestUnusedVal);
                    }
                }
            }

        }

        foreach (PuzzleSolverTile t in tilesList) {
            //if a tile is populated, add all other numbers to its prohibited list
            if (t.populated) {
                for (int i = 1; i <= size; i++) {
                    t.prohibitValue(i);
                    //t.prohibitedValues.Add(i);
                }
            }
        }
    }

    private void iterativePopulation() {
        //if size - 1 values are prohibited on a tile, then the tile can be populated with the last unprohibited value
        foreach(PuzzleSolverTile t in tilesList) {
            if (t.prohibitedValues.Count == size - 1) {
                t.populateLastValue();
            }
        }

        foreach (PuzzleSolverSideTile h in hintsList) {
            //if hint is between 1 and size (exclusive) and it is next to the only contiguous space in the row, you might be able to populate the row in ascending or descending order
            if ((h.hint > 1) && (h.hint < size) && h.isHintNextToContiguousUnpopulatedArea() && (h.isHighestValueInRow()) && (!h.isAnyUnusedNumHigherThanLowestVisible())) {
                //print(h.hint);
                //print(h.row[0].xValue + ", " + h.row[0].yValue);
                List<int> unusedNumbers = h.getUnusedNumbers();//assumes unusedNumbers is sorted
                int numVisible = h.numBuildingsCurrentlyVisible();
                int lowestVisible = h.lowestBuildingCurrentlyVisible();
                int numVisibleToFill = h.hint - numVisible;
                if (numVisibleToFill == 1) {
                    int highestAllowedNum = 0;
                    foreach (int n in unusedNumbers) {
                        if ((n > highestAllowedNum) && n < lowestVisible) {
                            highestAllowedNum = n;
                        }
                    }
                    if (highestAllowedNum < h.getHighestNumInRow()) {
                        h.row[0].populate(highestAllowedNum);
                    }
                }
                else if (numVisibleToFill == unusedNumbers.Count) {
                    for (int i = 0; i<unusedNumbers.Count; i++) {
                        h.row[i].populate(unusedNumbers[i]);
                    }
                }
            }
            //if row has size - 1 occurrences of the same value in its prohibited tiles, then the last unprohibited tile has to contain the value
            int[] valueCount = new int[size + 1];
            foreach (PuzzleSolverTile t in h.row) {
                foreach (int p in t.prohibitedValues) {
                    valueCount[p]++;
                }
            }
            for (int i = 1; i < valueCount.Length; i++) {
                int value = i;
                int count = valueCount[value];
                if (count == size - 1) {
                    foreach (PuzzleSolverTile t in h.row) {
                        if (!t.prohibitedValues.Contains(value)) {
                            t.populate(value);
                        }
                    }
                }
            
            }

        }
    }

    private void printCore() {
        string output = "";

        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++) {
                output += tilesArray[i, j].value;
            }
            output += "-";
        }
        //cut off last letter
        output = output.Substring(0, (output.Length - 1));
        Debug.Log(output);
    }

    private void print(string s) {
        Debug.Log(s);
    }

    private void print(int i) {
        Debug.Log(i + "");
    }

    private PuzzleSolverTile[] reverseList(PuzzleSolverTile[] original) {
        PuzzleSolverTile[] newList = new PuzzleSolverTile[size];
        for (int i = 0; i < size; i++) {
            int oppositeIndex = size - i;
            newList[i] = original[oppositeIndex - 1];
        }
        return newList;
    }

    private int getNumTilesPopulated() {
        int count = 0;
        foreach (PuzzleSolverTile t in tilesList) {
            if (t.populated) {
                count++;
            }
        }
        return count;
    }

    private int[] reverseList(int[] original) {
        int[] newList = new int[size];
        for (int i = 0; i < original.Length; i++) {
            int oppositeIndex = original.Length - i;
            newList[i] = original[oppositeIndex - 1];
        }
        return newList;
    }

    private List<int> reverseList(List<int> original) {
        List<int> newList = new List<int>();
        for (int i = 0; i < original.Count; i++) {
            int oppositeIndex = original.Count - i;
            newList.Add(original[oppositeIndex - 1]);
        }
        return newList;
    }

    private void printProhibitedValues() {
        foreach (PuzzleSolverTile t in tilesList) {
            string o = "coordinates: " + t.xValue + ", " + t.yValue + " ---  prohibited values: ";
            foreach (int p in t.prohibitedValues) {
                o += (p + " ");
            }
            print(o);
        }
    }

    private int getNumEmptyTiles() {
        int count = 0;
        foreach (PuzzleSolverTile t in tilesList) {
            if (!t.populated) {
                count++;
            }
        }
        return count;
    }

    private bool areCompletedTilesRight() {
        bool isRight = true;
        for (int i = 0; i<size; i++) {
            for (int j = 0; j<size; j++) {
                if ((tilesArray[i,j].value != rightAnswer[i, j]) && !(tilesArray[i,j].value == 0)){
                    isRight = false;
                }
            }
        }
        return isRight;
    }
}
