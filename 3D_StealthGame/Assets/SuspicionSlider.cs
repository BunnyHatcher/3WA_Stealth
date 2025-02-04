using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuspicionSlider : MonoBehaviour
{
    bool isHearing = false; public bool IsHearing
    { get 
        {
              return isHearing; 
        }
        set
        {
            isHearing = value;
            SliderTrigger(value);
        }
    }
    bool isSeeing = false; public bool IsSeeing
    {
        get
        {
            return IsSeeing;
        }
        set
        {
            IsSeeing = value;
            SliderTrigger(value);
        }
    }
    bool isIncrementing = false;

    float sliderProgress = 5f;
    float suspicionTime = 5f;
    float sliderIncrementationMultiplier = 2f;
    float sliderDecrementationMultiplier = 2f;

    public GuardClass guard;
   public Slider suspicionSlider;

    // 0 / 5 = 0
    // 1 / 5 = 0.2
    // 2 / 5 = 0.4
    // 3 / 5 = 0.6
    // 4 / 5 = 0.8
    // 5 / 5 = 1

    //Suspicion Slider Function

    private void Start()
    {
        suspicionTime = guard.guardStateMachine._suspicionTime;
    }

    void SliderTrigger(bool value)
    {          
       if (value)
          {
           SliderEnable();
          }

       else
          {
             if (!isHearing && !isSeeing) SliderDisable();
          }
    }


public void SliderEnable()
    {
        if (isHearing || isSeeing) return;

        isIncrementing = false;
        StopCoroutine(SliderDecrement());
    }

    public void SliderDisable()
    {
       
        StopCoroutine(SliderIncrement());
        StartCoroutine(SliderDecrement());
        isIncrementing = false;
    }

    private void Update()
    {
        if (isHearing || isSeeing) SliderInit();
    }

    void SliderInit()
    {
        if (!isIncrementing) StartCoroutine(SliderIncrement());
    }

    IEnumerator SliderIncrement()
    {
        if (!isHearing && !isSeeing) yield break;

        isIncrementing = true;
        suspicionSlider.transform.Find("Fill Area/Fill").GetComponent<Image>().color = Color.red;
        yield return new WaitForSeconds(0.01f);
        sliderProgress -= 0.01f;
        suspicionSlider.value = 1 - (sliderProgress / suspicionTime);

        if (sliderProgress >= 0.01f) 
        {
             StartCoroutine(SliderIncrement());
        }else
        {
            StartCoroutine(SliderIncrement());
        }
    }

    IEnumerator SliderDecrement()
    {
        yield return new WaitForSeconds(0.01f);
        sliderProgress += 0.01f;
        suspicionSlider.value = 1 - (sliderProgress / suspicionTime);

        if (sliderProgress <= 5f)
        {
            StartCoroutine(SliderDecrement());
        }
    }
}
