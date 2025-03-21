# Пиши-стирай
Данное приложение разработано в рамках учебной практики 4 курса. Приложение предоставляет функционал для работы с заказами в магазине по продаже канцелярских товаров ООО «Пиши-стирай».  
   
## Начало работы  
Эти инструкции предоставят вам копию проекта и помогут запустить на вашем компьютере для разработки и тестирования.  
### Необходимые условия  
* Операционная система: Windows 7 и выше  
* Процессор: 2 физических и 2 ГГц и больше  
* Оперативная память: 8 гигабайта и больше  
* Место на диске: 2 гигабайта свободного пространства и больше   
* IDE Visual Studio  
* [.Net 9.0 и выше](https://dotnet.microsoft.com/en-us/)  
* [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/cli/dotnet)  
* [шаблоны Avalonia для сборки проекта](https://avaloniachina.github.io/avalonia-docs/ru/docs/get-started/install/)  
    
### Установка с помощью Visual Studio  
1. Откройте Visual Studio  
2. Нажмите "Клонировать репозиторий"  
3. Вставьте ссылку на репозиторий  
4. Запустите проект  
   
### Примечание   
Для работы приложения создайте базу данных и заполните таблицы с помощью скрипта "database.txt".  
В классе YpContext (папка Models) нужно заменить строку подключения "UseNpgsql" на подключение к созданной вами базе.  
  
## Используемые технологии в проекте   
Приложении реализовано в соответсвии с паттерном MVVM и CommunityToolkit.  
* C# DotNETCore 9.0;   
* AvaloniaUI Framework;   
* EntityFrameworkCore;   
* PostgreSQL.  
  
## Описание коммитов  
| Название | Описание                                                             |  
| -------- | -------------------------------------------------------------------- |  
| docs     | обновление документации (readme)                                     |  
| feat     | добавление нового функционала                                        |  
| fix      | исправление ошибок                                                   |  
| test     | добавление unit-тестов к проекту или тестовой документации           |  
   
## Автор  
**Токарева Элина** - *Пиши-стирай* - [OtterUnderwater](https://gogs.ngknn.ru/Otter)  