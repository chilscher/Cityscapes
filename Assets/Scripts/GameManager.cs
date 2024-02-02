//for Cityscapes, copyright Cole Hilscher 2024

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

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
    private GameObject removeAllOfNumberButton;

    public GameObject specialButtonsAll;
    public GameObject specialButtonsUndoRedoRemove;
    public GameObject specialButtonsUndoRedoClear;
    public GameObject specialButtonsUndoRedo;
    [HideInInspector]
    public GameObject[] numberButtons;
    public GameObject[] numberButtonCorrectQuantities;
    public GameObject[] numberButtonTooManyQuantities;
    public GameObject numbers1to3;
    public GameObject numbers1to4;
    public GameObject numbers1to5;
    public GameObject numbers1to6;
    public GameObject numbers1to7;
    public GameObject menuButton;
    public GameObject shopButton;
    public GameObject settingsButton;
    
    //variables used in the tutorial
    public TutorialManager tutorialManager;
    public GameObject screenTappedMonitor;
    public Text tutorialTextBox;
    public Text tutorialContinueClue;
    public GameObject redStreetBorder;
    public Skin basicSkin; //the basic skin, used for loading the tutorial with the basic skin no matter what skin is currently selected
    public Transform tutorialHighlightsParent;
    public Text tutorialProgressText;
    public RectTransform tutorialProgressBar;

    //UI elements
    public GameObject streetCorner;
    public Sprite[] numberSprites;

    //properties of the current skin
    public Skin skin;
    public GameObject puzzleBackground;

    //variables used in determining a win, and handling the win pop-up
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
    public GameObject totalCoinsBox1s;
    public GameObject totalCoinsBox10s;
    public GameObject totalCoinsBox100s;
    public GameObject totalCoinsBox1000s;
    public GameObject totalCoinsBox10000s;
    public GameObject smallCityArt;
    public GameObject mediumCityArt;
    public GameObject largeCityArt;
    public GameObject hugeCityArt;
    public GameObject massiveCityArt;
    public Text anotherPuzzleText;

    //win popup stuff
    public GameObject winPopupMenuButton;
    public GameObject winPopupShopButton;
    public GameObject winPopupSettingsButton;
    public GameObject winPopupPuzzleButton;
    public Image winPopupBorder;
    public Image winPopupInterior;
    public Image winBlackSprite;


    //storing puzzle states
    private List<PuzzleState> previousPuzzleStates = new List<PuzzleState>(); // the list of puzzle states to be restored by the undo button
    private PuzzleState currentPuzzleState;
    private List<PuzzleState> nextPuzzleStates = new List<PuzzleState>();// the list of puzzle states to be restored by the redo button
    
    public bool showBuildings;
    public Color notEnoughQuantity;
    public Color rightAmountQuantity;
    public Color tooMuchQuantity;
    
    private void Start() {
        //choose which skin to use
        skin = StaticVariables.skin;
        if (StaticVariables.isTutorial)
            skin = basicSkin;
        //winPopupScale = winParent.transform.GetChild(1).localScale.x;

        size = StaticVariables.size;
        includeNote1Btn = StaticVariables.includeNotes1Button;
        includeNote2Btn = StaticVariables.includeNotes2Button;

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
            //tutorialParent.transform.Find("Tutorial Text Box").Find("Border").GetComponent<Image>().color = skin.popupBorder;

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
                clickTileAction = "Apply Selected";
            }
            //set up the visuals of the screen based on the puzzle size and what tools you have unlocked
            DrawFullPuzzle();
            SetNumberButtons();
            SetSelectionModeButtons();
            SetUndoRedoRemoveClearButtons();
            HighlightSelectedNumber(); 
            if (clickTileAction == "Clear Tile") { DisselectNumber(prevClickedNumButton); }
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
            //Color c = winParent.transform.Find("Black Background").GetComponent<Image>().color;
            //c.a = fadeToWinDarknessRatio;
            //winParent.transform.Find("Black Background").GetComponent<Image>().color = c;
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

            winParent.transform.localScale = Vector3.zero;
            winParent.transform.DOScale(Vector3.one, 0.5f);

            StaticVariables.hasSavedPuzzleState = false;
            Save();
        }

        //if the player pushes the back button on their phone, go to the main menu. Identical to pushing the "Menu" button on-screen
        if (Input.GetKeyDown(KeyCode.Escape))
            PushMainMenuButton();
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
        //apply the current skin colors to the menu button
        //if (!StaticVariables.isTutorial) {
            InterfaceFunctions.ColorMenuButton(menuButton, skin);
            InterfaceFunctions.ColorMenuButton(shopButton, skin);
            InterfaceFunctions.ColorMenuButton(settingsButton, skin);
        //}
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
    
    public void SetSelectionModeButtons() {
        //show the appropriate selection mode buttons. Those are build, note1, note2, and erase
        if (includeNote2Btn && includeNote1Btn) {
            selectionModeButtons0.SetActive(false);
            selectionModeButtons1.SetActive(true);
            selectionModeButtons2.SetActive(false);
            selectionModeButtons3.SetActive(false);
            buildButton = selectionModeButtons1.transform.GetChild(0).gameObject;
            note1Button = selectionModeButtons1.transform.GetChild(1).gameObject;
            note2Button = selectionModeButtons1.transform.GetChild(2).gameObject;
            clearTileButton = selectionModeButtons1.transform.Find("Erase").gameObject;
        }
        else if (!includeNote2Btn && !includeNote1Btn) {
            selectionModeButtons0.SetActive(true);
            selectionModeButtons1.SetActive(false);
            selectionModeButtons2.SetActive(false);
            selectionModeButtons3.SetActive(false);
            buildButton = selectionModeButtons0.transform.Find("Build").gameObject;
            clearTileButton = selectionModeButtons0.transform.Find("Erase").gameObject;
        }
        else if (!includeNote2Btn && includeNote1Btn) {
            selectionModeButtons0.SetActive(false);
            selectionModeButtons1.SetActive(false);
            selectionModeButtons2.SetActive(true);
            selectionModeButtons3.SetActive(false);
            buildButton = selectionModeButtons2.transform.GetChild(0).gameObject;
            note1Button = selectionModeButtons2.transform.GetChild(1).gameObject;
            clearTileButton = selectionModeButtons2.transform.Find("Erase").gameObject;
        }
        else if (includeNote2Btn && !includeNote1Btn) {
            selectionModeButtons0.SetActive(false);
            selectionModeButtons1.SetActive(false);
            selectionModeButtons2.SetActive(false);
            selectionModeButtons3.SetActive(true);
            buildButton = selectionModeButtons3.transform.GetChild(0).gameObject;
            note2Button = selectionModeButtons3.transform.GetChild(1).gameObject;
            clearTileButton = selectionModeButtons3.transform.Find("Erase").gameObject;
        }
    }

    private void SetUndoRedoRemoveClearButtons(){
        specialButtonsAll.SetActive(false);
        specialButtonsUndoRedoRemove.SetActive(false);
        specialButtonsUndoRedoClear.SetActive(false);
        specialButtonsUndoRedo.SetActive(false);

        if (StaticVariables.includeUndoRedo && StaticVariables.includeRemoveAllOfNumber && StaticVariables.includeClearPuzzle){
            specialButtonsAll.SetActive(true);
            removeAllOfNumberButton = specialButtonsAll.transform.GetChild(2).gameObject;
            InterfaceFunctions.ColorPuzzleButtonOff(specialButtonsAll.transform.GetChild(0).gameObject, skin);
            InterfaceFunctions.ColorPuzzleButtonOff(specialButtonsAll.transform.GetChild(1).gameObject, skin);
            InterfaceFunctions.ColorPuzzleButtonOff(specialButtonsAll.transform.GetChild(2).gameObject, skin);
            InterfaceFunctions.ColorPuzzleButtonOff(specialButtonsAll.transform.GetChild(3).gameObject, skin);
        }
        else if (StaticVariables.includeUndoRedo && StaticVariables.includeRemoveAllOfNumber && !StaticVariables.includeClearPuzzle){
            specialButtonsUndoRedoRemove.SetActive(true);
            removeAllOfNumberButton = specialButtonsUndoRedoRemove.transform.GetChild(2).gameObject;
            InterfaceFunctions.ColorPuzzleButtonOff(specialButtonsUndoRedoRemove.transform.GetChild(0).gameObject, skin);
            InterfaceFunctions.ColorPuzzleButtonOff(specialButtonsUndoRedoRemove.transform.GetChild(1).gameObject, skin);
            InterfaceFunctions.ColorPuzzleButtonOff(specialButtonsUndoRedoRemove.transform.GetChild(2).gameObject, skin);
        }
        else if (StaticVariables.includeUndoRedo && !StaticVariables.includeRemoveAllOfNumber && StaticVariables.includeClearPuzzle){
            specialButtonsUndoRedoClear.SetActive(true);
            InterfaceFunctions.ColorPuzzleButtonOff(specialButtonsUndoRedoClear.transform.GetChild(0).gameObject, skin);
            InterfaceFunctions.ColorPuzzleButtonOff(specialButtonsUndoRedoClear.transform.GetChild(1).gameObject, skin);
            InterfaceFunctions.ColorPuzzleButtonOff(specialButtonsUndoRedoClear.transform.GetChild(2).gameObject, skin);

        }
        else if (StaticVariables.includeUndoRedo && !StaticVariables.includeRemoveAllOfNumber && !StaticVariables.includeClearPuzzle){
            specialButtonsUndoRedo.SetActive(true);
            InterfaceFunctions.ColorPuzzleButtonOff(specialButtonsUndoRedo.transform.GetChild(0).gameObject, skin);
            InterfaceFunctions.ColorPuzzleButtonOff(specialButtonsUndoRedo.transform.GetChild(1).gameObject, skin);

        }
        //there is no way to include remove and clear without undo/redo
        //else if (!StaticVariables.includeUndoRedo && !StaticVariables.includeRemoveAllOfNumber && !StaticVariables.includeClearPuzzle) nothing has to happen here

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
        if (clickTileAction == "Clear Tile") {
            SelectNumber(prevClickedNumButton);
            foreach (PuzzleTile t in puzzleGenerator.puzzleTiles) { t.HighlightIfBuildingNumber(selectedNumber); }
        }
        clickTileAction = "Apply Selected";
        DisselectBuildAndNotes();
        InterfaceFunctions.ColorPuzzleButtonOn(buildButton, skin);
        UpdateRemoveSelectedNumber();
        Save();
    }

    public void PushNote1Button() {
        //choose the note 1 selection mode
        if (clickTileAction == "Clear Tile") {
            SelectNumber(prevClickedNumButton);
            foreach (PuzzleTile t in puzzleGenerator.puzzleTiles) { t.HighlightIfBuildingNumber(selectedNumber); }
        }
        clickTileAction = "Toggle Note 1";
        DisselectBuildAndNotes();
        InterfaceFunctions.ColorPuzzleButtonOn(note1Button, skin);
        UpdateRemoveSelectedNumber();
        Save();
    }

    public void PushNote2Button() {
        //choose the note 2 selection mode
        if (clickTileAction == "Clear Tile") {
            SelectNumber(prevClickedNumButton);
            foreach (PuzzleTile t in puzzleGenerator.puzzleTiles) { t.HighlightIfBuildingNumber(selectedNumber); }
        }
        clickTileAction = "Toggle Note 2";
        DisselectBuildAndNotes();

        InterfaceFunctions.ColorPuzzleButtonOn(note2Button, skin);
        UpdateRemoveSelectedNumber();
        Save();
    }
    
    public void PushEraseButton() {
        //choose the clear tile selection mode
        clickTileAction = "Clear Tile";
        DisselectNumber(prevClickedNumButton);
        DisselectBuildAndNotes();
        foreach (PuzzleTile t in puzzleGenerator.puzzleTiles) { t.UnhighlightBuildingNumber(); }
        InterfaceFunctions.ColorPuzzleButtonOn(clearTileButton, skin);
        UpdateRemoveSelectedNumber();
        Save();
    }

    public void DisselectBuildAndNotes() {
        //select the clear tile button and disselect the others
        InterfaceFunctions.ColorPuzzleButtonOff(buildButton, skin);
        if (includeNote1Btn)
            InterfaceFunctions.ColorPuzzleButtonOff(note1Button, skin);
        if (includeNote2Btn)
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
        if (clickTileAction == "Clear Tile") {
            clickTileAction = "Apply Selected";
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
        DisselectBuildAndNotes();

        GameObject button;
        if (clickTileAction == "Toggle Note 1" && includeNote1Btn)
            button = note1Button;
        else if (clickTileAction == "Toggle Note 2" && includeNote2Btn)
            button = note2Button;
        else if (clickTileAction == "Clear Tile")
            button = clearTileButton;
        else {
            clickTileAction = "Apply Selected";
            button = buildButton;
        }
        InterfaceFunctions.ColorPuzzleButtonOn(button, skin);
    }
    
    public void PushRemoveAllButton() {
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
                    if (t.DoesTileContainColoredNote(colorNum, num)) {
                        if (colorNum == 1)
                            t.ToggleNote1(num); 
                        else
                            t.ToggleNote2(num);
                        foundAnything = true;
                    }
                }
            }
            if (colorNum == 0 && num != 0) {
                foreach (PuzzleTile t in puzzleGenerator.puzzleTiles) {
                    if ((t.shownNumber == num) && (!t.isPermanentBuilding)) {
                        foundAnything = true;
                        t.RemoveNumberFromTile();
                    }
                }
            }
            if (foundAnything) {
                UpdateAllBuildingQuantities();
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
        //when the player changes their tool type or their selecred number, update the color and number displayed on the "remove all of type" button
        if (!StaticVariables.isTutorial && StaticVariables.includeRemoveAllOfNumber) {
            removeAllOfNumberButton.transform.Find("Dash").gameObject.SetActive(false);
            removeAllOfNumberButton.transform.Find("Number").gameObject.SetActive(true);
            removeAllOfNumberButton.transform.Find("Number").GetComponent<Image>().sprite = numberSprites[selectedNumber];
            Color c = Color.white;
            if (clickTileAction == "Toggle Note 1")
                c = StaticVariables.skin.note1;
            else if (clickTileAction == "Toggle Note 2")
                c = StaticVariables.skin.note2;
            removeAllOfNumberButton.transform.Find("Number").GetComponent<Image>().color = c;

            if (clickTileAction == "Clear Tile") {
                removeAllOfNumberButton.transform.Find("Dash").gameObject.SetActive(true);
                removeAllOfNumberButton.transform.Find("Number").gameObject.SetActive(false);

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