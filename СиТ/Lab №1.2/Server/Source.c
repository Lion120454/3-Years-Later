#include <stdio.h>
#include <locale.h>
#include <winsock2.h>
#pragma comment(lib, "Ws2_32.lib")

int main()
{
    setlocale(LC_ALL, "Russian");
    printf("Hello Server\n");

    WSADATA WsaData;//Инициализация библиотеки
    int err = WSAStartup(0x0101, &WsaData);//Выбор версии
    if (err == SOCKET_ERROR)//Проверка на ошибки
    {
        printf("WSAStartup() failed:% ld\n", GetLastError ());
        return 1;
    }

    int s = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);//Создание переменной типа сокет(дескриптер сокета)

    //Задаём параметры для сокета
    SOCKADDR_IN sin;
    sin.sin_family = AF_INET;
    sin.sin_port = htons(80);
    sin.sin_addr.s_addr = INADDR_ANY;

    err = bind(s, (LPSOCKADDR)&sin, sizeof(sin));//Определяем локальный адрес канала связи со средой
    err = listen(s, SOMAXCONN);//ожидаем запросы от клиента

    int len = sizeof(sin);



    while (listen(s, SOMAXCONN) ==0&& accept(s, (struct sockaddr*)&sin, &len)!=-1)
    {
        //err = listen(s, SOMAXCONN);//ожидаем запросы от клиента
        //printf("Проверка подключения...F\n");
        //s = accept(s, (struct sockaddr*)&sin, &len);//Установление связи с клиентом
        //printf("Клиент подключен\n");
        char bufIn[8];
        //Принимаем первое число
        recv(s, bufIn, sizeof(bufIn), 0);
        int arg1 = bufIn[0]-48;
        printf("Int Number= %d\n",arg1);

        send(s, "Good", sizeof("Good"), 0);

        //Принимаем второе число
        recv(s, bufIn, sizeof(bufIn), 0);
        double arg2 = ((bufIn[2]-48)*0.1)+(bufIn[0]-48);
        printf("Double Number= %lf\n\n",arg2);

        //Получаем сумму двух чисел
        double sum = arg1 + arg2;
        printf("Summa= %lf\n\n", sum);



        //Отправка первого числа
        char bufOut[8];
        snprintf(bufOut, sizeof(bufOut), "%d", arg1);    
        send(s, bufOut, sizeof(bufOut), 0);

        recv(s, bufIn, sizeof(bufIn), 0);

        //Отправка второго числа
        snprintf(bufOut, sizeof(bufOut), "%lf", arg2);    
        send(s, bufOut, sizeof(bufOut), 0);

        recv(s, bufIn, sizeof(bufIn), 0);

        //Отправка суммы чисел
        snprintf(bufOut, sizeof(bufOut), "%lf", sum);
        send(s, bufOut, sizeof(bufOut), 0);
    }
    return 0;
}