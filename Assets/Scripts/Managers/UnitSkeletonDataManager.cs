using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using System.Linq;
using Sirenix.OdinInspector;


public class UnitSkeletonDataManager : MonoBehaviour
{
    #region Singleton
    public static UnitSkeletonDataManager Instance { get; set; }
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        LoadSkeletonData();
    }
    #endregion
    [SerializeField] public string path;
    [SerializeField] private SkeletonDataAsset[] listDatas;
    [Button("Get Skeleton Data")]
    void LoadSkeletonData()
    {
        listDatas = Resources.LoadAll<SkeletonDataAsset>(path);
    }
    public SkeletonDataAsset GetSkeletonData(string name)
    {
        return listDatas.Where(d => d.name.ToLower().Contains(name.ToLower())).FirstOrDefault();
    }
}
