using System.Collections;
using UnityEngine;

public class InteractableTerrain : MonoBehaviour {

  private Renderer _renderer;
  public Color hoverColor = Color.red;
  private Color startColor;

  void Start() {
    _renderer = GetComponent<Renderer>();
    startColor = _renderer.material.color;
  }

  void OnMouseEnter() {
    _renderer.material.color = hoverColor;
  }

  void OnMouseExit() {
    _renderer.material.color = startColor;
  }
}