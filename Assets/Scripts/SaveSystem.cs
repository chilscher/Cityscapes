//for Cityscapes, copyright Cole Hilscher 2024

using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem{
    //handles the saving and loading of the player's stored game data. References SaveData.cs

    static private string path = Application.persistentDataPath + "/save.chh";

    public static void SaveGame() {
        BinaryFormatter formatter = new BinaryFormatter();

        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData();

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void LoadGame() {
        if (File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            if (stream.Length == 0) {
                stream.Close();
                FirstTimePlayingEver();
            }
            else {
                SaveData data = formatter.Deserialize(stream) as SaveData;
                stream.Close();
                data.LoadData();
            }
        }
        else
            FirstTimePlayingEver();
    }

    private static void FirstTimePlayingEver() {
        //if this is the first time that the player has opened the game, load the default values for some staticVariables elements
        StaticVariables.skin = InterfaceFunctions.GetDefaultSkin();
        StaticVariables.coins = 0;
        StaticVariables.highestUnlockedSize = 3;
        StaticVariables.ApplyDefaultKeybinds();
        SaveGame();
        LoadGame();
    }
}
