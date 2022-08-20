using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Numerics;



[RequireComponent(typeof(ScrollRect))]
public class PaintItemTableViewController : TableViewController<Paint>
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
        tableData = gameData.Paint;

        UpdateContents();
    }

    protected override void Awake()
    {
        base.Awake();
        SpriteSheetManager.Load("paints");
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
                profit += t.profit;
            }
            
        }
        PurchaseTxt.text = "�̼�ǰ �� ���Ű�: " + Utility.MoneyToString(purchase);
        CurrValueTxt.text = "���� �� ��ġ: " + Utility.MoneyToString(purchase + profit);
        ProfitTxt.text = "����: " + Utility.MoneyToString(profit);    
    }

}
