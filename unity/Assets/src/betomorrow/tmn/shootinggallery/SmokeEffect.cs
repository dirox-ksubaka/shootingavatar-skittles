using UnityEngine;
using System.Collections;

public class SmokeEffect : MonoBehaviour {
	
	public AnimationCurve  m_alphaCurve;
	
	private Material[] m_materials;
	private float m_startTime;
	private float m_curveDuration = 0;
	
	void Start () {
		MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
		int index=0;
		m_materials = new Material[renderers.Length];
		foreach (MeshRenderer renderer in renderers) {
			m_materials[index++] = renderer.material;
		}
		m_startTime = Time.realtimeSinceStartup;
		if (m_alphaCurve.length > 1) {
			m_curveDuration = m_alphaCurve[m_alphaCurve.length - 1].time - m_alphaCurve[0].time;
		}
	}
	
	void Update () {
		float localDt = Time.realtimeSinceStartup - m_startTime;
		if (localDt < m_curveDuration) {
			float alpha = m_alphaCurve.Evaluate (localDt);
			foreach (Material mat in m_materials) {
				Color c = mat.color;
				c.a = alpha;
				mat.color = c;
			}
		} else {
			GameObject.Destroy(gameObject);
		}
	}
}
