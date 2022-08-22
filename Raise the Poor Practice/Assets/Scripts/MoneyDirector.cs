using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Numerics;
using System;

public class UserData
{
    public int level = 0;
    public double my_money = 0;
    public double click_money = 2;
    public double per_second = 2;
    public double property_status = 0;
    public double levelup_cost = 10;
}

public class MoneyDirector : MonoBehaviour
{
    GameObject myMoney;
    GameObject clickMoney;
    GameObject perSecond;
    GameObject propertyStatus;

    DateTime startTime;
    DateTime dTime;
    
    [Inject]
    UserData userData;
    InjectObj InjectObj = new InjectObj();


    void Start()
    {
        InjectObj.Inject(this);
        this.myMoney = GameObject.Find("My Money");
        this.clickMoney = GameObject.Find("Click Money");
        this.perSecond = GameObject.Find("Per Second");
        this.propertyStatus = GameObject.Find("Property Status");
        startTime = DateTime.Now;
        
    }
    void Update()
    {
        
        this.myMoney.GetComponent<Text>().text = Utility.MoneyToString(userData.my_money);
        this.propertyStatus.GetComponent<Text>().text = "�ڻ���Ȳ:" + Utility.MoneyToString(userData.property_status);
        this.clickMoney.GetComponent<Text>().text = Utility.MoneyToString(userData.click_money)+"/Ŭ��";
        this.perSecond.GetComponent<Text>().text = Utility.MoneyToString(userData.per_second) + "/��";

        // 1�ʴ� �����ϴ� ��
        dTime = DateTime.Now;
        int diffSecond = (dTime - startTime).Seconds;
        if (diffSecond >= 1)
        {
            startTime = DateTime.Now;
            userData.my_money += userData.per_second*diffSecond;
        }

        // Ŭ���� �����ϴ� ��
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            userData.my_money += userData.click_money;
        }

        // ����� ġƮŰ
        if (Input.GetKeyDown(KeyCode.Q))
        {
            userData.my_money += double.Parse("100000000000000000000");
        }
    }
}
