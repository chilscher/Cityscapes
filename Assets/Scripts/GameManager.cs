//for Cityscapes, copyright Cole Hilscher 2024

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System;

public class GameManager : MonoBehaviour {
    //controls the puzzle canvas and gameplay. Only one is used, and only on the InPuzzle scene.
    [Header("Puzzle Setup")]
    [HideInInspector]
    public int size; //puzzle size
    public PuzzleGenerator puzzleGenerator; //the PuzzleGenerator object that will create the puzzle's solution
    public GameObject puzzlePositioning; //determines where the puzzle is going to go
    private float originalPuzzleScale;

    [Header("UI Elements")]
    public GameObject canvas;
    public GameObject winParent; //the canvas that is used when the player beats a puzzle
    public GameObject puzzleParent;
    public GameObject tutorialParent;
    public GameObject streetCorner;
    public Sprite[] numberSprites;
    [HideInInspector]
    public Skin skin;
    public GameObject puzzleBackground;

    //the tools that the player can choose from
    [Header("Buttons")]
    public GameObject buildButton;
    public GameObject note1Button;
    public GameObject note2Button;
    public GameObject clearTileButton;
    public GameObject undoButton;
    public GameObject redoButton;
    public GameObject removeAllOfNumberButton;
    public GameObject removeAllOfNumberButtonDash;
    public Image removeAllOfNumberButtonNumber;
    public GameObject removeAllOfNumberButtonClearText;
    public GameObject removeAllOfNumberButtonFillText;
    public GameObject clearPuzzleButton;
    public GameObject numbers1to3;
    public GameObject numbers1to4;
    public GameObject numbers1to5;
    public GameObject numbers1to6;
    public GameObject numbers1to7;
    public GameObject menuButton;
    public GameObject shopButton;
    public GameObject settingsButton;

    [Header("Building Quantity Alerts")]

    [HideInInspector]
    public GameObject[] numberButtons;
    public GameObject[] numberButtonCorrectQuantities;
    public GameObject[] numberButtonTooManyQuantities;
    
    [Header("Tutorial Stuff")]
    public TutorialManager tutorialManager;
    public GameObject screenTappedMonitor;
    public Text tutorialTextBox;
    public Text tutorialContinueClue;
    public GameObject redStreetBorder;
    public Skin basicSkin; //the basic skin, used for loading the tutorial with the basic skin no matter what skin is currently selected
    public Transform tutorialHighlightsParent;
    public Text tutorialProgressText;
    public RectTransform tutorialProgressBar;

    //variables used in determining a win, and handling the win pop-up
    [Header("Win Popup Stuff")]
    public int coinsFor3Win = 3;
    public int coinsFor4Win = 5;
    public int coinsFor5Win = 10;
    public int coinsFor6Win = 20;
    public int coinsFor7Win = 50;
    [HideInInspector]
    public bool canClick = true;
    private bool hasWonYet = false;
    public GameObject coinsBox1s;
    public GameObject coinsBox10s;
    public Image totalCoin1;
    public Image totalCoin10;
    public Image totalCoin100;
    public Image totalCoin1k;
    public Image totalCoin10k;
    public Image totalCoin100k;
    public Image totalCoin1m;
    public GameObject smallCityArt;
    public GameObject mediumCityArt;
    public GameObject largeCityArt;
    public GameObject hugeCityArt;
    public GameObject massiveCityArt;
    public Text anotherPuzzleText;
    public GameObject winPopupMenuButton;
    public GameObject winPopupShopButton;
    public GameObject winPopupSettingsButton;
    public GameObject winPopupPuzzleButton;
    public Image winPopupBorder;
    public Image winPopupInterior;
    public Image winBlackSprite;
    public GameObject winPopup;
    [Header("Misc")]
    public bool showBuildings;
    public Color notEnoughQuantity;
    public Color rightAmountQuantity;
    public Color tooMuchQuantity;

    
    //storing what the player is trying to do
    public enum ClickTileActions {Build, ToggleNote1, ToggleNote2, Erase};
    [HideInInspector]
    public ClickTileActions clickTileAction;
    //public string clickTileAction = "Apply Selected"; //what happens when you click a building tile - either toggle a building, toggle a note1, toggle a note2, or clear the tile
    [HideInInspector]
    public int selectedNumber = 0; //the number to place, either as a building or note
    private GameObject prevClickedNumButton; //when the player clicks the erase button, the selected number button is disselected. When they click any other tool, the previous number is selected again

