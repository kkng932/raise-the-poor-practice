using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Numerics;
using System;

public class UserData
{
    public int level=0;
    public double my_money = 0;
    public double click_money = 2;
    public double per_second = 2;
    public double property = 0;
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
    [Inject]
    GameData gameData;

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
        this.propertyStatus.GetComponent<Text>().text = "자산현황:" + Utility.MoneyToString(userData.property);
        this.clickMoney.GetComponent<Text>().text = Utility.MoneyToString(userData.click_money) + "/클릭";
        this.perSecond.GetComponent<Text>().text = Utility.MoneyToString(userData.per_second) + "/초";

        // 1초당 증가하는 돈
        dTime = DateTime.Now;
        int diffSecond = (dTime - startTime).Seconds;
        if (diffSecond >= 1)
        {
            startTime = DateTime.Now;
            userData.my_money += userData.per_second * diffSecond;
        }

        // 클릭시 증가하는 돈
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            userData.my_money += userData.click_money;
        }

        // 시험용 치트키
        if (Input.GetKeyDown(KeyCode.Q))
        {
            userData.my_money += double.Parse("100000000000000000000");
        }
        CalcProperty();

    }

    private void CalcProperty()
    {
        double realtyProperty = 0;
        foreach (var i in gameData.Realty)
        {
            if (i.buy_status)
                realtyProperty += i.profit + i.price;
        }
        double paintProperty = 0;
        foreach (var i in gameData.Paint)
        {
            if (i.buy_status)
                paintProperty += i.profit + i.price;
        }
        double arbeitHProperty = 0;
        foreach (var i in gameData.ArbeitHappiness)
        {
            if (i.buy_status)
                arbeitHProperty += i.profit + i.price;
        }
        double enterpriseProperty = 0;
        foreach (var i in gameData.Enterprise)
        {
            if (i.buy_status)
                enterpriseProperty += i.profit + i.price;
        }
        userData.property = realtyProperty + paintProperty + arbeitHProperty + enterpriseProperty;
    }
}
