using System.Collections;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [Header("Sway Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float sensitivityMultiplier = 1f;

    [Header("Recoil Settings")]
    [SerializeField] private float recoilAmount = 1f;        // How much the weapon recoils
    [SerializeField] private float recoilSpeed = 5f;         // How fast the recoil resets
    [SerializeField] private float recoilRotationAmount = 5f; // How much the weapon rotates during recoil

    [Header("Loading Animation Settings")]
    [SerializeField] private float loadingPullAmount = 2f;   // How much the weapon is pulled upwards during reload
    [SerializeField] private float loadingSpeed = 5f;        // How fast the weapon returns to its original position after reload

    private Quaternion refRotation;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private bool isReloading = false;

    private void Start()
    {
        // Store the original position and rotation of the weapon holder
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
    }

    private void Update()
    {
        if (!isReloading)
        {
            HandleSway();
            HandleRecoil();
        }
    }

    private void HandleSway()
    {
        // Get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * sensitivityMultiplier;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensitivityMultiplier;

        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, originalRotation * targetRotation, speed * Time.deltaTime);
    }

    private void HandleRecoil()
    {
        // Smoothly return the weapon to its original position and rotation after recoil
        transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, recoilSpeed * Time.deltaTime);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, originalRotation, recoilSpeed * Time.deltaTime);
    }

    public void ApplyRecoil()
    {
        // Apply recoil by moving the weapon holder upwards and rotating slightly
        transform.localPosition -= Vector3.up * recoilAmount;
        transform.localRotation *= Quaternion.Euler(Vector3.left * recoilRotationAmount);
    }

    public IEnumerator PlayLoadingAnimation()
    {
        isReloading = true; // Disable sway and recoil during reload

        // Move the weapon holder upwards over time to simulate pulling
        Vector3 targetPosition = originalPosition + Vector3.up * loadingPullAmount;
        while (Vector3.Distance(transform.localPosition, targetPosition) > 0.01f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, loadingSpeed * Time.deltaTime);
            yield return null;
        }

        // Smoothly return to the original position after pulling upwards
        while (Vector3.Distance(transform.localPosition, originalPosition) > 0.01f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, loadingSpeed * Time.deltaTime);
            yield return null;
        }

        // Ensure weapon is exactly at the original position
        transform.localPosition = originalPosition;
        transform.localRotation = originalRotation;

        isReloading = false; // Re-enable sway and recoil after reload
    }
}
