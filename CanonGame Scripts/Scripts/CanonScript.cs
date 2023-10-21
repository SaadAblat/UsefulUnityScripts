using UnityEngine;
using UnityEngine.InputSystem;

public class CanonScript : MonoBehaviour
{
    [Header("Setup Settings")]
    [SerializeField] Transform ballPos;
    [SerializeField] Transform Canon;
    [SerializeField] GameObject ballPrefab;
    [SerializeField] float rotSpeedx;
    [SerializeField] float rotSpeedy;
    [SerializeField] PlayerInput TouchInput;
    [SerializeField] float sensitivity = 1f; // Introducing sensitivity multiplier.

    float vertical;
    float horizontal;

    [Header("Level up Settings")]
    [SerializeField] public float fireDelay;
    float timeFromLastShoot;

    private void Start()
    {
        timeFromLastShoot = fireDelay;
    }

    void Update()
    {
        Vector2 aiming = TouchInput.actions["Rotate"].ReadValue<Vector2>();
        vertical = aiming.y * sensitivity; // Apply sensitivity multiplier.
        horizontal = aiming.x * sensitivity; // Apply sensitivity multiplier.

        timeFromLastShoot += Time.deltaTime;

        if (TouchInput.actions["Fire"].triggered)
        {
            LaunchBall();
        }

        AdjustHeight();
        Aim();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            LaunchBall();
        }
    }

    public void LaunchBall()
    {
        if (timeFromLastShoot >= fireDelay)
        {
            timeFromLastShoot = 0;
            Instantiate(ballPrefab, ballPos.position, Canon.rotation);
        }
    }

    private void Aim()
    {
        float newYRotation = Mathf.Lerp(transform.eulerAngles.y, transform.eulerAngles.y + horizontal * rotSpeedx, Time.deltaTime);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, newYRotation, 0);
    }

    private void AdjustHeight()
    {
        float newXRotation = Mathf.Lerp(Canon.eulerAngles.x, Canon.eulerAngles.x + vertical * rotSpeedy, Time.deltaTime);
        Canon.eulerAngles = new Vector3(newXRotation, Canon.eulerAngles.y, 0);
    }
}