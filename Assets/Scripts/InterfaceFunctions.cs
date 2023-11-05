//for Cityscapes, copyright Cole Hilscher 2020

using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;

public static class InterfaceFunctions{
    //here are several functions, used to color various objects in various scenes and various scripts
    //also, here are some useful functions for getting a specific skin object


    public static void ColorMenuButton(GameObject button) {
        //colors the interior and exterior components of the provided button to match the current skin
        //the button is assumed to be on one of the menu scenes, which use a specific color set per skin
        Color buttonColorExterior;
        Color buttonColorInterior;

        ColorUtility.TryParseHtmlString(StaticVariables.skin.mainMenuButtonExterior, out buttonColorExterior);
        ColorUtility.TryParseHtmlString(StaticVariables.skin.mainMenuButtonInterior, out buttonColorInterior);

        button.transform.Find("Button Image").Find("Borders").GetComponent<Image>().color = buttonColorExterior;
        button.transform.Find("Button Image").Find("Interior").GetComponent<Image>().color = buttonColorInterior;
    }
    public static void ColorMenuButton(Transform button) {
        //another way to call olorMenuButton
        ColorMenuButton(button.gameObject);
    }

    public static void ColorPuzzleButton(GameObject button) {
        //another way to call colorPuzzleButton
        ColorPuzzleButton(button, StaticVariables.skin);
    }
    public static void ColorPuzzleButton(Transform button) {
        //another way to call colorPuzzleButton
        ColorPuzzleButton(button.gameObject);
    }
    public static void ColorPuzzleButton(GameObject button, Skin skin) {
        //colors the interior and exterior components of the provided button to match the current skin
        //the button is assumed to be in the puzzle scene, which uses a specific color set per skin
        //this colors the button to the "off" coloration
        Color buttonColorExterior;
        Color buttonColorInterior;

        ColorUtility.TryParseHtmlString(skin.offButtonColorExterior, out buttonColorExterior);
        ColorUtility.TryParseHtmlString(skin.offButtonColorInterior, out buttonColorInterior);

        button.transform.Find("Button Image").Find("Borders").GetComponent<Image>().color = buttonColorExterior;
        button.transform.Find("Button Image").Find("Interior").GetComponent<Image>().color = buttonColorInterior;
    }
    public static void ColorPuzzleButton(Transform button, Skin skin) {
        //another wat to call colorPuzzleButton
        ColorPuzzleButton(button.gameObject, skin);
    }

    public static void ColorPuzzleButtonOn(GameObject button) {
        //another way to call colorPuzzleButtonOn
        ColorPuzzleButtonOn(button, StaticVariables.skin);
    }
    public static void ColorPuzzleButtonOn(Transform button) {
        //another way to call colorPuzzleButtonOn
        ColorPuzzleButtonOn(button.gameObject);
    }
    public static void ColorPuzzleButtonOn(GameObject button, Skin skin) {
        //colors the interior and exterior components of the provided button to match the current skin
        //the button is assumed to be in the puzzle scene, which uses a specific color set per skin
        //this colors the button to the "on" coloration
        Color buttonColorExterior;
        Color buttonColorInterior;

        ColorUtility.TryParseHtmlString(skin.onButtonColorExterior, out buttonColorExterior);
        ColorUtility.TryParseHtmlString(skin.onButtonColorInterior, out buttonColorInterior);

        button.transform.Find("Button Image").Find("Borders").GetComponent<Image>().color = buttonColorExterior;
        button.transform.Find("Button Image").Find("Interior").GetComponent<Image>().color = buttonColorInterior;
    }
    public static void ColorPuzzleButtonOn(Transform button, Skin skin) {
        //another way to call colorPuzzleButtonOn
        ColorPuzzleButtonOn(button.gameObject, skin);
    }

    public static Skin GetSkinFromName(string name) {
        //given a name, iterates through all of the skins in the whole game to find the skin with that name
        foreach (Skin skin in StaticVariables.allSkins) {
            if (skin.skinName == name) {
                return skin;
            }
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
