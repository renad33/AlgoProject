using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class viewOutline : MonoBehaviour
{
    public Sprite buildingSprite; 
    public Material outlineMaterial; 
    private float screenSplit = 15f; 

    void Start()
    {
       
        List<Building> buildings = new List<Building>
        {
            
            new Building(1, 11, 5),
            new Building(3, 8, 10),
            new Building(7, 15, 20),
            new Building(15, 20, 22)
        };

       
        BuildingOutline outlineSolver = new BuildingOutline();
        List<OutlinePoint> skyline = outlineSolver.GetSkyline(buildings);

      
        for (int i = 0; i < buildings.Count; i++)
        {
            DrawBuilding(buildings[i]);
        }

      
        DrawOutline(skyline);

        
        PrintOutlineToConsole(skyline);
    }

    void DrawBuilding(Building building)
    {
        
        GameObject buildingObj = new GameObject("Building");
        SpriteRenderer sr = buildingObj.AddComponent<SpriteRenderer>();
        sr.sprite = buildingSprite;

       
        sr.color = new Color(Random.value, Random.value, Random.value);

       
        buildingObj.transform.position = new Vector3(
            ((building.Left + building.Right) / 2f) - screenSplit, building.Height / 2f,  0); 

      
        buildingObj.transform.localScale = new Vector3(
            building.Right - building.Left, building.Height, 1); 

      
        GameObject textObj = new GameObject("BuildingText");
        TextMesh textMesh = textObj.AddComponent<TextMesh>();
        textMesh.text = $"({building.Left}, {building.Height}, {building.Right})";
        textMesh.fontSize = 10;
        textMesh.color = Color.black;
        textObj.transform.position = new Vector3(
            ((building.Left + building.Right) / 2f) - screenSplit, -1, 0);
    }

    void DrawOutline(List<OutlinePoint> outline)
    {
       
        GameObject outlineObj = new GameObject("Outline");
        LineRenderer lr = outlineObj.AddComponent<LineRenderer>();
        lr.material = outlineMaterial;
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.useWorldSpace = true;

        
        List<Vector3> adjustedPoints = new List<Vector3>();

        for (int i = 0; i < outline.Count - 1; i++)
        {
            var current = outline[i];
            var next = outline[i + 1];

           
            adjustedPoints.Add(new Vector3(current.X + screenSplit, current.Height, 0));

            
            if (current.Height != next.Height)
            {
                adjustedPoints.Add(new Vector3(next.X + screenSplit, current.Height, 0));
            }

           
            GameObject textObj = new GameObject("OutlinePointText");
            TextMesh tm = textObj.AddComponent<TextMesh>();
            tm.text = $"({current.X}, {current.Height})";
            tm.fontSize = 10;
            tm.color = Color.black;
            textObj.transform.position = new Vector3(current.X + screenSplit, -1, 0);
        }

        
        var last = outline[outline.Count - 1];
        adjustedPoints.Add(new Vector3(last.X + screenSplit, last.Height, 0));

        GameObject lastTextObj = new GameObject("OutlineLastPointText");
        TextMesh lastTextMesh = lastTextObj.AddComponent<TextMesh>();
        lastTextMesh.text = $"({last.X}, {last.Height})";
        lastTextMesh.fontSize = 10;
        lastTextMesh.color = Color.black;
        lastTextObj.transform.position = new Vector3(last.X + screenSplit, -1, 0);

       
        lr.positionCount = adjustedPoints.Count;
        lr.SetPositions(adjustedPoints.ToArray());
    }

    void PrintOutlineToConsole(List<OutlinePoint> outline)
    {
        
        Debug.Log("Outline:");
        for (int i = 0; i < outline.Count; i++)
        {
            Debug.Log($"({outline[i].X}, {outline[i].Height})");
        }
    }
}
