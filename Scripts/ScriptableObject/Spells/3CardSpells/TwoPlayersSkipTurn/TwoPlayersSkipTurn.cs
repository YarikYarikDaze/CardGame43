using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

[CreateAssetMenu(fileName = "TwoPlayersSkipTurn", menuName = "Scriptable Objects/TwoPlayersSkipTurn")]
public class TwoPlayersSkipTurn : SpellEffect
{
    void Awake()
    {
        this.duration = 1;
        this.targetsNumber = 2;
        this.spellType = 1;
        this.spellEffectsCount = 1;
        this.SelfCasted = false;
    }

    public override void InitializeSpell(int newCaster, int target, SpellManager spellManager)
    {
        this.caster = newCaster;
        this.spellManager = spellManager;
        GetNextPlayer(target);
    }

    void GetNextPlayer(int target)
    {
        this.targets = new int[targetsNumber];
        Array.Copy(spellManager.GetTwoNextPlayers(target, caster), targets, targetsNumber);
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
        this.spellManager.EndPlayerTurn(target);
        SendIdToClients();
    }   
}
