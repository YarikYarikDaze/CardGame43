using UnityEngine;

[CreateAssetMenu(fileName = "SkipTurn", menuName = "Scriptable Objects/SkipTurn")]
public class SkipTurn : SpellEffect
{
    void Awake()
    {
        this.duration = 1;
        this.targetsNumber = 1;
    }
    public override void OnHit(SpellEffect spell) { }

    public override void OnTurn(SpellEffect spell)
    {
        if (!this.HasEnded())
        {
            this.Effect(spell, targets[0]);
            this.EndSpell();
        }
    }

    public override void OnCast(SpellEffect spell) { }

    public override void Effect(SpellEffect spell, int index)
    {
        this.spellManager.EndPlayerTurn(index);
    }
}
