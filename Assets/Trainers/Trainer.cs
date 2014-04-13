using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Trainer : MonoBehaviour {
	public List<Pokemon> pokemon = new List<Pokemon>();
	public List<Item> inventory = new List<Item>();
	public int money = 500;

	Vector3 velocity = Vector3.zero;

	void Start(){
		//kanto starters, why not
		if (gameObject.name.Equals("Player")) {
			pokemon.Add (new Pokemon (1, true));
			pokemon.Add (new Pokemon (4, true));
			pokemon.Add (new Pokemon (7, true));
			Pokedex.states [1] = Pokedex.State.Captured;
			Pokedex.states [4] = Pokedex.State.Captured;
			Pokedex.states [7] = Pokedex.State.Captured;
			inventory.Add (new Item (ItemTypes.Pokeball, 5));
			inventory.Add (new Item (ItemTypes.Potion, 2));
		} else {
			List<int> availablePokemon = new List<int>();
			availablePokemon.Add(1);
			availablePokemon.Add(4);
			availablePokemon.Add(7);
			availablePokemon.Add(19);
			int nbsPokemons = Random.Range(1,6);
			for(int i=0;i<nbsPokemons;i++){
				pokemon.Add(new Pokemon(availablePokemon[Random.Range(0,availablePokemon.Count)],false));
			}
			int nbsPotions = Random.Range(0,3);
			inventory.Add(new Item(ItemTypes.Potion,nbsPotions));
			money = 250 * nbsPokemons + 150 * nbsPotions;
		}
	}

	void Update(){
		//inventoryMGR
		for(int i=0; i<inventory.Count; i++){
			if (inventory[i].number<=0)	inventory.Remove(inventory[i]);
		}
	}

	public void ThrowPokemon(Pokemon poke){
		if (poke.thrown)	return;
		poke.thrown = true;
		GameObject ball = (GameObject)Instantiate(Resources.Load("Pokeball"));
		ball.transform.position = transform.position;
		ball.rigidbody.AddForce( (transform.forward*2+ transform.up)*400 );
		ball.GetComponent<Pokeball>().pokemon = poke;
		ball.GetComponent<Pokeball>().trainer = this;

		//gamegui.SetChatWindow(ball.GetComponent<Pokeball>().pokemon.GetName() + "! I choose you!");
	}

	public void SetVelocity(Vector3 vel){
		velocity = vel;
		Animator ani = GetComponent<Animator>();

		if (vel.magnitude>0.1f){
			ani.SetBool("run",true);
			transform.rotation = Quaternion.LookRotation(vel);
		}else{
			ani.SetBool("run",false);
		}
	}
}