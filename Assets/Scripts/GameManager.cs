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
    [HideInInspector]
    public int size;
    [HideInInspector]
    public bool includeNote1Btn;
    [HideInInspector]
    public bool includeNote2Btn;



    public PuzzleGenerator puzzleGenerator;
    
    public GameObject puzzlePositioning;
    public GameObject puzzlePositioningImage;
    public GameObject canvas;
    public GameObject winCanvas;
    private GameObject prevClickedNumButton;
    private GameObject buildButton;
    private GameObject note1Button;
    private GameObject note2Button;
    private GameObject prevNumberSelectionButton;
    
    public GameObject streetCorner;
    public GameObject selectionModeButtons1;
    public GameObject selectionModeButtons2;
    public GameObject selectionModeButtons3;
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
    [HideInInspector]
    public SpriteRenderer blackSprite;
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
    
    /*
    [HideInInspector]
    public Color offButtonColorExterior;
    [HideInInspector]
    public Color onButtonColorExterior;
    [HideInInspector]
    public Color offButtonColorInterior;
    [HideInInspector]
    public Color onButtonColorInterior;
    */
    private Color winBackgroundColorInterior;
    private Color winBackgroundColorExterior;
    //private Color winPopupColor;

    public GameObject coinsBox1s;
    public GameObject coinsBox10s;
    
    private GameObject removeAllOfNumberButton;
    private GameObject clearPuzzleButton;

    public GameObject removeAllAndClearButtons;
    public GameObject onlyRemoveAllButton;
    public GameObject onlyClearButton;

    [HideInInspector]
    public GameObject[] numberButtons;
    public GameObject numbers1to3;
    public GameObject numbers1to4;
    public GameObject numbers1to5;
    public GameObject numbers1to6;

    public Sprite[] numberSprites;

    public GameObject menuButton;

    public GameObject puzzleCanvas;
    public GameObject tutorialCanvas;
    private float originalPuzzleScale;
    public GameObject puzzleBackground;

    public bool showBuildings;


    private void Start() {
        skin = StaticVariables.skin;
        if (StaticVariables.isFading && StaticVariables.fadingTo == "puzzle") {
            fadeTimer = fadeInTime;
        }
        blackSprite = blackForeground.GetComponent<SpriteRenderer>();
        winPopupScale = winCanvas.transform.GetChild(1).localScale.x;

        size = StaticVariables.size;
        includeNote1Btn = StaticVariables.includeNotes1Button;
        includeNote2Btn = StaticVariables.includeNotes2Button;

        /*
        ColorUtility.TryParseHtmlString(skin.onButtonColorExterior, out onButtonColorExterior);
        ColorUtility.TryParseHtmlString(skin.onButtonColorInterior, out onButtonColorInterior);
        ColorUtility.TryParseHtmlString(skin.offButtonColorExterior, out offButtonColorExterior);
        ColorUtility.TryParseHtmlString(skin.offButtonColorInterior, out offButtonColorInterior);
        */
        ColorUtility.TryParseHtmlString(skin.mainMenuButtonExterior, out winBackgroundColorExterior);
        ColorUtility.TryParseHtmlString(skin.mainMenuButtonInterior, out winBackgroundColorInterior);
        //ColorUtility.TryParseHtmlString(skin.winPopupColor, out winPopupColor);

        hideNumberButtons();

        colorMenuButton();

        if (StaticVariables.isTutorial) {
            originalPuzzleScale = puzzlePositioning.transform.localScale.x;
            setTutorialNumberButtons();
            tutorialCanvas.SetActive(true);
            puzzleCanvas.SetActive(false);

            tutorialManager = new TutorialManager();
            tutorialManager.gameManager = this;
            tutorialManager.startTutorial();
        }

        else if (!StaticVariables.isTutorial) {
            tutorialCanvas.SetActive(false);
            puzzleCanvas.SetActive(true);
            if (StaticVariables.hasSavedPuzzleState) {
                puzzleGenerator.restoreSavedPuzzle(StaticVariables.puzzleSolution, size);
                loadPuzzleStates();
            }
            else {
                puzzleGenerator.createPuzzle(size);
            }
            drawFullPuzzle();
            setRemoveAllAndClearButtons();
            setNumberButtons();
            pushNumberButton(size);
            hidePositioningObjects();
            setSelectionModeButtons();
            setUndoRedoButtons();
            hitBuildButton();
            puzzleBackground.GetComponent<SpriteRenderer>().sprite = skin.puzzleBackground;
            InterfaceFunctions.colorPuzzleButton(winCanvas.transform.Find("Win Popup").Find("Menu"));
            InterfaceFunctions.colorPuzzleButton(winCanvas.transform.Find("Win Popup").Find("Another Puzzle"));
            /*
            winCanvas.transform.Find("Win Popup").Find("Menu").Find("Button Image").Find("Interior").GetComponent<Image>().color = offButtonColorInterior;
            winCanvas.transform.Find("Win Popup").Find("Menu").Find("Button Image").Find("Borders").GetComponent<Image>().color = offButtonColorExterior;
            winCanvas.transform.Find("Win Popup").Find("Another Puzzle").Find("Button Image").Find("Interior").GetComponent<Image>().color = offButtonColorInterior;
            winCanvas.transform.Find("Win Popup").Find("Another Puzzle").Find("Button Image").Find("Borders").GetComponent<Image>().color = offButtonColorExterior;
            */
            winCanvas.transform.Find("Win Popup").Find("Win Popup Background Exterior").GetComponent<SpriteRenderer>().color = winBackgroundColorExterior;
            winCanvas.transform.Find("Win Popup").Find("Win Popup Background Interior").GetComponent<SpriteRenderer>().color = winBackgroundColorInterior;
            Color c = winCanvas.transform.Find("Black Background").GetComponent<SpriteRenderer>().color;
            c.a = fadeToWinDarknessRatio;
            winCanvas.transform.Find("Black Background").GetComponent<SpriteRenderer>().color = c;
            showCorrectCityArtOnWinScreen();


            int coins = 0;
            switch(size){
                case 3: coins = coinsFor3Win; break;
                case 4: coins = coinsFor4Win; break;
                case 5: coins = coinsFor5Win; break;
                case 6: coins = coinsFor6Win; break;
            }
            int onesDigit = coins % 10;
            int tensDigit = (coins / 10) % 10;

            coinsBox1s.GetComponent<SpriteRenderer>().sprite = numberSprites[onesDigit];
            coinsBox10s.GetComponent<SpriteRenderer>().sprite = numberSprites[tensDigit];


            currentPuzzleState = new PuzzleState(puzzleGenerator);
        }
        


    }

    private void Update() {
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

            winCanvas.transform.GetChild(1).localScale = new Vector3(scale, scale, scale);

            if (fadingToWinTimer <= 0f) {
                fadingToWin = false;
            }
        }
    }

    private void showCorrectCityArtOnWinScreen() {
        GameObject small = winCanvas.transform.Find("Win Popup").Find("City Art - Small").gameObject;
        GameObject med = winCanvas.transform.Find("Win Popup").Find("City Art - Medium").gameObject;
        GameObject large = winCanvas.transform.Find("Win Popup").Find("City Art - Large").gameObject;
        GameObject huge = winCanvas.transform.Find("Win Popup").Find("City Art - Huge").gameObject;
        small.SetActive(false);
        med.SetActive(false);
        large.SetActive(false);
        huge.SetActive(false);
        small.GetComponent<SpriteRenderer>().sprite = StaticVariables.skin.smallCityArt;
        med.GetComponent<SpriteRenderer>().sprite = StaticVariables.skin.medCityArt;
        large.GetComponent<SpriteRenderer>().sprite = StaticVariables.skin.largeCityArt;
        huge.GetComponent<SpriteRenderer>().sprite = StaticVariables.skin.hugeCityArt;
        switch (size) {
            case 3: small.SetActive(true); break;
            case 4: med.SetActive(true); break;
            case 5: large.SetActive(true); break;
            case 6: huge.SetActive(true); break;
        }
    }

    private void hideNumberButtons() {
        numbers1to3.SetActive(false);
        numbers1to4.SetActive(false);
        numbers1to5.SetActive(false);
        numbers1to6.SetActive(false);
    }

    private void colorMenuButton() {
        /*
        InterfaceFunctions.colorPuzzleButton(menuButton);
        //menuButton.transform.Find("Button Image").Find("Borders").GetComponent<Image>().color = offButtonColorExterior;
        //menuButton.transform.Find("Button Image").Find("Interior").GetComponent<Image>().color = offButtonColorInterior;
        if (StaticVariables.isTutorial) {
            InterfaceFunctions.colorPuzzleButton(tutorialCanvas.transform.Find("Menu"));
            //tutorialCanvas.transform.Find("Menu").Find("Button Image").Find("Borders").GetComponent<Image>().color = offButtonColorExterior;
            //tutorialCanvas.transform.Find("Menu").Find("Button Image").Find("Interior").GetComponent<Image>().color = offButtonColorInterior;
        }
        */
        if (!StaticVariables.isTutorial) {
            InterfaceFunctions.colorPuzzleButton(menuButton);
        }
        
    }

    public void setNumberButtons() {
        hideNumberButtons();
        numberButtons = new GameObject[size];
        switch (size) {
            case 3:
                setNBs(numbers1to3);
                break;
            case 4:
                setNBs(numbers1to4);
                break;
            case 5:
                setNBs(numbers1to5);
                break;
            case 6:
                setNBs(numbers1to6);
                break;
        }
    }

    public void setTutorialNumberButtons() {
        numberButtons = new GameObject[3];
        numberButtons[0] = tutorialCanvas.transform.Find("Numbers").Find("1").gameObject;
        numberButtons[1] = tutorialCanvas.transform.Find("Numbers").Find("2").gameObject;
        numberButtons[2] = tutorialCanvas.transform.Find("Numbers").Find("3").gameObject;
        disselectNumber(numberButtons[0]);
        disselectNumber(numberButtons[1]);
        disselectNumber(numberButtons[2]);

    }

    private void setNBs(GameObject nB) {
        nB.SetActive(true);
        for (int i = 0; i<size; i++) {
            numberButtons[i] = nB.transform.GetChild(i).gameObject;
            disselectNumber(numberButtons[i]);
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
        if (includeNote1Btn) { note1Button.transform.Find("Icon").GetComponent<Image>().sprite = skin.note1Icon; }
        if (includeNote2Btn) { note2Button.transform.Find("Icon").GetComponent<Image>().sprite = skin.note2Icon; }

    }

    
    public void setUndoRedoButtons() {
        InterfaceFunctions.colorPuzzleButton(undoRedoButtons.transform.Find("Undo"));
        InterfaceFunctions.colorPuzzleButton(undoRedoButtons.transform.Find("Redo"));

        //undoRedoButtons.transform.Find("Undo").Find("Button Image").Find("Borders").GetComponent<Image>().color = offButtonColorExterior;
        //undoRedoButtons.transform.Find("Undo").Find("Button Image").Find("Interior").GetComponent<Image>().color = offButtonColorInterior;
        undoRedoButtons.transform.Find("Undo").Find("Icon").GetComponent<Image>().sprite = skin.undoIcon;
        //undoRedoButtons.transform.Find("Redo").Find("Button Image").Find("Borders").GetComponent<Image>().color = offButtonColorExterior;
        //undoRedoButtons.transform.Find("Redo").Find("Button Image").Find("Interior").GetComponent<Image>().color = offButtonColorInterior;
        undoRedoButtons.transform.Find("Redo").Find("Icon").GetComponent<Image>().sprite = skin.redoIcon;

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
                removeAllOfNumberButton = removeAllAndClearButtons.transform.Find("Remove All").gameObject;
                clearPuzzleButton = removeAllAndClearButtons.transform.Find("Clear Puzzle").gameObject;
                removeAllAndClearButtons.SetActive(true);
            }
            else if (StaticVariables.includeRemoveAllOfNumber) {
                onlyRemoveAllButton.SetActive(true);
            }
            else if (StaticVariables.includeClearPuzzle) {
                onlyClearButton.SetActive(true);
            }
        }

        InterfaceFunctions.colorPuzzleButton(removeAllOfNumberButton);
        InterfaceFunctions.colorPuzzleButton(clearPuzzleButton);
        //removeAllOfNumberButton.transform.Find("Button Image").Find("Borders").GetComponent<Image>().color = offButtonColorExterior;
        //removeAllOfNumberButton.transform.Find("Button Image").Find("Interior").GetComponent<Image>().color = offButtonColorInterior;
        //clearPuzzleButton.transform.Find("Button Image").Find("Borders").GetComponent<Image>().color = offButtonColorExterior;
        //clearPuzzleButton.transform.Find("Button Image").Find("Interior").GetComponent<Image>().color = offButtonColorInterior;
    }

    public void hidePositioningObjects() {
        puzzlePositioning.transform.Find("Image").gameObject.SetActive(false);
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
                puzzleGenerator.tilesArray[i, j].transform.Find("Building").gameObject.SetActive(showBuildings);
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

    public void switchNumber(int num) {
        selectedNumber = num;
    }

    public void hitBuildButton() {
        clickTileAction = "Apply Selected";
        disselectBuildAndNotes();
        if (includeNote1Btn || includeNote2Btn) {

            InterfaceFunctions.colorPuzzleButtonOn(buildButton);
            //buildButton.transform.Find("Button Image").Find("Interior").GetComponent<Image>().color = onButtonColorInterior;
            //buildButton.transform.Find("Button Image").Find("Borders").GetComponent<Image>().color = onButtonColorExterior;
        }
        updateRemoveSelectedNumber();

    }

    public void hitNote1Button() {
        clickTileAction = "Toggle Note 1";
        disselectBuildAndNotes();
        InterfaceFunctions.colorPuzzleButtonOn(note1Button);
        //note1Button.transform.Find("Button Image").Find("Interior").GetComponent<Image>().color = onButtonColorInterior;
        //note1Button.transform.Find("Button Image").Find("Borders").GetComponent<Image>().color = onButtonColorExterior;
        updateRemoveSelectedNumber();
    }

    public void hitNote2Button() {
        clickTileAction = "Toggle Note 2";
        disselectBuildAndNotes();

        InterfaceFunctions.colorPuzzleButtonOn(note2Button);
        //note2Button.transform.Find("Button Image").Find("Interior").GetComponent<Image>().color = onButtonColorInterior;
        //note2Button.transform.Find("Button Image").Find("Borders").GetComponent<Image>().color = onButtonColorExterior;
        updateRemoveSelectedNumber();
    }

    public void disselectBuildAndNotes() {
        if (includeNote1Btn || includeNote2Btn) {
            InterfaceFunctions.colorPuzzleButton(buildButton);
            //buildButton.transform.Find("Button Image").Find("Interior").GetComponent<Image>().color = offButtonColorInterior;
            //buildButton.transform.Find("Button Image").Find("Borders").GetComponent<Image>().color = offButtonColorExterior;
        }
        if (includeNote1Btn) {
            InterfaceFunctions.colorPuzzleButton(note1Button);
            //note1Button.transform.Find("Button Image").Find("Interior").GetComponent<Image>().color = offButtonColorInterior;
            //note1Button.transform.Find("Button Image").Find("Borders").GetComponent<Image>().color = offButtonColorExterior;
        }
        if (includeNote2Btn) {
            InterfaceFunctions.colorPuzzleButton(note2Button);
            //note2Button.transform.Find("Button Image").Find("Interior").GetComponent<Image>().color = offButtonColorInterior;
            //note2Button.transform.Find("Button Image").Find("Borders").GetComponent<Image>().color = offButtonColorExterior;
        }
    }

    public void goToMainMenu() {
        if (!StaticVariables.isFading) {
            if (!hasWonYet) { savePuzzleStates(); }
            StaticVariables.fadingTo = "menu";
            startFadeOut();
        }
    }

    public void savePuzzleStates() {
        StaticVariables.hasSavedPuzzleState = true;

        StaticVariables.previousPuzzleStates = previousPuzzleStates;
        StaticVariables.currentPuzzleState = currentPuzzleState;
        StaticVariables.nextPuzzleStates = nextPuzzleStates;

        StaticVariables.puzzleSolution = puzzleGenerator.makeSolutionString();
        StaticVariables.savedPuzzleSize = size;
    }

    public void loadPuzzleStates() {
        StaticVariables.hasSavedPuzzleState = false;

        previousPuzzleStates = StaticVariables.previousPuzzleStates;
        currentPuzzleState = StaticVariables.currentPuzzleState;
        nextPuzzleStates = StaticVariables.nextPuzzleStates;

        currentPuzzleState.restorePuzzleState(puzzleGenerator);

        //load the actual puzzle solution
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
    }
    public void showNumberButtonClicked(GameObject nB) {
        if (prevClickedNumButton != null) {
            disselectNumber(prevClickedNumButton);
        }
        prevClickedNumButton = nB;
        selectNumber(nB);
        updateRemoveSelectedNumber();
    }
    private void selectNumber(GameObject btn) {

        InterfaceFunctions.colorPuzzleButtonOn(btn);
        //btn.transform.Find("Button Image").Find("Borders").GetComponent<Image>().color = onButtonColorExterior;
        //btn.transform.Find("Button Image").Find("Interior").GetComponent<Image>().color = onButtonColorInterior;
    }

    private void disselectNumber(GameObject btn) {
        InterfaceFunctions.colorPuzzleButton(btn);
        //btn.transform.Find("Button Image").Find("Borders").GetComponent<Image>().color = offButtonColorExterior;
        //btn.transform.Find("Button Image").Find("Interior").GetComponent<Image>().color = offButtonColorInterior;
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
        float borderScale = puzzlePositioning.transform.localScale.x / originalPuzzleScale;
        Vector3 s = redBorder.transform.localScale;
        s *= borderScale;
        redBorder.transform.localScale = s;
        
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

    public void pushNumberButton(int num) {
        switchNumber(num);
        showNumberButtonClicked(numberButtons[num - 1]);
    }

    public void pushTutorialNumberButton(int num) {
        switchNumber(num);
        showNumberButtonClicked(numberButtons[num - 1]);
        if (StaticVariables.isTutorial) {
            tutorialManager.tappedNumberButton(num);
        }
    }

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

    public void updateRemoveSelectedNumber() {
        if (!StaticVariables.isTutorial && StaticVariables.includeRemoveAllOfNumber) {
            
            removeAllOfNumberButton.transform.Find("Number").GetComponent<SpriteRenderer>().sprite = numberSprites[selectedNumber];
            Color buildingColor;
            Color note1Color;
            Color note2Color;
            ColorUtility.TryParseHtmlString(StaticVariables.whiteHex, out buildingColor);
            ColorUtility.TryParseHtmlString(skin.note1Color, out note1Color);
            ColorUtility.TryParseHtmlString(skin.note2Color, out note2Color);
            Color c = note1Color;
            if (clickTileAction == "Apply Selected" || selectedNumber == 0) {
                c = buildingColor;
            }
            else if (clickTileAction == "Toggle Note 1") {
                c = note1Color;
            }
            else if (clickTileAction == "Toggle Note 2") {
                c = note2Color;
            }
            removeAllOfNumberButton.transform.Find("Number").GetComponent<SpriteRenderer>().color = c;
        }
    }

}