    //storing puzzle states
    private List<PuzzleState> previousPuzzleStates = new List<PuzzleState>(); // the list of puzzle states to be restored by the undo button
    private PuzzleState currentPuzzleState;
    private List<PuzzleState> nextPuzzleStates = new List<PuzzleState>();// the list of puzzle states to be restored by the redo button

    
    private void Start() {
        //choose which skin to use
        skin = StaticVariables.skin;
        if (StaticVariables.isTutorial)
            skin = basicSkin;

        size = StaticVariables.size;

        HideNumberButtons();

        ColorMenuButton();

        if (StaticVariables.isTutorial) { //set up the tutorial. uses tutorialmanager
            originalPuzzleScale = puzzlePositioning.transform.localScale.x;
            SetTutorialNumberButtons();
            tutorialParent.SetActive(true);
            puzzleParent.SetActive(false);
            //hide menu button if it is your first time playing the tutorial
            tutorialParent.transform.Find("Menu").gameObject.SetActive(StaticVariables.hasBeatenTutorial);
            tutorialParent.transform.Find("Background").GetComponent<Image>().sprite = skin.puzzleBackground;
            tutorialParent.transform.Find("Tutorial Text Box").Find("Interior").GetComponent<Image>().color = skin.popupInside;
            InterfaceFunctions.ColorMenuButton(tutorialParent.transform.Find("Menu").gameObject, skin);

            tutorialManager = new TutorialManager();
            tutorialManager.gameManager = this;
            tutorialManager.StartTutorial();
        }

        else { //continue with the puzzle generation and setup
            tutorialParent.SetActive(false);
            puzzleParent.SetActive(true); 
            //load a puzzle if you already have one saved, otherwise generate a new one based on the size
            if (StaticVariables.hasSavedPuzzleState) {
                puzzleGenerator.RestoreSavedPuzzle(StaticVariables.puzzleSolution, size);
                LoadPuzzleStates();
            }
            else {
                puzzleGenerator.CreatePuzzle(size);
                puzzleGenerator.AutofillPermanentBuildings();
                selectedNumber = size; //you automatically start with the highest building size selected
                clickTileAction = ClickTileActions.Build;
            }
            //set up the visuals of the screen based on the puzzle size and what tools you have unlocked
            DrawFullPuzzle();
            SetNumberButtons();
            SetAllButtonAvailability();
            HighlightSelectedNumber(); 
            if (clickTileAction == ClickTileActions.Erase) { DisselectNumber(prevClickedNumButton); }
            HighlightBuildType();
            puzzleBackground.GetComponent<Image>().sprite = skin.puzzleBackground;
            InterfaceFunctions.ColorMenuButton(winPopupMenuButton, skin);
            InterfaceFunctions.ColorMenuButton(winPopupShopButton, skin);
            InterfaceFunctions.ColorMenuButton(winPopupSettingsButton, skin);
            InterfaceFunctions.ColorMenuButton(winPopupPuzzleButton, skin);
            
            winPopupBorder.color = skin.popupBorder;
            winPopupInterior.color = skin.popupInside;

            //set up the win popup process
            winParent.SetActive(false);
            ShowCorrectCityArtOnWinScreen();
            int coins = 0;
            switch(size){
                case 3: coins = coinsFor3Win; break;
                case 4: coins = coinsFor4Win; break;
                case 5: coins = coinsFor5Win; break;
                case 6: coins = coinsFor6Win; break;
                case 7: coins = coinsFor7Win; break;
            }
            int onesDigit = coins % 10;
            int tensDigit = (coins / 10) % 10;

            coinsBox1s.GetComponent<Image>().sprite = numberSprites[onesDigit];
            coinsBox10s.GetComponent<Image>().sprite = numberSprites[tensDigit];

            //set the first puzzle state, and you are ready to start playing!
            currentPuzzleState = new PuzzleState(puzzleGenerator);
            UpdateAllBuildingQuantities();
        }
    }

    private void Update() {
        //set the color of each resident, specifically if their viewpoint is satisfied
        foreach(SideHintTile h in puzzleGenerator.allHints) {
            h.SetAppropriateColor();
        }
        //check for a win
        if (!hasWonYet && puzzleGenerator.CheckPuzzle() && !StaticVariables.isTutorial) {
            hasWonYet = true;
            
            winParent.SetActive(true);
            canClick = false;
            IncrementCoinsForWin();

            Color nextColor = winBlackSprite.color;
            Color currentColor = Color.black;
            currentColor.a = 0;
            winBlackSprite.color = currentColor;
            winBlackSprite.gameObject.SetActive(true);
            winBlackSprite.DOColor(nextColor, 0.5f);

            winPopup.transform.localScale = Vector3.zero;
            winPopup.transform.DOScale(Vector3.one, 0.5f);

            StaticVariables.hasSavedPuzzleState = false;
            Save();
            return;
        }

        CheckForKeyboardInputs();

    }

