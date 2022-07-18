using UnityEngine;

public class TextureGenerator : MonoBehaviour
{
    [SerializeField] private Vector2 size;
    [SerializeField] private float plankWidth = 1;
    [SerializeField] private Vector2 plankLengthMinMax;
    [SerializeField] private int seed;
	[SerializeField] private Color[] cols;
	[SerializeField] private Material material;
	[SerializeField] private float noise;
	[SerializeField] private bool autoUpdate;
	
	private Transform holder;
	private bool needsUpdate;

	private void Update()
	{
		if (needsUpdate && autoUpdate)
		{
			needsUpdate = false;

			var o = Random.state;
			Generate();
			Random.state = o;
		}
	}

	[ContextMenu("Generate texture")]
	public void Generate()
	{
		if (transform.childCount > 0)
		{
			DestroyImmediate(transform.GetChild(0).gameObject);
		}
		Random.InitState(seed);
		holder = new GameObject("Holder").transform;
		holder.parent = transform;
		float currX = transform.position.x - size.x / 2 + plankWidth / 2;
		float startY = transform.position.z - size.y / 2;
		float currY = startY;
		float endY = transform.position.z + size.y / 2;

		while (currX < transform.position.x + size.x / 2 - 0.01f)
		{
			while (currY <= endY - 0.01f)
			{
				GameObject plank = GameObject.CreatePrimitive(PrimitiveType.Cube);
				DestroyImmediate(plank.GetComponent<BoxCollider>());
				plank.GetComponent<MeshRenderer>().sharedMaterial = new Material(material);
				float r = (Random.value - 0.5f) * noise;
				Color col = cols[Random.Range(0, cols.Length)] + new Color(r, r, r);
				plank.GetComponent<MeshRenderer>().sharedMaterial.color = col;
				plank.transform.parent = holder;
				float length = Random.Range(plankLengthMinMax.x, plankLengthMinMax.y);
				length = Mathf.Min(length, endY - currY);

				plank.transform.position = new Vector3(currX, 0, currY + length / 2);
				plank.transform.localScale = new Vector3(plankWidth, 0.01f, length);
				currY += length;
			}
			currX += plankWidth;
			currY = startY;
		}

	}

	private void OnValidate()
	{
		needsUpdate = true;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(transform.position, new Vector3(size.x, 0, size.y));
	}
}
