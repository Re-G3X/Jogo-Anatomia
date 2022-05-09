﻿using System;
using Game.GameManager.Player;
using UnityEngine;

namespace Game.GameManager
{
    public class BombController : MonoBehaviour
    {

        [SerializeField]
        private AudioClip popSnd;
        private AudioSource audioSrc;
        private Rigidbody2D rb;
        private Animator animator;
        private CircleCollider2D bombCollider;

        private bool canDestroy, hasBeenThrown, hasTimerBeenSet, isExploding;
        public int Damage { get; set; }
        public int EnemyThatShot { get; set; }

        [SerializeField]
        private float bombLifetime;
        private float bombCountdown;

        public static event EventHandler PlayerHitEventHandler;

        private bool isColliding;
        private static readonly int Explode = Animator.StringToHash("Explode");

        private Collider2D[] objectsInRange;
        private void Awake()
        {
            bombLifetime = 2.0f;
            canDestroy = false;
            hasBeenThrown = false;
            hasTimerBeenSet = false;
            isExploding = false;
            objectsInRange = new Collider2D[20];
        }

        private void OnEnable()
        {
            PlayerController.PlayerDeathEventHandler += PlayerHasDied;
        }

        private void OnDisable()
        {
            PlayerController.PlayerDeathEventHandler -= PlayerHasDied;
        }

        private void Start()
        {
            audioSrc = GetComponent<AudioSource>();
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            bombCollider = GetComponent<CircleCollider2D>();
            bombCollider.enabled = false;
        }

        private void PlayerHasDied(object sender, EventArgs eventArgs)
        {
            Destroy(gameObject);
        }

        //TODO change bomb explosion timer to coroutine
        private void Update()
        {
            if (hasBeenThrown && !hasTimerBeenSet)
            {
                bombCountdown = bombLifetime;
                hasTimerBeenSet = true;
            }
            if (hasTimerBeenSet && !isExploding)
            {
                if (bombCountdown >= 0.01f)
                    bombCountdown -= Time.deltaTime;
                else
                    ExplodeBomb();
                if (!isColliding && (bombCountdown < (bombLifetime - 0.2f)))
                {
                    isColliding = true;
                    bombCollider.enabled = isColliding;
                }
            }
            if (!audioSrc.isPlaying && canDestroy)
            {
                Destroy(gameObject);
            }
        }

        public void DestroyBomb()
        {
            canDestroy = true;
        }

        public void SetEnemyThatShot(int index)
        {
            EnemyThatShot = index;
        }

        protected virtual void OnPlayerHit()
        {
            PlayerHitEventHandler?.Invoke(null, EventArgs.Empty);
        }

        public void Shoot(Vector2 facingDirection)
        {
            isColliding = false;
            rb.AddForce(facingDirection, ForceMode2D.Impulse);
            hasBeenThrown = true;
        }

        private void ExplodeBomb()
        {
            animator.SetTrigger(Explode);
            audioSrc.PlayOneShot(popSnd, 0.3f);
            isExploding = true;
            var transform1 = transform;
            var currScale = transform1.localScale;
            transform1.localScale = new Vector3(currScale.x * 4, currScale.y * 4, currScale.z * 1);
            var position = rb.position;
            var size = Physics2D.OverlapCircleNonAlloc(new Vector2(position.x, position.y), 1.8f, objectsInRange);
            for(var i=0; i < size; ++i)
            {
                if (objectsInRange[i].gameObject.CompareTag("Player"))
                {
                    var collisionDirection =
                        Vector3.Normalize(objectsInRange[i].gameObject.transform.position - gameObject.transform.position);
                    OnPlayerHit();
                    objectsInRange[i].gameObject.GetComponent<HealthController>().ApplyDamage(Damage, collisionDirection, EnemyThatShot);
                }
            }
        }
    }
}
