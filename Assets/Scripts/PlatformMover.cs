using UnityEngine;

public class PlatformMover : MonoBehaviour
{
    private Transform[] pathPoints;
    private int currentPointIndex = 0;
    private bool isMoving = false;

    [SerializeField] private float speed = 5.0f; // Velocidad de la plataforma
    [SerializeField] private float waitTimeAtPoint = 1.0f; // Tiempo que la plataforma espera en cada punto

    public void InitializePath(Transform[] path)
    {
        pathPoints = path;
        if (pathPoints.Length > 0)
        {
            transform.position = pathPoints[0].position; // Inicia en el primer punto
            currentPointIndex = 0;
            isMoving = true;
        }
    }

    private void Update()
    {
        if (isMoving && pathPoints != null && currentPointIndex < pathPoints.Length)
        {
            MoveTowardsPoint();
        }
    }

    private void MoveTowardsPoint()
    {
        Transform targetPoint = pathPoints[currentPointIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            currentPointIndex++;
            if (currentPointIndex < pathPoints.Length)
            {
                Invoke(nameof(StartMoving), waitTimeAtPoint); // Espera antes de continuar
                isMoving = false;
            }
            else
            {
                Destroy(gameObject); // Destruye la plataforma al finalizar
            }
        }
    }

    private void StartMoving()
    {
        isMoving = true;
    }
}