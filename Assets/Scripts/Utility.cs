using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Utility
{
    public static void ShowAlertWindow(string title, string content, UnityAction closeAction)
    {
        UIAlertWindow.Show(title, content, closeAction);
    }

    public static void ShowAlertWindow(string title, string content, UnityAction yesAction, UnityAction noAction)
    {
        UIAlertWindow.Show(title, content, yesAction, noAction);
    }
}
