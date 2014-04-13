using UnityEngine;
using System.Collections;

public class TrainerAI : MonoBehaviour {
	Trainer trainer = null;
	Trainer enemyTrainer = null;

	Pokemon currentPokemon = null;
	Vector3 trainerPos = Vector3.zero;
	enum States {Idle, InBattle, Defeated};
	States currentState = States.Idle;

	void Start(){
		trainer = GetComponent<Trainer>();
	}

	void Update(){
		if (Player.trainer==this)	return;
		
		switch(currentState){
			
		case States.Idle:{
			Vector3 direct = Player.trainer.transform.position - transform.position;
			if (direct.sqrMagnitude<10*10 && Vector3.Dot(direct, transform.forward)>0){
				
				Dialog.inDialog = true;
				Dialog.NPCobj = gameObject;
				Dialog.NPCname = "Young Trainer";
				Dialog.text = "You're a pokemon trainer right? That means we have to battle!";
				if (Dialog.doneDialog){
					Dialog.inDialog = false;
					
					currentState = States.InBattle;
					trainerPos = transform.position - direct.normalized*10;
					enemyTrainer = Player.trainer;
				}
			}
			break;}
			
		case States.InBattle:	InBattle();	break;
			
		}
	}
	
	void InBattle(){
		Debug.Log ("TEST JIMMY InBattle v2");
		if (currentPokemon != null)
						Debug.Log ("TEST JIMMY " + currentPokemon.GetName () + " HP:" + currentPokemon.hp.ToString ());
		//move trainer to position
		Vector3 direct = trainerPos-transform.position;
		direct.y = 0;
		if (direct.sqrMagnitude>2){
			transform.rotation = Quaternion.LookRotation(direct);
			GetComponent<Animator>().SetBool("run", true);
		}else{
			if (direct.sqrMagnitude>1)	transform.position += direct;
			if (currentPokemon==null || currentPokemon.hp <= 0){
				foreach( Pokemon poke in trainer.pokemon){
					if(poke.hp>0){
						currentPokemon = poke;
						break;
					}
				}
			}

			if (currentPokemon.obj!=null){
				direct = currentPokemon.obj.transform.position-transform.position;
			}else{
				direct = enemyTrainer.transform.position-transform.position;
			}
			direct.y = 0;
			transform.rotation = Quaternion.LookRotation(direct);
			GetComponent<Animator>().SetBool("run", false);
			if (currentPokemon.obj==null && currentPokemon.hp > 0){
				trainer.ThrowPokemon(currentPokemon);
			}
			else if(currentPokemon.hp <=0){
				Player.trainer.money+=trainer.money;
				string items = "";
				for(int i=0;i<trainer.inventory.Count;i++){
					Item item = trainer.inventory[i];
					items = (items.Length > 0 ? items + "\n-" : "") + item.number.ToString()+ " "+item.type.ToString();
					Player.trainer.inventory.Add(item);
				}
				Dialog.inDialog = true;
				Dialog.NPCobj = gameObject;
				Dialog.NPCname = "Young Trainer";
				Dialog.text = "Okay, okay you win don't hit any more my pokemon!";
				Item.CombineInventory(Player.trainer.inventory);
				trainer.money=0;
				trainer.inventory.Clear();

				if (Dialog.doneDialog){
					Dialog.inDialog = false;
					currentState=States.Defeated;
				}
			}
			else{
				foreach(Pokemon poke in enemyTrainer.pokemon){
					if(poke.thrown && poke.obj!=null){
						currentPokemon.obj.transform.LookAt(poke.obj.transform);
						direct = poke.obj.transform.position-currentPokemon.obj.transform.position;
						direct.y=0;
						if(direct.sqrMagnitude > 2){	//IA pokemon go to player pokemon
							currentPokemon.obj.rigidbody.AddForce(direct * 100);
						}
						else{
							//TODO: Classify move by damage or effect to simplify IA pokemon
							bool done = false;
							foreach(Move move in currentPokemon.moves){
								if(move.moveType==MoveNames.Tackle || move.moveType==MoveNames.Scratch){
									done = currentPokemon.obj.UseMove(direct,move);
									if(done)
										break;
								}
							};
							if(!done && currentPokemon.moves.Count>0)
								currentPokemon.obj.UseMove(direct,currentPokemon.moves[0]);
						}
					}
				}
			}
		}
		
		/*if (currentPokemonObj!=null){
			PokemonTrainer pokeComp = currentPokemonObj.GetComponent<PokemonTrainer>;
			if (pokeComp!=null){
				if (Player.pokemonObj!=null){
					pokeComp.AttackEnemy(Player.pokemonObj);
				}
			}
		}*/
	}
}