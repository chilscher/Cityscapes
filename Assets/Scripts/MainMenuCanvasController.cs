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

    public GameObject blackForeground; //used to transition to/from the main menu
    private SpriteRenderer blackSprite; //derived from the blackForeground gameObject
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
        background.GetComponent<SpriteRenderer>().sprite = StaticVariables.skin.mainMenuBackground;
        colorButtons();
        applyCityArtSkin();

        //start the fade-in process from another scene
        blackSprite = blackForeground.GetComponent<SpriteRenderer>();
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
        //update the "city art" on the main menu to match the current skin
        //also updates the "return to puzzle/abandon puzzle" buttons to match the same city art
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

            //temp while testing
            //largeCenterTutorialButton.SetActive(false);
            //puzzleButtons.SetActive(true);
            //shopButton.SetActive(true);
            //tutorialButton.SetActive(true);
            //settingsButton.SetActive(true);
        }
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