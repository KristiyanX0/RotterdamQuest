using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO.Compression;

namespace RotterdamQuestGameUtils
{
    [Serializable]
    public class PairData<T, K, Z>
    {
        public T first;
        public K second;
        public Z third;
    }

    [Serializable]
    public class TripleData<T, K, Z>
    {
        public T first;
        public K second;
        public Z third;
    }

    [System.Serializable]
    public class SideQRewardData
    {
        public int id; // Unique identifier for the reward
        public Sprite icon; // Reward icon
        public float mapX; // X position on the map
        public float mapY; // Y position on the map
    }


    [Serializable]
    public class ActualPair<T, K>
    {
        public T first;
        public K second;
    }


    [Serializable]
    public class TutorialData
    {
        public enum ArrowMovementType
        {
            Bouncing,
            Sliding,
            Pulsing
        }

        public GameObject activatedObject; // When this object is active, tutorial starts
        public GameObject targetObject;
        public ArrowMovementType movementType;
        public float amount = 0f;
        public float duration = 0f;
        public Vector3 targetPosition;  // Manually set position in Inspector
        public Vector3 targetRotation;  // Manually set rotation in Inspector (Euler angles)
    }

    public class TutorialDataWrapper
    {
        public enum StateOfTutorial
        {
            NonActive,
            Active,
            Finished
        }

        public TutorialData data {get; set;}
        public StateOfTutorial state {get; set;}

        public TutorialDataWrapper(TutorialData data, StateOfTutorial state = StateOfTutorial.NonActive)
        {
            this.data = data;
            
            this.state = state;
        }
    }
}
