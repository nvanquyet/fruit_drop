
using Base;
using Config;
using Data;
using DesignPatterns;
using DG.Tweening;
using GameEvents;
using UnityEngine;
using UnityEngine.Serialization;

namespace Object
{
    public class Fruit : AInteractable, IPoolable
    {
        private FruitStruct _data;
        public FruitStruct Data => _data;
        
        public void Initialize(FruitStruct data)
        {
            _data = data;
            SetSprite(data.visualData.fruitSprite);
            InitializeObjectSize(_data.sizeMultiplier);
        }
        
        #region Collision Event 
        
        public override void OnInteract(IInteractable target)
        {
            if (target is not Fruit fruit) return;
            if (fruit._data.id != this._data.id) return;
            SetCanInteract(false);
            fruit.SetCanInteract(false);
            ObserverManager.Instance.Notify(new GameEvent.OnFruitMerged()
            {
                SourceFruit = fruit,
                TargetFruit =  this
            });
        }
        

        public override void OnExitInteract(IInteractable target)
        {
            
        }
        #endregion
        
        #region Input Move Object
        [FormerlySerializedAs("dragConfig")]
        [Header("Config Drag")]
        [SerializeField] private FruitConfig config;
        [SerializeField] private Rigidbody2D rigid;
        [SerializeField] private Collider2D col;
        [SerializeField] private Renderer render;
        
        
        private float _objectHalfWidth;
        private bool _isDragging = false;
        private Plane _dragPlane;
        private Vector3 _offset;
        private Camera _cam;

       
        public void ApplyPhysics(bool isEnable) => rigid.isKinematic = !isEnable;
        
        public void DisableAndReactivateInteraction()
        {
            SetCanInteract(false);
    
            // delay
            Invoke(nameof(ReactivateInteraction), 0.2f);
        }

        private void InitializeObjectSize(float size)
        {
            var localScale = Vector3.one * size;
            transform.DOScale(localScale, 0.25f).From(localScale * 1.0f / 3).OnComplete(InitializedSize);
        }
        
        private void InitializedSize()
        {
            if (col)
            {
                _objectHalfWidth = col.bounds.extents.x;
            }
            else if (render)
            {
                _objectHalfWidth = render.bounds.extents.x;
            }
            else
            {
                _objectHalfWidth = transform.localScale.x / 2;
            }
        }

        
        private void SetSprite(Sprite sprite)
        {
            if (render == null || sprite == null) return;
            // Nếu sử dụng SpriteRenderer
            if (render is SpriteRenderer spriteRenderer)
            {
                spriteRenderer.sprite = sprite;
            }
            // Nếu sử dụng MeshRenderer với material
            else
            {
                render.material.mainTexture = sprite.texture;
            }
        }

        private void ReactivateInteraction() => SetCanInteract(true);
        private void OnMouseDown()
        {
            if(!rigid.isKinematic) return;
            _isDragging = true;
            if(!_cam) _cam = Camera.main;
            if(!_cam) return;
            var mouseWorldPos = _cam.ScreenToWorldPoint(Input.mousePosition);
            _offset = transform.position - new Vector3(mouseWorldPos.x, transform.position.y, 0);
        }

        private void OnMouseDrag()
        {
            if (!_isDragging || !_cam || !config || !rigid.isKinematic) return;
            var mouseWorldPos = _cam.ScreenToWorldPoint(Input.mousePosition);
    
            var targetX = Mathf.Clamp(
                mouseWorldPos.x + _offset.x, 
                config.minX + _objectHalfWidth, 
                config.maxX - _objectHalfWidth
            );
    
            var targetPosition = new Vector3(targetX, transform.position.y, 0);
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * config.smoothSpeed);
        }

        private void OnMouseUp()
        {
            if(rigid.isKinematic == true) ObserverManager.Instance.Notify(new GameEvent.OnFruitDrop());
            _isDragging = false;
            ApplyPhysics(true);
        }

        #endregion

        #region Unity Events

#if  UNITY_EDITOR
        private void OnValidate()
        {
            rigid ??= GetComponent<Rigidbody2D>();
            col ??= GetComponent<Collider2D>();
            render ??= GetComponent<Renderer>();
        }
#endif
        #endregion
        
        
        public void OnDespawn(ObjectPooling<Fruit> pool)
        {
            transform.DOScale(Vector3.zero, 0.25f).OnComplete(() =>
            {
                pool.ReturnToPool(this);
            });
        }
        
        public void OnSpawn()
        {
            
        }

        public void OnDespawn()
        {
            
        }
    }
}