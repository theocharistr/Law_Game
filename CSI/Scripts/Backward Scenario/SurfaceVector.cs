using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SurfaceVector : MonoBehaviour
{
    public enum Directionality
    {
        left , 
        right
    }

    [SerializeField] float AzimuthAngle=0.0f;
    [SerializeField] float rotation = 0.0f;
    [SerializeField] private Directionality directionality;
    [SerializeField] private Transform transformExtraBulletHole;
    public GameObject ConePrefab;
   // public GameObject Shooter;
    Vector3 intersection;
    float threshold=1f;//increase this number to increase intersection threshold
    
    // Start is called before the first frame update
    void Start()
    {
        
        float dir = -1.0f;
        if ((float)directionality == 0)
            dir = 1.0f;

        float lineLength = 5;
        float directionality1 =(float) directionality;
        //print(directionality1 * (90.0f - AzimuthAngle));
        Vector3 direction = -transform.forward;

        direction = Quaternion.Euler(rotation, dir*(90.0f - AzimuthAngle), 0) * direction;
       // Debug.DrawLine(transform.position, transform.position + direction* lineLength, Color.yellow, 1000, false);


        
        Vector3 a1 = transform.position;
        Vector3 a2 = transform.position + direction * lineLength;
        Vector3 b1 = transformExtraBulletHole.position;
        Vector3 b2 = transformExtraBulletHole.position + Info.instance.new_direction* lineLength;
        GameObject Cone = Instantiate(ConePrefab);


        Debug.DrawLine(a1, a2, Color.yellow, 1000, false);
        Debug.DrawLine(b1, b2, Color.yellow, 1000, false);

        Cone.transform.position = a1;
        Cone.transform.localScale = new Vector3(1f, 1f, lineLength);
        //Cone.transform.rotation = Quaternion.LookRotation(a2);
        // Cone.transform.LookAt(2);
        Cone.transform.rotation = Quaternion.LookRotation(-direction);

        Cone = Instantiate(ConePrefab);

        Cone.transform.position = b1;
        Cone.transform.localScale = new Vector3(1f, 1f, lineLength);
        Cone.transform.rotation = Quaternion.LookRotation(-Info.instance.new_direction);


        //print("a1" + a1.ToString("F4") + "a2" + a2 + "b1" + b1 + "b2" + b2);
        Vector3 aDiff = a2 - a1;
        Vector3 bDiff = b2 - b1;
        if (SurfaceVector.LineLineIntersection(out intersection, a1, aDiff, b1, bDiff))
        {
            float aSqrMagnitude = aDiff.sqrMagnitude;
            float bSqrMagnitude = bDiff.sqrMagnitude;
            //print("ee");
            if ((intersection - a1).sqrMagnitude <= aSqrMagnitude
                 && (intersection - a2).sqrMagnitude <= aSqrMagnitude
                 && (intersection - b1).sqrMagnitude <= bSqrMagnitude
                 && (intersection - b2).sqrMagnitude <= bSqrMagnitude)
            {
                // there is an intersection between the two segments and 
                //   it is at intersection
                print(intersection + "Intersection");
                 
                //GameObject ShooterPosition = Instantiate(Shooter, intersection, Quaternion.identity);
             }
        } 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static bool LineLineIntersection(out Vector3 intersection, Vector3 linePoint1,
        Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2)
    {

        Vector3 lineVec3 = linePoint2 - linePoint1;
        Vector3 crossVec1and2 = Vector3.Cross(lineVec1, lineVec2);
        Vector3 crossVec3and2 = Vector3.Cross(lineVec3, lineVec2);

        float planarFactor = Vector3.Dot(lineVec3, crossVec1and2);

        //is coplanar, and not parallel
        if (Mathf.Abs(planarFactor) < threshold 
                && crossVec1and2.sqrMagnitude > 0.0001f)
        {
            float s = Vector3.Dot(crossVec3and2, crossVec1and2)
                    / crossVec1and2.sqrMagnitude;
            intersection = linePoint1 + (lineVec1 * s);
            return true;
        }
        else
        {
            intersection = Vector3.zero;
            return false;
        }
    }
}
