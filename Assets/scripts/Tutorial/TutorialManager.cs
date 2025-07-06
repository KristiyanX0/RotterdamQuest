using UnityEngine;
using System.Collections;
using RotterdamQuestGameUtils;
using System.Collections.Generic;
using System;
using System.Linq;

// https://gamedev.stackexchange.com/questions/205006/development-pattern-for-interactive-in-game-tutorials

// use Action Queue -> 
// Event Based -> 


// NOT USED - TutorialManagerQueue is used instead
public class TutorialManager : MonoBehaviour
{
    public List<TutorialData> dataX; // Assign in Inspector
    public ArrowController arrow; // Assign in Inspector (UI Arrow)
    private List<TutorialDataWrapper> data = new List<TutorialDataWrapper>();
    // Game Object to be deactivated during tutorial
    [SerializeField] private List<GameObject> ObjectToDeactivate; 

    void Start()
    {
        foreach (var step in dataX)
        {

            TutorialDataWrapper temp = new TutorialDataWrapper(step, TutorialDataWrapper.StateOfTutorial.NonActive);
            data.Add(temp);
        }
        StartCoroutine(CheckForActiveTutorials());
    }

    IEnumerator CheckForActiveTutorials()
    {
        bool isAnyActive = false;
        while (true)
        {
            isAnyActive = data.Any(x => x.state == TutorialDataWrapper.StateOfTutorial.Active);

            foreach (var step in data)
            {
                if (step.state == TutorialDataWrapper.StateOfTutorial.NonActive &&
                    step.data.activatedObject.activeInHierarchy)
                {
                    Debug.Log("Show Arrow");
                    step.state = TutorialDataWrapper.StateOfTutorial.Active;
                    arrow.Show(step.data);
                    GlobalInteractionManager.RegisterObject(step.data.targetObject);
                }
                else if (step.state == TutorialDataWrapper.StateOfTutorial.Active && 
                         arrow.gameObject.activeSelf && 
                         GlobalInteractionManager.WasInteracted(step.data.targetObject)) // Arrow is visible
                {
                    Debug.Log("Interaction detected! Hiding arrow.");
                    arrow.Hide();
                    step.state = TutorialDataWrapper.StateOfTutorial.Finished;
                }                
            }
            if (isAnyActive) 
            {
                Debug.Log("Deactivating objects");
                ObjectToDeactivate.ForEach(x => x.SetActive(false));
            }
            else 
            {
                Debug.Log("Activating objects");
                ObjectToDeactivate.ForEach(x => x.SetActive(true));
            }
            isAnyActive = false;
            yield return new WaitForSeconds(0.25f);
        }
    }

}
