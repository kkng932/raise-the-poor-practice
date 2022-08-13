using UnityEngine;
using UnityEngine.UI;
using System.Numerics;
public class GoldChangeEvent
{
    public BigInteger gold;

    static GoldChangeEvent _ev = new GoldChangeEvent();
    public static void Publish(BigInteger gold)
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
    // 셀의 내용을 갱신하는 메서드를 오버라이드
    public override void UpdateContent(Arbeit itemData)
    {
        arbeit = itemData;
        InjectObj.Inject(this);
        DescTxt.text = itemData.name+" Lv"+itemData.level.ToString();
        ButtonTxt.text = "비용: " + (itemData.pay + itemData.levelUpCost * itemData.level).ToString();
    }

    public void BuyItem()
    {
        if (userData.my_money < arbeit.pay)
            return;

        userData.my_money -= arbeit.pay;

        GoldChangeEvent.Publish(userData.my_money);

        //  EventBus.Publish(new GoldChangeEvent() { gold= userData.gold});

    }
}
