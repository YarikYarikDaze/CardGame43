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
        // spellConfigs[0].attackSegmentIndex = 0; spellConfigs[0].particleIndex = 0;
        // spellConfigs[1].attackSegmentIndex = 0; spellConfigs[1].particleIndex = 1;
        // spellConfigs[2].attackSegmentIndex = 0; spellConfigs[2].particleIndex = 2;
        // spellConfigs[3].attackSegmentIndex = 1; spellConfigs[3].particleIndex = 0;
        // spellConfigs[4].attackSegmentIndex = 1; spellConfigs[4].particleIndex = 1;
        // spellConfigs[5].attackSegmentIndex = 1; spellConfigs[5].particleIndex = 2;
        // spellConfigs[6].attackSegmentIndex = 2; spellConfigs[6].particleIndex = 0;
        // spellConfigs[7].attackSegmentIndex = 2; spellConfigs[7].particleIndex = 1;
        // spellConfigs[8].attackSegmentIndex = 2; spellConfigs[8].particleIndex = 2;

        // spellConfigs[9].attackSegmentIndex = 0; spellConfigs[9].particleIndex = 0;
        // spellConfigs[10].attackSegmentIndex = 0; spellConfigs[10].particleIndex = 0;
        // spellConfigs[11].attackSegmentIndex = 0; spellConfigs[11].particleIndex = 0;
        // spellConfigs[12].attackSegmentIndex = 0; spellConfigs[12].particleIndex = 1;
        // spellConfigs[13].attackSegmentIndex = 0; spellConfigs[13].particleIndex = 1;
        // spellConfigs[14].attackSegmentIndex = 0; spellConfigs[14].particleIndex = 1;
        // spellConfigs[15].attackSegmentIndex = 0; spellConfigs[15].particleIndex = 2;
        // spellConfigs[16].attackSegmentIndex = 0; spellConfigs[16].particleIndex = 2;
        // spellConfigs[17].attackSegmentIndex = 0; spellConfigs[17].particleIndex = 2;
        // spellConfigs[18].attackSegmentIndex = 1; spellConfigs[18].particleIndex = 0;
        // spellConfigs[19].attackSegmentIndex = 1; spellConfigs[19].particleIndex = 0;
        // spellConfigs[20].attackSegmentIndex = 1; spellConfigs[20].particleIndex = 0;
        // spellConfigs[21].attackSegmentIndex = 1; spellConfigs[21].particleIndex = 1;
        // spellConfigs[22].attackSegmentIndex = 1; spellConfigs[22].particleIndex = 1;
        // spellConfigs[23].attackSegmentIndex = 1; spellConfigs[23].particleIndex = 1;
        // spellConfigs[24].attackSegmentIndex = 1; spellConfigs[24].particleIndex = 2;
        // spellConfigs[25].attackSegmentIndex = 1; spellConfigs[25].particleIndex = 2;
        // spellConfigs[26].attackSegmentIndex = 1; spellConfigs[26].particleIndex = 2;
        // spellConfigs[27].attackSegmentIndex = 2; spellConfigs[27].particleIndex = 0;
        // spellConfigs[28].attackSegmentIndex = 2; spellConfigs[28].particleIndex = 0;
        // spellConfigs[29].attackSegmentIndex = 2; spellConfigs[29].particleIndex = 0;
        // spellConfigs[30].attackSegmentIndex = 2; spellConfigs[30].particleIndex = 1;
        // spellConfigs[31].attackSegmentIndex = 2; spellConfigs[31].particleIndex = 1;
        // spellConfigs[32].attackSegmentIndex = 2; spellConfigs[32].particleIndex = 1;
        // spellConfigs[33].attackSegmentIndex = 2; spellConfigs[33].particleIndex = 2;
        // spellConfigs[34].attackSegmentIndex = 2; spellConfigs[34].particleIndex = 2;
        // spellConfigs[35].attackSegmentIndex = 2; spellConfigs[35].particleIndex = 2;
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