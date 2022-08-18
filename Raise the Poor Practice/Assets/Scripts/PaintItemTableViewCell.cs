using UnityEngine;
using UnityEngine.UI;
using System.Numerics;
using System;

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


    public override void UpdateContent(Paint itemData)
    {
        paint = itemData;
        InjectObj.Inject(this);

        NameTxt.text = paint.name;

        iconImg.sprite = SpriteSheetManager.GetSpriteByName("paints", "paint" + itemData.code.ToString());

        if (!paint.buy_status)
        {
            BtnTxt.text = MoneyToString.MToS(paint.price);
            PriceTxt.text = "";
            CurrPriceTxt.text = "";
        }
        else
        {
            endTime = DateTime.Now;
            BtnTxt.text = "판매하기";
            PriceTxt.text = "구매가: " + paint.price;
            paint.profit = paint.per_second * ((endTime - startTime).Seconds);
            CurrPriceTxt.text = "현재가: " + (paint.price + paint.per_second * ((endTime - startTime).Seconds)).ToString();
        }
    }
    public BigInteger strToBI(string str)
    {
        // 지수 표기법일 때
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

        // 구입하기 전
        if (!paint.buy_status)
        {
            if (userData.my_money < paint.per_second)
                return;
            paint.buy_status = true;
            startTime = DateTime.Now;
        }
        else
        {

            paint.buy_status = false;
            userData.my_money += (paint.price + paint.per_second * ((endTime - startTime).Seconds));
        }

    }
    private void Update()
    {
        UpdateContent(paint);
    }
}
