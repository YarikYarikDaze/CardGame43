using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

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
        this.spellIndex = 14;
    }

    public override void InitializeSpell(int newCaster, int target, SpellManager spellManager)
    {
        this.caster = newCaster;
        this.spellManager = spellManager;
        this.GetTwoNextPlayers(target);
    }

    void GetTwoNextPlayers(int target)
    {
        this.targets = new int[targetsNumber];
        spellManager.GetTwoNextPlayers(target, caster);
        Array.Copy(spellManager.GetTwoNextPlayers(target, caster), this.targets, targetsNumber);
    }
    public override void OnHit(SpellEffect spell)
    {
        if (!this.HasEnded())
        {
            foreach (int index in targets)
            {
                if (index != -1)
                {
                    if (targets[0] == index)
                    {
                        this.Effect(null, index, caster);
                    }
                    this.Effect(null, index, caster);
                }
            }
            this.EndSpell();
        }
    }

    public override void Effect(SpellEffect spell, int target, int caster)
    {
        this.spellManager.GiveCardToPlayer(target);
        SendIdToClients();
    }
}
