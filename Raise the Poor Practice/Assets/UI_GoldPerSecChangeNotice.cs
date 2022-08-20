using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class UI_GoldPerSecChangeNotice : MonoBehaviour
{

    public AnimationClip showClip,hideClip;
    public Animation anim;
    public double lastGoldPerSec;
    public UnityEngine.UI.Text goldTxt;

    
    float time=0;
    float goldUpdateDuration = 0.5f;
    float showDuration = 1;
    double startGoldPerSec,targetGoldPerSec;
    public void Show(double goldPerSec)
    {
        this.gameObject.SetActive(true);
        startGoldPerSec = lastGoldPerSec;
        targetGoldPerSec = goldPerSec;
        time = 0;
        goldTxt.text = "√ ¥Á ∞ÒµÂ»πµÊ∑Æ " + lastGoldPerSec;
        anim.clip = showClip;
        anim.Play();


        Debug.Log("Show " + lastGoldPerSec);
    }

    public void Update()
    {
        if (anim.isPlaying)
            return;
        time += Time.deltaTime;
        var t= Mathf.Clamp01(time / goldUpdateDuration);
        

        if(time> showDuration)
        {
            Hide();
        }

        var v= startGoldPerSec + (targetGoldPerSec - startGoldPerSec) * t;


        goldTxt.text = "√ ¥Á ∞ÒµÂ»πµÊ∑Æ " + Utility.MoneyToString(v);
    }


    public void Hide()
    {
        lastGoldPerSec = targetGoldPerSec;

        Debug.Log("Hide " + lastGoldPerSec);
        this.gameObject.SetActive(true);
        anim.clip = hideClip;
        anim.Play();
    }


    public void Done()
    {
        this.gameObject.SetActive(false);
    }

}
