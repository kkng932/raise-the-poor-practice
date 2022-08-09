using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class ViewController : MonoBehaviour
{
    // Rect Transform ������Ʈ�� ĳ���Ѵ�
    private RectTransform cachedRectTransform;
    public RectTransform CachedRectTransform
    {
        get
        {
            if (cachedRectTransform == null)
            {
                cachedRectTransform = GetComponent<RectTransform>();
            }
            return cachedRectTransform;
        }
    }
    // ���� Ÿ��Ʋ�� �����ͼ� �����ϴ� ������Ƽ
    public virtual string Title { get { return ""; } set { } }


}
