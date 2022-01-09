using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[System.Serializable]
public class Pokemon
{
    [SerializeField] PokemonBase _base;
    [SerializeField] int level;

    public PokemonBase Base {
        get { return _base; }
    }
    public int Level {
        get { return level; }
    }
    public int HP { get; set; } //We'll import 3 possible attacks for the pokemon saved in a list to make it easier to code
    public List<Move> Moves { get; set; }
    public Dictionary<Stat, int> Stats { get; private set; } //Key is the stat and the value is the value of the attack
    public Dictionary<Stat, int> StatsBoost { get; private set; }
    public Condition Status { get; private set; }
    public Queue<string> StatusChanges { get; private set; } = new Queue<string>();
    public Move CurrentMove { get; set; }
    public bool HpChanged { get; set; }

    public void Init()
    {
        Moves = new List<Move>();

        //In our case it is necessary to find another way
        foreach (var move in Base.LearnableMoves)
        {
            if (move.Level <= Level)
            {
                Moves.Add(new Move(move.Base));
            }
            if (Moves.Count >= 3)
                break;
        }
        CalculateStats();
        HP = MaxHp;
        ResetStatBoost();
        
    }
    void ResetStatBoost()
    {
        StatsBoost = new Dictionary<Stat, int>()
        {
            {Stat.Attack, 0},
            {Stat.Defense, 0},
            {Stat.SpAttack, 0},
            {Stat.SpDefense, 0},
            {Stat.Speed, 0}
        };
    }

    void CalculateStats()
    {
        Stats = new Dictionary<Stat, int>();
        Stats.Add(Stat.Attack, Mathf.FloorToInt((Base.Attack * Level) / 100f) + 5);
        Stats.Add(Stat.Defense, Mathf.FloorToInt((Base.Defense * Level) / 100f) + 5);
        Stats.Add(Stat.SpAttack, Mathf.FloorToInt((Base.SpAttack * Level) / 100f) + 5);
        Stats.Add(Stat.SpDefense, Mathf.FloorToInt((Base.SpDefense * Level) / 100f) + 5);
        Stats.Add(Stat.Speed, Mathf.FloorToInt((Base.Speed * Level) / 100f) + 5);
        MaxHp = Mathf.FloorToInt((Base.Speed * Level) / 100f) + 10;
    }

    int GetStat(Stat stat)
    {
        int statVal = Stats[stat];

        // Apply stat boost
        int boost = StatsBoost[stat];
        var boostValues = new float[] { 1f, 1.5f, 2f, 2.5f, 3f, 3.5f, 4f };

        if (boost >= 0)
            statVal = Mathf.FloorToInt(statVal * boostValues[boost]);
        else
            statVal = Mathf.FloorToInt(statVal / boostValues[-boost]);

        return statVal;
    }
    public void ApplyBoosts(List<StatBoost> statBoosts)
    {
        foreach(var statBoost in statBoosts)
        {
            var stat = statBoost.stat;
            var boost = statBoost.boost;

            StatsBoost[stat] = Mathf.Clamp(StatsBoost[stat] + boost, -6, 6);

            if (boost > 0)
                StatusChanges.Enqueue($"{Base.Name}'s {stat} rose!");
            else
                StatusChanges.Enqueue($"{Base.Name}'s {stat} fell!");

            Debug.Log($"{stat} has been boosted to {StatsBoost}");
        }
    }
    public int Attack
    {
        get { return GetStat(Stat.Attack); } //Formula to calculate the damage of the attacks
    }
    public int Defense
    {
        get { return GetStat(Stat.Defense); } //Formula to calculate the damage of the defense
    }
    public int SpAttack
    {
        get { return GetStat(Stat.SpAttack); } //Formula to calculate the damage of the defense
    }
    public int SpDefense
    {
        get { return GetStat(Stat.SpDefense); } //Formula to calculate the damage of the defense
    }
    public int Speed
    {
        get { return GetStat(Stat.Speed); } //This formula originally uses Speed, do we need it?
    }
    public int MaxHp{ get; private set; }

    public DamageDetails TakeDamage(Move move, Pokemon attacker)
    {
        
        float critical = 1f;
        if (Random.value * 100f <= 6.25f)
            critical = 2f;

        float type = TypeChart.GetEffectiveness(move.Base.Type, this.Base.Type) * 2; //Si tinguesim 2 clases s'ha de posar aqui
        var damageDetails = new DamageDetails()
        {
            TypeEffectiveness = type,
            Critical = critical,
            Fainted = false

        };
        //float attack = (move.Base.isSpecial) ? attacker.SpAttack : attacker.Attack; //Volem atacs especials?? 
        float modifiers = Random.Range(0.85f, 1f) * type * critical;
        float a = (2 * attacker.Level + 10) / 250f;
        float d = a * move.Base.Power * ((float)attacker.Attack / Defense) + 2;
        int damage = Mathf.FloorToInt(d * modifiers);

        UpdateHP(damage);
        return damageDetails;
    }
    public void UpdateHP(int damage)
    {
        HP = Mathf.Clamp(HP - damage,0,MaxHp);
        HpChanged = true;
    }
    public void SetStatus(ConditionID conditionID)
    {
        Status = ConditionsDB.Conditions[conditionID];
        StatusChanges.Enqueue($"{Base.Name} {Status.StartMessage}");
    }

    public Move GetRandomMove()
    {
        int r = Random.Range(0, Moves.Count);
        return Moves[r];
    }
    public void OnAfterTurn()
    {
        Status?.OnAfterTurn?.Invoke(this); //Only invoke if it is not null
    }
    public void OnBattleOver()
    {
        ResetStatBoost();
    }
}

public class DamageDetails
{
    public bool Fainted { get; set; }
    public float Critical { get; set; }
    public float TypeEffectiveness { get; set; }
}
