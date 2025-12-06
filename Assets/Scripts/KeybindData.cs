//for Cityscapes, copyright Cole Hilscher 2024

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeybindData : MonoBehaviour {

    public enum Button {Size1, Size2, Size3, Size4, Size5, Size6, Size7, Build, Note1, Note2, Erase, Undo, Redo, RemoveAll, ClearPuzzle}
    public Button function;
    public KeyCode keycode;
    public KeyCode defaultKeycode;

}
