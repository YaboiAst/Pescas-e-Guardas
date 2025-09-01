using UnityEngine;

public class WaterObject : MonoBehaviour
{
    private Renderer _mesh;
    private void Start()
    {
        _mesh = this.GetComponent<Renderer>();
        _mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 10000f);
    }
}
