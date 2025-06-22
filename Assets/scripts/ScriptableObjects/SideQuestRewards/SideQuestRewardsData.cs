using System.Collections.Generic;
using RotterdamQuestGameUtils;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSideQuestRewardsData", menuName = "Side Quest/Rewards Data")]
public class SideQuestRewardsData : ScriptableObject
{
    public List<SideQRewardData> RewardsData;
}
