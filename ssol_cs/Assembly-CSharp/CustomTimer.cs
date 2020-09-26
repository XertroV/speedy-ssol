using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class CustomTimer
{
    public MonoBehaviour target;
    public DateTime startT;
    private bool alwaysPrint;

    public CustomTimer(MonoBehaviour target, bool alwaysPrint)
    {
        this.target = target;
        this.startT = DateTime.Now;
        this.alwaysPrint = alwaysPrint;
    }

    public void TimeSince()
    {
        TimeSince(this.startT);
    }

    // Token: 0x0600008C RID: 140 RVA: 0x000025FE File Offset: 0x000007FE
    public void TimeSince(DateTime earlier)
    {
        this.TimeSince(earlier, string.Empty, string.Empty, false);
    }

    // Token: 0x0600008D RID: 141 RVA: 0x00002612 File Offset: 0x00000812
    public void TimeSince(string msgPrefix)
    {
        this.TimeSince(this.startT, msgPrefix, string.Empty, false);
    }

    // Token: 0x0600008D RID: 141 RVA: 0x00002612 File Offset: 0x00000812
    public void TimeSince(DateTime earlier, string msgPrefix)
    {
        this.TimeSince(earlier, msgPrefix, string.Empty, false);
    }

    // Token: 0x0600008E RID: 142 RVA: 0x00002622 File Offset: 0x00000822
    public void TimeSince(DateTime earlier, string msgPrefix, string msgSuffix)
    {
        this.TimeSince(earlier, msgPrefix, msgSuffix, false);
    }

    public void TimeSince(string msgPrefix, string msgSuffix, bool alwasyPrint)
    {
        this.TimeSince(this.startT, msgPrefix, msgSuffix, alwasyPrint);
    }

    // Token: 0x0600008F RID: 143 RVA: 0x0000B838 File Offset: 0x00009A38
    public void TimeSince(DateTime earlier, string msgPrefix, string msgSuffix, bool alwasyPrint)
    {
        DateTime now = DateTime.Now;
        double totalSeconds = now.Subtract(earlier).TotalSeconds;
        string text = string.Empty;
        if (totalSeconds > 0.4 || alwasyPrint || this.alwaysPrint)
        {
            text = string.Concat(new string[]
            {
                text,
                text,
                "Time: ",
                now.ToLongTimeString(),
                " | Start took: ",
                totalSeconds.ToString("00.000")
            });
        }
        if (totalSeconds > 0.4 || alwasyPrint || this.alwaysPrint)
        {
            text = text + "\n" + string.Join(" <|> ", new string[]
            {
                this.target.name,
                this.target.tag,
                this.ToString()
            });
        }
        text = string.Join("\n", new string[]
        {
            msgPrefix,
            text,
            msgSuffix
        });
        if (text.Length > 2 || alwasyPrint || this.alwaysPrint)
        {
            Debug.Log(text);
        }
    }
}
