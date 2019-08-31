﻿using UnityEngine;
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

        if (File.Exists(path)) {

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();


            StaticVariables.coins = data.coins;
            StaticVariables.includeGreenNoteButton = data.includeGreenNotes;
            StaticVariables.includeRedNoteButton = data.includeRedNotes;
            StaticVariables.changeResidentColorOnCorrectRows = data.changeResidentColorOnCorrectRows;
            StaticVariables.highestUnlockedSize = data.highestUnlockedSize;

            StaticVariables.showMed = data.showMed;
            StaticVariables.showLarge = data.showLarge;
            StaticVariables.showHuge = data.showHuge;

        }
        else {
            //save file not found
            //Debug.LogError("Save file not found in " + path);
            
        }

    }



}
