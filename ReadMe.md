Способы запуска

Общие шаги

1) Клонировать(fork) репозиторий git из visual studio.

2) Поднять базу данных PostgreSQL, для конфигурации используйте SQLDbScripst.txt

Способ 1:
	Выберите(возле зеленой стрелочки запуска в режиме отладки) запуск http вместо Container (Dockerfile)
	Ctrl + F5(Запуск без отладки)
	Можно тестировать WebApi с помощью PostMan

Способ 2:
	Выберите(возле зеленой стрелочки запуска в режиме отладки) запуск Container (Dockerfile)
	Ctrl + F5(Запуск без отладки), обратите внимание, вам понадобится Docker Desktop
	Обратите внимание, visual studio использует любой свободный порт.
	Можно тестировать WebApi с помощью PostMan

Способ 3:
	Вынести файл Dockerfile на одну директорию выше, в расположение .sln файла.
	Из этой папки TestVit запустить cmd
		docker build -t testvit .
		Потом указываете строку подключения и номер порта в команде docker run, пример команды: 
			docker run -d -p 8080:8080 -e ConnectionStrings__Default="Host=host.docker.internal;Port=5432;Database=blogvit;Username=postgres;Password=connectionpassword" --name testvit-container testvit
	Можно тестировать WebApi с помощью PostMan