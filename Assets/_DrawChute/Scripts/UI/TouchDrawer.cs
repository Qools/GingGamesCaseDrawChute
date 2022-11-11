using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class TouchDrawer : MonoBehaviour
{
    Coroutine drawing;

    public GameObject lineRendererPrefab;

    private LineRenderer line;

    private Mesh lineMesh;

    public GraphicRaycaster drawZoneRayCaster;
    public EventSystem eventSystem;
    private PointerEventData pointerEventData;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            pointerEventData = new PointerEventData(eventSystem);
            pointerEventData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            drawZoneRayCaster.Raycast(pointerEventData, results);

            foreach (var result in results)
            {
                if (result.gameObject.CompareTag(PlayerPrefKeys.drawZone))
                {
                    if (lineMesh != null)
                    {
                        lineMesh.Clear();
                    }

                    StartLine();
                }
            }

           
        }
        else if (Input.GetMouseButtonUp(0))
        {
            FinishLine();
        }
    }

    void StartLine()
    {
        if (drawing != null)
        {
            StopCoroutine(drawing);
        }

        drawing = StartCoroutine(DrawLine());

    }

    void FinishLine()
    {
        if (drawing != null)
            StopCoroutine(drawing);

        Destroy(line.gameObject);

        BusSystem.CallDrawEnd(lineMesh);
    }

    IEnumerator DrawLine()
    {

        GameObject lineObject = Instantiate(lineRendererPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        line = lineObject.GetComponent<LineRenderer>();
        line.positionCount = 0;

        while (true)
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
            position.z = 0;
            line.positionCount++;
            line.SetPosition(line.positionCount - 1, position);

            lineMesh = new Mesh(); 
            line.BakeMesh(lineMesh);

            yield return null;
        }
    }
}