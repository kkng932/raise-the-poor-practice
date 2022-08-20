using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBuySystem : MonoBehaviour
{
    public PaintBuy PaintBuy;
    private void OnEnable()
    {
        EventBus.Subscribes<PaintStatusChangeEvent>(paintStatusChangeHandler);
    }

    private void paintStatusChangeHandler(PaintStatusChangeEvent obj)
    {
        PaintBuy.Show(obj.code, obj.status);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribes<PaintStatusChangeEvent>(paintStatusChangeHandler);
    }


}
