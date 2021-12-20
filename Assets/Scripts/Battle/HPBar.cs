using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    [SerializeField] GameObject health;

   

    public void SetHP(float hpNormalized)
    {
        //Function to change the health bar during a battle
        health.transform.localScale = new Vector3(hpNormalized, 1f);
    }
}

