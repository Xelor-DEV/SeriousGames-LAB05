using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class ArenaController : MonoBehaviourPunCallbacks
{

    #region UnityMethods
    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }
    #endregion

    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {

    }
}
