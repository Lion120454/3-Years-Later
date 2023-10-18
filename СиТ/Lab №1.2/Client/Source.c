#include <stdio.h>
#include <locale.h>
#include <winsock2.h>
#pragma comment(lib, "Ws2_32.lib")

int main()
{
    setlocale(LC_ALL, "Russian");
    printf("Hello Client\n");

    WSADATA WsaData;//Инициализация библиотеки
    int err = WSAStartup(0x0101, &WsaData);//Выбор версии
    if (err == SOCKET_ERROR)//Проверка на ошибки
    {
        printf("WSAStartup() failed:% ld\n", GetLastError());
        return 1;
    }

    int s = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);//Создание переменной типа сокет(дескриптер сокета)

    //Задаём параметры для сокета
    SOCKADDR_IN anAddr;
    anAddr.sin_family = AF_INET;
    anAddr.sin_port = htons(80);
    anAddr.sin_addr.S_un.S_addr = inet_addr("127.0.0.1");
    
    int sz=connect(s, (LPSOCKADDR)&anAddr,sizeof(anAddr));//Установление связи с сервером
    if (sz == -1) 
    {
        printf("Ошибка подключения");
        return 1;
    }
    else
    {
        printf("Подключение успешно\n");

        //Отправка первого числа
        int x,nw;
        printf("Введите целое число:");
        scanf_s("%d", &x);
        nw = htonl(x);
        if (send(s, &nw , sizeof(nw), 0) == -1) 
        {
            printf("Error");
        }

        //Отправка второго числа
        double y;
        printf("Введите вещественное число:");
        scanf_s("%lf", &y);
        char buf[8];
        snprintf(buf, sizeof(buf), "%lf", y);
        send(s, buf, sizeof(buf), 0);

        //Принятие первого числа
        recv(s, &x, sizeof(x), 0);
        x = ntohl(x);
        //Принятие второго числа
        recv(s, buf, sizeof(buf), 0);
        //Принятие суммы чисел
        char bufSum[8] = " ";
        recv(s, bufSum, sizeof(bufSum), 0);
        //Итоговый вывод
        printf("%d + %s = %s", x, buf, bufSum);
        return 0;
    }
}