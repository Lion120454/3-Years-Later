import socket
from html.parser import HTMLParser

server = 'ftp.vim.org'

class MyHTMLParser(HTMLParser):
    def __init__(self):
        super().__init__()
        self.links = []

    def handle_starttag(self, tag, attrs):
        if tag == 'a':
            for attr in attrs:
                if attr[0] == 'href':
                    self.links.append(attr[1])

def get_headers(host, path):
    with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
        s.connect((host, 80))
        request = f"HEAD {path} HTTP/1.1\r\nHost: {host}\r\n\r\n"
        s.sendall(request.encode())

        response = b""
        while True:
            data = s.recv(1024)
            if not data:
                break
            response += data

    # Разделяем заголовки от тела ответа
    headers, _ = response.split(b'\r\n\r\n', 1)
    return headers.decode()

def get_file(host, path):
    with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
        s.connect((host, 80))
        request = f"GET /{path} HTTP/1.1\r\nHost: {host}\r\n\r\n"
        s.sendall(request.encode())

        response = b""
        while True:
            data = s.recv(1024)
            if not data:
                break
            response += data

    return response


# Получаем заголовки ответа
headers = get_headers(server, '/')
print("Заголовки:")
print(headers)
print()

# Получаем содержимое HTML-страницы
html_content = get_file(server, '/').decode()

# Извлекаем путь до файла из HTML-страницы
parser = MyHTMLParser()
parser.feed(html_content)
links = parser.links

file_server = None
for link in links:
    if link.endswith('.nluug'):
        file_server = link
        print(file_server)

if file_server:
    file_content = get_file(server, file_server)
    with open('download/document.txt', 'wb') as file:
        file.write(file_content)
    print("Файл успешно скачан")
else:
    print("Файл не найден")