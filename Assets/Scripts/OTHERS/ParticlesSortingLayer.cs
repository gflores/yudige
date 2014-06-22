using UnityEngine;
using System.Collections;

public class ParticlesSortingLayer : MonoBehaviour {
	public string sorting_layer_name;
	public int sorting_order;
	// Use this for initialization
	void Start () {
		particleSystem.renderer.sortingLayerName = sorting_layer_name;
		particleSystem.renderer.sortingOrder = sorting_order;
	}
}
