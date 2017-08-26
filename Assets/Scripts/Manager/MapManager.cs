using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MapManager : MonoBehaviour {
	private static MapManager instance;

	private SpriteRenderer mapRenderer;
	private LimitArea limitArea;

	void Awake () {
		if (instance) {
			Debug.LogError("Error: already instance class. MapManager.cs");
			return;
		}

		instance = this;

		mapRenderer = GetComponent<SpriteRenderer>();
		Vector2 min = mapRenderer.bounds.min;
		Vector2 max = mapRenderer.bounds.max;

		limitArea = new LimitArea(min, max);
	}

	public static LimitArea LimitArea { 
		get { return instance.limitArea; }
	}
}
