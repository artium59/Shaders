//#define VISUAL_DEBUG

using UnityEngine;

public class TransparentManager : Singleton<TransparentManager> {

    #region <Consts>

    [SerializeField]private const float TransparentDegree = 0.3f;
    private const int Layermask = 1 << 1;
    private const int RayCastHitCandidateMaxNumber = 5;
    
    #endregion </Consts>

    #region <Fields>
  
    private Renderer[] _previousRendererGroup;
    private RaycastHit[] _raycastHitGroup;
    
    #endregion </Fields>

    protected override void Initialize()
    {
        base.Initialize();
        _raycastHitGroup = new RaycastHit[RayCastHitCandidateMaxNumber];
    }
    
    public void Trigger() {
                             
        var direction = CameraManager.GetInstance.MainCameraTransform.position 
                        - PlayerManager.GetInstance.PlayerChampion.GetUnitPosition;
        
        #if VISUAL_DEBUG
                DrawLine(direction);
        #endif
        
        var currentRendererGroup = new Renderer[RayCastHitCandidateMaxNumber];
        var rayCastHitNumber = Physics.RaycastNonAlloc(PlayerManager.GetInstance.PlayerChampion.GetUnitPosition,
            direction, _raycastHitGroup, Mathf.Infinity, Layermask);
        
        for (var rayCastHitindex = 0; rayCastHitindex < rayCastHitNumber; rayCastHitindex++)
        {
            var raycastHit = _raycastHitGroup[rayCastHitindex];
            var raycastHitRenderer = raycastHit.transform.GetComponent<Renderer>();

            ColorChange(raycastHitRenderer, TransparentDegree);
            currentRendererGroup[rayCastHitindex] = raycastHitRenderer;
        }                

        // if _currentRenderGroup contains _previousRenderGroup node.
        if (_previousRendererGroup != null)
        {
            for (var previousIndex = 0;
                previousIndex < _previousRendererGroup.Length && _previousRendererGroup[previousIndex] != null;
                ++previousIndex)
            {
                var isContained = false;
                var previousRender = _previousRendererGroup[previousIndex];

                for (var currentIndex = 0;
                    currentIndex < currentRendererGroup.Length && currentRendererGroup[currentIndex] != null;
                    ++currentIndex)
                {
                    var currentRender = currentRendererGroup[currentIndex];

                    if (previousRender != currentRender) continue;

                    isContained = true;
                    break;
                }

                // if not contains,
                if (!isContained)
                    ColorChange(previousRender, 1.0f);
            }
        }

        // @SEE<Carey>: Find an alternative of this memory allocaton.
        _previousRendererGroup = currentRendererGroup;
    }
    
    private void ColorChange(Renderer rendererToChange, float degree)
    {
        var rendererMaterialGroup = rendererToChange.materials;

        for (var rendererMaterialIndex = 0;
            rendererMaterialIndex < rendererMaterialGroup.Length;
            ++rendererMaterialIndex)
        {
            var materialColor = rendererMaterialGroup[rendererMaterialIndex].color;
            materialColor.a = degree;
            rendererMaterialGroup[rendererMaterialIndex].SetColor("_Color", materialColor);
        }
    }
    
#if VISUAL_DEBUG
    private void DrawLine(Vector3 way) {
        Debug.DrawRay(_playerTransform.position, way, Color.red);
    }
#endif
    
}