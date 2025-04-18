using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {

    // [SerializeField] private GameObject turretPrefab; 

    protected int coins = 20;
    public GameObject turretSelected;
    public GameObject turretPreview;

    public Material previewMaterial;

    public GameObject coinsTextObject;
    private TextMeshProUGUI coinsTextMeshPro;

    public void handleMinionDeath(){
        // add 10 coins to gameManager
        coins += 10;
    }
    void Start() {
        coinsTextMeshPro = coinsTextObject.GetComponent<TextMeshProUGUI>();
    }

    void Update() {
        coinsTextMeshPro.text = "Coins: " + coins.ToString();

        // if(turretSelected){
        //     if(!turretPreview){
        //         turretPreview = Instantiate(turretSelected,Vector3.zero, Quaternion.identity);

        //         MeshRenderer[] renderers = turretPreview.GetComponentsInChildren<MeshRenderer>();
        //         foreach (MeshRenderer renderer in renderers) {
        //             Material[] materials = renderer.materials;
        //             for (int i = 0; i < materials.Length; i++) {
        //                 materials[i] = previewMaterial;
        //             }
                    
        //             renderer.materials = materials;
        //         }

        //     }
        //     // turretPreview.SetActive(false);
        


        //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //     RaycastHit hit;
        //     if (Physics.Raycast(ray, out hit)) {
        //                         if (hit.collider.CompareTag("Terrain")) {

        //         if (hit.collider.CompareTag("Terrain")) {


        //         turretPreview.transform.position = hit.point;
        //         turretPreview.SetActive(true);
        //         }
        //                         }
        //     }
        //     else {
        //         // turretPreview.SetActive(false);
        //     }
        // }
        
        if (Input.GetMouseButtonDown(0)){

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) { // Basicamente clicou em algo
                // Debug.Log("Hit: " + hit.collider.name);

                // check if hit has a InteractableTerrain component
                InteractableTerrain interactableTerrain = hit.collider.GetComponent<InteractableTerrain>();
                if(turretSelected && interactableTerrain && coins >= 10){
                    coins -= 10;
                    // get position of the interactable terrain
                    Vector3 position = hit.transform.position;

                    // add 1.25 to Y axis
                    position.y += 1.25f;
                    // Debug.Log("Hit point: " + position);
                    Instantiate(turretSelected, position, Quaternion.identity);
                    // turretSelected = null;
                    // Debug.Log("Instantiated turret: " + turretSelected.name);
                }
            }
        }
        
    }
}
