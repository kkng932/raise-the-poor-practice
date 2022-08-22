using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBuy : MonoBehaviour
{
    [SerializeField]
    GameObject[] Paints;

    [Inject]
    GameData gameData;
    InjectObj injectObj = new InjectObj();

    private void OnEnable()
    {
        injectObj.Inject(this);
        for (int i=0;i<Paints.Length;i++)
        {
            Paints[i].SetActive(gameData.Paint[i].buy_status);
        }
        
    }
    public void Show(int code, bool status)
    {
        if(code <= Paints.Length)
            Paints[code - 1].SetActive(status);
    }
}
