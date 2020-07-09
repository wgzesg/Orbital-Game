using UnityEngine;

public class GameSetup : MonoBehaviour
{
    public static GameSetup GS;
    public Transform[] playerBirthPlace;
    public void OnEnable()
    {
        if(GameSetup.GS == null)
        {
            GS = this;
        }
        else
        {
            if(GS != this)
            {
                Destroy(GS.gameObject); 
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
