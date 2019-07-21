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
            else {
                //printCore();
            }
        }

        //print("iterations have ended.");
        
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
            List<int> containedValues = new List<int>();
            foreach (PuzzleSolverTile t in h.row) {
                if (t.value != 0) { containedValues.Add(t.value); }
            }
            foreach (PuzzleSolverTile t in h.row) {
                t.prohibitValues(containedValues);
            }
            /*
            if ((h.hint > 1) && (h.hint < size)) {

            }
            */
        }
    }

    private void iterativePopulation() {
        foreach(PuzzleSolverTile t in tilesList) {
            if (t.prohibitedValues.Count == size - 1) {
                t.populateLastValue();
            }
        }

        foreach (PuzzleSolverSideTile h in hintsList) {
            if ((h.hint > 1) && (h.hint < size) && h.hasATileInRowBeenPopulated() && h.isHintNextToContiguousUnpopulatedArea()) {
                //print("found a tile");
                List<int> unusedNumbers = h.getUnusedNumbers();
                int numVisible = h.numBuildingsCurrentlyVisible();
                int numVisibleToFill = h.hint - numVisible;
                if (numVisibleToFill == 1) {
                    int highestUnusedNum = 0;
                    foreach (int n in unusedNumbers) {
                        if (n > highestUnusedNum) {
                            highestUnusedNum = n;
                        }
                    }
                    if (highestUnusedNum < h.getHighestNumInRow()) {
                        h.row[0].populate(highestUnusedNum);
                    }
                }
                else if (numVisibleToFill == unusedNumbers.Count) {
                    //assumes unusedNumbers is sorted
                    for (int i = 0; i<unusedNumbers.Count; i++) {
                        h.row[i].populate(unusedNumbers[i]);
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


}
