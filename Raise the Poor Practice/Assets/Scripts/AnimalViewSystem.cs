using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalViewSystem : MonoBehaviour
{

    public AnimalView AnimalView;
    private void OnEnable()
    {
        EventBus.Subscribes<ViewChangeEvent>(viewChangeHandler);

    }

    private void viewChangeHandler(ViewChangeEvent obj)
    {
        AnimalView.Show(obj.code, obj.level);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribes<ViewChangeEvent>(viewChangeHandler);
    }


}
