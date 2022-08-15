using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Numerics;
using System;

public class UserData
{
    public int level = 0;
    public BigInteger my_money = 0;
    public BigInteger click_money = 2;
    public BigInteger per_second = 2;
    public BigInteger property_status = 0;
    public BigInteger levelup_cost = 10;
}
static public class MoneyToString
{
    static public string MToS(BigInteger money)
    {
        string str = "";
        BigInteger man, eok, jo, gyeong;
        //BigInteger hae;
        man = 10000;
        eok = 100000000;
        jo = 1000000000000;
        gyeong = 10000000000000000;
        //hae = 100000000000000000000;
        if (BigInteger.Compare(money, man) == -1)
        {
            str = money.ToString() + "원";
        }
        else if (BigInteger.Compare(money, eok) == -1)
        {
            string temp = money.ToString();
            str = temp.Substring(0, temp.Length - 4) + "만 " + temp.Substring(temp.Length - 4).TrimStart('0') + "원";
        }
        else if (BigInteger.Compare(money, jo) == -1)
        {
            string temp = money.ToString();
            string manValue = temp.Substring(temp.Length - 7, 4).TrimStart('0');
            str = temp.Substring(0, temp.Length - 7) + "억 ";
            if (manValue !="")
            {
                str += manValue + "만 ";
            }
            str += temp.Substring(temp.Length - 4).TrimStart('0') + "원";
            
            
        }
        else if (BigInteger.Compare(money, gyeong) == -1)
        {
            string temp = money.ToString();
            string eokValue = temp.Substring(temp.Length - 12, 4).TrimStart('0');
            str = temp.Substring(0, temp.Length - 12) + "조 ";
            if (eokValue != "")
            {
                str += temp.Substring(temp.Length - 12, 4).TrimStart('0') + "억 ";
            }
            str += temp.Substring(temp.Length - 4).TrimStart('0') + "원";
        }
        //else if (BigInteger.Compare(money, hae) == -1)
        //{
        //    string temp = money.ToString();
        //    str = temp.Substring(0, str.Length - 16) + "경 " + temp.Substring(str.Length - 16, 4) + "조 "
        //        + temp.Substring(str.Length - 12, 4) + "억 " + temp.Substring(str.Length - 8, 4) + "만 "
        //        + temp.Substring(str.Length - 4) + "원";
        //}
        return str;
        
    }
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
        
        this.myMoney.GetComponent<Text>().text = MoneyToString.MToS(userData.my_money);
        this.propertyStatus.GetComponent<Text>().text = "자산현황:" + MoneyToString.MToS(userData.property_status);
        this.clickMoney.GetComponent<Text>().text = MoneyToString.MToS(userData.click_money)+"/클릭";
        this.perSecond.GetComponent<Text>().text = MoneyToString.MToS(userData.per_second) + "/초";

        // 1초당 증가하는 돈
        dTime = DateTime.Now;
        int diffSecond = (dTime - startTime).Seconds;
        if (diffSecond >= 1)
        {
            startTime = DateTime.Now;
            userData.my_money += userData.per_second*diffSecond;
        }

        // 클릭시 증가하는 돈
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            userData.my_money += userData.click_money;
        }

        // 시험용 치트키
        if (Input.GetKeyDown(KeyCode.Q))
        {
            userData.my_money += 1000000000000;
        }
    }
}
