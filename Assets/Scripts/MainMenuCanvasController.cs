using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuCanvasController : MonoBehaviour {
    public GameObject puzzleButtons;
    public GameObject smallShadow;
    public GameObject mediumShadow;
    public GameObject largeShadow;
    public GameObject hugeShadow;

    public GameObject smallText;
    public GameObject mediumText;
    public GameObject largeText;
    public GameObject hugeText;

    public GameObject returnOrAbandonButtons;
    public GameObject background;

    public GameObject shopButton;
    public GameObject tutorialButton;
    public GameObject largeCenterTutorialButton;
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
            //StaticVariables.coins = 10000;
        }
        background.GetComponent<SpriteRenderer>().sprite = StaticVariables.skin.mainMenuBackground;
        colorButtons();
        applyCityArtSkin();

        blackSprite = blackForeground.GetComponent<SpriteRenderer>();

        if (StaticVariables.isFading && StaticVariables.fadingTo == "menu") {
            fadeTimer = fadeInTime;
        }

        showCityButtons();
        

    }

    private void Update() {
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
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
    }


    private void OnApplicationQuit() {
        SaveSystem.SaveGame();
    }

    public void startFadeOut() {
        fadeTimer = fadeOutTime;
        StaticVariables.isFading = true;
        StaticVariables.fadingFrom = "menu";
    }

    public void startPuzzle(int size) {
        if (getHighestUnlockedSize() >= size) {

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

    private void colorButtons() {
        InterfaceFunctions.colorMenuButton(puzzleButtons.transform.Find("3").gameObject);
        InterfaceFunctions.colorMenuButton(puzzleButtons.transform.Find("4").gameObject);
        InterfaceFunctions.colorMenuButton(puzzleButtons.transform.Find("5").gameObject);
        InterfaceFunctions.colorMenuButton(puzzleButtons.transform.Find("6").gameObject);

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
        InterfaceFunctions.colorMenuButton(largeCenterTutorialButton);
    }

    private void applyCityArtSkin() {

        puzzleButtons.transform.Find("3").Find("City Art").GetComponent<Image>().sprite = StaticVariables.skin.smallCityArt;
        puzzleButtons.transform.Find("4").Find("City Art").GetComponent<Image>().sprite = StaticVariables.skin.medCityArt;
        puzzleButtons.transform.Find("5").Find("City Art").GetComponent<Image>().sprite = StaticVariables.skin.largeCityArt;
        puzzleButtons.transform.Find("6").Find("City Art").GetComponent<Image>().sprite = StaticVariables.skin.hugeCityArt;

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

        if (!StaticVariables.isFading) {

            StaticVariables.size = StaticVariables.savedPuzzleSize;
            StaticVariables.isTutorial = false;

            StaticVariables.fadingTo = "puzzle";
            startFadeOut();
        }
        else {
            StaticVariables.waitingOnButtonClickAfterFadeIn = true;
            StaticVariables.buttonClickInWaiting = "puzzle" + StaticVariables.savedPuzzleSize;
        }
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
    }

    private void showCityButtons() {

        largeCenterTutorialButton.SetActive(false);
        returnOrAbandonButtons.SetActive(false);
        shopButton.SetActive(true);
        tutorialButton.SetActive(true);
        settingsButton.SetActive(true);
        smallShadow.SetActive(false);
        mediumShadow.SetActive(false);
        largeShadow.SetActive(false);
        hugeShadow.SetActive(false);
        smallText.SetActive(true);
        mediumText.SetActive(true);
        largeText.SetActive(true);
        hugeText.SetActive(true);

        int highestUnlockedSize = getHighestUnlockedSize();
        if (StaticVariables.hasSavedPuzzleState) {
            puzzleButtons.SetActive(false);
            returnOrAbandonButtons.SetActive(true);
        }
        else {
            puzzleButtons.SetActive(true);
            smallShadow.SetActive(!(highestUnlockedSize >= 3));
            mediumShadow.SetActive(!(highestUnlockedSize >= 4));
            largeShadow.SetActive(!(highestUnlockedSize >= 5));
            hugeShadow.SetActive(!(highestUnlockedSize >= 6));

            smallText.SetActive(highestUnlockedSize >= 3);
            mediumText.SetActive(highestUnlockedSize >= 4);
            largeText.SetActive(highestUnlockedSize >= 5);
            hugeText.SetActive(highestUnlockedSize >= 6);
        }

        if (!StaticVariables.hasBeatenTutorial) {
            returnOrAbandonButtons.SetActive(false);
            shopButton.SetActive(false);
            tutorialButton.SetActive(false);
            settingsButton.SetActive(false);

            puzzleButtons.SetActive(false);
            largeCenterTutorialButton.SetActive(true);
        }
    }

    public int getHighestUnlockedSize() {
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
        return highestUnlockedSize;
    }
}