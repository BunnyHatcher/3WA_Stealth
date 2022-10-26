using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    private GameObject  PlayerRef;
    private Animator    _anim;

    public Material InCone;
    public Material OutCone;
    public Material Spoted;

    //ref du GameObject sur lequel envoyer un raycast quand on veut vérifier la vision
    public GameObject RaycastTarget;

    public LayerMask PlayerMask;

    private void Awake()
    {
        TryGetComponent<Animator>(out _anim);
    }

    private void Update()
    {
        if (PlayerRef)
        {
            //Vector3 toPlayer = (PlayerRef.transform.position - transform.position).normalized;
            //Debug.DrawRay(transform.position, toPlayer * 10f, Color.yellow, 0.0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {   
        if(other.CompareTag("Player") && !PlayerRef)
        {
            Debug.Log("Player is the cone");
            PlayerRef = RaycastTarget;
            GetComponent<MeshRenderer>().material = InCone;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //si on a une ref vers le joueur, on lui envoie un raycast
        if (PlayerRef) 
        {
            //on trouve la direction vers le joueur
            Vector3 toPlayer = (PlayerRef.transform.position - transform.position).normalized;
            Debug.DrawRay(transform.position, toPlayer * 10f, Color.yellow,0.0f);

            if (Physics.Raycast(transform.position, toPlayer, out RaycastHit hit, Mathf.Infinity ))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    //foe stopped
                    _anim.enabled = false;
                    Debug.DrawRay(transform.position, toPlayer * hit.distance, Color.red, 0.1f);
                    Debug.Log("Player in sight");
                    GetComponent<MeshRenderer>().material = Spoted;
                }
                else
                {
                    //foe goes back to routine
                    _anim.enabled = true;
                    GetComponent<MeshRenderer>().material = InCone;

                }
                
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && PlayerRef)
        {
            //ennemi goes back to routine
            _anim.enabled = true;
            Debug.Log("Player left the cone");
            PlayerRef = null;
            GetComponent<MeshRenderer>().material = OutCone;
        }
    }
}
