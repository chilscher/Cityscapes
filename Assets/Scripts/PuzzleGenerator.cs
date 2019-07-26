using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleGenerator : MonoBehaviour{

    public int size;
    private Puzzle puzzle;
    public GameObject puzzleTilePrefab;
    public GameObject sideHintTilePrefab;
    private List<PuzzleTile> puzzleTiles;
    private bool hasWonYet = false;
    [Range(0.75f, 1f)]
    public float relativeWidth = 0.75f; //the width of the puzzle relative to the total screen width
    [Range(-1f, 1f)]
    public float relativeHeight = 0f; //how high up the screen the center of the puzzle is

    public bool usePredeterminedSolution;
    public string predeterminedSolution;

    //public bool showUniqueSolution;
    //public bool testUniqueness;
    //public bool randomSize;
    
    private SideHintTile[] topHints;
    private SideHintTile[] bottomHints;
    private SideHintTile[] leftHints;
    private SideHintTile[] rightHints;

    private SideHintTile[] allHints;
    private PuzzleTile[,] tilesArray;
    

    void Start(){
        /*
        if (randomSize) {
            
            System.Random random = new System.Random();
            int x = random.Next(2);
            size = 4 + x;
            
            size = 5;
        }
        */
        puzzleTiles = new List<PuzzleTile>();
        bool drawPuzzle = true;
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
                drawPuzzle = false;
            }
            puzzle = new Puzzle(size);
        }


        if (drawPuzzle) {
            drawFullPuzzle();
        }
        


        //if (drawPuzzle) {
            /*
            topHints = new SideHintTile[size];
            bottomHints = new SideHintTile[size];
            leftHints = new SideHintTile[size];
            rightHints = new SideHintTile[size];
            */
            //addRowsToSideHints();
            /*
            if (true) {
                relativeWidth = 0.9f;
                relativeHeight = -0.85f;
                drawFullPuzzle();
                showPuzzleSolutionOnTiles();

                relativeHeight = 0.85f;
                puzzle.getUniqueSolution();
                drawFullPuzzle();
                showPuzzleSolutionOnTiles();
            }
            */
            /*
            if (testUniqueness) {
                relativeWidth = 0.9f;
                relativeHeight = -0.85f;
                drawFullPuzzle();
                showPuzzleSolutionOnTiles();
                relativeHeight = 0.85f;
                puzzle.getUniqueSolution();
                drawFullPuzzle();
                showPuzzleSolutionOnTiles();
            }
            */
            /*
            else if (showUniqueSolution) {
                puzzle.getUniqueSolution();
                drawFullPuzzle();
                showPuzzleSolutionOnTiles();
            }
            */
            //else {
            //    drawFullPuzzle();
                //showPuzzleSolutionOnTiles();
            //}
        // }
        
    }

    private void Update() {
        
        if (!hasWonYet && checkPuzzle()) {
            hasWonYet = true;
            foreach (PuzzleTile t in puzzleTiles) {
            }
            print("you win!");
        }
        
        //print(checkPuzzle());
    }

    private void drawPuzzle(Vector2 center, float tileSize) {
        float defaultTileScale = puzzleTilePrefab.GetComponent<BoxCollider2D>().size.x;
        float totalSize = puzzle.size * tileSize * defaultTileScale;
        tilesArray = new PuzzleTile[size, size];
        for (int i = 0; i < puzzle.size; i++) {
            for (int j = 0; j < puzzle.size; j++) {
                Vector2 pos = new Vector2(center.x - (totalSize / 2) + (tileSize * defaultTileScale * (j + 0.5f)), center.y + (totalSize / 2) - (tileSize * defaultTileScale * (i + 0.5f)));
                GameObject tile = Instantiate(puzzleTilePrefab);
                tile.GetComponent<PuzzleTile>().initialize(pos, tileSize, puzzle.solution[i, j], this.transform, puzzle.size);
                puzzleTiles.Add(tile.GetComponent<PuzzleTile>());
                tilesArray[i, j] = tile.GetComponent<PuzzleTile>();
            }
        }
    }

    private void drawSideHints(Vector2 center, float tileSize) {
        float defaultTileScale = sideHintTilePrefab.GetComponent<BoxCollider2D>().size.x;
        float puzzleTotalSize = puzzle.size * tileSize * puzzleTilePrefab.GetComponent<BoxCollider2D>().size.x;
        float topy = center.y + ((puzzleTotalSize) / 2) + (tileSize * defaultTileScale * 0.5f);
        float bottomy = center.y - ((puzzleTotalSize) / 2) - (tileSize * defaultTileScale * 0.5f);
        float leftx = center.x - (puzzleTotalSize / 2) - (tileSize * defaultTileScale * (0.5f));
        float rightx = center.x + (puzzleTotalSize / 2) + (tileSize * defaultTileScale * (0.5f));
        topHints = new SideHintTile[size];
        bottomHints = new SideHintTile[size];
        leftHints = new SideHintTile[size];
        rightHints = new SideHintTile[size];
        for (int i = 0; i < puzzle.size; i++) {
            float tbx = center.x - (puzzleTotalSize / 2) + (tileSize * defaultTileScale * (i + 0.5f));
            float lry = center.y + ((puzzleTotalSize) / 2) - (tileSize * defaultTileScale * (i + 0.5f));
            GameObject topTile = Instantiate(sideHintTilePrefab);
            GameObject bottomTile = Instantiate(sideHintTilePrefab);
            GameObject rightTile = Instantiate(sideHintTilePrefab);
            GameObject leftTile = Instantiate(sideHintTilePrefab);
            topTile.GetComponent<SideHintTile>().initialize(new Vector2(tbx, topy), tileSize, puzzle.topNums[i], this.transform);
            bottomTile.GetComponent<SideHintTile>().initialize(new Vector2(tbx, bottomy), tileSize, puzzle.bottomNums[i], this.transform);
            leftTile.GetComponent<SideHintTile>().initialize(new Vector2(leftx, lry), tileSize, puzzle.leftNums[i], this.transform);
            rightTile.GetComponent<SideHintTile>().initialize(new Vector2(rightx, lry), tileSize, puzzle.rightNums[i], this.transform);
            
            topHints[i] = topTile.GetComponent<SideHintTile>();
            bottomHints[i] = bottomTile.GetComponent<SideHintTile>();
            leftHints[i] = leftTile.GetComponent<SideHintTile>();
            rightHints[i] = rightTile.GetComponent<SideHintTile>(); 
        }
        addRowsToSideHints();
    }

    private bool checkPuzzle() {
        bool isCorrect = true;
        foreach (SideHintTile h in allHints) {
            if (!h.isRowValid()) {
                isCorrect = false;
            }
        }
        return isCorrect;
    }

    private void showPuzzleSolutionOnTiles() {
        foreach(PuzzleTile t in puzzleTiles) {
            t.showSolutionOnTile();
        }
    }

    
    private void drawFullPuzzle() {
        float totalScreenWidth = Camera.main.orthographicSize * 2f * Screen.width / Screen.height;
        float desiredPuzzleSize = totalScreenWidth * relativeWidth;
        float currentPuzzleSize = (puzzle.size + 2) * puzzleTilePrefab.GetComponent<BoxCollider2D>().size.x;
        float scaleFactor = desiredPuzzleSize / currentPuzzleSize;

        float totalScreenHeight = Camera.main.orthographicSize * 2f;
        float maximumCenterHeight = totalScreenHeight - (desiredPuzzleSize); // the maximum height of the screen that can be the center of the puzzle, while still allowing the full puzzle to fit
        float heightCenter = (maximumCenterHeight * relativeHeight) / 2;
        Vector2 puzzleCenter = new Vector2(0, heightCenter);

        drawPuzzle(puzzleCenter, scaleFactor);
        drawSideHints(puzzleCenter, scaleFactor);
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
