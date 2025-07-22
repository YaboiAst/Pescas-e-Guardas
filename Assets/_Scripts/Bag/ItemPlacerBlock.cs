using UnityEngine;

public class ItemPlacerBlock : MonoBehaviour
{
    private RectTransform rect;
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }
    
    public Rect GetRect()
    {
        float w = rect.rect.width;
        float h = rect.rect.height;
        Rect r = new Rect(rect.position.x + w/2, rect.position.y + h/2, w/2.2f, h/2.2f);
        return r;
    }

#if  UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return; 
        
        var r = GetRect();
        Gizmos.DrawWireCube(r.position - new Vector2(rect.rect.width/2, rect.rect.height/2), new Vector3(r.width, r.height, 1));
    }
#endif
}
