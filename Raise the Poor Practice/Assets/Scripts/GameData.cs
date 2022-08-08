using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Arbeit
{
    public int code;
    public string name;
    public ulong pay;
    public int bonus;
}

public class GameData : ScriptableObject
{
    public List<Arbeit> Arbeit = new List<Arbeit>();
}
