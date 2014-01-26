using UnityEngine;
using System.Collections;

public class GlowShader : MonoBehaviour {
	float effectTime;
	
	void Update () {
		if (effectTime > 0) {
			if (effectTime > 300){
				renderer.sharedMaterial.SetVector("_ShieldColor", new Vector4(1, 1, 1, 0.5f));
			}
			
			effectTime -= Time.deltaTime * 1000;
			
			renderer.sharedMaterial.SetFloat("_EffectTime", effectTime);
		}
	}
	
	void OnTriggerEnter(Collider col) {
		
		Vector3 contact = collider.ClosestPointOnBounds (col.gameObject.transform.localPosition);
			
		renderer.sharedMaterial.SetVector("_ShieldColor", new Vector4(1, 1, 1, 0.5f));
			
		renderer.sharedMaterial.SetVector("_Position", transform.InverseTransformPoint(contact));
		print (contact);
			
		effectTime = 500;
	}
}
