using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Info : MonoBehaviour
{
    public enum Directionality
    {
        left , 
        right
    }

    [SerializeField] float AzimuthAngle=0.0f;
    [SerializeField] float rotation = 0.0f;
    [SerializeField] public Directionality directionality;
    public static Info instance;
    public Vector3 new_direction;
    public void Awake()
    {

        if (instance == null) // if instance is not initilized then instance is equal to class
            instance = this;
        float dir = -1.0f;
        if ((float)directionality == 0)
            dir = 1.0f;
        float directionality1 = (float)directionality;
       // print(directionality1 * (90.0f - AzimuthAngle));
        Vector3 direction = -transform.forward;
        new_direction = Quaternion.Euler(rotation, dir * (90.0f - AzimuthAngle), 0) * direction;
     }

    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
}
