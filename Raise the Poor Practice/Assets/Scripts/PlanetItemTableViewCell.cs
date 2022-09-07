using UnityEngine;
using UnityEngine.UI;
using System.Numerics;
using System;
public class PlanetStatusChangeEvent
{
    public int code;
    public bool status;
    static PlanetStatusChangeEvent _ev = new PlanetStatusChangeEvent();
    public static void Publish(int code, bool status)
    {
        _ev.code = code;
        _ev.status = status;
        EventBus.Publish(_ev);
    }
}

public class PlanetItemTableViewCell : TableViewCell<Planet>
{
    [SerializeField] private Image iconImg;
    [SerializeField] private Text NameTxt;
    [SerializeField] private Text PriceTxt;
    [SerializeField] private Text CurrPriceTxt;
    [SerializeField] private Text BtnTxt;


    [Inject]
    UserData userData;

    InjectObj InjectObj = new InjectObj();

    Planet planet;

    DateTime startTime;
    DateTime endTime;

    private void OnEnable()
    {
        InjectObj.Inject(this);
    }
    public override void UpdateContent(Planet itemData)
    {
        planet = itemData;


        NameTxt.text = planet.name;

        iconImg.sprite = SpriteSheetManager.GetSpriteByName("planets", "planets_" + (itemData.code-1).ToString());

        if (!planet.buy_status)
        {
            BtnTxt.text = Utility.MoneyToString(planet.price);
            PriceTxt.text = "";
            CurrPriceTxt.text = "";
        }
        else
        {
            endTime = DateTime.Now;
            BtnTxt.text = "판매하기";
            PriceTxt.text = "구매가: " + planet.price;
            //planet.profit = planet.per_second * ((endTime - startTime).Seconds);
            CurrPriceTxt.text = "현재가: " + (planet.price + planet.per_second * ((endTime - startTime).Seconds)).ToString();
        }
    }
    public void BuyItem()
    {

        // 구입하기 전
        if (!planet.buy_status)
        {
            if (userData.my_money < planet.price)
                return;
            planet.buy_status = true;
            userData.my_money -= planet.price;
            startTime = DateTime.Now;
        }
        else
        {
            planet.buy_status = false;
            userData.my_money += (planet.price + planet.per_second * ((endTime - startTime).Seconds));
        }
        PlanetStatusChangeEvent.Publish(planet.code, planet.buy_status);


    }
    private void Update()
    {
        UpdateContent(planet);
    }
}
