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

    // ���� ������ �����ϴ� �޼��带 �������̵�
    public override void UpdateContent(City itemData)
    {
        city = itemData;
        InjectObj.Inject(this);

        DescTxt.text = city.name + " Lv" + city.level.ToString();

        double cost = city.pay + city.pay / 100 * city.level;
        ButtonTxt.text = "���: " + Utility.MoneyToString(cost) + "\n" + Utility.MoneyToString(city.perSecond * city.level) + "/��";
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
