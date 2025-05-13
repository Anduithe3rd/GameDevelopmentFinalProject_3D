using UnityEngine;

public class PlayerDisabler : MonoBehaviour
{
    public Camera playerCamera;

    public void DisableAllExceptCamera()
    {
        // Disable all components (scripts) on the player and children
        MonoBehaviour[] allScripts = GetComponentsInChildren<MonoBehaviour>();
        foreach (MonoBehaviour script in allScripts)
        {
            // Don't disable this script or the camera component
            if (script != this)
                script.enabled = false;
        }

        // Disable all GameObjects under the player EXCEPT the camera
        foreach (Transform child in transform.GetComponentsInChildren<Transform>(true))
        {
            if (child.GetComponent<Camera>() != playerCamera)
            {
                child.gameObject.SetActive(false);
            }
        }

        // Re-enable the camera and its GameObject
        if (playerCamera != null)
        {
            playerCamera.gameObject.SetActive(true);
        }
    }
}
