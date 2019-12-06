using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class ShopCanvasController : MonoBehaviour {

    public int medCityPrice = 10;
    public int largeCityPrice = 40;
    public int hugeCityPrice = 100;
    public int notes1Price = 10;
    public int notes2Price = 10;
    public int residentColorPrice = 10;
    public int undoRedoPrice = 10;
    public int removeAllPrice = 10;
    public int clearPrice = 10;


    public GameObject coinsBox1s;
    public GameObject coinsBox10s;
    public GameObject coinsBox100s;
    public GameObject coinsBox1000s;
    public GameObject coinsBox10000s;

    private SpriteRenderer sprite1s;
    private SpriteRenderer sprite10s;
    private SpriteRenderer sprite100s;
    private SpriteRenderer sprite1000s;
    private SpriteRenderer sprite10000s;

    public Sprite[] numbers = new Sprite[10];

    public GameObject expandMedButton;
    public GameObject expandLargeButton;
    public GameObject expandHugeButton;
    public GameObject expandNotes1Button;
    public GameObject expandNotes2Button;
    public GameObject expandResidentColorButton;
    public GameObject expandUndoRedoButton;
    public GameObject expandRemoveAllButton;
    public GameObject expandClearButton;


    public GameObject blackForeground; //used to transition to/from the puzzle menu
    private SpriteRenderer blackSprite;
    public float fadeOutTime;
    public float fadeInTime;
    private float fadeTimer;

    public GameObject scrollView;


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

    private GameObject expandedButton;

    public GameObject background;

    public GameObject menuButton;

    private void Start() {
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

        updateButtons();

        background.GetComponent<SpriteRenderer>().sprite = StaticVariables.skin.shopBackground;
        blackSprite = blackForeground.GetComponent<SpriteRenderer>();

        InterfaceFunctions.colorMenuButton(menuButton);

        if (StaticVariables.isFading && StaticVariables.fadingTo == "shop") {
            fadeTimer = fadeInTime;
        }

        setScrollViewHeight();

    }

    private void Update() {
        if (StaticVariables.isFading && StaticVariables.fadingFrom == "shop") {
            fadeTimer -= Time.deltaTime;
            Color c = blackSprite.color;
            c.a = (fadeOutTime - fadeTimer) / fadeOutTime;
            blackSprite.color = c;
            if (fadeTimer <= 0f) {
                if (StaticVariables.fadingTo == "menu") { SceneManager.LoadScene("MainMenu"); }
            }
        }
        if (StaticVariables.isFading && StaticVariables.fadingTo == "shop") {
            fadeTimer -= Time.deltaTime;
            Color c = blackSprite.color;
            c.a = (fadeTimer) / fadeInTime;
            blackSprite.color = c;
            if (fadeTimer <= 0f) {
                StaticVariables.isFading = false;
            }
        }
    }


    private void OnApplicationQuit() {
        SaveSystem.SaveGame();
    }

    public void goToMainMenu() {
        if (!StaticVariables.isFading) {
            StaticVariables.fadingTo = "menu";
            startFadeOut();
        }
    }

    public void startFadeOut() {
        fadeTimer = fadeOutTime;
        StaticVariables.isFading = true;
        StaticVariables.fadingFrom = "shop";
    }


    public void displayCoinsAmount() {
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

        Color black = Color.black;
        ColorUtility.TryParseHtmlString("#FFEB42", out black);
        ColorUtility.TryParseHtmlString("#3A3A3A", out black);

        Color grey = Color.grey;
        updateButton(expandMedButton, StaticVariables.unlockedMedium, medCityPrice);
        updateButton(expandLargeButton, StaticVariables.unlockedLarge, largeCityPrice, StaticVariables.unlockedMedium);
        updateButton(expandHugeButton, StaticVariables.unlockedHuge, hugeCityPrice, StaticVariables.unlockedLarge);
        updateButton(expandNotes1Button, StaticVariables.unlockedNotes1, notes1Price);
        updateButton(expandNotes2Button, StaticVariables.unlockedNotes2, notes2Price, StaticVariables.unlockedNotes1);
        updateButton(expandResidentColorButton, StaticVariables.unlockedResidentsChangeColor, residentColorPrice);
        updateButton(expandUndoRedoButton, StaticVariables.unlockedUndoRedo, undoRedoPrice);
        updateButton(expandRemoveAllButton, StaticVariables.unlockedRemoveAllOfNumber, removeAllPrice, StaticVariables.includeUndoRedo);
        updateButton(expandClearButton, StaticVariables.unlockedClearPuzzle, clearPrice, StaticVariables.includeUndoRedo);

        setScrollViewHeight();

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

    public void clearPurchases() {
        //some kind of popup to confirm that the player wants to clear their purchases? 
        //"are you sure you want to reset all of your purchases? you will NOT get your spent coins back!!!"
        //if so, then do the following...

        lockAll();

        if (StaticVariables.coins < 50) { StaticVariables.coins = 300; }

        displayCoinsAmount();
    }

    public void lockAll() {

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

        StaticVariables.includeNotes1Button = false;
        StaticVariables.includeNotes2Button = false;
        StaticVariables.changeResidentColorOnCorrectRows = false;
        StaticVariables.includeUndoRedo = false;
        StaticVariables.includeRemoveAllOfNumber = false;
        StaticVariables.includeClearPuzzle = false;
        updateButtons();
    }

    public void unlockAll() {

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

        StaticVariables.includeNotes1Button = true;
        StaticVariables.includeNotes2Button = true;
        StaticVariables.changeResidentColorOnCorrectRows = true;
        StaticVariables.includeUndoRedo = true;
        StaticVariables.includeRemoveAllOfNumber = true;
        StaticVariables.includeClearPuzzle = true;
        updateButtons();
    }

    public void addCoins() {
        StaticVariables.coins += 40;
        displayCoinsAmount();
        updateButtons();
    }

    public void removeCoins() {
        StaticVariables.coins = 0;
        displayCoinsAmount();
    }

    public void pushMedButton() { clickedButton(expandMedButton); }
    
    public void pushLargeButton() { clickedButton(expandLargeButton); }

    public void pushHugeButton() { clickedButton(expandHugeButton); }

    public void pushNotes1Button() { clickedButton(expandNotes1Button); }

    public void pushNotes2Button() { clickedButton(expandNotes2Button); }

    public void pushResidentColorButton() { clickedButton(expandResidentColorButton); }

    public void pushUndoRedoButton() { clickedButton(expandUndoRedoButton); }

    public void pushRemoveAllButton() { clickedButton(expandRemoveAllButton); }

    public void pushClearButton() { clickedButton(expandClearButton); }

    public void unlockMedium() {
        if (!StaticVariables.unlockedMedium && StaticVariables.coins >= medCityPrice) {

            StaticVariables.unlockedMedium = true;
            StaticVariables.highestUnlockedSize = 4;
            StaticVariables.showMed = true;

            makePurchase(medCityPrice);
            updateButtons();
            contractPreviousExpansion();
        }
    }

    public void unlockLarge() {
        if (!StaticVariables.unlockedLarge && StaticVariables.unlockedMedium && StaticVariables.coins >= largeCityPrice) {

            StaticVariables.unlockedLarge = true;
            StaticVariables.highestUnlockedSize = 5;
            StaticVariables.showMed = true;
            StaticVariables.showLarge = true;

            makePurchase(largeCityPrice);
            updateButtons();
            contractPreviousExpansion();
        }
    }

    public void unlockHuge() {
        if (!StaticVariables.unlockedHuge && StaticVariables.unlockedLarge && StaticVariables.unlockedMedium && StaticVariables.coins >= hugeCityPrice) {

            StaticVariables.unlockedHuge = true;
            StaticVariables.highestUnlockedSize = 6;
            StaticVariables.showMed = true;
            StaticVariables.showLarge = true;
            StaticVariables.showHuge = true;

            makePurchase(hugeCityPrice);
            updateButtons();
            contractPreviousExpansion();
        }
    }

    public void unlockNotes1() {
        if (!StaticVariables.unlockedNotes1 && StaticVariables.coins >= notes1Price) {

            StaticVariables.unlockedNotes1 = true;
            StaticVariables.includeNotes1Button = true;

            makePurchase(notes1Price);
            updateButtons();
            contractPreviousExpansion();
        }
    }
    public void unlockNotes2() {
        if (!StaticVariables.unlockedNotes2 && StaticVariables.unlockedNotes1 && StaticVariables.coins >= notes2Price) {

            StaticVariables.unlockedNotes2 = true;
            StaticVariables.includeNotes2Button = true;

            makePurchase(notes2Price);
            updateButtons();
            contractPreviousExpansion();
        }
    }
    public void unlockResidentsChangeColor() {
        if (!StaticVariables.unlockedResidentsChangeColor && StaticVariables.coins >= residentColorPrice) {

            StaticVariables.unlockedResidentsChangeColor = true;
            StaticVariables.changeResidentColorOnCorrectRows = true;

            makePurchase(residentColorPrice);
            updateButtons();
            contractPreviousExpansion();
        }
    }
    public void unlockUndoRedo() {
        if (!StaticVariables.unlockedUndoRedo && StaticVariables.coins >= undoRedoPrice) {

            StaticVariables.unlockedUndoRedo = true;
            StaticVariables.includeUndoRedo = true;

            makePurchase(undoRedoPrice);
            updateButtons();
            contractPreviousExpansion();
        }
    }
    public void unlockRemoveAllOfNumber() {
        if (!StaticVariables.unlockedRemoveAllOfNumber && StaticVariables.includeUndoRedo && StaticVariables.coins >= removeAllPrice) {
            StaticVariables.unlockedRemoveAllOfNumber = true;
            StaticVariables.includeRemoveAllOfNumber = true;

            makePurchase(removeAllPrice);
            updateButtons();
            contractPreviousExpansion();
        }
    }

    public void unlockClearPuzzle() {
        if (!StaticVariables.unlockedClearPuzzle && StaticVariables.includeUndoRedo && StaticVariables.coins >= clearPrice) {
            StaticVariables.unlockedClearPuzzle = true;
            StaticVariables.includeClearPuzzle = true;

            makePurchase(clearPrice);
            updateButtons();
            contractPreviousExpansion();
        }
    }



    public void expandSiblings(GameObject button) {
        //sets all siblings of the chosen button to be active, and resizes all necessary scroll views

        expandedButton = button;

        GameObject parentBox = button.transform.parent.gameObject;
        bool switchTo = true;
        
        for (int i = 1; i < parentBox.transform.childCount; i++) {
            parentBox.transform.GetChild(i).gameObject.SetActive(switchTo);
        }
        resizeToFitChildren(parentBox);
        setScrollViewHeight();
    }

    public void contractSiblings(GameObject button) {
        //sets all siblings of the chosen button to be inactive, and resizes all necessary scroll views
        if (expandedButton == button) {
            expandedButton = null;
        }
        GameObject parentBox = button.transform.parent.gameObject;
        bool switchTo = false;

        for (int i = 1; i < parentBox.transform.childCount; i++) {
            parentBox.transform.GetChild(i).gameObject.SetActive(switchTo);
        }
        resizeToFitChildren(parentBox);
        setScrollViewHeight();
    }

    public void contractPreviousExpansion() {
        contractSiblings(expandedButton);
    }

    public void clickedButton(GameObject button) {
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
    

    public void resizeToFitChildren(GameObject parent) {
        //get height of contents, including spaces between objects and top and bottom padding
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
        //if(newHeight < parent.transform.parent.GetComponent<RectTransform>().sizeDelta.y) { newHeight = parent.transform.parent.GetComponent<RectTransform>().sizeDelta.y; }

        Vector2 newSize = new Vector2(parent.GetComponent<RectTransform>().sizeDelta.x, newHeight);
        parent.GetComponent<RectTransform>().sizeDelta = newSize;

    }

    public void resizeToFitChildrenMinSize(GameObject parent) {
        //exactly the same as resizeToFitChildren, but also the container height has to be at least the height of its parent
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
        if (newHeight < parent.transform.parent.GetComponent<RectTransform>().sizeDelta.y) { newHeight = parent.transform.parent.GetComponent<RectTransform>().sizeDelta.y; }

        Vector2 newSize = new Vector2(parent.GetComponent<RectTransform>().sizeDelta.x, newHeight);
        parent.GetComponent<RectTransform>().sizeDelta = newSize;
    }

    public void setScrollViewHeight() {
        //sets the scroll viewer (vertical layout group) height to match its contents. minimum is the height of its parent scrollable container
        //to be called whenever an item is shown or hidden in the settings window

        //define the current top height - for use at the end of the function
        float topHeight = (float)Math.Round(((1 - scrollView.transform.parent.GetComponent<ScrollRect>().verticalNormalizedPosition) * (scrollView.GetComponent<RectTransform>().sizeDelta.y - scrollView.transform.parent.GetComponent<RectTransform>().sizeDelta.y)), 2);

        resizeToFitChildrenMinSize(scrollView);

        //set the scroll view to be at the same position as previously
        scrollView.transform.parent.GetComponent<ScrollRect>().verticalNormalizedPosition = 1 - (topHeight / (scrollView.GetComponent<RectTransform>().sizeDelta.y - scrollView.transform.parent.GetComponent<RectTransform>().sizeDelta.y));

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

    public void makePurchase(int cost) {
        StaticVariables.coins -= cost;
        displayCoinsAmount();
    }

}