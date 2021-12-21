using System.Collections;
using System.Collections.Generic;
using Features.GameStates.Character;
using Features.Maze_Namespace.Scripts;
using UnityEngine;
using UnityEngine.Events;
using Utils.Variables;
using Random = UnityEngine.Random;

namespace Features.Hunter_Namespace.Scripts
{
    public class HunterBehaviour : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CharacterStateController_SO characterStateController;
        [SerializeField] private Vector2IntVariable hunterPos;
        [SerializeField] private Vector2IntVariable playerIntPosition;
        [SerializeField] private Vector2Variable playerFloatPosition;
        [SerializeField] private TileList_SO tiles;
        [SerializeField] private MazePositionUtils_SO posControl;
        [SerializeField] private UnityEvent onPlayerDashComplete;

        [Header("Hunter Balancing")]
        [SerializeField] private float hunterBahviourUpdateTime;
        [SerializeField] private int playerChasePathfindingDepth = 1;
        [SerializeField] private float walkSpeed;
        [SerializeField] private float dashSpeed;

        private TileBehaviour _lastTile;
        private Animator _animator;
        private bool _validBehaviourUpdate;
    
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int Vertical = Animator.StringToHash("Vertical");
        private static readonly int KillPlayer = Animator.StringToHash("KillPlayer");

        private bool PlayerIsHuntable => !(characterStateController.GetState() is DeathState_SO) &&
                                         !(characterStateController.GetState() is HideState_SO);
    
        public void InitializeHunter()
        {
            transform.position = (Vector2)hunterPos.Get();
            _lastTile = tiles.GetTileAt(hunterPos.Get());
            _validBehaviourUpdate = true;

            StartCoroutine(MoveUpdate());
        }
    
        //Used by an animator event
        public void PlayerDash()
        {
            float tweenTime = Vector2.Distance(playerFloatPosition.Get(), transform.position) / dashSpeed;
            LeanTween.move(gameObject, (Vector2) playerFloatPosition.Get(), tweenTime).setOnComplete(() => onPlayerDashComplete.Invoke());
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (!_validBehaviourUpdate) return;
        
            //update hunter position
            var position = transform.position;
            hunterPos.Set(new Vector2Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y)));
        
            //kill animation condition
            if (PlayerIsHuntable && hunterPos.Get() == playerIntPosition.Get())
            {
                _validBehaviourUpdate = false;
                _animator.SetTrigger(KillPlayer);
                LeanTween.cancel(gameObject);
            }
        }
    
        private IEnumerator MoveUpdate()
        {
            while (_validBehaviourUpdate)
            {
                //set positions
                TileBehaviour currentTile = tiles.GetTileAt(hunterPos.Get());
                TileBehaviour nextTile;
                if (PlayerIsHuntable && GetNextPathfindingPosition(currentTile, playerIntPosition.Get(), playerChasePathfindingDepth, out TileBehaviour foundTile))
                {
                    nextTile = foundTile;
                }
                else
                {
                    nextTile = GetRandomDestination(currentTile);
                }

                //animations
                Vector2Int path = nextTile.position - currentTile.position;
                _animator.SetFloat(Horizontal, path.x);
                _animator.SetFloat(Vertical, path.y);

                //execute moving
                float tweenTime = Vector2.Distance(transform.position, nextTile.position) / walkSpeed;
                LeanTween.move(gameObject, nextTile.position, tweenTime).setOnComplete(() =>
                {
                    _animator.SetFloat(Horizontal, 0);
                    _animator.SetFloat(Vertical, 0);
                });
            
                yield return new WaitForSeconds(hunterBahviourUpdateTime);
            }
        }
    
        /// <summary>
        /// A simple Pathfinding algorithm that can search from a startTileBehaviour for a targetPosition by a tile depth.
        /// </summary>
        /// <param name="startTile">The position the pathfinding will start from</param>
        /// <param name="targetPosition">The position the pathfinding is trying to reach.</param>
        /// <param name="searchDepth">The amount of tiles away from the startPosition</param>
        /// <param name="nextTile">If a path was found this parameter will return the next tile to the target. Will return null if none found</param>
        /// <returns>Returns true if the targetPosition was found with the provided depth. False if not.</returns>
        private bool GetNextPathfindingPosition(TileBehaviour startTile, Vector2Int targetPosition, int searchDepth, out TileBehaviour nextTile)
        {
            foreach (var surroundingTile in posControl.GetConnectedTiles(startTile))
            {
                if (posControl.GetPathsByDepth_ExcludeParentPositions(surroundingTile, searchDepth - 1, startTile).Find(x => x.position == targetPosition))
                {
                    nextTile = surroundingTile;
                    return true;
                }
            }

            nextTile = null;
            return false;
        }

        /// <summary>
        /// This method will return a random new direction a entity can walk to.
        /// It will not take the tile it came from unless there is a dead end in the maze.
        /// </summary>
        /// <param name="currentTile">The position the randomization will start from</param>
        /// <returns>Returns a random tile around the currentTile.</returns>
        private TileBehaviour GetRandomDestination(TileBehaviour currentTile)
        {
            List<TileBehaviour> surroundingTiles = posControl.GetConnectedTiles(currentTile);
        
            if (_lastTile != null && surroundingTiles.Contains(_lastTile) && surroundingTiles.Count > 1)
            {
                surroundingTiles.Remove(_lastTile);
            }
            _lastTile = currentTile;
        
            return surroundingTiles[Random.Range(0, surroundingTiles.Count)];
        }
    }
}
