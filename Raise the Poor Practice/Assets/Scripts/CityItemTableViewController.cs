using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



[RequireComponent(typeof(ScrollRect))]
public class CityItemTableViewController : TableViewController<City>
{
    [Inject]
    GameData gameData;
    InjectObj injectObj = new InjectObj();

    private void LoadData()
    {
        injectObj.Inject(this);
        tableData = gameData.City;

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
