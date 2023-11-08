using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This class manages input from archeryArea
/// </summary>
public class ArcheryArea : MonoBehaviour
{
    [SerializeField] private ArcherAnimationController archer;
    [SerializeField] private Trajectory mouseTrajectoryRenderer;
    [SerializeField] private Trajectory arrowTrajectoryRenderer;
    /// <summary>
    /// Where arrows should spawn
    /// </summary>
    [SerializeField] private Transform arrowSpawn;
    [SerializeField] private GameObject arrowPrefab;


    private float speed = 40f;
    private bool isMousePressed = false;
    private Vector3 startPosition;
    private Camera cam;

    private Vector2 finalArrowDirection;
    private SpriteRenderer areaImage;

    private bool canShoot = false;

    void Start()
    {
        startPosition = transform.position;
        cam = Camera.main;
        areaImage = GetComponent<SpriteRenderer>();
        // Adding method that shoots arrow to the event of shooting 
        archer.OnArcherShooting.AddListener(ShootArrow);
    }

    public void GetDragInput(InputAction.CallbackContext ctx)
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = cam.ScreenPointToRay(mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        // Normalized vector from start position to mouse position
        Vector2 startToMouse = (startPosition - cam.ScreenToWorldPoint(mousePosition)).normalized;

        if (hit.collider && hit.collider.gameObject == gameObject)
        {
            // when the mouse was clicked on this object
            if (ctx.started)
            {
                // enabling image of area
                if (areaImage)
                {
                    areaImage.enabled = true;
                }

                isMousePressed = true;

                // starting targeting animation
                archer.StartShoot();
                // starting showing trajectories
                StartCoroutine(ShowTrajectoriesRotateArcher());
            }
        }
        // when the mouse was released
        if (ctx.canceled)
        {
            if (isMousePressed)
            {
                // hiding image of area
                if (areaImage)
                {
                    areaImage.enabled = false;
                }
                isMousePressed = false;

                // clearing trajectories
                mouseTrajectoryRenderer.Clear();
                arrowTrajectoryRenderer.Clear();


                // calling end animation for shooting
                if (canShoot)
                {
                    archer.EndShoot();
                }
                else
                {
                    archer.EndTarget();
                }
                canShoot = false;
            }
        }
    }

    /// <summary>
    /// Coroutine for drawing tragectories and rotating archer while targeting
    /// </summary>
    /// <returns></returns>
    IEnumerator ShowTrajectoriesRotateArcher()
    {
        while (isMousePressed)
        {
            Vector3 mousePosition = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());            
            Vector2 direction = (startPosition - mousePosition).normalized;

            // the archer can only aim to the right 
            if (direction.x < 0)
            {
                direction = new Vector2(0, direction.y);
                canShoot = false;
            }
            else
            {
                canShoot = true;
                mouseTrajectoryRenderer.SetLineTrajectory(startPosition, mousePosition);

                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                archer.Targeting(angle);
                arrowTrajectoryRenderer.SetBallisticTrajectory(arrowSpawn.position, direction * speed);
            }
            finalArrowDirection = direction;
            yield return null;
        }
    }

    /// <summary>
    /// Instantiate and move arrow to target
    /// </summary>
    private void ShootArrow()
    {
        Rigidbody2D arrowRb = Instantiate(arrowPrefab, arrowSpawn.position, Quaternion.identity).GetComponent<Rigidbody2D>();
        arrowRb.velocity = finalArrowDirection * speed;
    }
}
