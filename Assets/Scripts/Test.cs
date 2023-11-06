using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[ExecuteAlways]
public class Test : MonoBehaviour
{
    [SerializeField] RectTransform rulerEnd;
    [SerializeField, ReadOnly] float dist;
    private void Start()
    {
    }

    void Update()
    {
        if (rulerEnd) dist = Vector2.Distance(GetComponent<RectTransform>().anchoredPosition, rulerEnd.anchoredPosition);
    }
}
