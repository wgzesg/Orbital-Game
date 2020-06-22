using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootFollowTarget : MonoBehaviour
{
    public Transform Target;
    Vector3 _velocity = Vector3.zero;
    public float MinModifier;
    public float MaxModifier;
    bool _isFollowing = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void StartFollowing()
    {
        _isFollowing = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (_isFollowing)
        {
            transform.position = Vector3.SmoothDamp(transform.position, Target.position, ref _velocity, Time.deltaTime * Random.Range(MinModifier, MaxModifier));
        }
    }
}

