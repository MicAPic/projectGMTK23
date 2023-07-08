using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public Rigidbody2D[] boxes;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }

        Instance = this;
    }
    
    // Start is called before the first frame update
    // void Start()
    // {
    //     
    // }

    // Update is called once per frame
    // void Update()
    // {
    //     
    // }
}
