using UnityEngine;

[CreateAssetMenu(fileName = "DiscardCard", menuName = "Scriptable Objects/DiscardCard")]
public class DiscardCard : SpellEffect
{
    void Awake()
    {
        this.duration = 1;
        this.targetsNumber = 1;
        this.spellType = 1;
        this.spellEffectsCount = 1;
        this.SelfCasted = false;
    }

    public override void OnCast()
    {
        if (!HasEnded())
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
        this.spellManager.DiscardCard(target);
        SendIdToClients();
    }
}
