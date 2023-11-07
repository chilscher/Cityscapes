//for Cityscapes, copyright Cole Hilscher 2020

using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;

public static class InterfaceFunctions{
    //here are several functions, used to color various objects in various scenes and various scripts
    //also, here are some useful functions for getting a specific skin object


    public static void ColorMenuButton(GameObject button, Skin skin) {
        //colors the interior and exterior components of the provided button to match the current skin
        //the button is assumed to be on one of the menu scenes, which use a specific color set per skin
        button.transform.Find("Button Image").Find("Borders").GetComponent<Image>().color = skin.menuButtonBorder;
        button.transform.Find("Button Image").Find("Interior").GetComponent<Image>().color = skin.menuButtonInside;
    }

    public static void ColorPuzzleButtonOff(GameObject button, Skin skin) {
        button.transform.Find("Button Image").Find("Borders").GetComponent<Image>().color = skin.puzzleButtonBorder_Off;
        button.transform.Find("Button Image").Find("Interior").GetComponent<Image>().color = skin.puzzleButtonInside_Off;
    }

    public static void ColorPuzzleButtonOn(GameObject button, Skin skin) {
        button.transform.Find("Button Image").Find("Borders").GetComponent<Image>().color = skin.puzzleButtonBorder_On;
        button.transform.Find("Button Image").Find("Interior").GetComponent<Image>().color = skin.puzzleButtonInside_On;
    }

    public static Skin GetSkinFromName(string name) {
        //given a name, iterates through all of the skins in the whole game to find the skin with that name
        foreach (Skin skin in StaticVariables.allSkins) {
            if (skin.skinName == name)
                return skin;
        }
        return null;
    }
    public static Skin GetDefaultSkin() {
        //returns the basic skin
        return StaticVariables.allSkins[0];
    }
    public static Color GetColorFromString(string s) {
        //takes the string hex version of a color, and returns the color
        Color c;
        ColorUtility.TryParseHtmlString(s, out c);
        return c;
    }
}
