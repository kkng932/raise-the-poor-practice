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
        DescTxt.text = "�������Lv" + userData.level.ToString();
        ButtonTxt.text = "���" + userData.levelup_cost.ToString()
            + "\n" + userData.click_money.ToString() + "/Ŭ��";
        ClickMoneyTxt.text = "�����/Ŭ��: " + userData.click_money.ToString() + "��";
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
