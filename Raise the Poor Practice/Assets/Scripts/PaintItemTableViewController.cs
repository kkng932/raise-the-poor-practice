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

        foreach (var t in tableData)
        {
            double purchase = 0;
            double profit = 0;
            if (t.buy_status)
            {
                purchase += t.price;
                profit += t.profit;
            }
            PurchaseTxt.text = "�̼�ǰ �� ���Ű�: " + purchase.ToString();
            CurrValueTxt.text = "���� �� ��ġ: " + (purchase + profit).ToString();
            ProfitTxt.text = "����: " + profit.ToString();
        }
    }

}
