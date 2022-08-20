using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalView : MonoBehaviour
{
    [SerializeField]
    GameObject[] Animals;


    public void Show(int code, int level)
    {
        if (code <= Animals.Length)
            if (level > 0)
                Animals[code - 1].SetActive(true);
    }

}
