using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoorPower_Arbeit20Bonus : MonoBehaviour
{
    [SerializeField] private Image[] BonusImg;
    [SerializeField] private Text[] BonusTxt;

    [Inject]
    GameData gameData;
    InjectObj InjectObj = new InjectObj();
    
    private void OnEnable()
    {
        SpriteSheetManager.Load("profiles");
        InjectObj.Inject(this);
        UpdateContent();
    }
    private void UpdateContent()
    {
        for (int i = 0; i < gameData.Arbeit.Count; i++)
        {
            BonusImg[i].sprite = SpriteSheetManager.GetSpriteByName("profiles", "profiles_" + i.ToString());
            if (gameData.Arbeit[i].level >= 20)
            {
                BonusImg[i].color = Color.white;
                BonusTxt[i].text = "<color=red>";
            }
            else
            {
                BonusImg[i].color = Color.gray;
                BonusTxt[i].text = "<color=#3F3F3F>";
            }
            BonusTxt[i].text += gameData.Arbeit[i].name + "\n +" + gameData.Arbeit[i].bonus.ToString() + "%</color>";
        }
    }

}
