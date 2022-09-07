using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoorPower_Enterprise : MonoBehaviour
{
    [SerializeField] private Image[] BonusImg;
    [SerializeField] private Text[] BonusTxt;

    [Inject]
    GameData gameData;
    InjectObj InjectObj = new InjectObj();

    private void OnEnable()
    {
        SpriteSheetManager.Load("enterprise");
        InjectObj.Inject(this);
        UpdateContent();
    }
    private void UpdateContent()
    {
        for (int i = 0; i < gameData.Enterprise.Count; i++)
        {
            BonusImg[i].sprite = SpriteSheetManager.GetSpriteByName("enterprise", "enterprise_" + i.ToString());
            if (gameData.Enterprise[i].buy_status)
            {
                BonusImg[i].color = Color.white;
                BonusTxt[i].text = "<color=red>";
            }
            else
            {
                BonusImg[i].color = Color.gray;
                BonusTxt[i].text = "<color=#3F3F3F>";
            }
            BonusTxt[i].text += gameData.Enterprise[i].name + "\n +" + gameData.Enterprise[i].bonus.ToString() + "%</color>";
        }
    }

}
