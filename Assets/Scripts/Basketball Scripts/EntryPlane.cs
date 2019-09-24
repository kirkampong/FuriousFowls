using UnityEngine;
using System.Collections;

public class EntryPlane : MonoBehaviour
{

    public Vector3 pos0;//position of p0 point
    public Vector3 pos1;//position of p1 point
    public Transform p0, p1;

    // Use this for initialization
    void Start()
    {
        if (p0 == null)
        {
            p0 = transform.Find("P0").transform;
        }

        if (p1 == null)
        {
            p1 = transform.Find("P1").transform;
        }

        pos0 = p0.position;
        pos1 = p1.position;
    }
}