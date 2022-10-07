using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Linq;
using UnityEditor;

public class UIManager : MonoBehaviour
{
    #region Singleton
    public static UIManager Instance { get; set; }
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

    }
    #endregion

    private void Start()
    {
        mainMenuPanel.gameObject.SetActive(true);
    }

    public MainMenuPanel mainMenuPanel;
    public YouWinPanel youWinPanel;
    public YouLosePanel youLosePanel;
    public UnitLibaryManager libaryPanel;
    public NewUnitPanel newUnitPanel;
    public SettingPanel settingPanel;
    public RatePanel ratePanel;
    public StatePanel satePanel;
}
