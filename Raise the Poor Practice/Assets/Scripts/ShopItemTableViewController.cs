using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(ScrollRect))]
public class ShopItemTableViewController : TableViewController<Arbeit>
{
    [Inject]
    GameData gameData;
    InjectObj injectObj = new InjectObj();

    private void LoadData()
    {
        injectObj.Inject(this);
        //tableData = new List<Item>();
        tableData = gameData.Arbeit;
        foreach(var g in gameData.Arbeit)
        {
            Debug.Log(g.name);
        }

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

}
