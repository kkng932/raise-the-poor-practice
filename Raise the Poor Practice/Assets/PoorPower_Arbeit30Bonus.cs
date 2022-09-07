using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoorPower_Arbeit30Bonus : MonoBehaviour
{
    [SerializeField] private Image[] iconImg;
    [SerializeField] private Text[] DescTxt;

    [Inject]
    GameData gameData;
    InjectObj InjectObj = new InjectObj();
    void OnEnable()
    {
        InjectObj.Inject(this);
        SpriteSheetManager.Load("fruits");
        UpdateContent();
    }
    void UpdateContent()
    {
        var arHap = gameData.ArbeitHappiness;
        for(int i=0;i<arHap.Count;i++)
        {
            if(arHap[i].buy_status)
            {
                iconImg[i].sprite = SpriteSheetManager.GetSpriteByName("fruits", "fruits_" + i.ToString());
                DescTxt[i].text = arHap[i].name+"\n(원/클릭 " + arHap[i].bonus.ToString() + "% 버프)";
            }
            else
            {
                iconImg[i].sprite = Resources.Load<Sprite>("lock") as Sprite;
                DescTxt[i].text = gameData.Arbeit[i].name+"의 보물\n(조건:"+gameData.Arbeit[i].name+"의 행복 달성)";
            }
        }
    }
}
