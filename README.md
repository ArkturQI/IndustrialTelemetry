# IndustrialTelemetry

Backend для промышленной телеметрии на ASP.NET Core.

## Фичи
- Clean Architecture
- Идемпотентность запросов
- Optimistic Concurrency
- PostgreSQL + EF Core

## Architecture
```
IndustrialTelemetry./
├── Domain/                 # Бизнес-логика
├── Application/            # Сервисы и DTOs
├── Infrastructure/         # Работа с БД
└── API/                    # Web API
```

## Запуск
```bash
git clone: https://github.com/ArkturQI/IndustrialTelemetry.git
cd IndustrialTelemetry
```
## Swagger
```
http://localhost:5005/swagger
```
