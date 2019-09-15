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

    public GameObject blackForeground; //used to transition to/from the puzzle menu
    private SpriteRenderer blackSprite;
    public float fadeOutTime;
    public float fadeInTime;
    private float fadeTimer;

    private void Start() {
        if (StaticVariables.isApplicationLaunchingFirstTime) {
            SaveSystem.LoadGame();
            StaticVariables.isApplicationLaunchingFirstTime = false;
        }

        blackSprite = blackForeground.GetComponent<SpriteRenderer>();

        if (StaticVariables.isFading && StaticVariables.fadingTo == "menu") {
            fadeTimer = fadeInTime;
        }

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
        onlySmallPuzzleButton.SetActive(highestUnlockedSize == 3);
        smallAndMedPuzzleButtons.SetActive(highestUnlockedSize == 4);
        smallMedLargePuzzleButtons.SetActive(highestUnlockedSize == 5);
        smallMedLargeHugePuzzleButtons.SetActive(highestUnlockedSize == 6);

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
            }
        }
        if (StaticVariables.isFading && StaticVariables.fadingTo == "menu") {
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

    public void startFadeOut() {
        fadeTimer = fadeOutTime;
        StaticVariables.isFading = true;
        StaticVariables.fadingFrom = "menu";
    }

    public void startPuzzle(int size) {
        StaticVariables.size = size;
        StaticVariables.isTutorial = false;

        StaticVariables.fadingTo = "puzzle";
        startFadeOut();
        //SceneManager.LoadScene("InPuzzle");
    }

    public void startTutorial() {
        StaticVariables.size = 3;
        StaticVariables.isTutorial = true;
        StaticVariables.fadingTo = "puzzle";
        startFadeOut();
        //SceneManager.LoadScene("InPuzzle");
    }

    public void goToShop() {
        StaticVariables.fadingTo = "shop";
        startFadeOut();
    }

    public void goToSettings() {
        StaticVariables.fadingTo = "settings";
        startFadeOut();
        //SceneManager.LoadScene("Settings");
    }
}