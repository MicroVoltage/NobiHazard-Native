using UnityEngine;
using System.Collections;

public class HealthController : MonoBehaviour {
	public float maxHealth = 100.0f;
	public float health = 100.0f;

	public GameObject[] DeadEvents;

	public void ApplyDamage (float damage) {
		Debug.Log(gameObject.name + " " + health);
		SubHealth(damage);
		if (!HasHealth()) {
			for (int i=0; i<DeadEvents.Length; i++) {
				DeadEvents[i].SendMessage("OnChainEnter", SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	public void AddHealth (float value) {
		health += value;
		if (health > maxHealth) {
			health = maxHealth;
		}
	}
	public bool SubHealth (float value) {
		health -= value;
		if (!HasHealth()) {
			health = 0;
		}
		return HasHealth();
	}
	public void CleanHealth (int itemIndex) {
		health = 0;
	}
	public bool HasHealth () {
		return health > 0;
	}
	public float HealthValue () {
		return health;
	}
}
