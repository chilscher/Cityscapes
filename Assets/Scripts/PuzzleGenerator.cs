using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleGenerator : MonoBehaviour{

    private int size;
    [HideInInspector]
    public Puzzle puzzle;
    public GameObject puzzleTilePrefab;
    public GameObject sideHintTilePrefab;
    private List<PuzzleTile> puzzleTiles;

    public bool usePredeterminedSolution;
    public string predeterminedSolution;
    
    public SideHintTile[] topHints;
    public SideHintTile[] bottomHints;
    public SideHintTile[] leftHints;
    public SideHintTile[] rightHints;

    public SideHintTile[] allHints;
    public PuzzleTile[,] tilesArray;

    public GameManager gameManagerObject;
    

    public void createPuzzle(int size) {
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
        bool isCorrect = true;
        foreach (SideHintTile h in allHints) {
            if (!h.isRowValid()) {
                isCorrect = false;
            }
        }
        return isCorrect;
    }
    
    
    private void addRowsToSideHints() {
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
        PuzzleTile[] newList = new PuzzleTile[size];
        for (int i = 0; i < original.Length; i++) {
            int oppositeIndex = original.Length - i;
            newList[i] = original[oppositeIndex - 1];
        }
        return newList;
    }

    private void createHintsList() {
        allHints = new SideHintTile[size * 4];
        for (int i = 0; i < size; i++) {
            allHints[i] = topHints[i];
            allHints[size + i] = bottomHints[i];
            allHints[size + size + i] = leftHints[i];
            allHints[size + size + size + i] = rightHints[i];
        }
    }

}
