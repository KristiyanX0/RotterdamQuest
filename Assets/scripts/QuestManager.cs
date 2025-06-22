using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public List<GameObject> quests; // List of quest panels
    public GameObject questSelectedPanel; // Panel for "Quest Selected"
    public GameObject questCompletedPanel; // Panel for "Quest Completed"
    public GameObject ClaimRewardPanel; // Panel for "Claim your reward"
    public GameObject emptyPanel; // Panel for text background
    public GameObject rewardPanel;

    public Button leftArrowButton;
    public Button rightArrowButton;
    // public Button selectButton;
    // public Button completeButton;
    // public Button claimRewardButton;

    private int currentQuestIndex = 0;

    void Start()
    {
        // Initialize all panels
        UpdateQuestDisplay();
        
        questSelectedPanel.SetActive(false);
        questCompletedPanel.SetActive(false);
        ClaimRewardPanel.SetActive(false);
        rewardPanel.SetActive(false);
        emptyPanel.SetActive(false);
    }

    void UpdateQuestDisplay()
    {
        for (int i = 0; i < quests.Count; i++)
        {
            quests[i].SetActive(i == currentQuestIndex);
        }

        // Disable/Enable arrow buttons based on position
        leftArrowButton.gameObject.SetActive(currentQuestIndex > 0);
        rightArrowButton.gameObject.SetActive(currentQuestIndex < quests.Count - 1);
    }

    public void PreviousQuest()
    {
        if (currentQuestIndex > 0)
        {
            currentQuestIndex--;
            UpdateQuestDisplay();
        }
    }

    public void NextQuest()
    {
        if (currentQuestIndex < quests.Count - 1)
        {
            currentQuestIndex++;
            UpdateQuestDisplay();
        }
    }

    public void SelectQuest()
    {
        Debug.Log("Quest selected: " + quests[currentQuestIndex].GetComponentInChildren<TextMeshProUGUI>().text);
        emptyPanel.SetActive(true);
        emptyPanel.GetComponentInChildren<TextMeshProUGUI>().text = quests[currentQuestIndex].GetComponentInChildren<TextMeshProUGUI>().text;
        quests[currentQuestIndex].SetActive(false);
        questSelectedPanel.SetActive(true);
        leftArrowButton.gameObject.SetActive(false);
        rightArrowButton.gameObject.SetActive(false);
    }

    public void CompleteQuest()
    {
        questSelectedPanel.SetActive(false);
        questCompletedPanel.SetActive(true);
    }

    public void BackToQuests()
    {
        questSelectedPanel.SetActive(false);
        questCompletedPanel.SetActive(false);
        UpdateQuestDisplay();
    }

    public void ClaimReward()
    {
        emptyPanel.SetActive(false);
        questCompletedPanel.SetActive(false);
        ClaimRewardPanel.SetActive(true);
    }

    public void RewardPanel()
    {
        ClaimRewardPanel.SetActive(false);
        rewardPanel.SetActive(true);
    }
    
    public void FinishQuest()
    {
        rewardPanel.SetActive(false);
    }
}
