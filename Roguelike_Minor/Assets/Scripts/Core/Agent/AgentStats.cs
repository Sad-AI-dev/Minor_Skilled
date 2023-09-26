namespace Game.Core {
    [System.Serializable]
    public class AgentStats
    {
        public float maxHealth = 10f;
        public float baseDamage = 1;

        //cooldowns
        public float attackSpeed = 1;
        public float coolDownMultiplier = 1;
    }
}
