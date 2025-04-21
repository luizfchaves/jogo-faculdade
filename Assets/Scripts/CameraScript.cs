using UnityEngine;
using TMPro;

public class CameraScript : MonoBehaviour {
  public float speed = 10f;
  public float shiftMultiplier = 2f;
  
  public float scrollSpeed = 10000f;

  public float currentZoom = 0f;
  public float minZoom = -2f;
  public float maxZoom = 20f;


  void handleZoom(float speed){
    if(Input.GetAxis("Mouse ScrollWheel") != 0f){
      Debug.Log("Current zoom: " + currentZoom);
      currentZoom -= Input.GetAxis("Mouse ScrollWheel") * speed;
      if(currentZoom < minZoom || currentZoom > maxZoom){
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
        return;
      }
      Vector3 newPosition = transform.position + transform.forward * Input.GetAxis("Mouse ScrollWheel") * speed;
      transform.position = newPosition;
    }
  }

  void handleMovement(float speed){
    // Here the transform should move the camera on global coordinates, only change X and z axis
    Vector3 newPosition = transform.position + new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * speed * Time.deltaTime;

    transform.position = newPosition;
  }

  void Update(){
    bool applyMultiplier = false;
    if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
      applyMultiplier = true;
    }

    handleZoom(applyMultiplier ? scrollSpeed * shiftMultiplier : scrollSpeed);
    handleMovement(applyMultiplier ? speed * shiftMultiplier : speed);

  }
}
