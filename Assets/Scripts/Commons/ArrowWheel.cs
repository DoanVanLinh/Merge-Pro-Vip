using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArrowWheel : MonoBehaviour
{
    public float speed;
    public bool isRotate;
    public int extra;
    public GameObject higlight;
    public TextMeshProUGUI textCoinsClaim;
    void Update()
    {
        if (!isRotate)
            return;

        transform.Rotate(Vector3.forward * Time.deltaTime * speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(Helper.UNIT_EXTRA_TAG))
            return;

        extra = int.Parse(collision.name);

        textCoinsClaim.text = (GameManager.Instance.coinsEachLevel * extra).ToString();
        higlight.GetComponent<RectTransform>().rotation = collision.GetComponent<RectTransform>().rotation;
    }
}
