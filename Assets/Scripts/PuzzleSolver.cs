﻿//for Cityscapes, copyright Cole Hilscher 2020

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSolver{
    //an object created to solve a puzzle based on the side hint numbers
    //uses objects unique to solving the puzzle, PuzzleSolverTile and PuzzleSolverSideTile
    //these objects are not interacted with by the player nor used outside of the puzzle generation system

    //side hint numbers are stored here
    public PuzzleSolverSideTile[] topHints;
    public PuzzleSolverSideTile[] bottomHints;
    public PuzzleSolverSideTile[] leftHints;
    public PuzzleSolverSideTile[] rightHints;
    public PuzzleSolverSideTile[] hintsList;

    //tile numbers are stored here
    public PuzzleSolverTile[,] tilesArray;
    public PuzzleSolverTile[] tilesList;
    
    public int size; //the puzzle size
    
    // ---------------------------------------------------
    //FUNCTIONS THAT SET UP THE SOLVING SYSTEM, INCLUDING CREATING AND ORGANIZING OBJECTS
    // ---------------------------------------------------

    private void setUp(int[] topHints, int[] bottomHints, int[] leftHints, int[] rightHints, int[,] startingSetup) {
        //creates all of the PuzzleSolverSideTile and PuzzleSolverTile objects necessary to iteratively solve the puzzle
        size = topHints.Length;
        this.topHints = createHintTiles(topHints);
        this.bottomHints = createHintTiles(bottomHints);
        this.leftHints = createHintTiles(leftHints);
        this.rightHints = createHintTiles(rightHints);

        createEmptyPuzzle();
        fillEmptyPuzzle(startingSetup);
        createHintsList();
        addOppositeHints(this.topHints, this.bottomHints);
        addOppositeHints(this.leftHints, this.rightHints);
        addHintsToTiles();
        addTilesToHints();

    }

    private void createEmptyPuzzle() {
        //creates an empty puzzle of PuzzleSolverTile objects
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

    private void fillEmptyPuzzle(int[,] startingSetup){
        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++) {
                if (startingSetup[i,j] != 0)
                    tilesArray[i,j].populate(startingSetup[i,j]);
            }
        }
    }

    private PuzzleSolverSideTile[] createHintTiles(int[] values) {
        //creates the set of PuzzleSolverSideTiles, and populates them with the provided values
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
        //each PuzzleSolverSideTile is given the tile on the opposite side of its street to it
        //the opposite hint for the leftmost tile in the top row is the leftmost tile in the bottom row
        for (int i = 0; i < size; i++) {
            PuzzleSolverSideTile a = groupA[i];
            PuzzleSolverSideTile b = groupB[i];
            a.oppositeHint = b;
            b.oppositeHint = a;
        }
    }

    private void createHintsList() {
        //put all of the hints into a list
        hintsList = new PuzzleSolverSideTile[size * 4];
        for (int i = 0; i < size; i++) {
            hintsList[i] = topHints[i];
            hintsList[size + i] = bottomHints[i];
            hintsList[size + size + i] = leftHints[i];
            hintsList[size + size + size + i] = rightHints[i];
        }
    }

    private void addHintsToTiles() {
        //each Tile has 4 hints that it contributes to. This adds those hints as properties of the tiles
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
        //each Hint has a row that it looks down. This adds those tiles to each hint in the order of viewing
        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++) {
                topHints[j].row[i] = tilesArray[i, j];
                leftHints[i].row[j] = tilesArray[i, j];
            }

        }
        for (int i = 0; i < size; i++) {
            bottomHints[i].row = reverseList(topHints[i].row);
            rightHints[i].row = reverseList(leftHints[i].row);
        }
    }

    // ---------------------------------------------------
    //FUNCTIONS THAT SOLVE THE PUZZLE AS MUCH AS CAN BE DONE WITH THE PROVIDED HINTS
    // ---------------------------------------------------

    public void solvePuzzle(int[] topHints, int[] bottomHints, int[] leftHints, int[] rightHints, int[,] startingSetup) {
        //uses the hints provided to solve the puzzle
        //this is done in two stages - immediate and iterative
        //first, create all necessary objects to store data, in the setUp function
        setUp(topHints, bottomHints, leftHints, rightHints, startingSetup);

        //next, using just the numbers on the sides, fill in any immediately-knowable information
        immediatePopulation();
        immediateProhibition();

        //next, make several iterations of attempting to solve the puzzle
        //if no progress is made through iterations, the puzzle solving cannot continue.
        //at that point, isPuzzleValid takes the progress made and determines if the puzzle is valid
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

    }

    private void immediatePopulation() {
        //the part of the puzzle-solving algorithm that happens first. Called by SolvePuzzle
        //any tile numbers that are immediately knowable from the hintsare filled in at the start

        foreach (PuzzleSolverSideTile h in hintsList) {
            //if hint is 1, the adjacent tile has to be the highest possible number
            if (h.hint == 1) { h.row[0].populate(size); }

            //if hint is 2 and the opposite hint is a 1, the adjacent tile has to be the second highest possible number
            if ((h.hint == 2) && h.oppositeHint.hint == 1) { h.row[0].populate(size - 1); }

            //if hint is the puzzle size, fill in all tiles in the row in ascending order
            if (h.hint == size) {
                for (int i = 0; i < size; i++) {
                    h.row[i].populate(i + 1);
                }
            }

            //if hint is between 1 and the max number (exclusive) and if the hint and the opposite hint add to max value + 1, the highest possible number has to be hint-value spaces into the current row
            if ((h.hint > 1) && (h.hint < size) && ((h.hint + h.oppositeHint.hint) == (size + 1))) {
                h.row[h.hint - 1].populate(size);
            }
        }
    }

    private void immediateProhibition() {
        //the part of the puzzle-solving algorithm that happens second. Called by SolvePuzzle
        //any tile with numbers that can immediately be removed as possible solutions are crossed-out here

        foreach (PuzzleSolverSideTile h in hintsList) {
            //if hint is between 1 and the max number (exclusive), the tiles next to it cannot be high numbers
            if ((h.hint > 1) && (h.hint < size)) {
                int numTiles = h.hint - 1;
                for (int i = 0; i < numTiles; i++) {
                    PuzzleSolverTile tile = h.row[i];
                    int prohibitTopX = numTiles - i;
                    for (int j = 0; j < prohibitTopX; j++) {
                        tile.prohibitValue(size - j);
                    }
                }
            }
        }
    }

    private void iterativeProhibition() {
        //every iteration, checks all hints and tiles to see which numbers can be crossed-out. Called by SolvePuzzle

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
                for (int i = 0; i < tilesToProhibit; i++) {
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
                }
            }
        }
    }

    private void iterativePopulation() {
        //every iteration, checks all hints and tiles to see which values can be filled-in. Called by SolvePuzzle
        //if size - 1 values are prohibited on a tile, then the tile can be populated with the last unprohibited value
        foreach (PuzzleSolverTile t in tilesList) {
            if (t.prohibitedValues.Count == size - 1) {
                t.populateLastValue();
            }
        }

        foreach (PuzzleSolverSideTile h in hintsList) {
            //if hint is between 1 and size (exclusive) and it is next to the only contiguous space in the row, you might be able to populate the row in ascending or descending order
            if ((h.hint > 1) && (h.hint < size) && h.isHintNextToContiguousUnpopulatedArea() && (h.isHighestValueInRow()) && (!h.isAnyUnusedNumHigherThanLowestVisible())) {
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
                    for (int i = 0; i < unusedNumbers.Count; i++) {
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

    private int getNumTilesPopulated() {
        //finds the total number of tiles populated thus far, used in checking if the solving algorithm has made any progress since the last iteration
        int count = 0;
        foreach (PuzzleSolverTile t in tilesList) {
            if (t.populated) {
                count++;
            }
        }
        return count;
    }

    // ---------------------------------------------------
    //FUNCTIONS THAT CHECK THE SOLUTION, AND ALSO THE FUNCTION THAT STARTS THIS WHOLE CHECKING PROCESS
    // ---------------------------------------------------
    
    public bool isPuzzleValid(int[] topHints, int[] bottomHints, int[] leftHints, int[] rightHints, int[,] startingSetup) {
        //attempts to solve the puzzle using the provided hints. The puzzle is valid if there are fewer than 7 empty spots remaining in the puzzle after being solved as much as possible
        solvePuzzle(topHints, bottomHints, leftHints, rightHints, startingSetup);
        return getNumEmptyTiles() < 2;
    }

    private int getNumEmptyTiles() {
        //get the number of empty tiles, used in checking if the solution is valid
        int count = 0;
        foreach (PuzzleSolverTile t in tilesList) {
            if (!t.populated) {
                count++;
            }
        }
        return count;
    }

    // ---------------------------------------------------
    //GENERAL USEFUL FUNCTIONS
    // ---------------------------------------------------

    private PuzzleSolverTile[] reverseList(PuzzleSolverTile[] original) {
        //reverses a list of PuzzleSolverTile objects
        PuzzleSolverTile[] newList = new PuzzleSolverTile[size];
        for (int i = 0; i < size; i++) {
            int oppositeIndex = size - i;
            newList[i] = original[oppositeIndex - 1];
        }
        return newList;
    }

    private int[] reverseList(int[] original) {
        //reverses a list of Ints
        int[] newList = new int[size];
        for (int i = 0; i < original.Length; i++) {
            int oppositeIndex = original.Length - i;
            newList[i] = original[oppositeIndex - 1];
        }
        return newList;
    }

    private List<int> reverseList(List<int> original) {
        //reverses a list of Ints
        List<int> newList = new List<int>();
        for (int i = 0; i < original.Count; i++) {
            int oppositeIndex = original.Count - i;
            newList.Add(original[oppositeIndex - 1]);
        }
        return newList;
    }
}
