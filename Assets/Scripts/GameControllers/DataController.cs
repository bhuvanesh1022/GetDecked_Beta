using UnityEngine;

public class DataController : MonoBehaviour
{
    public static DataController dataController;

    public string myName;
    public string myCharacter;
    public int MyId;
    public AudioClip[] Sounds;
    private void Awake()
    {
        if (dataController == null)
        {
            dataController = this;
        }
        else
        {
            if (dataController != this)
            {
                Destroy(dataController.gameObject);
                dataController = this;
            }
        }

        DontDestroyOnLoad(gameObject);
    }
}
