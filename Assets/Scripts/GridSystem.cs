using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    public GameObject Generator;
    public GameObject Pulsar;
    public GameObject Sentry;
    public float gridSize = 1f;
    public GameObject highlightPrefab;

    private GameObject objectToPlace;
    private GameObject ghostObject;
    private GameObject highlightQuad;
    private HashSet<Vector3> occupiedPositions = new HashSet<Vector3>();
    private float planeY;
    private float highlightOffset = 0.01f;

    private void Start()
    {
        planeY = transform.position.y;
        CreateGhostObject();
        CreateHighlightQuad();
    }

    private void Update()
    {
        UpdateGhostPosition();

        if (Input.GetMouseButtonDown(0) && objectToPlace != null)
            PlaceObject();

        if (Input.GetKeyDown(KeyCode.Alpha1))
            SwitchTo(Generator);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            SwitchTo(Pulsar);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            SwitchTo(Sentry);


        if (Input.GetKeyDown(KeyCode.Backspace))
            ClearSelection();
    }

    void SwitchTo(GameObject prefab)
    {
        objectToPlace = prefab;
        Destroy(ghostObject);
        CreateGhostObject();
    }

    void ClearSelection()
    {
        objectToPlace = null;
        Destroy(ghostObject);
    }

    void CreateGhostObject()
    {
        if (objectToPlace == null) return;
        ghostObject = Instantiate(objectToPlace);
        Renderer[] renderers = ghostObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers)
        {
            Material m = r.material;
            Color c = m.color; c.a = 0.5f; m.color = c;
            m.SetFloat("_Mode", 2);
            m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            m.SetInt("_ZWrite", 0);
            m.DisableKeyword("_ALPHATEST_ON");
            m.EnableKeyword("_ALPHABLEND_ON");
            m.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            m.renderQueue = 3000;
        }

        // Disable all scripts on the ghost object
        MonoBehaviour[] scripts = ghostObject.GetComponentsInChildren<MonoBehaviour>();
        foreach (var script in scripts)
        {
            script.enabled = false;
        }

        // Disable all colliders on the ghost object
        Collider[] colliders = ghostObject.GetComponentsInChildren<Collider>();
        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }
    }

    void CreateHighlightQuad()
    {
        if (highlightPrefab != null)
            highlightQuad = Instantiate(highlightPrefab);
        else
        {
            highlightQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            Destroy(highlightQuad.GetComponent<Collider>());
            var mat = new Material(Shader.Find("Unlit/Color"));
            mat.color = new Color(0.8f, 0.8f, 0.8f, 0.3f);
            highlightQuad.GetComponent<Renderer>().material = mat;
        }
        highlightQuad.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        highlightQuad.transform.localScale = new Vector3(gridSize, gridSize, 1f);
    }

    void UpdateGhostPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 snapped = new Vector3(
                Mathf.Round(hit.point.x / gridSize) * gridSize,
                planeY,
                Mathf.Round(hit.point.z / gridSize) * gridSize
            );

            highlightQuad.SetActive(true);
            highlightQuad.transform.position = new Vector3(snapped.x, planeY + highlightOffset, snapped.z);

            bool occupied = occupiedPositions.Contains(snapped);
            if (ghostObject != null)
            {
                if (occupied)
                {
                    ghostObject.SetActive(false);
                }
                else
                {
                    ghostObject.SetActive(true);
                    ghostObject.transform.position = snapped;
                    SetGhostColor(new Color(1f, 1f, 1f, 0.5f));
                }
            }
        }
        else
        {
            if (highlightQuad != null) highlightQuad.SetActive(false);
            if (ghostObject != null) ghostObject.SetActive(false);
        }
    }


    void SetGhostColor(Color color)
    {
        foreach (Renderer r in ghostObject.GetComponentsInChildren<Renderer>())
            r.material.color = color;
    }

    void PlaceObject()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Snap to grid
            Vector3 snapped = new Vector3(
                Mathf.Round(hit.point.x / gridSize) * gridSize,
                planeY,
                Mathf.Round(hit.point.z / gridSize) * gridSize
            );

            // Only place if that snapped cell is not already occupied
            if (!occupiedPositions.Contains(snapped))
            {
                Instantiate(objectToPlace, snapped, Quaternion.identity);
                occupiedPositions.Add(snapped);
            }
        }
    }

}
