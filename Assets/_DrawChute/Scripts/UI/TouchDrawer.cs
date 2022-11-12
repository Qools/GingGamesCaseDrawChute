using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class TouchDrawer : MonoBehaviour
{
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

        if (Input.GetMouseButton(0))
        {
            pointerEventData = new PointerEventData(eventSystem);
            pointerEventData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            
            drawZoneRayCaster.Raycast(pointerEventData, results);

            foreach (var result in results)
            {
                if (result.gameObject.CompareTag(PlayerPrefKeys.drawZone))
                {
                    DrawLine();
                }

                if (result.gameObject.CompareTag(PlayerPrefKeys.otherZone))
                {
                    FinishLine();
                }
            }
        }
    }

    void StartLine()
    {
        GameObject lineObject = Instantiate(lineRendererPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        line = lineObject.GetComponent<LineRenderer>();
        line.positionCount = 0;

    }

    void FinishLine()
    {
        if (line != null)
        {
            Destroy(line.gameObject);

            line.Simplify(.05f);

            lineMesh = new Mesh();
            line.BakeMesh(lineMesh);

            lineMesh.MarkDynamic();
            lineMesh.MarkModified();

            BusSystem.CallDrawEnd(lineMesh);
        }
    }

    private void DrawLine()
    {
        Vector3 position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        position.z = 0;
        line.positionCount++;
        line.SetPosition(line.positionCount - 1, position);
    }
}