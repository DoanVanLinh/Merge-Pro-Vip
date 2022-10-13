using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class CoinsPanel : MonoBehaviour
{
    public TextMeshProUGUI textCoins;
    void Start()
    {
        UpdateTextCoins();
    }

    public void UpdateTextCoins()
    {
        textCoins.text = DataManager.Instance.GetDataGame().currentCoins.ToString();
    }
}
