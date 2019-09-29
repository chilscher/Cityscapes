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
    public GameObject puzzlePositioning;
    public GameObject puzzlePositioningImage;
    public GameObject numbersPositioning;
    public GameObject numbersPositioningImage;
    public GameObject canvas;
    public GameObject winCanvas;
    private NumberButton prevClickedNumButton;
    private GameObject buildButton;
    private GameObject note1Button;
    private GameObject note2Button;
    private GameObject prevNumberSelectionButton;


    //public Sprite selectionModeOn;
    //public Sprite selectionModeOff;

    


    public GameObject streetCorner;
    public bool includeNote1Btn;
    public bool includeNote2Btn;
    public GameObject selectionModeButtons1;
    public GameObject selectionModeButtons2;
    public GameObject selectionModeButtons3;
    //public GameObject undoButton;
    //public GameObject redoButton;
    public GameObject undoRedoButtons;

    public TutorialManager tutorialManager;
    public GameObject screenTappedMonitor;
    public GameObject tutorialTextBox;
    public GameObject redStreetBorder;

    public int coinsFor3Win = 3;
    public int coinsFor4Win = 5;
    public int coinsFor5Win = 10;
    public int coinsFor6Win = 20;
    
    public GameObject blackForeground; //used to transition to/from the main menu
    private SpriteRenderer blackSprite;
    public float fadeOutTime;
    public float fadeInTime;
    private float fadeTimer;

    public SpriteRenderer winBlackSprite;
    private bool fadingToWin = false;
    private float fadingToWinTimer;
    public float fadeToWinTime;
    public float fadeToWinDarknessRatio;
    private float winPopupScale;

    public Skin skin;

    [HideInInspector]
    public bool canClick = true;
    
    private bool hasWonYet = false;

    private List<PuzzleState> previousPuzzleStates = new List<PuzzleState>(); // the list of puzzle states to be restored by the undo button
    private PuzzleState currentPuzzleState;
    private List<PuzzleState> nextPuzzleStates = new List<PuzzleState>();// the list of puzzle states to be restored by the redo button
    

    private Color offButtonColor;
    private Color onButtonColor;

    public GameObject coinsBox1s;
    public GameObject coinsBox10s;

    //public GameObject removeColoredHintsOfChosenNumberButton;
    private GameObject removeAllOfNumberButton;
    private GameObject clearPuzzleButton;

    public GameObject removeAllAndClearButtons;
    public GameObject onlyRemoveAllButton;
    public GameObject onlyClearButton;

    private void Start() {
        if (StaticVariables.isFading && StaticVariables.fadingTo == "puzzle") {
            fadeTimer = fadeInTime;
        }
        blackSprite = blackForeground.GetComponent<SpriteRenderer>();
        winPopupScale = winCanvas.transform.GetChild(1).localScale.x;

        size = StaticVariables.size;
        includeNote1Btn = StaticVariables.includeNotes1Button;
        includeNote2Btn = StaticVariables.includeNotes2Button;


        ColorUtility.TryParseHtmlString(skin.offButtonColor, out offButtonColor);
        ColorUtility.TryParseHtmlString(skin.onButtonColor, out onButtonColor);

        if (StaticVariables.isTutorial) {
            includeNote1Btn = false;
            includeNote2Btn = false;
            tutorialTextBox.GetComponent<Image>().color = offButtonColor;
        }

        if (!StaticVariables.isTutorial) {
            puzzleGenerator.createPuzzle(size);
            drawFullPuzzle();
            setRemoveAllAndClearButtons();
            drawNumberButtons();
            switchNumber(size);
            //showNumberButtonClicked();
            hidePositioningObjects();
            setSelectionModeButtons();
            setUndoRedoButtons();
            //setRemoveColoredNotesButton();
            //setClearPuzzleButton();
            hitBuildButton();
            winCanvas.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = onButtonColor;
            winCanvas.transform.GetChild(1).transform.GetChild(3).transform.GetChild(0).GetComponent<Image>().color = offButtonColor;
            winCanvas.transform.GetChild(1).transform.GetChild(4).transform.GetChild(0).GetComponent<Image>().color = offButtonColor;


            int coins = 0;
            switch(size){
                case 3: coins = coinsFor3Win; break;
                case 4: coins = coinsFor4Win; break;
                case 5: coins = coinsFor5Win; break;
                case 6: coins = coinsFor6Win; break;
            }
            int onesDigit = coins % 10;
            int tensDigit = (coins / 10) % 10;

            coinsBox1s.GetComponent<SpriteRenderer>().sprite = numberButtonPrefab.GetComponent<NumberButton>().numberSprites[onesDigit];
            coinsBox10s.GetComponent<SpriteRenderer>().sprite = numberButtonPrefab.GetComponent<NumberButton>().numberSprites[tensDigit];


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
            h.setAppropriateColor();
        }
        if (!hasWonYet && puzzleGenerator.checkPuzzle() && !StaticVariables.isTutorial) {
            hasWonYet = true;
            winCanvas.SetActive(true);
            canClick = false;
            incrementCoinsForWin();

            fadingToWin = true;
            fadingToWinTimer = fadeToWinTime;
        }

        if (StaticVariables.isFading && StaticVariables.fadingTo == "puzzle" && StaticVariables.fadingFrom == "puzzle") {
            if (StaticVariables.fadingIntoPuzzleSameSize) {
                fadeTimer -= Time.deltaTime;
                Color c = blackSprite.color;
                c.a = (fadeTimer) / fadeInTime;
                blackSprite.color = c;
                if (fadeTimer <= 0f) {
                    StaticVariables.isFading = false;
                }
            }
            else {
                fadeTimer -= Time.deltaTime;
                Color c = blackSprite.color;
                c.a = (fadeOutTime - fadeTimer) / fadeOutTime;
                blackSprite.color = c;
                if (fadeTimer <= 0f) {
                    StaticVariables.fadingIntoPuzzleSameSize = true;
                    if (StaticVariables.fadingTo == "puzzle") {
                        SceneManager.LoadScene("InPuzzle");
                    }

                }
            }
        }

        else if (StaticVariables.isFading && StaticVariables.fadingTo == "puzzle") {
            fadeTimer -= Time.deltaTime;
            Color c = blackSprite.color;
            c.a = (fadeTimer) / fadeInTime;
            blackSprite.color = c;
            if (fadeTimer <= 0f) {
                StaticVariables.isFading = false;
            }
        }
        else if (StaticVariables.isFading && StaticVariables.fadingFrom == "puzzle") {
            fadeTimer -= Time.deltaTime;
            Color c = blackSprite.color;
            c.a = (fadeOutTime - fadeTimer) / fadeOutTime;
            blackSprite.color = c;
            if (fadeTimer <= 0f) {
                if (StaticVariables.fadingTo == "menu") {
                    SceneManager.LoadScene("MainMenu");
                }

            }
        }

        if (fadingToWin) {
            fadingToWinTimer -= Time.deltaTime;
            if (fadingToWinTimer < 0f) { fadingToWinTimer = 0f; }
            float fadePercent = 1 - (fadingToWinTimer / fadeToWinTime); // goes from 0 to 1 as time progresses

            Color c = winBlackSprite.color;
            c.a = fadePercent * fadeToWinDarknessRatio;
            winBlackSprite.color = c;

            float scale = fadePercent * winPopupScale;
            //float scale = winPopupScale;

            winCanvas.transform.GetChild(1).localScale = new Vector3(scale, scale, scale);

            if (fadingToWinTimer <= 0f) {
                fadingToWin = false;
            }
        }
    }





    public void setSelectionModeButtons() {
        if (includeNote2Btn && includeNote1Btn) {
            selectionModeButtons1.SetActive(true);
            selectionModeButtons2.SetActive(false);
            selectionModeButtons3.SetActive(false);
            buildButton = selectionModeButtons1.transform.GetChild(0).gameObject;
            note1Button = selectionModeButtons1.transform.GetChild(1).gameObject;
            note2Button = selectionModeButtons1.transform.GetChild(2).gameObject;
        }
        else if (!includeNote2Btn && !includeNote1Btn) {
            selectionModeButtons1.SetActive(false);
            selectionModeButtons2.SetActive(false);
            selectionModeButtons3.SetActive(false);
        }
        else if (!includeNote2Btn && includeNote1Btn){
            selectionModeButtons1.SetActive(false);
            selectionModeButtons2.SetActive(true);
            selectionModeButtons3.SetActive(false);
            buildButton = selectionModeButtons2.transform.GetChild(0).gameObject;
            note1Button = selectionModeButtons2.transform.GetChild(1).gameObject;
        }
        else if (includeNote2Btn && !includeNote1Btn) {
            selectionModeButtons1.SetActive(false);
            selectionModeButtons2.SetActive(false);
            selectionModeButtons3.SetActive(true);
            buildButton = selectionModeButtons3.transform.GetChild(0).gameObject;
            note2Button = selectionModeButtons3.transform.GetChild(1).gameObject;
        }

        if (includeNote1Btn || includeNote2Btn) { buildButton.transform.GetChild(1).GetComponent<Image>().sprite = skin.buildIcon; }
        if (includeNote1Btn) { note1Button.transform.GetChild(1).GetComponent<Image>().sprite = skin.note1Icon; }
        if (includeNote2Btn) { note2Button.transform.GetChild(1).GetComponent<Image>().sprite = skin.note2Icon; }

    }

    
    public void setUndoRedoButtons() {
        undoRedoButtons.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = offButtonColor;
        undoRedoButtons.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = skin.undoIcon;
        undoRedoButtons.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = offButtonColor;
        undoRedoButtons.transform.GetChild(1).GetChild(1).GetComponent<Image>().sprite = skin.redoIcon;

        undoRedoButtons.SetActive(StaticVariables.includeUndoRedo);
    }

    public void setRemoveAllAndClearButtons() {
        removeAllAndClearButtons.SetActive(false);
        onlyRemoveAllButton.SetActive(false);
        onlyClearButton.SetActive(false);
        removeAllOfNumberButton = onlyRemoveAllButton.transform.GetChild(0).gameObject;
        clearPuzzleButton = onlyClearButton.transform.GetChild(0).gameObject;
        if (!StaticVariables.isTutorial) {
            if (StaticVariables.includeRemoveAllOfNumber && StaticVariables.includeClearPuzzle) {
                removeAllOfNumberButton = removeAllAndClearButtons.transform.GetChild(0).gameObject;
                clearPuzzleButton = removeAllAndClearButtons.transform.GetChild(1).gameObject;
                removeAllAndClearButtons.SetActive(true);
            }
            else if (StaticVariables.includeRemoveAllOfNumber) {
                onlyRemoveAllButton.SetActive(true);
            }
            else if (StaticVariables.includeClearPuzzle) {
                onlyClearButton.SetActive(true);
            }
        }
        removeAllOfNumberButton.transform.GetChild(0).GetComponent<Image>().color = offButtonColor;
        clearPuzzleButton.transform.GetChild(0).GetComponent<Image>().color = offButtonColor;
    }
    /*
    public void setRemoveColoredNotesButton() {
        //removeColoredHintsOfChosenNumberButton.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = offButtonColor;
        //removeColoredHintsOfChosenNumberButton.SetActive(StaticVariables.includeRemoveColoredNotesOfChosenNumber);
        removeAllOfNumberButton.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = offButtonColor;
        removeAllOfNumberButton.SetActive(StaticVariables.includeRemoveAllOfNumber);
    }

    public void setClearPuzzleButton() {
        clearPuzzleButton.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = offButtonColor;
        clearPuzzleButton.SetActive(StaticVariables.includeClearPuzzle);
    }
    */

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

            disselectNumber(button);
            if (i + 1 == size && !StaticVariables.isTutorial) { showNumberButtonClicked(button); }
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
        disselectBuildAndNotes();
        if (includeNote1Btn || includeNote2Btn) { buildButton.transform.GetChild(0).GetComponent<Image>().color = onButtonColor; }
        //updateRemoveColoredNumber();
        updateRemoveSelectedNumber();

    }
    public void hitNote1Button() {
        clickTileAction = "Toggle Note 1";
        disselectBuildAndNotes();
        note1Button.transform.GetChild(0).GetComponent<Image>().color = onButtonColor;
        //updateRemoveColoredNumber();
        updateRemoveSelectedNumber();
    }

    public void hitNote2Button() {
        clickTileAction = "Toggle Note 2";
        disselectBuildAndNotes();
        note2Button.transform.GetChild(0).GetComponent<Image>().color = onButtonColor;
        //updateRemoveColoredNumber();
        updateRemoveSelectedNumber();
    }

    public void disselectBuildAndNotes() {
        if (includeNote1Btn || includeNote2Btn) { buildButton.transform.GetChild(0).GetComponent<Image>().color = offButtonColor; }
        if (includeNote1Btn) { note1Button.transform.GetChild(0).GetComponent<Image>().color = offButtonColor; }
        if (includeNote2Btn) { note2Button.transform.GetChild(0).GetComponent<Image>().color = offButtonColor; }
    }

    public void goToMainMenu() {
        if (!StaticVariables.isFading) {
            StaticVariables.fadingTo = "menu";
            startFadeOut();
        }

        //SceneManager.LoadScene("MainMenu");
    }
    public void startFadeOut() {
        fadeTimer = fadeOutTime;
        StaticVariables.isFading = true;
        StaticVariables.fadingFrom = "puzzle";
    }

    public void generateNewPuzzleSameSize() {
        if (!StaticVariables.isFading) {
            StaticVariables.fadingTo = "puzzle";
            StaticVariables.fadingIntoPuzzleSameSize = false;
            startFadeOut();
        }
        //SceneManager.LoadScene("InPuzzle");
    }
    
    public void showNumberButtonClicked(NumberButton nb) {
        if (prevClickedNumButton != null) {
            disselectNumber(prevClickedNumButton);
        }
        prevClickedNumButton = nb;
        selectNumber(nb);
        //updateRemoveColoredNumber();
        updateRemoveSelectedNumber();
    }
    

    private void selectNumber(NumberButton btn) {
        SpriteRenderer s = btn.transform.GetChild(0).GetComponent<SpriteRenderer>();
        s.color = onButtonColor;

    }

    private void disselectNumber(NumberButton btn) {
        SpriteRenderer s = btn.transform.GetChild(0).GetComponent<SpriteRenderer>();
        s.color = offButtonColor;
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
    /*
    public void pushRemoveAllColorNumberNotesButton() {
        bool foundAnything = false;
        int num = selectedNumber;
        int colorNum = 0;
        if (clickTileAction == "Toggle Note 1") { colorNum = 1; }
        else if (clickTileAction == "Toggle Note 2") { colorNum = 2; }
        if (colorNum != 0 && num != 0) {
            foreach (PuzzleTile t in puzzleGenerator.puzzleTiles) {
                if (t.doesTileContainColoredNote(colorNum, num)) {
                    //print("found a tile that contains " + num + " on note #" + colorNum);
                    if (colorNum == 1) { t.toggleNote1(num); }
                    else { t.toggleNote2(num); }
                    foundAnything = true;
                }
            }
        }
        if (foundAnything) {
            addToPuzzleHistory();
        }
    }
    */

    public void pushRemoveAllNumbersButton() {
        bool foundAnything = false;
        int num = selectedNumber;
        int colorNum = 0; // 0 is build, 1 is note 1, 2 is note 2
        if (clickTileAction == "Apply Selected") { colorNum = 0; }
        else if (clickTileAction == "Toggle Note 1") { colorNum = 1; }
        else if (clickTileAction == "Toggle Note 2") { colorNum = 2; }
        if (colorNum != 0 && num != 0) {
            foreach (PuzzleTile t in puzzleGenerator.puzzleTiles) {
                if (t.doesTileContainColoredNote(colorNum, num)) {
                    //print("found a tile that contains " + num + " on note #" + colorNum);
                    if (colorNum == 1) { t.toggleNote1(num); }
                    else { t.toggleNote2(num); }
                    foundAnything = true;
                }
            }
        }
        if (colorNum == 0 && num != 0) {
            foreach(PuzzleTile t in puzzleGenerator.puzzleTiles) {
                if (t.shownNumber == num) {
                    foundAnything = true;
                    t.removeNumberFromTile();
                }
            }
        }
        if (foundAnything) {
            addToPuzzleHistory();
        }
    }


    public void pushClearPuzzleButton() {
        bool changedAnything = false;
        foreach (PuzzleTile t in puzzleGenerator.puzzleTiles) {
            if (t.doesTileContainAnything()) { changedAnything = true; }
            t.clearColoredNotes();
            t.removeNumberFromTile();
            
        }
        if (changedAnything) { addToPuzzleHistory(); }
    }

    /*
    public void updateRemoveColoredNumber() {
        if (StaticVariables.includeRemoveColoredNotesOfChosenNumber) {

            removeColoredHintsOfChosenNumberButton.transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().sprite = numberButtonPrefab.GetComponent<NumberButton>().numberSprites[selectedNumber];
            //print(selectedNumber);
            Color buildingColor;
            Color note1Color;
            Color note2Color;
            ColorUtility.TryParseHtmlString(StaticVariables.whiteHex, out buildingColor);
            ColorUtility.TryParseHtmlString(skin.note1Color, out note1Color);
            ColorUtility.TryParseHtmlString(skin.note2Color, out note2Color);
            Color c = note1Color;
            if (clickTileAction == "Apply Selected" || selectedNumber == 0) {
                c = buildingColor;
                //removeColoredHintsOfChosenNumberButton.SetActive(false);
            }
            else if (clickTileAction == "Toggle Note 1") {
                c = note1Color;
                //removeColoredHintsOfChosenNumberButton.SetActive(true);
            }
            else if (clickTileAction == "Toggle Note 2") {
                c = note2Color;
                //removeColoredHintsOfChosenNumberButton.SetActive(true);
            }

            removeColoredHintsOfChosenNumberButton.transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().color = c;
        }
    }
    */

    public void updateRemoveSelectedNumber() {
        if (!StaticVariables.isTutorial && StaticVariables.includeRemoveAllOfNumber) {

            //removeAllOfNumberButton.transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().sprite = numberButtonPrefab.GetComponent<NumberButton>().numberSprites[selectedNumber];
            removeAllOfNumberButton.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = numberButtonPrefab.GetComponent<NumberButton>().numberSprites[selectedNumber];
            //print(selectedNumber);
            Color buildingColor;
            Color note1Color;
            Color note2Color;
            ColorUtility.TryParseHtmlString(StaticVariables.whiteHex, out buildingColor);
            ColorUtility.TryParseHtmlString(skin.note1Color, out note1Color);
            ColorUtility.TryParseHtmlString(skin.note2Color, out note2Color);
            Color c = note1Color;
            if (clickTileAction == "Apply Selected" || selectedNumber == 0) {
                c = buildingColor;
                //removeColoredHintsOfChosenNumberButton.SetActive(false);
            }
            else if (clickTileAction == "Toggle Note 1") {
                c = note1Color;
                //removeColoredHintsOfChosenNumberButton.SetActive(true);
            }
            else if (clickTileAction == "Toggle Note 2") {
                c = note2Color;
                //removeColoredHintsOfChosenNumberButton.SetActive(true);
            }

            //removeAllOfNumberButton.transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().color = c;
            removeAllOfNumberButton.transform.GetChild(1).GetComponent<SpriteRenderer>().color = c;
        }
    }


}