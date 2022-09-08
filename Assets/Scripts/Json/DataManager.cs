using UnityEngine;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    [SerializeField] const string fileName = "Data.json";
    [SerializeField] private GameData gameData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            if (File.Exists(Application.persistentDataPath + "/" + fileName))
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
                gameData.listUnits.Add(new UnitDataJson(item.Data, item.GetDefaulLoc().x, item.GetDefaulLoc().y, item.tag));
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
}