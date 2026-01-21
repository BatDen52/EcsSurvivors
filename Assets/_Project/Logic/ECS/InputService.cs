using UnityEngine;

public class InputService
{
    private const string Horizontal = "Horizontal";
    private const string Vertical = "Vertical";

    public Vector3 GetDirection()
    {
        return new Vector2(
                Input.GetAxisRaw(Horizontal),
                Input.GetAxisRaw(Vertical)
            );
    }
}