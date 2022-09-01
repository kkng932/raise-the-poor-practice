using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostBar : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    private bool boost;
    


    void Start()
    {
        boost = false;

        slider.value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!boost)
        {
            if (Input.GetMouseButtonDown(0))
            {
                slider.value += 0.01f;
            }
            if (slider.value >= 1)
                boost = true;
        }
        else
        {


            //이벤트버스로 부스트 이벤트 만들기 

            slider.value -= 0.0001f;
            if (slider.value <= 0)
                boost = false;
        }
    }
}
