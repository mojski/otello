## Otello web api

.Net poc app showing how to use open telemetry colector and seq. In this scenario server app is monitoring vendor angostic. App push logs to collector, then collector push data to seq (or other services in future).

```mermaid
graph TD
    Serwer["Serwer"] --"grpc 4317"--> OTEL["Otel Collector"]
    OTEL --"HTTP 5341"--> Seq
    OTEL --"grpc 18889"--> Aspire
    OTEL --"HTTP 3100"--> node_1
    OTEL --"grpc 4317"--> node_2

    node_1["Loki"]
    node_2["Tempo"]
    Seq["Seq"]
    Aspire["Aspire"]
```

Steps to run

- stop local windows seq if installed or other container that use port 5341 - running other seq instance 

- run docker-compose (docker compose up -d) (from infra directory)

- run application, you should see starting logs at localhost:5341 seq UI. 

- hit http://localhost:5080/api/test (GET) to trigger error log

## Seq

![alt text](/doc/readme_assets/seq.png)

## .NET Aspire
![alt text](/doc/readme_assets/asp.png)

## Grafana Loki
query used:
```
{service_name="Otello"}
```
![alt text](/doc/readme_assets/gl.png)

## Grafana Tempo
query used:

```
{resource.service.name="Otello" && name="GET /api/test"} 
```
![alt text](/doc/readme_assets/tempo.png)