    private void CheckForKeyboardInputs(){
        if (Input.GetKeyDown(KeyCode.Escape)){ //not rebindable. also works for pushing the back button on a mobile device
            if (!StaticVariables.isTutorial || StaticVariables.hasBeatenTutorial)
                PushMainMenuButton();
        }
        if (StaticVariables.osType == StaticVariables.OSTypes.PC){
            if (Input.GetKeyDown(StaticVariables.keybindBuilding1)){
                if (StaticVariables.isTutorial)
                    PushTutorialNumberButton(1);
                else
                    PushNumberButton(1);
            }
            if (Input.GetKeyDown(StaticVariables.keybindBuilding2)){
                if (StaticVariables.isTutorial)
                    PushTutorialNumberButton(2);
                else
                    PushNumberButton(2);
            }
            if (Input.GetKeyDown(StaticVariables.keybindBuilding3)){
                if (StaticVariables.isTutorial)
                    PushTutorialNumberButton(3);
                else
                    PushNumberButton(3);
            }
            if (Input.GetKeyDown(StaticVariables.keybindBuilding4) && size >= 4)
                PushNumberButton(4);
            if (Input.GetKeyDown(StaticVariables.keybindBuilding5) && size >= 5)
                PushNumberButton(5);
            if (Input.GetKeyDown(StaticVariables.keybindBuilding6) && size >= 6)
                PushNumberButton(6);
            if (Input.GetKeyDown(StaticVariables.keybindBuilding7) && size >= 7)
                PushNumberButton(7);
            if (Input.GetKeyDown(StaticVariables.keybindBuild))
                PushBuildButton();
            if (Input.GetKeyDown(StaticVariables.keybindNote1) && StaticVariables.includeNotes1Button)
                PushNote1Button();
            if (Input.GetKeyDown(StaticVariables.keybindNote2) && StaticVariables.includeNotes2Button)
                PushNote2Button();
            if (Input.GetKeyDown(StaticVariables.keybindErase))
                PushEraseButton();
            if (Input.GetKeyDown(StaticVariables.keybindUndo) && StaticVariables.includeUndoRedo)
                PushUndoButton();
            if (Input.GetKeyDown(StaticVariables.keybindRedo) && StaticVariables.includeUndoRedo)
                PushRedoButton();
            if (Input.GetKeyDown(StaticVariables.keybindRemoveAll) && StaticVariables.includeRemoveAllOfNumber)
                PushRemoveAllButton();
            if (Input.GetKeyDown(StaticVariables.keybindClearPuzzle) && StaticVariables.includeClearPuzzle)
                PushClearPuzzleButton();
        }
    }

    // ---------------------------------------------------
    //ALL OF THE FUNCTIONS THAT ARE USED TO SET UP THE PUZZLE SCENE, BEFORE PLAYING
    // ---------------------------------------------------

    private void HideNumberButtons() {
        //hide all number buttons, used at the start of the puzzle. later in setNumberButtons, the correct numbers are turned back on.
        numbers1to3.SetActive(false);
        numbers1to4.SetActive(false);
        numbers1to5.SetActive(false);
        numbers1to6.SetActive(false);
        numbers1to7.SetActive(false);
    }

    private void ColorMenuButton() {
        InterfaceFunctions.ColorMenuButton(menuButton, skin);
        InterfaceFunctions.ColorMenuButton(shopButton, skin);
        InterfaceFunctions.ColorMenuButton(settingsButton, skin);
    }

    public void SetNumberButtons() {
        //show the correct number buttons depending on the puzzle size, and hide the rest
        HideNumberButtons();
        numberButtons = new GameObject[size];
        numberButtonCorrectQuantities = new GameObject[size];
        numberButtonTooManyQuantities = new GameObject[size];
        switch (size) {
            case 3:
                SetNBs(numbers1to3);
                break;
            case 4:
                SetNBs(numbers1to4);
                break;
            case 5:
                SetNBs(numbers1to5);
                break;
            case 6:
                SetNBs(numbers1to6);
                break;
            case 7:
                SetNBs(numbers1to7);
                break;
        }
    }

    private void SetNBs(GameObject nB) {
        //set the number button gameObjects
        nB.SetActive(true);
        for (int i = 0; i < size; i++) {
            numberButtons[i] = nB.transform.GetChild(i).gameObject;
            numberButtonCorrectQuantities[i] = numberButtons[i].transform.Find("Quantity Icon - Correct Quantity").gameObject;
            numberButtonTooManyQuantities[i] = numberButtons[i].transform.Find("Quantity Icon - Too Many").gameObject;
            DisselectNumber(numberButtons[i]);
        }
    }
    
    public void SetAllButtonAvailability() {
        buildButton.SetActive(true);
        clearTileButton.SetActive(true);
        note1Button.SetActive(StaticVariables.includeNotes1Button);
        note2Button.SetActive(StaticVariables.includeNotes2Button);
        undoButton.SetActive(StaticVariables.includeUndoRedo);
        redoButton.SetActive(StaticVariables.includeUndoRedo);
        removeAllOfNumberButton.SetActive(StaticVariables.includeRemoveAllOfNumber);
        clearPuzzleButton.SetActive(StaticVariables.includeClearPuzzle);
        
        InterfaceFunctions.ColorPuzzleButtonOff(buildButton, skin);
        InterfaceFunctions.ColorPuzzleButtonOff(clearTileButton, skin);
        InterfaceFunctions.ColorPuzzleButtonOff(note1Button, skin);
        InterfaceFunctions.ColorPuzzleButtonOff(note2Button, skin);
        InterfaceFunctions.ColorPuzzleButtonOff(undoButton, skin);
        InterfaceFunctions.ColorPuzzleButtonOff(redoButton, skin);
        InterfaceFunctions.ColorPuzzleButtonOff(removeAllOfNumberButton, skin);
        InterfaceFunctions.ColorPuzzleButtonOff(clearPuzzleButton, skin);
    }

