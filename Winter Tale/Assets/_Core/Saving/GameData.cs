using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using RPG.Characters;
using System.IO;

[System.Serializable]
public struct Position
{
    public float x, y, z;

    public Position(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
}

[System.Serializable]
public class SaveData
{
    public Position spawnPoint;
    public int currSongIndex;
}

public class GameData : MonoBehaviour
{
    [SerializeField] PlayerControl player;

    public static GameData Instance;

    public SaveData saveData;

    void Awake()
    {
        Instance = this;

        Load();
        if (saveData.spawnPoint.x != 0 && saveData.spawnPoint.y != 0)
        {
            player.gameObject.SetActive(false);
            SetPlayer();
            player.gameObject.SetActive(true);
        }
    }

    private void Start()
    {
        MusicPlaylist.Instance.PlaySongWithIndex(saveData.currSongIndex);
    }

    public void Save()
    {
        string path = Application.persistentDataPath + "/GameData.data";
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream stream = File.Open(path, FileMode.Create);

        SaveData data = new SaveData();
        data = saveData;

        binaryFormatter.Serialize(stream, data);
        stream.Close();
    }

    public void Load()
    {
        string path = Application.persistentDataPath + "/GameData.data";
        if (File.Exists(path))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream stream = File.Open(path, FileMode.Open);

            saveData = binaryFormatter.Deserialize(stream) as SaveData;
            stream.Close();
        }
    }

    void SetPlayer()
    {
        var pos = new Vector3(saveData.spawnPoint.x, saveData.spawnPoint.y, saveData.spawnPoint.z);
        print(pos);
        player.transform.position = pos;
    }

    public void EraseGameData()
    {
        string path = Application.persistentDataPath + "/GameData.data";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}
