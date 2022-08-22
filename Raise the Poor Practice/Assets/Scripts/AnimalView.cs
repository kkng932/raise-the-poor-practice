using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalView : MonoBehaviour
{
    [SerializeField]
    GameObject[] Animals;

    [Inject]
    GameData gameData;
    InjectObj injectObj = new InjectObj();

    private void OnEnable()
    {
        injectObj.Inject(this);
        for(int i=0;i<Animals.Length;i++)
        {
            Animals[i].SetActive(gameData.Arbeit[i].level > 0);
        }
    }
    public void Show(int code, int level)
    {
        if (code <= Animals.Length)
            if (level > 0)
                Animals[code - 1].SetActive(true);
    }

}
