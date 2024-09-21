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
    //���� ��� : C:\Users\����ڸ�\AppData\LocalLow\DefaultCompany\...
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
            Debug.LogError($"���� �߻�: {ex.Message}");
        }
    }
    //�ҷ����� ��� : C:\Users\����ڸ�\AppData\LocalLow\DefaultCompany\...
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