    public void DrawFullPuzzle() {
        //draws the visuals for the puzzle tiles, borders, residents, and street corners
        float desiredPuzzleSize = puzzlePositioning.transform.localScale.x;

        if (canvas.GetComponent<RectTransform>().rect.height > canvas.GetComponent<CanvasScaler>().referenceResolution.y) 
            desiredPuzzleSize *= (canvas.GetComponent<CanvasScaler>().referenceResolution.y / canvas.GetComponent<RectTransform>().rect.height);

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
                puzzleGenerator.tilesArray[i, j].SetValues(pos, scaleFactor, parent);
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
            topTile.SetValues(new Vector2(tbx, topy), scaleFactor, parent);
            bottomTile.SetValues(new Vector2(tbx, bottomy), scaleFactor, parent);
            leftTile.SetValues(new Vector2(leftx, lry), scaleFactor, parent);
            rightTile.SetValues(new Vector2(rightx, lry), scaleFactor, parent);
            topTile.RotateHint(0, (totalSize / size));
            bottomTile.RotateHint(180, (totalSize / size));
            leftTile.RotateHint(90, (totalSize / size));
            rightTile.RotateHint(270, (totalSize / size));
        }
        foreach (SideHintTile h in puzzleGenerator.allHints)
            h.AddHint();

