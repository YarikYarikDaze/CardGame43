using UnityEngine;

[CreateAssetMenu(fileName = "TakeAdditionalCard", menuName = "Scriptable Objects/TakeAdditionalCard")]
public class TakeAdditionalCard : SpellEffect
{
    void Awake()
    {
        this.duration = 2;
        this.targetsNumber = 1;
    }
    public override void OnHit(SpellEffect spell)
    { 
        if(!this.HasEnded()) {
            this.Effect(spell, targets[0]);
            this.duration--;
        }
    }

    public override void OnTurn(SpellEffect spell)
    {
        this.EndSpell();
    }

    public override void OnCast(SpellEffect spell) { }

    public override void Effect(SpellEffect spell, int index)
    {
        this.spellManager.GiveCardToPlayer(index);
    }
}
