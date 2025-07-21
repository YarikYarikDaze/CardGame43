using UnityEngine;

public class SpellAnimator : MonoBehaviour
{
    public Animator attackAnimator;    // Animator для AttackSegment
    public Animator particleAnimator;  // Animator для Particle
    public Animator portalAnimator;    // Animator для Portal

    [System.Serializable]
    public class SpellAnimationConfig
    {
        public int attackSegmentIndex; // 0: Electro, 1: Fire, 2: Water
        public int particleIndex;      // 0: Electro, 1: Fire, 2: Water
    }

    [SerializeField]
    public SpellAnimationConfig[] spellConfigs = new SpellAnimationConfig[36];

    private void Awake()
    {
        // Инициализация массива spellConfigs
        for (int i = 0; i < spellConfigs.Length; i++)
        {
            spellConfigs[i] = new SpellAnimationConfig();
        }

        // Заполнение конфигурации для каждого заклинания

        for (int i = 0; i < 9; i++) {
            spellConfigs[i].attackSegmentIndex = i / 3;
            spellConfigs[i].particleIndex = i % 3;
        } for (int i = 9; i < 36; i++) {
            spellConfigs[i].attackSegmentIndex = i / 9 - 1;
            spellConfigs[i].particleIndex = i / 3 % 3;
        }
    }

    public void PlaySpellAnimation(int spellIndex)
    {
        if (spellIndex < 0 || spellIndex >= 36)
        {
            Debug.LogWarning("Spell index out of range: " + spellIndex);
            return;
        }

        var config = spellConfigs[spellIndex];
        int attackIndex = config.attackSegmentIndex;
        int particleIndex = config.particleIndex;

        attackAnimator.SetInteger("attackType", attackIndex);
        particleAnimator.SetInteger("particleType", particleIndex);
        portalAnimator.SetTrigger("PlayPortal");
    }
}