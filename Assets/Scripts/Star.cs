using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    public float maxLifeTime = 10f;
    void Start()
    {
        Destroy(gameObject, maxLifeTime);
    }

    
}
