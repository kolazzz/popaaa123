using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed;

    [Header("Animation Settings")]
    [SerializeField] private Sprite[] idleSprites;
    [SerializeField] private Sprite[] runSprites;
    [SerializeField] private Sprite[] deathSprites;
    [SerializeField] private float animationSpeed = 0.1f;

    [SerializeField] private LayerMask damagelayerMask;

    private Rigidbody2D _rb;
    private Transform _followtarget;
    private SpriteRenderer _spriteRenderer;
    private bool _isCollided;
    private bool _isDead;
    private float _animationTimer;
    private int _currentFrame;
    private Sprite[] _currentSprites;

    private enum AnimationState { Idle, Run, Death }
    private AnimationState _currentState;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        SetAnimationState(AnimationState.Idle);
    }

    void Update()
    {
        if (!_isDead)
        {
            FollowPlayer();
            Animate();
        }
    }

    private void SetAnimationState(AnimationState newState)
    {
        if (_currentState == newState) return;

        _currentState = newState;
        _currentFrame = 0;
        _animationTimer = 0f;

        switch (_currentState)
        {
            case AnimationState.Idle:
                _currentSprites = idleSprites;
                break;
            case AnimationState.Run:
                _currentSprites = runSprites;
                break;
            case AnimationState.Death:
                _currentSprites = deathSprites;
                StartCoroutine(PlayDeathAnimation());
                break;
        }
    }

    private void Animate()
    {
        if (_currentSprites == null || _currentSprites.Length == 0 || _isDead) return;

        _animationTimer += Time.deltaTime;
        if (_animationTimer >= animationSpeed)
        {
            _animationTimer = 0f;
            _currentFrame = (_currentFrame + 1) % _currentSprites.Length;
            _spriteRenderer.sprite = _currentSprites[_currentFrame];
        }
    }

    private void FollowPlayer()
    {
        if (_followtarget && !_isCollided)
        {
            SetAnimationState(AnimationState.Run);
            transform.position = Vector2.MoveTowards(transform.position, _followtarget.position, speed * Time.deltaTime);

            // Поворачиваем врага в сторону движения
            if (_followtarget.position.x < transform.position.x)
            {
                // Враг движется влево
                FlipSprite(true);
            }
            else if (_followtarget.position.x > transform.position.x)
            {
                // Враг движется вправо
                FlipSprite(false);
            }
        }
        else
        {
            SetAnimationState(AnimationState.Idle);
        }
    }

    private void FlipSprite(bool isFlipped)
    {
        Vector3 localScale = _spriteRenderer.transform.localScale;
        localScale.x = isFlipped ? -Mathf.Abs(localScale.x) : Mathf.Abs(localScale.x); // Меняем знак по оси X
        _spriteRenderer.transform.localScale = localScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out TopDownCharacterController _))
        {
            _followtarget = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out TopDownCharacterController _))
        {
            _followtarget = null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out TopDownCharacterController _))
        {
            _isCollided = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (LayerMaskUtil.ContainsLayer(damagelayerMask, collision.gameObject) && !_isDead)
        {
            _isDead = true;
            SetAnimationState(AnimationState.Death);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out TopDownCharacterController _))
        {
            _isCollided = false;
        }
    }

    private IEnumerator PlayDeathAnimation()
    {
        foreach (var sprite in deathSprites)
        {
            _spriteRenderer.sprite = sprite;
            yield return new WaitForSeconds(animationSpeed);
        }

        gameObject.SetActive(false); // Отключаем объект после завершения анимации
    }
}
