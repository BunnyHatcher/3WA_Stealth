using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuspicionSlider : MonoBehaviour
{
    bool isHearing = false;
    bool isIncrementing = false;

    float sliderProgress = 5f;
    float suspicionTime = 5f;


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

    public void GuardTrigger_SliderEnable()
    {
        isHearing = true;
        isIncrementing = false;
        StopCoroutine(SliderDecrement());
    }

    public void GuardTrigger_SliderDisable()
    {
        StopCoroutine(SliderIncrement());
        StartCoroutine(SliderDecrement());
        isHearing = false;
        isIncrementing = false;
    }

    private void Update()
    {
        if (isHearing) SliderInit();
    }

    void SliderInit()
    {
        if (!isIncrementing) StartCoroutine(SliderIncrement());
    }

    IEnumerator SliderIncrement()
    {
        if (!isHearing) yield break;

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
