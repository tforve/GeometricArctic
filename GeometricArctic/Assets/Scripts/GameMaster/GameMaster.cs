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

    [SerializeField] private Vector3 lastCheckPointPos;               // Save the last transform.position of any Checkpoints

    public Vector3 MyLastCheckpointPos
    {
        get { return lastCheckPointPos; }
        set { lastCheckPointPos = value; }
    }

    ///<summary>Reload the Current Scene. Used after Player died</summary>
    public void LoadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
