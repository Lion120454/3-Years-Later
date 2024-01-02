import socket

# Создаем сокет
print("Client started")
sock = socket.socket()

#Подключаемся к серверу
sock.connect(('127.0.0.1', 80))

# Отправляем данные серверу
bufInt=input()
bufFloat=input()
sock.send(bufInt.encode('utf-8'))

print(sock.recv(1024).decode('utf-8'))

# Получаем данные от сервера
sock.send(bufFloat.encode('utf-8'))

sock.recv(1024)#Первое число
sock.send("1 Yes".encode('utf-8'))
sock.recv(1024).decode('utf-8')#Второе число
sock.send("2 Yes".encode('utf-8'))
print(sock.recv(1024).decode('utf-8'))#Сумма
# Закрываем подключение после обработки всех данных сервера
sock.close()
print("Client Sleep")