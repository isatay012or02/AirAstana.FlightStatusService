# AirAstana.FlightStatusService

Инструкция по пользованию Сервисом по получению данных о статусе рейсов

### 1) Получения токена
**Request**
```
curl -X POST http://localhost:5238/api/v1/auth/login \
--param 'username="{username}"' \
--param 'password="{password}"'
```
- {username} - имя пользователя 
- {password} - пароль пользователя

**Response**
```
{Token: {token}}
```
- {token} - токен

### 2) Получения списка рейсов
```
curl -X Get http://localhost:5238/api/v1/flight/list \
Request Header
Authorization: Bearer: {token}
```
Где
- {token} - токен полученный в пункте 1
```
Request Params
--param 'origin="{origin}"' \
--param 'destination="{destination}"'
```
**Response**
```
Response Body
{
    [
        {
            "origin": "string",
            "destination": "string",
            "departure": "2024-04-14T12:24:02.352Z",
            "arrival": "2024-04-14T12:24:02.352Z",
            "status": "InTime",
            "id": 0
        }
    ]
}
```

### 3) Добавление рейсов
```
curl -X POST http://localhost:5238/api/v1/flight \
Request Header
Authorization: Bearer: {token}
```
Где
- {token} - токен полученный в пункте 1
```
Request Body
{
    "origin": "string",
    "destination": "string",
    "departure": "2024-04-14T12:23:13.587Z",
    "arrival": "2024-04-14T12:23:13.587Z",
    "status": "InTime",
    "userName": "string"
}
```

### 4) Обновление рейсов
```
curl -X PUT http://localhost:5238/api/v1/flight \
Request Header
Authorization: Bearer: {token}
```
Где
- {token} - токен полученный в пункте 1

```
Request Params
--param 'id="{id}"' \
--param 'status="{status}"'
```
