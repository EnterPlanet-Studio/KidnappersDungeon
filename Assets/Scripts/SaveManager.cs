using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    public SaveData activeSave;
    public bool hasLoaded;

    private void Awake()
    {
    //  в качестве образца instance делаем сами себя
        instance = this;
        Load();
    }

    public void Save()
    {
        string dataPath = Application.persistentDataPath;
        // можно указать любой путь руками

        var fSerializer = new XmlSerializer(typeof(SaveData));
        var fStream = new FileStream(dataPath + "/" + activeSave.saveName + ".save", FileMode.Create);
        fSerializer.Serialize(fStream, activeSave);
        fStream.Close();

        Debug.Log("Saved");
    }

    public void Load()
    {
        string dataPath = Application.persistentDataPath;
        // можно указать любой путь руками

        // Проверяем наличие файл для начала сохранения
        if(System.IO.File.Exists(dataPath + "/" + activeSave.saveName + ".save"))
        {
            // создаем Сериалайзер с соответствующим типом
            var fSerializer = new XmlSerializer(typeof(SaveData));
            // создаем файловый поток к файлу сохранения
            var fStream = new FileStream(dataPath + "/" + activeSave.saveName + ".save", FileMode.Open);
            // десериализуем данные обратно в игровые данные из файла
            activeSave = fSerializer.Deserialize(fStream) as SaveData;
            // закрываем файловый поток
            fStream.Close();
            // Устанавливаем логическую переменную загрузки
            hasLoaded = true;
        }
    }

    public void SaveDelete()
    {
        string dataPath = Application.persistentDataPath;
        // можно указать любой путь руками
        // Проверяем наличие файл для начала сохранения
        if (System.IO.File.Exists(dataPath + "/" + activeSave.saveName + ".save"))
        {
            // удаляем найденный файл сохранения
            File.Delete(dataPath + "/" + activeSave.saveName + ".save");
        }
    }
}

[System.Serializable]
public class SaveData
{
    public string saveName = "pos";
    public Vector3 respawnPosition;
}
