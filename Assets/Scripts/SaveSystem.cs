using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem{

    static private string path = Application.persistentDataPath + "/save.chh";

    public static void SaveGame() {

        BinaryFormatter formatter = new BinaryFormatter();

        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData();

        formatter.Serialize(stream, data);
        stream.Close();

    }

    public static void LoadGame() {
        //Debug.Log(Application.persistentDataPath);
        if (File.Exists(path)) {

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            if (stream.Length == 0) {
                stream.Close();

                //Debug.Log("first time");
                firstTimePlayingEver();

            }
            else {
                //Debug.Log("returning player");
                SaveData data = formatter.Deserialize(stream) as SaveData;
                stream.Close();

                data.LoadData();

            }
        }
        else {
            firstTimePlayingEver();
            //save file not found
            //Debug.LogError("Save file not found in " + path);
            
        }

    }

    private static void firstTimePlayingEver() {
        StaticVariables.skin = InterfaceFunctions.getDefaultSkin();
        StaticVariables.coins = 0;
        StaticVariables.highestUnlockedSize = 3;
        SaveGame();
        LoadGame();
    }



}
