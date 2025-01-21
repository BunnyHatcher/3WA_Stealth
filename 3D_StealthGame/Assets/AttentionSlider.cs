using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttentionSlider : MonoBehaviour
{
    bool isHearing = false;
    bool isIncrementing = false;

    float attentionCountdown = 5f;
    float suspicionTime = 5f;

   public GuardClass guard;
   public Slider attentionSlider;

    // 0 / 5 = 0
    // 1 / 5 = 0.2
    // 2 / 5 = 0.4
    // 3 / 5 = 0.6
    // 4 / 5 = 0.8
    // 5 / 5 = 1

    //Attention Slider Function

    private void Start()
    {
        suspicionTime = guard.guardStateMachine._suspicionTime;
        attentionCountdown = suspicionTime;
    }

    private void Update()
    {
        if (isHearing) SliderInit();
    }

    public void SliderEnable()
    {
        isHearing = true;
        isIncrementing = true;
    }

    public void SliderDisable()
    {
        isHearing = false;
        isIncrementing = false;
        StopCoroutine(SliderIncrement());
    }

    void SliderInit()
    {
        if (!isIncrementing) StartCoroutine(SliderIncrement());
    }

    IEnumerator SliderIncrement()
    {
        isIncrementing = true;
        yield return new WaitForSeconds(0.01f);
        attentionCountdown -= 0.01f;
        attentionSlider.value = 1 - (attentionCountdown / suspicionTime);

        if (attentionCountdown >= 0.01f) 
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
        attentionCountdown += 0.01f;
        attentionSlider.value = 1 - (attentionCountdown / suspicionTime);

        if (attentionCountdown <= 5f)
        {
            StartCoroutine(SliderDecrement());
        }
    }
}
