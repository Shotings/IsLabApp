\# Регламент обслуживания ИС (Runbook)



\## 1. Проверка состояния системы


```bash

\# Статус контейнеров

docker compose ps


\# Логи приложения

docker compose logs --tail 50 app

\# Проверка доступности

curl http://127.0.0.1:5000/health

curl http://127.0.0.1:5000/db/ping

