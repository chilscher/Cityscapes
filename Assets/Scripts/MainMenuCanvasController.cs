//for Cityscapes, copyright Cole Hilscher 2020

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuCanvasController : MonoBehaviour {
    //controls the main menu canvas. Only one is used, and only on the main menu scene.

    //buttons, and children of buttons, that the player interacts with from the main menu
    public GameObject puzzleButtons;
    public GameObject smallShadow;
    public GameObject mediumShadow;
    public GameObject largeShadow;
    public GameObject hugeShadow;
    public GameObject massiveShadow;
    public GameObject smallCityArt;
    public GameObject mediumCityArt;
    public GameObject largeCityArt;
    public GameObject hugeCityArt;
    public GameObject massiveCityArt;
    public GameObject smallText;
    public GameObject mediumText;
    public GameObject largeText;
    public GameObject hugeText;
    public GameObject massiveText;
    public GameObject returnOrAbandonButtons;
    public GameObject background;
    public GameObject shopButton;
    public GameObject tutorialButton;
    public GameObject largeCenterTutorialButton;
    public GameObject settingsButton;
    public GameObject leftSmallText;
    public GameObject leftMediumText;
    public GameObject leftLargeText;
    public GameObject leftHugeText;
    public GameObject leftMassiveText;
    public GameObject returnSmall;
    public GameObject returnMedium;
    public GameObject returnLarge;
    public GameObject returnHuge;
    public GameObject returnMassive;
    public GameObject abandonSmall;
    public GameObject abandonMedium;
    public GameObject abandonLarge;
    public GameObject abandonHuge;
    public GameObject abandonMassive;

    public GameObject blackForeground; //used to transition to/from the main menu
    private Image blackSprite; //derived from the blackForeground gameObject
    public float fadeOutTime; //total time for fade-out, from complete light to complete darkness
    public float fadeInTime; //total time for fade-in, from complete darkness to complete light
    private float fadeTimer; //the timer on which the fadeout and fadein mechanics operate

    public Skin defaultSkin; //the default skin used when the player boots up the game for the first time

    public Skin[] skins; //a list of all skins. To work effectively, any new skin must be added to this list in the inspector


    private void Start() {
        //check to see if the game is being opened, or if the game is transitioning from another menu
        if (StaticVariables.isApplicationLaunchingFirstTime) {
            StaticVariables.allSkins = skins;
            SaveSystem.LoadGame();
            StaticVariables.isApplicationLaunchingFirstTime = false;
        }
        //apply the current skin
        background.GetComponent<Image>().sprite = StaticVariables.skin.mainMenuBackground;
        colorButtons();
        showReturnAbandon();

        //start the fade-in process from another scene
        blackSprite = blackForeground.GetComponent<Image>();
        if (StaticVariables.isFading && StaticVariables.fadingTo == "menu") {
            fadeTimer = fadeInTime;
        }

        //update the puzzle buttons based on what puzzle sizes the player has unlocked
        showCityButtons();
       
    }

    private void Update() {
        //if the player is fading from main menu to another scene, this block handles that fading process
        if (StaticVariables.isFading && StaticVariables.fadingFrom == "menu") {
            fadeTimer -= Time.deltaTime;
            Color c = blackSprite.color;
            c.a = (fadeOutTime - fadeTimer) / fadeOutTime;
            blackSprite.color = c;
            if (fadeTimer <= 0f) { //after the fade timer is done, and the screen is dark, transition to another scene.
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
        //if the player is fading into the main menu scene, this block handles that fading process
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
        //if the player presses their phone's back button, call the Quit function
        //identical to if the player closed the game
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
    }
    
    // ---------------------------------------------------
    //ALL OF THE FUNCTIONS THAT ARE USED TO UPDATE THE VISUALS FOR THE MAIN MENU SCENE
    // ---------------------------------------------------

    private void colorButtons() {
        //color all of the menu buttons to fit the current skin
        InterfaceFunctions.colorMenuButton(puzzleButtons.transform.Find("3").gameObject);
        InterfaceFunctions.colorMenuButton(puzzleButtons.transform.Find("4").gameObject);
        InterfaceFunctions.colorMenuButton(puzzleButtons.transform.Find("5").gameObject);
        InterfaceFunctions.colorMenuButton(puzzleButtons.transform.Find("6").gameObject);
        InterfaceFunctions.colorMenuButton(puzzleButtons.transform.Find("7").gameObject);

        InterfaceFunctions.colorMenuButton(returnOrAbandonButtons.transform.Find("Popup").Find("Return").gameObject);
        InterfaceFunctions.colorMenuButton(returnOrAbandonButtons.transform.Find("Popup").Find("Abandon").gameObject);

        Color exter;
        Color inter;
        ColorUtility.TryParseHtmlString(StaticVariables.skin.resumePuzzleExterior, out exter);
        ColorUtility.TryParseHtmlString(StaticVariables.skin.resumePuzzleInterior, out inter);
        returnOrAbandonButtons.transform.Find("Popup").Find("Backdrop").Find("Border").GetComponent<Image>().color = exter;
        returnOrAbandonButtons.transform.Find("Popup").Find("Backdrop").Find("Interior").GetComponent<Image>().color = inter;

        InterfaceFunctions.colorMenuButton(shopButton);
        InterfaceFunctions.colorMenuButton(tutorialButton);
        InterfaceFunctions.colorMenuButton(settingsButton);
        InterfaceFunctions.colorMenuButton(largeCenterTutorialButton);
    }

    private void showReturnAbandon() {
        if (StaticVariables.hasSavedPuzzleState) {
            leftSmallText.SetActive(StaticVariables.savedPuzzleSize == 3);
            returnSmall.SetActive(StaticVariables.savedPuzzleSize == 3);
            abandonSmall.SetActive(StaticVariables.savedPuzzleSize == 3);
            returnMedium.SetActive(StaticVariables.savedPuzzleSize == 4);
            abandonMedium.SetActive(StaticVariables.savedPuzzleSize == 4);
            leftMediumText.SetActive(StaticVariables.savedPuzzleSize == 4);
            returnLarge.SetActive(StaticVariables.savedPuzzleSize == 5);
            abandonLarge.SetActive(StaticVariables.savedPuzzleSize == 5);
            leftLargeText.SetActive(StaticVariables.savedPuzzleSize == 5);
            returnHuge.SetActive(StaticVariables.savedPuzzleSize == 6);
            abandonHuge.SetActive(StaticVariables.savedPuzzleSize == 6);
            leftHugeText.SetActive(StaticVariables.savedPuzzleSize == 6);
            returnMassive.SetActive(StaticVariables.savedPuzzleSize == 7);
            abandonMassive.SetActive(StaticVariables.savedPuzzleSize == 7);
            leftMassiveText.SetActive(StaticVariables.savedPuzzleSize == 7);
        }
    }

    private void showCityButtons() {
        //shows or hides buttons on the menu depending on if the player has completed the tutorial, has a puzzle currently in progress, or has not unlocked all of the puzzle size upgrades yet
        largeCenterTutorialButton.SetActive(false);
        returnOrAbandonButtons.SetActive(false);
        shopButton.SetActive(true);
        tutorialButton.SetActive(true);
        settingsButton.SetActive(true);
        smallShadow.SetActive(false);
        mediumShadow.SetActive(false);
        largeShadow.SetActive(false);
        hugeShadow.SetActive(false);
        massiveShadow.SetActive(false);
        smallText.SetActive(true);
        mediumText.SetActive(true);
        largeText.SetActive(true);
        hugeText.SetActive(true);
        massiveText.SetActive(true);

        int highestUnlockedSize = getHighestUnlockedSize();
        if (StaticVariables.hasSavedPuzzleState) {
            puzzleButtons.SetActive(false);
            returnOrAbandonButtons.SetActive(true);
        }
        else {
            puzzleButtons.SetActive(true);
            smallShadow.SetActive(!(highestUnlockedSize >= 3));
            smallCityArt.SetActive(highestUnlockedSize >= 3);
            mediumShadow.SetActive(!(highestUnlockedSize >= 4));
            mediumCityArt.SetActive(highestUnlockedSize >= 4);
            largeShadow.SetActive(!(highestUnlockedSize >= 5));
            largeCityArt.SetActive(highestUnlockedSize >= 5);
            hugeShadow.SetActive(!(highestUnlockedSize >= 6));
            hugeCityArt.SetActive(highestUnlockedSize >= 6);
            massiveShadow.SetActive(!(highestUnlockedSize >= 7));
            massiveCityArt.SetActive(highestUnlockedSize >= 7);

            smallText.SetActive(highestUnlockedSize >= 3);
            mediumText.SetActive(highestUnlockedSize >= 4);
            largeText.SetActive(highestUnlockedSize >= 5);
            hugeText.SetActive(highestUnlockedSize >= 6);
            massiveText.SetActive(highestUnlockedSize >= 7);
        }

        /*
        if (!StaticVariables.hasBeatenTutorial) {
            returnOrAbandonButtons.SetActive(false);
            shopButton.SetActive(false);
            tutorialButton.SetActive(false);
            settingsButton.SetActive(false);

            puzzleButtons.SetActive(false);
            largeCenterTutorialButton.SetActive(true);

            //temp while testing
            
            largeCenterTutorialButton.SetActive(false);
            puzzleButtons.SetActive(true);
            shopButton.SetActive(true);
            tutorialButton.SetActive(true);
            settingsButton.SetActive(true);
            
        }
        */
    }

    public int getHighestUnlockedSize() {
        //gets the highest size puzzle that the player has unlocked as an int
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
        if (StaticVariables.showMassive) {
            highestUnlockedSize = 7;
        }
        return highestUnlockedSize;
    }

    // ---------------------------------------------------
    //ALL OF THE FUNCTIONS THAT ARE USED TO LEAVE THE MAIN MENU SCENE
    // ---------------------------------------------------
    
    private void OnApplicationQuit() {
        SaveSystem.SaveGame();
    }

    public void startFadeOut() {
        //begin the fade-out process. This function is called by several functions that transition to another scene
        //the fade-out mechanics of darkening the screen are implemented in the Update function
        fadeTimer = fadeOutTime;
        StaticVariables.isFading = true;
        StaticVariables.fadingFrom = "menu";
    }

    public void startPuzzle(int size) {
        //start fading out, and after the fade-out process is completed, go to the puzzle scene
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
        //start fading out, and after the fade-out process is completed, go to the puzzle scene, in tutorial mode
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
        //start fading out, and after the fade-out process is completed, go to the shop scene
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
        //start fading out, and after the fade-out process is completed, go to the settings scene
        if (!StaticVariables.isFading) {
            StaticVariables.fadingTo = "settings";
            startFadeOut();
        }
        else {
            StaticVariables.waitingOnButtonClickAfterFadeIn = true;
            StaticVariables.buttonClickInWaiting = "settings";
        }
    }


    public void pushReturnToPuzzleButton() {
        //start fading out, and after the fade-out process is completed, go to the puzzle scene and load a previous puzzle
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
        //start fading out, and after the fade-out process is completed, reload the main menu scene, but without the "return/abandon puzzle" options
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
}