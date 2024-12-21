using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Building
{
    public int Left;
    public int Height;
    public int Right;

    public Building(int left, int height, int right)
    {
        Left = left;
        Height = height;
        Right = right;
    }
}

public struct OutlinePoint
{
    public int X;
    public int Height;

    public OutlinePoint(int x, int height)
    {
        X = x;
        Height = height;
    }
}

public class BuildingOutline
{
    public List<OutlinePoint> GetSkyline(List<Building> buildings)
    {
        if (buildings == null || buildings.Count == 0)
        {
            return new List<OutlinePoint>();
        }

        return DivideAndConquer(buildings, 0, buildings.Count - 1);
    }

    private List<OutlinePoint> DivideAndConquer(List<Building> buildings, int low, int high)
    {
        if (low > high)
        {
            return new List<OutlinePoint>();
        }

        if (low == high)
        {
            List<OutlinePoint> result = new List<OutlinePoint>
            {
                new OutlinePoint(buildings[low].Left, buildings[low].Height),
                new OutlinePoint(buildings[low].Right, 0)
            };
            return result;
        }

        int mid = low + (high - low) / 2;

        List<OutlinePoint> leftSkyline = DivideAndConquer(buildings, low, mid);
        List<OutlinePoint> rightSkyline = DivideAndConquer(buildings, mid + 1, high);

        return MergeSkyline(leftSkyline, rightSkyline);
    }

    private List<OutlinePoint> MergeSkyline(List<OutlinePoint> skyline1, List<OutlinePoint> skyline2)
    {
        List<OutlinePoint> mergedSkyline = new List<OutlinePoint>();
        int h1 = 0, h2 = 0, currentHeight = 0;

        int i = 0, j = 0;

        while (i < skyline1.Count && j < skyline2.Count)
        {
            OutlinePoint point1 = skyline1[i];
            OutlinePoint point2 = skyline2[j];

            if (point1.X < point2.X)
            {
                h1 = point1.Height;
                int maxHeight = Mathf.Max(h1, h2);
                if (maxHeight != currentHeight)
                {
                    mergedSkyline.Add(new OutlinePoint(point1.X, maxHeight));
                    currentHeight = maxHeight;
                }
                i++;
            }
            else if (point1.X > point2.X)
            {
                h2 = point2.Height;
                int maxHeight = Mathf.Max(h1, h2);
                if (maxHeight != currentHeight)
                {
                    mergedSkyline.Add(new OutlinePoint(point2.X, maxHeight));
                    currentHeight = maxHeight;
                }
                j++;
            }
            else
            {
                h1 = point1.Height;
                h2 = point2.Height;
                int maxHeight = Mathf.Max(h1, h2);
                if (maxHeight != currentHeight)
                {
                    mergedSkyline.Add(new OutlinePoint(point1.X, maxHeight));
                    currentHeight = maxHeight;
                }
                i++;
                j++;
            }
        }

        while (i < skyline1.Count)
        {
            mergedSkyline.Add(skyline1[i]);
            i++;
        }

        while (j < skyline2.Count)
        {
            mergedSkyline.Add(skyline2[j]);
            j++;
        }

        return RemoveRedundantPoints(mergedSkyline);
    }

    private List<OutlinePoint> RemoveRedundantPoints(List<OutlinePoint> points)
    {
        for (int i = 0; i < points.Count - 1; i++)
        {
            while (i < points.Count - 1 && points[i].Height == points[i + 1].Height)
            {
                points.RemoveAt(i + 1);
            }
        }
        return points;
    }
}