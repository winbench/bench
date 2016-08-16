Start-Process rabbitmq-server

echo "Waiting 10s for the broker to boot ..."
[Threading.Thread]::Sleep(10000)

echo "Activating Web UI ..."
rabbitmq-plugins enable rabbitmq_management

echo "Killing the broker ..."
rabbitmqctl stop
