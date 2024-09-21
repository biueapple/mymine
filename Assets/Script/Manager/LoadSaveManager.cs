using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System;

public class LoadSaveManager
{
    private static LoadSaveManager instance;
    public static LoadSaveManager Instance
    {
        get
        {
            if (instance == null)
                instance = new LoadSaveManager();
            return instance;
        }
    }
    //저장 경로 : C:\Users\사용자명\AppData\LocalLow\DefaultCompany\...
    public void Save<T>(T t, string path) where T : class
    {
        try
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, "/" + path), FileMode.Create, FileAccess.Write);
            formatter.Serialize(stream, t);
            stream.Close();
        }
        catch (Exception ex)
        {
            Debug.LogError($"예외 발생: {ex.Message}");
        }
    }
    //불러오기 경로 : C:\Users\사용자명\AppData\LocalLow\DefaultCompany\...
    public bool Load<T>(ref T t, string path)
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, "/" + path)))
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, "/" + path), FileMode.Open, FileAccess.Read);
            t = (T)formatter.Deserialize(stream);
            
            stream.Close();
            return true;
        }
        return false;
    }
}
