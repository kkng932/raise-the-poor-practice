using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Numerics;

public class MoneyDirector : MonoBehaviour
{
    GameObject myMoney;
    GameObject clickMoney;
    GameObject perSecond;
    GameObject propertyStatus;

    float span = 1.0f;
    float delta = 0;

    BigInteger my_money= 0;
    BigInteger click_money = 2;
    BigInteger per_second = 2;
    BigInteger property_status = 0;

    void Start()
    {
        this.myMoney = GameObject.Find("My Money");
        this.clickMoney = GameObject.Find("Click Money");
        this.perSecond = GameObject.Find("Per Second");
        this.propertyStatus = GameObject.Find("Property Status");
        
    }
    void Update()
    {
        this.myMoney.GetComponent<Text>().text = this.my_money.ToString()+ "��";
        this.propertyStatus.GetComponent<Text>().text = "�ڻ���Ȳ:" + this.property_status.ToString() + "��";


        // 1�ʴ� �����ϴ� ��
        this.delta += Time.deltaTime;
        if(this.delta>this.span)
        {
            this.delta = 0;
            this.my_money += this.per_second;
        }

        // Ŭ���� �����ϴ� ��
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            this.my_money += this.click_money;
        }
    }
}
