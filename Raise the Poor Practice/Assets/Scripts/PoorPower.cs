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
        ButtonTxt.text = "비용" + userData.levelup_cost.ToString()
            + "\n" + userData.click_money.ToString() + "/클릭";
        ClickMoneyTxt.text = "현재원/클릭: " + userData.click_money.ToString() + "원";
    }


    public void levelUp()
    {
        if (userData.my_money < userData.levelup_cost)
            return;
        userData.my_money -= userData.levelup_cost;
        userData.levelup_cost += userData.level * userData.levelup_cost/10;
        userData.click_money += 2;
        userData.level++;
        UpdateContent();
    }
}
