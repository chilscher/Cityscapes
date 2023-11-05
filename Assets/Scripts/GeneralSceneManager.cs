using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GeneralSceneManager : MonoBehaviour{

    public Image fadeImage;
    private bool hasStarted = false;

    void Start(){
        Setup();
    }

    public void Setup(){
        if (!hasStarted){
            StaticVariables.tweenDummy = transform;
            StaticVariables.fadeImage = fadeImage;
            StaticVariables.FadeIntoScene();
            hasStarted = true;
        }
    }
}