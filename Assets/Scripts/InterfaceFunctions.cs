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
        Color buttonColorExterior;
        Color buttonColorInterior;

        ColorUtility.TryParseHtmlString(StaticVariables.skin.offButtonColorExterior, out buttonColorExterior);
        ColorUtility.TryParseHtmlString(StaticVariables.skin.offButtonColorInterior, out buttonColorInterior);

        button.transform.Find("Button Image").Find("Borders").GetComponent<Image>().color = buttonColorExterior;
        button.transform.Find("Button Image").Find("Interior").GetComponent<Image>().color = buttonColorInterior;
    }
    public static void colorPuzzleButton(Transform button) {
        colorPuzzleButton(button.gameObject);
    }

    public static void colorPuzzleButtonOn(GameObject button) {
        Color buttonColorExterior;
        Color buttonColorInterior;

        ColorUtility.TryParseHtmlString(StaticVariables.skin.onButtonColorExterior, out buttonColorExterior);
        ColorUtility.TryParseHtmlString(StaticVariables.skin.onButtonColorInterior, out buttonColorInterior);

        button.transform.Find("Button Image").Find("Borders").GetComponent<Image>().color = buttonColorExterior;
        button.transform.Find("Button Image").Find("Interior").GetComponent<Image>().color = buttonColorInterior;
    }
    public static void colorPuzzleButtonOn(Transform button) {
        colorPuzzleButtonOn(button.gameObject);
    }
}
