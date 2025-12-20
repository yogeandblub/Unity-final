using UnityEngine;

public class WristMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform head;
    [SerializeField] private Transform menuRoot; // parent of the UI (WristCanvas or a holder)

    [Header("Follow")]
    [SerializeField] private float distance = 0.7f;
    [SerializeField] private float heightOffset = -0.1f;
    [SerializeField] private float smooth = 10f;

    public bool Visible => menuRoot && menuRoot.gameObject.activeSelf;

    private void Reset()
    {
        menuRoot = transform;
    }

    private void LateUpdate()
    {
        if (!menuRoot || !menuRoot.gameObject.activeSelf || !head) return;

        Vector3 targetPos = head.position + head.forward * distance + Vector3.up * heightOffset;
        Quaternion targetRot = Quaternion.LookRotation(head.forward, Vector3.up);

        menuRoot.position = Vector3.Lerp(menuRoot.position, targetPos, Time.deltaTime * smooth);
        menuRoot.rotation = Quaternion.Slerp(menuRoot.rotation, targetRot, Time.deltaTime * smooth);
    }

    public void Toggle()
    {
        if (!menuRoot) return;
        menuRoot.gameObject.SetActive(!menuRoot.gameObject.activeSelf);
    }

    public void SetVisible(bool visible)
    {
        if (!menuRoot) return;
        menuRoot.gameObject.SetActive(visible);
    }
}
