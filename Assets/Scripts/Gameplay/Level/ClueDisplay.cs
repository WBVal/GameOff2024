using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Gameplay.Level
{
    public class ClueDisplay : MonoBehaviour
    {
        [SerializeField]
        TextMeshPro clueText;

        public void SetClue(Clue clue)
        {
            clueText.text = "Target "
                + (clue.IsEqual ? "has " : "has no ")
                + clue.Attribute.ToString();
        }
    }
}
