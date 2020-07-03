using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public float DestroyTime = 3f;
    public Vector3 Offset = new Vector3(0, 2, 0);
    public Vector3 RandomizeIntensity = new Vector3((float) 0.5, 0, 0);
    Transform enemytTransform; // test

    // Start is called before the first frame update
    void Start()
    {
        enemytTransform = transform.parent; //test
        Destroy(gameObject, DestroyTime);
        transform.rotation = enemytTransform.rotation;// test for orientation
        transform.localPosition += Offset;
        transform.localPosition += new Vector3(Random.Range(-RandomizeIntensity.x, RandomizeIntensity.y),
            Random.Range(-RandomizeIntensity.y, RandomizeIntensity.y),
            Random.Range(-RandomizeIntensity.z, RandomizeIntensity.z));
    }
}
