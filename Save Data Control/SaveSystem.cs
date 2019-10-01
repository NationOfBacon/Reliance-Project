using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveData(HUBTracker tracker, InventoryManager manager, string fileName)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + fileName + ".plr";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(tracker);

        formatter.Serialize(stream, data);

        Debug.Log("Data saved to file at " + path);

        string path1 = Application.persistentDataPath + "/" + fileName + ".inv";
        FileStream stream1 = new FileStream(path1, FileMode.Create);

        InventoryData data1 = new InventoryData(manager);

        formatter.Serialize(stream1, data1);

        Debug.Log("Data saved to file at " + path1);

        stream.Close();
        stream1.Close();
    }

    public static void SaveFileData(SaveFileManager saveMgr)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/SaveFileNames.data";
        FileStream stream = new FileStream(path, FileMode.Create);

        FileData data = new FileData(saveMgr);

        formatter.Serialize(stream, data);

        Debug.Log("Data saved to file at " + path);

        stream.Close();
    }

    public static PlayerData LoadData(string fileName)
    {
        string path = Application.persistentDataPath + "/" + fileName + ".plr";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            Debug.Log("Save data retrieved from file at " + path);
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save data not found in " + path);
            return null;
        }
    }

    public static InventoryData LoadInventoryData(string fileName)
    {
        string path = Application.persistentDataPath + "/" + fileName + ".inv";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            InventoryData data = formatter.Deserialize(stream) as InventoryData;
            Debug.Log("Save data retrieved from file at " + path);
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save data not found in " + path);
            return null;
        }
    }
    
    public static FileData LoadFileData()
    {
        string path = Application.persistentDataPath + "/SaveFileNames.data";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            FileData data = formatter.Deserialize(stream) as FileData;
            Debug.Log("Save data retrieved from file at " + path);
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save data not found in " + path);
            return null;
        }
    }

    public static void DeleteSave(string fileName)
    {
        string path = Application.persistentDataPath + "/" + fileName + ".plr";
        string path1 = Application.persistentDataPath + "/" + fileName + ".inv";
        File.Delete(path);
        File.Delete(path1);

        Debug.Log("Data file deleted at " + path);
        Debug.Log("Data file deleted at " + path1);

    }
}
