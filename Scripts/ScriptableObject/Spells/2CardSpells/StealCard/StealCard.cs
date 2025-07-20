using UnityEngine;

[CreateAssetMenu(fileName = "StealCard", menuName = "Scriptable Objects/StealCard")]
public class StealCard : SpellEffect
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
        this.spellManager.StealCard(caster, target);
        SendIdToClients();
    }
}
