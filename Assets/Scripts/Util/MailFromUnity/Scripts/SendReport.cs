using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.UI;

public class SendReport : MonoBehaviour
{
    public static SendReport instance;
    string From = "game@aemaze.studio";
    string Name = "Aemaze";
    public string To = "game.log@aemaze.studio";
    public string Subject = "Game Log";
    public string Message = "Game report log";
    
    public string AttachmentFilename = "logo.jpg";

    public void Start() 
    {
        instance = this;
    }

    public void SendPlainMail()
    {
        MailSingleton.Instance.SendPlainMail(
            From, 
            Name, 
            To, 
            Subject, 
            Message
        );
    }

    public void SetAndSendMessage (string playerName, string playerReputation)
    {
        Subject = "Raport: " + playerName + " wybrał opcję " + playerReputation ; 
        Message = playerName + " wybrał opcję " + playerReputation + " o godzinie: " + System.DateTime.Now; 
        SendPlainMail();
    }
}
