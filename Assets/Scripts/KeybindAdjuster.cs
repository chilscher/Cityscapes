//for Cityscapes, copyright Cole Hilscher 2024

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeybindAdjuster : MonoBehaviour {

    public Text currentValidKeybindText;
    public GameObject validKeybind;
    public GameObject noKeybind;
    public GameObject clickToChange;
    public GameObject pressAnyKey;

    public void DisplayKeybind(KeyCode keyCode, bool currentlyEditable){
        clickToChange.SetActive(!currentlyEditable);
        pressAnyKey.SetActive(currentlyEditable);
        validKeybind.SetActive(keyCode != KeyCode.None);
        noKeybind.SetActive(keyCode == KeyCode.None);
        currentValidKeybindText.text = "KEYBIND: " + GetCurrentKeybindString(keyCode).ToUpper();
    }

    private string GetCurrentKeybindString(KeyCode kc){
        if (kc == KeyCode.Alpha0)
            return "0";
        if (kc == KeyCode.Alpha1)
            return "1";
        if (kc == KeyCode.Alpha2)
            return "2";
        if (kc == KeyCode.Alpha3)
            return "3";
        if (kc == KeyCode.Alpha4)
            return "4";
        if (kc == KeyCode.Alpha5)
            return "5";
        if (kc == KeyCode.Alpha6)
            return "6";
        if (kc == KeyCode.Alpha7)
            return "7";
        if (kc == KeyCode.Alpha8)
            return "8";
        if (kc == KeyCode.Alpha9)
            return "9";
        return kc.ToString();
    }
}
