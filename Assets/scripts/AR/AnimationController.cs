using UnityEngine;
using UnityEngine.UI;  // For working with UI buttons

public class AnimationController : MonoBehaviour
{
    public Animator animator;       // Reference to Animator
    public Button attackButton;     // Reference to Attack Button
    public Button leaveButton;      // Reference to Leave Button

    void Start()
    {
        // Add listeners to button click events
        attackButton.onClick.AddListener(StartAttack);
        leaveButton.onClick.AddListener(StartLeave);
        animator.SetBool("Defeated", false);
        animator.SetBool("Attack", false);
    }

    void Update()
    {
        CheckAndDeactivateAttack();
        CheckAndDeactivateLeave();
    }

    // Function to start the Attack animation
    void StartAttack()
    {
        Debug.Log("Attack button clicked!");
        animator.SetBool("Attack", true);
    }

    // Function to start the Leave animation
    void StartLeave()
    {
        Debug.Log("Leave button clicked!");
        animator.SetBool("Defeated", true);   // Set Leave parameter to true
    }

    // Check if the Attack animation is running and deactivate the Attack boolean
    void CheckAndDeactivateAttack()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0); // 0 for the base layer

        // Check if the Attack animation is playing
        if (stateInfo.IsName("attack"))
        {
            // Deactivate the Attack boolean when the Attack animation is running
            animator.SetBool("Attack", false);
        }
    }

    // Check if the Leave animation is running and deactivate the Leave boolean
    void CheckAndDeactivateLeave()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0); // 0 for the base layer

        // Check if the Leave animation is playing
        if (stateInfo.IsName("disappear"))
        {
            // Deactivate the Leave boolean when the Leave animation is running
            animator.SetBool("Defeated", false);
        }
    }
}
