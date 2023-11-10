using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.GameSystems
{
    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField] private AK.Wwise.Event[] audioEvents;

        public void Play(int index = 0)
        {
            if (index < audioEvents.Length)
            {
                audioEvents[index].Post(gameObject);
            }
        }
    }
}
