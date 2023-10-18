#include <stdio.h>
#include <locale.h>
#include <winsock2.h>
#pragma comment(lib, "Ws2_32.lib")

int main()
{
    setlocale(LC_ALL, "Russian");
    printf("Hello Client\n");

    WSADATA WsaData;//������������� ����������
    int err = WSAStartup(0x0101, &WsaData);//����� ������
    if (err == SOCKET_ERROR)//�������� �� ������
    {
        printf("WSAStartup() failed:% ld\n", GetLastError());
        return 1;
    }

    int s = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);//�������� ���������� ���� �����(���������� ������)

    //����� ��������� ��� ������
    SOCKADDR_IN anAddr;
    anAddr.sin_family = AF_INET;
    anAddr.sin_port = htons(80);
    anAddr.sin_addr.S_un.S_addr = inet_addr("127.0.0.1");
    
    int sz=connect(s, (LPSOCKADDR)&anAddr,sizeof(anAddr));//������������ ����� � ��������
    if (sz == -1) 
    {
        printf("������ �����������");
        return 1;
    }
    else
    {
        printf("����������� �������\n");

        //�������� ������� �����
        int x,nw;
        printf("������� ����� �����:");
        scanf_s("%d", &x);
        nw = htonl(x);
        if (send(s, &nw , sizeof(nw), 0) == -1) 
        {
            printf("Error");
        }

        //�������� ������� �����
        double y;
        printf("������� ������������ �����:");
        scanf_s("%lf", &y);
        char buf[8];
        snprintf(buf, sizeof(buf), "%lf", y);
        send(s, buf, sizeof(buf), 0);

        //�������� ������� �����
        recv(s, &x, sizeof(x), 0);
        x = ntohl(x);
        //�������� ������� �����
        recv(s, buf, sizeof(buf), 0);
        //�������� ����� �����
        char bufSum[8] = " ";
        recv(s, bufSum, sizeof(bufSum), 0);
        //�������� �����
        printf("%d + %s = %s", x, buf, bufSum);
        return 0;
    }
}