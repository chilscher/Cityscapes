//for Cityscapes, copyright Cole Hilscher 2020

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleGenerator : MonoBehaviour{

    private int size;
    [HideInInspector]
    public Puzzle puzzle; //the Puzzle object that creates the solution and side hints
    public GameObject puzzleTilePrefab; //the prefab to generate a bunch of Puzzle Tiles
    public GameObject sideHintTilePrefab; //the prefab to generate a bunch of SideHintTiles

    public bool usePredeterminedSolution;
    public string predeterminedSolution; //provided by the inspector
    
    //the lists of Puzzle Tile and SideHintTile objects
    [HideInInspector]
    public SideHintTile[] topHints;
    [HideInInspector]
    public SideHintTile[] bottomHints;
    [HideInInspector]
    public SideHintTile[] leftHints;
    [HideInInspector]
    public SideHintTile[] rightHints;
    [HideInInspector]
    public List<PuzzleTile> puzzleTiles;
    [HideInInspector]
    public SideHintTile[] allHints;
    [HideInInspector]
    public PuzzleTile[,] tilesArray;

    public GameManager gameManagerObject;
    

    public void createPuzzle(int size) {
        //creates a random puzzle using the Puzzle class, unless there is a predermined solution provided, in which case just use that
        this.size = size;
        puzzleTiles = new List<PuzzleTile>();
        bool makePuzzle = true;
        if (usePredeterminedSolution && predeterminedSolution != "") {
            size = (int)Mathf.Sqrt(predeterminedSolution.Length);
            int[,] x = new int[size, size];
            for (int i = 0; i < predeterminedSolution.Length; i++) {
                char c = predeterminedSolution[i];
                int ind1 = i % size;
                int ind2 = (i - ind1) / size;
                x[ind2, ind1] = (int)char.GetNumericValue(c);
            }
            puzzle = new Puzzle(x);
        }
        else {
            if (size < 2 || size > 7) {
                print("the puzzle size has to be between 2 and 7. It is currently " + size);
                makePuzzle = false;
            }
            puzzle = new Puzzle(size);
        }
        if (makePuzzle) {
            createTiles();
            createHints();
        }
    }
    
    private void createTiles() {
        //create an array of PuzzleTiles to hold the puzzle's buildings and notes
        tilesArray = new PuzzleTile[size, size];
        for (int i = 0; i < puzzle.size; i++) {
            for (int j = 0; j < puzzle.size; j++) {
                GameObject tile = Instantiate(puzzleTilePrefab);
                tile.GetComponent<PuzzleTile>().initialize(puzzle.solution[i, j], this.transform, puzzle.size, gameManagerObject.GetComponent<GameManager>());
                puzzleTiles.Add(tile.GetComponent<PuzzleTile>());
                tilesArray[i, j] = tile.GetComponent<PuzzleTile>();
            }
        }
    }

    private void createHints() {
        //create 4 arrays of SideHintTiles to hold the resident hints
        topHints = new SideHintTile[size];
        bottomHints = new SideHintTile[size];
        leftHints = new SideHintTile[size];
        rightHints = new SideHintTile[size];

        for (int i = 0; i < puzzle.size; i++) {
            GameObject topTile = Instantiate(sideHintTilePrefab);
            GameObject bottomTile = Instantiate(sideHintTilePrefab);
            GameObject rightTile = Instantiate(sideHintTilePrefab);
            GameObject leftTile = Instantiate(sideHintTilePrefab);
            topTile.GetComponent<SideHintTile>().initialize(puzzle.topNums[i]);
            bottomTile.GetComponent<SideHintTile>().initialize(puzzle.bottomNums[i]);
            leftTile.GetComponent<SideHintTile>().initialize(puzzle.leftNums[i]);
            rightTile.GetComponent<SideHintTile>().initialize(puzzle.rightNums[i]);

            topHints[i] = topTile.GetComponent<SideHintTile>();
            bottomHints[i] = bottomTile.GetComponent<SideHintTile>();
            leftHints[i] = leftTile.GetComponent<SideHintTile>();
            rightHints[i] = rightTile.GetComponent<SideHintTile>();
        }
        addRowsToSideHints();
    }

    public bool checkPuzzle() {
        //checks if the puzzle is solved, based on the SideHintTiles and their PuzzleTiles
        bool isCorrect = true;
        foreach (SideHintTile h in allHints) {
            if (!h.isRowValid()) {
                isCorrect = false;
            }
        }
        return isCorrect;
    }
    
    private void addRowsToSideHints() {
        //provides each SideHintTile with its list of PuzzleTiles that it looks towards
        //this list of PuzzleTiles is used to check the puzzle for solutions while the player is solving it
        createHintsList();
        foreach(SideHintTile h in allHints) {
            h.row = new PuzzleTile[size];
        }
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

    private PuzzleTile[] reverseList(PuzzleTile[] original) {
        //reverses a list
        PuzzleTile[] newList = new PuzzleTile[size];
        for (int i = 0; i < original.Length; i++) {
            int oppositeIndex = original.Length - i;
            newList[i] = original[oppositeIndex - 1];
        }
        return newList;
    }

    private void createHintsList() {
        //compiles all of the SideHintTiles into one list
        allHints = new SideHintTile[size * 4];
        for (int i = 0; i < size; i++) {
            allHints[i] = topHints[i];
            allHints[size + i] = bottomHints[i];
            allHints[size + size + i] = leftHints[i];
            allHints[size + size + size + i] = rightHints[i];
        }
    }

    public string makeSolutionString() {
        //creates a string that represents the puzzle solution
        //used for loading a saved puzzle, or using a predetermined puzzle from the inspector
        string s = "";
        foreach (PuzzleTile t in tilesArray) {
            s += t.solution;
        }
        return s;
    }

    public void restoreSavedPuzzle(string solutionString, int size) {
        //creates a puzzle based off of a provided solution string
        predeterminedSolution = solutionString;
        usePredeterminedSolution = true;
        createPuzzle(size);
    }

}
