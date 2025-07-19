using UnityEngine;

[CreateAssetMenu(fileName = "ThreePlayersTakeCards", menuName = "Scriptable Objects/ThreePlayersTakeCards")]
public class ThreePlayersTakeCards : SpellEffect
{
    void Awake()
    {
        this.duration = 1;
        this.targetsNumber = 3;
        this.spellType = 2;
        this.spellEffectsCount = 1;
        this.SelfCasted = false;
    }

    public override void InitializeSpell(int newCaster, int target, SpellManager spellManager)
    {
        this.caster = newCaster;
        this.spellManager = spellManager;
        this.GetRandomPlayer();
    }

    void GetRandomPlayer()
    {
        this.targets = new int[1];
        this.targets[0] = spellManager.GetRandomPlayer();
    }
    public override void OnHit(SpellEffect spell)
    {
        if (!this.HasEnded())
        {
            foreach (int index in targets)
            {
                if (index != -1)
                {
                    this.Effect(null, index, caster);
                }
            }
            this.EndSpell();
        }
    }

    public override void Effect(SpellEffect spell, int target, int caster)
    {
        this.spellManager.GiveCardToPlayer(target);
    }
}
