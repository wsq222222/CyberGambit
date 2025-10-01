using UnityEngine;
using System.Collections;

public enum GamePhase { Tactical, Action }

public class GamePhaseManager : MonoBehaviour
{
    public static GamePhaseManager Instance;
    public GamePhase currentPhase = GamePhase.Tactical;

    [Header("Cameras")]
    public Camera tacticalCamera;
    public Camera fpsCamera;

    private void Awake()
    {
        Instance = this;
        ApplyPhaseCameras();
    }

    public void StartActionPhase()
    {
        currentPhase = GamePhase.Action;
        ApplyPhaseCameras();
        StartCoroutine(EndActionAfterTime(5f));
    }

    private IEnumerator EndActionAfterTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        currentPhase = GamePhase.Tactical;
        ApplyPhaseCameras();
    }

    private void ApplyPhaseCameras()
    {
        if (tacticalCamera != null) tacticalCamera.enabled = (currentPhase == GamePhase.Tactical);
        if (fpsCamera != null) fpsCamera.enabled = (currentPhase == GamePhase.Action);
    }
}
