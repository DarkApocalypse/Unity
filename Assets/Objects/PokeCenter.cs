using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PokeCenter : MonoBehaviour {
	public void Update(){
		Bounds b = gameObject.collider.bounds;
		GameObject[] objs = GameObject.FindGameObjectsWithTag ("trainer");
		for (int i=0; i<objs.Length; i++) {
			if(b.Contains(objs[i].collider.bounds.center)){
				Debug.Log ("A trainer in PokeCenter!");
				Trainer trainer = (Trainer)objs[i].GetComponent("Trainer");
				if(trainer!=null){
					Debug.Log ("Healing his pokemon");
					HealPokemon(trainer);
				}
			}
		}
	}
	public static void HealPokemon() {
		foreach (var pokemon in Player.trainer.pokemon) {
			//pokemon.hp = pokemon.health;
			pokemon.hp = 1;
		}
	}
	public static void HealPokemon(Trainer trainer) {
		foreach (var pokemon in trainer.pokemon) {
			//pokemon.hp = pokemon.health;
			pokemon.hp = 1;
		}
	}
}