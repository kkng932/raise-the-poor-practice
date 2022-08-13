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
    public BigInteger pay;
    public int bonus;
    public int level;
    public BigInteger levelUpCost;
}

public class GameData : ScriptableObject
{
    public List<Arbeit> Arbeit = new List<Arbeit>();
}
