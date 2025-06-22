using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RotterdamQuestGameUtils;
using System.Runtime.CompilerServices;

public class HintSequence : MonoBehaviour
{
    public Image target;
    public List<PairData<GameObject, List<Sprite>, Boolean>> hints;
    private Sprite currentHint;

    void Start()
    {
        UpdateCurrentHint();

        if (target != null)
        {
            target.gameObject.SetActive(false);
        }
    }

    public void Toggle() {
        target.gameObject.SetActive(!target.gameObject.activeSelf);
        UpdateCurrentHint();
    }

    private void UpdateCurrentHint() 
    {
        Sprite temp = getUpdatedSprite();
        
        
        if (temp != null && temp != currentHint) 
        {
            currentHint = temp;
            ImageResizer.AdjustImageToSprite(target, currentHint);
        }
    }

    void Update()
    {
        UpdateCurrentHint();
    }

    private Sprite getUpdatedSprite() {
        Sprite hint = null;
        int index = 0;
        for (int i = 0; i < hints.Count; i++) 
        {
            if (hints[i].first.activeSelf)
            {
                if (hints[i].first.CompareTag("SideQuestChooser")) {
                    SideQuestQuestion sideQuestQuestion = hints[i].first.GetComponent<SideQuestQuestion>();
                    if (sideQuestQuestion != null && sideQuestQuestion.SelectedSideQuest.activeSelf) {
                        ImageLoader imageLoader = sideQuestQuestion.getImageLoader();
                        index = imageLoader.currentIndex;
                    }
                }
                hint = hints[i].second[index];
            }
        }
        return hint;
    }
}