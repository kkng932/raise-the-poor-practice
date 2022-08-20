using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoticeSystem : MonoBehaviour
{

    public UI_GoldPerSecChangeNotice UI_GoldPerSecChangeNotice;

    private void OnEnable()
    {
        EventBus.Subscribes<GoldPerSecondChangeEvent>(goldPerSecChangeHandler);
        
    }

    private void goldPerSecChangeHandler(GoldPerSecondChangeEvent obj)
    {
        UI_GoldPerSecChangeNotice.Show(obj.gold);

    }

    private void OnDisable()
    {
        EventBus.Unsubscribes<GoldPerSecondChangeEvent>(goldPerSecChangeHandler);
    }


}
