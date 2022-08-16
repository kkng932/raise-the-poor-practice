using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;


[Serializable]
public class Arbeit
{
    public int code;
    public string name;
    public string pay;
    public string levelUpCost;
    public int bonus;
    public int level;
    public string perSecond;
}

[Serializable]
public class Realty
{
    public int code;
    public string name;
    public string price;
    public string per_second;
    public bool buy_status;
    public string profit;

}
public class GameData : ScriptableObject
{
    public List<Arbeit> Arbeit = new List<Arbeit>();
    public List<Realty> Realty = new List<Realty>();
}
