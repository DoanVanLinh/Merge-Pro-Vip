using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragable : MonoBehaviour
{
    public Vector2 defaultLoc;

    private Camera mainCam;
    private bool isSelecting;
    private MergeController unitMergeController;
    private Unit unitInfo;
    private void Start()
    {
        mainCam = Camera.main;
        defaultLoc = transform.position;
        isSelecting = false;
        unitMergeController = GetComponent<MergeController>();
        unitInfo = GetComponent<Unit>();
    }
    private void OnMouseDown()
    {
        if (GameManager.Instance.isStart)
            return;

        isSelecting = true;
    }
    private void OnMouseDrag()
    {
        if (!isSelecting)
            return;

        transform.position = (Vector2)mainCam.ScreenToWorldPoint(Input.mousePosition);
    }
    private void OnMouseUp()
    {
        if (!isSelecting)
            return;

        if (GameManager.Instance.isOnDeleteField)
            Destroy(gameObject);

        if (GameManager.Instance.isOnUndoField)
            if (unitMergeController.Split())
                return;

        isSelecting = false;
        Snap();
    }
    void Snap()
    {
        int x = Mathf.RoundToInt(transform.position.x);
        int y = Mathf.RoundToInt(transform.position.y);
        Vector2 newLoc = new Vector2(x, y);



        if ((x >= 0 && x < GameManager.Instance.col) && (y >= 0 && y < GameManager.Instance.row))
        {
            transform.position = newLoc;

            Unit otherUnit = GameManager.fields[newLoc];
            if (otherUnit == null)//to null loc
            {
                GameManager.fields[newLoc] = unitInfo;
                GameManager.fields[defaultLoc] = null;
                defaultLoc = newLoc;
                unitInfo.SetDefaulLoc();
                return;
            }

            if (gameObject.CompareTag(Helper.PLAYER_UNIT_TAG))
            {
                if (!unitMergeController.Merge(otherUnit))//merge
                    SwapUnit(ref otherUnit);//Swap
            }
            else
                SwapUnit(ref otherUnit);//Swap

            defaultLoc = newLoc;
        }
        else
            transform.position = defaultLoc;
    }
    void SwapUnit(ref Unit otherUnit)
    {
        otherUnit.transform.position = defaultLoc;
        otherUnit.GetComponent<Dragable>().defaultLoc = defaultLoc;

        GameManager.fields[otherUnit.transform.position] = otherUnit;
        GameManager.fields[transform.position] = unitInfo;

        otherUnit.SetDefaulLoc();
        unitInfo.SetDefaulLoc();
    }
}
