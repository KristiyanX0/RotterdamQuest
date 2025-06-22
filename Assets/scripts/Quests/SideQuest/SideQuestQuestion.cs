using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using RotterdamQuestGameUtils;
public class SideQuestQuestion : MonoBehaviour
{
    public SideQuestRewardsData rewardsData;
    
    public GameObject sideQuestQuestions;
    public GameObject SelectedSideQuest;
    public GameObject wellDonePanel;
    public GameObject rewardPanel;
    
    private ImageLoader loader;
    // SideQRewardData currentSideQRewardDataChosen;
    private int currentSideQuestId;

    private void Start()
    {
        currentSideQuestId = 0;
        loader = SelectedSideQuest.GetComponentInChildren<ImageLoader>();
    }

    public ImageLoader getImageLoader() {
        return loader;
    }

    public void ShowSideQuestPanel()
    {
        sideQuestQuestions.SetActive(true);
        SelectedSideQuest.SetActive(false);
        wellDonePanel.SetActive(false);
        rewardPanel.SetActive(false);
    }

    public void ShowSelectedQuest(int index)
    {
        SelectedSideQuest.SetActive(true);
        loader.SetImage(index);
        sideQuestQuestions.SetActive(false);
    }

    public void ShowWellDone()
    {
        wellDonePanel.SetActive(true);
        SelectedSideQuest.SetActive(false);
    }

    public void ShowReward()
    {
        rewardPanel.SetActive(true);

        Image image = rewardPanel.GetComponentsInChildren<Image>()
                                  .FirstOrDefault(img => img.CompareTag("SideQuestRewardPanel"));
        if (image != null)
        {
            Debug.Log("currentQuestionsIndex: " + currentSideQuestId);
            Debug.Log("Icon Name: " + rewardsData.RewardsData[currentSideQuestId].icon.name);
            image.sprite = rewardsData.RewardsData[currentSideQuestId].icon;
        }

        wellDonePanel.SetActive(false);
    }

    public void BackToSideQuestQuestion()
    {
        sideQuestQuestions.SetActive(true);
        SelectedSideQuest.SetActive(false);
        wellDonePanel.SetActive(false);
        rewardPanel.SetActive(false);
    }

    public void Locate()
    {
        MapMarkerSpawner.instance.SpawnMarker(currentSideQuestId);
    }

    public void Hide()
    {
        sideQuestQuestions.SetActive(true);
        SelectedSideQuest.SetActive(false);
        wellDonePanel.SetActive(false);
        rewardPanel.SetActive(false);

        gameObject.SetActive(false);
    }

    public void SetSideQuestId(int id)
    {
        currentSideQuestId = id;
    }
}
