using UnityEngine;

[CreateAssetMenu(fileName = "NULLSpell", menuName = "Scriptable Objects/NULLSpell")]
public class NULLSpell : SpellEffect
{
    void Awake()
    {
        this.duration = 0;
        this.targetsNumber = 0;
        this.spellType = 0;
        this.spellEffectsCount = 0;
    }
    public override void OnHit(SpellEffect spell)
    {
    }

    public override void OnTurn()
    {
    }

    public override void OnCast() { }

    public override void Effect(SpellEffect spell, int target, int caster)
    {
        //Null
    }
}
