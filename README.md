Для запуска работоспособного приложения, осуществите следующие действия:
1. Настройка PostgreSQL:
- Требуется создать пользователя, под которым будет осуществляться авторизация (данные по-умолчанию: Логин - "ParusnikTestTaskUser", Пароль - "ParusnikTestTaskPassword")
    CREATE ROLE "ParusnikTestTaskUser" WITH
  	LOGIN
  	NOSUPERUSER
  	NOCREATEDB
  	NOCREATEROLE
  	NOINHERIT
  	NOREPLICATION
  	CONNECTION LIMIT -1
  	PASSWORD 'ParusnikTestTaskPassword';
- Требуется создать базу данных, в которой будет осуществляться работа (имя по-умолчанию "ParusnikTestTaskDB") с кодировкой UTF-8 и владельцем-пользователем созданным ранее
    CREATE DATABASE "ParusnikTestTaskDB" 
  	WITH OWNER = "ParusnikTestTaskUser"
  	ENCODING = 'UTF8'
  	CONNECTION LIMIT = -1;
2. Установка среды исполнения .NET:
  - CentOS: dnf install dotnet
  - Windows, Остальные дистрибутивы linux: ручная установка
  --- https://dotnet.microsoft.com/en-us/download/dotnet/8.0
3. Непосредственная сборка и запуск приложения:
  - Сборка и публикация одним из методов.
  - Запуск .dll-файла командой dotnet Backend.dll
4.* Дополнительная конфигурация файла appsettings.json уместна перед запуском после сборки.
    - Поле AppUrl:Host отвечает за ip-адрес, по которому будет работать приложение
    - Поле AppUrl:Port отвечает за порт приложения
   
============
Тестовый стенд запущен по адресу http://82.114.226.145:1444/swagger/index.html
