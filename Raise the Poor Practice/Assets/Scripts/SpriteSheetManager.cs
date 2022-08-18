using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSheetManager
{
    // ��������Ʈ ��Ʈ�� ���Ե� ��������Ʈ�� ĳ���ϴ� ��ųʸ�
    private static Dictionary<string, Dictionary<string, Sprite>> spriteSheets = new Dictionary<string, Dictionary<string, Sprite>>();

    // ��������Ʈ ��Ʈ�� ���Ե� ��������Ʈ�� �о� �鿩 ĳ���ϴ� �޼���
    public static void Load(string path)
    {
        if (!spriteSheets.ContainsKey(path))
        {
            spriteSheets.Add(path, new Dictionary<string, Sprite>());
        }

        // ��������Ʈ�� �о� �鿩 �̸��� ������� ĳ���Ѵ�.
        Sprite[] sprites = Resources.LoadAll<Sprite>(path);
        foreach (Sprite sprite in sprites)
        {
            if (!spriteSheets[path].ContainsKey(sprite.name))
            {

                spriteSheets[path].Add(sprite.name, sprite);
            }
        }
    }
    // ��������Ʈ �̸��� ���� ��������Ʈ ��Ʈ�� ���Ե� ��������Ʈ�� ��ȯ�ϴ� �޼���
    public static Sprite GetSpriteByName(string path, string name)
    {
        if (spriteSheets.ContainsKey(path) && spriteSheets[path].ContainsKey(name))
        {
            return spriteSheets[path][name];
        }
        return null;
    }

}
