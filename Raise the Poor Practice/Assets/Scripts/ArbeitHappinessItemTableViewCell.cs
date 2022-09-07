using UnityEngine;
using UnityEngine.UI;
using System.Numerics;
using System;
public class ArbeitHappinessStatusChangeEvent
{
    public int code;
    public bool status;
    static ArbeitHappinessStatusChangeEvent _ev = new ArbeitHappinessStatusChangeEvent();
    public static void Publish(int code, bool status)
    {
        _ev.code = code;
        _ev.status = status;
        EventBus.Publish(_ev);
    }
}

public class ArbeitHappinessItemTableViewCell : TableViewCell<ArbeitHappiness>
{
    [SerializeField] private Image iconImg;
    [SerializeField] private Text NameTxt;
    [SerializeField] private Text PriceTxt;
    [SerializeField] private Text CurrPriceTxt;
    [SerializeField] private Text BtnTxt;
    [SerializeField] private Button BuyBtn;


    [Inject]
    UserData userData;

    [Inject]
    GameData gameData;

    InjectObj InjectObj = new InjectObj();

    ArbeitHappiness arbeitHappiness;

    DateTime startTime;
    DateTime endTime;

    private void OnEnable()
    {
        InjectObj.Inject(this);
    }
    public override void UpdateContent(ArbeitHappiness itemData)
    {
        arbeitHappiness = itemData;
        
        // 알바의 행복 잠김
        if (gameData.Arbeit[arbeitHappiness.code - 1].level < 30)
        {
            string name = gameData.Arbeit[arbeitHappiness.code-1].name;
            BuyBtn.gameObject.SetActive(false);
            NameTxt.text = name + "의 행복 잠김\n조건: "+name+" 30레벨";
            iconImg.sprite = Resources.Load<Sprite>("lock") as Sprite;
        }
        else if (!arbeitHappiness.buy_status)
        {
            NameTxt.text = arbeitHappiness.name;

            iconImg.sprite = SpriteSheetManager.GetSpriteByName("fruits", "fruits_" + (itemData.code-1).ToString());
            BuyBtn.gameObject.SetActive(true);
            BtnTxt.text = Utility.MoneyToString(arbeitHappiness.price);
            PriceTxt.text = "";
            CurrPriceTxt.text = "";
        }
        else
        {
            iconImg.sprite = SpriteSheetManager.GetSpriteByName("fruits", "fruits_" + (itemData.code - 1).ToString());
            BuyBtn.gameObject.SetActive(true);
            endTime = DateTime.Now;
            BtnTxt.text = "판매하기";
            PriceTxt.text = "구매가: " + arbeitHappiness.price;
            arbeitHappiness.profit = arbeitHappiness.per_second * ((endTime - startTime).Seconds);
            CurrPriceTxt.text = "현재가: " + (arbeitHappiness.price + arbeitHappiness.per_second * ((endTime - startTime).Seconds)).ToString();
        }
    }
    public void BuyItem()
    {

        // 구입하기 전
        if (!arbeitHappiness.buy_status)
        {
            if (userData.my_money < arbeitHappiness.price)
                return;
            arbeitHappiness.buy_status = true;
            userData.my_money -= arbeitHappiness.price;
            startTime = DateTime.Now;
        }
        else
        {
            arbeitHappiness.buy_status = false;
            userData.my_money += (arbeitHappiness.price + arbeitHappiness.per_second * ((endTime - startTime).Seconds));
        }
        ArbeitHappinessStatusChangeEvent.Publish(arbeitHappiness.code, arbeitHappiness.buy_status);

    }
    private void Update()
    {
        UpdateContent(arbeitHappiness);
    }
    
}
