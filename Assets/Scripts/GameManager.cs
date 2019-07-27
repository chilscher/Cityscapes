using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    //public string clickTileAction = "Click Through";
    //public string clickTileAction = "Apply Selected";
    //public string clickTileAction = "Toggle Red Hint";
    //public string clickTileAction = "Toggle Green Hint";
    [HideInInspector]
    public string clickTileAction = "Apply Selected";
    [HideInInspector]
    public int selectedNumber = 0;
    public PuzzleGenerator puzzleGenerator;
    [Range(0.75f, 1f)]
    public float relativeWidth = 0.9f; //the width of the puzzle relative to the total screen width
    [Range(-1f, 1f)]
    public float relativeHeight = 0.85f; //how high up the screen the center of the puzzle is
    [Range(0.6f, 1f)]
    public float numberButtonWidth = 0.7f; //the width of the puzzle relative to the total screen width
    [Range(-1f, 1f)]
    public float numberButtonHeight = -0.2f; //how high up the screen the center of the puzzle is
    public int size;

    public GameObject numberButtonPrefab;



    private bool hasWonYet = false;


    private void Start() {
        puzzleGenerator.createPuzzle(size);
        drawFullPuzzle();
        drawNumberButtons();
    }

    private void Update() {

        if (!hasWonYet && puzzleGenerator.checkPuzzle()) {
            hasWonYet = true;
            print("you win!");
        }
    }




    private void drawFullPuzzle() {
        float totalScreenWidth = Camera.main.orthographicSize * 2f * Screen.width / Screen.height;
        float desiredPuzzleSize = totalScreenWidth * relativeWidth;
        float defaultTileScale = puzzleGenerator.puzzleTilePrefab.GetComponent<BoxCollider2D>().size.x;
        float currentPuzzleSize = (size + 2) * defaultTileScale;
        float scaleFactor = desiredPuzzleSize / currentPuzzleSize;

        float totalScreenHeight = Camera.main.orthographicSize * 2f;
        float maximumCenterHeight = totalScreenHeight - (desiredPuzzleSize); // the maximum height of the screen that can be the center of the puzzle, while still allowing the full puzzle to fit
        float heightCenter = (maximumCenterHeight * relativeHeight) / 2;
        Vector2 puzzleCenter = new Vector2(0, heightCenter);

        //draw puzzle
        float totalSize = size * scaleFactor * defaultTileScale;
        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++) {
                Vector2 pos = new Vector2(puzzleCenter.x - (totalSize / 2) + (scaleFactor * defaultTileScale * (j + 0.5f)), puzzleCenter.y + (totalSize / 2) - (scaleFactor * defaultTileScale * (i + 0.5f)));
                puzzleGenerator.tilesArray[i, j].setValues(pos, scaleFactor, this.transform);
            }
        }

        //draw hints
        float topy = puzzleCenter.y + ((totalSize) / 2) + (scaleFactor * defaultTileScale * 0.5f);
        float bottomy = puzzleCenter.y - ((totalSize) / 2) - (scaleFactor * defaultTileScale * 0.5f);
        float leftx = puzzleCenter.x - (totalSize / 2) - (scaleFactor * defaultTileScale * (0.5f));
        float rightx = puzzleCenter.x + (totalSize / 2) + (scaleFactor * defaultTileScale * (0.5f));
        for (int i = 0; i < size; i++) {
            float tbx = puzzleCenter.x - (totalSize / 2) + (scaleFactor * defaultTileScale * (i + 0.5f));
            float lry = puzzleCenter.y + ((totalSize) / 2) - (scaleFactor * defaultTileScale * (i + 0.5f));
            SideHintTile topTile = puzzleGenerator.topHints[i];
            SideHintTile bottomTile = puzzleGenerator.bottomHints[i];
            SideHintTile leftTile = puzzleGenerator.leftHints[i];
            SideHintTile rightTile = puzzleGenerator.rightHints[i];
            topTile.setValues(new Vector2(tbx, topy), scaleFactor, this.transform);
            bottomTile.setValues(new Vector2(tbx, bottomy), scaleFactor, this.transform);
            leftTile.setValues(new Vector2(leftx, lry), scaleFactor, this.transform);
            rightTile.setValues(new Vector2(rightx, lry), scaleFactor, this.transform);
        }
        foreach (SideHintTile h in puzzleGenerator.allHints) {
            h.addHint();
        }
    }

    private void drawNumberButtons() {

        float totalScreenWidth = Camera.main.orthographicSize * 2f * Screen.width / Screen.height;
        float desiredWidth = totalScreenWidth * numberButtonWidth;
        float buttonSpacing = 0.2f;
        float defaultButtonScale = numberButtonPrefab.GetComponent<BoxCollider2D>().size.x;
        float currentPuzzleSize = (size + (buttonSpacing * (size - 1))) * defaultButtonScale;
        float scaleFactor = desiredWidth / currentPuzzleSize;

        float totalScreenHeight = Camera.main.orthographicSize * 2f;
        float heightCenter = (totalScreenHeight * numberButtonHeight) / 2;
        Vector2 numberButtonCenter = new Vector2(0, heightCenter);

        float totalSize = (size + (buttonSpacing * (size - 1))) * scaleFactor * defaultButtonScale;
        for (int i = 0; i < size; i++) {
            Vector2 pos = new Vector2(numberButtonCenter.x - (totalSize / 2) + (scaleFactor * defaultButtonScale * (i + 0.5f + (i * buttonSpacing))), numberButtonCenter.y);

            GameObject b = Instantiate(numberButtonPrefab);
            NumberButton button = b.GetComponent<NumberButton>();
            button.initialize(i + 1, this);
            button.setValues(pos, scaleFactor, this.transform);
        }
    }

    public void switchNumber(int num) {
        selectedNumber = num;
    }

    public void hitBuildButton() {
        clickTileAction = "Apply Selected";
    }
    public void hitRedButton() {
        clickTileAction = "Toggle Red Hint";
    }

    public void hitGreenButton() {
        clickTileAction = "Toggle Green Hint";
    }

    

}