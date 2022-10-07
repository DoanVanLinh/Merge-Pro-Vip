using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleClickButton : MonoBehaviour
{
    private List<Button> listButtons = new List<Button>();
    private void Start()
    {
        listButtons.AddRange(transform.GetComponentsInChildren<Button>());
        Button button;
        if (transform.TryGetComponent<Button>(out button))
            listButtons.Add(button);
    }

    public void DisableAllButton()
    {
        foreach (Button button in listButtons)
        {
            button.interactable = false;
        }
    }
    public void EnableAllButton()
    {
        foreach (Button button in listButtons)
        {
            button.interactable = true;
        }
    }
}
