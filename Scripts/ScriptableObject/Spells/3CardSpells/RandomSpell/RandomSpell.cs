using UnityEngine;

[CreateAssetMenu(fileName = "RandomSpell", menuName = "Scriptable Objects/RandomSpell")]
public class RandomSpell : SpellEffect
{
    void Awake()
    {
        this.duration = 1;
        this.targetsNumber = 0;
        this.spellType = 1;
        this.spellEffectsCount = 1;
        this.SelfCasted = true;
        this.spellIndex = 16;
    }

    public override void InitializeSpell(int newCaster, int target, SpellManager spellManager)
    {
        this.caster = newCaster;
        this.spellManager = spellManager;
        this.targets = new int[0];
    }
    public override void OnCast()
    {
        this.Effect(null, caster, caster);
    }

    public override void Effect(SpellEffect spell, int target, int caster)
    {
        this.spellManager.CreateRandomSpell(caster);
    }
}
