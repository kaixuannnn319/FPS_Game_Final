using UnityEngine;
using UnityEngine.Splines;

public class TrainSplineMover : MonoBehaviour
{
    public SplineContainer spline;

    public float speed = 0.1f;

    float progress = 0f;

    void Update()
    {
        progress += speed * Time.deltaTime;

        if (progress > 1f)
            progress -= 1f;

        Vector3 pos =
            spline.EvaluatePosition(progress);

        Vector3 forward =
            spline.EvaluateTangent(progress);

        transform.position = pos;

        forward.y = 0;

        transform.rotation =
    Quaternion.LookRotation(forward) * Quaternion.Euler(0, 90, 0);
    }
}