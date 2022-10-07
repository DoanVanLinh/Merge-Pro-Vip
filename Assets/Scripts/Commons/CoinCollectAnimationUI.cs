using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using System;

public class CoinCollectAnimationUI : MonoBehaviour
{
    [SerializeField] private RectTransform target;
    [SerializeField] private GameObject coinsImg;

    public void CollectAnimationButton(Action actionAfterDone = null)
    {
        StartCoroutine(CollectAnimation(actionAfterDone));
    }
    public IEnumerator CollectAnimation(Action actionAfterDone = null)
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject clone = Instantiate(coinsImg, transform.position, Quaternion.identity, target);
            clone.GetComponent<RectTransform>().DOLocalMove(Vector3.zero, 1f);
            Destroy(clone.gameObject, 1f);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.5f);
        actionAfterDone?.Invoke();
        StopAllCoroutines();
    }
}
