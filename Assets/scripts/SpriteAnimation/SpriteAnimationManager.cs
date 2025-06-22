using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimationManager : MonoBehaviour
{
    public Animator animator; // Assign in Inspector
    public bool loopAnimations = true; // If true, loop animations; otherwise, stop at the last one.
    public List<AnimationClip> animations; // List of animations
    private int currentIndex = 0;

    void Start()
    {
        if (animations.Count > 0)
        {
            PlayAnimation(currentIndex);
        }
    }

    public void PlayAnimation(int index)
    {
        if (index >= 0 && index < animations.Count)
        {
            animator.Play(animations[index].name);
            currentIndex = index;
        }
    }

    public void NextAnimation()
    {
        if (loopAnimations)
        {
            currentIndex = (currentIndex + 1) % animations.Count;
        }
        else
        {
            if (currentIndex < animations.Count - 1)
            {
                currentIndex++;
            }
        }
        PlayAnimation(currentIndex);
    }

    public void PrevAnimation()
    {
        currentIndex = (currentIndex - 1) % animations.Count;
        PlayAnimation(currentIndex);
    }
}
