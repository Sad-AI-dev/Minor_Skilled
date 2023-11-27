using Game.Core;

namespace Game.Core.GameSystems
{
    //========= Objectives ==========
    public class ObjectiveCompleteEvent : Event
    {
        public ObjectiveCompleteEvent(ObjectiveManager manager, Objective objective)
        {
            objectiveManager = manager;
            this.objective = objective;
        }

        public ObjectiveManager objectiveManager;
        public Objective objective;
    }
}
