using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoorPower_CurrClick : MonoBehaviour
{
    [SerializeField] private Text PerClick;
    [SerializeField] private Text ArbeitBonus;
    [SerializeField] private Text ArbeitTreasureBonus;
    [SerializeField] private Text EnterpriseBonus;

    [Inject]
    UserData userData;
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
        PerClick.text = "현재원/클릭: " + Utility.MoneyToString(userData.per_second);
        
        double arbeitBonus=0;
        foreach(var a in gameData.Arbeit)
        {
            if (a.level >= 20)
                arbeitBonus += a.bonus;
        }

        ArbeitBonus.text = "알바 20레벨 보너스: +" + arbeitBonus.ToString() + "%";

        double arbeitTreasure = 0;
        for (int i =0;i<gameData.ArbeitHappiness.Count;i++)
        {
            if (gameData.ArbeitHappiness[i].buy_status)
                arbeitTreasure += gameData.Arbeit[i].bonus;
        }

        ArbeitTreasureBonus.text = "알바의 보물 보너스: " + arbeitTreasure.ToString() + "%";

        double enterpriseBonus = 0;
        foreach(var e in gameData.Enterprise)
        {
            if (e.buy_status)
                enterpriseBonus+=e.bonus;
        }
        EnterpriseBonus.text = "기업 인수 보너스: +" + enterpriseBonus.ToString() + "%";

        double currPerClick = 0;
        currPerClick += arbeitBonus + arbeitTreasure + enterpriseBonus+100;
        PerClick.text += "->" + Utility.MoneyToString(userData.per_second*currPerClick/100);


    }
}
