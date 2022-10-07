using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Linq;
using UnityEditor;

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

    public bool isOnDeleteField;
    public bool isOnUndoField;

    private FightStatus fightStatus = FightStatus.Null;

    private Sprite[] iconType;

    public bool IsPause { get; set; }
    private void Start()
    {
        iconType = Resources.LoadAll<Sprite>("IconType");
        LoadCurrentTeam();
        UIManager.Instance.satePanel.UpdateStatePanel();
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
#endif  
    [Button("Load Level")]
    private void LoadCurrentLevel(int state, int level, string path = "States/")
    {
        List<Vector2> enemyLoc = fields.Where(f => f.Value != null && f.Value.CompareTag(Helper.ENEMY_UNIT_TAG)).Select(f => f.Key).ToList();

        foreach (var item in enemyLoc)
        {
            Destroy(fields[item].gameObject);
            fields[item] = null;
        }

        State currentState = Resources.LoadAll<State>(path).Where(s => s.id == state.ToString()).FirstOrDefault();

        Level currentLevel = currentState.listLevel.Where(l => l.id == level.ToString()).FirstOrDefault();

        foreach (var item in currentLevel.listEnemy)
        {
            if (item.unitTag.Equals(Helper.ENEMY_UNIT_TAG))
                new EnemyUnit(GetDataMerge().listUnits.Where(u => u.unitName == item.nameUnit).FirstOrDefault(), new Vector2(item.px, item.py));
        }
    }

    public void LoadCurrentTeam()
    {
        InitalFields();
        List<UnitDataJson> data = DataManager.Instance.GetDataGame().listUnits;
        data.Reverse();

        foreach (var item in data)
        {
            if (item.unitTag.Equals(Helper.PLAYER_UNIT_TAG))
                new PlayerUnit(GetDataMerge().listUnits.Where(u => u.unitName == item.nameUnit).FirstOrDefault(), new Vector2(item.px, item.py));
        }

        LoadCurrentLevel(DataManager.Instance.GetDataGame().GetCurrentState(), DataManager.Instance.GetDataGame().GetCurrentLevel(), "States/");
    }
    private void InitalFields()
    {
        List<Vector2> playerLoc = fields.Where(f => f.Value != null && f.Value.CompareTag(Helper.PLAYER_UNIT_TAG)).Select(f => f.Key).ToList();

        foreach (var item in playerLoc)
        {
            Destroy(fields[item].gameObject);
            fields[item] = null;
        }
    }
    public void StartFight()
    {
        isStart = true;
        SetFightStatus(FightStatus.Null);
        DataManager.Instance.SaveFieldData();
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
}
public enum FightStatus
{
    Null,
    Win,
    Lose
}