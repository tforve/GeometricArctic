using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    private static GameMaster instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // --------------

    [SerializeField] private GameObject levelStart;
    [SerializeField] private Vector3 lastCheckPointPos;               // Save the last transform.position of any Checkpoints

    public Vector3 MyLastCheckpointPos
    {
        get { return lastCheckPointPos; }
        set { lastCheckPointPos = value; }
    }

    void Start()
    {
        //Setup Levelstart as std 
        lastCheckPointPos.x = levelStart.transform.position.x;
        lastCheckPointPos.y = levelStart.transform.position.y;
        lastCheckPointPos.z = levelStart.transform.position.z;
    }

    ///<summary>Reload the Current Scene. Used after Player died</summary>
    public void LoadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    ///<summary> Set lastCheckpointPos to the pos of last checkpoint you where colliding with</summary>
    public void SetLastCheckpoint(Vector3 pos)
    {
        lastCheckPointPos = pos;
    }
}

