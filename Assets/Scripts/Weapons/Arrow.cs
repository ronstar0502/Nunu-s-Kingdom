using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D _rb;
    private GameObject _target;
    private Vector2 _targetDirection;
    private int _arrowDamage;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (_target == null)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (_target != null)
        {
            _rb.velocity = _targetDirection * speed * Time.deltaTime;
        }
    }

    public void InitArrow(GameObject targetObj, Vector2 direction, int damage ,bool isFromTower)
    {
        if (targetObj != null)
        {
            _target = targetObj;
            _targetDirection = direction;
            if (isFromTower)
            {
                _targetDirection.Normalize(); //normalzie for consistend direction
                float arrowRotation = Mathf.Atan2(_targetDirection.y, _targetDirection.x) * Mathf.Rad2Deg;
                _rb.rotation = arrowRotation;

                _targetDirection = direction; //preserve the original direction so the speed wont change dramatically
            }
            _arrowDamage = damage;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == _target && _target !=null)
        {
            print($"arrow hit target: {collision.gameObject.name} with {_arrowDamage} damage");
            collision.gameObject.GetComponent<IDamageable>().TakeDamage(_arrowDamage);
            Destroy(gameObject);
        }
    }
}
