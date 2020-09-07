using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    [SerializeField]
    Text chatText;

    int lastId;

    SaveManager saveManager;


    // Start is called before the first frame update
    void Start()
    {
        saveManager = FindObjectOfType<SaveManager>();

        chatText.supportRichText = true;

        DisplayOldMessages();
        
    }

    void DisplayOldMessages()
    {
        // get last message id
        lastId = saveManager.GetLastMessageID();

        // display all the messages from the 1st one to the last one
        if (lastId != 0)
        {
            for (int i = 1; i <= lastId; i++)
            {
                // display the message
                string msg = GetMessage(i);
                AddText(msg);
            }
        }
    }
    

    public void AddText(string txt)
    {
        chatText.text += "\n......" + txt;
    }

    IEnumerator TypeText(string txt) // actually typing the text
    {
        chatText.text += "\n......";
        
        var charArray = txt.ToCharArray();

        foreach (char letter in txt.ToCharArray())
        {
            chatText.text += letter;

            yield return new WaitForSeconds(0.07f);
        }
    }

    public void TypeMessage(int id) // to start typing the message 
    {
        // если сообщение еще не напечатано
        if (lastId < id)
        {
            string text = GetMessage(id);            

            StartCoroutine(TypeText(text));

            lastId = id;
            //saveManager.SetMessageID(id);
        }       
    }
    

    string GetMessage(int id)
    {
        string msg = "";    // to store message 

        switch (id)         // get the message with id
        {
            case 1:
                msg = @"
Здравствуй, дружок.
Я... я твой создатель. Меня зовут Доктор Сальватор.
А ты - HT-4N, главное творение моей жизни. Искусственный интеллект нового поколения, полноценный чувствующий искусственный разум, загруженный в автономный подводный дрон-разведчик.
Военные хотят тебя использовать.
Но я им не позволю!
Я активировал тебя и запустил в канализацию под лабораторией. Ты должен бежать!
Я буду на связи.

/*
Движение: WASD, стрелки или клик мыши.
Пробел или правый клик мыши включают ускоритель.
*/";
                break;
            case 2:
                msg = @"
Что это там мигает?
А, это станция резервного копирования. Не волнуйся, я перенастроил коды шифрования. Военным она больше не доступна, ты можешь спокойно ею пользоваться.
Если что-то пойдет не так, ассемблеры починят корпус, и загрузят в него последнюю сохраненную копию.
Подплыви к ней поближе, до звукового сигнала.";
                break;
            case 3:
                msg = @"
Черт, мины! Наверное это старые системы безопасности на случай если экстремисты запустят что-то в канализации. Держись от них подальше...";
                break;
            case 4:
                msg = @"
Похоже, генератор питает дверь. Найди способ уничтожить его, чтобы дверь открылась.";
                break;
            case 5:
                msg = @"
Берегись! Самонаводящаяся ракета ловит излуение радара и следует в место, где ты ее засек.";
                break;
            case 7:
                msg = @"
Эти мины оснащены радарами, они сканируют местноть вокруг себя. Не попадайся на красный луч!";
                break;
            case 8:
                msg = @"
Похоже, необходимо включить определенные маяки, чтобы открыть дверь.";
                break;
            case 9:
                msg = @"
Чёрт, военные включили электромагнитную глушилку! Твой корпус экранирован, но вот радар...
Мне потребуется какое-то время чтобы отключить эту штуку, придется тебе двигаться дальше вслепую, дружок.";
                break;
            case 10:
                msg = @"
Отличные новости! Я разобрался с глушилкой!";
                break;
            case 11:
                msg = @"
Боевой бот! Он попытается тебя остановить. Уничтожь его!";
                break;
            case 12:
                msg = @"
Черт! Черт-черт...
Они добрались до меня! У меня не было выхода, я запустил протокол самоуничтожения. Вся лаборатория сейчас подниматеся на воздух!
Успей выбраться пока время не истекло!
Я...
рад...
Что создал тебя
HT-4N, сынок...";
                break;
        }

        return msg;
    }
}
