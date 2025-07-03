using UnityEngine;

[CreateAssetMenu(fileName = "ShieldSpell", menuName = "Scriptable Objects/ShieldSpell")]
public class ShieldSpell : SpellEffect
{
    void Awake()
    {
        this.duration = 1;
    }
    public override void OnHit(SpellEffect spell)
    {
        if (!this.HasEnded())
        {
            spell.BlockSpell();
            this.duration = 0;
        }
    }

    public override void OnTurn(SpellEffect spell)
    {
        this.duration = 0;
    }

    public override void OnCast(SpellEffect spell) {}
}
