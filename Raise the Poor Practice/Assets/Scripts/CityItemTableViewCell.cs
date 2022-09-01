using UnityEngine;
using UnityEngine.UI;
using System.Numerics;




public class CityItemTableViewCell : TableViewCell<City>
{
    [SerializeField] private Text DescTxt;
    [SerializeField] private Text ButtonTxt;

    [Inject]
    UserData userData;

    InjectObj InjectObj = new InjectObj();

    City city;

    // 셀의 내용을 갱신하는 메서드를 오버라이드
    public override void UpdateContent(City itemData)
    {
        city = itemData;
        InjectObj.Inject(this);

        DescTxt.text = city.name + " Lv" + city.level.ToString();

        double cost = city.pay + city.pay / 100 * city.level;
        ButtonTxt.text = "비용: " + Utility.MoneyToString(cost) + "\n" + Utility.MoneyToString(city.perSecond * city.level) + "/초";
    }

    public void BuyItem()
    {

        double cost = city.pay + city.pay / 100 * city.level;

        if (userData.my_money < cost)
            return;

        userData.my_money -= cost;

        city.level++;


        userData.per_second += city.perSecond;
        UpdateContent(city);

        

    }
}
