using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Numerics;



[RequireComponent(typeof(ScrollRect))]
public class RealtyItemTableViewController : TableViewController<Realty>
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
        tableData = gameData.Realty;

        UpdateContents();
    }

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
        LoadData();
    }
    private void Update()
    {
        
        foreach(var t in tableData)
        {
            double purchase = 0;
            double profit = 0;
            if(t.buy_status)
            {
                purchase += t.price;
                profit += t.profit;
            }
            PurchaseTxt.text = "�ε��� �� ���Ű�: " + purchase.ToString();
            CurrValueTxt.text = "���� �� ��ġ: " + (purchase + profit).ToString();
            ProfitTxt.text = "����: " + profit.ToString();
        }
    }

}
