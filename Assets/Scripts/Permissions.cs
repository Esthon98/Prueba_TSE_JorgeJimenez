using UnityEngine;
using UnityEngine.Android;
using TMPro;

public class Permissions : MonoBehaviour
{
    // Assign a TextMeshProUGUI element to display permission status.
    public TextMeshProUGUI permissionStatusText;

    void Start() => CheckLocationPermission();

    // Check location permission.
    void CheckLocationPermission()
    {
        if (Permission.HasUserAuthorizedPermission(Permission.FineLocation)) UpdateUI("Location Permission: GRANTED");
        else
        {
            UpdateUI("Location Permission: NOT GRANTED");
            RequestLocationPermission();
        }
    }

    // Request location permission.
    void RequestLocationPermission()
    {
        Permission.RequestUserPermission(Permission.FineLocation);
        StartCoroutine(CheckPermissionAfterDelay());
    }

    // Verify permission status.
    System.Collections.IEnumerator CheckPermissionAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        if (Permission.HasUserAuthorizedPermission(Permission.FineLocation)) UpdateUI("Location Permission: GRANTED");
        else UpdateUI("Location Permission: DENIED");
    }

    // Update UI.
    void UpdateUI(string message) { if (permissionStatusText != null) permissionStatusText.text = message; }
}