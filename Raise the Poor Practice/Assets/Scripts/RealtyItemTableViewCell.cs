using UnityEngine;
using UnityEngine.UI;
using System.Numerics;
using System;

public class RealtyItemTableViewCell : TableViewCell<Realty>
{
    [SerializeField] private Text NameTxt;
    [SerializeField] private Text PriceTxt;
    [SerializeField] private Text CurrPriceTxt;
    [SerializeField] private Text BtnTxt;


    [Inject]
    UserData userData;

    InjectObj InjectObj = new InjectObj();

    Realty realty;

    DateTime startTime;
    DateTime endTime;

    
    public override void UpdateContent(Realty itemData)
    {
        realty = itemData;
        InjectObj.Inject(this);

        NameTxt.text = realty.name;

        BigInteger price = BigInteger.Parse(realty.price);
        BigInteger per_second = BigInteger.Parse(realty.per_second);
        
        if(!realty.buy_status)
        {
            BtnTxt.text = MoneyToString.MToS(price);
            PriceTxt.text = "";
            CurrPriceTxt.text = "";
        }
        else
        {
            endTime = DateTime.Now;
            BtnTxt.text = "판매하기";
            PriceTxt.text = "구매가: " + realty.price;
            realty.profit = (per_second * ((endTime - startTime).Seconds)).ToString();
            CurrPriceTxt.text = "현재가: "+ (price+per_second*((endTime-startTime).Seconds)).ToString();
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
        BigInteger per_second = BigInteger.Parse(realty.per_second);
        // 구입하기 전
        if(!realty.buy_status)
        {
            if (userData.my_money < per_second)
                return;
            realty.buy_status = true;
            startTime = DateTime.Now;
        }
        else
        {
            BigInteger price = BigInteger.Parse(realty.price);
            realty.buy_status = false;
            userData.my_money += (price + per_second * ((endTime - startTime).Seconds));
        }

    }
    private void Update()
    {
        UpdateContent(realty);
    }
}
