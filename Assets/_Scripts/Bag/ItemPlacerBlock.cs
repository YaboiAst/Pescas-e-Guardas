using UnityEngine;

public class ItemPlacerBlock : MonoBehaviour
{
    private RectTransform rect;
    public Rect GetRect()
    {
        float w = rect.rect.width;
        float h = rect.rect.height;
        Rect r = new Rect(rect.position.x, rect.position.y, w/2.2f, h/2.2f);
        return r;
    }
    
    
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return; 
        
        var r = GetRect();
        Gizmos.DrawWireCube(r.position, new Vector3(r.width, r.height, 1));
    }

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }
}
