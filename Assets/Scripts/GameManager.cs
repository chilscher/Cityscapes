using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    
    [HideInInspector]
    public string clickTileAction = "Apply Selected";
    [HideInInspector]
    public int selectedNumber = 0;
    public PuzzleGenerator puzzleGenerator;
    public int size;

    public GameObject numberButtonPrefab;
    //public GameObject selectionModeButtonPrefab;
    public GameObject puzzlePositioning;
    public GameObject puzzlePositioningImage;
    public GameObject numbersPositioning;
    public GameObject numbersPositioningImage;
    public GameObject canvas;
    public GameObject winCanvas;
    private NumberButton prevClickedNumButton;
    public GameObject buildButton;
    public GameObject redButton;
    public GameObject greenButton;
    private GameObject prevNumberSelectionButton;
    public Sprite selectionModeOn;
    public Sprite selectionModeOff;
    public GameObject streetCorner;
    public bool includeRedNoteBtn;
    public bool includeGreenNoteBtn;
    public GameObject selectionModeButtons1;
    public GameObject selectionModeButtons2;
    public GameObject selectionModeButtons3;
    public GameObject undoButton;
    public GameObject redoButton;

    public TutorialManager tutorialManager;
    public GameObject screenTappedMonitor;
    public GameObject tutorialTextBox;
    public GameObject redStreetBorder;

    public int coinsFor3Win = 3;
    public int coinsFor4Win = 5;
    public int coinsFor5Win = 10;
    public int coinsFor6Win = 20;

    [HideInInspector]
    public bool canClick = true;
    
    private bool hasWonYet = false;

    private List<PuzzleState> previousPuzzleStates = new List<PuzzleState>(); // the list of puzzle states to be restored by the undo button
    private PuzzleState currentPuzzleState;
    private List<PuzzleState> nextPuzzleStates = new List<PuzzleState>();// the list of puzzle states to be restored by the redo button


    private void Start() {
        size = StaticVariables.size;
        includeGreenNoteBtn = StaticVariables.includeGreenNoteButton;
        includeRedNoteBtn = StaticVariables.includeRedNoteButton;
        if (StaticVariables.isTutorial) {
            includeGreenNoteBtn = false;
            includeRedNoteBtn = false;
        }

        if (!StaticVariables.isTutorial) {
            puzzleGenerator.createPuzzle(size);
            drawFullPuzzle();
            drawNumberButtons();
            hidePositioningObjects();
            setSelectionModeButtons();
            setUndoRedoButtons();
            hitBuildButton();

            currentPuzzleState = new PuzzleState(puzzleGenerator);
        }
        
        else {
            tutorialManager = new TutorialManager();
            tutorialManager.gameManager = this;
            tutorialManager.startTutorial();
        }
        


    }

    private void Update() {
        //print(StaticVariables.coins);
        foreach(SideHintTile h in puzzleGenerator.allHints) {
            h.setSpriteToAppropriateColor();
        }
        if (!hasWonYet && puzzleGenerator.checkPuzzle() && !StaticVariables.isTutorial) {
            hasWonYet = true;
            winCanvas.SetActive(true);
            canClick = false;
            incrementCoinsForWin();
        }
    }

    public void setSelectionModeButtons() {
        if (includeGreenNoteBtn && includeRedNoteBtn) {
            selectionModeButtons1.SetActive(true);
            selectionModeButtons2.SetActive(false);
            selectionModeButtons3.SetActive(false);
            buildButton = selectionModeButtons1.transform.GetChild(0).gameObject;
            redButton = selectionModeButtons1.transform.GetChild(1).gameObject;
            greenButton = selectionModeButtons1.transform.GetChild(2).gameObject;
        }
        else if (!includeGreenNoteBtn && !includeRedNoteBtn) {
            selectionModeButtons1.SetActive(false);
            selectionModeButtons2.SetActive(false);
            selectionModeButtons3.SetActive(false);
        }
        else if (!includeGreenNoteBtn && includeRedNoteBtn){
            selectionModeButtons1.SetActive(false);
            selectionModeButtons2.SetActive(true);
            selectionModeButtons3.SetActive(false);
            buildButton = selectionModeButtons2.transform.GetChild(0).gameObject;
            redButton = selectionModeButtons2.transform.GetChild(1).gameObject;
        }
        else if (includeGreenNoteBtn && !includeRedNoteBtn) {
            selectionModeButtons1.SetActive(false);
            selectionModeButtons2.SetActive(false);
            selectionModeButtons3.SetActive(true);
            buildButton = selectionModeButtons3.transform.GetChild(0).gameObject;
            greenButton = selectionModeButtons3.transform.GetChild(1).gameObject;
        }
    }

    public void hideUndoAndRedo() {
        undoButton.SetActive(false);
        redoButton.SetActive(false);
    }

    public void setUndoRedoButtons() {
        if (StaticVariables.includeUndoRedo) {
            undoButton.SetActive(true);
            redoButton.SetActive(true);
        }
        else {
            hideUndoAndRedo();
        }
    }

    public void hidePositioningObjects() {
        puzzlePositioningImage.SetActive(false);
        numbersPositioningImage.SetActive(false);
    }

    public void drawFullPuzzle() {
        float desiredPuzzleSize = puzzlePositioning.transform.localScale.x;
        
        if (canvas.GetComponent<RectTransform>().rect.height > canvas.GetComponent<CanvasScaler>().referenceResolution.y) {
            desiredPuzzleSize *= (canvas.GetComponent<CanvasScaler>().referenceResolution.y / canvas.GetComponent<RectTransform>().rect.height);
        }


        float defaultTileScale = puzzleGenerator.puzzleTilePrefab.GetComponent<BoxCollider2D>().size.x;
        float currentPuzzleSize = (size + 2) * defaultTileScale;
        float scaleFactor = desiredPuzzleSize / currentPuzzleSize;
        Vector2 puzzleCenter = puzzlePositioning.transform.position;

        //draw puzzle
        Transform parent = puzzlePositioning.transform;
        float totalSize = size * scaleFactor * defaultTileScale;
        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++) {
                Vector2 pos = new Vector2(puzzleCenter.x - (totalSize / 2) + (scaleFactor * defaultTileScale * (j + 0.5f)), puzzleCenter.y + (totalSize / 2) - (scaleFactor * defaultTileScale * (i + 0.5f)));
                puzzleGenerator.tilesArray[i, j].setValues(pos, scaleFactor, parent);
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
            topTile.setValues(new Vector2(tbx, topy), scaleFactor, parent);
            bottomTile.setValues(new Vector2(tbx, bottomy), scaleFactor, parent);
            leftTile.setValues(new Vector2(leftx, lry), scaleFactor, parent);
            rightTile.setValues(new Vector2(rightx, lry), scaleFactor, parent);
            topTile.rotateHint(0, (totalSize / size));
            bottomTile.rotateHint(180, (totalSize / size));
            leftTile.rotateHint(90, (totalSize / size));
            rightTile.rotateHint(270, (totalSize / size));
        }
        foreach (SideHintTile h in puzzleGenerator.allHints) {
            h.addHint();
        }

        //draw street corners
        Vector2 topLeftPos = new Vector2(leftx, topy);
        Vector2 bottomLeftPos = new Vector2(leftx, bottomy);
        Vector2 topRightPos = new Vector2(rightx, topy);
        Vector2 bottomRightPos = new Vector2(rightx, bottomy);
        createCorner(topLeftPos, scaleFactor, 0, parent);
        createCorner(bottomLeftPos, scaleFactor, 90, parent);
        createCorner(topRightPos, scaleFactor, 270, parent);
        createCorner(bottomRightPos, scaleFactor, 180, parent);
    }

    public void drawNumberButtons() {
        //assumes numbersPositioning object is a rectangle
        float numberSize = numbersPositioning.transform.localScale.y;
        float totalWidth = numbersPositioning.transform.localScale.x;
        if (canvas.GetComponent<RectTransform>().rect.height > canvas.GetComponent<CanvasScaler>().referenceResolution.y) {
            totalWidth *= (canvas.GetComponent<CanvasScaler>().referenceResolution.y / canvas.GetComponent<RectTransform>().rect.height);
            numberSize *= (canvas.GetComponent<CanvasScaler>().referenceResolution.y / canvas.GetComponent<RectTransform>().rect.height);
        }

        //rescale so the numbers aren't jammed together
        float minSpacing = numberSize * 0.2f;
        if (totalWidth < numberSize * size) {
            numberSize = (totalWidth - (minSpacing * (size - 1)))/ size;
        }


        float spaceWidth = (totalWidth - (numberSize * size)) / (size - 1);
        Vector2 center = numbersPositioning.transform.position;
        float defaultButtonScale = numberButtonPrefab.GetComponent<BoxCollider2D>().size.x;
        float scaleFactor = numberSize / defaultButtonScale;
        
        Transform parent = numbersPositioning.transform;
        for (int i = 0; i < size; i++) {
            float xpos = center.x + (spaceWidth * i) + ((i + 0.5f)* numberSize) - totalWidth / 2;
            Vector2 pos = new Vector2(xpos, center.y);

            GameObject b = Instantiate(numberButtonPrefab);
            NumberButton button = b.GetComponent<NumberButton>();
            button.initialize(i + 1, this);
            button.setValues(pos, scaleFactor, parent);
        }
    }

    public void switchNumber(int num) {
        selectedNumber = num;
        if (StaticVariables.isTutorial) {
            tutorialManager.tappedNumberButton(num);
        }
    }

    public void hitBuildButton() {
        clickTileAction = "Apply Selected";
        showNumberSelectionButtonClicked(buildButton);
    }
    public void hitRedButton() {
        clickTileAction = "Toggle Red Hint";
        showNumberSelectionButtonClicked(redButton);
    }

    public void hitGreenButton() {
        clickTileAction = "Toggle Green Hint";
        showNumberSelectionButtonClicked(greenButton);
    }

    public void goToMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    public void generateNewPuzzleSameSize() {
        SceneManager.LoadScene("InPuzzle");
    }

    public void showNumberButtonClicked(NumberButton nb) {
        if (prevClickedNumButton != null) {
            disselectNumber(prevClickedNumButton);
        }
        prevClickedNumButton = nb;
        selectNumber(nb);
    }

    public void showNumberSelectionButtonClicked(GameObject btn) {
        if (prevNumberSelectionButton != null) {
            disselectButton(prevNumberSelectionButton);
        }
        prevNumberSelectionButton = btn;
        selectButton(btn);
    }
    
    private void disselectButton(GameObject btn) {
        btn.GetComponent<Image>().sprite = selectionModeOff;
    }

    private void selectButton(GameObject btn) {
        btn.GetComponent<Image>().sprite = selectionModeOn;
    }

    private void selectNumber(NumberButton btn) {
        btn.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = selectionModeOn;
    }

    private void disselectNumber(NumberButton btn) {
        btn.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = selectionModeOff;
    }

    private void createCorner(Vector2 p, float scale, int rot, Transform parent) {

        GameObject corner = Instantiate(streetCorner);
        corner.transform.position = p;
        corner.transform.localScale *= scale;
        corner.transform.Rotate(new Vector3(0, 0, rot));
        corner.transform.parent = parent;


        Vector3 pos = corner.transform.localPosition;
        pos.z = 0;
        corner.transform.localPosition = pos;
    }

    public void tappedScreen() {
        tutorialManager.tappedScreen();
    }

    public void hideHints() {

        foreach (SideHintTile h in puzzleGenerator.allHints) {

            h.GetComponent<Transform>().GetChild(1).gameObject.SetActive(false);
            h.GetComponent<Transform>().GetChild(2).gameObject.SetActive(false);
        }
    }

    public void showHints() {
        foreach (SideHintTile h in puzzleGenerator.allHints) {

            h.GetComponent<Transform>().GetChild(1).gameObject.SetActive(true);
            h.GetComponent<Transform>().GetChild(2).gameObject.SetActive(true);
        }
    }

    public void addRedStreetBorderForTutorial(Vector3 centerPoint, int rotation) {
        GameObject redBorder = Instantiate(redStreetBorder);
        redBorder.transform.SetParent(puzzlePositioning.transform);
        redBorder.transform.position = centerPoint;
        redBorder.transform.Rotate(new Vector3(0, 0, rotation));
    }

    public void removeRedStreetBordersForTutorial() {
        GameObject[] RedBorders = GameObject.FindGameObjectsWithTag("Red Border");
        foreach (GameObject r in RedBorders){
            Destroy(r);
        }
    }

    public void deleteCityForTutorial() {
        GameObject[] oldPuzzleTiles = GameObject.FindGameObjectsWithTag("Puzzle Tile");
        foreach (GameObject t in oldPuzzleTiles) {
            Destroy(t);
        }
        GameObject[] oldSideTiles = GameObject.FindGameObjectsWithTag("Side Tile");
        foreach (GameObject t in oldSideTiles) {
            Destroy(t);
        }
    }

    private void incrementCoinsForWin() {
        int amt = size;
        switch (size) {
            case 3:
                amt = coinsFor3Win;
                break;
            case 4:
                amt = coinsFor4Win;
                break;
            case 5:
                amt = coinsFor5Win;
                break;
            case 6:
                amt = coinsFor6Win;
                break;
        }
        StaticVariables.coins += amt;
    }


    private void OnApplicationQuit() {
        SaveSystem.SaveGame();
    }
    /*
    public void savePuzzleState() {
        PuzzleState s = new PuzzleState(puzzleGenerator);
        puzzleState = s;
    }

    public void restorePuzzleState() {
        puzzleState.restorePuzzleState(puzzleGenerator);
    }
    */

    public void addToPuzzleHistory() {
        previousPuzzleStates.Add(currentPuzzleState);
        PuzzleState currentState = new PuzzleState(puzzleGenerator);
        currentPuzzleState = currentState;
        //clear all "redo" steps
        if (nextPuzzleStates.Count > 0) {
            nextPuzzleStates = new List<PuzzleState>();
        }
    }

    public void pushUndoButton() {
        if (previousPuzzleStates.Count > 0) {
            nextPuzzleStates.Add(currentPuzzleState);

            PuzzleState currentState = previousPuzzleStates[previousPuzzleStates.Count - 1];


            currentState.restorePuzzleState(puzzleGenerator);

            currentPuzzleState = currentState;
            previousPuzzleStates.RemoveAt(previousPuzzleStates.Count - 1);
        }
    }

    public void pushRedoButton() {
        if (nextPuzzleStates.Count > 0) {
            previousPuzzleStates.Add(currentPuzzleState);
            PuzzleState currentState = nextPuzzleStates[nextPuzzleStates.Count - 1];
            currentState.restorePuzzleState(puzzleGenerator);
            currentPuzzleState = currentState;
            nextPuzzleStates.RemoveAt(nextPuzzleStates.Count - 1);
        }
    }
}