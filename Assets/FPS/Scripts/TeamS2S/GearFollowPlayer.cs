using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearFollowPlayer : MonoBehaviour
{
    public Transform Target;
    public float MinModifier = 7;
    public float MaxModifier = 11;
    Vector3 _velocity = Vector3.zero;
    bool _isfollowing = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartFollowing()
    {
        _isfollowing = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isfollowing)
        {
            if (Target.position != null)
            {
                transform.position = Vector3.SmoothDamp(transform.position, Target.position, ref _velocity, Time.deltaTime * Random.Range(MinModifier, MaxModifier));
            }
            else
            {
                Destroy(transform.parent.gameObject);
            }
        }
    }
}
