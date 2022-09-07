using UnityEngine;
using UnityEngine.UI;
using System.Numerics;
using System;
public class PaintStatusChangeEvent
{
    public int code;
    public bool status;
    static PaintStatusChangeEvent _ev = new PaintStatusChangeEvent();
    public static void Publish(int code, bool status)
    {
        _ev.code = code;
        _ev.status = status;
        EventBus.Publish(_ev);
    }
}

public class PaintItemTableViewCell : TableViewCell<Paint>
{
    [SerializeField] private Image iconImg;
    [SerializeField] private Text NameTxt;
    [SerializeField] private Text PriceTxt;
    [SerializeField] private Text CurrPriceTxt;
    [SerializeField] private Text BtnTxt;


    [Inject]
    UserData userData;

    InjectObj InjectObj = new InjectObj();

    Paint paint;

    DateTime startTime;
    DateTime endTime;

    private void OnEnable()
    {
        InjectObj.Inject(this);
    }
    public override void UpdateContent(Paint itemData)
    {
        paint = itemData;
        

        NameTxt.text = paint.name;

        iconImg.sprite = SpriteSheetManager.GetSpriteByName("paints", "paint" + itemData.code.ToString());

        if (!paint.buy_status)
        {
            BtnTxt.text = Utility.MoneyToString(paint.price);
            PriceTxt.text = "";
            CurrPriceTxt.text = "";
        }
        else
        {
            endTime = DateTime.Now;
            BtnTxt.text = "판매하기";
            PriceTxt.text = "구매가: " + Utility.MoneyToString(paint.price);
            paint.profit = paint.per_second * ((endTime - startTime).Seconds);
            CurrPriceTxt.text = "현재가: " + Utility.MoneyToString(paint.price + paint.per_second * ((endTime - startTime).Seconds));
        }
    }
    public void BuyItem()
    {

        // 구입하기 전
        if (!paint.buy_status)
        {
            if (userData.my_money < paint.price)
                return;
            paint.buy_status = true;
            userData.my_money -= paint.price;
            startTime = DateTime.Now;
        }
        else
        {
            paint.buy_status = false;
            userData.my_money += (paint.price + paint.per_second * ((endTime - startTime).Seconds));
        }
        PaintStatusChangeEvent.Publish(paint.code, paint.buy_status);


    }
    private void Update()
    {
        UpdateContent(paint);
    }
}
