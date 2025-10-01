using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class VectorDrawer : MonoBehaviour
{
    [Header("References")]
    public Camera tacticalCamera;
    public MechController mech;

    private Vector3 startPoint;
    private bool isDrawing = false;
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
    }

    void Update()
    {
        if (GamePhaseManager.Instance == null) return;
        if (GamePhaseManager.Instance.currentPhase != GamePhase.Tactical) return;

        // Проверяем зажатие ЛКМ
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = tacticalCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 2f);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log("Ray hit: " + hit.collider.name);
                startPoint = hit.point;
                isDrawing = true;

                // Линия от старта
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, startPoint);
                lineRenderer.SetPosition(1, startPoint);
            }
            else
            {
                Debug.Log("Raycast missed");
            }
        }

        // Рисуем линию пока зажата кнопка
        if (Mouse.current.leftButton.isPressed && isDrawing)
        {
            Ray ray = tacticalCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                lineRenderer.SetPosition(1, hit.point);
            }
        }

        // Отпуск ЛКМ — запуск действия
        if (Mouse.current.leftButton.wasReleasedThisFrame && isDrawing)
        {
            Ray ray = tacticalCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 direction = hit.point - startPoint;
                mech.SetActionDirection(direction);

                GamePhaseManager.Instance.StartActionPhase();
            }

            isDrawing = false;
            lineRenderer.positionCount = 0;
        }
    }
}
