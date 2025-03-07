using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ProjectileLine : MonoBehaviour
{
    static List <ProjectileLine> PROJ_LINES = new List<ProjectileLine>();
    private const float DIM_MULT = 0.75f;
    private LineRenderer _line;
    private bool _drawing = false;
    private BallLifetime _ballLifetime;

    void Start()
    {
        _line = GetComponent<LineRenderer>();
        _line.positionCount = 0;

        ADD_LINE(this);
    }

    void FixedUpdate()
    {
        if (!_drawing || _ballLifetime == null) return;

        // Add the current position of the projectile to the line renderer
        _line.positionCount++;
        _line.SetPosition(_line.positionCount - 1, _ballLifetime.transform.position);

        // Stop drawing if the ball is no longer awake
        if (!_ballLifetime.awake)
        {
            _drawing = false;
            _ballLifetime = null;
        }
    }

    // Public method to start drawing the projectile line
    public void StartDrawing(BallLifetime ball)
    {
        if (_line == null) _line = GetComponent<LineRenderer>();

        _ballLifetime = ball; // Store reference to the BallLifetime instance
        _line.positionCount = 1;
        _line.SetPosition(0, ball.transform.position); 
        _drawing = true;
    }

    private void OnDestroy() {
        PROJ_LINES.Remove(this);
    }

    static void ADD_LINE(ProjectileLine newLine) {
        Color col;

        foreach (ProjectileLine pl in PROJ_LINES) {
            col = pl._line.startColor;
            col = col * DIM_MULT;
            pl._line.startColor = pl._line.endColor = col;
        }

        PROJ_LINES.Add(newLine);
    }
}
