using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public enum EState
    {
        walkToPoster,
        walkToEntrance,
        walkToSecurity,
        walkToDeskGuide,
        walkToDesk,
        walkToBoxGuide1,
        walkToBoxGuide2,
        walkToBox,
        walkToChair,
        walkOut,
        scapeInFear,
        exit
    }

    public new Camera camera;
    public bool isTroll = false;
    public EState initState = EState.walkToPoster;
    public EState state = EState.walkToPoster;

    private NavMeshAgent navMeshAgent;
    private Animator animator;

    private WalkToPoster walkToPoster;
    private WalkToEntrance walkToEntrance;
    private WalkToSecurity walkToSecurity;
    private WalkToDeskGuide walkToDeskGuide;
    private WalkToDesk walkToDesk;
    private WalkToBoxGuide walkToBoxGuide;
    private WalkToBox walkToBox;
    private WalkToChair walkToChair;
    private WalkOut walkOut;
    private ScapeInFear scapeInFear;

    private bool isInFear = false;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        walkToPoster = GetComponent<WalkToPoster>();
        walkToEntrance = GetComponent<WalkToEntrance>();        
        walkToSecurity = GetComponent<WalkToSecurity>();
        walkToDeskGuide = GetComponent<WalkToDeskGuide>();
        walkToDesk = GetComponent<WalkToDesk>();
        walkToBoxGuide = GetComponent<WalkToBoxGuide>();
        walkToBox = GetComponent<WalkToBox>();
        walkToChair = GetComponent<WalkToChair>();
        walkOut = GetComponent<WalkOut>();
        scapeInFear = GetComponent<ScapeInFear>();
    }

    private IEnumerator MoveToPosition()
    {
        InitState();
        ConfigState();
        while (state != EState.exit)
        {
            if (CheckNeedToMove())
            {
                GetNextPosition();
                animator.SetBool("Wait", false);
                yield return new WaitForSeconds(.5f);
                while (navMeshAgent.enabled == false
                    || navMeshAgent.pathPending
                    || navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
                    yield return new WaitForSeconds(1);
                animator.SetBool("Wait", true);
            }
            else
                yield return new WaitForSeconds(1);
            if (CheckNextState())
            {
                if (ChangeState(GetNextState()))
                    InitState();
                ConfigState();
            }
        }
        End();
    }

    public void SetDestination(Vector3 position) => SetDestination(position, Vector3.zero);

    public void SetDestination(Vector3 position, Vector3 displacement, float stopDistance = 1f)
    {
        navMeshAgent.destination = position + displacement;
        navMeshAgent.stoppingDistance = stopDistance;
    }

    private bool ChangeState(EState state)
    {
        if (isInFear && state!= EState.exit)
            state = EState.scapeInFear;
        if (state == this.state)
            return false;
        this.state = state;
        return true;
    }

    private bool CheckNextState()
    {
        return state switch
        {
            EState.walkToPoster => walkToPoster.CheckNextState(isInFear),
            EState.walkToEntrance => walkToEntrance.CheckNextState(isInFear),
            EState.walkToSecurity => walkToSecurity.CheckNextState(isInFear),
            EState.walkToDeskGuide => walkToDeskGuide.CheckNextState(isInFear),
            EState.walkToDesk => walkToDesk.CheckNextState(isInFear),
            EState.walkToBoxGuide1 => walkToBoxGuide.CheckNextState(isInFear),
            EState.walkToBoxGuide2 => walkToBoxGuide.CheckNextState(isInFear),
            EState.walkToBox => walkToBox.CheckNextState(isInFear),
            EState.walkToChair => walkToChair.CheckNextState(isInFear),
            EState.walkOut => walkOut.CheckNextState(isInFear),
            EState.scapeInFear => scapeInFear.CheckNextState(isInFear),
            _ => false,
        };
    }

    private bool CheckNeedToMove()
    {
        return state switch
        {
            EState.walkToPoster => walkToPoster.CheckNeedToMove(),
            EState.walkToEntrance => walkToEntrance.CheckNeedToMove(),
            EState.walkToSecurity => walkToSecurity.CheckNeedToMove(),
            EState.walkToDeskGuide => walkToDeskGuide.CheckNeedToMove(),
            EState.walkToDesk => walkToDesk.CheckNeedToMove(),
            EState.walkToBoxGuide1 => walkToBoxGuide.CheckNeedToMove(),
            EState.walkToBoxGuide2 => walkToBoxGuide.CheckNeedToMove(),
            EState.walkToBox => walkToBox.CheckNeedToMove(),
            EState.walkToChair => walkToChair.CheckNeedToMove(),
            EState.walkOut => walkOut.CheckNeedToMove(),
            EState.scapeInFear => scapeInFear.CheckNeedToMove(),
            _ => false,
        };
    }

    private void InitState()
    {
        switch (state)
        {
            case EState.walkToPoster: walkToPoster.InitState(); break;
            case EState.walkToEntrance: walkToEntrance.InitState(); break;
            case EState.walkToSecurity: walkToSecurity.InitState(); break;
            case EState.walkToDeskGuide: walkToDeskGuide.InitState(); break;
            case EState.walkToDesk: walkToDesk.InitState(); break;
            case EState.walkToBoxGuide1: walkToBoxGuide.InitState(); break;
            case EState.walkToBoxGuide2: walkToBoxGuide.InitState(); break;
            case EState.walkToBox: walkToBox.InitState(); break;
            case EState.walkToChair: walkToChair.InitState(); break;
            case EState.walkOut: walkOut.InitState(); break;
            case EState.scapeInFear: scapeInFear.InitState(); break;
        }
    }

    private void GetNextPosition()
    {
        switch (state)
        {
            case EState.walkToPoster: walkToPoster.GetNextPosition(); break;
            case EState.walkToEntrance: walkToEntrance.GetNextPosition(); break;
            case EState.walkToSecurity: walkToSecurity.GetNextPosition(); break;
            case EState.walkToDeskGuide: walkToDeskGuide.GetNextPosition(); break;
            case EState.walkToDesk: walkToDesk.GetNextPosition(); break;
            case EState.walkToBoxGuide1: walkToBoxGuide.GetNextPosition(); break;
            case EState.walkToBoxGuide2: walkToBoxGuide.GetNextPosition(); break;
            case EState.walkToBox: walkToBox.GetNextPosition(); break;
            case EState.walkToChair: walkToChair.GetNextPosition(); break;
            case EState.walkOut: walkOut.GetNextPosition(); break;
            case EState.scapeInFear: scapeInFear.GetNextPosition(); break;
        }
    }

    private PlayerController.EState GetNextState()
    {
        return state switch
        {
            EState.walkToPoster => walkToPoster.GetNextState(),
            EState.walkToEntrance => walkToEntrance.GetNextState(),
            EState.walkToSecurity => walkToSecurity.GetNextState(),
            EState.walkToDeskGuide => walkToDeskGuide.GetNextState(),
            EState.walkToDesk => walkToDesk.GetNextState(),
            EState.walkToBoxGuide1 => walkToBoxGuide.GetNextState(),
            EState.walkToBoxGuide2 => walkToBoxGuide.GetNextState(),
            EState.walkToBox => walkToBox.GetNextState(),
            EState.walkToChair => walkToChair.GetNextState(),
            EState.walkOut => walkOut.GetNextState(),
            EState.scapeInFear => scapeInFear.GetNextState(),
            _ => EState.exit,
        };
    }

    private void ConfigState()
    {
        switch (state)
        {
            case EState.walkToPoster: walkToPoster.ConfigState(); break;
            case EState.walkToEntrance: walkToEntrance.ConfigState(); break;
            case EState.walkToSecurity: walkToSecurity.ConfigState(); break;
            case EState.walkToDeskGuide: walkToDeskGuide.ConfigState(); break;
            case EState.walkToDesk: walkToDesk.ConfigState(); break;
            case EState.walkToBoxGuide1: walkToBoxGuide.ConfigState(); break;
            case EState.walkToBoxGuide2: walkToBoxGuide.ConfigState(); break;
            case EState.walkToBox: walkToBox.ConfigState(); break;
            case EState.walkToChair: walkToChair.ConfigState(); break;
            case EState.walkOut: walkOut.ConfigState(); break;
            case EState.scapeInFear: scapeInFear.ConfigState(); break;
        }
    }

    public void SetPoster(Poster poster) => walkToPoster.poster = poster;

    public void SetBox(Box box) => walkToBox.box = box;

    public void SetDesktop(Desktop d) => walkToDesk.desktop = d;

    public void Sit(Vector3 forward)
    {
        navMeshAgent.enabled = false;
        transform.forward = forward;
        animator.SetBool("Sitting", true);
    }

    public void Up()
    {
        navMeshAgent.enabled = true;
        animator.SetBool("Sitting", false);
    }

    private void End() => transform.DOShakeScale(1f).OnComplete(Requeue);

    private void Requeue()
    {
        gameObject.SetActive(false);
        if (!isTroll)
            GameManager.Queue(this);
    }

    public static void Init(PlayerController playerController)
    {
        playerController.animator.SetBool("Run", false);
        playerController.navMeshAgent.speed = 2;
        playerController.isInFear = false;

        playerController.state = playerController.initState;

        Poster[] list = FindObjectsOfType<Poster>().Where(x => x.human == false).ToArray();
        int i = Random.Range(0, list.Length);
        playerController.walkToPoster.hour = list[i].hour;
        playerController.walkToPoster.minute = list[i].minute;

        playerController.transform.position = new Vector3(Random.Range(-89, -70), .3f, Random.Range(-89, -85f));
        playerController.gameObject.SetActive(true);
        playerController.transform.DOShakeScale(1f);
        playerController.Begin();
    }

    private void Begin() => StartCoroutine(MoveToPosition());

    public void Fear()
    {
        animator.SetBool("Run", true);
        navMeshAgent.speed = 4;
        isInFear = true;
    }
}