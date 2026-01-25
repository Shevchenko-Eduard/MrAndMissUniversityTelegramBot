# Страшные вещи

## Если нету BD

### Если отсутствуют миграции

`dotnet tool install --global dotnet-ef`
Это для установки инструмента миграции.
При необходимости следовать рекомендациям в терминале.

`dotnet ef migrations add Migration01`
Создаем первую миграцию. И сразу же вторую (так надо).
`dotnet ef migrations add Migration02`

### Нету BD

`dotnet ef database update`
Создаем/обновляем BD.

## Запуск бота

В файле `Program.cs` замените `<YourToken>` на ваш токен, и запустите программу через терминал: `dotnet run`
