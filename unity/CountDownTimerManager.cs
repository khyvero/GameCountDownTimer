using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownTimerManager : MonoBehaviour
{
    private static CountDownTimerManager Instance;

    private Dictionary<string, TimerWrapper> Timers = new Dictionary<string, TimerWrapper>();

    private bool removeAll;

    private void Start()
    {
        Instance = this;
        DontDestroyOnLoad(this);
        this.removeAll = false;
    }

    /// <summary>
    /// doing an action after timeRemaining is 0
    /// </summary>
    /// <param name="timeRemaining">time</param>
    /// <param name="doOntimme">action</param>
    public void CountDown(float timeRemaining,  DoOnTime doOntimme)
    {
        string id = System.Guid.NewGuid().ToString();

        this.CountDown(id, timeRemaining, (t)=> { }, doOntimme);
    }

    /// <summary>
    /// do an action every time and doing an action after timeRemaining is 0
    /// </summary>
    /// <param name="timeRemaining">time</param>
    /// <param name="publishTimeRemaining">do an action when the timer update</param>
    /// <param name="doOntimme">action</param>
    public void CountDown(float timeRemaining, PublishTimeRemaining publishTimeRemaining , DoOnTime doOntimme)
    {
        string id = System.Guid.NewGuid().ToString() ;

        this.CountDown(id, timeRemaining, publishTimeRemaining, doOntimme);
    }

    /// <summary>
    /// give an id for the timer(for easier control different timer), doing an action after timeRemaining is 0
    /// </summary>
    /// <param name="id">name of the timer</param>
    /// <param name="timeRemaining">time</param>
    /// <param name="doOntimme">action</param>
    public void CountDown(string id, float timeRemaining,  DoOnTime doOntimme)
    {
        if (Timers.ContainsKey(id))
        {
            return;
        }

        TimerWrapper wrapper = new TimerWrapper(id, timeRemaining, (t) => { }, doOntimme);

        Timers.Add(id, wrapper);
    }

    /// <summary>
    /// give an id for the timer(for easier control different timer), do an action every time and doing an action after timeRemaining is 0
    /// </summary>
    /// <param name="id">name of the timer</param>
    /// <param name="timeRemaining">time</param>
    /// <param name="publishTimeRemaining">do an action when the timer update</param>
    /// <param name="doOntimme">action</param>
    public void CountDown(string id, float timeRemaining, PublishTimeRemaining publishTimeRemaining, DoOnTime doOntimme)
    {
        if (Timers.ContainsKey(id))
        {
            return;
        }

        TimerWrapper wrapper = new TimerWrapper(id, timeRemaining, publishTimeRemaining, doOntimme);

        Timers.Add(id, wrapper);
    }

    /// <summary>
    /// add or reduce time on the specific timer
    /// </summary>
    /// <param name="id">name of the timer</param>
    /// <param name="timeToAdd">time you want to add or reduce</param>
    public void AddTime(string id, float timeToAdd)
    {
        if (!Timers.ContainsKey(id))
        {
            return;
        }

        Timers[id].UpdateTime(-timeToAdd);
    }

    /// <summary>
    /// remove all timers form Dictionary
    /// </summary>
    public void RemoveAllTimers()
    {
        this.removeAll = true;
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;
        List<string> dones = new List<string>();

        if(removeAll == true)
        {
            Timers.Clear();
            this.removeAll = false;
        }

        foreach(string timerKey in Timers.Keys)
        {
            TimerWrapper wrapper = Timers[timerKey];
            if (wrapper.TimeIsUp())
            {
                wrapper.DoTask();
                dones.Add(timerKey);
            }
            else
            {
                wrapper.UpdateTime(deltaTime);
            }
        }

        foreach (string done in dones)
        {
            Timers.Remove(done);
        }
    }

    

    public class TimerWrapper
    {
        private DoOnTime DoOnTime;
        private PublishTimeRemaining publishTimeRemaining;
        private float timeRemaining;
        private bool Done;
        private string id;

        public TimerWrapper(string id ,float timeRemaining, PublishTimeRemaining publishTimeRemaining, DoOnTime doOnTime)
        {
            this.timeRemaining = timeRemaining;
            this.DoOnTime = doOnTime;
            this.id = id;

            if(publishTimeRemaining == null)
            {
                this.publishTimeRemaining = (t) => { Debug.Log("Time Remaining For Timer:" + id + " is: " + t); };
            }
            else
            {
                this.publishTimeRemaining = publishTimeRemaining;
            }
        }

        public void UpdateTime(float DeltaTime)
        {
            timeRemaining -= DeltaTime;
            publishTimeRemaining.Invoke(timeRemaining);
        }

        public bool TimeIsUp()
        {
            return timeRemaining <= 0;
        }

        public void DoTask()
        {
            if (!Done)
            {
                Debug.Log("Doing timer job");
                DoOnTime.Invoke();
                Done = true;
            }
        }
    }

    
    public static CountDownTimerManager GetInstance()
    {
        return Instance;
    }


    public delegate void DoOnTime();
    public delegate void PublishTimeRemaining(float timeRemaining);
    
}
