using UnityEngine;

[CreateAssetMenu(fileName = "DashImproved",
    menuName = "Skills/DashImproved")]
public class DashImproved : SkillBase {
    public float minDistance;
    public float maxDistance;
    public float channelTimeForMaxDistance;
    public GameObject TargetPositionIndicator;
    public float heightOffset;
    private GameObject currentTargetPositionIndicator;
    [SerializeField] private float currentChannelTime;
    private Vector3 offset;
    private Vector2 targetDestination;
    public GameObject particleSystem;
    private ParticleSystem ps;
    private PlayerStateManager PSManager;
    private Vector2 startPosition;


    protected override bool OnButtonDown() {
        if(PSManager == null)
        {
            PSManager = GameManager.Instance.PSManager;
        }
        startPosition = character.transform.position;
        currentChannelTime = 0;
        offset = Vector3.up * heightOffset;
        currentTargetPositionIndicator = GameObject.Instantiate(TargetPositionIndicator,
            character.transform.position + offset, Quaternion.identity);
        character.GetComponent<PlayerStateManager>().SetSlowed(true);
        PSManager.anim.SetBool("transBat", true);

        return false;
    }

    protected override void OnChannelEnd() {
        character.transform.position = DetermineCurrentTargetPosition(true);       
        character.GetComponent<PlayerStateManager>().SetSlowed(false);
        Destroy(currentTargetPositionIndicator);
        StartPSSystem();
        PSManager.anim.SetBool("transBat", false);
        //PSManager.anim.SetTrigger("teleport");
    }

    public void StartPSSystem()
    {
        ps = GameObject.Instantiate(particleSystem, startPosition, Quaternion.identity).GetComponent<ParticleSystem>();
        ps.GetComponent<MoveParticlesToTarget>().Target = character;       
    }

    private Vector3 DetermineCurrentTargetPosition(bool correction) {
        float dashDistance =
            minDistance + (maxDistance - minDistance) * (currentChannelTime / channelTimeForMaxDistance);
        SpriteRenderer spriteRenderer = currentTargetPositionIndicator.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) {
            Debug.LogWarning(character + " has no SpriteRenderer, is this correct?");
        }

        PlayerStateManager stateManager = character.GetComponent<PlayerStateManager>();
        if (stateManager == null) {
            Debug.LogWarning(character + " has no PlayerStateManager, is this correct?");
        }

        RaycastHit2D hit = Physics2D.BoxCast(character.transform.position + offset, spriteRenderer.bounds.size, 0,
            stateManager.lookingLeft ? -character.transform.right : character.transform.right, dashDistance,
            GameLayer.WallMask);
        Vector2 target;
        if (hit.collider == null) {
            target = character.transform.position +
                     (stateManager.lookingLeft ? -character.transform.right : character.transform.right).normalized *
                     dashDistance;
        }
        else {
            target = hit.centroid - (Vector2) offset;
        }

        if (correction) {
            target = CorrectPositionToFreeSapce(target, stateManager);
        }
        targetDestination = target;
        return target;
    }

    protected override bool OnChannelUpdate() {
        currentChannelTime += Time.deltaTime;
        if (currentChannelTime >= channelTimeForMaxDistance) {
            currentChannelTime = channelTimeForMaxDistance;
        }

        currentTargetPositionIndicator.transform.position = DetermineCurrentTargetPosition(false)+offset;
        SpriteRenderer indicatorRenderer = currentTargetPositionIndicator.GetComponent<SpriteRenderer>();
        if (indicatorRenderer != null) {
            indicatorRenderer.flipX = character.GetComponent<PlayerStateManager>().lookingLeft;
        }

        if (currentChannelTime >= channelTimeForMaxDistance) {
            return true;
        }

        return false;
    }

    private Vector2 CorrectPositionToFreeSapce(Vector2 position, PlayerStateManager playerState) {
        Collider2D overlap;
        do {
            overlap = Physics2D.OverlapBox(position, character.GetComponent<Collider2D>().bounds.size*0.9f, 0,GameLayer.WallMask);
            if (overlap == null) continue;
            if (playerState.lookingLeft) {
                position.x = overlap.bounds.max.x + character.GetComponent<Collider2D>().bounds.extents.x+0.1f;
            }
            else {
                position.x = overlap.bounds.min.x - character.GetComponent<Collider2D>().bounds.extents.x-0.1f;
            }
        } while (overlap != null);
        
        //do not tp backward
        if ((character.GetComponent<PlayerStateManager>().lookingLeft && position.x > character.transform.position.x) ||
            (!character.GetComponent<PlayerStateManager>().lookingLeft &&
             position.x < character.transform.position.x)) {
            position.x = character.transform.position.x;
        }
        return position;
    }

    protected override void OnAddToCharacter() {
    }
}