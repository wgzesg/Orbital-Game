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
        Debug.Log("The length of PLayer list is " + PlayerManager.PMinstance.playerList.Count);
        Debug.Log("another player is " + PlayerManager.PMinstance.FindAnotherPlayer());
        GameObject anotherPlayer = PlayerManager.PMinstance.FindAnotherPlayer().playerAvatar;
        follow = anotherPlayer.transform;
    }
    private void OnDisable()
    {
        follow = null;
    }

    private void LateUpdate()
    {
        if (follow == null)
            return;
        targetPosition = follow.position + Vector3.up * distanceUp - Vector3.back * distanceAway;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smooth);
        transform.LookAt(follow);
    }
}
