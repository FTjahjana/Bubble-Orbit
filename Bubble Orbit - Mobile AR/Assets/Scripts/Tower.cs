using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Tower : MonoBehaviour
{
    [Header("Tower Range Boundaries")]
    public Vector2 innerAndOuterBoundary = new Vector2(4f, 25f);
    public float lowerBoundary_a = 30f;
    public float lowerBoundary_r, lowerBoundary_h;
    public Vector3 lowerBoundary_p;

    void Start()
    {
        (lowerBoundary_r, lowerBoundary_h, lowerBoundary_p) = FindLowerCircle();
    }

    (float, float, Vector3) FindLowerCircle()
    {
        double lbr = lowerBoundary_a * Math.PI / 180.0;
        double cr = innerAndOuterBoundary.y * Math.Cos(lbr);
        double h = innerAndOuterBoundary.y * (1 + Math.Sin(lbr));

        Vector3 pos = transform.position;
        pos.y += innerAndOuterBoundary.y - (float)h;

        return ((float)cr, (float)h, pos);
    }

    #if UNITY_EDITOR
        void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow; Handles.color = Color.green;

            Gizmos.DrawWireSphere(transform.position, innerAndOuterBoundary.x);
            Gizmos.DrawWireSphere(transform.position, innerAndOuterBoundary.y);

            (float cr, float h, Vector3 pos) = FindLowerCircle();
            
            Handles.DrawWireDisc(pos, Vector3.up, cr, 2);

        }
    #endif
}
