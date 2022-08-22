using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArbeitHappinessSystem : MonoBehaviour
{

    public ArbeitHappinessItemTableViewCell ArbeitHappinessItemTableViewCell;
    private void OnEnable()
    {
        Debug.Log("ArbeitHappinessSystem.OnEnable");
        EventBus.Subscribes<ArbeitLvOver30Event>(arbeitLvOver30Handler);
    }

    private void arbeitLvOver30Handler(ArbeitLvOver30Event obj)
    {
        Debug.Log("arbeitLvOver30Handler");
        ArbeitHappinessItemTableViewCell.Show(obj.code);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribes<ArbeitLvOver30Event>(arbeitLvOver30Handler);
    }
}
