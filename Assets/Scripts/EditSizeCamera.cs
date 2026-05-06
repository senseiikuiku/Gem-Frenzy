using UnityEngine;

public class EditSizeCamera : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject[] Block_BG;

    private void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

    }

    private void UpdateSizeSpriteRenderer(float size = 7f)
    {
        for (int i = 0; i < Block_BG.Length; i++)
        {
            Block_BG[i].GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Sliced;
            if (Block_BG[i] != null && i == 0 && Block_BG[i].GetComponent<SpriteRenderer>() != null)
            {
                Block_BG[i].GetComponent<SpriteRenderer>().size = new Vector2(size, size);
            }
            else
            {
                Block_BG[i].GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Sliced;
                Block_BG[i].GetComponent<SpriteRenderer>().size = new Vector2(Block_BG[i - 1].GetComponent<SpriteRenderer>().size.x + 0.2f, Block_BG[i - 1].GetComponent<SpriteRenderer>().size.y + 0.2f);
            }
        }
    }

    private void UpdatePositionBlock_BG(Vector2 pos)
    {
        foreach (GameObject block in Block_BG)
        {
            block.transform.localPosition = new Vector3(pos.x, pos.y, block.transform.localPosition.z);
        }
    }

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GetValueSizeCamera(mainCamera);
        }

        UpdateScaleBlock_BG();
    }

    private void UpdateScaleBlock_BG()
    {
        switch (mainCamera.orthographicSize)
        {
            case 4f:
                UpdateSizeSpriteRenderer(7f);
                UpdatePositionBlock_BG(new Vector2(0, 0));
                break;
            case 5f:
                UpdateSizeSpriteRenderer(8f);
                UpdatePositionBlock_BG(new Vector2(0.5f, 0.5f));
                break;
            case 6f:
                UpdateSizeSpriteRenderer(10f);
                break;
            default:
                UpdateSizeSpriteRenderer(7f);
                break;
        }
    }
}
