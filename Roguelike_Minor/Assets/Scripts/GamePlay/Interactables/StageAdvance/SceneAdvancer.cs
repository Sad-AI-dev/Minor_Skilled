using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    /// <summary>
    /// This is basically just a wrapper class for the game state manager
    /// </summary>
    public class SceneAdvancer : MonoBehaviour
    {
        public void AdvanceToNextPlanet()
        {
            GameStateManager.instance.AdvanceToNextPlanet();
        }

        public void AdvanceToNextStage()
        {
            GameStateManager.instance.AdvanceToNextStage();
        }
    }
}