        //draw street corners
        Vector2 topLeftPos = new Vector2(leftx, topy);
        Vector2 bottomLeftPos = new Vector2(leftx, bottomy);
        Vector2 topRightPos = new Vector2(rightx, topy);
        Vector2 bottomRightPos = new Vector2(rightx, bottomy);
        CreateCorner(topLeftPos, scaleFactor, 0, parent);
        CreateCorner(bottomLeftPos, scaleFactor, 90, parent);
        CreateCorner(topRightPos, scaleFactor, 270, parent);
        CreateCorner(bottomRightPos, scaleFactor, 180, parent);
    }

    private void CreateCorner(Vector2 p, float scale, int rot, Transform parent) {
        //add a street corner to the puzzle display. They serve no mechanical purpose and are just there to look nice
        GameObject corner = Instantiate(streetCorner);
        corner.transform.position = p;
        corner.transform.localScale *= scale;
        corner.transform.Rotate(new Vector3(0, 0, rot));
        corner.transform.SetParent(parent);


        corner.GetComponent<Image>().color = skin.street;


        Vector3 pos = corner.transform.localPosition;
        pos.z = 0;
        corner.transform.localPosition = pos;
    }

    // ---------------------------------------------------
    //ALL OF THE FUNCTIONS THAT INTERACT WITH AND SET UP THE WIN POPUP
    // ---------------------------------------------------

    private void ShowCorrectCityArtOnWinScreen() {
        //shows the "city art" on the win popup, depending on which skin and which city size the player is using
        smallCityArt.SetActive(size == 3);
        mediumCityArt.SetActive(size == 4);
        largeCityArt.SetActive(size == 5);
        hugeCityArt.SetActive(size == 6);
        massiveCityArt.SetActive(size == 7);

        switch (size) {
            case 3: anotherPuzzleText.text = "ANOTHER SMALL CITY"; break;
            case 4: anotherPuzzleText.text = "ANOTHER MEDIUM CITY"; break;
            case 5: anotherPuzzleText.text = "ANOTHER LARGE CITY"; break;
            case 6: anotherPuzzleText.text = "ANOTHER HUGE CITY"; break;
            case 7: anotherPuzzleText.text = "ANOTHER MASSIVE CITY"; break;
        }
    }

    public void DisplayTotalCoinsAmount() {
        //show the player's total coins on the win popup screen
        int value1 = StaticVariables.coins % 10;
        int value10 = (StaticVariables.coins / 10) % 10;
        int value100 = (StaticVariables.coins / 100) % 10;
        int value1k = (StaticVariables.coins / 1000) % 10;
        int value10k = (StaticVariables.coins / 10000) % 10;
        int value100k = (StaticVariables.coins / 100000) % 10;
        int value1m = (StaticVariables.coins / 100000) % 10;
        totalCoin1.sprite = numberSprites[value1];
        totalCoin10.sprite = numberSprites[value10];
        totalCoin100.sprite = numberSprites[value100];
        totalCoin1k.sprite = numberSprites[value1k];
        totalCoin10k.sprite = numberSprites[value10k];
        totalCoin100k.sprite = numberSprites[value100k];
        totalCoin1m.sprite = numberSprites[value1m];

        totalCoin1.gameObject.SetActive(true);
        totalCoin10.gameObject.SetActive(StaticVariables.coins > 9);
        totalCoin100.gameObject.SetActive(StaticVariables.coins > 99);
        totalCoin1k.gameObject.SetActive(StaticVariables.coins > 999);
        totalCoin10k.gameObject.SetActive(StaticVariables.coins > 9999);
        totalCoin100k.gameObject.SetActive(StaticVariables.coins > 99999);
        totalCoin1m.gameObject.SetActive(StaticVariables.coins > 999999);
    }


    private void IncrementCoinsForWin() {
        //when the player wins the puzzle, add to their coin total, and show the new amount on the win popup
        switch (size) {
            case 3:
                StaticVariables.AddCoins(coinsFor3Win);
                break;
            case 4:
                StaticVariables.AddCoins(coinsFor4Win);
                break;
            case 5:
                StaticVariables.AddCoins(coinsFor5Win);
                break;
            case 6:
                StaticVariables.AddCoins(coinsFor6Win);
                break;
            case 7:
                StaticVariables.AddCoins(coinsFor7Win);
                break;
        }

        DisplayTotalCoinsAmount();
    }

    // ---------------------------------------------------
    //ALL OF THE FUNCTIONS THAT ARE USED BY THE TUTORIAL MANAGER
    // ---------------------------------------------------

    public void SetTutorialNumberButtons() {
        //show the number buttons, specifically used for the tutorial
        numberButtons = new GameObject[3];
        numberButtons[0] = tutorialParent.transform.Find("Numbers").Find("1").gameObject;
        numberButtons[1] = tutorialParent.transform.Find("Numbers").Find("2").gameObject;
        numberButtons[2] = tutorialParent.transform.Find("Numbers").Find("3").gameObject;
        DisselectNumber(numberButtons[0]);
        DisselectNumber(numberButtons[1]);
        DisselectNumber(numberButtons[2]);

    }

    public void TappedScreen() {
        //used to advance the tutorial
        tutorialManager.TappedScreen();
    }

    public void HideHints() {
        //hide the side hints (residents), used in the tutorial
        foreach (SideHintTile h in puzzleGenerator.allHints) {
            h.GetComponent<Transform>().GetChild(1).gameObject.SetActive(false);
            h.GetComponent<Transform>().GetChild(2).gameObject.SetActive(false);
        }
    }

    public void ShowHints() {
        //show the side hints (residents), used in the tutorial
        foreach (SideHintTile h in puzzleGenerator.allHints) {
            h.GetComponent<Transform>().GetChild(1).gameObject.SetActive(true);
            h.GetComponent<Transform>().GetChild(2).gameObject.SetActive(true);
        }
    }

    public void AddRedStreetBorderForTutorial(Vector3 centerPoint, int rotation) {
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

    public void RemoveRedStreetBordersForTutorial() {
        //remove all borders around streets, used in the tutorial
        GameObject[] RedBorders = GameObject.FindGameObjectsWithTag("Red Border");
        foreach (GameObject r in RedBorders)
            Destroy(r);
    }

    public void DeleteCityForTutorial() {
        //used in the tutorial to advance from the first pre-defined puzzle to the second one
        GameObject[] oldPuzzleTiles = GameObject.FindGameObjectsWithTag("Puzzle Tile");
        foreach (GameObject t in oldPuzzleTiles)
            Destroy(t);
        GameObject[] oldSideTiles = GameObject.FindGameObjectsWithTag("Side Tile");
        foreach (GameObject t in oldSideTiles)
            Destroy(t);
    }

    public void PushTutorialNumberButton(int num) {
        //when the player pushes a number button on the tutorial, select it, and maybe advance some tutorial dialogue
        SwitchNumber(num);
        ShowNumberButtonClicked(numberButtons[num - 1]);
        if (StaticVariables.isTutorial)
            tutorialManager.TappedNumberButton(num);
    }


    // ---------------------------------------------------
    //ALL OF THE FUNCTIONS THAT ARE CALLED WHEN THE PLAYER INTERACTS WITH THE PUZZLE
    // ---------------------------------------------------
    
    public void SwitchNumber(int num) {
        //change the selected number
        selectedNumber = num;
    }

    public void PushBuildButton() {
        //choose the build selection mode
        if (clickTileAction == ClickTileActions.Erase) {
            SelectNumber(prevClickedNumButton);
            foreach (PuzzleTile t in puzzleGenerator.puzzleTiles) { t.HighlightIfBuildingNumber(selectedNumber); }
        }
        clickTileAction = ClickTileActions.Build;
        DisselectBuildNotesAndEraseButtons();
        InterfaceFunctions.ColorPuzzleButtonOn(buildButton, skin);
        UpdateRemoveSelectedNumber();
        Save();
    }

    public void PushNote1Button() {
        //choose the note 1 selection mode
        if (clickTileAction == ClickTileActions.Erase) {
            SelectNumber(prevClickedNumButton);
            foreach (PuzzleTile t in puzzleGenerator.puzzleTiles) { t.HighlightIfBuildingNumber(selectedNumber); }
        }
        clickTileAction = ClickTileActions.ToggleNote1;
        DisselectBuildNotesAndEraseButtons();
        InterfaceFunctions.ColorPuzzleButtonOn(note1Button, skin);
        UpdateRemoveSelectedNumber();
        Save();
    }

    public void PushNote2Button() {
        //choose the note 2 selection mode
        if (clickTileAction == ClickTileActions.Erase) {
            SelectNumber(prevClickedNumButton);
            foreach (PuzzleTile t in puzzleGenerator.puzzleTiles) { t.HighlightIfBuildingNumber(selectedNumber); }
        }
        clickTileAction = ClickTileActions.ToggleNote2;
        DisselectBuildNotesAndEraseButtons();

        InterfaceFunctions.ColorPuzzleButtonOn(note2Button, skin);
        UpdateRemoveSelectedNumber();
        Save();
    }
    
    public void PushEraseButton() {
        //choose the clear tile selection mode
        clickTileAction = ClickTileActions.Erase;
        DisselectNumber(prevClickedNumButton);
        DisselectBuildNotesAndEraseButtons();
        foreach (PuzzleTile t in puzzleGenerator.puzzleTiles) { t.UnhighlightBuildingNumber(); }
        InterfaceFunctions.ColorPuzzleButtonOn(clearTileButton, skin);
        UpdateRemoveSelectedNumber();
        Save();
    }

    public void DisselectBuildNotesAndEraseButtons() {
        //select the clear tile button and disselect the others
        InterfaceFunctions.ColorPuzzleButtonOff(buildButton, skin);
        InterfaceFunctions.ColorPuzzleButtonOff(note1Button, skin);
        InterfaceFunctions.ColorPuzzleButtonOff(note2Button, skin);
        InterfaceFunctions.ColorPuzzleButtonOff(clearTileButton, skin);
    }
    
    public void ShowNumberButtonClicked(GameObject nB) {
        //when the player clicks a number button, highlight that number, and also highlight all copies of that number in the puzzle (if the player has the highlightBuildings upgrade)
        if (prevClickedNumButton != null)
            DisselectNumber(prevClickedNumButton);
        prevClickedNumButton = nB;
        SelectNumber(nB);
        UpdateRemoveSelectedNumber();

        if (!StaticVariables.isTutorial && StaticVariables.includeHighlightBuildings) {
            foreach (PuzzleTile t in puzzleGenerator.puzzleTiles)
                t.HighlightIfBuildingNumber(selectedNumber);
        }
    }

    private void SelectNumber(GameObject btn) {
        //color chosen the number button to the "on" coloration from the current skin
        InterfaceFunctions.ColorPuzzleButtonOn(btn, skin);
    }

    private void DisselectNumber(GameObject btn) {
        //color chosen the number button to the "off" coloration from the current skin
        InterfaceFunctions.ColorPuzzleButtonOff(btn, skin);
    }
    
    public void AddToPuzzleHistory() {
        //add the current puzzle state to the list of states when the player makes a move
        //used in the undo/redo process
        //also clears the list of "next puzzle states", so the player can't "redo" to get to those states any more
        previousPuzzleStates.Add(currentPuzzleState);
        PuzzleState currentState = new PuzzleState(puzzleGenerator);
        currentPuzzleState = currentState;
        if (nextPuzzleStates.Count > 0)
            nextPuzzleStates = new List<PuzzleState>();
        Save();
    }

    public void PushUndoButton() {
        //returns the player to a previous puzzle state, and moves the most recent puzzle state to the list of "next puzzle states"
        //used in the undo/redo process
        if (previousPuzzleStates.Count > 0) {
            nextPuzzleStates.Add(currentPuzzleState);
            PuzzleState currentState = previousPuzzleStates[previousPuzzleStates.Count - 1];
            currentState.RestorePuzzleState(puzzleGenerator);
            currentPuzzleState = currentState;
            previousPuzzleStates.RemoveAt(previousPuzzleStates.Count - 1);
            UpdateAllBuildingQuantities();
            Save();
        }
    }

    public void PushRedoButton() {
        //returns the player to a "next" puzzle state, and moves the most recent state to the list of "previous" states
        //used in the undo/redo process
        if (nextPuzzleStates.Count > 0) {
            previousPuzzleStates.Add(currentPuzzleState);
            PuzzleState currentState = nextPuzzleStates[nextPuzzleStates.Count - 1];
            currentState.RestorePuzzleState(puzzleGenerator);
            currentPuzzleState = currentState;
            nextPuzzleStates.RemoveAt(nextPuzzleStates.Count - 1);
            UpdateAllBuildingQuantities();
            Save();
        }
    }

    public void PushNumberButton(int num) {
        //when the player taps a number selection button, highlight it and make it the currently-selected number
        SwitchNumber(num);
        ShowNumberButtonClicked(numberButtons[num - 1]);
        if (clickTileAction == ClickTileActions.Erase) {
            clickTileAction = ClickTileActions.Build;
            HighlightBuildType();
            UpdateRemoveSelectedNumber();
        }
        Save();
    }

    public void HighlightSelectedNumber() {
        //highlight the currently-selected number button
        ShowNumberButtonClicked(numberButtons[selectedNumber - 1]);
    }

    public void HighlightBuildType() {
        //if the build type is set to be included, then highlight it. otherwise, highlight the build button
        DisselectBuildNotesAndEraseButtons();

        GameObject button;
        if (clickTileAction == ClickTileActions.ToggleNote1 && StaticVariables.includeNotes1Button)
            button = note1Button;
        else if (clickTileAction == ClickTileActions.ToggleNote2 && StaticVariables.includeNotes2Button)
            button = note2Button;
        else if (clickTileAction == ClickTileActions.Erase)
            button = clearTileButton;
        else {
            clickTileAction = ClickTileActions.Build;
            button = buildButton;
        }
        InterfaceFunctions.ColorPuzzleButtonOn(button, skin);
    }
    
    public void PushRemoveAllButton() {
        //clear the entire puzzle of all of the currently-selected number of the currently-selected type.
        //for example, when #3 and "red notes" are selected, this function clears all red 3's from the puzzle
        if ((clickTileAction != ClickTileActions.Erase) && (selectedNumber != 0)) {
            bool somethingChanged = false;
            if (clickTileAction == ClickTileActions.Build){
                foreach (PuzzleTile t in puzzleGenerator.puzzleTiles) {
                    if ((t.shownNumber == selectedNumber) && (!t.isPermanentBuilding)) {
                        somethingChanged = true;
                        t.RemoveNumberFromTile();
                    }
                }
            }
            else if (clickTileAction == ClickTileActions.ToggleNote1){
                bool foundAnything = false;
                foreach (PuzzleTile t in puzzleGenerator.puzzleTiles) {
                    if (t.DoesTileContainColoredNote(1, selectedNumber)){
                        t.ToggleNote1(selectedNumber); 
                        somethingChanged = true;
                        foundAnything = true;
                    }
                }
                if (!foundAnything && StaticVariables.includeRemoveButtonFillsNotes){
                    somethingChanged = true;
                    foreach (PuzzleTile t in puzzleGenerator.puzzleTiles) {
                    if (t.shownNumber == 0)
                        t.ToggleNote1(selectedNumber); 
                    }
                }
            }
            else if (clickTileAction == ClickTileActions.ToggleNote2){
                bool foundAnything = false;
                foreach (PuzzleTile t in puzzleGenerator.puzzleTiles) {
                    if (t.DoesTileContainColoredNote(2, selectedNumber)){
                        t.ToggleNote2(selectedNumber); 
                        somethingChanged = true;
                        foundAnything = true;
                    }
                }
                if (!foundAnything && StaticVariables.includeRemoveButtonFillsNotes){
                    somethingChanged = true;
                    foreach (PuzzleTile t in puzzleGenerator.puzzleTiles) {
                    if (t.shownNumber == 0)
                        t.ToggleNote2(selectedNumber); 
                    }
                }
            }
            if (somethingChanged) {
                UpdateAllBuildingQuantities();
                UpdateRemoveSelectedNumber();
                AddToPuzzleHistory();
            }
        }
    }
    
    public void PushClearPuzzleButton() {
        //removes all buildings and notes of all types from the puzzle
        bool changedAnything = false;
        foreach (PuzzleTile t in puzzleGenerator.puzzleTiles) {
            if ((t.DoesTileContainAnything()) && (!t.isPermanentBuilding))
                changedAnything = true;
            t.ClearColoredNotes();
            if (!t.isPermanentBuilding)
                t.RemoveNumberFromTile();

        }
        if (changedAnything) {
            UpdateAllBuildingQuantities();
            AddToPuzzleHistory();
        }
    }
    
    public void UpdateRemoveSelectedNumber() {
        //when the player changes their tool type or their selected number, update the color and number displayed on the "remove all of type" button
        if (!StaticVariables.isTutorial && StaticVariables.includeRemoveAllOfNumber) {
            removeAllOfNumberButtonDash.SetActive(false);
            removeAllOfNumberButtonNumber.gameObject.SetActive(true);
            removeAllOfNumberButtonNumber.sprite = numberSprites[selectedNumber];
            removeAllOfNumberButtonClearText.SetActive(true);
            removeAllOfNumberButtonFillText.SetActive(false);
            Color c = Color.white;
            if (clickTileAction == ClickTileActions.ToggleNote1)
                c = StaticVariables.skin.note1;
            else if (clickTileAction == ClickTileActions.ToggleNote2)
                c = StaticVariables.skin.note2;
            removeAllOfNumberButtonNumber.color = c;

            if (clickTileAction == ClickTileActions.Erase) {
                removeAllOfNumberButtonDash.SetActive(true);
                removeAllOfNumberButtonNumber.gameObject.SetActive(false);
            }
            else if ((clickTileAction == ClickTileActions.ToggleNote1) && (StaticVariables.includeRemoveButtonFillsNotes)){
                bool foundAnything = false;
                foreach (PuzzleTile t in puzzleGenerator.puzzleTiles) {
                    if (t.DoesTileContainColoredNote(1, selectedNumber))
                        foundAnything = true;
                }
                if (!foundAnything){
                    removeAllOfNumberButtonClearText.SetActive(false);
                    removeAllOfNumberButtonFillText.SetActive(true);
                }
            }
            else if ((clickTileAction == ClickTileActions.ToggleNote2) && (StaticVariables.includeRemoveButtonFillsNotes)){
                bool foundAnything = false;
                foreach (PuzzleTile t in puzzleGenerator.puzzleTiles) {
                    if (t.DoesTileContainColoredNote(2, selectedNumber))
                        foundAnything = true;
                }
                if (!foundAnything){
                    removeAllOfNumberButtonClearText.SetActive(false);
                    removeAllOfNumberButtonFillText.SetActive(true);
                }
            }
        }
    }

    public void UpdateAllBuildingQuantities() {
        if (!StaticVariables.unlockedBuildingQuantityStatus || !StaticVariables.includeBuildingQuantityStatus || StaticVariables.isTutorial)
            return;
        //get the quantity of each building
        int[] quantities = new int[size];
        foreach (PuzzleTile t in puzzleGenerator.puzzleTiles) {
            if (t.shownNumber != 0)
                quantities[t.shownNumber - 1]++;
        }

        //update the quantity display for each building size
        for (int i = 0; i< size; i++) {
            int buildingSize = i + 1;
            UpdateBuildingQuantity(buildingSize, quantities[i]);
        }
    }

    private void UpdateBuildingQuantity(int buildingSize, int quantity) {
        GameObject correct = numberButtonCorrectQuantities[buildingSize - 1];
        GameObject tooMany = numberButtonTooManyQuantities[buildingSize - 1];
        correct.SetActive(false);
        tooMany.SetActive(false);
        if (quantity > size)
            tooMany.SetActive(true);
        else if (quantity == size)
            correct.SetActive(true);
    }

    // ---------------------------------------------------
    //ALL OF THE FUNCTIONS THAT INVOLVE TRANSITIONING BETWEEN SCENES, FADING OUT OF A SCENE, OR SAVING/LOADING THE GAME
    // ---------------------------------------------------
    
    public void PushMainMenuButton() {
        Save();
        StaticVariables.FadeOutThenLoadScene("MainMenu");
    }

    public void PushShopButton() {
        Save();
        StaticVariables.FadeOutThenLoadScene("Shop");
    }
    
    public void PushSettingsButton() {
        Save();
        StaticVariables.FadeOutThenLoadScene("Settings");
    }

    public void Save() {
        //save the puzzle state and save the game
        if (!StaticVariables.isTutorial) {
            SavePuzzleStates();
            SaveSystem.SaveGame();
        }

    }

    public void SavePuzzleStates() {
        //store all of the puzzle state objects, and the currently selected build and tool options in StaticVariables. Used when the player leaves the puzzle partway through
        bool hasAnythingHappenedYet = false;
        if (previousPuzzleStates.Count > 0) { hasAnythingHappenedYet = true; }
        if (nextPuzzleStates.Count > 0) { hasAnythingHappenedYet = true; }
        if (hasAnythingHappenedYet) {
            StaticVariables.hasSavedPuzzleState = !hasWonYet;

            StaticVariables.previousPuzzleStates = previousPuzzleStates;
            StaticVariables.currentPuzzleState = currentPuzzleState;
            StaticVariables.nextPuzzleStates = nextPuzzleStates;

            StaticVariables.puzzleSolution = puzzleGenerator.MakeSolutionString();
            StaticVariables.savedPuzzleSize = size;

            StaticVariables.savedBuildNumber = selectedNumber;
            StaticVariables.savedBuildType = clickTileAction;
        }
        else {
            StaticVariables.hasSavedPuzzleState = false;
        }
    }

    public void LoadPuzzleStates() {
        //load all of the puzzle state objects, and the selected build and tool options from StaticVariables. Used when the player returns to their puzzle from the main menu
        StaticVariables.hasSavedPuzzleState = false;

        previousPuzzleStates = StaticVariables.previousPuzzleStates;
        currentPuzzleState = StaticVariables.currentPuzzleState;
        nextPuzzleStates = StaticVariables.nextPuzzleStates;

        currentPuzzleState.RestorePuzzleState(puzzleGenerator);

        selectedNumber = StaticVariables.savedBuildNumber;
        clickTileAction = StaticVariables.savedBuildType;
    }

    public void PushAnotherPuzzleButton() {
        StaticVariables.fadingIntoPuzzleSameSize = false;
        StaticVariables.FadeOutThenLoadScene("InPuzzle");
    }
    
    private void OnApplicationQuit() {
        Save();
    }
}