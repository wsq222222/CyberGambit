using UnityEngine;
using UnityEngine.InputSystem;

public class MechController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    private Vector3 actionDirection = Vector3.zero;
    private float maxDistance = 0f;   // длина вектора
    private float traveledDistance = 0f;

    void Update()
    {
        if (GamePhaseManager.Instance != null && GamePhaseManager.Instance.currentPhase == GamePhase.Action)
        {
            // Новый Input System для WASD
            Vector3 input = Vector3.zero;
            if (Keyboard.current.wKey.isPressed) input.z += 1;
            if (Keyboard.current.sKey.isPressed) input.z -= 1;
            if (Keyboard.current.aKey.isPressed) input.x -= 1;
            if (Keyboard.current.dKey.isPressed) input.x += 1;

            Vector3 combined = actionDirection + input;

            if (combined.sqrMagnitude > 0.001f && traveledDistance < maxDistance)
            {
                // Рассчитываем движение
                Vector3 moveStep = combined.normalized * moveSpeed * Time.deltaTime;

                // Ограничиваем движение по длине вектора
                float remaining = maxDistance - traveledDistance;
                if (moveStep.magnitude > remaining)
                    moveStep = moveStep.normalized * remaining;

                transform.Translate(moveStep, Space.World);
                traveledDistance += moveStep.magnitude;

                // Поворот меха по движению
                Vector3 lookDir = new Vector3(combined.x, 0f, combined.z);
                if (lookDir.sqrMagnitude > 0.001f)
                {
                    transform.rotation = Quaternion.LookRotation(lookDir);
                }
            }
        }
    }

    public void SetActionDirection(Vector3 dir)
    {
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.001f)
        {
            actionDirection = Vector3.zero;
            maxDistance = 0f;
        }
        else
        {
            actionDirection = dir.normalized;
            maxDistance = dir.magnitude;   // дистанция = длина линии, которую нарисовал игрок
        }

        traveledDistance = 0f;
        Debug.Log("Action direction set: " + actionDirection + ", distance: " + maxDistance);
    }
}
