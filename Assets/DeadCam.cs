using UnityEngine;

public class DeadCam : MonoBehaviour
{
    public float distanceAway;
    public float distanceUp;
    public float smooth;
    public Transform follow;

    private Vector3 targetPosition;

    void OnEnable()
    {
        GameObject anotherPlayer = PlayerManager.PMinstance.findLivingPlayer().playerAvatar;
        follow = anotherPlayer.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        targetPosition = follow.position + Vector3.up * distanceUp - Vector3.back * distanceAway;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smooth);
        transform.LookAt(follow);
    }
}
