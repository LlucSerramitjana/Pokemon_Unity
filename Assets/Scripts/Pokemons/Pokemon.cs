using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public int MaxHP { get; set; }
    public List<Move> Moves { get; set; }

    public void Init()
    {
        HP = _base.getMaxHP();
        MaxHP = _base.getMaxHP();
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
    }

    public int Attack
    {
        get { return Mathf.FloorToInt((Base.Attack * Level) / 100f) + 5; } //Formula to calculate the damage of the attacks
    }
    public int Defense
    {
        get { return Mathf.FloorToInt((Base.Defense * Level) / 100f) + 5; } //Formula to calculate the damage of the defense
    }
    public int Speed
    {
        get { return Mathf.FloorToInt((Base.Speed * Level) / 100f) + 5; } //This formula originally uses Speed, do we need it?
    }
    public float MaxHp
    {
        get { return Mathf.FloorToInt((Base.Speed * Level) / 100f) + 10; } //This formula originally uses Speed, do we need it?
    }

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

        HP -= damage;
        if (HP <= 0)
        {
            HP = 0;
            damageDetails.Fainted = true;
        }
        return damageDetails;
    }

    public Move GetRandomMove()
    {
        int r = Random.Range(0, Moves.Count);
        return Moves[r];
    }
}

public class DamageDetails
{
    public bool Fainted { get; set; }
    public float Critical { get; set; }
    public float TypeEffectiveness { get; set; }
}
