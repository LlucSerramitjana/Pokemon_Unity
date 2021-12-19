using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Pokemon", menuName = "Pokemon/Create new pokemon")]
public class PokemonBase : ScriptableObject
{
    [SerializeField] string name;
    [SerializeField] Sprite frontSprite;
    [SerializeField] Sprite backSprite;
    [SerializeField] PokemonType type;
    [SerializeField] int maxHp;
    [SerializeField] int attack;
    [SerializeField] int defense;
}

public enum PokemonType
{
    Normal,
    Fire,
    Water,
    Grass,
    Ice,
    Rock,
    None
}
