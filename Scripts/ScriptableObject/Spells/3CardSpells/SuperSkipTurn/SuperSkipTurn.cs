using UnityEngine;

[CreateAssetMenu(fileName = "SuperSkipTurn", menuName = "Scriptable Objects/SuperSkipTurn")]
public class SuperSkipTurn : SpellEffect
{
    void Awake()
    {
        this.duration = 1;
        this.targetsNumber = 1;
        this.spellType = 2;
        this.spellEffectsCount = 1;
    }
    public override void OnHit(SpellEffect spell) { }

    public override void OnTurn()
    {
        if (!this.HasEnded())
        {
            this.Effect(null, targets[0], caster);
            this.EndSpell();
        }
    }

    public override void OnCast()
    {
    }

    public override void Effect(SpellEffect spell, int target, int caster)
    {
        this.spellManager.EndPlayerTurn(target);
        this.spellManager.GiveCardToPlayer(target);
    }   
}
