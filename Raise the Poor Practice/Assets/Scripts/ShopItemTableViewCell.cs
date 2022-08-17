using UnityEngine;
using UnityEngine.UI;
using System.Numerics;
public class GoldPerSecondChangeEvent
{
    public double gold;

    
    static GoldPerSecondChangeEvent _ev = new GoldPerSecondChangeEvent();
    public static void Publish(double gold)
    {
        _ev.gold = gold;
        EventBus.Publish(_ev);
    }
}

public class ShopItemTableViewCell : TableViewCell<Arbeit>
{
    [SerializeField] private Text DescTxt;
    [SerializeField] private Text ButtonTxt;

    [Inject]
    UserData userData;

    InjectObj InjectObj = new InjectObj();

    Arbeit arbeit;

    // ���� ������ �����ϴ� �޼��带 �������̵�
    public override void UpdateContent(Arbeit itemData)
    {
        arbeit = itemData;
        InjectObj.Inject(this);

        DescTxt.text = arbeit.name+" Lv"+arbeit.level.ToString();

        //BigInteger bPay = strToBI(arbeit.pay);
        //BigInteger cost = bPay + bPay / 100 * arbeit.level;
        //BigInteger bPerSecond = strToBI(arbeit.perSecond);
        //ButtonTxt.text = "���: " + cost.ToString()+"\n"+bPerSecond.ToString()+"/��";

        double cost = arbeit.pay + arbeit.pay / 100 * arbeit.level;
        ButtonTxt.text="���: "+cost.ToString()+"\n"+arbeit.perSecond.ToString()+"/��";
    }
    public BigInteger strToBI(string str)
    {
        // ���� ǥ����� ��
        if (str.Contains("E") || str.Contains("e"))
        {

            int fraction = int.Parse(str.Substring(str.IndexOf("+")));
            BigInteger bStr = BigInteger.Parse(str[0].ToString());
            for (int i = 0; i < fraction; i++)
                bStr *= 10;
            return bStr;
        }
        else
            return BigInteger.Parse(str);

    }
    public void BuyItem()
    {
        //BigInteger bPay = strToBI(arbeit.pay);
        //BigInteger cost = bPay + bPay/100 * arbeit.level;

        double cost = arbeit.pay + arbeit.pay / 100 * arbeit.level;

        if (userData.my_money < cost)
            return;

        userData.my_money -= cost;
       
        arbeit.level++;

        arbeit.perSecond = arbeit.perSecond * (arbeit.level + 1);
        userData.per_second += arbeit.perSecond;
        UpdateContent(arbeit);
        
        GoldPerSecondChangeEvent.Publish(userData.per_second);

        //  EventBus.Publish(new GoldChangeEvent() { gold= userData.gold});

    }
}
