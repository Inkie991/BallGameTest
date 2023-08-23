using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController: MonoBehaviour
{
    private const float PLAYER_SCALE_FACTOR = 0.1f;
    private const float FIREBALL_SCALE_FACTOR = 0.3f;

    [SerializeField] private Vector3 _fireballSpawnPointOffset;
    [SerializeField] private GameObject _fireballPrefab;
    [SerializeField] private Transform _doorTransform;
    [SerializeField] private PathPreview _path;
    [SerializeField] private float _jumpForce;
    private GameObject _currentFireball;
    private Transform _currentFireballTransform;
    private Rigidbody _body;
    private bool _isScaling;
    private bool _isJumping;

    private void Awake()
    {
        EventManager.AddListener<PathIsClearEvent>(OnPathCleared);
    }

    // Start is called before the first frame update
    void Start()
    {
        _isScaling = false;
        _isJumping = false;
        _body = GetComponent<Rigidbody>();
        transform.LookAt(_doorTransform);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<PathIsClearEvent>(OnPathCleared);
    }

    // Update is called once per frame
    void Update()
    {
        if (Managers.Gameplay.GameOver) return;
        
        if (transform.localScale.y <= 0.15f && !Managers.Gameplay.GameOver)
            EventManager.Broadcast(Events.PlayerLoseEvent);

        if (transform.position.y < 1.1f) if (CheckPathToDoor()) Jump(_doorTransform.position);
        
        if (!_isJumping)
        {
            if (Input.touchCount > 0 || Input.GetMouseButtonDown(0))
            {
                if (_currentFireball == null)
                {
                    _currentFireball = Instantiate(_fireballPrefab);
                    _currentFireballTransform = _currentFireball.transform;
                    _currentFireballTransform.position = transform.position + _fireballSpawnPointOffset;
                }
                else
                {
                    if (!_isScaling)
                    {
                        _isScaling = true;
                        StartCoroutine(BallScaling());
                    }
                }
            }
            else
            {
                if (_currentFireball != null)
                {
                    _currentFireball.GetComponent<Fireball>().Shoot(_doorTransform);
                }
            }

        }
    }

    private void OnPathCleared(PathIsClearEvent evt)
    {
        Vector3 targetPoint = evt.FireballPos - _fireballSpawnPointOffset;
        Jump(targetPoint);
    }

    void Jump(Vector3 target)
    {
        if (Managers.Gameplay.GameOver) return;

        _isJumping = true;
        float distance = (Vector3.Distance(transform.position, target));
        int jumpsNum;
        if (distance < 1f) jumpsNum = 1;
        else jumpsNum = (int)Math.Round(distance);
        float duration = (float)jumpsNum / 2;
        //Debug.Log("duration " + duration);
        _body.DOJump(target, _jumpForce, jumpsNum, duration);
        StartCoroutine(WaitForJumps(target));
    }

    private bool CheckPathToDoor()
    {
        Vector3 tempPos = transform.position;
        float tempScale = transform.localScale.y;
        float maxDitance = Vector3.Distance(tempPos, _doorTransform.position);

        RaycastHit hit;
        
        bool center = Physics.Raycast(tempPos,
            _doorTransform.position - tempPos, out hit, maxDitance);
        if (hit.collider.CompareTag("Obstacle")) center = true;
        else center = false;
        
        Vector3 rightPoint = new Vector3(tempPos.x + tempScale/2, tempPos.y, tempPos.z);
        bool right = Physics.Raycast(rightPoint,
            _doorTransform.position - rightPoint, out hit, maxDitance - 1f);
        if (hit.collider.CompareTag("Obstacle")) right = true;
        else right = false;
        
        Vector3 leftPoint = new Vector3(tempPos.x, tempPos.y, tempPos.z + tempScale / 2);
        bool left = Physics.Raycast(leftPoint,
            _doorTransform.position - leftPoint,out hit, maxDitance - 1f);
        if (hit.collider.CompareTag("Obstacle")) left = true;
        else left = false;
        
        //Debug.Log(left + " " + center + " " + right);

        if (center && right && left) return false;
        else return true;
    }

    private IEnumerator BallScaling()
    {
        //Do it with DOTween
        //transform.localScale = new Vector3(PLAYER_SCALE_FACTOR, PLAYER_SCALE_FACTOR, PLAYER_SCALE_FACTOR);
        //_currentFireballTransform.localScale += new Vector3(FIREBALL_SCALE_FACTOR, FIREBALL_SCALE_FACTOR, FIREBALL_SCALE_FACTOR);
        
        
        //transform.DOScale(transform.localScale.y - PLAYER_SCALE_FACTOR, Managers.Gameplay.scalerStepTime);
        //_currentFireballTransform.DOScale(_currentFireballTransform.localScale.y + FIREBALL_SCALE_FACTOR, Managers.Gameplay.scalerStepTime);
        
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed);
        _currentFireballTransform.position = new Vector3(_currentFireballTransform.position.x,
            _currentFireballTransform.localScale.y / 2 + 0.1f,
            _currentFireballTransform.position.z);
        _path.PathScaler(PLAYER_SCALE_FACTOR);
        yield return new WaitForSecondsRealtime(Managers.Gameplay.scalerStepTime);
        _isScaling = false;
    }

    private IEnumerator WaitForJumps(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.6f)
        {
            Debug.Log(Vector3.Distance(transform.position, target));
            yield return new WaitForSeconds(0.1f);
        }

        _isJumping = false;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x + (transform.localScale.y / 2),
            transform.position.y, transform.position.z), 0.5f);
        Gizmos.DrawWireSphere(new Vector3(transform.position.x,
            transform.position.y, transform.position.z + (transform.localScale.y / 2)), 0.5f);
    }
}
