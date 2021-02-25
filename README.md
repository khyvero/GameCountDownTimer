# GameCountDownTimer
Count Down Timer utility for C#/Unity games/Monogame


- To count dowm some logic do the follow: 
#####
           CountDownTimer.GetInstance().CountDown("name_of_timer", 60f, (t) =>    //name of timer is opttional
           {
                TimerNum.text = string.Format("{0:0}", t);
           }, () =>
           {
                DebugLog("Time Is Up");
           });
        
- To add more time in same timer
#####
           CountDownTimer.GetInstance().AddTime("name_of_timer", 50f);             //must have the same name of timer
            
- To remove all timers
#####
           CountDownTimer.GetInstance().ClearAllTimers();
           
