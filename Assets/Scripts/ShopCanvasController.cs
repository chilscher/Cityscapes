//for Cityscapes, copyright Cole Hilscher 2024

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using DG.Tweening;

public class ShopCanvasController : MonoBehaviour {
    //controls the shop canvas. Only one is used, and only on the shop scene.

    //the following are the costs of the various upgrades. The actual values are set in the inspector
    public int medCityPrice = 10;
    public int largeCityPrice = 40;
    public int hugeCityPrice = 100;
    public int massiveCityPrice = 300;
    public int notes1Price = 10;
    public int notes2Price = 10;
    public int residentColorPrice = 10;
    public int undoRedoPrice = 10;
    public int removeAllPrice = 10;
    public int clearPrice = 10;
    public int highlightBuildingsPrice = 10;
    public int buildingQuantityStatusPrice = 10;

    //the gameobjects used to display the player's coin amounts
    public GameObject coinsBox1s;
    public GameObject coinsBox10s;
    public GameObject coinsBox100s;
    public GameObject coinsBox1000s;
    public GameObject coinsBox10000s;
    //the sprites below are derived from the coins box gameObjects
    private Image sprite1s;
    private Image sprite10s;
    private Image sprite100s;
    private Image sprite1000s;
    private Image sprite10000s;
    
    //the following are the buttons that expand to show purchasable upgrades
    public GameObject expandMedButton;
    public GameObject expandLargeButton;
    public GameObject expandHugeButton;
    public GameObject expandMassiveButton;
    public GameObject expandNotes1Button;
    public GameObject expandNotes2Button;
    public GameObject expandResidentColorButton;
    public GameObject expandUndoRedoButton;
    public GameObject expandRemoveAllButton;
    public GameObject expandClearButton;
    public GameObject expandHighlightBuildingsButton;
    public GameObject expandBuildingQuantityStatusButton;
    private GameObject[] skinButtons; // a list of all skins. This will expand as more skins are added, and can be done entirely within the inspector

    //the following text elements appear when the player has purchased all upgrades of a specific category
    public GameObject unlockedAllFeaturesText;
    public GameObject unlockedAllSkinsText;
    public GameObject skinPriceDisclaimerText;

    //the following are headers placed above the "new features" (upgrades) category, and the "cosmetics" (skins) category.
    //they are dynamically hidden if the player purchased all items of their category
    public GameObject newFeaturesText;
    public GameObject cosmeticsText;

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
    public GameObject settingsButton;
    private bool stopScrollRect = false;
    public Image popupBorder;
    public Image popupInside;

    public GameObject skinPreview;
    public Image skinPreviewBorder;
    public Image skinPreviewInside;
    public Image skinPreviewBlackBackground;
    public Image skinPreviewImage;
    public Text skinPreviewText;
    private Color previewBackgroundColor;
    public Transform skinPreviewBackButton;


    private void Start() {
        //set some private variables based on elements edited by the inspector
        ColorUtility.TryParseHtmlString(affordableCoinColor, out affordableColor);
        ColorUtility.TryParseHtmlString(unaffordableCoinColor, out unaffordableColor);
        ColorUtility.TryParseHtmlString(purchaseButtonExterior, out purchaseButtonExteriorColor);
        ColorUtility.TryParseHtmlString(purchaseButtonInterior, out purchaseButtonInteriorColor);
        ColorUtility.TryParseHtmlString(noPurchaseButtonExterior, out noPurchaseButtonExteriorColor);
        ColorUtility.TryParseHtmlString(noPurchaseButtonInterior, out noPurchaseButtonInteriorColor);
        sprite1s = coinsBox1s.GetComponent<Image>();
        sprite10s = coinsBox10s.GetComponent<Image>();
        sprite100s = coinsBox100s.GetComponent<Image>();
        sprite1000s = coinsBox1000s.GetComponent<Image>();
        sprite10000s = coinsBox10000s.GetComponent<Image>();

        //show the amount of coins the player has, and also the cost of various upgrades
        DisplayCoinsAmount();
        DisplayCoinsOnButton(expandMedButton, medCityPrice);
        DisplayCoinsOnButton(expandLargeButton, largeCityPrice);
        DisplayCoinsOnButton(expandHugeButton, hugeCityPrice);
        DisplayCoinsOnButton(expandMassiveButton, massiveCityPrice);
        DisplayCoinsOnButton(expandNotes1Button, notes1Price);
        DisplayCoinsOnButton(expandNotes2Button, notes2Price);
        DisplayCoinsOnButton(expandResidentColorButton, residentColorPrice);
        DisplayCoinsOnButton(expandUndoRedoButton, undoRedoPrice);
        DisplayCoinsOnButton(expandRemoveAllButton, removeAllPrice);
        DisplayCoinsOnButton(expandClearButton, clearPrice);
        DisplayCoinsOnButton(expandHighlightBuildingsButton, highlightBuildingsPrice);
        DisplayCoinsOnButton(expandBuildingQuantityStatusButton, buildingQuantityStatusPrice);

        //show the cost of the various skins
        FindSkinButtons();
        foreach (GameObject parent in skinButtons)
            DisplayCoinsOnButton(parent.transform.Find("Expand Button").gameObject, GetSkinPrice((InterfaceFunctions.GetSkinFromName(parent.name))));

        //update the buttons for the various upgrades to denote if they can be purchased right now
        UpdateButtons();

        ApplySkin();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
            StaticVariables.FadeOutThenLoadScene("MainMenu");
    }

