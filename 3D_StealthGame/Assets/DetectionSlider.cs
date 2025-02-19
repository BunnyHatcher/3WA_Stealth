using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetectionSlider : MonoBehaviour
{
    float hearingMultiplier = 1f;
    float seeingMultiplier = 2f;
    float currentMultiplier = 1f;
    float decrementationMultiplier = 2f;

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
            return isSeeing;
        }
        set
        {
            isSeeing = value;
            SliderTrigger(value);
        }
    }
    bool isAlreadyIncrementing = false;

    float sliderProgress = 5f;
    float suspicionTime = 5f;
    float sliderIncrementationMultiplier = 2f;
    float sliderDecrementationMultiplier = 2f;

    
    //Detection Bools
    public bool _investigatingDetection = false;
    public bool _fullDetection = false;
    public bool _suspicionDetection = false;

    public float _suspicionTimer = 1f;
    public float _detectionTimer = 1f;

    public GuardClass guard;
   public Slider detectionSlider;

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
        if (isSeeing)
        {

        currentMultiplier = seeingMultiplier;

        }


        if (isHearing)
        {
         currentMultiplier = hearingMultiplier;
        }

        SliderInit();

        //isAlreadyIncrementing = true;
        StopCoroutine(SliderDecrement());
    }

    public void SliderDisable()
    {
       
        StopCoroutine(SliderIncrement());
        StartCoroutine(SliderDecrement());
        isAlreadyIncrementing = false;
    }

    private void Update()
    {
       // if (isHearing || isSeeing) SliderInit();
    }

    void SliderInit()
    {
        if (!isAlreadyIncrementing) StartCoroutine(SliderIncrement());
    }

    IEnumerator SliderIncrement()
    {
        if (!isHearing && !isSeeing) yield break;

        isAlreadyIncrementing = true;

        Image sliderImage = detectionSlider.transform.Find("Fill Area/Fill").GetComponent<Image>();
        if (currentMultiplier == seeingMultiplier) sliderImage.color = Color.red;
        if (currentMultiplier == hearingMultiplier) sliderImage.color = Color.yellow;

        yield return new WaitForSeconds(0.01f);
        sliderProgress -= 0.01f * currentMultiplier;
        detectionSlider.value = 1 - (sliderProgress / suspicionTime);

        if (sliderProgress >= 0.01f) 
        {
             StartCoroutine(SliderIncrement());
        }
        else
        {
            guard.suspicionDetection._investigatingDetection = true;
        }
    }

    IEnumerator SliderDecrement()
    {
        yield return new WaitForSeconds(0.01f);

        Image sliderImage = detectionSlider.transform.Find("Fill Area/Fill").GetComponent<Image>();
        sliderImage.color = Color.green;

        sliderProgress += 0.01f * decrementationMultiplier;
        detectionSlider.value = 1 - (sliderProgress / suspicionTime);

        if (sliderProgress <= 5f)
        {
            StartCoroutine(SliderDecrement());
        }
    }
}
