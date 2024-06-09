export STREAM_API_BASE_URL="http://streaming-app-backend:8080/api"
export STREAM_API_KEY="${STREAM_API_KEY:-AIzaSyD-9tSrke72PouQMnMX-a7eZSW0jkFMBWY}"

export ASPNETCORE_ENVIRONMENT="${ASPNETCORE_ENVIRONMENT:-Docker}"

docker compose up --force-recreate --build -d && docker container restart nginx && docker ps