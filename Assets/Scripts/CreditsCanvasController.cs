//for Cityscapes, copyright Fancy Bus Games 2026

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsCanvasController : MonoBehaviour {

    public GameObject background;
    public List<SkinApplicator> skinApplicators;
    
    
    private void Start() {
        background.GetComponent<Image>().sprite = StaticVariables.skin.mainMenuBackground;
        foreach (SkinApplicator sa in skinApplicators)
            sa.ApplySkin(StaticVariables.skin);
    }
    
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneChanger.GoMenu();
    }

    public void PushMainMenuButton() {
        SceneChanger.GoMenu();
    }

    public void PushSettingsButton() {
        SceneChanger.GoSettings();
    }

    private void OnApplicationQuit() {
        SaveSystem.SaveGame();
    }
}