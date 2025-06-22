using System.Collections.Generic;
using RotterdamQuestGameUtils;
using UnityEngine;

[CreateAssetMenu(fileName = "TutorialsData", menuName = "Tutorial/Create New Tutorials Data")]
public class TutorialsData : ScriptableObject
{
    public List<TutorialData> data;
}
