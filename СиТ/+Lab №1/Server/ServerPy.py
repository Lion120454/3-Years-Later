import socket
import json

# Создаем сокет
print('Server started')
sock = socket.socket()
sock.bind(('127.0.0.1', 80))
sock.listen(1)

# Принимаем подключение
conn, addr = sock.accept()
print('Connected:', addr)

# Получаем данные от клиента
Buffer = conn.recv(1024)
Buffer = json.loads(Buffer.decode('utf-8'))
print("Первое число = ",Buffer['Int'])
print("Второе число =",Buffer['Float'])

#Отправляем данные клиенту
Buffer['sum']=float(Buffer['Int'])+float(Buffer['Float'])
conn.send(json.dumps(Buffer).encode('utf-8'))

# Закрываем подключение после обработки всех данных клиента
conn.close()
sock.close()
print('Server stopped')