    void LateUpdate(){
        if (stopScrollRect){
            stopScrollRect = false;
            scrollView.GetComponent<ScrollRect>().StopMovement();
        }

    }

    // ---------------------------------------------------
    //ALL OF THE FUNCTIONS THAT ARE USED TO LEAVE THE SHOP SCENE
    // ---------------------------------------------------

    public void PushMainMenuButton() {
        StaticVariables.FadeOutThenLoadScene("MainMenu");
    }
    public void PushSettingsButton() {
        StaticVariables.FadeOutThenLoadScene("Settings");
    }

    private void OnApplicationQuit() {
        SaveSystem.SaveGame();
    }

    // ---------------------------------------------------
    //ALL OF THE FUNCTIONS THAT ARE USED TO UPDATE THE VISUALS FOR THE SHOP SCENE
    // ---------------------------------------------------

    private void ApplySkin(){
        background.GetComponent<Image>().sprite = StaticVariables.skin.mainMenuBackground;
        InterfaceFunctions.ColorMenuButton(menuButton, StaticVariables.skin);
        InterfaceFunctions.ColorMenuButton(settingsButton, StaticVariables.skin);
        popupBorder.color = StaticVariables.skin.popupBorder;
        popupInside.color = StaticVariables.skin.popupInside;
        skinPreviewBorder.color = StaticVariables.skin.popupBorder;
        skinPreviewInside.color = StaticVariables.skin.popupInside;
        skinPreviewBackButton.Find("Borders").GetComponent<Image>().color = StaticVariables.skin.menuButtonBorder;
        skinPreviewBackButton.Find("Interior").GetComponent<Image>().color = StaticVariables.skin.menuButtonInside;

        ColorShopButton(expandResidentColorButton);
        ColorShopButton(expandHighlightBuildingsButton);
        ColorShopButton(expandMedButton);
        ColorShopButton(expandUndoRedoButton);
        ColorShopButton(expandNotes1Button);
        ColorShopButton(expandRemoveAllButton);
        ColorShopButton(expandClearButton);
        ColorShopButton(expandLargeButton);
        ColorShopButton(expandNotes2Button);
        ColorShopButton(expandHugeButton);
        ColorShopButton(expandMassiveButton);
        ColorShopButton(expandBuildingQuantityStatusButton);

        foreach(GameObject go in skinButtons)
            ColorShopButton(go.transform.GetChild(0).gameObject);
        
    }

