using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using WE.Unit;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance { get; set; }
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        fields = new Dictionary<Vector2, Unit>();
        for (int i = 0; i < row /** 2 + 1*/; i++)
        {
            for (int j = 0; j < col; j++)
            {
                fields[new Vector2(j, i)] = null;
                FieldManager.fieldPlayer[new Vector2(j, i)] = null;
            }
        }
    }
    #endregion
    public static Dictionary<Vector2, Unit> fields;
    public static Dictionary<Vector2, Unit> fieldEnemy;

    public int row;
    public int col;
    [Range(1f, 100f)]
    public float gridScale;
    public ListTypes dataType;
    public GameObject playerZone;
    public bool isStart;
    public int coinsEachLevel;
    public int coinBase;
    public float muiltiCoins;
    public GameObject unitBasePrefab;
    [HideInInspector]
    public bool isOnDeleteField;
    [HideInInspector]
    public bool isOnUndoField;



    private FightStatus fightStatus = FightStatus.Null;

    private Sprite[] iconType;

    public bool IsPause { get; set; }
    private void Start()
    {
        iconType = Resources.LoadAll<Sprite>("IconType");
        LoadCurrentTeam();
        UIManager.Instance.satePanel.UpdateStatePanel();

        fieldEnemy = new Dictionary<Vector2, Unit>(fields);
    }
    [Button("Spawn Player Unit")]
    public void SpawnPlayerUnit(string index)
    {
        int[] indexUnit = index.Split(',').Select(i => int.Parse(i)).ToArray();

        int amountEmpty = fields.Where(f => f.Value == null).Count();

        if (amountEmpty == 0)
            return;

        float random = Random.Range(-1f, 1f);

        UnitData unitData = GetDataMerge().listUnits[indexUnit[Random.Range(0, indexUnit.Length)]];

        CPlayerPrefs.SetBool(unitData.unitName, true);
        Vector2 loc = fields.Where(f => f.Value == null).Select(f => f.Key).OrderBy(o => random).FirstOrDefault();

        new PlayerUnit(unitData, loc);
    }
    [Button("Spawn Player Unit Base")]
    public void SpawnPlayerBaseUnit(string index)
    {
        int[] indexUnit = index.Split(',').Select(i => int.Parse(i)).ToArray();

        int amountEmpty = FieldManager.fieldPlayer.Where(f => f.Value == null).Count();

        if (amountEmpty == 0)
            return;

        float random = Random.Range(-1f, 1f);

        UnitData unitData = GetDataMerge().listUnits[indexUnit[Random.Range(0, indexUnit.Length)]];

        CPlayerPrefs.SetBool(unitData.unitName, true);
        Vector2 loc = FieldManager.fieldPlayer.Where(f => f.Value == null).Select(f => f.Key).OrderBy(o => random).FirstOrDefault();

        SpawnBaseUnit(loc, unitData, Helper.PLAYER_UNIT_TAG);
    }
    public void SpawnBaseUnit(Vector2 loc, UnitData unitData, string tag)
    {
        GameObject clone = Instantiate(unitBasePrefab, loc, Quaternion.identity);
        clone.tag = tag;

        BaseUnit unit = clone.GetComponent<BaseUnit>();
        unit.unitStats = unitData;
        unit.Init();
    }
    [Button("Spawn Enemy Unit")]
    private void SpawnEnemyUnit(int index)
    {
        float random = Random.Range(-1f, 1f);

        UnitData unitData = GetDataMerge().listUnits[index];
        Vector2 loc = fields.Where(f => f.Value == null).Select(f => f.Key).OrderBy(o => random).FirstOrDefault();

        new EnemyUnit(unitData, loc + Vector2.up * 4f);
    }
#if UNITY_EDITOR
    [Button("Create Level")]
    private void CreateLevel(State state)
    {
        if (state == null)
            return;

        Level level = ScriptableObject.CreateInstance<Level>();

        level.id = state.listLevel.Count.ToString();

        List<Unit> unitDatas = fields.Where(f => f.Value != null && f.Value.CompareTag(Helper.ENEMY_UNIT_TAG)).Select(f => f.Value).ToList();
        level.listEnemy = new List<UnitDataJson>();
        foreach (var item in unitDatas)
        {
            level.listEnemy.Add(new UnitDataJson(item.Data.unitName, item.GetDefaulLoc().x, item.GetDefaulLoc().y, item.tag));
        }
        state.listLevel.Add(level);

        AssetDatabase.CreateAsset(level, "Assets/Resources/States/State " + state.id + "/Level " + level.id + ".asset");

        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = level;

        UnityEditor.EditorUtility.SetDirty(level);
        UnityEditor.EditorUtility.SetDirty(state);
    }

    [Button("Load coin each Level")]
    private void LoadCoinLevel()
    {
        List<State> listState = Resources.LoadAll<State>("States/").ToList();
        int counterLevel = 0;

        foreach (State state in listState)
        {
            foreach (var level in state.listLevel)
            {
                level.cointReward = (int)(coinBase * muiltiCoins) * ++counterLevel;
                UnityEditor.EditorUtility.SetDirty(level);
            }
        }
    }
