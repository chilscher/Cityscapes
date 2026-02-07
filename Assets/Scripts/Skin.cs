//for Cityscapes, copyright Fancy Bus Games 2026

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Skin: MonoBehaviour{
    //to add a new skin:
    //duplicate a preexisting skin prefab
    //change the images and colors of the new prefab, and set the skinName in the inspector
    //in the launchGameSetup scene, add the new skin to the list of all skins
    //in the shop scene, duplicate a preexisting skin button and move it to the right spot alphabetically
    //update the button name and the button text
    //add the skinApplicator of the new button to the shopCanvasController skinApplicators list
    //in the settings scene, duplicate a preexisting skin button and move it to the right spot alphabetically
    //update the button name and the button text
    //add the skinApplicator of the new button to the settingsCanvasController skinApplicators list

    [Header("Metadata")]
    public string skinName;

    [Header("Sprites")]
    public Sprite mainMenuBackground;
    public Sprite puzzleBackground;
    public Sprite logo;
    
    [Header("Main Menu Button Colors")]
    public Color menuButtonInside; //ex: the menu-changing buttons, all buttons on the main menu
    public Color menuButtonBorder;
    public Color popupInside; //ex: the win popup or return to puzzle popup
    public Color popupBorder;
    public Color settingsText_On;
    public Color settingsText_Off;

    [Header("Puzzle Button Colors")]
    public Color puzzleButtonInside_On; //ex: the build button when build is selected
    public Color puzzleButtonBorder_On;
    public Color puzzleButtonInside_Off;  //ex: the build button when note 1 is selected
    public Color puzzleButtonBorder_Off;

    [Header("Puzzle component colors")]
    public Color normalCitizen;
    public Color satisfiedCitizen;
    public Color street;
    public Color highlightBuilding;
    public Color note1;
    public Color note2;
    public Color tileBackground;
}
