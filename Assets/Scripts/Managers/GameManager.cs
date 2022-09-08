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
        for (int i = 0; i < row * 2 + 1; i++)
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
    public ListUnitData dataMerge;
    public GameObject playerZone;

    public bool isStart;
    private FightStatus fightStatus = FightStatus.Null;

    private void Start()
    {
        LoadCurrentTeam();
    }
    [Button("Spawn Player Unit")]
    public void SpawnPlayerUnit(string index)
    {
        int[] indexUnit = index.Split(',').Select(i => int.Parse(i)).ToArray();

        int amountEmpty = fields.Where(f => f.Value == null).Count();

        if (amountEmpty == 0)
            return;

        float random = Random.Range(-1f, 1f);

        UnitData unitData = dataMerge.listUnits[indexUnit[Random.Range(0, indexUnit.Length)]];
        Vector2 loc = fields.Where(f => f.Value == null).Select(f => f.Key).OrderBy(o => random).FirstOrDefault();

        new PlayerUnit(unitData, loc);
    }
    [Button("Spawn Enemy Unit")]
    private void SpawnEnemyUnit(int index)
    {
        float random = Random.Range(-1f, 1f);

        UnitData unitData = dataMerge.listUnits[index];
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
            level.listEnemy.Add(new UnitDataJson(item.Data, item.GetDefaulLoc().x, item.GetDefaulLoc().y, item.tag));
        }
        state.listLevel.Add(level);

        AssetDatabase.CreateAsset(level, "Assets/Resources/States/State " + state.id + "/Level " + level.id + ".asset");

        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = level;

        UnityEditor.EditorUtility.SetDirty(level);
        UnityEditor.EditorUtility.SetDirty(state);
    }
    [Button("Load Level")]
    private void LoadCurrentLevel(int state, int level, string path)
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
                new EnemyUnit(item.data, new Vector2(item.px, item.py));
        }
    }
#endif
    public void LoadCurrentTeam()
    {
        InitalFields();
        foreach (var item in DataManager.Instance.GetDataGame().listUnits)
        {
            if (item.unitTag.Equals(Helper.PLAYER_UNIT_TAG))
                new PlayerUnit(item.data, new Vector2(item.px, item.py));
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
            Debug.Log(this.fightStatus);
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
                UIManager.Instance.youLosePanel.gameObject.SetActive(true);
        }
    }
    public FightStatus GetFightStatus()
    {
        return this.fightStatus;
    }
}

public enum FightStatus
{
    Null,
    Win,
    Lose
}