#endif  
    [Button("Load Level")]
    private void LoadCurrentLevel(int state, int level, string path = "States/")
    {
        List<Vector2> enemyLoc = FieldManager.fieldEnemy.Where(f => f.Value != null).Select(f => f.Key).ToList();

        foreach (var item in enemyLoc)
        {
            Destroy(FieldManager.fieldPlayer[item].gameObject);
            FieldManager.AddToField(item, null);
        }

        State currentState = Resources.LoadAll<State>(path).Where(s => s.id == state.ToString()).FirstOrDefault();

        Level currentLevel = currentState.listLevel.Where(l => l.id == level.ToString()).FirstOrDefault();

        foreach (var item in currentLevel.listEnemy)
        {
            if (item.unitTag.Equals(Helper.ENEMY_UNIT_TAG))
                SpawnBaseUnit(new Vector2(item.px, item.py), GetDataMerge().listUnits.Where(u => u.unitName == item.nameUnit).FirstOrDefault(), Helper.ENEMY_UNIT_TAG);
        }
    }

    public void LoadCurrentTeam()
    {
        InitalFields();
        List<UnitDataJson> data = DataManager.Instance.GetDataGame().listUnits;
        data.Reverse();

        foreach (var item in data)
        {
            SpawnBaseUnit(new Vector2(item.px, item.py), GetDataMerge().listUnits.Where(u => u.unitName == item.nameUnit).FirstOrDefault(), Helper.PLAYER_UNIT_TAG);
        }

        LoadCurrentLevel(DataManager.Instance.GetDataGame().GetCurrentState(), DataManager.Instance.GetDataGame().GetCurrentLevel(), "States/");
    }
    private void InitalFields()
    {
        List<Vector2> playerLoc = FieldManager.fieldPlayer.Where(f => f.Value != null).Select(f => f.Key).ToList();

        foreach (var item in playerLoc)
        {
            Destroy(FieldManager.fieldPlayer[item].gameObject);
            FieldManager.AddToField(item,null);
        }
    }
    public void StartFight()
    {
        isStart = true;
        SetFightStatus(FightStatus.Null);
        DataManager.Instance.SaveFieldData();
        FieldManager.StartFight();
    }

    public void SetFightStatus(FightStatus status)
    {
        if (status == FightStatus.Null)
        {
            this.fightStatus = status;
        }
        else
        {
            if (this.fightStatus != FightStatus.Null) return;
            this.fightStatus = status;
            isStart = false;

            coinsEachLevel = GetCointRewardCurrentLevel(DataManager.Instance.GetDataGame().currentState, DataManager.Instance.GetDataGame().currentLevel);

            if (this.fightStatus == FightStatus.Win)
            {
                UIManager.Instance.youWinPanel.gameObject.SetActive(true);
                DataManager.Instance.GetDataGame().NextLevel();
            }
            else
                UIManager.Instance.youWinPanel.gameObject.SetActive(true);
        }
    }
    public ListUnitData GetDataMerge()
    {
        return Resources.Load<ListUnitData>("Data Merge/List Unit Data");
    }
    public FightStatus GetFightStatus()
    {
        return this.fightStatus;
    }
    public float MultipleDame(NameTypeUnit firstType, NameTypeUnit secondType)
    {
        return dataType.listTypes[firstType][secondType];
    }
    public Sprite GetIconTypeByname(string name)
    {
        return iconType.Where(i => i.name == name).First();
    }
    public void DeleteUnitInField(Vector2 loc)
    {
        fields[loc] = null;
    }
    public void AddUnitToField(Vector2 loc, Unit unit)
    {
        fields[loc] = unit;
    }
    public int GetCointRewardCurrentLevel(int state, int level)
    {
        return Resources.LoadAll<State>("States/").Where(s => s.id == state.ToString()).FirstOrDefault().listLevel.Where(l => l.id == level.ToString()).First().cointReward;
    }
}
public enum FightStatus
{
    Null,
    Win,
    Lose
}