    private void ColorShopButton(GameObject button){
        button.transform.GetChild(0).Find("Borders").GetComponent<Image>().color = StaticVariables.skin.menuButtonBorder;
        button.transform.GetChild(0).Find("Interior").GetComponent<Image>().color = StaticVariables.skin.menuButtonInside;
        Transform dropdown = button.transform.parent.Find("Dropdown");
        dropdown.Find("Dropdown Image").Find("Exterior").GetComponent<Image>().color = StaticVariables.skin.menuButtonBorder;
        dropdown.Find("Dropdown Image").Find("Interior").GetComponent<Image>().color = StaticVariables.skin.menuButtonInside;
    }
    public void DisplayCoinsAmount() {
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
    
    private void UpdateButtons() {
        //updates every button's color to denote if the player can purchase it
        //each button has a price that must be met, and also some have a prerequisite purchase that must be made
        Color grey = Color.grey;
        UpdateButton(expandMedButton, StaticVariables.unlockedMedium, medCityPrice);
        UpdateButton(expandLargeButton, StaticVariables.unlockedLarge, largeCityPrice, StaticVariables.unlockedMedium);
        UpdateButton(expandHugeButton, StaticVariables.unlockedHuge, hugeCityPrice, StaticVariables.unlockedLarge);
        UpdateButton(expandMassiveButton, StaticVariables.unlockedMassive, massiveCityPrice, StaticVariables.unlockedHuge);
        UpdateButton(expandNotes1Button, StaticVariables.unlockedNotes1, notes1Price);
        UpdateButton(expandNotes2Button, StaticVariables.unlockedNotes2, notes2Price, StaticVariables.unlockedNotes1);
        UpdateButton(expandResidentColorButton, StaticVariables.unlockedResidentsChangeColor, residentColorPrice);
        UpdateButton(expandUndoRedoButton, StaticVariables.unlockedUndoRedo, undoRedoPrice);
        UpdateButton(expandRemoveAllButton, StaticVariables.unlockedRemoveAllOfNumber, removeAllPrice, StaticVariables.unlockedUndoRedo);
        UpdateButton(expandClearButton, StaticVariables.unlockedClearPuzzle, clearPrice, StaticVariables.unlockedUndoRedo);
        UpdateButton(expandHighlightBuildingsButton, StaticVariables.unlockedHighlightBuildings, highlightBuildingsPrice);
        UpdateButton(expandBuildingQuantityStatusButton, StaticVariables.unlockedBuildingQuantityStatus, buildingQuantityStatusPrice);

        foreach (GameObject parent in skinButtons)
            UpdateButton(parent.transform.Find("Expand Button").gameObject, StaticVariables.unlockedSkins.Contains(InterfaceFunctions.GetSkinFromName(parent.name)), GetSkinPrice((InterfaceFunctions.GetSkinFromName(parent.name))));

        //also update text shown when all upgrades of a single type have been purchased.
        //these texts should only appear if the player chooses to hide purchased upgrades
        bool allCities = StaticVariables.unlockedMedium && StaticVariables.unlockedLarge && StaticVariables.unlockedHuge && StaticVariables.unlockedMassive;
        bool allFeatures = StaticVariables.unlockedNotes1 && StaticVariables.unlockedNotes2 && StaticVariables.unlockedResidentsChangeColor && StaticVariables.unlockedUndoRedo && StaticVariables.unlockedRemoveAllOfNumber && StaticVariables.unlockedClearPuzzle && StaticVariables.unlockedHighlightBuildings && StaticVariables.unlockedBuildingQuantityStatus;
        bool allSkins = StaticVariables.unlockedSkins.Count == StaticVariables.allSkins.Length;

        newFeaturesText.SetActive(true);
        unlockedAllFeaturesText.SetActive(allCities && allFeatures);
        cosmeticsText.SetActive(true);
        unlockedAllSkinsText.SetActive(allSkins);
        skinPriceDisclaimerText.SetActive(!allSkins);
    }

    private void UpdateButton(GameObject button, bool condition, int cost, bool uniqueUnlockCondition = true) {
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

    public void PushPreviewSkinButton(GameObject parent){
        //background.GetComponent<Image>().sprite = InterfaceFunctions.GetSkinFromName(parent.name).mainMenuBackground;
        skinPreviewImage.sprite = InterfaceFunctions.GetSkinFromName(parent.name).mainMenuBackground;
        skinPreviewText.text = parent.name.ToUpper() + "\nSKIN PREVIEW";

        Color nextColor = skinPreviewBlackBackground.color;
        Color currentColor = Color.black;
        currentColor.a = 0;
        skinPreviewBlackBackground.color = currentColor;
        skinPreviewBlackBackground.gameObject.SetActive(true);
        skinPreviewBlackBackground.DOColor(nextColor, 0.5f);

        skinPreview.transform.parent.gameObject.SetActive(true);
        skinPreview.transform.localScale = Vector3.zero;
        skinPreview.transform.DOScale(Vector3.one, 0.5f);
    }

    public void PushClosePreviewButton(){
        Color nextColor = Color.black;
        nextColor.a = 0;
        previewBackgroundColor = skinPreviewBlackBackground.color;
        skinPreviewBlackBackground.gameObject.SetActive(true);
        skinPreviewBlackBackground.DOColor(nextColor, 0.3f);
        skinPreview.transform.DOScale(Vector3.zero, 0.3f);
        StaticVariables.WaitTimeThenCallFunction(0.3f, ClosePreview);
    }

    private void ClosePreview(){
        skinPreview.transform.parent.gameObject.SetActive(false);
        skinPreviewBlackBackground.color = previewBackgroundColor;
    }
    
    public void PushExpandContractButton(GameObject parent) {
        //takes the parent of the current button as a parameter.
        //expands/contracts the children of the button that the player clicked. Also contracts the previous expanded button, if there is one
        GameObject button = parent.transform.Find("Expand Button").gameObject;
        if (expandedButton == null)
            ExpandSiblings(button);
        else if (expandedButton == button)
            ContractPreviousExpansion();
        else {
            ContractPreviousExpansion();
            ExpandSiblings(button);
        }
    }


    public void ExpandSiblings(GameObject button) {
        //sets all siblings of the chosen button to be active, and resizes the scroll view
        expandedButton = button;

        //Canvas.ForceUpdateCanvases();
        //AdjustButtonDimensions(button.transform.parent);

        GameObject parentBox = button.transform.parent.gameObject;
        bool switchTo = true;

        for (int i = 1; i < parentBox.transform.childCount; i++)
            parentBox.transform.GetChild(i).gameObject.SetActive(switchTo);
        ResizeToFitChildren(parentBox, false);

        //if bottom of new stuff is out of view of the scroll view, then move the scroll view up
        float originalPos = parentBox.transform.localPosition.y;
        float totalHeight = parentBox.transform.GetChild(0).GetComponent<RectTransform>().rect.height;
        totalHeight += parentBox.transform.GetChild(1).GetComponent<RectTransform>().rect.height;
        totalHeight += parentBox.transform.GetChild(2).GetComponent<RectTransform>().rect.height;
        float scrollDepth = parentBox.transform.parent.localPosition.y;
        float distOffscreen = originalPos - totalHeight + scrollDepth + 1830f;
        if (distOffscreen < 0)
            parentBox.transform.parent.localPosition += new Vector3(0,-distOffscreen,0);

        Canvas.ForceUpdateCanvases();
        AdjustButtonDimensions(button.transform.parent);
        Canvas.ForceUpdateCanvases();
        
        parentBox.transform.parent.parent.parent.GetComponent<ScrollRect>().StopMovement();
    }

    public void ContractSiblings(GameObject button) {
        //sets all siblings of the chosen button to be inactive, and resizes the scroll view
        if (expandedButton == button)
            expandedButton = null;
        GameObject parentBox = button.transform.parent.gameObject;
        bool switchTo = false;

        for (int i = 1; i < parentBox.transform.childCount; i++)
            parentBox.transform.GetChild(i).gameObject.SetActive(switchTo);
        ResizeToFitChildren(parentBox, false);
    }

    public void ContractPreviousExpansion() {
        ContractSiblings(expandedButton);
    }
    
    public void ResizeToFitChildren(GameObject parent, bool minSize) {
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

    public void DisplayCoinsOnButton(GameObject button, int cost) {
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
                if (value10s == 0)
                    imageObject10s.SetActive(false);
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

    private void AdjustButtonDimensions(Transform button){
        bool isSkinButton = button.Find("Dropdown").Find("Preview") != null;
        if (!isSkinButton){
            RectTransform dropdown = button.Find("Dropdown").GetComponent<RectTransform>();
            RectTransform dropdown1 = dropdown.Find("Dropdown Image").GetChild(0).GetComponent<RectTransform>();
            RectTransform dropdown2 = dropdown.Find("Dropdown Image").GetChild(1).GetComponent<RectTransform>();
            RectTransform description = dropdown.Find("Description").GetComponent<RectTransform>();
            RectTransform unlock = dropdown.Find("Unlock").GetComponent<RectTransform>();
            float spacing = 30;
            float descriptionHeight = description.sizeDelta.y;
            float unlockHeight = unlock.rect.height;

            float unlockPos = -((spacing * 2) + descriptionHeight);
            float dropdownHeight = (spacing * 3) + descriptionHeight + unlockHeight;
            unlock.anchoredPosition =new Vector3(unlock.anchoredPosition.x, unlockPos, 0);
            dropdown1.sizeDelta = new Vector2(dropdown1.rect.width, dropdownHeight);
            dropdown2.sizeDelta = new Vector2(dropdown2.rect.width, dropdownHeight);
            dropdown.sizeDelta = new Vector2(dropdown2.rect.width, dropdownHeight);
        }
    }

    // ---------------------------------------------------
    //ALL OF THE FUNCTIONS THAT PARSE THROUGH SKIN-SPECIFIC INFORMATION
    // ---------------------------------------------------

    private void FindSkinButtons() {
        //finds all of the buttons that let the player unlock various skins
        //specifically, finds all gameobjects between the skin start and end points from the inspector
        int start = skinsStart.transform.GetSiblingIndex();
        int end = skinsEnd.transform.GetSiblingIndex();
        List<GameObject> buttons = new List<GameObject>();
        GameObject parent = expandMedButton.transform.parent.parent.gameObject;
        for (int i = start + 1; i<end; i++)
            buttons.Add(parent.transform.GetChild(i).gameObject);
        skinButtons = buttons.ToArray();
    }

    private int GetSkinPrice(Skin skin) {
        //returns the price to unlock a skin in the shop
        int ownedSkins = StaticVariables.unlockedSkins.Count;
        int price = 0; //price if you only own one skin (rural)
        int i = ownedSkins - 1;
        while (i > 0){
            price += i * 10;
            i --;
        }
        return price;
    }

    private void UpdateAllSkinCosts(){
        foreach (GameObject parent in skinButtons)
            DisplayCoinsOnButton(parent.transform.Find("Expand Button").gameObject, GetSkinPrice((InterfaceFunctions.GetSkinFromName(parent.name))));
    }

    // ---------------------------------------------------
    //ALL OF THE FUNCTIONS THAT HELP PURCHASE AN UPGRADE. FUNCTIONS THAT MAKE THE PURCHASE FOR ONE SPECIFIC UPGRADE ARE LATER
    // ---------------------------------------------------

    private bool CanPurchase(bool notCond, int cost) {
        //if the prerequisite purchase condition is met, and the upgrade's cost is met, then returns true
        //used in the unlock functions
        return (!notCond && StaticVariables.coins >= cost);
    }

    private void DoPurchase(int price) {
        //purchases an upgrade and contracts the expanded drop-down under the purchased upgrade's title
        //used in the unlock functions
        MakePurchase(price);
        UpdateButtons();
        ContractPreviousExpansion();

        SaveSystem.SaveGame();
    }

    public void MakePurchase(int cost) {
        //deducts the cost of a purchase from the player's coin total, and displays the new amount
        StaticVariables.coins -= cost;
        DisplayCoinsAmount();
    }

    // ---------------------------------------------------
    //ALL OF THE FUNCTIONS THAT DIRECTLY UNLOCK AN UPGRADE. EACH ONE ALSO SETS THE UNLOCK AS ACTIVE, WHICH CAN BE DEACTIVATED FROM SETTINGS
    // ---------------------------------------------------

    public void PushUnlockMediumButton() {
        if (CanPurchase(StaticVariables.unlockedMedium,medCityPrice)) {
            StaticVariables.unlockedMedium = true;
            StaticVariables.highestUnlockedSize = 4;
            StaticVariables.showMed = true;
            DoPurchase(medCityPrice);
        }
    }
    public void PushUnlockLargeButton() {
        if (CanPurchase(StaticVariables.unlockedLarge, largeCityPrice) && StaticVariables.unlockedMedium) {
            StaticVariables.unlockedLarge = true;
            StaticVariables.highestUnlockedSize = 5;
            StaticVariables.showMed = true;
            StaticVariables.showLarge = true;
            DoPurchase(largeCityPrice);
        }
    }
    public void PushUnlockHugeButton() {
        if (CanPurchase(StaticVariables.unlockedHuge, hugeCityPrice) && StaticVariables.unlockedLarge && StaticVariables.unlockedMedium) {
            StaticVariables.unlockedHuge = true;
            StaticVariables.highestUnlockedSize = 6;
            StaticVariables.showMed = true;
            StaticVariables.showLarge = true;
            StaticVariables.showHuge = true;
            DoPurchase(hugeCityPrice);
        }
    }
    public void PushUnlockMassiveButton() {
        if (CanPurchase(StaticVariables.unlockedMassive, massiveCityPrice) && StaticVariables.unlockedHuge && StaticVariables.unlockedLarge && StaticVariables.unlockedMedium) {
            StaticVariables.unlockedMassive = true;
            StaticVariables.highestUnlockedSize = 7;
            StaticVariables.showMed = true;
            StaticVariables.showLarge = true;
            StaticVariables.showHuge = true;
            StaticVariables.showMassive = true;
            DoPurchase(massiveCityPrice);
        }
    }
    public void PushUnlockNotes1Button() {
        if (CanPurchase(StaticVariables.unlockedNotes1,notes1Price)) {
            StaticVariables.unlockedNotes1 = true;
            StaticVariables.includeNotes1Button = true;
            DoPurchase(notes1Price);
        }
    }
    public void PushUnlockNotes2Button() {
        if (CanPurchase(StaticVariables.unlockedNotes2, notes2Price) && StaticVariables.unlockedNotes1) {
            StaticVariables.unlockedNotes2 = true;
            StaticVariables.includeNotes2Button = true;
            DoPurchase(notes2Price);
        }
    }
    public void PushUnlockResidentColorButton() {
        if (CanPurchase(StaticVariables.unlockedResidentsChangeColor,residentColorPrice)) {
            StaticVariables.unlockedResidentsChangeColor = true;
            StaticVariables.changeResidentColorOnCorrectRows = true;
            DoPurchase(residentColorPrice);
        }
    }
    public void PushUnlockUndoRedoButton() {
        if (CanPurchase(StaticVariables.unlockedUndoRedo,undoRedoPrice)) {
            StaticVariables.unlockedUndoRedo = true;
            StaticVariables.includeUndoRedo = true;
            DoPurchase(undoRedoPrice);
        }
    }
    public void PushUnlockRemoveAllButton() {
        if (CanPurchase(StaticVariables.unlockedRemoveAllOfNumber, removeAllPrice) && StaticVariables.unlockedUndoRedo) {
            StaticVariables.unlockedRemoveAllOfNumber = true;
            StaticVariables.includeRemoveAllOfNumber = true;
            DoPurchase(removeAllPrice);
        }
    }
    public void PushUnlockClearPuzzleButton() {
        if (CanPurchase(StaticVariables.unlockedClearPuzzle, clearPrice) && StaticVariables.unlockedUndoRedo) {
            StaticVariables.unlockedClearPuzzle = true;
            StaticVariables.includeClearPuzzle = true;
            DoPurchase(clearPrice);
        }
    }

    public void PushUnlockHighlightBuildingsButton() {
        if (CanPurchase(StaticVariables.unlockedHighlightBuildings, highlightBuildingsPrice)) {
            StaticVariables.unlockedHighlightBuildings = true;
            StaticVariables.includeHighlightBuildings = true;
            DoPurchase(highlightBuildingsPrice);
        }
    }


    public void PushUnlockSkinButton(GameObject parent) {
        Skin skin = InterfaceFunctions.GetSkinFromName(parent.name);
        if (CanPurchase(StaticVariables.unlockedSkins.Contains(skin), GetSkinPrice(skin))){
            DoPurchase(GetSkinPrice(skin));
            StaticVariables.unlockedSkins.Add(skin);
            StaticVariables.skin = skin;
            ApplySkin();
            UpdateAllSkinCosts();
            UpdateButtons();
        }

    }
    public void PushUnlockBuildingQuantityStatusButton() {
        if (CanPurchase(StaticVariables.unlockedBuildingQuantityStatus, buildingQuantityStatusPrice)) {
            StaticVariables.unlockedBuildingQuantityStatus = true;
            StaticVariables.includeBuildingQuantityStatus = true;
            DoPurchase(buildingQuantityStatusPrice);
        }
    }

    // ---------------------------------------------------
    //ALL OF THE FUNCTIONS THAT WERE USED IN TESTING THE SHOP FUNCTIONALITY. THE BUTTONS THAT CALL THESE FUNCTIONS ARE HIDDEN IN THE INSPECTOR
    // ---------------------------------------------------

    public void PushClearPurchasesButton() {
        //the clear purchases button is only used during testing, for the purposes of testing shop mechanics 
        PushLockAllButton();
        if (StaticVariables.coins < 50) 
            StaticVariables.coins = 300;
        DisplayCoinsAmount();
    }

    public void PushLockAllButton() {
        //lock all is also only used during testing
        StaticVariables.unlockedMedium = false;
        StaticVariables.unlockedLarge = false;
        StaticVariables.unlockedHuge = false;
        StaticVariables.unlockedMassive = false;
        StaticVariables.highestUnlockedSize = 3;
        StaticVariables.showMed = false;
        StaticVariables.showLarge = false;
        StaticVariables.showHuge = false;
        StaticVariables.showMassive = false;

        StaticVariables.unlockedNotes1 = false;
        StaticVariables.unlockedNotes2 = false;
        StaticVariables.unlockedResidentsChangeColor = false;
        StaticVariables.unlockedUndoRedo = false;
        StaticVariables.unlockedRemoveAllOfNumber = false;
        StaticVariables.unlockedClearPuzzle = false;
        StaticVariables.unlockedHighlightBuildings = false;
        StaticVariables.unlockedBuildingQuantityStatus = false;

        StaticVariables.includeNotes1Button = false;
        StaticVariables.includeNotes2Button = false;
        StaticVariables.changeResidentColorOnCorrectRows = false;
        StaticVariables.includeUndoRedo = false;
        StaticVariables.includeRemoveAllOfNumber = false;
        StaticVariables.includeClearPuzzle = false;
        StaticVariables.includeHighlightBuildings = false;
        StaticVariables.includeBuildingQuantityStatus = false;

        StaticVariables.unlockedSkins = new List<Skin>();
        StaticVariables.unlockedSkins.Add(InterfaceFunctions.GetDefaultSkin());
        StaticVariables.skin = InterfaceFunctions.GetDefaultSkin();
        background.GetComponent<Image>().sprite = StaticVariables.skin.mainMenuBackground;
        InterfaceFunctions.ColorMenuButton(menuButton, StaticVariables.skin);
        InterfaceFunctions.ColorMenuButton(settingsButton, StaticVariables.skin);
        UpdateButtons();
    }

    public void PushUnlockAllButton() {
        //unlock all is also only used during testing
        StaticVariables.unlockedMedium = true;
        StaticVariables.unlockedLarge = true;
        StaticVariables.unlockedHuge = true;
        StaticVariables.unlockedMassive = true;
        StaticVariables.highestUnlockedSize = 7;
        StaticVariables.showMed = true;
        StaticVariables.showLarge = true;
        StaticVariables.showHuge = true;
        StaticVariables.showMassive = true;

        StaticVariables.unlockedNotes1 = true;
        StaticVariables.unlockedNotes2 = true;
        StaticVariables.unlockedResidentsChangeColor = true;
        StaticVariables.unlockedUndoRedo = true;
        StaticVariables.unlockedRemoveAllOfNumber = true;
        StaticVariables.unlockedClearPuzzle = true;
        StaticVariables.unlockedHighlightBuildings = true;
        StaticVariables.unlockedBuildingQuantityStatus = true;

        StaticVariables.includeNotes1Button = true;
        StaticVariables.includeNotes2Button = true;
        StaticVariables.changeResidentColorOnCorrectRows = true;
        StaticVariables.includeUndoRedo = true;
        StaticVariables.includeRemoveAllOfNumber = true;
        StaticVariables.includeClearPuzzle = true;
        StaticVariables.includeHighlightBuildings = true;
        StaticVariables.includeBuildingQuantityStatus = true;

        PushUnlockAllSkinsButton();
        UpdateButtons();
    }

    private void PushUnlockAllSkinsButton() {
        //unlockAllSkins is also only used during testing
        foreach (Skin skin in StaticVariables.allSkins) {
            if (skin != InterfaceFunctions.GetDefaultSkin() && !StaticVariables.unlockedSkins.Contains(skin))
                StaticVariables.unlockedSkins.Add(skin);

        }
    }

    public void PushAddCoinsButton() {
        //addCoins is also only used during testing
        StaticVariables.AddCoins(40000);
        DisplayCoinsAmount();
        UpdateButtons();
    }

    public void PushFreshInstallButton() {
        //also only used during testing
        PushRemoveCoinsButton();
        PushLockAllButton();
        StaticVariables.hasBeatenTutorial = false;
        StaticVariables.FadeOutThenLoadScene("MainMenu");
    }

    public void PushRemoveCoinsButton() {
        //also only used during testing
        StaticVariables.coins = 0;
        DisplayCoinsAmount();
    }
}