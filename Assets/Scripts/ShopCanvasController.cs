//for Cityscapes, copyright Cole Hilscher 2020

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class ShopCanvasController : MonoBehaviour {
    //controls the shop canvas. Only one is used, and only on the shop scene.

    //the following are the costs of the various upgrades. The actual values are set in the inspector
    public int medCityPrice = 10;
    public int largeCityPrice = 40;
    public int hugeCityPrice = 100;
    public int notes1Price = 10;
    public int notes2Price = 10;
    public int residentColorPrice = 10;
    public int undoRedoPrice = 10;
    public int removeAllPrice = 10;
    public int clearPrice = 10;
    public int highlightBuildingsPrice = 10;
    
    //the cost of different skins based on their tiers
    public int skinTier1Price = 10;
    public int skinTier2Price = 20;

    //the gameobjects used to display the player's coin amounts
    public GameObject coinsBox1s;
    public GameObject coinsBox10s;
    public GameObject coinsBox100s;
    public GameObject coinsBox1000s;
    public GameObject coinsBox10000s;
    //the sprites below are derived from the coins box gameObjects
    private SpriteRenderer sprite1s;
    private SpriteRenderer sprite10s;
    private SpriteRenderer sprite100s;
    private SpriteRenderer sprite1000s;
    private SpriteRenderer sprite10000s;
    
    //the following are the buttons that expand to show purchasable upgrades
    public GameObject expandMedButton;
    public GameObject expandLargeButton;
    public GameObject expandHugeButton;
    public GameObject expandNotes1Button;
    public GameObject expandNotes2Button;
    public GameObject expandResidentColorButton;
    public GameObject expandUndoRedoButton;
    public GameObject expandRemoveAllButton;
    public GameObject expandClearButton;
    public GameObject expandHighlightBuildingsButton;
    private GameObject[] skinButtons; // a list of all skins. This will expand as more skins are added, and can be done entirely within the inspector

    //the following text elements appear when the player has purchased all upgrades of a specific category
    public GameObject unlockedEverythingText;
    public GameObject unlockedAllFeaturesText;
    public GameObject unlockedAllSkinsText;

    //the following are headers placed above the "new features" (upgrades) category, and the "cosmetics" (skins) category.
    //they are dynamically hidden if the player purchased all items of their category
    public GameObject newFeaturesText;
    public GameObject cosmeticsText;


    public GameObject blackForeground; //used to transition to/from the puzzle menu
    private SpriteRenderer blackSprite; //derived from the blackForeground gameObject
    public float fadeOutTime; //total time for fade-out, from complete light to complete darkness
    public float fadeInTime; //total time for fade-in, from complete darkness to complete light
    private float fadeTimer; //the timer on which the fadeout and fadein mechanics operate
    public GameObject scrollView; //the gameobject that is used to hide all buttons and text outside of the scrollable shop window
    public Sprite[] numbers = new Sprite[10]; //the sprites for numbers 0-9

    //the following are the various colors used in the shop's scrollable window. These do not vary with skin and are modified in the inspector
    public string affordableCoinColor;
    public string unaffordableCoinColor;
    private Color affordableColor;
    private Color unaffordableColor;
    public string purchaseButtonExterior;
    public string purchaseButtonInterior;
    public string noPurchaseButtonExterior;
    public string noPurchaseButtonInterior;
    private Color purchaseButtonExteriorColor;
    private Color purchaseButtonInteriorColor;
    private Color noPurchaseButtonExteriorColor;
    private Color noPurchaseButtonInteriorColor;

    private GameObject expandedButton; //which button is currently expanded
    public GameObject skinsStart; //the object right before the first skin's expand-button
    public GameObject skinsEnd; //the object right after the last skin's expand-button

    //the following are properties taken from the skin
    public GameObject background;
    public GameObject menuButton;


    private void Start() {
        //set some private variables based on elements edited by the inspector
        ColorUtility.TryParseHtmlString(affordableCoinColor, out affordableColor);
        ColorUtility.TryParseHtmlString(unaffordableCoinColor, out unaffordableColor);
        ColorUtility.TryParseHtmlString(purchaseButtonExterior, out purchaseButtonExteriorColor);
        ColorUtility.TryParseHtmlString(purchaseButtonInterior, out purchaseButtonInteriorColor);
        ColorUtility.TryParseHtmlString(noPurchaseButtonExterior, out noPurchaseButtonExteriorColor);
        ColorUtility.TryParseHtmlString(noPurchaseButtonInterior, out noPurchaseButtonInteriorColor);
        sprite1s = coinsBox1s.GetComponent<SpriteRenderer>();
        sprite10s = coinsBox10s.GetComponent<SpriteRenderer>();
        sprite100s = coinsBox100s.GetComponent<SpriteRenderer>();
        sprite1000s = coinsBox1000s.GetComponent<SpriteRenderer>();
        sprite10000s = coinsBox10000s.GetComponent<SpriteRenderer>();
        blackSprite = blackForeground.GetComponent<SpriteRenderer>();

        //show the amount of coins the player has, and also the cost of various upgrades
        displayCoinsAmount();
        displayCoinsOnButton(expandMedButton, medCityPrice);
        displayCoinsOnButton(expandLargeButton, largeCityPrice);
        displayCoinsOnButton(expandHugeButton, hugeCityPrice);
        displayCoinsOnButton(expandNotes1Button, notes1Price);
        displayCoinsOnButton(expandNotes2Button, notes2Price);
        displayCoinsOnButton(expandResidentColorButton, residentColorPrice);
        displayCoinsOnButton(expandUndoRedoButton, undoRedoPrice);
        displayCoinsOnButton(expandRemoveAllButton, removeAllPrice);
        displayCoinsOnButton(expandClearButton, clearPrice);
        displayCoinsOnButton(expandHighlightBuildingsButton, highlightBuildingsPrice);

        //show the cost of the various skins
        findSkinButtons();
        foreach (GameObject parent in skinButtons) {
            displayCoinsOnButton(parent.transform.Find("Expand Button").gameObject, getSkinPrice((InterfaceFunctions.getSkinFromName(parent.name))));
        }

        //update the buttons for the various upgrades to denote if they can be purchased right now
        updateButtons();

        //apply the cosmetics from the current skin
        background.GetComponent<SpriteRenderer>().sprite = StaticVariables.skin.shopBackground;
        InterfaceFunctions.colorMenuButton(menuButton);

        //starts the fade-in process, which is carried out in the Update function
        if (StaticVariables.isFading && StaticVariables.fadingTo == "shop") {
            fadeTimer = fadeInTime;
        }

    }

    private void Update() {
        //if the player is fading from the shop scene back to the main menu, this block handles that fading process
        if (StaticVariables.isFading && StaticVariables.fadingFrom == "shop") {
            fadeTimer -= Time.deltaTime;
            Color c = blackSprite.color;
            c.a = (fadeOutTime - fadeTimer) / fadeOutTime;
            blackSprite.color = c;
            if (fadeTimer <= 0f) { //after the fade timer is done, and the screen is dark, transition to another scene.
                if (StaticVariables.fadingTo == "menu") { SceneManager.LoadScene("MainMenu"); }
            }
        }
        //if the player is fading into the shop scene, this block handles that fading process
        if (StaticVariables.isFading && StaticVariables.fadingTo == "shop") {
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
        //if the player presses their phone's back button, call the goToMainMenu function
        //identical to if the player just pushed the on-screen menu button
        if (Input.GetKeyDown(KeyCode.Escape)) {
            goToMainMenu();
        }
    }

    // ---------------------------------------------------
    //ALL OF THE FUNCTIONS THAT ARE USED TO LEAVE THE SHOP SCENE
    // ---------------------------------------------------

    public void goToMainMenu() {
        //pushing the on-screen menu button calls this function, which returns the player to the main menu
        if (!StaticVariables.isFading) {
            StaticVariables.fadingTo = "menu";
            startFadeOut();
        }
        //if the player pushes the menu button while the scene is still fading in, then implement the button press after the fade-in is done
        else {
            StaticVariables.waitingOnButtonClickAfterFadeIn = true;
            StaticVariables.buttonClickInWaiting = "menu";
        }
    }

    public void startFadeOut() {
        //begin the fade-out process. This function is called by goToMainMenu
        //the fade-out mechanics of darkening the screen are implemented in the Update function
        fadeTimer = fadeOutTime;
        StaticVariables.isFading = true;
        StaticVariables.fadingFrom = "shop";
    }

    private void OnApplicationQuit() {
        SaveSystem.SaveGame();
    }

    // ---------------------------------------------------
    //ALL OF THE FUNCTIONS THAT ARE USED TO UPDATE THE VISUALS FOR THE SHOP SCENE
    // ---------------------------------------------------

    public void displayCoinsAmount() {
        //show the amount of coins the player has in the top-right corner of the shop screen
        int value1s = StaticVariables.coins % 10;
        int value10s = (StaticVariables.coins / 10) % 10;
        int value100s = (StaticVariables.coins / 100) % 10;
        int value1000s = (StaticVariables.coins / 1000) % 10;
        int value10000s = (StaticVariables.coins / 10000) % 10;
        sprite1s.sprite = numbers[value1s];
        sprite10s.sprite = numbers[value10s];
        sprite100s.sprite = numbers[value100s];
        sprite1000s.sprite = numbers[value1000s];
        sprite10000s.sprite = numbers[value10000s];

        coinsBox1s.SetActive(true);
        coinsBox10s.SetActive(true);
        coinsBox100s.SetActive(true);
        coinsBox1000s.SetActive(true);
        coinsBox10000s.SetActive(true);

        if (value10000s == 0) {
            coinsBox10000s.SetActive(false);
            if (value1000s == 0) {
                coinsBox1000s.SetActive(false);
                if (value100s == 0) {
                    coinsBox100s.SetActive(false);
                    if (value10s == 0) {
                        coinsBox10s.SetActive(false);
                    }
                }
            }
        }
    }
    
    private void updateButtons() {
        //updates every button's color to denote if the player can purchase it
        //each button has a price that must be met, and also some have a prerequisite purchase that must be made
        Color grey = Color.grey;
        updateButton(expandMedButton, StaticVariables.unlockedMedium, medCityPrice);
        updateButton(expandLargeButton, StaticVariables.unlockedLarge, largeCityPrice, StaticVariables.unlockedMedium);
        updateButton(expandHugeButton, StaticVariables.unlockedHuge, hugeCityPrice, StaticVariables.unlockedLarge);
        updateButton(expandNotes1Button, StaticVariables.unlockedNotes1, notes1Price);
        updateButton(expandNotes2Button, StaticVariables.unlockedNotes2, notes2Price, StaticVariables.unlockedNotes1);
        updateButton(expandResidentColorButton, StaticVariables.unlockedResidentsChangeColor, residentColorPrice);
        updateButton(expandUndoRedoButton, StaticVariables.unlockedUndoRedo, undoRedoPrice);
        updateButton(expandRemoveAllButton, StaticVariables.unlockedRemoveAllOfNumber, removeAllPrice, StaticVariables.unlockedUndoRedo);
        updateButton(expandClearButton, StaticVariables.unlockedClearPuzzle, clearPrice, StaticVariables.unlockedUndoRedo);
        updateButton(expandHighlightBuildingsButton, StaticVariables.unlockedHighlightBuildings, highlightBuildingsPrice);

        foreach (GameObject parent in skinButtons) {
            updateButton(parent.transform.Find("Expand Button").gameObject, StaticVariables.unlockedSkins.Contains(InterfaceFunctions.getSkinFromName(parent.name)), getSkinPrice((InterfaceFunctions.getSkinFromName(parent.name))));
        }

        //also update text shown when all upgrades of a single type have been purchased.
        //these texts should only appear if the player chooses to hide purchased upgrades
        bool allCities = StaticVariables.unlockedMedium && StaticVariables.unlockedLarge && StaticVariables.unlockedHuge;
        bool allFeatures = StaticVariables.unlockedNotes1 && StaticVariables.unlockedNotes2 && StaticVariables.unlockedResidentsChangeColor && StaticVariables.unlockedUndoRedo && StaticVariables.unlockedRemoveAllOfNumber && StaticVariables.unlockedClearPuzzle && StaticVariables.unlockedHighlightBuildings;
        bool allSkins = StaticVariables.unlockedSkins.Count + 1 == StaticVariables.allSkins.Length;
        bool allContent = allCities && allFeatures && allSkins;
        unlockedAllFeaturesText.SetActive(allFeatures && allCities && StaticVariables.hidePurchasedUpgrades);
        unlockedAllSkinsText.SetActive(allSkins && StaticVariables.hidePurchasedUpgrades);
        unlockedEverythingText.SetActive(allContent);
        newFeaturesText.SetActive(true);
        cosmeticsText.SetActive(true);
        if (allContent && StaticVariables.hidePurchasedUpgrades) {
            unlockedAllFeaturesText.SetActive(false);
            unlockedAllSkinsText.SetActive(false);
            newFeaturesText.SetActive(false);
            cosmeticsText.SetActive(false);
        }

        //after all of that, resize the scroll view
        //setScrollViewHeight();
    }

    private void updateButton(GameObject button, bool condition, int cost, bool uniqueUnlockCondition = true) {
        //shows if the item has already been purchased or not, and also sets the coin amount to the appropriate color
        //uniqueUnlockCondition for purchasing the large puzzle would be that the medium puzzle has to already have been purchased

        //by default, show the button
        button.transform.parent.gameObject.SetActive(true);
        if (condition) {
            //hide coins
            button.transform.Find("Coins").gameObject.SetActive(false);
            //show purchase complete symbol
            button.transform.Find("Purchased Symbol").gameObject.SetActive(true);
            //if the upgrade has been purchased and purchases are supposed to be hidden, then hide the button
            if (StaticVariables.hidePurchasedUpgrades) { button.transform.parent.gameObject.SetActive(false); }
        }
        else {
            //show coins
            button.transform.Find("Coins").gameObject.SetActive(true);
            //hide purchase complete symbol
            button.transform.Find("Purchased Symbol").gameObject.SetActive(false);
        }

        //set the coin cost colors and the text shown in place of the unlock button depending on if the player can make the purchase
        GameObject coinObject = button.transform.Find("Coins").Find("Coin").gameObject;
        GameObject imageObject1s = button.transform.Find("Coins").Find("Coins - 1s").gameObject;
        GameObject imageObject10s = button.transform.Find("Coins").Find("Coins - 10s").gameObject;
        GameObject imageObject100s = button.transform.Find("Coins").Find("Coins - 100s").gameObject;
        GameObject imageObject1000s = button.transform.Find("Coins").Find("Coins - 1000s").gameObject;

        GameObject unlockButton = button.transform.parent.Find("Dropdown").Find("Unlock").gameObject;
        unlockButton.transform.GetChild(1).gameObject.SetActive(false); //unlock text
        unlockButton.transform.GetChild(2).gameObject.SetActive(false); //already owned text
        unlockButton.transform.GetChild(3).gameObject.SetActive(false); //cannot afford text
        if (unlockButton.transform.childCount >= 5) { unlockButton.transform.GetChild(4).gameObject.SetActive(false); }//prerequisite not yet purchased text

        if (cost <= StaticVariables.coins) {
            coinObject.GetComponent<Image>().color = Color.white;
            imageObject1s.GetComponent<Image>().color = affordableColor;
            imageObject10s.GetComponent<Image>().color = affordableColor;
            imageObject100s.GetComponent<Image>().color = affordableColor;
            imageObject1000s.GetComponent<Image>().color = affordableColor;
        }
        else {
            coinObject.GetComponent<Image>().color = unaffordableColor;
            imageObject1s.GetComponent<Image>().color = unaffordableColor;
            imageObject10s.GetComponent<Image>().color = unaffordableColor;
            imageObject100s.GetComponent<Image>().color = unaffordableColor;
            imageObject1000s.GetComponent<Image>().color = unaffordableColor;
        }
        if (!uniqueUnlockCondition) {
            unlockButton.transform.GetChild(0).Find("Borders").GetComponent<Image>().color = noPurchaseButtonExteriorColor;
            unlockButton.transform.GetChild(0).Find("Interior").GetComponent<Image>().color = noPurchaseButtonInteriorColor;
            unlockButton.transform.GetChild(4).gameObject.SetActive(true);
        }
        else if (condition) {
            unlockButton.transform.GetChild(0).Find("Borders").GetComponent<Image>().color = noPurchaseButtonExteriorColor;
            unlockButton.transform.GetChild(0).Find("Interior").GetComponent<Image>().color = noPurchaseButtonInteriorColor;
            unlockButton.transform.GetChild(2).gameObject.SetActive(true);
        }
        else if (cost > StaticVariables.coins) {
            unlockButton.transform.GetChild(0).Find("Borders").GetComponent<Image>().color = noPurchaseButtonExteriorColor;
            unlockButton.transform.GetChild(0).Find("Interior").GetComponent<Image>().color = noPurchaseButtonInteriorColor;
            unlockButton.transform.GetChild(3).gameObject.SetActive(true);
        }
        else {
            unlockButton.transform.GetChild(0).Find("Borders").GetComponent<Image>().color = purchaseButtonExteriorColor;
            unlockButton.transform.GetChild(0).Find("Interior").GetComponent<Image>().color = purchaseButtonInteriorColor;
            unlockButton.transform.GetChild(1).gameObject.SetActive(true);
        }

    }
    
    public void pushButton(GameObject parent) {
        //takes the parent of the current button as a parameter. Expands/contracts child dropdown
        clickedButton(parent.transform.Find("Expand Button").gameObject);
    }


    public void expandSiblings(GameObject button) {
        //sets all siblings of the chosen button to be active, and resizes the scroll view
        expandedButton = button;

        GameObject parentBox = button.transform.parent.gameObject;
        bool switchTo = true;

        for (int i = 1; i < parentBox.transform.childCount; i++) {
            parentBox.transform.GetChild(i).gameObject.SetActive(switchTo);
        }
        resizeToFitChildren(parentBox, false);
        //setScrollViewHeight();
    }

    public void contractSiblings(GameObject button) {
        //sets all siblings of the chosen button to be inactive, and resizes the scroll view
        if (expandedButton == button) {
            expandedButton = null;
        }
        GameObject parentBox = button.transform.parent.gameObject;
        bool switchTo = false;

        for (int i = 1; i < parentBox.transform.childCount; i++) {
            parentBox.transform.GetChild(i).gameObject.SetActive(switchTo);
        }
        resizeToFitChildren(parentBox, false);
        //setScrollViewHeight();
    }

    public void contractPreviousExpansion() {
        contractSiblings(expandedButton);
    }

    public void clickedButton(GameObject button) {
        //expands/contracts the children of the button that the player clicked. Also contracts the previous expanded button, if there is one
        if (expandedButton == null) {
            expandSiblings(button);
        }
        else if (expandedButton == button) {
            contractPreviousExpansion();
        }
        else {
            contractPreviousExpansion();
            expandSiblings(button);
        }
    }
    
    public void resizeToFitChildren(GameObject parent, bool minSize) {
        //resizes the bounds of an element so that all of the child gameobjects are visibile. Used for expandButton and setScrollViewHeight

        //get height of contents, including spaces between objects and top and bottom padding
        //if minSize is true, the container height has to be at least the height of its parent
        float newHeight = 0f;
        int activeCount = 0;
        for (int i = 0; i < parent.transform.childCount; i++) {
            float h = parent.transform.GetChild(i).GetComponent<RectTransform>().sizeDelta.y;
            if (parent.transform.GetChild(i).gameObject.activeSelf) {
                newHeight += h;
                activeCount++;
            }
        }
        newHeight += ((activeCount - 1) * parent.GetComponent<VerticalLayoutGroup>().spacing);
        newHeight += parent.GetComponent<VerticalLayoutGroup>().padding.top + parent.GetComponent<VerticalLayoutGroup>().padding.bottom;

        //set the container height to be at least the height of the parent
        if (minSize && newHeight < parent.transform.parent.GetComponent<RectTransform>().sizeDelta.y) { newHeight = parent.transform.parent.GetComponent<RectTransform>().sizeDelta.y; }

        Vector2 newSize = new Vector2(parent.GetComponent<RectTransform>().sizeDelta.x, newHeight);
        parent.GetComponent<RectTransform>().sizeDelta = newSize;
    }

    public void displayCoinsOnButton(GameObject button, int cost) {
        //assumes coins are all image components, with the ones place being child(1), all the way through thousands place being child(4).
        //also colors the coin amount depending on if the player can afford them.

        GameObject imageObject1s = button.transform.Find("Coins").Find("Coins - 1s").gameObject;
        GameObject imageObject10s = button.transform.Find("Coins").Find("Coins - 10s").gameObject;
        GameObject imageObject100s = button.transform.Find("Coins").Find("Coins - 100s").gameObject;
        GameObject imageObject1000s = button.transform.Find("Coins").Find("Coins - 1000s").gameObject;

        int value1s = cost % 10;
        int value10s = (cost / 10) % 10;
        int value100s = (cost / 100) % 10;
        int value1000s = (cost / 1000) % 10;
        imageObject1s.GetComponent<Image>().sprite = numbers[value1s];
        imageObject10s.GetComponent<Image>().sprite = numbers[value10s];
        imageObject100s.GetComponent<Image>().sprite = numbers[value100s];
        imageObject1000s.GetComponent<Image>().sprite = numbers[value1000s];

        imageObject1s.SetActive(true);
        imageObject10s.SetActive(true);
        imageObject100s.SetActive(true);
        imageObject1000s.SetActive(true);

        if (value1000s == 0) {
            imageObject1000s.SetActive(false);
            if (value100s == 0) {
                imageObject100s.SetActive(false);
                if (value10s == 0) {
                    imageObject10s.SetActive(false);
                }
            }
        }

        if (cost <= StaticVariables.coins) {
            imageObject1s.GetComponent<Image>().color = affordableColor;
            imageObject10s.GetComponent<Image>().color = affordableColor;
            imageObject100s.GetComponent<Image>().color = affordableColor;
            imageObject1000s.GetComponent<Image>().color = affordableColor;
        }
        else {
            imageObject1s.GetComponent<Image>().color = unaffordableColor;
            imageObject10s.GetComponent<Image>().color = unaffordableColor;
            imageObject100s.GetComponent<Image>().color = unaffordableColor;
            imageObject1000s.GetComponent<Image>().color = unaffordableColor;
        }
    }

    // ---------------------------------------------------
    //ALL OF THE FUNCTIONS THAT PARSE THROUGH SKIN-SPECIFIC INFORMATION
    // ---------------------------------------------------

    private void findSkinButtons() {
        //finds all of the buttons that let the player unlock various skins
        //specifically, finds all gameobjects between the skin start and and points from the inspector
        int start = skinsStart.transform.GetSiblingIndex();
        int end = skinsEnd.transform.GetSiblingIndex();
        List<GameObject> buttons = new List<GameObject>();
        GameObject parent = expandMedButton.transform.parent.parent.gameObject;
        for (int i = start + 1; i<end; i++) {
            buttons.Add(parent.transform.GetChild(i).gameObject);
        }
        skinButtons = buttons.ToArray();
    }

    private int getSkinPrice(Skin skin) {
        //gets the price in coins for a skin, based on the skinTier from the skin's script and the corresponding price from the inspector in the shop
        switch (skin.skinTier) {
            case 1:
                return skinTier1Price;
            case 2:
                return skinTier2Price;
        }
        return 0;
    }

    // ---------------------------------------------------
    //ALL OF THE FUNCTIONS THAT HELP PURCHASE AN UPGRADE. FUNCTIONS THAT MAKE THE PURCHASE FOR ONE SPECIFIC UPGRADE ARE LATER
    // ---------------------------------------------------

    private bool canPurchase(bool notCond, int cost) {
        //if the prerequisite purchase condition is met, and the upgrade's cost is met, then returns true
        //used in the unlock functions
        return (!notCond && StaticVariables.coins >= cost);
    }

    private void doPurchase(int price) {
        //purchases an upgrade and contracts the expanded drop-down under the purchased upgrade's title
        //used in the unlock functions
        makePurchase(price);
        updateButtons();
        contractPreviousExpansion();

        SaveSystem.SaveGame();
    }

    public void makePurchase(int cost) {
        //deducts the cost of a purchase from the player's coin total, and displays the new amount
        StaticVariables.coins -= cost;
        displayCoinsAmount();
    }

    // ---------------------------------------------------
    //ALL OF THE FUNCTIONS THAT DIRECTLY UNLOCK AN UPGRADE. EACH ONE ALSO SETS THE UNLOCK AS ACTIVE, WHICH CAN BE DEACTIVATED FROM SETTINGS
    // ---------------------------------------------------

    public void unlockMedium() {
        if (canPurchase(StaticVariables.unlockedMedium,medCityPrice)) {
            StaticVariables.unlockedMedium = true;
            StaticVariables.highestUnlockedSize = 4;
            StaticVariables.showMed = true;
            doPurchase(medCityPrice);
        }
    }
    public void unlockLarge() {
        if (canPurchase(StaticVariables.unlockedLarge, largeCityPrice) && StaticVariables.unlockedMedium) {
            StaticVariables.unlockedLarge = true;
            StaticVariables.highestUnlockedSize = 5;
            StaticVariables.showMed = true;
            StaticVariables.showLarge = true;
            doPurchase(largeCityPrice);
        }
    }
    public void unlockHuge() {
        if (canPurchase(StaticVariables.unlockedHuge, hugeCityPrice) && StaticVariables.unlockedLarge && StaticVariables.unlockedMedium) {
            StaticVariables.unlockedHuge = true;
            StaticVariables.highestUnlockedSize = 6;
            StaticVariables.showMed = true;
            StaticVariables.showLarge = true;
            StaticVariables.showHuge = true;
            doPurchase(hugeCityPrice);
        }
    }
    public void unlockNotes1() {
        if (canPurchase(StaticVariables.unlockedNotes1,notes1Price)) {
            StaticVariables.unlockedNotes1 = true;
            StaticVariables.includeNotes1Button = true;
            doPurchase(notes1Price);
        }
    }
    public void unlockNotes2() {
        if (canPurchase(StaticVariables.unlockedNotes2, notes2Price) && StaticVariables.unlockedNotes1) {
            StaticVariables.unlockedNotes2 = true;
            StaticVariables.includeNotes2Button = true;
            doPurchase(notes2Price);
        }
    }
    public void unlockResidentsChangeColor() {
        if (canPurchase(StaticVariables.unlockedResidentsChangeColor,residentColorPrice)) {
            StaticVariables.unlockedResidentsChangeColor = true;
            StaticVariables.changeResidentColorOnCorrectRows = true;
            doPurchase(residentColorPrice);
        }
    }
    public void unlockUndoRedo() {
        if (canPurchase(StaticVariables.unlockedUndoRedo,undoRedoPrice)) {
            StaticVariables.unlockedUndoRedo = true;
            StaticVariables.includeUndoRedo = true;
            doPurchase(undoRedoPrice);
        }
    }
    public void unlockRemoveAllOfNumber() {
        if (canPurchase(StaticVariables.unlockedRemoveAllOfNumber, removeAllPrice) && StaticVariables.unlockedUndoRedo) {
            StaticVariables.unlockedRemoveAllOfNumber = true;
            StaticVariables.includeRemoveAllOfNumber = true;
            doPurchase(removeAllPrice);
        }
    }
    public void unlockClearPuzzle() {
        if (canPurchase(StaticVariables.unlockedClearPuzzle, clearPrice) && StaticVariables.unlockedUndoRedo) {
            StaticVariables.unlockedClearPuzzle = true;
            StaticVariables.includeClearPuzzle = true;
            doPurchase(clearPrice);
        }
    }

    public void unlockHighlightBuildings() {
        if (canPurchase(StaticVariables.unlockedHighlightBuildings, highlightBuildingsPrice)) {
            StaticVariables.unlockedHighlightBuildings = true;
            StaticVariables.includeHighlightBuildings = true;
            doPurchase(highlightBuildingsPrice);
        }
    }


    public void unlockSkin(GameObject parent) {
        Skin skin = InterfaceFunctions.getSkinFromName(parent.name);
        if (canPurchase(StaticVariables.unlockedSkins.Contains(skin), getSkinPrice(skin))){
            StaticVariables.unlockedSkins.Add(skin);
            StaticVariables.skin = skin;
            background.GetComponent<SpriteRenderer>().sprite = StaticVariables.skin.shopBackground;
            InterfaceFunctions.colorMenuButton(menuButton);
            doPurchase(getSkinPrice(skin));
        }

    }

    // ---------------------------------------------------
    //ALL OF THE FUNCTIONS THAT WERE USED IN TESTING THE SHOP FUNCTIONALITY. THE BUTTONS THAT CALL THESE FUNCTIONS ARE HIDDEN IN THE INSPECTOR
    // ---------------------------------------------------

    public void clearPurchases() {
        //the clear purchases button is only used during testing, for the purposes of testing shop mechanics 
        lockAll();
        if (StaticVariables.coins < 50) { StaticVariables.coins = 300; }
        displayCoinsAmount();
    }

    public void lockAll() {
        //lock all is also only used during testing
        StaticVariables.unlockedMedium = false;
        StaticVariables.unlockedLarge = false;
        StaticVariables.unlockedHuge = false;
        StaticVariables.highestUnlockedSize = 3;
        StaticVariables.showMed = false;
        StaticVariables.showLarge = false;
        StaticVariables.showHuge = false;

        StaticVariables.unlockedNotes1 = false;
        StaticVariables.unlockedNotes2 = false;
        StaticVariables.unlockedResidentsChangeColor = false;
        StaticVariables.unlockedUndoRedo = false;
        StaticVariables.unlockedRemoveAllOfNumber = false;
        StaticVariables.unlockedClearPuzzle = false;
        StaticVariables.unlockedHighlightBuildings = false;

        StaticVariables.includeNotes1Button = false;
        StaticVariables.includeNotes2Button = false;
        StaticVariables.changeResidentColorOnCorrectRows = false;
        StaticVariables.includeUndoRedo = false;
        StaticVariables.includeRemoveAllOfNumber = false;
        StaticVariables.includeClearPuzzle = false;
        StaticVariables.includeHighlightBuildings = false;

        StaticVariables.unlockedSkins = new List<Skin>();
        StaticVariables.skin = InterfaceFunctions.getDefaultSkin();
        background.GetComponent<SpriteRenderer>().sprite = StaticVariables.skin.shopBackground;
        InterfaceFunctions.colorMenuButton(menuButton);
        updateButtons();
    }

    public void unlockAll() {
        //unlock all is also only used during testing
        StaticVariables.unlockedMedium = true;
        StaticVariables.unlockedLarge = true;
        StaticVariables.unlockedHuge = true;
        StaticVariables.highestUnlockedSize = 6;
        StaticVariables.showMed = true;
        StaticVariables.showLarge = true;
        StaticVariables.showHuge = true;

        StaticVariables.unlockedNotes1 = true;
        StaticVariables.unlockedNotes2 = true;
        StaticVariables.unlockedResidentsChangeColor = true;
        StaticVariables.unlockedUndoRedo = true;
        StaticVariables.unlockedRemoveAllOfNumber = true;
        StaticVariables.unlockedClearPuzzle = true;
        StaticVariables.unlockedHighlightBuildings = true;

        StaticVariables.includeNotes1Button = true;
        StaticVariables.includeNotes2Button = true;
        StaticVariables.changeResidentColorOnCorrectRows = true;
        StaticVariables.includeUndoRedo = true;
        StaticVariables.includeRemoveAllOfNumber = true;
        StaticVariables.includeClearPuzzle = true;
        StaticVariables.includeHighlightBuildings = true;

        unlockAllSkins();
        updateButtons();
    }

    private void unlockAllSkins() {
        //unlockAllSkins is also only used during testing
        foreach (Skin skin in StaticVariables.allSkins) {
            if (skin != InterfaceFunctions.getDefaultSkin() && !StaticVariables.unlockedSkins.Contains(skin)) {
                StaticVariables.unlockedSkins.Add(skin);
            }

        }
    }

    public void addCoins() {
        //addCoins is also only used during testing
        StaticVariables.coins += 400;
        displayCoinsAmount();
        updateButtons();
    }

    public void foreshInstall() {
        //the name of this function is misspelled, but that is okay because this is also only used during testing
        removeCoins();
        lockAll();
        StaticVariables.hasBeatenTutorial = false;
        goToMainMenu();
    }

    public void removeCoins() {
        //also only used during testing
        StaticVariables.coins = 0;
        displayCoinsAmount();
    }
}