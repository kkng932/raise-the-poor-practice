using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Numerics;

[RequireComponent(typeof(ScrollRect))]
public class PlanetItemTableViewController : TableViewController<Planet>
{

    [SerializeField] private Text PurchaseTxt;
    [SerializeField] private Text CurrValueTxt;
    [SerializeField] private Text ProfitTxt;

    [Inject]
    GameData gameData;
    InjectObj injectObj = new InjectObj();

    private void LoadData()
    {
        injectObj.Inject(this);
        tableData = gameData.Planet;

        UpdateContents();
    }

    protected override void Awake()
    {
        base.Awake();
        SpriteSheetManager.Load("planets");
    }
    protected override void Start()
    {
        base.Start();
        LoadData();
    }
    private void Update()
    {
        double purchase = 0;
        double profit = 0;
        foreach (var t in tableData)
        {

            if (t.buy_status)
            {
                purchase += t.price;
                //profit += t.profit;
            }

        }
        PurchaseTxt.text = "미술품 총 구매가: " + Utility.MoneyToString(purchase);
        CurrValueTxt.text = "현재 총 가치: " + Utility.MoneyToString(purchase + profit);
        ProfitTxt.text = "이익: " + Utility.MoneyToString(profit);
    }

}
