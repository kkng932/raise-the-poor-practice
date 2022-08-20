using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

static public class Utility 
{
    static public string MoneyToString(double money)
    {
        money = Math.Truncate(money);

        string str = "";
        double man, eok, jo, gyeong, hae;
        man = 10000;
        eok = 100000000;
        jo = 1000000000000;
        gyeong = 10000000000000000;
        hae = double.Parse("100000000000000000000");
        if (money < man)
        {
            str = money.ToString() + "원";
        }
        else if (money < eok)
        {
            string temp = money.ToString();
            str = temp.Substring(0, temp.Length - 4) + "만 " + temp.Substring(temp.Length - 4).TrimStart('0') + "원";
        }
        else if (money < jo)
        {
            string temp = money.ToString();
            
            str = temp.Substring(0, temp.Length - 8) + "억 ";
            string manValue = temp.Substring(temp.Length - 7, 4).TrimStart('0');
            if (manValue != "")
            {
                str += manValue + "만 ";
            }
            str += temp.Substring(temp.Length - 4).TrimStart('0') + "원";


        }
        else if (money < gyeong)
        {
            string temp = money.ToString();
            
            str = temp.Substring(0, temp.Length - 12) + "조 ";
            string eokValue = temp.Substring(temp.Length - 12, 4).TrimStart('0');
            if (eokValue != "")
            {
                str += eokValue+ "억 ";
            }
            string manValue = temp.Substring(temp.Length - 8, 4).TrimStart('0');
            if(manValue!="")
            {
                str += manValue + "만 ";
            }
            str += "원";
        }
        else if (money <hae)
        {
            string temp = money.ToString();
            
            str = temp.Substring(0, str.Length - 16) + "경 ";
            string joValue = temp.Substring(str.Length - 16, 4).TrimStart('0');
            if (joValue !="")
            {
                str += joValue + "조";
            }
            string eokValue = temp.Substring(str.Length - 12, 4).TrimStart('0');
            if ( eokValue !="")
            {
                str += eokValue + "억";
            }
            str += "원";
        }
        return str;

    }
}
