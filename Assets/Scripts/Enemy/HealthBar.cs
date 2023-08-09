using UnityEngine;

//Got from https://www.stevestreeting.com/2019/02/22/enemy-health-bars-in-1-draw-call-in-unity/
public class HealthBar : MonoBehaviour {

    MaterialPropertyBlock matBlock;
    MeshRenderer meshRenderer;
    Camera mainCamera;
    HealthManager damageable;

    private void Awake() {
        meshRenderer = GetComponent<MeshRenderer>();
        matBlock = new MaterialPropertyBlock();
        damageable = GetComponentInParent<HealthManager>();
    }

    private void Start() {
        mainCamera = Camera.main;
    }

    private void Update() {
        if (damageable.GetHealth() < damageable.GetMaxHealth()) {
            meshRenderer.enabled = true;
            AlignCamera();
            UpdateParams();
        } else {
            meshRenderer.enabled = false;
        }
    }

    private void UpdateParams() {
        meshRenderer.GetPropertyBlock(matBlock);
        matBlock.SetFloat("_Fill", damageable.GetHealth() / (float)damageable.GetMaxHealth());
        meshRenderer.SetPropertyBlock(matBlock);
    }

    private void AlignCamera() {
        if (mainCamera != null) {
            var camXform = mainCamera.transform;
            var forward = transform.position - camXform.position;
            forward.Normalize();
            var up = Vector3.Cross(forward, camXform.right);
            transform.rotation = Quaternion.LookRotation(forward, up);
        }
    }

}