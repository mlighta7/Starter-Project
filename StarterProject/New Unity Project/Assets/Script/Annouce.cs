using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Annouce : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioSource annouce =gameObject.GetComponent<AudioSource>();
        Destroy(annouce, 2);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
