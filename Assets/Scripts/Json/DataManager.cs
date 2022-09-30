using UnityEngine;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Sirenix.OdinInspector;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    [SerializeField] const string fileName = "Data.json";
    [SerializeField] private GameData gameData;

    private string path;
    private void Awake()
    {
        path = Application.persistentDataPath + "/" + fileName;
        if (Instance == null)
        {
            Instance = this;
            if (File.Exists(path))
                LoadData();
            else
            {
                gameData = new GameData();
                CreateData();
            }
        }
        else
            Destroy(gameObject);

        DontDestroyOnLoad(this);
    }
    public GameData GetDataGame()
    {
        return gameData;
    }
    public void SaveFieldData()
    {
        gameData.listUnits = new List<UnitDataJson>();
        List<Unit> unitDatas = GameManager.fields.Where(f => f.Value != null).Select(f => f.Value).ToList();

        foreach (var item in unitDatas)
        {
            if (item.CompareTag(Helper.PLAYER_UNIT_TAG))
                gameData.listUnits.Add(new UnitDataJson(item.Data.unitName, item.GetDefaulLoc().x, item.GetDefaulLoc().y, item.tag));
        }
        SaveLoadJson.Create(fileName, gameData);
    }

    public void CreateData()
    {
        SaveLoadJson.Create(fileName, gameData);
    }
    public void LoadData()
    {
        gameData = SaveLoadJson.Load<GameData>(fileName);
    }

#if UNITY_EDITOR
    [Button("Delete Data")]
    void DeleteFileDataJson()
    {
        string _path = Application.persistentDataPath + "/" + fileName;
        if (File.Exists(_path))
            File.Delete(_path);
    }
#endif
}