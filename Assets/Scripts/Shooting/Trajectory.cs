using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for drawing trajectories
/// </summary>
public class Trajectory : MonoBehaviour
{
    private LineRenderer trajectoryRenderer;

    private void Reset()
    {
        // this class needs Line Renderer Component
        if (!TryGetComponent<LineRenderer>(out trajectoryRenderer))
        {
            gameObject.AddComponent<LineRenderer>();
        }
    }

    private void Start()
    {
        trajectoryRenderer = gameObject.GetComponent<LineRenderer>();
    }

    /// <summary>
    /// Draws line with Line Renderer using two points
    /// </summary>
    /// <param name="startPosition"> First point </param>
    /// <param name="endPosition"> Second (end) point </param>
    public void SetLineTrajectory(Vector2 startPosition, Vector2 endPosition)
    {
        trajectoryRenderer.positionCount = 2;

        trajectoryRenderer.SetPosition(0, startPosition);
        trajectoryRenderer.SetPosition(1, endPosition);
    }
    
    /// <summary>
    /// Draws parabola (ballistic trajectory) with Line Renderer
    /// </summary>
    /// <param name="startPosition"> Start point </param>
    /// <param name="speed"> Velocity of simulated object </param>
    public void SetBallisticTrajectory(Vector2 startPosition, Vector2 speed)
    {
        trajectoryRenderer.positionCount = 20;

        for (int i = 0; i < trajectoryRenderer.positionCount; i++)
        {
            // begin to draw from the second time point (to make distance from start point)
            float time = 0.1f * (i + 1);
            Vector2 position = startPosition + speed * time + Physics2D.gravity * time * time / 2;
            trajectoryRenderer.SetPosition(i, position);
            // cut the line
            if (position.y < 0)
            {
                trajectoryRenderer.positionCount = i + 1;
                break;
            }
        }
    }

    /// <summary>
    /// Clear all points
    /// </summary>
    public void Clear()
    {
        trajectoryRenderer.positionCount = 0;
    }
}
