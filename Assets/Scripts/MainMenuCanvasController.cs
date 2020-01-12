using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuCanvasController : MonoBehaviour {

    public GameObject onlySmallPuzzleButton;
    public GameObject smallAndMedPuzzleButtons;
    public GameObject smallMedLargePuzzleButtons;
    public GameObject smallMedLargeHugePuzzleButtons;
    public GameObject returnOrAbandonButtons;
    public GameObject background;

    public GameObject shopButton;
    public GameObject tutorialButton;
    public GameObject settingsButton;

    public GameObject blackForeground; //used to transition to/from the puzzle menu
    private SpriteRenderer blackSprite;
    public float fadeOutTime;
    public float fadeInTime;
    private float fadeTimer;


    public Skin defaultSkin;

    public Skin[] skins;


    private void Start() {

        if (StaticVariables.isApplicationLaunchingFirstTime) {
            StaticVariables.allSkins = skins;
            SaveSystem.LoadGame();
            StaticVariables.isApplicationLaunchingFirstTime = false;
        }
        //print(StaticVariables.skin.name);
        background.GetComponent<SpriteRenderer>().sprite = StaticVariables.skin.mainMenuBackground;
        //ColorUtility.TryParseHtmlString(StaticVariables.skin.mainMenuButtonExterior, out buttonColorExterior);
        //ColorUtility.TryParseHtmlString(StaticVariables.skin.mainMenuButtonInterior, out buttonColorInterior);
        colorButtons();
        applyCityArtSkin();

        blackSprite = blackForeground.GetComponent<SpriteRenderer>();

        if (StaticVariables.isFading && StaticVariables.fadingTo == "menu") {
            fadeTimer = fadeInTime;
        }

        showCityButtons();

    }

    private void Update() {
        //
        if (StaticVariables.isFading && StaticVariables.fadingFrom == "menu") {
            fadeTimer -= Time.deltaTime;
            Color c = blackSprite.color;
            c.a = (fadeOutTime - fadeTimer) / fadeOutTime;
            blackSprite.color = c;
            if (fadeTimer <= 0f) {
                if (StaticVariables.fadingTo == "puzzle") { SceneManager.LoadScene("InPuzzle"); }
                if (StaticVariables.fadingTo == "shop") { SceneManager.LoadScene("Shop"); }
                if (StaticVariables.fadingTo == "settings") { SceneManager.LoadScene("Settings"); }
                if (StaticVariables.fadingTo == "reload") {
                    StaticVariables.fadingTo = "menu";
                    StaticVariables.fadingFrom = "reloaded";
                    SceneManager.LoadScene("MainMenu");
                }
            }
        }
        else if (StaticVariables.isFading && StaticVariables.fadingTo == "menu") {
            fadeTimer -= Time.deltaTime;
            Color c = blackSprite.color;
            c.a = (fadeTimer) / fadeInTime;
            blackSprite.color = c;
            if (fadeTimer <= 0f) {
                StaticVariables.isFading = false;
                if (StaticVariables.waitingOnButtonClickAfterFadeIn) {
                    StaticVariables.waitingOnButtonClickAfterFadeIn = false;
                    if (StaticVariables.buttonClickInWaiting.Contains("puzzle")) {
                        int s = int.Parse(StaticVariables.buttonClickInWaiting[StaticVariables.buttonClickInWaiting.Length -1] + "");
                        //print("going to enter puzzle with size of " + s);
                        startPuzzle(s);
                    }
                    else if (StaticVariables.buttonClickInWaiting == "tutorial") {
                        startTutorial();
                    }
                    else if (StaticVariables.buttonClickInWaiting == "shop") {
                        goToShop();
                    }
                    else if (StaticVariables.buttonClickInWaiting == "settings") {
                        goToSettings();
                    }
                    else if (StaticVariables.buttonClickInWaiting == "abandon") {
                        pushAbandonPuzzleButton();
                    }
                }
            }
        }
    }


    private void OnApplicationQuit() {
        //PerformOverflow();
        SaveSystem.SaveGame();
    }

    private void PerformOverflow() {
        PerformOverflow();
    }

    public void startFadeOut() {
        fadeTimer = fadeOutTime;
        StaticVariables.isFading = true;
        StaticVariables.fadingFrom = "menu";
    }

    public void startPuzzle(int size) {
        if (!StaticVariables.isFading) {

            StaticVariables.size = size;
            StaticVariables.isTutorial = false;

            StaticVariables.fadingTo = "puzzle";
            startFadeOut();
        }
        else {
            StaticVariables.waitingOnButtonClickAfterFadeIn = true;
            StaticVariables.buttonClickInWaiting = "puzzle" + size;
        }
    }
    
    public void startTutorial() {
        if (!StaticVariables.isFading) {
            StaticVariables.size = 3;
            StaticVariables.isTutorial = true;
            StaticVariables.fadingTo = "puzzle";
            startFadeOut();
        }
        else {
            StaticVariables.waitingOnButtonClickAfterFadeIn = true;
            StaticVariables.buttonClickInWaiting = "tutorial";
        }
    }

    public void goToShop() {
        if (!StaticVariables.isFading) {
            StaticVariables.fadingTo = "shop";
            startFadeOut();
        }
        else {
            StaticVariables.waitingOnButtonClickAfterFadeIn = true;
            StaticVariables.buttonClickInWaiting = "shop";
        }
    }

    public void goToSettings() {
        if (!StaticVariables.isFading) {
            StaticVariables.fadingTo = "settings";
            startFadeOut();
        }
        else {
            StaticVariables.waitingOnButtonClickAfterFadeIn = true;
            StaticVariables.buttonClickInWaiting = "settings";
        }
    }
    /*
    private void updateButtonColors(GameObject button) {
        button.transform.Find("Button Image").Find("Borders").GetComponent<Image>().color = buttonColorExterior;
        button.transform.Find("Button Image").Find("Interior").GetComponent<Image>().color = buttonColorInterior;
    }
    */

    private void colorButtons() {
        InterfaceFunctions.colorMenuButton(onlySmallPuzzleButton.transform.Find("3").gameObject);
        InterfaceFunctions.colorMenuButton(smallAndMedPuzzleButtons.transform.Find("3").gameObject);
        InterfaceFunctions.colorMenuButton(smallAndMedPuzzleButtons.transform.Find("4").gameObject);
        InterfaceFunctions.colorMenuButton(smallMedLargePuzzleButtons.transform.Find("3").gameObject);
        InterfaceFunctions.colorMenuButton(smallMedLargePuzzleButtons.transform.Find("4").gameObject);
        InterfaceFunctions.colorMenuButton(smallMedLargePuzzleButtons.transform.Find("5").gameObject);
        InterfaceFunctions.colorMenuButton(smallMedLargeHugePuzzleButtons.transform.Find("3").gameObject);
        InterfaceFunctions.colorMenuButton(smallMedLargeHugePuzzleButtons.transform.Find("4").gameObject);
        InterfaceFunctions.colorMenuButton(smallMedLargeHugePuzzleButtons.transform.Find("5").gameObject);
        InterfaceFunctions.colorMenuButton(smallMedLargeHugePuzzleButtons.transform.Find("6").gameObject);
        InterfaceFunctions.colorMenuButton(returnOrAbandonButtons.transform.Find("Popup").Find("Return").gameObject);
        InterfaceFunctions.colorMenuButton(returnOrAbandonButtons.transform.Find("Popup").Find("Abandon").gameObject);

        Color exter;
        Color inter;
        ColorUtility.TryParseHtmlString(StaticVariables.skin.resumePuzzleExterior, out exter);
        ColorUtility.TryParseHtmlString(StaticVariables.skin.resumePuzzleInterior, out inter);
        returnOrAbandonButtons.transform.Find("Popup").Find("Backdrop").Find("Border").GetComponent<SpriteRenderer>().color = exter;
        returnOrAbandonButtons.transform.Find("Popup").Find("Backdrop").Find("Interior").GetComponent<SpriteRenderer>().color = inter;

        InterfaceFunctions.colorMenuButton(shopButton);
        InterfaceFunctions.colorMenuButton(tutorialButton);
        InterfaceFunctions.colorMenuButton(settingsButton);
    }

    private void applyCityArtSkin() {
        onlySmallPuzzleButton.transform.Find("3").Find("City Art").GetComponent<Image>().sprite = StaticVariables.skin.smallCityArt;
        smallAndMedPuzzleButtons.transform.Find("3").Find("City Art").GetComponent<Image>().sprite = StaticVariables.skin.smallCityArt;
        smallAndMedPuzzleButtons.transform.Find("4").Find("City Art").GetComponent<Image>().sprite = StaticVariables.skin.medCityArt;
        smallMedLargePuzzleButtons.transform.Find("3").Find("City Art").GetComponent<Image>().sprite = StaticVariables.skin.smallCityArt;
        smallMedLargePuzzleButtons.transform.Find("4").Find("City Art").GetComponent<Image>().sprite = StaticVariables.skin.medCityArt;
        smallMedLargePuzzleButtons.transform.Find("5").Find("City Art").GetComponent<Image>().sprite = StaticVariables.skin.largeCityArt;
        smallMedLargeHugePuzzleButtons.transform.Find("3").Find("City Art").GetComponent<Image>().sprite = StaticVariables.skin.smallCityArt;
        smallMedLargeHugePuzzleButtons.transform.Find("4").Find("City Art").GetComponent<Image>().sprite = StaticVariables.skin.medCityArt;
        smallMedLargeHugePuzzleButtons.transform.Find("5").Find("City Art").GetComponent<Image>().sprite = StaticVariables.skin.largeCityArt;
        smallMedLargeHugePuzzleButtons.transform.Find("6").Find("City Art").GetComponent<Image>().sprite = StaticVariables.skin.hugeCityArt;

        if (StaticVariables.hasSavedPuzzleState) {
            string[] arr = new string[2];
            arr[0] = "Return";
            arr[1] = "Abandon";
            foreach (string s in arr) {

                returnOrAbandonButtons.transform.Find("Popup").Find(s).Find("City Art - Small").gameObject.SetActive(false);
                returnOrAbandonButtons.transform.Find("Popup").Find(s).Find("City Art - Small").GetComponent<Image>().sprite = StaticVariables.skin.smallCityArt;
                returnOrAbandonButtons.transform.Find("Popup").Find(s).Find("City Art - Med").gameObject.SetActive(false);
                returnOrAbandonButtons.transform.Find("Popup").Find(s).Find("City Art - Med").GetComponent<Image>().sprite = StaticVariables.skin.medCityArt;
                returnOrAbandonButtons.transform.Find("Popup").Find(s).Find("City Art - Large").gameObject.SetActive(false);
                returnOrAbandonButtons.transform.Find("Popup").Find(s).Find("City Art - Large").GetComponent<Image>().sprite = StaticVariables.skin.largeCityArt;
                returnOrAbandonButtons.transform.Find("Popup").Find(s).Find("City Art - Huge").gameObject.SetActive(false);
                returnOrAbandonButtons.transform.Find("Popup").Find(s).Find("City Art - Huge").GetComponent<Image>().sprite = StaticVariables.skin.hugeCityArt;
                if (StaticVariables.savedPuzzleSize == 3) {
                    returnOrAbandonButtons.transform.Find("Popup").Find(s).Find("City Art - Small").gameObject.SetActive(true);
                }
                if (StaticVariables.savedPuzzleSize == 4) {
                    returnOrAbandonButtons.transform.Find("Popup").Find(s).Find("City Art - Med").gameObject.SetActive(true);
                }
                if (StaticVariables.savedPuzzleSize == 5) {
                    returnOrAbandonButtons.transform.Find("Popup").Find(s).Find("City Art - Large").gameObject.SetActive(true);
                }
                if (StaticVariables.savedPuzzleSize == 6) {
                    returnOrAbandonButtons.transform.Find("Popup").Find(s).Find("City Art - Huge").gameObject.SetActive(true);
                }
            }
        }

    }


    public void pushReturnToPuzzleButton() {
        startPuzzle(StaticVariables.savedPuzzleSize);
    }

    public void pushAbandonPuzzleButton() {
        StaticVariables.hasSavedPuzzleState = false;
        SaveSystem.SaveGame();
        
        if (!StaticVariables.isFading) {
            

            StaticVariables.fadingTo = "reload";
            startFadeOut();
        }
        else {
            StaticVariables.waitingOnButtonClickAfterFadeIn = true;
            StaticVariables.buttonClickInWaiting = "abandon";
        }

        //showCityButtons();
    }

    private void showCityButtons() {

        returnOrAbandonButtons.SetActive(false);
        shopButton.SetActive(true);
        tutorialButton.SetActive(true);
        settingsButton.SetActive(true);

        int highestUnlockedSize = 3;
        if (StaticVariables.showMed) {
            highestUnlockedSize = 4;
        }
        if (StaticVariables.showLarge) {
            highestUnlockedSize = 5;
        }
        if (StaticVariables.showHuge) {
            highestUnlockedSize = 6;
        }
        if (StaticVariables.hasSavedPuzzleState) {
            onlySmallPuzzleButton.SetActive(false);
            smallAndMedPuzzleButtons.SetActive(false);
            smallMedLargePuzzleButtons.SetActive(false);
            smallMedLargeHugePuzzleButtons.SetActive(false);
            returnOrAbandonButtons.SetActive(true);
            shopButton.SetActive(false);
            tutorialButton.SetActive(false);
            settingsButton.SetActive(false);
            //print("we have a saved puzzle state!");
        }
        else {
            //print("no saved puzzle state... :(");

            onlySmallPuzzleButton.SetActive(highestUnlockedSize == 3);
            smallAndMedPuzzleButtons.SetActive(highestUnlockedSize == 4);
            smallMedLargePuzzleButtons.SetActive(highestUnlockedSize == 5);
            smallMedLargeHugePuzzleButtons.SetActive(highestUnlockedSize == 6);
        }
        /*
        if (StaticVariables.hasSavedPuzzleState) {
        }
        */
        //returnOrAbandonButtons.SetActive(StaticVariables.hasSavedPuzzleState);
    }
}