using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;


public class ShootingR : MonoBehaviour
{
    Vector3 intersection;
    public GameObject ConePrefab;
    //public Rigidbody projectile;
    public GameObject projectile;
    public Vector3 intersectPoint;
    public Vector3 BulletHoleVector;
    public GameObject cursor;
    public Transform shootPoint;
    public LayerMask layer;
    public LineRenderer lineVisual;
    public int lineSegment = 10;
    public float flightTime = 1f;
    [SerializeField] private GameObject _bulletHolePrefab;
    int K = 0;

    List<Vector3> startVector = new List<Vector3>();
    List<Vector3> endVector = new List<Vector3>();

    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Camera 2").GetComponent<Camera>();
         //cam = Camera.main;
        lineVisual.positionCount = lineSegment + 1;
    }

    // Update is called once per frame
    void Update()
    {
        LaunchProjectile();
    }

    void LaunchProjectile()
    {
        Ray camRay = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(camRay, out hit, 100f, layer))
        {

            cursor.SetActive(true);
            cursor.transform.position = hit.point;
            Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;

            Vector3 vo = CalculateVelocity(hit.point, shootPoint.position, flightTime);
            Visualize(vo, cursor.transform.position); //we include the cursor position as the final nodes for the line visual position

            transform.rotation = Quaternion.LookRotation(vo);

            if (Input.GetMouseButtonDown(0))
            {

                Vector3 directionShooter = hit.point - shootPoint.position;
                float LineLength = 4f;
                //Debug.DrawLine(hit.point, hit.point - (directionShooter.normalized * LineLength), Color.green, 1000, false);
                GameObject Cone = Instantiate(ConePrefab);

                Cone.transform.position = hit.point;
                Cone.transform.localScale = new Vector3(1f, 1f, LineLength);
                Cone.transform.rotation = Quaternion.LookRotation(directionShooter.normalized);

                Debug.DrawLine(hit.point, hit.point - (directionShooter.normalized * LineLength), Color.green, 1000, false);
                // Debug.DrawLine(hit.point, shootPoint.position, Color.yellow, 1000, false); 
             
                K=K+1;
 
                startVector.Add((hit.point));
               // endVector.Add(GameObject.Find("Glock G22").transform.position);
                endVector.Add((hit.point - (directionShooter.normalized * LineLength)));
                GameObject obj = Instantiate(projectile, shootPoint.position, Quaternion.identity);
                obj.GetComponent<Rigidbody>().velocity = vo;
                //print(startVector);
               // print(K);

/*
                Transform transform1 = GameObject.Find("65").transform;
                 Vector3 direction = -transform1.forward;
                 direction = Quaternion.Euler(0,0, 0) * direction;
                Debug.DrawLine(transform1.position, transform1.position + direction , Color.yellow, 1000, false);


                transform2.Rotate(0, 50, 0, Space.World);
                Debug.DrawLine(transform2.position, transform2.position + -transform2.forward, Color.yellow, 1000, false);
                transform2.Rotate(0, -100, 0, Space.World);
                Debug.DrawLine(transform2.position, transform2.position + -transform2.forward, Color.green, 1000, false);
               
               Transform transform1 = GameObject.Find("Cartridge_Case_Damaged_Caliber_9mm").transform;
               Transform transform2 = GameObject.Find("Glock G22").transform;
               Transform BulletHoleTransform2 = GameObject.Find("BulletHoleExample").transform;
               Transform SurfaceVector = GameObject.Find("Surface Vector").transform;


               float lineLength = 5;
               Vector3 lineStart = BulletHoleTransform2.position;
               Vector3 lineStart1 = SurfaceVector.position;

               SurfaceVector.Rotate(0, -15, 0, Space.World);
               Vector3 lineEnd = BulletHoleTransform2.position + (lineLength * BulletHoleTransform2.forward);
               Vector3 lineEnd1 = SurfaceVector.position + (lineLength * SurfaceVector.right);

               Debug.DrawLine(lineStart, lineEnd, Color.red, 1000, false);
               Debug.DrawLine(lineStart1, lineEnd1, Color.blue, 1000, false);*/

                //print("Distance between Catridges and Glock G22 is :"+ Vector3.Distance(transform1.position, transform2.position ));
                if (K == 2)
                {
                    print("In");
                    /*startVector[0] = new Vector3(6, 8, 4);
                    endVector[0] = new Vector3(12, 15, 4);
                    startVector[1] = new Vector3(6, 8, 2);
                    endVector[1] = new Vector3(12, 15, 6);
                    /* startVector[0] = new Vector3(3, 4, 7);
                      endVector[0] = new Vector3(1, 1, 1);
                      startVector[1] = new Vector3(1, 1, 1);
                      endVector[1] = new Vector3(6, -3, 4);
                     /* print(LineLineIntersection(out intersection, startVector[0],
                   endVector[0], startVector[1], endVector[1]));*/

                    Vector3 a1 = startVector[0];
                    Vector3 a2 = endVector[0];
                    Vector3 b1 = startVector[1];
                    Vector3 b2 = endVector[1];
                    print("a1" + a1.ToString("F4") + "a2" + a2 + "b1" + b1 + "b2" + b2);
                    Vector3 aDiff = a2 - a1;
                    Vector3 bDiff = b2 - b1;
                     if (LineLineIntersection(out intersection, a1, aDiff, b1, bDiff))
                    {
                        float aSqrMagnitude = aDiff.sqrMagnitude;
                        float bSqrMagnitude = bDiff.sqrMagnitude;
                        print("ee");
                        if ((intersection - a1).sqrMagnitude <= aSqrMagnitude
                             && (intersection - a2).sqrMagnitude <= aSqrMagnitude
                             && (intersection - b1).sqrMagnitude <= bSqrMagnitude
                             && (intersection - b2).sqrMagnitude <= bSqrMagnitude)
                        {
                            // there is an intersection between the two segments and 
                            //   it is at intersection
                            print(intersection + "Intersection");
                        }
                    }
                } 


            }
        }
    } 

    
    //added final position argument to draw the last line node to the actual target
      void Visualize(Vector3 vo, Vector3 finalPos)
     {
        for (int i = 0; i < lineSegment; i++)
       {
           Vector3 pos = CalculatePosInTime(vo, (i / (float)lineSegment) * flightTime);
           lineVisual.SetPosition(i, pos);
       }

        lineVisual.SetPosition(lineSegment, finalPos);
    }

 
    Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float time)
    {
        Vector3 distance = target - origin;
        Vector3 distanceXz = distance;
        distanceXz.y = 0f;

        float sY = distance.y;
        float sXz = distanceXz.magnitude;

        float Vxz = sXz / time;
        // float Vy = (sY / time) + (0.5f * Mathf.Abs(Physics.gravity.y) * time);
        float Vy = sY / time;

        Vector3 result = distanceXz.normalized;
        result *= Vxz;
        result.y = Vy;

        return result;
    }

    Vector3 CalculatePosInTime(Vector3 vo, float time)
     {
         Vector3 Vxz = vo;
         Vxz.y = 0;
        //Vector3 result = shootPoint.position;
         Vector3 result = shootPoint.position + vo * time;
         //float sY = (-0.5f * Mathf.Abs(Physics.gravity.y) * (time * time)) + (vo.y * time) + shootPoint.position.y;
         float sY = shootPoint.position.y + vo.y * time ;

        result.y = sY;

         return result;
     }

    public static bool LineLineIntersection(out Vector3 intersection, Vector3 linePoint1,
        Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2)
    {

        Vector3 lineVec3 = linePoint2 - linePoint1;
        Vector3 crossVec1and2 = Vector3.Cross(lineVec1, lineVec2);
        Vector3 crossVec3and2 = Vector3.Cross(lineVec3, lineVec2);

        float planarFactor = Vector3.Dot(lineVec3, crossVec1and2);

        //is coplanar, and not parallel
        if (Mathf.Abs(planarFactor) < 0.0001f
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