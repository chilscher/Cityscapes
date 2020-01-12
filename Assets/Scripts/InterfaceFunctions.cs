using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;

public static class InterfaceFunctions{

    public static void colorMenuButton(GameObject button) {
        Color buttonColorExterior;
        Color buttonColorInterior;

        ColorUtility.TryParseHtmlString(StaticVariables.skin.mainMenuButtonExterior, out buttonColorExterior);
        ColorUtility.TryParseHtmlString(StaticVariables.skin.mainMenuButtonInterior, out buttonColorInterior);

        button.transform.Find("Button Image").Find("Borders").GetComponent<Image>().color = buttonColorExterior;
        button.transform.Find("Button Image").Find("Interior").GetComponent<Image>().color = buttonColorInterior;
    }
    public static void colorMenuButton(Transform button) {
        colorMenuButton(button.gameObject);
    }

    public static void colorPuzzleButton(GameObject button) {
        colorPuzzleButton(button, StaticVariables.skin);
        /*
        Color buttonColorExterior;
        Color buttonColorInterior;

        ColorUtility.TryParseHtmlString(StaticVariables.skin.offButtonColorExterior, out buttonColorExterior);
        ColorUtility.TryParseHtmlString(StaticVariables.skin.offButtonColorInterior, out buttonColorInterior);

        button.transform.Find("Button Image").Find("Borders").GetComponent<Image>().color = buttonColorExterior;
        button.transform.Find("Button Image").Find("Interior").GetComponent<Image>().color = buttonColorInterior;
        */
    }
    public static void colorPuzzleButton(Transform button) {
        colorPuzzleButton(button.gameObject);
    }
    public static void colorPuzzleButton(GameObject button, Skin skin) {
        Color buttonColorExterior;
        Color buttonColorInterior;

        ColorUtility.TryParseHtmlString(skin.offButtonColorExterior, out buttonColorExterior);
        ColorUtility.TryParseHtmlString(skin.offButtonColorInterior, out buttonColorInterior);

        button.transform.Find("Button Image").Find("Borders").GetComponent<Image>().color = buttonColorExterior;
        button.transform.Find("Button Image").Find("Interior").GetComponent<Image>().color = buttonColorInterior;
    }
    public static void colorPuzzleButton(Transform button, Skin skin) {
        colorPuzzleButton(button.gameObject, skin);
    }

    public static void colorPuzzleButtonOn(GameObject button) {
        colorPuzzleButtonOn(button, StaticVariables.skin);
        /*
        Color buttonColorExterior;
        Color buttonColorInterior;

        ColorUtility.TryParseHtmlString(StaticVariables.skin.onButtonColorExterior, out buttonColorExterior);
        ColorUtility.TryParseHtmlString(StaticVariables.skin.onButtonColorInterior, out buttonColorInterior);

        button.transform.Find("Button Image").Find("Borders").GetComponent<Image>().color = buttonColorExterior;
        button.transform.Find("Button Image").Find("Interior").GetComponent<Image>().color = buttonColorInterior;
        */
    }
    public static void colorPuzzleButtonOn(Transform button) {
        colorPuzzleButtonOn(button.gameObject);
    }
    public static void colorPuzzleButtonOn(GameObject button, Skin skin) {
        Color buttonColorExterior;
        Color buttonColorInterior;

        ColorUtility.TryParseHtmlString(skin.onButtonColorExterior, out buttonColorExterior);
        ColorUtility.TryParseHtmlString(skin.onButtonColorInterior, out buttonColorInterior);

        button.transform.Find("Button Image").Find("Borders").GetComponent<Image>().color = buttonColorExterior;
        button.transform.Find("Button Image").Find("Interior").GetComponent<Image>().color = buttonColorInterior;
    }
    public static void colorPuzzleButtonOn(Transform button, Skin skin) {
        colorPuzzleButtonOn(button.gameObject, skin);
    }

    public static Skin getSkinFromName(string name) {
        foreach (Skin skin in StaticVariables.allSkins) {
            if (skin.skinName == name) {
                return skin;
            }
        }
        return null;
    }
    public static Skin getDefaultSkin() {
        return StaticVariables.allSkins[0];
    }
    public static Color getColorFromString(string s) {
        Color c;
        ColorUtility.TryParseHtmlString(s, out c);
        return c;
    }
}
