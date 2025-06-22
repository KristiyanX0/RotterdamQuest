using System.Collections.Generic;
using RotterdamQuestGameUtils;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCopyableHint", menuName = "Hint/Copyable Hint Data")]
public class CopyHintsData : ScriptableObject
{
    public List<ActualPair<Sprite, string>> copyableHints;
}
