using UnityEngine;
using System.Collections;

public class CloudsUpdater : MonoBehaviour {

    public float heightMultiplier = 100f;
    public float xzMultiplier = .001f;
    public float timeMultiplier = .001f;
    public float heightOffset = -100;
    public float scaleLow = .5f;
    public float scaleHigh = 2f;
    protected Transform[] children;
    protected Vector3[] positions;
    protected float time = 0f;

	// Use this for initialization
	void Start () {
        int childCount = transform.childCount;
        children = new Transform[childCount];
        positions = new Vector3[childCount];
        for (int i = 0; i < childCount; i++)
        {
            children[i] = transform.GetChild(i);
            positions[i] = children[i].position;
            children[i].localScale = Vector3.one * Random.Range(scaleLow, scaleHigh);
            children[i].rotation = Quaternion.Euler(-90, Random.Range(0, 360), 0);
        }
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime * timeMultiplier;
        Vector3 pos;
        for(int i = 0; i < children.Length; i++)
        {
            pos = positions[i];
            pos.y = Mathf.PerlinNoise(xzMultiplier*pos.x + time, xzMultiplier*pos.z) * heightMultiplier + heightOffset;
            children[i].position = pos;
        }
	}
}
