using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager instance;
    private Vector3 _targetPosition;
    private string _to;

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
        currentScene = GameManager.instance.gameStatus.nameScene;
    }

    public void InitSwitchScene(string to,Vector3 targetPosition)
    {
        StartCoroutine(Transition(to, targetPosition));
    }

    private IEnumerator Transition(string to, Vector3 targetPosition)
    {
        screenTint.Tint();
        yield return new WaitForSeconds(1f / screenTint.speed + 0.1f); // 1 second divided by speed of tining and small addition of time offset
        SwitchScene(to,targetPosition);
        while(load == null)
        {
            yield return null;
        }
        while(load.isDone == false && unLoad.isDone == false)
        {
            yield return null; // Skip 1 frame
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(currentScene));
        cameraConfiner.UpdateBounds();
        screenTint.UnTint();
    }
    public void SwitchScene(string to,Vector3 targetPosition)
    {
        Debug.Log(currentScene);
        _to = to;
        _targetPosition = targetPosition;
        load = SceneManager.LoadSceneAsync(to, LoadSceneMode.Additive);
        unLoad = SceneManager.UnloadSceneAsync(currentScene);
        currentScene = _to;
        GameManager.instance.gameStatus.nameScene = _to;
        Transform playerTransform = GameManager.instance.player.transform;

        CinemachineBrain currentCamera = Camera.main.GetComponent<CinemachineBrain>();
        currentCamera.ActiveVirtualCamera.OnTargetObjectWarped(playerTransform,_targetPosition - playerTransform.position); 

        playerTransform.position = new Vector3(_targetPosition.x, _targetPosition.y,0);
        // load.completed -= OnCompleteLoadScene;
        // load.completed += OnCompleteLoadScene;
    }
    private void OnCompleteLoadScene(AsyncOperation asyncOperation)
    {
    }
}
