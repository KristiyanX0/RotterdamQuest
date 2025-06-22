using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class JumpToTag : MonoBehaviour
{
    public SequenceManager manager;

    public void JumpToNextTag(string tag) {
        List<string> tags = manager.GameObjectElement.Select(obj => obj.tag).ToList();
        int currentIndex = manager.getIndex();
        while (tag != tags[currentIndex]) {
            currentIndex++;
        }
        manager.GoToIndex(currentIndex);
    }
}
