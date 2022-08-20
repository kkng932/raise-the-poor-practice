using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintView : MonoBehaviour
{
    [SerializeField] GameObject[] Paints;
    void OnEnable()
    {
        foreach (var p in Paints)
            p.SetActive(false);
    }
    
}
