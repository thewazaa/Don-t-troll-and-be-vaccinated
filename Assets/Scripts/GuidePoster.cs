using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuidePoster : MonoBehaviour
{
    public enum EChangePoster
    {
        none,
        poster3,
        poster2,
        poster1
    }

    public EChangePoster changePoster = EChangePoster.none;
    public Poster poster1, poster2, poster3;
    public float waitTime = 60 * 9;

    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private TimeControl timeControl;
    private Poster poster;
    private Speak speak;
    private GuideEntrance guideEntrance;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        poster = GetComponent<Poster>();
        speak = GetComponentInChildren<Speak>();
    }

    void Start()
    {
        guideEntrance = FindObjectOfType<GuideEntrance>();
        timeControl = FindObjectOfType<TimeControl>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.destination = poster3.transform.position - poster3.transform.right;
        navMeshAgent.stoppingDistance = 2;
        StartCoroutine(Walk());
    }

    private void Update()
    {
        if (waitTime <= timeControl.currentTime)
        {
            waitTime += 15;
            changePoster = EChangePoster.poster3;
            StartChangePoster();
        }
    }

    private void StartChangePoster() => animator.SetTrigger("ChangePoster");

    public void EndChangePoster()
    {
        List<PlayerController> tmp = poster.queue;
        switch (changePoster)
        {
            case EChangePoster.poster3:
                poster.UpdateText(poster3.Time, poster3.queue);
                poster3.UpdateText(poster3.Time + 15, tmp);
                navMeshAgent.destination = poster2.transform.position - poster2.transform.right;
                speak.ShowText(false, $"The guys from {poster.hour:00}:{poster.minute:00}, follow me");
                break;
            case EChangePoster.poster2:
                poster.UpdateText(poster2.Time, poster2.queue);
                poster2.UpdateText(poster2.Time + 15, tmp);
                navMeshAgent.destination = poster1.transform.position - poster1.transform.right;
                speak.ShowText(false, $"The guys from {poster.hour:00}:{poster.minute:00}, follow me");
                break;
            case EChangePoster.poster1:
                foreach (PlayerController x in poster1.queue)
                    guideEntrance.queue.Add(x);
                poster.UpdateText(poster1.Time + 60, null);
                poster1.UpdateText(poster1.Time + 15, tmp);
                navMeshAgent.destination = poster3.transform.position - poster3.transform.right;
                speak.ShowText(false, $"The guys from {poster.hour:00}:{poster.minute:00}, go on");
                break;
        }
        StartCoroutine(Walk());
    }

    private IEnumerator Walk()
    {
        bool wait = false;
        animator.SetBool("Wait", false);
        while (wait == false)
        {
            if (!navMeshAgent.pathPending
                && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance
                && (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f))
                wait = true;
            yield return new WaitForSeconds(.5f);
        }
        animator.SetBool("Wait", true);

        changePoster = changePoster switch
        {
            EChangePoster.poster3 => EChangePoster.poster2,
            EChangePoster.poster2 => EChangePoster.poster1,
            EChangePoster.poster1 => EChangePoster.none,
            _ => EChangePoster.none,
        };
        if (changePoster != EChangePoster.none)
            StartChangePoster();
    }
}