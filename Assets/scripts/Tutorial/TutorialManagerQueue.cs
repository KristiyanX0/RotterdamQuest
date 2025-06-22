using UnityEngine;
using System.Collections;
using RotterdamQuestGameUtils;
using System.Collections.Generic;
using System;
using System.Linq;

public class TutorialManagerQueue : MonoBehaviour
{
    public List<TutorialData> tutorialDataList; // Assign in Inspector
    public ArrowController arrow; // Assign in Inspector (UI Arrow)
    [SerializeField] private List<GameObject> ObjectToDeactivate; 
    
    private List<TutorialDataWrapper> tutorialQueue = new List<TutorialDataWrapper>();


    void Start()
    {
        foreach (var step in tutorialDataList)
        {
            TutorialDataWrapper temp = new TutorialDataWrapper(step, TutorialDataWrapper.StateOfTutorial.NonActive);
            tutorialQueue.Add(temp);
        }

        StartCoroutine(HandleTutorialQueue());
    }

    IEnumerator HandleTutorialQueue()
    {
        while (tutorialQueue.Count > 0)
        {
            
            // Get the first tutorial step without removing it
            // Wait until the activatedObject is active in hierarchy
            yield return new WaitUntil(() => tutorialQueue.ToList().Any(x => x.data.activatedObject.activeInHierarchy));
            var currentTutorial = tutorialQueue.Find(x => x.data.activatedObject.activeInHierarchy);

            currentTutorial.state = TutorialDataWrapper.StateOfTutorial.Active;
            arrow.Show(currentTutorial.data);
            GlobalInteractionManager.RegisterObject(currentTutorial.data.targetObject);
            UpdateObjectActivation(false);

            // Wait until the targetObject is interacted with
            yield return new WaitUntil(() => 
                arrow.gameObject.activeSelf && GlobalInteractionManager.WasInteracted(currentTutorial.data.targetObject)
            );

            arrow.Hide();
            currentTutorial.state = TutorialDataWrapper.StateOfTutorial.Finished;
            tutorialQueue.Remove(currentTutorial); // Remove the completed tutorial step

            // Update object activation status based on remaining tutorial steps
            UpdateObjectActivation(true);
        }
    }

    void UpdateObjectActivation(bool state)
    {
        ObjectToDeactivate.ForEach(x => x.SetActive(state));
    }
}
