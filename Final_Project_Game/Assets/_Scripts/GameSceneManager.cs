using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager instance;
    private void Awake()
    {
        instance = this;
    }

    [SerializeField] private ScreenTint screenTint;
    [SerializeField] private CameraConfiner cameraConfiner;
    string currentScene;
    private AsyncOperation unLoad;
    private AsyncOperation load;
    private void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;
    }

    public void InitSwitchScene(string to,Vector3 targetPosition)
    {
        StartCoroutine(Transition(to, targetPosition));
    }

    private IEnumerator Transition(string to, Vector3 targetPosition)
    {
        screenTint.Tint();
        yield return new WaitForSeconds(1f / screenTint.speed + 0.1f); // 1 second devided by speed of tining and small addition of time offset
        SwitchScene(to,targetPosition);
        while(load != null && unLoad != null)
        {
            if (load.isDone)
                load = null;
            if (unLoad.isDone)
                unLoad = null;
            yield return new WaitForSeconds(0.1f);
        }
        cameraConfiner.UpdateBounds();
        screenTint.UnTint();
    }
    public void SwitchScene(string to,Vector3 targetPosition)
    {
        load = SceneManager.LoadSceneAsync(to, LoadSceneMode.Additive);
         unLoad = SceneManager.UnloadSceneAsync(currentScene);
        currentScene = to;
        Transform playerTransform = GameManager.instance.player.transform;

        CinemachineBrain currentCamera = Camera.main.GetComponent<CinemachineBrain>();
        currentCamera.ActiveVirtualCamera.OnTargetObjectWarped(playerTransform,targetPosition - playerTransform.position); 

        playerTransform.position = new Vector3(targetPosition.x,targetPosition.y,0);
    }
}
