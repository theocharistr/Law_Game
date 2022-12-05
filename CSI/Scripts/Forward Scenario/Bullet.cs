using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject bullethole;
    // Start is called before the first frame update
    void Start()
    {
     }
    //void OnTriggerEnter(Collider col)
   // {
   //     print(col);
     //   Vector3 pos = transform.position;
      //  Vector3 angle = Vector3.zero;
     //   Wall wall = col.GetComponent<Wall>();//add floor;

     //   if (wall.side == WallSide.Left) { angle.y = 90; }
    //    else if (wall.side == WallSide.Right) { angle.y = -90; }
    //    else if (wall.side == WallSide.Floor) { angle.x = 90; }
     //   Quaternion rot = Quaternion.Euler(angle);
//Instantiate(bullethole, pos, rot);
      //  Destroy(gameObject);}
     
 

    void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        Quaternion rotation = Quaternion.LookRotation(contact.normal);
        Vector3 position = contact.point;
        Instantiate(bullethole, position, rotation);
        Destroy(gameObject);
    }
// Update is called once per frame
void Update()
    {
        
    }
}
