using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class MapManager : MonoBehaviour
{
    public GameObject mapsX;
    public GameObject mapsminusX;
    public GameObject mapsZ;
    public GameObject mapszminusZ;

    public GameObject[] MapObjs;
    [HideInInspector]
    public GameObject mapObjtemp;

    [Header("MapSize")]
    public float[] MapSizeX;
    public float[] MapSizeZ;
    public int mapindex;

    public void MapInitSetting()
    {
        mapindex = Random.Range(0, MapObjs.Length);

        mapObjtemp = Instantiate(MapObjs[mapindex]);

        float mapsizeX = MapSizeX[mapindex];
        float mapsizeZ = MapSizeZ[mapindex];

        mapsX.transform.DORewind();
        mapsminusX.transform.DORewind();
        mapsZ.transform.DORewind();
        mapszminusZ.transform.DORewind();

        mapsX.transform.position = new Vector3(mapsizeX, 0.5f, 0);
        mapsX.transform.localScale = new Vector3(mapsizeZ * 2 + 1, 1, 1);

        mapsminusX.transform.position = new Vector3(-mapsizeX, 0.5f, 0);
        mapsminusX.transform.localScale = new Vector3(mapsizeZ * 2 + 1, 1, 1);

        mapsZ.transform.position = new Vector3(0, 0.5f, mapsizeZ);
        mapsZ.transform.localScale = new Vector3(mapsizeX * 2 + 1, 1, 1);

        mapszminusZ.transform.position = new Vector3(0, 0.5f, -mapsizeZ);
        mapszminusZ.transform.localScale = new Vector3(mapsizeX * 2 + 1, 1, 1);

        StartCoroutine(MapShrinkCor());
    }

    IEnumerator MapShrinkCor()
    {
        yield return new WaitForSeconds(10);
        MapShrink();
    }

    public void MapShrink()
    {
        mapsX.transform.DORewind();
        mapsminusX.transform.DORewind();
        mapsZ.transform.DORewind();
        mapszminusZ.transform.DORewind();

        mapsX.transform.DOMoveX(10, 30);
        mapsX.transform.DOScaleX(21, 30);

        mapsminusX.transform.DOMoveX(-10, 30);
        mapsminusX.transform.DOScaleX(21, 30);

        mapsZ.transform.DOMoveZ(10, 30);
        mapsZ.transform.DOScaleX(21, 30);

        mapszminusZ.transform.DOMoveZ(-10, 30);
        mapszminusZ.transform.DOScaleX(21, 30);
    }
}
