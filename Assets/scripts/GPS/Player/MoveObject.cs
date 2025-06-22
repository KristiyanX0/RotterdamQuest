using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public List<GameObject> referenceGameObjects; // Reference to the GameObject to be touched in order to drag
    public GameObject movableGameObject;  // Reference to the GameObject to be moved
    public float dragSpeed = 5f;       // Speed at which the object moves with the touch

    private Vector2 lastTouchPosition;    // Last position of the touch
    private bool isDragging = false;     // Whether dragging is active

    void Update()
    {
        // Detect one-finger touch
        if (Input.touchCount == 1 &&
            UITools.IsPointerOverAnyUIObject(referenceGameObjects, Input.GetTouch(0).fingerId))
        {
            Touch touch = Input.GetTouch(0);

            // Check if the touch began or if it's already dragging
            if (touch.phase == TouchPhase.Began)
            {
                isDragging = true;
                lastTouchPosition = touch.position; // Store the starting touch position
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                // Calculate the delta movement
                Vector2 deltaPosition = touch.position - lastTouchPosition;

                // Apply the delta to the GameObject's position
                Vector3 newPosition = movableGameObject.transform.position;
                newPosition.x += deltaPosition.x * dragSpeed * Time.deltaTime;
                newPosition.y += deltaPosition.y * dragSpeed * Time.deltaTime;
                movableGameObject.transform.position = newPosition;

                // Update the last touch position
                lastTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                isDragging = false; // Stop dragging when the touch ends
            }
        }
        else
        {
            isDragging = false; // Reset dragging state when touch count changes
        }
    }
}
