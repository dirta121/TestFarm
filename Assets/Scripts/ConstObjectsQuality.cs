using UnityEngine;
using System;
public class ConstObjectsQuality : MonoBehaviour
{
	public string spriteSheet;
	private string qSuffix;
	void Awake()
	{
		qSuffix = GetQuality();
		ManageQuality();
	}
	private string GetQuality()
	{
		int screenH = Screen.height;
		print(screenH);
		if (screenH > 1440)
			return "4x";
		else if (screenH < 720)
			return "1x";
		else
			return "2x";
	}
	private void ManageQuality()
	{
		if (qSuffix == "1x" || qSuffix == "4x")
		{
			Sprite[] sprites = Resources.LoadAll<Sprite>(spriteSheet + "@" + qSuffix);

			if (sprites != null)
			{
				SpriteRenderer[] renderers = GameObject.FindObjectsOfType<SpriteRenderer>();
				if (renderers.Length > 0)
				{
					foreach (SpriteRenderer r in renderers)
					{
						if (r.name != null)
						{
							string spriteName = r.sprite.name;
							Sprite newSprite = Array.Find(sprites, item => item.name == spriteName);

							if (newSprite)
								r.sprite = newSprite;
						}
					}
				}
			}
		}
		Resources.UnloadUnusedAssets();
	}
}