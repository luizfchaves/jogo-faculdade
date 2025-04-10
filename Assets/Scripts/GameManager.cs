using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {

    // [SerializeField] private GameObject turretPrefab; 

    protected int coins = 10;
    public GameObject turretSelected;
    public GameObject turretPreview;

    public Material previewMaterial;

    // Text component TMP for coins
    public GameObject coinsTextObject;
    private TextMeshProUGUI coinsTextMeshPro;

    public void handleMinionDeath(){
        // add 10 coins to gameManager
        coins += 10;
    }
    void Start() {
        coinsTextMeshPro = coinsTextObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update() {
        coinsTextMeshPro.text = "Coins: " + coins.ToString();

        // if(turretSelected){
        //     if(!turretPreview){
        //         turretPreview = Instantiate(turretSelected,Vector3.zero, Quaternion.identity);

        //         // put a opaque material on the turret prefab
        //         MeshRenderer[] renderers = turretPreview.GetComponentsInChildren<MeshRenderer>();
        //         foreach (MeshRenderer renderer in renderers) {
        //             Material[] materials = renderer.materials;
        //             for (int i = 0; i < materials.Length; i++) {
        //                 materials[i] = previewMaterial;
        //             }
                    
        //             renderer.materials = materials;
        //         }

        //     }
        //     // Deactivate the turret prefab to not be visible in the scene
        //     // turretPreview.SetActive(false);
        


        //     //Preview the turret prefab with a green outline on the mouse position
        //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //     RaycastHit hit;
        //     // check if hitted has a terrain tag
        //     if (Physics.Raycast(ray, out hit)) {
        //                         if (hit.collider.CompareTag("Terrain")) {

        //         // check if the hit object has a tag "Terrain"
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



                if(turretSelected && hit.collider.CompareTag("Terrain")){
                    Instantiate(turretSelected, hit.point, Quaternion.identity);
                    turretSelected = null;
                }
            }
        }
        
    }
}
