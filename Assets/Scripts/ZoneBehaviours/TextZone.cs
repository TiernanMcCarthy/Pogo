using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZoneBehaviour
{
    public class TextZone : ZoneBehaviour
    {
        [SerializeField,TextArea] string textPrompt;

        [SerializeField] float displayTime = 4;

        protected override void FirstEntry(ColliderRigidbodyReference rigid)
        {
            PlayerManagement.instance.UpdateText(textPrompt,displayTime);
        }
    }
}
