using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoorPower_Arbeit20Bonus : MonoBehaviour
{
    [SerializeField] private Text[] Arbeit20Bonus;

    [Inject]
    GameData gameData;
    InjectObj InjectObj = new InjectObj();
    
    private void OnEnable()
    {
        InjectObj.Inject(this);
        UpdateContent();
    }
    private void UpdateContent()
    {
        for (int i = 0; i < gameData.Arbeit.Count; i++)
        {
            if (gameData.Arbeit[i].level >= 20)
                Arbeit20Bonus[i].text = "<color=red>";
            else
                Arbeit20Bonus[i].text = "<color=#3F3F3F>";
            Arbeit20Bonus[i].text += gameData.Arbeit[i].name + "\n +" + gameData.Arbeit[i].bonus.ToString() + "%</color>";
        }
    }

}
