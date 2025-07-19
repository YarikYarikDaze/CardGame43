using UnityEngine;

[CreateAssetMenu(fileName = "TakeCard", menuName = "Scriptable Objects/TakeCard")]
public class TakeCard : SpellEffect
{
    void Awake()
    {
        this.duration = 1;
        this.targetsNumber = 1;
        this.spellType = 2;
        this.spellEffectsCount = 1;
        this.SelfCasted = false;
    }

    public override void OnCast()
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
