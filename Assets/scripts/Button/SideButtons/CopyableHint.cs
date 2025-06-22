using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RotterdamQuestGameUtils;
using System.Runtime.CompilerServices;

public class CopyableHint : MonoBehaviour
{
    private List<ActualPair<Sprite, string>> copyableHints;
    [SerializeField] private CopyHintsData copyHintsData;
    [SerializeField] private Image image;

    void Start()
    {
        copyableHints = copyHintsData.copyableHints;
    }

    public void CopyToClipboard()
    {
        string textToCopy = copyableHints.Find(x => x.first == image.sprite)?.second;
        if (textToCopy != null)
        {
            GUIUtility.systemCopyBuffer = textToCopy; // Copies the text to clipboard
            Debug.Log("Copied to clipboard: " + textToCopy);
        }
    }
}