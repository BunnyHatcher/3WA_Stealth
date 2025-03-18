using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DetectionSlider : MonoBehaviour
{
    // Dans vision cone 
    public float seeingMultiplier = 2f;

    // Dans hearing sphere
    public float runningMultiplier = 2f;
    public float walkingMultiplier = 1f;
    public float sneakingMultiplier = 0.5f;

    public float idleMultiplier = -1f;


   
  



  

   
   public float currentMultiplier = 1f;
   public float decrementationMultiplier = 2f;

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

    public float _suspicionTimer = 1f;
    public float _detectionTimer = 1f;

    public GuardClass guard;
   public Slider detectionSlider;
    public PlayerStateMachine playerState;

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
        CurrentPlayerState(PlayerManager.instance.player._currentState);

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

        /*
        if (isHearing)
        {

            CurrentPlayerState(playerState.GetState());


        }
        */

        SliderInit();

        //isAlreadyIncrementing = true;
        StopCoroutine(SliderDecrement());
    }

    public void CurrentPlayerState(PlayerState ToState)
    {
        if (isSeeing || !isHearing)
        {
            return;
        }

            switch (playerState.GetState())
            {
                case PlayerState.IDLE:
                    currentMultiplier = idleMultiplier;
                    break;

                //add condition that allows incrementration only when moveSpeed in Animator is not 0
                case PlayerState.SNEAKING:
                    currentMultiplier = sneakingMultiplier;
                    break;

                case PlayerState.RUNNING:
                    currentMultiplier = runningMultiplier;
                    break;
                
                 default:
                    currentMultiplier = walkingMultiplier;
                    break;
            }

    }
    public void SliderDisable()
    {
       
        StopCoroutine(SliderIncrement());
        StartCoroutine(SliderDecrement()); // à abolir
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
        if (currentMultiplier == sneakingMultiplier) sliderImage.color = Color.yellow;
        if (currentMultiplier == walkingMultiplier) sliderImage.color = new Color(1, 0.5f, 0);
        if (currentMultiplier == runningMultiplier) sliderImage.color = Color.red;

        if (currentMultiplier == seeingMultiplier) sliderImage.color = Color.red;
       

        yield return new WaitForSeconds(0.01f);
        sliderProgress -= 0.01f * currentMultiplier;
        detectionSlider.value = 1 - (sliderProgress / suspicionTime);

        if (    sliderProgress >= 0.01f && detectionSlider.value < 1  // se repète si la gauge augmente et n'est pas maxé 
            ||  sliderProgress <= 0.01f && detectionSlider.value > 0) // respète si la gauge diminue et n'est pas vidé
        {
             StartCoroutine(SliderIncrement());
        }

        // manière alternative pour lignes en dessous

       /* bool isFullSlider = detectionSlider.value >= 1f;
        isFullSlider = detectionSlider.value == 0;

        guard.suspicionDetection._fullSlider = isFullSlider;
        guard.guardStateMachine.canWanderElsewhere = isFullSlider;*/

        if (detectionSlider.value >= 1f)
        {
            Debug.Log("Full  Slider1");
            guard.suspicionDetection._fullSlider = true;
            guard.guardStateMachine.canWander = true;
        }

        if (detectionSlider.value == 0)
        {
            Debug.Log("Full  Slider2");
            guard.suspicionDetection._fullSlider = false;
            guard.guardStateMachine.canWander = false;
        }
    }

   // Replace Slider Decrement with negative incrementation funtion   

    IEnumerator SliderDecrement()
    {
        yield return new WaitForSeconds(0.01f);

        if (guard.guardStateMachine._animator.GetBool("Chase")) yield break;

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
