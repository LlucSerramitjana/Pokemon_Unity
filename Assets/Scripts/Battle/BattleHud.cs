using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HPBar hpBar;
    [SerializeField] Pokemon _pokemon;

    public void SetData(Pokemon pokemon)
    {
        nameText.text = pokemon.Base.Name;
        levelText.text = "Lvl" + pokemon.Level;
        _pokemon = pokemon;
        float calc = (float)pokemon.HP / pokemon.MaxHP;
        hpBar.SetHP(calc);
    }
    public IEnumerator UpdateHP()
    {
        float calc = (float) _pokemon.HP / _pokemon.MaxHP;

        yield return hpBar.SetHPSmooth(calc);

    }
}
