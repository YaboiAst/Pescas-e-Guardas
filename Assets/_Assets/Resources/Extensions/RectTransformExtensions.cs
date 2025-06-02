using UnityEngine;

public static class RectTransformExtensions
{
    public static Rect GetWorldRect(this RectTransform rectTransform)
    {
        var corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        Vector2 min = corners[0];
        Vector2 max = corners[2];
        Vector2 size = max - min;
 
        return new Rect(min, size);
    }
    
    public static bool FullyContains (this RectTransform rectTransform, RectTransform other)
    {       
        var rect = rectTransform.GetWorldRect();
        var otherRect = other.GetWorldRect();

        // Now that we have the world space rects simply check
        // if the other rect lies completely between min and max of this rect
        return rect.xMin <= otherRect.xMin 
               && rect.yMin <= otherRect.yMin 
               && rect.xMax >= otherRect.xMax 
               && rect.yMax >= otherRect.yMax;
    }
    
    public static Vector2 FullyContainsMoveInside_SSC(this RectTransform rectTransform, RectTransform other)
    {
        var rect = rectTransform.GetWorldRect();
        var otherRect = other.GetWorldRect();
    
        if (!FullyContains(rectTransform, other))
        {
            if (rect.xMin >= otherRect.xMin)
            {
                var distance = 0 + (other.rect.width/2);
                if (rect.yMin >= otherRect.yMin)
                    return new Vector2(distance, 0 + (other.rect.height / 2));
                if (rect.yMax <= otherRect.yMax)
                    return new Vector2(distance, rectTransform.rect.height - (other.rect.height / 2));
    
                return new Vector2(distance, other.anchoredPosition.y);
            }
    
            if (rect.xMax <= otherRect.xMax)
            {
                var distance = rectTransform.rect.width - (other.rect.width/2);
                if (rect.yMin >= otherRect.yMin)
                    return new Vector2(distance, 0 + (other.rect.height / 2));
                if (rect.yMax <= otherRect.yMax)
                    return new Vector2(distance, rectTransform.rect.height - (other.rect.height / 2));
                
                return new Vector2(distance, other.anchoredPosition.y);
            }
    
            if (rect.yMin >= otherRect.yMin)
                return new Vector2(other.anchoredPosition.x, 0 + (other.rect.height / 2));
    
            if (rect.yMax <= otherRect.yMax)
                return new Vector2(other.anchoredPosition.x, rectTransform.rect.height - (other.rect.height / 2));
        }
        
        return other.anchoredPosition;
    }
    
    public static Vector2 FullyContainsMoveInside_SSO(this RectTransform rectTransform, RectTransform other)
    {
        var rect = rectTransform.GetWorldRect();
        var otherRect = other.GetWorldRect();
    
        if (!FullyContains(rectTransform, other))
        {
            if (rect.xMin >= otherRect.xMin)
            {
                if (rect.yMin >= otherRect.yMin)
                    return new Vector2(rect.xMin + (otherRect.width / 2), rect.yMin + (otherRect.height / 2));
                if (rect.yMax <= otherRect.yMax)
                    return new Vector2(rect.xMin + (otherRect.width / 2), rect.yMax - (otherRect.height / 2));
    
                return new Vector2(rect.xMin + (otherRect.width / 2), other.position.y);
            }
    
            if (rect.xMax <= otherRect.xMax)
            {
                if (rect.yMin >= otherRect.yMin)
                    return new Vector2(rect.xMax - (otherRect.width / 2), rect.yMin + (otherRect.height / 2));
                if (rect.yMax <= otherRect.yMax)
                    return new Vector2(rect.xMax - (otherRect.width / 2), rect.yMax - (otherRect.height / 2));
                
                return new Vector2(rect.xMax - (otherRect.width / 2), other.position.y);
            }
    
            if (rect.yMin >= otherRect.yMin)
                return new Vector2(other.position.x, rect.yMin + (otherRect.height / 2));
    
            if (rect.yMax <= otherRect.yMax)
                return new Vector2(other.position.x, rect.yMax - (otherRect.height / 2));
        }
        
        
        return other.position;
    }
    
    
    
}
