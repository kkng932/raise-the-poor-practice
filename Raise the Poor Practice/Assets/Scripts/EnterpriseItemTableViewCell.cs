using UnityEngine;
using UnityEngine.UI;
using System.Numerics;
using System;
public class EnterpriseStatusChangeEvent
{
    public int code;
    public bool status;
    static EnterpriseStatusChangeEvent _ev = new EnterpriseStatusChangeEvent();
    public static void Publish(int code, bool status)
    {
        _ev.code = code;
        _ev.status = status;
        EventBus.Publish(_ev);
    }
}

public class EnterpriseItemTableViewCell : TableViewCell<Enterprise>
{
    [SerializeField] private Image iconImg;
    [SerializeField] private Text NameTxt;
    [SerializeField] private Text PriceTxt;
    [SerializeField] private Text CurrPriceTxt;
    [SerializeField] private Text BtnTxt;


    [Inject]
    UserData userData;

    InjectObj InjectObj = new InjectObj();

    Enterprise enterprise;

    DateTime startTime;
    DateTime endTime;

    private void OnEnable()
    {
        InjectObj.Inject(this);
    }
    public override void UpdateContent(Enterprise itemData)
    {
        enterprise = itemData;


        NameTxt.text = enterprise.name;

        iconImg.sprite = SpriteSheetManager.GetSpriteByName("enterprise", "enterprise_" + (itemData.code-1).ToString());

        if (!enterprise.buy_status)
        {
            BtnTxt.text = Utility.MoneyToString(enterprise.price);
            PriceTxt.text = "";
            CurrPriceTxt.text = "";
        }
        else
        {
            endTime = DateTime.Now;
            BtnTxt.text = "판매하기";
            PriceTxt.text = "구매가: " + Utility.MoneyToString(enterprise.price);
            enterprise.profit = enterprise.per_second * ((endTime - startTime).Seconds);
            CurrPriceTxt.text = "현재가: " + Utility.MoneyToString(enterprise.price + enterprise.per_second * ((endTime - startTime).Seconds));
        }
    }
    public void BuyItem()
    {

        // 구입하기 전
        if (!enterprise.buy_status)
        {
            if (userData.my_money < enterprise.price)
                return;
            enterprise.buy_status = true;
            userData.my_money -= enterprise.price;
            startTime = DateTime.Now;
        }
        else
        {
            enterprise.buy_status = false;
            userData.my_money += (enterprise.price + enterprise.per_second * ((endTime - startTime).Seconds));
        }
        EnterpriseStatusChangeEvent.Publish(enterprise.code, enterprise.buy_status);


    }
    private void Update()
    {
        UpdateContent(enterprise);
    }
}
