//for Cityscapes, copyright Cole Hilscher 2024

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsCanvasController : MonoBehaviour {
    //controls the credits canvas. Only one is used, and only on the credits scene.

    public GameObject background;
    public List<SkinApplicator> skinApplicators;
    
    


    private void Start() {
        background.GetComponent<Image>().sprite = StaticVariables.skin.mainMenuBackground;
        foreach (SkinApplicator sa in skinApplicators)
            sa.ApplySkin(StaticVariables.skin);
    }
    
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
            StaticVariables.FadeOutThenLoadScene("MainMenu");
    }

    public void PushMainMenuButton() {
        StaticVariables.FadeOutThenLoadScene("MainMenu");
    }

    public void PushSettingsButton() {
        StaticVariables.FadeOutThenLoadScene("Settings");
    }

    private void OnApplicationQuit() {
        SaveSystem.SaveGame();
    }
}