using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoorPower : MonoBehaviour
{
    [SerializeField] private Text DescTxt;
    [SerializeField] private Text ButtonTxt;
    [SerializeField] private Text ClickMoneyTxt;

    [Inject]
    UserData userData;

    InjectObj InjectObj = new InjectObj();

    
    
    private void Start()
    {
        InjectObj.Inject(this);

        UpdateContent();
        
    }
    private void UpdateContent()
    {
        DescTxt.text = "사장거지Lv" + userData.level.ToString();
        ButtonTxt.text = "비용" + Utility.MoneyToString(userData.levelup_cost)
            + "\n" + Utility.MoneyToString(userData.click_money) + "/클릭";
        ClickMoneyTxt.text = "현재원/클릭: " + Utility.MoneyToString(userData.click_money) + "원";
    }


    public void levelUp()
    {
        if (userData.my_money < userData.levelup_cost)
            return;
        userData.my_money -= userData.levelup_cost;
        userData.levelup_cost *= 1.2;
        userData.click_money += 2*userData.level;
        userData.level++;
        UpdateContent();
    }
}
