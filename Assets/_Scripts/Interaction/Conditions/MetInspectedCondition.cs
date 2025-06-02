using UnityEngine;

public class MetInspectedCondition : MonoBehaviour, IMet
{
    [SerializeField] private Interactable _requiredInteractable;

    public bool Met() => _requiredInteractable == null || _requiredInteractable.WasFullyInteracted;

    public string MetMessage { get; }

    public string NotMetMessage { get; }

    private void OnDrawGizmos()
    {
        Gizmos.color = Met() ? Color.green : Color.red;

        if (_requiredInteractable != null)
        {
            var dir = transform.position - _requiredInteractable.transform.position;
            Gizmos.DrawLine(transform.position, _requiredInteractable.transform.position);
            DrawArrow.ForGizmo(_requiredInteractable.transform.position, dir/2, 10);
        }
    }
}