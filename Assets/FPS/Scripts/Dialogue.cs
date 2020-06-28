using UnityEngine;

[CreateAssetMenu(fileName = "New dialogue")]
public class Dialogue : ScriptableObject
{
    public string dialogName;
    [TextArea(3,5)]
    public string[] conversation;
}
