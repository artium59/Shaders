// #define VISUAL_DEBUG

using UnityEngine;

public class OutlineMaker : MonoBehaviour
{
	#region <Consts>

	// Wall's Layer Number
	private const int LayerNumber = 1 << 1;
	// Check only walls
	private const int RayCastHitCandidateMaxNumber = 2;
	private const string OriginalShaderName = "Mobile/Bumped Diffuse";
	// DON'T CHANGE
	private const string OutlineShaderName = "Mobile/Outline";

	#endregion </Consts>

	#region <Fields>

	// body, weapon, shield
	[SerializeField] private Renderer[] _rendererGroup;
	
	private RaycastHit[] _raycastHitGroup;
	private Transform _transform;

	#endregion </Fields>
	
	private void Start()
	{
		_raycastHitGroup = new RaycastHit[RayCastHitCandidateMaxNumber];
		
		// @SEE: head is the best place, maybe
		_transform = GetComponent<Transform>();
	}
	
	private void Update()
	{
		// _transform.position + new Vector3(0, 2.4f, 0) = skeleton's head tramsform
		var direction = CameraManager.GetInstance.MainCameraTransform.position - (_transform.position + new Vector3(0, 2.4f, 0));
		
#if VISUAL_DEBUG
		DrawLine(direction);
#endif
		
		var rayCastHitNumber = Physics.RaycastNonAlloc(_transform.position + new Vector3(0, 2.4f, 0), direction, _raycastHitGroup, Mathf.Infinity, LayerNumber);

		// if (rayCastHitNumber <= 0) return;
		
		print(rayCastHitNumber.ToString());
		
		for (var index = 0; index < _rendererGroup.Length; index++)
		{
			// var outlineObjectMaterial = _outlineObjects[index].GetComponent<Renderer>().material;
	
			ShaderNameChange(_rendererGroup[index].material, rayCastHitNumber == 0 ? OriginalShaderName : OutlineShaderName);
			// outlineObjectMaterial.shader = Shader.Find(rayCastHitNumber == 0 ? OriginalShaderName : OutlineShaderName);
		}
	}

	private static void ShaderNameChange(Material mat, string shaderName)
	{
		if (mat.name == shaderName) return;
		
		mat.shader = Shader.Find(shaderName);
	}
	
#if VISUAL_DEBUG
	private void DrawLine(Vector3 way) {
		Debug.DrawRay(_transform.position + new Vector3(0, 2.4f, 0), way, Color.red);
	}
#endif
		
}
