using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsCanvasController : MonoBehaviour {
    
    public GameObject redNoteButton;
    public GameObject greenNoteButton;
    public GameObject changeCorrectResidentColorButton;

    public GameObject showMedButton;
    public GameObject showLargeButton;
    public GameObject showHugeButton;

    private void Start() {
        updateButtons();
    }
    

    private void OnApplicationQuit() {
        SaveSystem.SaveGame();
    }

    public void goToMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }
    
    

    public void clickedRedNoteButton() {
        StaticVariables.includeRedNoteButton = !StaticVariables.includeRedNoteButton;
        updateButtons();
    }
    public void clickedGreenNoteButton() {
        StaticVariables.includeGreenNoteButton = !StaticVariables.includeGreenNoteButton;
        updateButtons();
    }
    public void clickedCorrectResidentButton() {
        StaticVariables.changeResidentColorOnCorrectRows = !StaticVariables.changeResidentColorOnCorrectRows;
        updateButtons();
    }

    private void updateButtons() {
        redNoteButton.transform.GetChild(0).gameObject.SetActive(StaticVariables.includeRedNoteButton);
        greenNoteButton.transform.GetChild(0).gameObject.SetActive(StaticVariables.includeGreenNoteButton);
        changeCorrectResidentColorButton.transform.GetChild(0).gameObject.SetActive(StaticVariables.changeResidentColorOnCorrectRows);


        showMedButton.transform.GetChild(0).gameObject.SetActive(StaticVariables.showMed);
        showLargeButton.transform.GetChild(0).gameObject.SetActive(StaticVariables.showLarge);
        showHugeButton.transform.GetChild(0).gameObject.SetActive(StaticVariables.showHuge);

    }

    public void clicked4Button() {
        if (!StaticVariables.showLarge && !StaticVariables.showHuge) {
            StaticVariables.showMed = !StaticVariables.showMed;
        }
        updateButtons();
    }
    public void clicked5Button() {
        if (!StaticVariables.showHuge && StaticVariables.showMed) {
            StaticVariables.showLarge = !StaticVariables.showLarge;
        }
        updateButtons();
    }
    public void clicked6Button() {
        if (StaticVariables.showLarge && StaticVariables.showMed) {
            StaticVariables.showHuge = !StaticVariables.showHuge;
        }
        updateButtons();
    }
}