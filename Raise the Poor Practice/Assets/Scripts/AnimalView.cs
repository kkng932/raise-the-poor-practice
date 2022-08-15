using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalView : MonoBehaviour
{
    [SerializeField]
    GameObject[] Animals;

    private void Start()
    {
        
    }
    public void employee(int code)
    {
        Animals[code-1].SetActive(true);
    }
}
