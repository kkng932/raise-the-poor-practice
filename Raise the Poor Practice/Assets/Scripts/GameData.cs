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
    public double pay;
    public double levelUpCost;
    public int bonus;
    public int level;
    public double perSecond;
}

[Serializable]
public class Realty
{
    public int code;
    public string name;
    public double price;
    public double per_second;
    public bool buy_status;
    public double profit;

}
public class GameData : ScriptableObject
{
    public List<Arbeit> Arbeit = new List<Arbeit>();
    public List<Realty> Realty = new List<Realty>();
}
