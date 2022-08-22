using UnityEngine;
using UnityEngine.UI;
using System.Numerics;


public class ViewChangeEvent
{
    public int code;
    public int level;
    static ViewChangeEvent _ev = new ViewChangeEvent();
    public static void Publish(int code, int level)
    {
        _ev.code = code;
        _ev.level = level;
        EventBus.Publish(_ev);
    }
}

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

// 알바 레벨 30 달성
public class ArbeitLvOver30Event
{
    public int code;

    static ArbeitLvOver30Event _ev = new ArbeitLvOver30Event();
    public static void Publish(int code)
    {
        _ev.code = code;
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

    // 셀의 내용을 갱신하는 메서드를 오버라이드
    public override void UpdateContent(Arbeit itemData)
    {
        arbeit = itemData;
        InjectObj.Inject(this);

        DescTxt.text = arbeit.name+" Lv"+arbeit.level.ToString();


        double cost = arbeit.pay + arbeit.pay / 100 * arbeit.level;
        ButtonTxt.text="비용: "+cost.ToString()+"\n"+(arbeit.perSecond*arbeit.level).ToString()+"/초";
    }
    public void BuyItem()
    {

        double cost = arbeit.pay + arbeit.pay / 100 * arbeit.level;

        if (userData.my_money < cost)
            return;

        userData.my_money -= cost;
       
        arbeit.level++;


        userData.per_second += arbeit.perSecond;
        UpdateContent(arbeit);
        
        GoldPerSecondChangeEvent.Publish(userData.per_second);
        ViewChangeEvent.Publish(arbeit.code, arbeit.level);

        if(arbeit.level>=30)
        {
            ArbeitLvOver30Event.Publish(arbeit.code);
        }

    }
}
