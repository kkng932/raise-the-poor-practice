using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBuy : MonoBehaviour
{
    [SerializeField]
    GameObject[] Paints;

    private void Start()
    {
        foreach (var a in Paints)
            a.SetActive(false);
    }
    public void Show(int code, bool status)
    {
        if(code <= Paints.Length)
            Paints[code - 1].SetActive(status);
    }
}
