//for Cityscapes, copyright Cole Hilscher 2020

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    //controls the puzzle canvas and gameplay. Only one is used, and only on the InPuzzle scene.

    //variables used to store what the player is trying to do
    [HideInInspector]
    public string clickTileAction = "Apply Selected"; //what happens when you click a building tile - either toggle a building, toggle a note1, toggle a note2, or clear the tile
    [HideInInspector]
    public int selectedNumber = 0; //the number to place, either as a building or note
    private GameObject prevClickedNumButton; //when the player clicks the erase button, the selected number button is disselected. When they click any other tool, the previous number is selected again


    //variables used in setting up the game
    [HideInInspector]
    public int size; //puzzle size
    [HideInInspector]
    public bool includeNote1Btn;
    [HideInInspector]
    public bool includeNote2Btn;
    public PuzzleGenerator puzzleGenerator; //the PuzzleGenerator object that will create the puzzle's solution
    public GameObject puzzlePositioning; //determines where the puzzle is going to go
    public GameObject puzzlePositioningImage;
    public GameObject selectionModeButtons0;
    public GameObject selectionModeButtons1;
    public GameObject selectionModeButtons2;
    public GameObject selectionModeButtons3;
    private float originalPuzzleScale;

    //game canvases
    public GameObject canvas;
    public GameObject winParent; //the canvas that is used when the player beats a puzzle
    public GameObject puzzleParent;
    public GameObject tutorialParent;

    //the tools that the player can choose from
    private GameObject buildButton;
    private GameObject note1Button;
    private GameObject note2Button;
    private GameObject clearTileButton;
    public GameObject undoRedoButtons;
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
    public GameObject menuButton;
    
    //variables used in the tutorial
    public TutorialManager tutorialManager;
    public GameObject screenTappedMonitor;
    public GameObject tutorialTextBox;
    public GameObject redStreetBorder;
    public Skin basicSkin; //the basic skin, used for loading the tutorial with the basic skin no matter what skin is currently selected


    //variables used in screen fade-in and fade-out
    public GameObject blackForeground; //used to transition to/from the main menu
    [HideInInspector]
    public Image blackSprite;
    public float fadeOutTime;
    public float fadeInTime;
    private float fadeTimer;

    //UI elements
    public GameObject streetCorner;
    public Image winBlackSprite;
    private bool fadingToWin = false;
    private float fadingToWinTimer;
    public float fadeToWinTime;
    public float fadeToWinDarknessRatio;
    private float winPopupScale;
    public Sprite[] numberSprites;

    //properties of the current skin
    public Skin skin;
    private Color winBackgroundColorInterior;
    private Color winBackgroundColorExterior;
    public GameObject puzzleBackground;

    //variables used in determining a win, and handling the win pop-up
    public int coinsFor3Win = 3;
    public int coinsFor4Win = 5;
    public int coinsFor5Win = 10;
    public int coinsFor6Win = 20;
    [HideInInspector]
    public bool canClick = true;
    private bool hasWonYet = false;
    public GameObject coinsBox1s;
    public GameObject coinsBox10s;
    public GameObject totalCoinsBox1s;
    public GameObject totalCoinsBox10s;
    public GameObject totalCoinsBox100s;
    public GameObject totalCoinsBox1000s;
    public GameObject totalCoinsBox10000s;


    //storing puzzle states
    private List<PuzzleState> previousPuzzleStates = new List<PuzzleState>(); // the list of puzzle states to be restored by the undo button
    private PuzzleState currentPuzzleState;
    private List<PuzzleState> nextPuzzleStates = new List<PuzzleState>();// the list of puzzle states to be restored by the redo button
    
    public bool showBuildings;
    
    private void Start() {
        //choose which skin to use
        skin = StaticVariables.skin;
        if (StaticVariables.isTutorial)
            skin = basicSkin;
        //set up the fade-in timer
        if (StaticVariables.isFading && StaticVariables.fadingTo == "puzzle")
            fadeTimer = fadeInTime;
        blackSprite = blackForeground.GetComponent<Image>();
        winPopupScale = winParent.transform.GetChild(1).localScale.x;

        size = StaticVariables.size;
        includeNote1Btn = StaticVariables.includeNotes1Button;
        includeNote2Btn = StaticVariables.includeNotes2Button;
        
        ColorUtility.TryParseHtmlString(skin.mainMenuButtonExterior, out winBackgroundColorExterior);
        ColorUtility.TryParseHtmlString(skin.mainMenuButtonInterior, out winBackgroundColorInterior);

        hideNumberButtons();

        colorMenuButton();

        if (StaticVariables.isTutorial) { //set up the tutorial. uses tutorialmanager
            originalPuzzleScale = puzzlePositioning.transform.localScale.x;
            setTutorialNumberButtons();
            tutorialParent.SetActive(true);
            puzzleParent.SetActive(false);
            //hide menu button if it is your first time playing the tutorial
            tutorialParent.transform.Find("Menu").gameObject.SetActive(StaticVariables.hasBeatenTutorial);

            tutorialManager = new TutorialManager();
            tutorialManager.gameManager = this;
            tutorialManager.startTutorial();
        }

        else if (!StaticVariables.isTutorial) { //continue with the puzzle generation and setup
            tutorialParent.SetActive(false);
            puzzleParent.SetActive(true); 
            //load a puzzle if you already have one saved, otherwise generate a new one based on the size
            if (StaticVariables.hasSavedPuzzleState) {
                puzzleGenerator.restoreSavedPuzzle(StaticVariables.puzzleSolution, size);
                loadPuzzleStates();
            }
            else {
                puzzleGenerator.createPuzzle(size);
                selectedNumber = size; //you automatically start with the highest building size selected
                clickTileAction = "Apply Selected";
            }
            //set up the visuals of the screen based on the puzzle size and what tools you have unlocked
            drawFullPuzzle();
            setRemoveAllAndClearButtons();
            setNumberButtons();
            highlightSelectedNumber(); 
            if (clickTileAction == "Clear Tile") { disselectNumber(prevClickedNumButton); }

            hidePositioningObjects();
            setSelectionModeButtons();
            setUndoRedoButtons();
            highlightBuildType();
            //apply the current skin
            puzzleBackground.GetComponent<Image>().sprite = skin.puzzleBackground;
            InterfaceFunctions.colorPuzzleButton(winParent.transform.Find("Win Popup").Find("Menu"));
            InterfaceFunctions.colorPuzzleButton(winParent.transform.Find("Win Popup").Find("Another Puzzle"));
            InterfaceFunctions.colorPuzzleButton(winParent.transform.Find("Win Popup").Find("Shop"));
            winParent.transform.Find("Win Popup").Find("Win Popup Background Exterior").GetComponent<Image>().color = winBackgroundColorExterior;
            winParent.transform.Find("Win Popup").Find("Win Popup Background Interior").GetComponent<Image>().color = winBackgroundColorInterior;
            //set up the win popup process
            Color c = winParent.transform.Find("Black Background").GetComponent<Image>().color;
            c.a = fadeToWinDarknessRatio;
            winParent.transform.Find("Black Background").GetComponent<Image>().color = c;
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

            coinsBox1s.GetComponent<Image>().sprite = numberSprites[onesDigit];
            coinsBox10s.GetComponent<Image>().sprite = numberSprites[tensDigit];

            //set the first puzzle state, and you are ready to start playing!
            currentPuzzleState = new PuzzleState(puzzleGenerator);
        }
    }

    private void Update() {
        //set the color of each resident, specifically if their viewpoint is satisfied
        foreach(SideHintTile h in puzzleGenerator.allHints) {
            h.setAppropriateColor();
        }
        //check for a win
        if (!hasWonYet && puzzleGenerator.checkPuzzle() && !StaticVariables.isTutorial) {
            hasWonYet = true;
            
            winParent.SetActive(true);
            canClick = false;
            incrementCoinsForWin();

            fadingToWin = true;
            fadingToWinTimer = fadeToWinTime;

            StaticVariables.hasSavedPuzzleState = false;
            save();
        }

        //if the player is fading out of their current puzzle and into another puzzle of the same size, run this block
        //that option is accessible from the win pop-up screen
        if (StaticVariables.isFading && StaticVariables.fadingTo == "puzzle" && StaticVariables.fadingFrom == "puzzle") {
            if (StaticVariables.fadingIntoPuzzleSameSize) {
                fadeTimer -= Time.deltaTime;
                Color c = blackSprite.color;
                c.a = (fadeTimer) / fadeInTime;
                blackSprite.color = c;
                if (fadeTimer <= 0f) {
                    StaticVariables.isFading = false;

                    if (StaticVariables.waitingOnButtonClickAfterFadeIn) {
                        StaticVariables.waitingOnButtonClickAfterFadeIn = false;
                        if (StaticVariables.buttonClickInWaiting.Contains("menu")) {
                            goToMainMenu();
                        }
                    }
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

        //if the player is fading from the main menu to the puzzle scene, run this code block
        else if (StaticVariables.isFading && StaticVariables.fadingTo == "puzzle") {
            fadeTimer -= Time.deltaTime;
            Color c = blackSprite.color;
            c.a = (fadeTimer) / fadeInTime;
            blackSprite.color = c;
            if (fadeTimer <= 0f) {
                StaticVariables.isFading = false;

                if (StaticVariables.waitingOnButtonClickAfterFadeIn) {
                    StaticVariables.waitingOnButtonClickAfterFadeIn = false;
                    if (StaticVariables.buttonClickInWaiting.Contains("menu")) {
                        goToMainMenu();
                    }
                }
            }
        }

        //if the player is fading from the puzzle scene and into the shop or the main menu, run this block
        else if (StaticVariables.isFading && StaticVariables.fadingFrom == "puzzle") {
            fadeTimer -= Time.deltaTime;
            Color c = blackSprite.color;
            c.a = (fadeOutTime - fadeTimer) / fadeOutTime;
            blackSprite.color = c;
            if (fadeTimer <= 0f) {
                if (StaticVariables.fadingTo == "menu") {
                    SceneManager.LoadScene("MainMenu");
                }
                if (StaticVariables.fadingTo == "shop") {
                    SceneManager.LoadScene("Shop");
                }

            }
        }

        //if the player has just won, the win popup needs to appear and fade in
        if (fadingToWin) {
            fadingToWinTimer -= Time.deltaTime;
            if (fadingToWinTimer < 0f) { fadingToWinTimer = 0f; }
            float fadePercent = 1 - (fadingToWinTimer / fadeToWinTime); // goes from 0 to 1 as time progresses

            Color c = winBlackSprite.color;
            c.a = fadePercent * fadeToWinDarknessRatio;
            winBlackSprite.color = c;

            float scale = fadePercent * winPopupScale;

            winParent.transform.GetChild(1).localScale = new Vector3(scale, scale, scale);

            if (fadingToWinTimer <= 0f) {
                fadingToWin = false;
            }
        }

        //if the player pushes the back button on their phone, go to the main menu. Identical to pushing the "Menu" button on-screen
        if (Input.GetKeyDown(KeyCode.Escape)) {
            goToMainMenu();
        }
    }

    // ---------------------------------------------------
    //ALL OF THE FUNCTIONS THAT ARE USED TO SET UP THE PUZZLE SCENE, BEFORE PLAYING
    // ---------------------------------------------------

    private void hideNumberButtons() {
        //hide all number buttons, used at the start of the puzzle. later in setNumberButtons, the correct numbers are turned back on.
        numbers1to3.SetActive(false);
        numbers1to4.SetActive(false);
        numbers1to5.SetActive(false);
        numbers1to6.SetActive(false);
    }

    private void colorMenuButton() {
        //apply the current skin colors to the menu button
        if (!StaticVariables.isTutorial) {
            InterfaceFunctions.colorPuzzleButton(menuButton);
        }

    }

    public void setNumberButtons() {
        //show the correct number buttons depending on the puzzle size, and hide the rest
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

    private void setNBs(GameObject nB) {
        //set the number button gameObjects
        nB.SetActive(true);
        for (int i = 0; i < size; i++) {
            numberButtons[i] = nB.transform.GetChild(i).gameObject;
            disselectNumber(numberButtons[i]);
        }
    }
    
    public void setSelectionModeButtons() {
        //show the appropriate selection mode buttons. Those are build, note1, note2, and erase
        if (includeNote2Btn && includeNote1Btn) {
            selectionModeButtons0.SetActive(false);
            selectionModeButtons1.SetActive(true);
            selectionModeButtons2.SetActive(false);
            selectionModeButtons3.SetActive(false);
            buildButton = selectionModeButtons1.transform.GetChild(0).gameObject;
            note1Button = selectionModeButtons1.transform.GetChild(1).gameObject;
            note2Button = selectionModeButtons1.transform.GetChild(2).gameObject;
            clearTileButton = selectionModeButtons1.transform.Find("Clear").gameObject;
        }
        else if (!includeNote2Btn && !includeNote1Btn) {
            selectionModeButtons0.SetActive(true);
            selectionModeButtons1.SetActive(false);
            selectionModeButtons2.SetActive(false);
            selectionModeButtons3.SetActive(false);
            buildButton = selectionModeButtons0.transform.Find("Build").gameObject;
            clearTileButton = selectionModeButtons0.transform.Find("Clear").gameObject;
        }
        else if (!includeNote2Btn && includeNote1Btn) {
            selectionModeButtons0.SetActive(false);
            selectionModeButtons1.SetActive(false);
            selectionModeButtons2.SetActive(true);
            selectionModeButtons3.SetActive(false);
            buildButton = selectionModeButtons2.transform.GetChild(0).gameObject;
            note1Button = selectionModeButtons2.transform.GetChild(1).gameObject;
            clearTileButton = selectionModeButtons2.transform.Find("Clear").gameObject;
        }
        else if (includeNote2Btn && !includeNote1Btn) {
            selectionModeButtons0.SetActive(false);
            selectionModeButtons1.SetActive(false);
            selectionModeButtons2.SetActive(false);
            selectionModeButtons3.SetActive(true);
            buildButton = selectionModeButtons3.transform.GetChild(0).gameObject;
            note2Button = selectionModeButtons3.transform.GetChild(1).gameObject;
            clearTileButton = selectionModeButtons3.transform.Find("Clear").gameObject;
        }
    }
    
    public void setUndoRedoButtons() {
        //show the undo and redo buttons if they are toggled on
        InterfaceFunctions.colorPuzzleButton(undoRedoButtons.transform.Find("Undo"));
        InterfaceFunctions.colorPuzzleButton(undoRedoButtons.transform.Find("Redo"));
        undoRedoButtons.SetActive(StaticVariables.includeUndoRedo);
    }

    public void setRemoveAllAndClearButtons() {
        //show the appropriate remove-all and clear buttons. You can have either one, or both turned on
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
    }

    public void hidePositioningObjects() {
        //hide the puzzle positioning before the game starts
        //puzzlePositioning.transform.Find("Image").gameObject.SetActive(false);
    }

    public void drawFullPuzzle() {
        //draws the visuals for the puzzle tiles, borders, residents, and street corners
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

    private void createCorner(Vector2 p, float scale, int rot, Transform parent) {
        //add a street corner to the puzzle display. They serve no mechanical purpose and are just there to look nice
        GameObject corner = Instantiate(streetCorner);
        corner.transform.position = p;
        corner.transform.localScale *= scale;
        corner.transform.Rotate(new Vector3(0, 0, rot));
        corner.transform.SetParent(parent);


        corner.GetComponent<Image>().color = InterfaceFunctions.getColorFromString(skin.streetColor);


        Vector3 pos = corner.transform.localPosition;
        pos.z = 0;
        corner.transform.localPosition = pos;
    }

    // ---------------------------------------------------
    //ALL OF THE FUNCTIONS THAT INTERACT WITH AND SET UP THE WIN POPUP
    // ---------------------------------------------------

    private void showCorrectCityArtOnWinScreen() {
        //shows the "city art" on the win popup, depending on which skin and which city size the player is using
        GameObject small = winParent.transform.Find("Win Popup").Find("City Art - Small").gameObject;
        GameObject med = winParent.transform.Find("Win Popup").Find("City Art - Medium").gameObject;
        GameObject large = winParent.transform.Find("Win Popup").Find("City Art - Large").gameObject;
        GameObject huge = winParent.transform.Find("Win Popup").Find("City Art - Huge").gameObject;
        small.SetActive(false);
        med.SetActive(false);
        large.SetActive(false);
        huge.SetActive(false);
        small.GetComponent<Image>().sprite = StaticVariables.skin.smallCityArt;
        med.GetComponent<Image>().sprite = StaticVariables.skin.medCityArt;
        large.GetComponent<Image>().sprite = StaticVariables.skin.largeCityArt;
        huge.GetComponent<Image>().sprite = StaticVariables.skin.hugeCityArt;
        switch (size) {
            case 3: small.SetActive(true); break;
            case 4: med.SetActive(true); break;
            case 5: large.SetActive(true); break;
            case 6: huge.SetActive(true); break;
        }
    }

    public void displayTotalCoinsAmount() {
        //show the player's total coins on the win popup screen
        int value1s = StaticVariables.coins % 10;
        int value10s = (StaticVariables.coins / 10) % 10;
        int value100s = (StaticVariables.coins / 100) % 10;
        int value1000s = (StaticVariables.coins / 1000) % 10;
        int value10000s = (StaticVariables.coins / 10000) % 10;
        totalCoinsBox1s.GetComponent<Image>().sprite = numberSprites[value1s];
        totalCoinsBox10s.GetComponent<Image>().sprite = numberSprites[value10s];
        totalCoinsBox100s.GetComponent<Image>().sprite = numberSprites[value100s];
        totalCoinsBox1000s.GetComponent<Image>().sprite = numberSprites[value1000s];
        totalCoinsBox10000s.GetComponent<Image>().sprite = numberSprites[value10000s];

        totalCoinsBox1s.SetActive(true);
        totalCoinsBox10s.SetActive(true);
        totalCoinsBox100s.SetActive(true);
        totalCoinsBox1000s.SetActive(true);
        totalCoinsBox10000s.SetActive(true);

        if (value10000s == 0) {
            totalCoinsBox10000s.SetActive(false);
            if (value1000s == 0) {
                totalCoinsBox1000s.SetActive(false);
                if (value100s == 0) {
                    totalCoinsBox100s.SetActive(false);
                    if (value10s == 0) {
                        totalCoinsBox10s.SetActive(false);
                    }
                }
            }
        }

        // shift the coins images over so that they are next to the text
        int shiftAmount = 0;
        if (!totalCoinsBox10000s.activeSelf) { shiftAmount++; }
        if (!totalCoinsBox1000s.activeSelf) { shiftAmount++; }
        if (!totalCoinsBox100s.activeSelf) { shiftAmount++; }
        if (!totalCoinsBox10s.activeSelf) { shiftAmount++; }

        float shiftPer = totalCoinsBox10000s.transform.position.x - totalCoinsBox1000s.transform.position.x;
        float totalShift = shiftAmount * shiftPer;

        Transform parentTransform = totalCoinsBox1s.transform.parent;
        Vector3 pos = parentTransform.position;
        pos.x += totalShift;
        parentTransform.position = pos;

        //shift the coins image and associated text over so they are centered
        Transform parent2Transform = parentTransform.parent;
        pos = parent2Transform.position;
        pos.x -= totalShift / 2;
        parent2Transform.position = pos;
    }


    private void incrementCoinsForWin() {
        //when the player wins the puzzle, add to their coin total, and show the new amount on the win popup
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

        displayTotalCoinsAmount();
    }

    // ---------------------------------------------------
    //ALL OF THE FUNCTIONS THAT ARE USED BY THE TUTORIAL MANAGER
    // ---------------------------------------------------

    public void setTutorialNumberButtons() {
        //show the number buttons, specifically used for the tutorial
        numberButtons = new GameObject[3];
        numberButtons[0] = tutorialParent.transform.Find("Numbers").Find("1").gameObject;
        numberButtons[1] = tutorialParent.transform.Find("Numbers").Find("2").gameObject;
        numberButtons[2] = tutorialParent.transform.Find("Numbers").Find("3").gameObject;
        disselectNumber(numberButtons[0]);
        disselectNumber(numberButtons[1]);
        disselectNumber(numberButtons[2]);

    }

    public void tappedScreen() {
        //used to advance the tutorial
        tutorialManager.tappedScreen();
    }

    public void hideHints() {
        //hide the side hints (residents), used in the tutorial
        foreach (SideHintTile h in puzzleGenerator.allHints) {

            h.GetComponent<Transform>().GetChild(1).gameObject.SetActive(false);
            h.GetComponent<Transform>().GetChild(2).gameObject.SetActive(false);
        }
    }

    public void showHints() {
        //show the side hints (residents), used in the tutorial
        foreach (SideHintTile h in puzzleGenerator.allHints) {

            h.GetComponent<Transform>().GetChild(1).gameObject.SetActive(true);
            h.GetComponent<Transform>().GetChild(2).gameObject.SetActive(true);
        }
    }

    public void addRedStreetBorderForTutorial(Vector3 centerPoint, int rotation) {
        //add a border around a whole street, used in the tutorial
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
        //remove all borders around streets, used in the tutorial
        GameObject[] RedBorders = GameObject.FindGameObjectsWithTag("Red Border");
        foreach (GameObject r in RedBorders) {
            Destroy(r);
        }
    }

    public void deleteCityForTutorial() {
        //used in the tutorial to advance from the first pre-defined puzzle to the second one
        GameObject[] oldPuzzleTiles = GameObject.FindGameObjectsWithTag("Puzzle Tile");
        foreach (GameObject t in oldPuzzleTiles) {
            Destroy(t);
        }
        GameObject[] oldSideTiles = GameObject.FindGameObjectsWithTag("Side Tile");
        foreach (GameObject t in oldSideTiles) {
            Destroy(t);
        }
    }

    public void pushTutorialNumberButton(int num) {
        //when the player pushes a number button on the tutorial, select it, and maybe advance some tutorial dialogue
        switchNumber(num);
        showNumberButtonClicked(numberButtons[num - 1]);
        if (StaticVariables.isTutorial) {
            tutorialManager.tappedNumberButton(num);
        }
    }


    // ---------------------------------------------------
    //ALL OF THE FUNCTIONS THAT ARE CALLED WHEN THE PLAYER INTERACTS WITH THE PUZZLE
    // ---------------------------------------------------
    
    public void switchNumber(int num) {
        //change the selected number
        selectedNumber = num;
    }

    public void hitBuildButton() {
        //choose the build selection mode
        if (clickTileAction == "Clear Tile") {
            selectNumber(prevClickedNumButton);
            foreach (PuzzleTile t in puzzleGenerator.puzzleTiles) { t.highlightIfBuildingNumber(selectedNumber); }
        }
        clickTileAction = "Apply Selected";
        disselectBuildAndNotes();
        InterfaceFunctions.colorPuzzleButtonOn(buildButton);
        updateRemoveSelectedNumber();
        save();
    }

    public void hitNote1Button() {
        //choose the note 1 selection mode
        if (clickTileAction == "Clear Tile") {
            selectNumber(prevClickedNumButton);
            foreach (PuzzleTile t in puzzleGenerator.puzzleTiles) { t.highlightIfBuildingNumber(selectedNumber); }
        }
        clickTileAction = "Toggle Note 1";
        disselectBuildAndNotes();
        InterfaceFunctions.colorPuzzleButtonOn(note1Button);
        updateRemoveSelectedNumber();
        save();
    }

    public void hitNote2Button() {
        //choose the note 2 selection mode
        if (clickTileAction == "Clear Tile") {
            selectNumber(prevClickedNumButton);
            foreach (PuzzleTile t in puzzleGenerator.puzzleTiles) { t.highlightIfBuildingNumber(selectedNumber); }
        }
        clickTileAction = "Toggle Note 2";
        disselectBuildAndNotes();

        InterfaceFunctions.colorPuzzleButtonOn(note2Button);
        updateRemoveSelectedNumber();
        save();
    }
    
    public void hitClearButton() {
        //choose the clear tile selection mode
        clickTileAction = "Clear Tile";
        disselectNumber(prevClickedNumButton);
        disselectBuildAndNotes();
        foreach (PuzzleTile t in puzzleGenerator.puzzleTiles) { t.unhighlightBuildingNumber(); }
        InterfaceFunctions.colorPuzzleButtonOn(clearTileButton);
        updateRemoveSelectedNumber();
        save();
    }

    public void disselectBuildAndNotes() {
        //select the clear tile button and disselect the others
        InterfaceFunctions.colorPuzzleButton(buildButton);
        if (includeNote1Btn) {
            InterfaceFunctions.colorPuzzleButton(note1Button);
        }
        if (includeNote2Btn) {
            InterfaceFunctions.colorPuzzleButton(note2Button);
        }
        InterfaceFunctions.colorPuzzleButton(clearTileButton);
    }
    
    public void showNumberButtonClicked(GameObject nB) {
        //when the player clicks a number button, highlight that number, and also highlight all copies of that number in the puzzle (if the player has the highlightBuildings upgrade)
        if (prevClickedNumButton != null) {
            disselectNumber(prevClickedNumButton);
        }
        prevClickedNumButton = nB;
        selectNumber(nB);
        updateRemoveSelectedNumber();

        if (!StaticVariables.isTutorial && StaticVariables.includeHighlightBuildings) {
            foreach (PuzzleTile t in puzzleGenerator.puzzleTiles) { t.highlightIfBuildingNumber(selectedNumber); }
        }
    }

    private void selectNumber(GameObject btn) {
        //color chosen the number button to the "on" coloration from the current skin
        InterfaceFunctions.colorPuzzleButtonOn(btn, skin);
    }

    private void disselectNumber(GameObject btn) {
        //color chosen the number button to the "off" coloration from the current skin
        InterfaceFunctions.colorPuzzleButton(btn, skin);
    }
    
    public void addToPuzzleHistory() {
        //add the current puzzle state to the list of states when the player makes a move
        //used in the undo/redo process
        //also clears the list of "next puzzle states", so the player can't "redo" to get to those states any more
        previousPuzzleStates.Add(currentPuzzleState);
        PuzzleState currentState = new PuzzleState(puzzleGenerator);
        currentPuzzleState = currentState;
        if (nextPuzzleStates.Count > 0) {
            nextPuzzleStates = new List<PuzzleState>();
        }
        save();
    }

    public void pushUndoButton() {
        //returns the player to a previous puzzle state, and moves the most recent puzzle state to the list of "next puzzle states"
        //used in the undo/redo process
        if (previousPuzzleStates.Count > 0) {
            nextPuzzleStates.Add(currentPuzzleState);

            PuzzleState currentState = previousPuzzleStates[previousPuzzleStates.Count - 1];


            currentState.restorePuzzleState(puzzleGenerator);

            currentPuzzleState = currentState;
            previousPuzzleStates.RemoveAt(previousPuzzleStates.Count - 1);
            save();
        }
    }

    public void pushRedoButton() {
        //returns the player to a "next" puzzle state, and moves the most recent state to the list of "previous" states
        //used in the undo/redo process
        if (nextPuzzleStates.Count > 0) {
            previousPuzzleStates.Add(currentPuzzleState);
            PuzzleState currentState = nextPuzzleStates[nextPuzzleStates.Count - 1];
            currentState.restorePuzzleState(puzzleGenerator);
            currentPuzzleState = currentState;
            nextPuzzleStates.RemoveAt(nextPuzzleStates.Count - 1);
            save();
        }
    }

    public void pushNumberButton(int num) {
        //when the player taps a number selection button, highlight it and make it the currently-selected number
        switchNumber(num);
        showNumberButtonClicked(numberButtons[num - 1]);
        if (clickTileAction == "Clear Tile") {
            clickTileAction = "Apply Selected";
            highlightBuildType();
            updateRemoveSelectedNumber();
        }
        save();
    }

    public void highlightSelectedNumber() {
        //highlight the currently-selected number button
        showNumberButtonClicked(numberButtons[selectedNumber - 1]);
    }

    public void highlightBuildType() {
        //if the build type is set to be included, then highlight it. otherwise, highlight the build button
        disselectBuildAndNotes();

        GameObject button;
        if (clickTileAction == "Toggle Note 1" && includeNote1Btn) {
            button = note1Button;
        }
        else if (clickTileAction == "Toggle Note 2" && includeNote2Btn) {
            button = note2Button;
        }
        else if (clickTileAction == "Clear Tile") {
            button = clearTileButton;
        }
        else {
            clickTileAction = "Apply Selected";
            button = buildButton;
        }
        InterfaceFunctions.colorPuzzleButtonOn(button);
    }
    
    public void pushRemoveAllNumbersButton() {
        //clear the entire puzzle of aall of the currently-selected number of the currently-selected type.
        //for example, when #3 and "red notes" are selected, this function clears all red 3's from the puzzle
        if (clickTileAction != "Clear Tile") {
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
                foreach (PuzzleTile t in puzzleGenerator.puzzleTiles) {
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
    }
    
    public void pushClearPuzzleButton() {
        //removes all buildings and notes of all types from the puzzle
        bool changedAnything = false;
        foreach (PuzzleTile t in puzzleGenerator.puzzleTiles) {
            if (t.doesTileContainAnything()) { changedAnything = true; }
            t.clearColoredNotes();
            t.removeNumberFromTile();

        }
        if (changedAnything) { addToPuzzleHistory(); }
    }
    
    public void updateRemoveSelectedNumber() {
        //when the player changes their tool type or their selecred number, update the color and number displayed on the "remove all of type" button
        if (!StaticVariables.isTutorial && StaticVariables.includeRemoveAllOfNumber) {
            removeAllOfNumberButton.transform.Find("Dash").gameObject.SetActive(false);
            removeAllOfNumberButton.transform.Find("Number").gameObject.SetActive(true);
            removeAllOfNumberButton.transform.Find("Number").GetComponent<Image>().sprite = numberSprites[selectedNumber];
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
            removeAllOfNumberButton.transform.Find("Number").GetComponent<Image>().color = c;

            if (clickTileAction == "Clear Tile") {
                removeAllOfNumberButton.transform.Find("Dash").gameObject.SetActive(true);
                removeAllOfNumberButton.transform.Find("Number").gameObject.SetActive(false);

            }
        }
    }

    // ---------------------------------------------------
    //ALL OF THE FUNCTIONS THAT INVOLVE TRANSITIONING BETWEEN SCENES, FADING OUT OF A SCENE, OR SAVING/LOADING THE GAME
    // ---------------------------------------------------
    
    public void goToMainMenu() {
        //start the fading process to return to the main menu in the middle of a puzzle
        if (!hasWonYet) {
            if (!StaticVariables.isFading) {
                save();
                StaticVariables.fadingTo = "menu";
                startFadeOut();
            }
            else {
                StaticVariables.waitingOnButtonClickAfterFadeIn = true;
                StaticVariables.buttonClickInWaiting = "menu";
            }
        }
    }

    public void goToMainMenuWinPopup() {
        //start the fading process to return to the main menu after beating the puzzle
        if (!StaticVariables.isFading) {
            save();
            StaticVariables.fadingTo = "menu";
            startFadeOut();
        }
        else {
            StaticVariables.waitingOnButtonClickAfterFadeIn = true;
            StaticVariables.buttonClickInWaiting = "menu";
        }
    }

    public void goToShop() {
        //start the fading process to go to the shop after beating the puzzle
        if (!StaticVariables.isFading) {
            save();
            StaticVariables.fadingTo = "shop";
            startFadeOut();
        }
        else {
            StaticVariables.waitingOnButtonClickAfterFadeIn = true;
            StaticVariables.buttonClickInWaiting = "shop";
        }
    }

    public void save() {
        //save the puzzle state and save the game
        if (!StaticVariables.isTutorial) {
            savePuzzleStates();
            SaveSystem.SaveGame();
        }

    }

    public void savePuzzleStates() {
        //store all of the puzzle state objects, and the currently selected build and tool options in StaticVariables. Used when the player leaves the puzzle partway through
        bool hasAnythingHappenedYet = false;
        if (previousPuzzleStates.Count > 0) { hasAnythingHappenedYet = true; }
        if (nextPuzzleStates.Count > 0) { hasAnythingHappenedYet = true; }
        if (hasAnythingHappenedYet) {
            StaticVariables.hasSavedPuzzleState = !hasWonYet;

            StaticVariables.previousPuzzleStates = previousPuzzleStates;
            StaticVariables.currentPuzzleState = currentPuzzleState;
            StaticVariables.nextPuzzleStates = nextPuzzleStates;

            StaticVariables.puzzleSolution = puzzleGenerator.makeSolutionString();
            StaticVariables.savedPuzzleSize = size;

            StaticVariables.savedBuildNumber = selectedNumber;
            StaticVariables.savedBuildType = clickTileAction;
        }
        else {
            StaticVariables.hasSavedPuzzleState = false;
        }
    }

    public void loadPuzzleStates() {
        //load all of the puzzle state objects, and the selected build and tool options from StaticVariables. Used when the player returns to their puzzle from the main menu
        StaticVariables.hasSavedPuzzleState = false;

        previousPuzzleStates = StaticVariables.previousPuzzleStates;
        currentPuzzleState = StaticVariables.currentPuzzleState;
        nextPuzzleStates = StaticVariables.nextPuzzleStates;

        currentPuzzleState.restorePuzzleState(puzzleGenerator);

        selectedNumber = StaticVariables.savedBuildNumber;
        clickTileAction = StaticVariables.savedBuildType;
    }

    public void startFadeOut() {
        //start the fade-out process to go to another scene
        fadeTimer = fadeOutTime;
        StaticVariables.isFading = true;
        StaticVariables.fadingFrom = "puzzle";
    }

    public void generateNewPuzzleSameSize() {
        //start the fade-out process to directly go to a new puzzle
        if (!StaticVariables.isFading) {
            StaticVariables.fadingTo = "puzzle";
            StaticVariables.fadingIntoPuzzleSameSize = false;
            startFadeOut();
        }
    }
    
    private void OnApplicationQuit() {
        save();
    }
}